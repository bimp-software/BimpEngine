using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Scripting
{
    public class NodePort
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; set; } = "";
        public PortType DataType { get; set; } = PortType.Exec;
        public PortDirection Direction { get; set; } = PortDirection.Input;
        public object? Value { get; set; }

        public object? RuntimeValue { get; set; }
        public Point ScreenPos { get; set; }

        public static Color ColorFor(PortType t) => t switch
        {
            PortType.Exec => Color.White,
            PortType.Float => Color.FromArgb(0, 200, 100),
            PortType.Bool => Color.FromArgb(220, 80, 80),
            PortType.String => Color.FromArgb(220, 160, 60),
            PortType.Vector3 => Color.FromArgb(100, 160, 255),
            PortType.Object => Color.FromArgb(180, 100, 255),
            _ => Color.Gray
        };
    }
}
