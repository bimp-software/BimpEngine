using BimpEngine.Engine.Scripting.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Scripting
{
    public class GraphVariable
    {
        public string Nombre { get; set; } = "miVariable";
        public VariableType Tipo { get; set; } = VariableType.Float;
        public object? ValorDefecto { get; set; } = 0.0;
        public string? ReferenciaName { get; set; }

        public Color Color => Tipo switch
        {
            VariableType.Float => Color.FromArgb(0, 200, 100),
            VariableType.Bool => Color.FromArgb(220, 80, 80),
            VariableType.String => Color.FromArgb(220, 160, 60),
            VariableType.Vector3 => Color.FromArgb(100, 160, 255),
            VariableType.Objeto => Color.FromArgb(180, 100, 255),
            VariableType.Escena => Color.FromArgb(80, 180, 220),
            VariableType.Transform => Color.FromArgb(255, 140, 0),
            VariableType.Camera => Color.FromArgb(60, 200, 180),
            VariableType.Canvas => Color.FromArgb(200, 200, 60),
            VariableType.ListaFloat => Color.FromArgb(0, 160, 80),
            VariableType.ListaObjetos => Color.FromArgb(140, 80, 200),
            _ => Color.Gray
        };

        public string NombreTipo => Tipo switch
        {
            VariableType.Float => "Número",
            VariableType.Bool => "Booleano",
            VariableType.String => "Texto",
            VariableType.Vector3 => "Vector3",
            VariableType.Objeto => "Objeto",
            VariableType.Escena => "Escena",
            VariableType.Transform => "Transform",
            VariableType.Camera => "Cámara",
            VariableType.Canvas => "Canvas UI",
            VariableType.ListaFloat => "Lista Números",
            VariableType.ListaObjetos => "Lista Objetos",
            _ => "Desconocido"
        };

        public PortType PortType => Tipo switch
        {
            VariableType.Float => PortType.Float,
            VariableType.Bool => PortType.Bool,
            VariableType.String => PortType.String,
            VariableType.Vector3 => PortType.Vector3,
            VariableType.Objeto => PortType.Object,
            VariableType.Escena => PortType.Object,
            VariableType.Transform => PortType.Object,
            VariableType.Camera => PortType.Object,
            VariableType.Canvas => PortType.Object,
            VariableType.ListaFloat => PortType.Object,
            VariableType.ListaObjetos => PortType.Object,
            _ => PortType.Float
        };
    }
}
