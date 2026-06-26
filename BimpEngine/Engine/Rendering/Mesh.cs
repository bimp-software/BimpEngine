namespace BimpEngine.Engine.Rendering
{
    public class Mesh
    {
        public string Name { get; set; } = "Mesh";
        public List<Vertex> Vertices { get; set; } = new List<Vertex>();
        public List<int> Triangles { get; set; } = new List<int>();
    }
}
