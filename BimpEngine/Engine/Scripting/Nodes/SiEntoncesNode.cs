using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Scripting.Nodes
{
    public class SiEntoncesNode : ScriptNode
    {
        private readonly NodePort _execIn, _condIn, _si, _sino;
        public SiEntoncesNode()
        {
            Title = "Si / Sino";
            Category = NodeCategory.Condition;
            _execIn = AddInput("▶", PortType.Exec);
            _condIn = AddInput("Condición", PortType.Bool, false);
            _si = AddOutput("Si (Verdad)", PortType.Exec);
            _sino = AddOutput("Sino (Falso)", PortType.Exec);
        }
        public override NodePort? Execute(ScriptContext ctx)
        {
            bool cond = ToBool(_condIn.RuntimeValue ?? _condIn.Value);
            return cond ? _si : _sino;
        }
        private static bool ToBool(object? v) => v is bool b ? b : false;
    }

    public class CompararNode : ScriptNode
    {
        private readonly NodePort _a, _b, _op, _result;
        public CompararNode()
        {
            Title = "Comparar";
            Category = NodeCategory.Condition;
            _a = AddInput("A", PortType.Float, 0.0);
            _b = AddInput("B", PortType.Float, 0.0);
            _op = AddInput("Op (0=>, 1=<, 2===)", PortType.Float, 0.0);
            _result = AddOutput("Resultado", PortType.Bool);
        }
        public override NodePort? Execute(ScriptContext ctx)
        {
            double a = ToD(_a.RuntimeValue ?? _a.Value);
            double b = ToD(_b.RuntimeValue ?? _b.Value);
            int op = (int)ToD(_op.RuntimeValue ?? _op.Value);
            _result.RuntimeValue = op switch { 0 => a >= b, 1 => a < b, _ => System.Math.Abs(a - b) < 0.0001 };
            return null;
        }
        private static double ToD(object? v) => v is double d ? d : v is float f ? f : 0;
    }
}
