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

    

}
