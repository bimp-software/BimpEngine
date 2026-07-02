namespace BimpEngine.Engine.Math
{
    public class Vector3
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector3() { }

        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3 Zero => new Vector3(0, 0, 0);
        public static Vector3 One => new Vector3(1, 1, 1);
        public static Vector3 Up => new Vector3(0, 1, 0);
        public static Vector3 Right => new Vector3(1, 0, 0);
        public static Vector3 Forward => new Vector3(0, 0, 1);

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vector3 operator *(Vector3 a, double value)
        {
            return new Vector3(a.X * value, a.Y * value, a.Z * value);
        }

        public static Vector3 operator *(double value, Vector3 a)
        {
            return new Vector3(a.X * value, a.Y * value, a.Z * value);
        }

        public static Vector3 operator /(Vector3 a, double value)
        {
            if (value == 0)
                throw new DivideByZeroException("No se puede dividir un Vector3 por cero.");

            return new Vector3(a.X / value, a.Y / value, a.Z / value);
        }

        public static implicit operator Vector3((float x, float y, float z) value)
        {
            return new Vector3(value.x, value.y, value.z);
        }

        public static implicit operator Vector3((double x, double y, double z) value)
        {
            return new Vector3(value.x, value.y, value.z);
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }
    }
}