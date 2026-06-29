using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Scripting.Nodes
{
    public class TemporizadorNode : ScriptNode
    {
        private readonly NodePort _execIn, _execOut, _intervalo;
        private double _acum = 0;
        public TemporizadorNode()
        {
            Title = "Temporizador";
            Category = NodeCategory.Action;
            _execIn = AddInput("▶", PortType.Exec);
            _intervalo = AddInput("Cada (seg)", PortType.Float, 1.0);
            _execOut = AddOutput("▶", PortType.Exec);
        }
        public override NodePort? Execute(ScriptContext ctx)
        {
            double intervalo = ToD(_intervalo.RuntimeValue ?? _intervalo.Value);
            _acum += ctx.DeltaTime;
            if (_acum >= intervalo) { _acum = 0; return _execOut; }
            return null;
        }
        private static double ToD(object? v) => v is double d ? d : v is float f ? f : 0;
    }
}
