using SharpGL;

namespace BimpEngine.Engine.Rendering
{
    public class MeshRenderer
    {
        public Material Material { get; set; } = new Material();

        public void Draw(OpenGL gl, Mesh mesh, bool isSelected = false)
        {
            if (mesh == null) return;

            gl.Color(
                Material.Color.R / 255.0,
                Material.Color.G / 255.0,
                Material.Color.B / 255.0
            );

            gl.Begin(OpenGL.GL_TRIANGLES);
            foreach (int index in mesh.Triangles)
            {
                var v = mesh.Vertices[index];
                gl.Vertex(v.vector.X, v.vector.Y, v.vector.Z);
            }
            gl.End();

            if (isSelected)
                DrawSelectionBox(gl, mesh);
        }

        private void DrawSelectionBox(OpenGL gl, Mesh mesh)
        {
            // Calcular AABB
            double minX = double.MaxValue, minY = double.MaxValue, minZ = double.MaxValue;
            double maxX = double.MinValue, maxY = double.MinValue, maxZ = double.MinValue;

            foreach (var v in mesh.Vertices)
            {
                if (v.vector.X < minX) minX = v.vector.X;
                if (v.vector.Y < minY) minY = v.vector.Y;
                if (v.vector.Z < minZ) minZ = v.vector.Z;
                if (v.vector.X > maxX) maxX = v.vector.X;
                if (v.vector.Y > maxY) maxY = v.vector.Y;
                if (v.vector.Z > maxZ) maxZ = v.vector.Z;
            }

            // Expandir levemente para que no quede pegado al mesh
            double offset = 0.02;
            minX -= offset; minY -= offset; minZ -= offset;
            maxX += offset; maxY += offset; maxZ += offset;

            gl.Color(1.0, 0.5, 0.0); // naranja
            gl.LineWidth(1.5f);

            gl.Begin(OpenGL.GL_LINES);

            // Cara inferior
            gl.Vertex(minX, minY, minZ); gl.Vertex(maxX, minY, minZ);
            gl.Vertex(maxX, minY, minZ); gl.Vertex(maxX, minY, maxZ);
            gl.Vertex(maxX, minY, maxZ); gl.Vertex(minX, minY, maxZ);
            gl.Vertex(minX, minY, maxZ); gl.Vertex(minX, minY, minZ);

            // Cara superior
            gl.Vertex(minX, maxY, minZ); gl.Vertex(maxX, maxY, minZ);
            gl.Vertex(maxX, maxY, minZ); gl.Vertex(maxX, maxY, maxZ);
            gl.Vertex(maxX, maxY, maxZ); gl.Vertex(minX, maxY, maxZ);
            gl.Vertex(minX, maxY, maxZ); gl.Vertex(minX, maxY, minZ);

            // Aristas verticales
            gl.Vertex(minX, minY, minZ); gl.Vertex(minX, maxY, minZ);
            gl.Vertex(maxX, minY, minZ); gl.Vertex(maxX, maxY, minZ);
            gl.Vertex(maxX, minY, maxZ); gl.Vertex(maxX, maxY, maxZ);
            gl.Vertex(minX, minY, maxZ); gl.Vertex(minX, maxY, maxZ);

            gl.End();
        }
    }
}
