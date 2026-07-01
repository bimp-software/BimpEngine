namespace BimpEngine.Engine.Scripting.Nodes
{
    public class ImprimirNode : ScriptNode
    {
        private readonly NodePort _execIn, _execOut, _msg;
        public ImprimirNode()
        {
            Title = "Imprimir";
            Category = NodeCategory.Action;
            _execIn = AddInput("▶", PortType.Exec);
            _msg = AddInput("Mensaje", PortType.String, "Hola Mundo");
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
