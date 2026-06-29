using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Scripting.Nodes
{
    public class TeclaPresionadaNode : ScriptNode
    {
        private readonly NodePort _out, _tecla;
        public TeclaPresionadaNode()
        {
            Title = "Al Presionar Tecla";
            Category = NodeCategory.Event;
            _tecla = AddInput("Tecla", PortType.String, "W");
            _out = AddOutput("▶", PortType.Exec);
        }
        public override NodePort? Execute(ScriptContext ctx)
        {
            string tecla = _tecla.Value?.ToString() ?? "W";
            if (System.Enum.TryParse<Keys>(tecla, out var key) && ctx.KeysPressed.Contains(key))
                return _out;
            return null;
        }
    }
}
