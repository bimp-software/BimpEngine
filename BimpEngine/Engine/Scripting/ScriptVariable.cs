using BimpEngine.Engine.Scripting.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Scripting
{
    public class ScriptVariable
    {
        public string Name { get; set; } = "";
        public VariableType Type { get; set; } = VariableType.Float;
        public string ValueRaw { get; set; } = "";
    }
}
