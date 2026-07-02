using BimpEngine.Engine.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Physics
{
    public abstract class ColliderComponent
    {
        public bool Enabled { get; set; } = true;
        public bool IsTrigger { get; set; } = false;
        public Vector3 Offset { get; set; } = new Vector3(0,0,0);
        public abstract ColliderType Type { get; }
    }
}
