using BimpEngine.Engine.Rendering;
using BimpEngine.Engine.World;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Entities.Primitive
{
    public class Sphere : PrimitiveObject
    {
        public Sphere() : base(PrimitiveType.Sphere,"Sphere",PrimitiveMeshFactory.CreateSphere()) { }
    }
}
