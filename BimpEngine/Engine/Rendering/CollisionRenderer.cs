using BimpEngine.Engine.World;
using SharpGL;

namespace BimpEngine.Engine.Rendering
{
    public class CollisionRenderer
    {
        public void Draw(OpenGLControl glControl, Objetos obj)
        {
            if (obj?.BoxCollider == null) return;
            if (!obj.BoxCollider.Enabled) return;

            DrawBoxCollider(glControl.OpenGL, obj);
        }

        private void DrawBoxCollider(OpenGL gl, Objetos obj)
        {
            var c = obj.BoxCollider;

            gl.PushMatrix();

            gl.Translate(
                obj.Transform.Position.X + c.Offset.X,
                obj.Transform.Position.Y + c.Offset.Y,
                obj.Transform.Position.Z + c.Offset.Z
            );

            gl.Rotate(obj.Transform.Rotation.X, 1, 0, 0);
            gl.Rotate(obj.Transform.Rotation.Y, 0, 1, 0);
            gl.Rotate(obj.Transform.Rotation.Z, 0, 0, 1);

            gl.Scale(
                c.Size.X,
                c.Size.Y,
                c.Size.Z
            );

            gl.Disable(OpenGL.GL_LIGHTING);
            gl.Disable(OpenGL.GL_TEXTURE_2D);

            DrawTransparentBox(gl);
            DrawWireCube(gl);
            DrawCornerPoints(gl);

            gl.PopMatrix();
        }

        private void DrawTransparentBox(OpenGL gl)
        {
            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);

            gl.Color(0.2, 1.0, 0.2, 0.08);

            double s = 0.5;

            gl.Begin(OpenGL.GL_QUADS);

            // Frente
            gl.Vertex(-s, -s, s); gl.Vertex(s, -s, s); gl.Vertex(s, s, s); gl.Vertex(-s, s, s);
            // Atrás
            gl.Vertex(-s, -s, -s); gl.Vertex(-s, s, -s); gl.Vertex(s, s, -s); gl.Vertex(s, -s, -s);
            // Arriba
            gl.Vertex(-s, s, -s); gl.Vertex(-s, s, s); gl.Vertex(s, s, s); gl.Vertex(s, s, -s);
            // Abajo
            gl.Vertex(-s, -s, -s); gl.Vertex(s, -s, -s); gl.Vertex(s, -s, s); gl.Vertex(-s, -s, s);
            // Derecha
            gl.Vertex(s, -s, -s); gl.Vertex(s, s, -s); gl.Vertex(s, s, s); gl.Vertex(s, -s, s);
            // Izquierda
            gl.Vertex(-s, -s, -s); gl.Vertex(-s, -s, s); gl.Vertex(-s, s, s); gl.Vertex(-s, s, -s);

            gl.End();

            gl.Disable(OpenGL.GL_BLEND);
        }

        private void DrawWireCube(OpenGL gl)
        {
            gl.Color(0.1, 1.0, 0.1);
            gl.LineWidth(3);

            double s = 0.5;

            double[,] v =
            {
                { -s, -s, -s }, { s, -s, -s }, { s, s, -s }, { -s, s, -s },
                { -s, -s,  s }, { s, -s,  s }, { s, s,  s }, { -s, s,  s }
            };

            int[,] edges =
            {
                { 0, 1 }, { 1, 2 }, { 2, 3 }, { 3, 0 },
                { 4, 5 }, { 5, 6 }, { 6, 7 }, { 7, 4 },
                { 0, 4 }, { 1, 5 }, { 2, 6 }, { 3, 7 }
            };

            gl.Begin(OpenGL.GL_LINES);

            for (int i = 0; i < edges.GetLength(0); i++)
            {
                int a = edges[i, 0];
                int b = edges[i, 1];

                gl.Vertex(v[a, 0], v[a, 1], v[a, 2]);
                gl.Vertex(v[b, 0], v[b, 1], v[b, 2]);
            }

            gl.End();
        }

        private void DrawCornerPoints(OpenGL gl)
        {
            gl.Color(0.0, 1.0, 0.0);
            gl.PointSize(7);

            double s = 0.5;

            gl.Begin(OpenGL.GL_POINTS);

            gl.Vertex(-s, -s, -s);
            gl.Vertex(s, -s, -s);
            gl.Vertex(s, s, -s);
            gl.Vertex(-s, s, -s);

            gl.Vertex(-s, -s, s);
            gl.Vertex(s, -s, s);
            gl.Vertex(s, s, s);
            gl.Vertex(-s, s, s);

            gl.End();
        }
    }
}