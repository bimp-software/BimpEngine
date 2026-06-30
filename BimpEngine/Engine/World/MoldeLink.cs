using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.World
{
    public class MoldeLink
    {
        public string? MoldeFilePath { get; set; }
        public Guid? MoldeId { get; set; }
        public bool EsInstanciaDeMolde => MoldeFilePath != null;
    }
}
