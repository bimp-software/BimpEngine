namespace BimpEngine.Engine.Assets
{
    public class ImportedMaterial
    {
        public string Name { get; set; } = "Material";
        public System.Drawing.Color DiffuseColor { get; set; } = System.Drawing.Color.White;
        public string? TexturePath { get; set; } 
        public float Shininess { get; set; } = 32f;
    }
}
