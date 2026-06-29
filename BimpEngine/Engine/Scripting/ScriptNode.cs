using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Scripting
{
    public abstract class ScriptNode
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Title { get; set; } = "Nodo";
        public NodeCategory Category { get; protected set; } = NodeCategory.Action;
        public Point Position { get; set; } = new(100, 100);
        public List<NodePort> Ports { get; } = new();

        protected NodePort AddInput(string name, PortType type, object? defaultVal = null)
        {
            var p = new NodePort
            {
                Name = name,
                DataType = type,
                Direction = PortDirection.Input,
                Value = defaultVal
            };
            Ports.Add(p); return p;
        }

        protected NodePort AddOutput(string name, PortType type)
        {
            var p = new NodePort
            {
                Name = name,
                DataType = type,
                Direction = PortDirection.Output
            };
            Ports.Add(p); return p;
        }

        public IEnumerable<NodePort> Inputs => Ports.Where(p => p.Direction == PortDirection.Input);
        public IEnumerable<NodePort> Outputs => Ports.Where(p => p.Direction == PortDirection.Output);

        public abstract NodePort? Execute(ScriptContext ctx);

        public Size Size => new(180, 30 + System.Math.Max(Inputs.Count(), Outputs.Count()) * 24 + 8);

        public Color HeaderColor => Category switch
        {
            NodeCategory.Event => Color.FromArgb(140, 60, 60),
            NodeCategory.Action => Color.FromArgb(50, 90, 160),
            NodeCategory.Condition => Color.FromArgb(120, 90, 20),
            NodeCategory.Variable => Color.FromArgb(50, 120, 80),
            NodeCategory.Math => Color.FromArgb(80, 60, 130),
            _ => Color.FromArgb(60, 60, 60)
        };
    }
}
