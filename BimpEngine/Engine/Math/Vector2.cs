using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Math
{
    public class Vector2
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2() { }

        public Vector2(double X, double Y)
        {
            this.X = X; this.Y = Y;
        }
    }
}
