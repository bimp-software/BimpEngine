using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Scripting.Interface
{
    public interface IScriptEngine
    {
        void Load(string code, ScriptContext context);
        void Start();
        void Update(float deltaTime);
        void OnDestroy();
        void SetVariable(string name, object value);
        object? GetVariable(string name);
        IEnumerable<ScriptVariable> GetExposedVariables();
    }
}
