using BimpEngine.Engine.Core;
using BimpEngine.Engine.World;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Scripting
{
    public class ScriptContext
    {
        public Objetos? Owner { get; set; }
        public Dictionary<string, object> Variables { get; } = new();
        public HashSet<Keys> KeysPressed { get; set; } = new();
        public Scene? Scene { get; set; }
        public double DeltaTime { get; set; } = 0.016; // ~60fps
        public bool Running { get; set; } = true;
        public int Steps { get; set; } = 0;
        public int MaxSteps { get; set; } = 1000;
    }
}
