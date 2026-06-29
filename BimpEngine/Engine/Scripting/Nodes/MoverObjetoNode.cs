using System;

namespace BimpEngine.Engine.Scripting.Nodes
{
    public class MoverObjetoNode : ScriptNode
    {
        private readonly NodePort _execIn, _execOut, _x, _y, _z;
        public MoverObjetoNode()
        {
            Title = "Mover Objeto";
            Category = NodeCategory.Action;
            _execIn = AddInput("▶", PortType.Exec);
            _x = AddInput("X", PortType.Float, 0.0);
            _y = AddInput("Y", PortType.Float, 0.0);
            _z = AddInput("Z", PortType.Float, 0.0);
            _execOut = AddOutput("▶", PortType.Exec);
        }
        public override NodePort? Execute(ScriptContext ctx)
        {
            if (ctx.Owner == null) return _execOut;
            double x = Convert(_x.RuntimeValue ?? _x.Value);
            double y = Convert(_y.RuntimeValue ?? _y.Value);
            double z = Convert(_z.RuntimeValue ?? _z.Value);
            ctx.Owner.Transform.Position.X += x;
            ctx.Owner.Transform.Position.Y += y;
            ctx.Owner.Transform.Position.Z += z;
            return _execOut;
        }
        private static double Convert(object? v) => v is double d ? d : v is float f ? f : 0;
    }

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

    public class ImprimirNode : ScriptNode
    {
        private readonly NodePort _execIn, _execOut, _msg;
        public ImprimirNode()
        {
            Title = "Imprimir";
            Category = NodeCategory.Action;
            _execIn = AddInput("▶", PortType.Exec);
            _msg = AddInput("Mensaje", PortType.String, "Hola");
            _execOut = AddOutput("▶", PortType.Exec);
        }
        public override NodePort? Execute(ScriptContext ctx)
        {
            string msg = (_msg.RuntimeValue ?? _msg.Value)?.ToString() ?? "";
            Debug.Console.Log(msg, ctx.Owner?.Name ?? "Script");
            return _execOut;
        }
    }
}
