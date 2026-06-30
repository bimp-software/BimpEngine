using BimpEngine.Engine.Rendering;
using BimpEngine.Engine.World;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Entities.Primitive
{
    public class Cylinder : PrimitiveObject
    {
        public Cylinder() : base(PrimitiveType.Cylinder,"Cylinder", PrimitiveMeshFactory.CreateCylinder()) { }
    }
}
