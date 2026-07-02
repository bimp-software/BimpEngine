using BimpEngine.Engine.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Physics
{
    public class BoxCollider : ColliderComponent
    {
        public bool AutoFitToMesh { get; set; } = true;
        public Vector3 Size { get; set; } = new Vector3(1, 1, 1);

        public override ColliderType Type => ColliderType.Box;
    }
}
