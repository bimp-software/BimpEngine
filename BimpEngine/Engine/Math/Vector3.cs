using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Math
{
    public class Vector3
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector3() { }

        public Vector3(double X, double Y, double Z)
        {
            this.X = X; this.Y = Y; this.Z = Z;
        }
    }
}
