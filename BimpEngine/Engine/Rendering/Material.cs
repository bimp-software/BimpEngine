using SharpGL.SceneGraph.Assets;

namespace BimpEngine.Engine.Rendering
{
    public class Material
    {
        public string Name { get; set; } = "Material";
        public Color Color { get; set; } = Color.White;
        public bool IsSolid { get; set; } = true;
        public Texture Texture { get; set; } = new Texture();
        public bool BlueprintMode { get; set; } = false;
        public string? TexturePath { get; set; }
        public uint TextureId { get; set; } = 0;
        public bool HasTexture => !string.IsNullOrEmpty(TexturePath);
        public float Shininess { get; set; } = 32.0f;
    }
}
