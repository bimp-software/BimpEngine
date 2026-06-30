using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Project
{
    public class LayerEntry
    {
        public int Index { get; set; }
        public string Name { get; set; } = "";

        public LayerEntry() { }
        public LayerEntry(int index, string name) { Index = index; Name = name; }

        public override string ToString() => $"{Index}: {Name}";
    }
}
