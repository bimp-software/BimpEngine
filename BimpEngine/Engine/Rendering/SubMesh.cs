namespace BimpEngine.Engine.Rendering
{
    public class SubMesh
    {
        public List<int> Triangles { get; set; } = new List<int>();
        public int MaterialIndex { get; set; } = 0;
    }
}
