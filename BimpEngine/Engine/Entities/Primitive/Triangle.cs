using BimpEngine.Engine.Rendering;
using BimpEngine.Engine.World;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Entities.Primitive
{
    public class Triangle : PrimitiveObject
    {
        public Triangle() : base(PrimitiveType.Triangle,"Triangle",PrimitiveMeshFactory.CreateTriangle()) { }
    }
}
