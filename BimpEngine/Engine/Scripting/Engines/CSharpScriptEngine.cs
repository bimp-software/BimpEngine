using BimpEngine.Engine.Scripting.Interface;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Runtime.Loader;
using System.Text;

namespace BimpEngine.Engine.Scripting.Engines
{
    public class CSharpScriptEngine : IScriptEngine
    {
        private IScript? _instance;
        private AssemblyLoadContext? _loadContext;

        public void Load(string code, ScriptContext context)
        {
            string codigoCompleto = InyectarUsings(code);
            var syntaxTree = CSharpSyntaxTree.ParseText(codigoCompleto);

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
        }

        public void SetVariable(string name, object value) { }
        public object? GetVariable(string name) => null;

        private static string InyectarUsings(string code)
        {
            const string usings =
                "using System;\n" +
                "using System.Collections.Generic;\n" +
                "using BimpEngine.Engine.Scripting;\n" +
                "using BimpEngine.Engine.Core;\n" +
                "using BimpEngine.Engine.Math;\n" +
                "using BimpEngine.Engine.World;\n";

            return usings + code;
        }
    }
}
