using BimpEngine.Engine.Core;
using BimpEngine.Engine.Entities;
using BimpEngine.Engine.Math;
using BimpEngine.Engine.Physics;
using BimpEngine.Engine.Rendering;
using BimpEngine.Engine.Scripting;
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
        public Objetos Parent { get; set; } = null;
        public List<Objetos> Children { get; } = new List<Objetos>();
        public MoldeLink? Molde { get; set; } = null;
        public BoxCollider BoxCollider { get; set; }
        public List<Object> Components { get; } = new();

        public NodeGraph Script { get; set; } = new NodeGraph();
        public List<ScriptComponent> Scripts { get; } = new();

        protected Objetos() { }
        public virtual void Update() { }
        public virtual void Draw(OpenGLControl glControl) => Draw(glControl, true);
        public virtual void Draw(OpenGLControl glControl, bool mostrarGizmosEditor)
        {
            var gl = glControl.OpenGL;

            CargarTexturasSiFaltan(gl);

            gl.PushMatrix();
            gl.Translate(Transform.Position.X, Transform.Position.Y, Transform.Position.Z);
            gl.Rotate(Transform.Rotation.X, 1, 0, 0);
            gl.Rotate(Transform.Rotation.Y, 0, 1, 0);
            gl.Rotate(Transform.Rotation.Z, 0, 0, 1);

            gl.PushMatrix();
            gl.Scale(Transform.Scale.X, Transform.Scale.Y, Transform.Scale.Z);
            MeshRenderer.Draw(gl, MeshFilter.Mesh, IsSelected);
            gl.PopMatrix();

            foreach (var child in Children)
                if (child.Enabled)
                    child.Draw(glControl);

            gl.PopMatrix();
        }

        private void CargarTexturasSiFaltan(OpenGL gl)
        {
            if (MeshRenderer.Material.HasTexture && MeshRenderer.Material.TextureId == 0)
            {
                MeshRenderer.Material.TextureId = TextureManager.GetOrLoad(gl, MeshRenderer.Material.TexturePath);

                foreach (var mat in MeshRenderer.Materials)
                {
                    if (mat.HasTexture && mat.TextureId == 0)
                        mat.TextureId = TextureManager.GetOrLoad(gl, mat.TexturePath);
                }
            }
        }

        public virtual Objetos Clone()
        {
            var clon = (Objetos)MemberwiseClone();
            clon.Transform = new Transform
            {
                Position = new Vector3(Transform.Position.X + 0.2, Transform.Position.Y, Transform.Position.Z),
                Rotation = new Vector3(Transform.Rotation.X, Transform.Rotation.Y, Transform.Rotation.Z),
                Scale = new Vector3(Transform.Scale.X, Transform.Scale.Y, Transform.Scale.Z)
            };
            return clon;
        }

        public void AddChild(Objetos child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        public void RemoveChild(Objetos child)
        {
            child.Parent = null;
            Children.Remove(child);
        }

        public T GetComponent<T>() where T : class
        {
            return Components.OfType<T>().FirstOrDefault();
        }

        public void AddComponent(object component)
        {
            Components.Add(component);
        }
    }
}