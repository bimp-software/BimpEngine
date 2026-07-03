using BimpEngine.Engine.Scripting.Enum;
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
        public IScript? Instance { get; set; }
    }
}
