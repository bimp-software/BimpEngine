using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Scripting.Enum
{
    public class ScriptData
    {
        public Language Language { get; set; } = Language.CSharp;
        public string ScriptName { get; set; } = "";
        public string ScriptPath { get; set; } = "";
    }
}
