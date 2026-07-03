using BimpEngine.Engine.Scripting.Interface;
using BimpEngine.Engine.World;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;
using System.Runtime.Loader;

namespace BimpEngine.Engine.Scripting.Engines
{
    public class CSharpScriptEngine : IScriptEngine
    {
        private IScript? _instance;
        private AssemblyLoadContext? _loadContext;

        public void Load(string code, ScriptContext context)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(InyectarUsings(code));

            var references = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic && !string.IsNullOrEmpty(a.Location))
                .Select(a => (MetadataReference)MetadataReference.CreateFromFile(a.Location))
                .ToList();

            var compilation = CSharpCompilation.Create(
                "UserScript_" + Guid.NewGuid().ToString("N"),
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                var errores = string.Join("\n", result.Diagnostics
                    .Where(d => d.Severity == DiagnosticSeverity.Error)
                    .Select(d => d.ToString()));

                Debug.Console.LogError("Error de compilación C#:\n" + errores, context.Owner?.Name ?? "CSharp");
                return;
            }

            ms.Seek(0, SeekOrigin.Begin);
            _loadContext = new AssemblyLoadContext("Script_" + Guid.NewGuid(), isCollectible: true);
            var assembly = _loadContext.LoadFromStream(ms);

            var tipo = assembly.GetTypes().FirstOrDefault(t => typeof(IScript).IsAssignableFrom(t) && !t.IsAbstract);

            if (tipo == null)
            {
                Debug.Console.LogError("No se encontró una clase pública que herede de IScript.", context.Owner?.Name ?? "CSharp");
                return;
            }

            _instance = (IScript)Activator.CreateInstance(tipo)!;
            _instance.gameObject = context.Owner;
            _instance.SetContext(context);
        }

        public void Start() => _instance?.Start();
        public void Update(float deltaTime) => _instance?.Update(deltaTime);

        public void OnDestroy()
        {
            _instance?.OnDestroy();
            _loadContext?.Unload();
            _loadContext = null;
            _instance = null;
        }

        private static string InyectarUsings(string code) =>
            "using System;\n" +
            "using System.Collections.Generic;\n" +
            "using BimpEngine.Engine.Scripting;\n" +
            "using BimpEngine.Engine.Core;\n" +
            "using BimpEngine.Engine.Math;\n" +
            "using BimpEngine.Engine.World;\n" + code;

        // --- Variables públicas expuestas (campos y propiedades declarados en la clase del usuario) ---

        public IEnumerable<ScriptVariable> GetExposedVariables()
        {
            if (_instance == null) yield break;

            var tipo = _instance.GetType();

            foreach (var campo in tipo.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                var vt = ScriptVariableUtils.MapClrType(campo.FieldType);
                if (vt == null) continue;

                yield return new ScriptVariable
                {
                    Name = campo.Name,
                    Type = vt.Value,
                    ValueRaw = ScriptVariableUtils.FormatValor(campo.GetValue(_instance), vt.Value)
                };
            }

            foreach (var prop in tipo.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                if (!prop.CanRead || !prop.CanWrite || prop.GetIndexParameters().Length > 0) continue;

                var vt = ScriptVariableUtils.MapClrType(prop.PropertyType);
                if (vt == null) continue;

                yield return new ScriptVariable
                {
                    Name = prop.Name,
                    Type = vt.Value,
                    ValueRaw = ScriptVariableUtils.FormatValor(prop.GetValue(_instance), vt.Value)
                };
            }
        }

        public void SetVariable(string name, object value)
        {
            if (_instance == null) return;
            var tipo = _instance.GetType();

            var campo = tipo.GetField(name, BindingFlags.Public | BindingFlags.Instance);
            if (campo != null)
            {
                campo.SetValue(_instance, ScriptVariableUtils.ConvertirParaClr(value, campo.FieldType));
                return;
            }

            var prop = tipo.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.CanWrite)
                prop.SetValue(_instance, ScriptVariableUtils.ConvertirParaClr(value, prop.PropertyType));
        }

        public object? GetVariable(string name)
        {
            if (_instance == null) return null;
            var tipo = _instance.GetType();

            var campo = tipo.GetField(name, BindingFlags.Public | BindingFlags.Instance);
            if (campo != null) return campo.GetValue(_instance);

            var prop = tipo.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
            return prop?.GetValue(_instance);
        }
    }
}