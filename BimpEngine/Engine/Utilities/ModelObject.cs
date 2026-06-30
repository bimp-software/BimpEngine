using BimpEngine.Engine.Assets;
using BimpEngine.Engine.Rendering;
using BimpEngine.Engine.World;

namespace BimpEngine.Engine.Utilities
{
    public class ModelObject : Objetos
    {
        public string SourcePath { get; private set; } = "";

        public ModelObject() { }

        public ModelObject(ImportedModel imported)
        {
            Name = string.IsNullOrWhiteSpace(imported.Mesh.Name) ? Path.GetFileNameWithoutExtension(imported.SourcePath) : imported.Mesh.Name;
            SourcePath = imported.SourcePath;
            MeshFilter.Mesh = imported.Mesh;

            foreach (var im in imported.Materials)
            {
                var mat = new Material
                {
                    Name = im.Name,
                    Color = im.DiffuseColor,
                    TexturePath = im.TexturePath,
                    Shininess = im.Shininess,
                };
                MeshRenderer.Materials.Add(mat);
            }

            if (MeshRenderer.Materials.Count > 0)
                MeshRenderer.Material = MeshRenderer.Materials[0];
        }
    }
}
