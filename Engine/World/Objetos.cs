using Engine.Core;
using Engine.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.World
{
    public abstract class Objetos
    {
        public string Name { get; set; } = "Objeto";

        public int Type { get; set; }

        public Transform Transform { get; set; } = new Transform();

        public MeshFilter Mesh { get; set; } = new MeshFilter();

        public MeshRenderer MeshRenderer { get; set; } = new MeshRenderer();

        protected Objetos() { }

        public virtual void Update() { }

        public abstract void Draw(OpenGLControl gl);
    }
}
