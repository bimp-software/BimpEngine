using BimpEngine.Engine.Scripting.Enum;
using BimpEngine.Engine.Scripting.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Scripting
{
    public class ScriptComponent
    {
        public Language Language { get; set; } = Language.CSharp;
        public string ScriptName { get; set; } = "";
        public string ScriptPath { get; set; } = "";
        public string Code { get; set; } = "";

        public List<ScriptVariable> Variables { get; set; } = new();

        public IScript? Instance { get; set; }
        public IScriptEngine? Engine { get; set; }
        public bool Started { get; set; }
    }
}
