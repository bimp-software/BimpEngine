using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Scripting.Nodes
{
    public class SumarNode : ScriptNode
    {
        private readonly NodePort _a, _b, _result;
        public SumarNode()
        {
            Title = "Sumar"; Category = NodeCategory.Math;
            _a = AddInput("A", PortType.Float, 0.0);
            _b = AddInput("B", PortType.Float, 0.0);
            _result = AddOutput("A+B", PortType.Float);
        }
        public override NodePort? Execute(ScriptContext ctx)
        { _result.RuntimeValue = D(_a) + D(_b); return null; }
        private static double D(NodePort p) => p.RuntimeValue is double d ? d : p.Value is double v ? v : 0;
    }

    public class RestarNode : ScriptNode
    {
        private readonly NodePort _a, _b, _result;
        public RestarNode()
        {
            Title = "Restar"; Category = NodeCategory.Math;
            _a = AddInput("A", PortType.Float, 0.0);
            _b = AddInput("B", PortType.Float, 0.0);
            _result = AddOutput("A-B", PortType.Float);
        }
        public override NodePort? Execute(ScriptContext ctx)
        { _result.RuntimeValue = D(_a) - D(_b); return null; }
        private static double D(NodePort p) => p.RuntimeValue is double d ? d : p.Value is double v ? v : 0;
    }

    public class MultiplicarNode : ScriptNode
    {
        private readonly NodePort _a, _b, _result;
        public MultiplicarNode()
        {
            Title = "Multiplicar"; Category = NodeCategory.Math;
            _a = AddInput("A", PortType.Float, 1.0);
            _b = AddInput("B", PortType.Float, 1.0);
            _result = AddOutput("A×B", PortType.Float);
        }
        public override NodePort? Execute(ScriptContext ctx)
        { _result.RuntimeValue = D(_a) * D(_b); return null; }
        private static double D(NodePort p) => p.RuntimeValue is double d ? d : p.Value is double v ? v : 0;
    }

    public class SinusNode : ScriptNode
    {
        private readonly NodePort _ang, _result;
        public SinusNode()
        {
            Title = "Seno"; Category = NodeCategory.Math;
            _ang = AddInput("Ángulo°", PortType.Float, 0.0);
            _result = AddOutput("Seno", PortType.Float);
        }
        public override NodePort? Execute(ScriptContext ctx)
        {
            double ang = D(_ang) * System.Math.PI / 180.0;
            _result.RuntimeValue = System.Math.Sin(ang);
            return null;
        }
        private static double D(NodePort p) => p.RuntimeValue is double d ? d : p.Value is double v ? v : 0;
    }
}
