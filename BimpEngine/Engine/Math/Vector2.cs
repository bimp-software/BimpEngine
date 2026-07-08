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

        public static Vector2 Zero() => new Vector2(0, 0);
        public static Vector2 One() => new Vector2(1, 1);
        public static Vector2 Up() => new Vector2(0, 1);
        public static Vector2 Right() => new Vector2(0, -1);


        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
