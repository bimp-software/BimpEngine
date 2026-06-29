using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Scripting.Nodes
{
    public class ObtenerVariableNode : ScriptNode
    {
        private readonly NodePort _valor;
        private readonly object? _valorDefecto;

        public ObtenerVariableNode(GraphVariable? v = null)
        {
            string nombre = v?.Nombre ?? "miVariable";
            PortType tipo = v?.PortType ?? PortType.Float;

            Title = $"Obtener {nombre}";
            Category = NodeCategory.Variable;
            _valor = AddOutput(nombre, tipo);
            _valor.Value = nombre;
            _valorDefecto = v?.ValorDefecto; // guardar valor del panel
        }

        public override NodePort? Execute(ScriptContext ctx)
        {
            string name = _valor.Value?.ToString() ?? "";
            if (ctx.Variables.TryGetValue(name, out var val))
                _valor.RuntimeValue = val;
            else
                _valor.RuntimeValue = _valorDefecto;
            return null;
        }
    }

    public class SetVariableNode : ScriptNode
    {
        private readonly NodePort _execIn, _execOut, _valor;
        private readonly string _nombre;

        public SetVariableNode(GraphVariable? v = null)
        {
            _nombre = v?.Nombre ?? "miVariable";
            PortType tipo = v?.PortType ?? PortType.Float;

            Title = $"Asignar {_nombre}";
            Category = NodeCategory.Variable;
            _execIn = AddInput("▶", PortType.Exec);
            _valor = AddInput(_nombre, tipo);
            _execOut = AddOutput("▶", PortType.Exec);
        }

        public override NodePort? Execute(ScriptContext ctx)
        {
            ctx.Variables[_nombre] = _valor.RuntimeValue ?? _valor.Value;
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
