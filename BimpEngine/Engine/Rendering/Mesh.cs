namespace BimpEngine.Engine.Rendering
{
    public class Mesh
    {
        public string Name { get; set; } = "Mesh";
        public List<Vertex> Vertices { get; set; } = new List<Vertex>();
        public List<int> Triangles { get; set; } = new List<int>();
        public List<int> Edges { get; set; } = new List<int>();

        public List<SubMesh> SubMeshes { get; set; } = new List<SubMesh>();
        public bool IsImported { get; set; } = false;
    }
}
