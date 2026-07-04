using BimpEngine.Engine.Input;
using BimpEngine.Engine.Core;
using BimpEngine.Engine.Math;
using BimpEngine.Engine.World;
using MoonSharp.Interpreter;
using System;
using BimpEngine.Engine.Scripting.Interface;

namespace BimpEngine.Engine.Scripting.Engines
{
    public class LuaScriptEngine : IScriptEngine
    {
        private Script? _script;
        private DynValue? _startFn, _updateFn, _destroyFn;
        private Objetos? _owner;

        static LuaScriptEngine()
        {
            UserData.RegisterType<Objetos>();
            UserData.RegisterType<Transform>();
            UserData.RegisterType<Vector3>();
        }

        public void Load(string code, ScriptContext context)
        {
            _owner = context.Owner;
            _script = new Script(CoreModules.Preset_SoftSandbox);

            _script.Globals["gameObject"] = _owner;
            _script.Globals["transform"] = _owner?.Transform;
            _script.Globals["log"] = (Action<string>)(m => Debug.Console.Log(m, _owner?.Name ?? "Lua"));
            _script.Globals["logWarning"] = (Action<string>)(m => Debug.Console.LogWarning(m, _owner?.Name ?? "Lua"));
            _script.Globals["logError"] = (Action<string>)(m => Debug.Console.LogError(m, _owner?.Name ?? "Lua"));
            _script.Globals["getKey"] = (Func<string, bool>)(k => InputManager.GetAction(k));
            _script.Globals["getAxis"] = (Func<string, float>)(n => InputManager.GetAxis(n));
            _script.Globals["getAxisRaw"] = (Func<string, float>)(n => InputManager.GetAxisRaw(n));
            _script.Globals["find"] = (Func<string, Objetos?>)(n => context.Scene?.FindByName(n));

            try
            {
                _script.DoString(code);
            }
            catch (Exception ex)
            {
                Debug.Console.LogError("Error Lua: " + ex.Message, _owner?.Name ?? "Lua");
                return;
            }

            _startFn = _script.Globals.Get("start");
            _updateFn = _script.Globals.Get("update");
            _destroyFn = _script.Globals.Get("onDestroy");
        }

        public void Start()
        {
            if (_startFn != null && _startFn.Type == DataType.Function)
                _script!.Call(_startFn);
        }

        public void Update(float deltaTime)
        {
            if (_updateFn != null && _updateFn.Type == DataType.Function)
                _script!.Call(_updateFn, deltaTime);
        }

        public void OnDestroy()
        {
            if (_destroyFn != null && _destroyFn.Type == DataType.Function)
                _script!.Call(_destroyFn);
        }

        public void SetVariable(string name, object value) { if (_script != null) _script.Globals[name] = value; }
        public object? GetVariable(string name) => _script?.Globals.Get(name);

        private static readonly HashSet<string> Reservadas = new(StringComparer.OrdinalIgnoreCase)
        {
            "gameObject", "transform", "log", "logWarning", "logError", "getKey", "getAxis", "getAxisRaw", "find", "start", "update", "onDestroy"
        };

        public IEnumerable<ScriptVariable> GetExposedVariables()
        {
            if (_script == null) yield break;

            foreach (var par in _script.Globals.Pairs)
            {
                if (par.Key.Type != DataType.String) continue;

                string nombre = par.Key.String;
                if (Reservadas.Contains(nombre)) continue;

                var valor = par.Value;

                Enum.VariableType? tipo = valor.Type switch
                {
                    DataType.Number => Enum.VariableType.Float,
                    DataType.Boolean => Enum.VariableType.Bool,
                    DataType.String => Enum.VariableType.String,
                    DataType.UserData when valor.UserData?.Object is Math.Vector3 => Enum.VariableType.Vector3,
                    _ => (Enum.VariableType?)null
                };

                if (tipo == null) continue;

                object? nativo = tipo == Enum.VariableType.Vector3 ? valor.UserData!.Object : valor.ToObject();
                yield return new ScriptVariable { Name = nombre, Type = tipo.Value, ValueRaw = ScriptVariableUtils.FormatValor(nativo, tipo.Value) };
            }
        }
    }
}