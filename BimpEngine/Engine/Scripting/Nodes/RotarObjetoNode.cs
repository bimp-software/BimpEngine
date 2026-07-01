using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Scripting.Nodes
{
    public class RotarObjetoNode : ScriptNode
    {
        private readonly NodePort _execIn, _execOut, _x, _y, _z;
        public RotarObjetoNode()
        {
            Title = "Rotar Objeto";
            Category = NodeCategory.Action;
            _execIn = AddInput("▶", PortType.Exec);
            _x = AddInput("X°", PortType.Float, 0.0);
            _y = AddInput("Y°", PortType.Float, 1.0);
            _z = AddInput("Z°", PortType.Float, 0.0);
            _execOut = AddOutput("▶", PortType.Exec);
        }
        public override NodePort? Execute(ScriptContext ctx)
        {
            if (ctx.Owner == null) return _execOut;
            double x = ToD(_x.RuntimeValue ?? _x.Value);
            double y = ToD(_y.RuntimeValue ?? _y.Value);
            double z = ToD(_z.RuntimeValue ?? _z.Value);
            ctx.Owner.Transform.Rotation.X += x;
            ctx.Owner.Transform.Rotation.Y += y;
            ctx.Owner.Transform.Rotation.Z += z;
            return _execOut;
        }
        private static double ToD(object? v) => v is double d ? d : v is float f ? f : 0;
    }
}
