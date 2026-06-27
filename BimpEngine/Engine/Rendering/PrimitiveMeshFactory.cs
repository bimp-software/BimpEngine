using BimpEngine.Engine.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Rendering
{
    public static class PrimitiveMeshFactory
    {
        public static Mesh CreateCube(double size = 2.0)
        {
            Mesh mesh = new Mesh();

            double h = size / 2.0;

            mesh.Vertices.Add(new Vertex(-h, -h, -h)); // 0
            mesh.Vertices.Add(new Vertex(-h, h, -h)); // 1
            mesh.Vertices.Add(new Vertex(h, h, -h)); // 2
            mesh.Vertices.Add(new Vertex(h, -h, -h)); // 3

            mesh.Vertices.Add(new Vertex(-h, -h, h)); // 4
            mesh.Vertices.Add(new Vertex(-h, h, h)); // 5
            mesh.Vertices.Add(new Vertex(h, h, h)); // 6
            mesh.Vertices.Add(new Vertex(h, -h, h)); // 7

            mesh.Triangles.AddRange(new int[]
            {
                0,1,2, 0,2,3,
                4,6,5, 4,7,6,
                0,4,5, 0,5,1,
                3,2,6, 3,6,7,
                1,5,6, 1,6,2,
                0,3,7, 0,7,4
            });

            return mesh;
        }

        public static Mesh CreateTriangle(double size = 2.0)
        {
            Mesh mesh = new Mesh();

            double h = size / 2.0;
            double apex = size * 0.816; 
            mesh.Vertices.Add(new Vertex(0, -h, h));      
            mesh.Vertices.Add(new Vertex(-h, -h, -h * 0.5));     
            mesh.Vertices.Add(new Vertex(h, -h, -h * 0.5));    
            mesh.Vertices.Add(new Vertex(0, apex - h, 0));         

            mesh.Triangles.AddRange(new int[]
            {
                0, 2, 1,
                0, 1, 3,
                1, 2, 3,
                2, 0, 3,
            });

            return mesh;
        }

        public static Mesh CreatePlane(double size = 2)
        {
            Mesh mesh = new Mesh();
            double h = size / 2.0;

            mesh.Vertices.Add(new Vertex(-h, 0, -h)); // 0
            mesh.Vertices.Add(new Vertex(-h, 0, h));  // 1
            mesh.Vertices.Add(new Vertex(h, 0, h));   // 2
            mesh.Vertices.Add(new Vertex(h, 0, -h));  // 3

            mesh.Triangles.AddRange(new int[]
            {
                0, 1, 2,
                0, 2, 3
            });

            return mesh;
        }

        public static Mesh CreateCircle(double radius = 1, int segments = 32)
        {
            Mesh mesh = new Mesh();

            mesh.Vertices.Add(new Vertex(0, 0, 0)); // centro

            for (int i = 0; i < segments; i++)
            {
                double angle = i * System.Math.PI * 2.0 / segments;
                mesh.Vertices.Add(new Vertex(
                    System.Math.Cos(angle) * radius,
                    0,
                    System.Math.Sin(angle) * radius
                ));
            }

            for (int i = 1; i <= segments; i++)
            {
                int next = i == segments ? 1 : i + 1;
                mesh.Triangles.AddRange(new int[] { 0, i, next });
            }

            return mesh;
        }

        public static Mesh CreateCylinder(double radius = 1, double height = 2, int segments = 32)
        {
            Mesh mesh = new Mesh();
            double h = height / 2.0;

            int bottomCenter = 0;
            int topCenter = 1;

            mesh.Vertices.Add(new Vertex(0, -h, 0));
            mesh.Vertices.Add(new Vertex(0, h, 0));

            for (int i = 0; i < segments; i++)
            {
                double angle = i * System.Math.PI * 2.0 / segments;
                double x = System.Math.Cos(angle) * radius;
                double z = System.Math.Sin(angle) * radius;

                mesh.Vertices.Add(new Vertex(x, -h, z)); // bottom
                mesh.Vertices.Add(new Vertex(x, h, z));  // top
            }

            for (int i = 0; i < segments; i++)
            {
                int currentBottom = 2 + i * 2;
                int currentTop = currentBottom + 1;

                int nextBottom = 2 + ((i + 1) % segments) * 2;
                int nextTop = nextBottom + 1;

                // tapa inferior
                mesh.Triangles.AddRange(new int[]
                {
                    bottomCenter, nextBottom, currentBottom
                });

                // tapa superior
                mesh.Triangles.AddRange(new int[]
                {
                    topCenter, currentTop, nextTop
                });

                // lateral
                mesh.Triangles.AddRange(new int[]
                {
                    currentBottom, nextBottom, currentTop,
                    currentTop, nextBottom, nextTop
                });
            }

            return mesh;
        }

        public static Mesh CreateCone(double radius = 1, double height = 2, int segments = 32)
        {
            Mesh mesh = new Mesh();
            double h = height / 2.0;

            int bottomCenter = 0;
            int topVertex = 1;

            mesh.Vertices.Add(new Vertex(0, -h, 0));
            mesh.Vertices.Add(new Vertex(0, h, 0));

            for (int i = 0; i < segments; i++)
            {
                double angle = i * System.Math.PI * 2.0 / segments;

                mesh.Vertices.Add(new Vertex(
                    System.Math.Cos(angle) * radius,
                    -h,
                    System.Math.Sin(angle) * radius
                ));
            }

            for (int i = 0; i < segments; i++)
            {
                int current = 2 + i;
                int next = 2 + ((i + 1) % segments);

                // base
                mesh.Triangles.AddRange(new int[]
                {
                    bottomCenter, next, current
                });

                // lateral
                mesh.Triangles.AddRange(new int[]
                {
                    topVertex, current, next
                });
            }

            return mesh;
        }

        public static Mesh CreateSphere(double radius = 1, int segments = 32, int rings = 16)
        {
            Mesh mesh = new Mesh();

            for (int y = 0; y <= rings; y++)
            {
                double v = (double)y / rings;
                double phi = v * System.Math.PI;

                for (int x = 0; x <= segments; x++)
                {
                    double u = (double)x / segments;
                    double theta = u * System.Math.PI * 2.0;

                    double px = System.Math.Cos(theta) * System.Math.Sin(phi) * radius;
                    double py = System.Math.Cos(phi) * radius;
                    double pz = System.Math.Sin(theta) * System.Math.Sin(phi) * radius;

                    mesh.Vertices.Add(new Vertex(px, py, pz));
                }
            }

            for (int y = 0; y < rings; y++)
            {
                for (int x = 0; x < segments; x++)
                {
                    int a = y * (segments + 1) + x;
                    int b = a + segments + 1;
                    int c = b + 1;
                    int d = a + 1;

                    mesh.Triangles.AddRange(new int[]
                    {
                        a, b, d,
                        d, b, c
                    });
                }
            }

            return mesh;
        }
    }
}
