using SharpGL;

namespace BimpEngine.Engine.Rendering
{
    public class MeshRenderer
    {
        public Material Material { get; set; } = new Material();

        public void Draw(OpenGL gl, Mesh mesh, bool isSelected = false)
        {
            if (mesh == null) return;

            if (Material.BlueprintMode)
                DrawBlueprint(gl, mesh, isSelected);
            else
                DrawSolid(gl, mesh, isSelected);
        }

        private void DrawSolid(OpenGL gl, Mesh mesh, bool isSelected)
        {
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

        private void DrawBlueprint(OpenGL gl, Mesh mesh, bool isSelected)
        {
            // 1. Caras rellenas con azul oscuro semisólido
            gl.Enable(OpenGL.GL_POLYGON_OFFSET_FILL);
            gl.PolygonOffset(1f, 1f);

            gl.Color(0.05, 0.12, 0.28); // azul muy oscuro

            gl.Begin(OpenGL.GL_TRIANGLES);
            foreach (int index in mesh.Triangles)
            {
                var v = mesh.Vertices[index];
                gl.Vertex(v.vector.X, v.vector.Y, v.vector.Z);
            }
            gl.End();

            gl.Disable(OpenGL.GL_POLYGON_OFFSET_FILL);

            // 2. Aristas en azul claro
            gl.Color(0.3, 0.7, 1.0);
            gl.LineWidth(1.2f);

            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_LINE);
            gl.Begin(OpenGL.GL_TRIANGLES);
            foreach (int index in mesh.Triangles)
            {
                var v = mesh.Vertices[index];
                gl.Vertex(v.vector.X, v.vector.Y, v.vector.Z);
            }
            gl.End();
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);

            // 3. Si está seleccionado, bounding box en naranja
            if (isSelected)
                DrawSelectionBox(gl, mesh);
        }

        private void DrawSelectionBox(OpenGL gl, Mesh mesh)
        {
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

            double o = 0.02;
            minX -= o; minY -= o; minZ -= o;
            maxX += o; maxY += o; maxZ += o;

            gl.Color(1.0, 0.5, 0.0);
            gl.LineWidth(1.5f);

            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(minX, minY, minZ); gl.Vertex(maxX, minY, minZ);
            gl.Vertex(maxX, minY, minZ); gl.Vertex(maxX, minY, maxZ);
            gl.Vertex(maxX, minY, maxZ); gl.Vertex(minX, minY, maxZ);
            gl.Vertex(minX, minY, maxZ); gl.Vertex(minX, minY, minZ);
            gl.Vertex(minX, maxY, minZ); gl.Vertex(maxX, maxY, minZ);
            gl.Vertex(maxX, maxY, minZ); gl.Vertex(maxX, maxY, maxZ);
            gl.Vertex(maxX, maxY, maxZ); gl.Vertex(minX, maxY, maxZ);
            gl.Vertex(minX, maxY, maxZ); gl.Vertex(minX, maxY, minZ);
            gl.Vertex(minX, minY, minZ); gl.Vertex(minX, maxY, minZ);
            gl.Vertex(maxX, minY, minZ); gl.Vertex(maxX, maxY, minZ);
            gl.Vertex(maxX, minY, maxZ); gl.Vertex(maxX, maxY, maxZ);
            gl.Vertex(minX, minY, maxZ); gl.Vertex(minX, maxY, maxZ);
            gl.End();
        }
    }
}