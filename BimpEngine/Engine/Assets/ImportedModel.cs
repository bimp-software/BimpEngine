using BimpEngine.Engine.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Assets
{
    public class ImportedModel
    {
        public Mesh Mesh { get; set; } = new Mesh();
        public List<ImportedMaterial> Materials { get; set; } = new List<ImportedMaterial>();
        public string SourcePath { get; set; } = "";
    }
}
