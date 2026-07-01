using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Project
{
    public class VariableData
    {
        public string Nombre { get; set; } = "";
        public string Tipo { get; set; } = "Float";
        public string? ValorDefecto { get; set; }
        public string? ReferenciaName { get; set; }
    }
}
