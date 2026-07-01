using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Scripting.Nodes
{
    public class EscalarObjetoNode : ScriptNode
    {
        private readonly NodePort _execIn, _execOut, _x, _y, _z;
        public EscalarObjetoNode()
        {
            Title = "Escalar Objeto";
            Category = NodeCategory.Action;
            _execIn = AddInput("▶", PortType.Exec);
            _x = AddInput("X", PortType.Float, 1.0);
            _y = AddInput("Y", PortType.Float, 1.0);
            _z = AddInput("Z", PortType.Float, 1.0);
            _execOut = AddOutput("▶", PortType.Exec);
        }
        public override NodePort? Execute(ScriptContext ctx)
        {
            if (ctx.Owner == null) return _execOut;
            ctx.Owner.Transform.Scale.X = ToD(_x.RuntimeValue ?? _x.Value);
            ctx.Owner.Transform.Scale.Y = ToD(_y.RuntimeValue ?? _y.Value);
            ctx.Owner.Transform.Scale.Z = ToD(_z.RuntimeValue ?? _z.Value);
            return _execOut;
        }
        private static double ToD(object? v) => v is double d ? d : v is float f ? f : 0;
    }
}
