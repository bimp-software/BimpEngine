using BimpEngine.Engine.Rendering;
using BimpEngine.Engine.World;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace BimpEngine.Engine.Entities
{
    public abstract class PrimitiveObject : Objetos
    {
        protected PrimitiveObject(PrimitiveType primitiveType, string name, Mesh mesh)
        {
            Name = name;
            PrimitiveType = primitiveType;
            MeshFilter.Mesh = mesh;
            MeshFilter.Mesh.Name = name;
        }
    }
}
