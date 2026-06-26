using BimpEngine.Engine.Rendering;
using BimpEngine.Engine.World;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Entities.Primitive
{
    public class Plane : PrimitiveObject
    {
        public Plane() : base(PrimitiveType.Plane,"Plane", PrimitiveMeshFactory.CreatePlane()) { }
    }
}
