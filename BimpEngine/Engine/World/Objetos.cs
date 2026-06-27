using BimpEngine.Engine.Entities;
using BimpEngine.Engine.Core;
using BimpEngine.Engine.Rendering;
using SharpGL;

namespace BimpEngine.Engine.World
{
    public abstract class Objetos
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; set; } = "GameObject";
        public bool Enabled { get; set; } = true;
        public string Tag { get; set; } = "Untagged";
        public int Layer { get; set; } = 0;
        public PrimitiveType PrimitiveType { get; set; }
        public Transform Transform { get; set; } = new Transform();
        public MeshFilter MeshFilter { get; set; } = new MeshFilter();
        public MeshRenderer MeshRenderer { get; set; } = new MeshRenderer();
        public bool IsSelected { get; set; }
        protected Objetos() { }
        public virtual void Update() { }
        public virtual void Draw(OpenGLControl glControl)
        {
            var gl = glControl.OpenGL;

            gl.PushMatrix();

            gl.Translate(
                Transform.Position.X,
                Transform.Position.Y,
                Transform.Position.Z);

            gl.Rotate(Transform.Rotation.X, 1, 0, 0);
            gl.Rotate(Transform.Rotation.Y, 0, 1, 0);
            gl.Rotate(Transform.Rotation.Z, 0, 0, 1);

            gl.Scale(
                Transform.Scale.X,
                Transform.Scale.Y,
                Transform.Scale.Z);

            MeshRenderer.Draw(gl, MeshFilter.Mesh, IsSelected);

            gl.PopMatrix();
        }
    }
}
