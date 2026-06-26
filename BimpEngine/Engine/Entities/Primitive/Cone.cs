using BimpEngine.Engine.Rendering;
using BimpEngine.Engine.World;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Entities.Primitive
{
    public class Cone : PrimitiveObject
    {
        public Cone() : base(PrimitiveType.Cone,"Cone", PrimitiveMeshFactory.CreateCone()) { }
    }
}
