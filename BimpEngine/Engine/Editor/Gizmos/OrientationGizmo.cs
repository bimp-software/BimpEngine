using SharpGL;

namespace BimpEngine.Engine.Editor.Gizmos
{
    public class OrientationGizmo
    {
        private const int GizmoSize = 80;
        private const int Margin = 14;
        private const int AxisLen = 26;
        private const int Center = GizmoSize / 2;

        public void Draw(OpenGLControl glControl, EditorCamera camera)
        {
            var gl = glControl.OpenGL;
            int W = glControl.Width;
            int H = glControl.Height;

            int ox = W - GizmoSize - Margin;
            int oy = Margin;
            int cx = ox + Center;
            int cy = oy + Center;

            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.PushMatrix();
            gl.LoadIdentity();
            gl.Ortho(0, W, H, 0, -1, 1);

            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.PushMatrix();
            gl.LoadIdentity();

            gl.Disable(OpenGL.GL_DEPTH_TEST);
            gl.Disable(OpenGL.GL_LIGHTING);

            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Color(0.08, 0.08, 0.10, 0.60);
            DrawFilledCircle(gl, cx, cy, Center - 1);
            gl.Disable(OpenGL.GL_BLEND);

            ProjectAxis(camera, 1, 0, 0, out double xPx, out double xPy);
            ProjectAxis(camera, 0, 1, 0, out double yPx, out double yPy);
            ProjectAxis(camera, 0, 0, 1, out double zPx, out double zPy);

            gl.LineWidth(2.0f);

            DrawEje(gl, cx, cy, -(int)xPx, -(int)xPy, 0.40, 0.10, 0.10, false);
            DrawEje(gl, cx, cy, -(int)yPx, -(int)yPy, 0.10, 0.40, 0.10, false);
            DrawEje(gl, cx, cy, -(int)zPx, -(int)zPy, 0.10, 0.20, 0.50, false);

            DrawEje(gl, cx, cy, (int)xPx, (int)xPy, 0.90, 0.20, 0.20, true);
            DrawEje(gl, cx, cy, (int)yPx, (int)yPy, 0.20, 0.90, 0.20, true);
            DrawEje(gl, cx, cy, (int)zPx, (int)zPy, 0.20, 0.55, 1.00, true);

            gl.LineWidth(1.0f);

            gl.Enable(OpenGL.GL_DEPTH_TEST);

            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.PopMatrix();
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.PopMatrix();
            gl.MatrixMode(OpenGL.GL_MODELVIEW);

            DibujarTextoOpenGL(gl, camera, ox, oy, H);
        }

        private void ProjectAxis(EditorCamera cam,
                                  double ax, double ay, double az,
                                  out double sx, out double sy)
        {
            double yaw = cam.Yaw * System.Math.PI / 180.0;
            double pitch = cam.Pitch * System.Math.PI / 180.0;

            double rx = System.Math.Cos(yaw);
            double rz = -System.Math.Sin(yaw);

            double upX = -System.Math.Sin(pitch) * System.Math.Sin(yaw);
            double upY = System.Math.Cos(pitch);
            double upZ = -System.Math.Sin(pitch) * System.Math.Cos(yaw);

            sx = (ax * rx + ay * 0 + az * rz) * AxisLen;
            sy = -(ax * upX + ay * upY + az * upZ) * AxisLen;
        }

        private void DrawEje(OpenGL gl, int cx, int cy,
                              int ex, int ey,
                              double r, double g, double b,
                              bool positivo)
        {
            int tx = cx + ex;
            int ty = cy + ey;

            gl.Color(r, g, b);
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(cx, cy, 0);
            gl.Vertex(tx, ty, 0);
            gl.End();

            int radio = positivo ? 6 : 4;
            double alpha = positivo ? 1.0 : 0.45;
            gl.Color(r * alpha, g * alpha, b * alpha);
            DrawFilledCircle(gl, tx, ty, radio);
        }

        private void DibujarTextoOpenGL(OpenGL gl, EditorCamera camera, int ox, int oy, int screenH)
        {
            int cx = ox + Center;
            int cy = oy + Center;

            ProjectAxis(camera, 1, 0, 0, out double xPx, out double xPy);
            ProjectAxis(camera, 0, 1, 0, out double yPx, out double yPy);
            ProjectAxis(camera, 0, 0, 1, out double zPx, out double zPy);

            DrawText(gl, cx + (int)xPx - 4, cy + (int)xPy - 6, screenH,
                1.0f, 0.25f, 0.25f, "X");

            DrawText(gl, cx + (int)yPx - 4, cy + (int)yPy - 6, screenH,
                0.25f, 1.0f, 0.25f, "Y");

            DrawText(gl, cx + (int)zPx - 4, cy + (int)zPy - 6, screenH,
                0.25f, 0.65f, 1.0f, "Z");

            int yBase = oy + GizmoSize + 4;

            DrawText(gl, ox + Center - 14, yBase, screenH,
                0.8f, 0.8f, 0.8f, "Persp");

            yBase += 16;

            string[] lineas =
            {
                $"Pos X {camera.EyeX:+0.0;-0.0;0.0}",
                $"Pos Y {camera.EyeY:+0.0;-0.0;0.0}",
                $"Pos Z {camera.EyeZ:+0.0;-0.0;0.0}",
                $"Rot H {camera.Yaw:F1}°",
                $"Rot V {camera.Pitch:F1}°",
                $"Dist {camera.Distance:F1}",
            };

            int panelW = 100;
            int panelH = lineas.Length * 13 + 8;
            int panelX = ox + Center - panelW / 2;

            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);

            gl.Color(0.07, 0.07, 0.09, 0.65);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Vertex(panelX - 2, yBase - 2, 0);
            gl.Vertex(panelX + panelW + 2, yBase - 2, 0);
            gl.Vertex(panelX + panelW + 2, yBase + panelH, 0);
            gl.Vertex(panelX - 2, yBase + panelH, 0);
            gl.End();

            for (int i = 0; i < lineas.Length; i++)
            {
                DrawText(gl, panelX + 4, yBase + 2 + i * 13, screenH,
                    0.85f, 0.85f, 0.85f, lineas[i]);
            }

            gl.Disable(OpenGL.GL_BLEND);
        }

        private void DrawText(OpenGL gl, int x, int y, int screenH,
                      float r, float g, float b, string text)
        {
            gl.DrawText(
                x,
                screenH - y,
                r,
                g,
                b,
                "Consolas",
                9.0f,
                text);
        }

        private void DrawFilledCircle(OpenGL gl, int cx, int cy, int r)
        {
            gl.Begin(OpenGL.GL_TRIANGLE_FAN);
            gl.Vertex(cx, cy, 0);
            for (int i = 0; i <= 32; i++)
            {
                double angle = i * System.Math.PI * 2.0 / 32;
                gl.Vertex(cx + r * System.Math.Cos(angle),
                           cy + r * System.Math.Sin(angle), 0);
            }
            gl.End();
        }
    }
}
