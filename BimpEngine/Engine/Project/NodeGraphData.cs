using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Project
{
    public class NodeGraphData
    {
        public List<NodeData> Nodes { get; set; } = new();
        public List<ConnectionData> Connections { get; set; } = new();
        public List<VariableData> Variables { get; set; } = new();
    }
}
