using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Project
{
    public class MoldeData
    {
        public Guid MoldeId { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "NuevoMolde";
        public List<ObjectData> Objects { get; set; } = new();
    }
}
