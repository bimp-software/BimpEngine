using BimpEngine.Engine.Input;
using BimpEngine.Engine.Scripting.Interface;
using BimpEngine.Engine.World;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;

namespace BimpEngine.Engine.Scripting.Engines
{
    public class PythonScriptEngine : IScriptEngine
    {
        private ScriptEngine? _engine;
        private ScriptScope? _scope;
        private Objetos? _owner;

        public void Load(string code, ScriptContext context)
        {
            _owner = context.Owner;
            _engine = Python.CreateEngine();
            _scope = _engine.CreateScope();

            _scope.SetVariable("gameObject", _owner);
            _scope.SetVariable("transform", _owner?.Transform);
            _scope.SetVariable("log", new Action<string>(m => Debug.Console.Log(m, _owner?.Name ?? "Python")));
            _scope.SetVariable("log_warning", new Action<string>(m => Debug.Console.LogWarning(m, _owner?.Name ?? "Python")));
            _scope.SetVariable("log_error", new Action<string>(m => Debug.Console.LogError(m, _owner?.Name ?? "Python")));
            _scope.SetVariable("get_key", new Func<string, bool>(k => InputManager.GetAction(k)));
            _scope.SetVariable("get_axis", new Func<string, float>(n => InputManager.GetAxis(n)));
            _scope.SetVariable("get_axis_raw", new Func<string, float>(n => InputManager.GetAxisRaw(n)));
            _scope.SetVariable("find", new Func<string, Objetos?>(n => context.Scene?.FindByName(n)));

            try
            {
                _engine.Execute(code, _scope);
            }
            catch (Exception ex)
            {
                Debug.Console.LogError("Error Python: " + ex.Message, _owner?.Name ?? "Python");
            }
        }

        public void Start() => Invoke("start");
        public void Update(float deltaTime) => Invoke("update", deltaTime);
        public void OnDestroy() => Invoke("on_destroy");

        private void Invoke(string funcName, params object[] args)
        {
            if (_scope == null || _engine == null) return;
            if (!_scope.TryGetVariable(funcName, out object fn) || fn == null) return;

            try
            {
                _engine.Operations.Invoke(fn, args);
            }
            catch (Exception ex)
            {
                Debug.Console.LogError($"Error Python en {funcName}: {ex.Message}", _owner?.Name ?? "Python");
            }
        }

        public void SetVariable(string name, object value) => _scope?.SetVariable(name, value);
        public object? GetVariable(string name) => _scope?.GetVariable(name);

        private static readonly HashSet<string> Reservadas = new(StringComparer.OrdinalIgnoreCase)
        {
            "gameObject", "transform", "log", "log_warning", "log_error", "get_key", "get_axis", "get_axis_raw", "find", "start", "update", "on_destroy"
        };

        public IEnumerable<ScriptVariable> GetExposedVariables()
        {
            if (_scope == null) yield break;

            foreach (var nombre in _scope.GetVariableNames())
            {
                if (Reservadas.Contains(nombre)) continue;
                if (!_scope.TryGetVariable(nombre, out object valor) || valor == null) continue;

                Enum.VariableType? tipo = valor switch
                {
                    double or float => Enum.VariableType.Float,
                    int => Enum.VariableType.Int,
                    bool => Enum.VariableType.Bool,
                    string => Enum.VariableType.String,
                    Math.Vector3 => Enum.VariableType.Vector3,
                    _ => (Enum.VariableType?)null
                };

                if (tipo == null) continue;

                yield return new ScriptVariable { Name = nombre, Type = tipo.Value, ValueRaw = ScriptVariableUtils.FormatValor(valor, tipo.Value) };
            }
        }
    }
}