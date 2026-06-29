using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Math
{
    public class Vec3Data
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vec3Data() { }
        public Vec3Data(double x, double y, double z) { X = x; Y = y; Z = z; }
    }
}
