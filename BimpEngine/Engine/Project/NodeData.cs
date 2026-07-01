using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Project
{
    public class NodeData
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = ""; 
        public int X { get; set; }
        public int Y { get; set; }
        public List<PortValueData> PortValues { get; set; } = new();
    }
}
