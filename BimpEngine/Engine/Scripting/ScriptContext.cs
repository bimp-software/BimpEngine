using BimpEngine.Engine.World;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Scripting
{
    public class ScriptContext
    {
        public Objetos? Owner { get; set; }
        public Dictionary<string, object?> Variables { get; } = new();
        public bool Running { get; set; } = true;
        public int MaxSteps { get; set; } = 1000;
        public int Steps { get; set; } = 0;
    }
}
