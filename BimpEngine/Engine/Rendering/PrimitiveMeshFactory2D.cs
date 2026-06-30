using System;

namespace BimpEngine.Engine.Rendering
{
    public static class PrimitiveMeshFactory2D
    {
        // ── Square ───────────────────────────────────────────────────────────
        public static Mesh CreateSquare(double size = 1.0)
        {
            var mesh = new Mesh();
            double h = size / 2.0;

            mesh.Vertices.Add(new Vertex(-h, -h, 0));  // 0
            mesh.Vertices.Add(new Vertex(-h, h, 0));  // 1
            mesh.Vertices.Add(new Vertex(h, h, 0));  // 2
            mesh.Vertices.Add(new Vertex(h, -h, 0));  // 3

            mesh.Triangles.AddRange(new[] { 0, 1, 2, 0, 2, 3 });
            mesh.Edges.AddRange(new[] { 0, 1, 1, 2, 2, 3, 3, 0 });

            return mesh;
        }

        // ── Rectangle ────────────────────────────────────────────────────────
        public static Mesh CreateRectangle(double width = 2.0, double height = 1.0)
        {
            var mesh = new Mesh();
            double hw = width / 2.0;
            double hh = height / 2.0;

            mesh.Vertices.Add(new Vertex(-hw, -hh, 0));  // 0
            mesh.Vertices.Add(new Vertex(-hw, hh, 0));  // 1
            mesh.Vertices.Add(new Vertex(hw, hh, 0));  // 2
            mesh.Vertices.Add(new Vertex(hw, -hh, 0));  // 3

            mesh.Triangles.AddRange(new[] { 0, 1, 2, 0, 2, 3 });
            mesh.Edges.AddRange(new[] { 0, 1, 1, 2, 2, 3, 3, 0 });

            return mesh;
        }

        // ── Circle ───────────────────────────────────────────────────────────
        public static Mesh CreateCircle(double radius = 0.5, int segments = 36)
        {
            var mesh = new Mesh();

            mesh.Vertices.Add(new Vertex(0, 0, 0));  // 0 = centro

            for (int i = 0; i < segments; i++)
            {
                double angle = i * System.Math.PI * 2.0 / segments;
                mesh.Vertices.Add(new Vertex(
                    System.Math.Cos(angle) * radius,
                    System.Math.Sin(angle) * radius,
                    0
                ));
            }

            for (int i = 1; i <= segments; i++)
            {
                int next = (i % segments) + 1;
                mesh.Triangles.AddRange(new[] { 0, i, next });
                mesh.Edges.AddRange(new[] { i, next });
            }

            return mesh;
        }

        // ── Triangle ─────────────────────────────────────────────────────────
        public static Mesh CreateTriangle(double size = 1.0)
        {
            var mesh = new Mesh();
            double h = size / 2.0;
            double apex = size * System.Math.Sqrt(3.0) / 2.0;

            mesh.Vertices.Add(new Vertex(-h, -apex / 3.0 * 2, 0));  // 0 abajo-izq
            mesh.Vertices.Add(new Vertex(h, -apex / 3.0 * 2, 0));  // 1 abajo-der
            mesh.Vertices.Add(new Vertex(0, apex / 3.0, 0));  // 2 arriba

            mesh.Triangles.AddRange(new[] { 0, 1, 2 });
            mesh.Edges.AddRange(new[] { 0, 1, 1, 2, 2, 0 });

            return mesh;
        }

        // ── Diamond ──────────────────────────────────────────────────────────
        public static Mesh CreateDiamond(double width = 1.0, double height = 1.5)
        {
            var mesh = new Mesh();
            double hw = width / 2.0;
            double hh = height / 2.0;

            mesh.Vertices.Add(new Vertex(-hw, 0, 0));  // 0 izq
            mesh.Vertices.Add(new Vertex(0, hh, 0));  // 1 arriba
            mesh.Vertices.Add(new Vertex(hw, 0, 0));  // 2 der
            mesh.Vertices.Add(new Vertex(0, -hh, 0));  // 3 abajo

            mesh.Triangles.AddRange(new[] { 0, 1, 2, 0, 2, 3 });
            mesh.Edges.AddRange(new[] { 0, 1, 1, 2, 2, 3, 3, 0 });

            return mesh;
        }

        // ── Pentagon ─────────────────────────────────────────────────────────
        public static Mesh CreatePolygon(int sides, double radius = 0.5)
        {
            if (sides < 3) sides = 3;
            var mesh = new Mesh();

            mesh.Vertices.Add(new Vertex(0, 0, 0));  // centro

            double startAngle = -System.Math.PI / 2.0;

            for (int i = 0; i < sides; i++)
            {
                double angle = startAngle + i * System.Math.PI * 2.0 / sides;
                mesh.Vertices.Add(new Vertex(
                    System.Math.Cos(angle) * radius,
                    System.Math.Sin(angle) * radius,
                    0
                ));
            }

            for (int i = 1; i <= sides; i++)
            {
                int next = (i % sides) + 1;
                mesh.Triangles.AddRange(new[] { 0, i, next });
                mesh.Edges.AddRange(new[] { i, next });
            }

            return mesh;
        }

        public static Mesh CreateStar(int points = 5, double outerRadius = 0.5, double innerRadius = 0.22)
        {
            var mesh = new Mesh();

            mesh.Vertices.Add(new Vertex(0, 0, 0));  // 0 = centro

            int total = points * 2;
            double startAngle = -System.Math.PI / 2.0;

            for (int i = 0; i < total; i++)
            {
                double angle = startAngle + i * System.Math.PI / points;
                double r = (i % 2 == 0) ? outerRadius : innerRadius;
                mesh.Vertices.Add(new Vertex(
                    System.Math.Cos(angle) * r,
                    System.Math.Sin(angle) * r,
                    0
                ));
            }

            for (int i = 1; i <= total; i++)
            {
                int next = (i % total) + 1;
                mesh.Triangles.AddRange(new[] { 0, i, next });
                mesh.Edges.AddRange(new[] { i, next });
            }

            return mesh;
        }

        public static Mesh CreateArrow(double length = 1.0, double width = 0.4)
        {
            var mesh = new Mesh();

            double hw = width / 2.0;
            double hl = length / 2.0;
            double bodyEnd = hl * 0.45;   // donde termina el cuerpo y empieza la punta
            double bodyWidth = hw * 0.45;

            mesh.Vertices.Add(new Vertex(-hl, bodyWidth, 0));  // 0
            mesh.Vertices.Add(new Vertex(-hl, -bodyWidth, 0));  // 1
            mesh.Vertices.Add(new Vertex(bodyEnd, -bodyWidth, 0));  // 2
            mesh.Vertices.Add(new Vertex(bodyEnd, bodyWidth, 0));  // 3
            mesh.Vertices.Add(new Vertex(bodyEnd, -hw, 0));    // 4  base inferior punta
            mesh.Vertices.Add(new Vertex(bodyEnd, hw, 0));    // 5  base superior punta
            mesh.Vertices.Add(new Vertex(hl, 0, 0));           // 6  vértice punta

            mesh.Triangles.AddRange(new[] { 0, 1, 2, 0, 2, 3 });
            mesh.Triangles.AddRange(new[] { 4, 6, 5 });
            mesh.Edges.AddRange(new[]
            {
                0, 1,  1, 2,  2, 4,  4, 6,  6, 5,  5, 3,  3, 0
            });

            return mesh;
        }

        public static Mesh CreateLine(double length = 1.0)
        {
            var mesh = new Mesh();
            double h = length / 2.0;

            mesh.Vertices.Add(new Vertex(-h, 0, 0)); 
            mesh.Vertices.Add(new Vertex(h, 0, 0));  

            mesh.Edges.AddRange(new[] { 0, 1 });

            return mesh;
        }
    }
}