using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Scripting.Nodes
{
    public class ObtenerVariableNode : ScriptNode
    {
        private readonly NodePort _nombre, _valor;
        public ObtenerVariableNode()
        {
            Title = "Obtener Variable";
            Category = NodeCategory.Variable;
            _nombre = AddInput("Nombre", PortType.String, "miVar");
            _valor = AddOutput("Valor", PortType.Float);
        }
        public override NodePort? Execute(ScriptContext ctx)
        {
            string name = _nombre.Value?.ToString() ?? "miVar";
            ctx.Variables.TryGetValue(name, out var val);
            _valor.RuntimeValue = val ?? 0.0;
            return null;
        }
    }

    public class SetVariableNode : ScriptNode
    {
        private readonly NodePort _execIn, _execOut, _nombre, _valor;
        public SetVariableNode()
        {
            Title = "Asignar Variable";
            Category = NodeCategory.Variable;
            _execIn = AddInput("▶", PortType.Exec);
            _nombre = AddInput("Nombre", PortType.String, "miVar");
            _valor = AddInput("Valor", PortType.Float, 0.0);
            _execOut = AddOutput("▶", PortType.Exec);
        }
        public override NodePort? Execute(ScriptContext ctx)
        {
            string name = _nombre.Value?.ToString() ?? "miVar";
            ctx.Variables[name] = _valor.RuntimeValue ?? _valor.Value;
            return _execOut;
        }
    }

    public class NumeroConstanteNode : ScriptNode
    {
        private readonly NodePort _val;
        public NumeroConstanteNode()
        {
            Title = "Número";
            Category = NodeCategory.Variable;
            _val = AddOutput("Valor", PortType.Float);
        }
        public double Valor { get; set; } = 0;
        public override NodePort? Execute(ScriptContext ctx)
        {
            _val.RuntimeValue = Valor;
            return null;
        }
    }
}
