using BimpEngine.Engine.Entities.Primitive;
using BimpEngine.Engine.World;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Entities
{
    public static class PrimitiveFactory
    {
        public static Objetos Create(PrimitiveType tipo) => tipo switch
        {
            PrimitiveType.Cube => new Cube(),
            PrimitiveType.Sphere => new Sphere(),
            PrimitiveType.Cylinder => new Cylinder(),
            PrimitiveType.Plane => new Plane(),
            PrimitiveType.Cone => new Cone(),
            _ => new Cube()
        };
    }
}
