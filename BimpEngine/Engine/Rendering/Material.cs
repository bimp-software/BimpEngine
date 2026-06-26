using SharpGL.SceneGraph.Assets;

namespace BimpEngine.Engine.Rendering
{
    public class Material
    {
        public Color Color { get; set; } = Color.White;
        public bool IsSolid { get; set; } = true;
        public bool HasTexture { get; set; }
        public Texture Texture { get; set; } = new Texture();
    }
}
