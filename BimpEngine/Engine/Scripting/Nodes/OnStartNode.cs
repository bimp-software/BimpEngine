using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Scripting.Nodes
{
    public class OnStartNode : ScriptNode
    {
        private readonly NodePort _out;
        public OnStartNode() { Title = "Al Iniciar"; Category = NodeCategory.Event; _out = AddOutput("▶", PortType.Exec); }
        public override NodePort? Execute(ScriptContext ctx) => _out;
    }

    public class OnClickNode : ScriptNode
    {
        private readonly NodePort _out;
        public OnClickNode() { Title = "Al Hacer Clic"; Category = NodeCategory.Event; _out = AddOutput("▶", PortType.Exec); }
        public override NodePort? Execute(ScriptContext ctx) => _out;
    }

    public class OnUpdateNode : ScriptNode
    {
        private readonly NodePort _out;
        public OnUpdateNode() { Title = "Cada Frame"; Category = NodeCategory.Event; _out = AddOutput("▶", PortType.Exec); }
        public override NodePort? Execute(ScriptContext ctx) => _out;
    }
}

