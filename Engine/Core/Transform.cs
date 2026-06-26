using Engine.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Core
{
    public class Transform
    {
        public Vector3 Position { get; set; } = new Vector3();

        public Vector3 Rotation { get; set; } = new Vector3();

        public Vector3 Scale { get; set; } = new Vector3(1, 1, 1);
    }
}
