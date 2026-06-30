using BimpEngine.Engine.Rendering;
using BimpEngine.Engine.World;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Entities.Primitive
{
    public class Cube : PrimitiveObject
    {
        public Cube() : base(PrimitiveType.Cube,"Cube", PrimitiveMeshFactory.CreateCube()) { }
    }
}
