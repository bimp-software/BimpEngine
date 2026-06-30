using BimpEngine.Engine.Math;

namespace BimpEngine.Engine.Rendering
{
    public class Vertex
    {
        public Vector3 vector = new Vector3();
        public Vector3 normal = new Vector3();
        public float U = 0f;
        public float V = 0f;

        public Vertex() 
        {
            vector.X = 0; vector.Y = 0; vector.Z = 0;
        }

        public Vertex(Vector3 vector)
        {
            this.vector = vector;
        }

        public Vertex(double a, double _length, double c)
        {
            vector.X = a; vector.Y = _length; vector.Z = c;
        }

        public Vertex(Vertex A)
        {
            vector.X = A.vector.X;
            vector.Y = A.vector.Y;
            vector.Z = A.vector.Z;
            normal = A.normal;
            U = A.U;
            V = A.V;
        }

        ~Vertex() { }
    }
}
