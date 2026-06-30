using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Entities
{
    public abstract class PrimitiveObject2D : World.Objetos
    {
        public PrimitiveType2D PrimitiveType2D { get; }

        protected PrimitiveObject2D(
            PrimitiveType2D type2D,
            string name,
            Rendering.Mesh mesh)
        {
            PrimitiveType2D = type2D;
            Name = name;
            MeshFilter.Mesh = mesh;
            MeshFilter.Mesh.Name = name;
        }
    }
}
