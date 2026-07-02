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

            // ── Cambiar a proyección 2D ortográfica ───────────────────
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.PushMatrix();
            gl.LoadIdentity();
            gl.Ortho(0, W, H, 0, -1, 1);

            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.PushMatrix();
            gl.LoadIdentity();

            gl.Disable(OpenGL.GL_DEPTH_TEST);
            gl.Disable(OpenGL.GL_LIGHTING);

            // ── Fondo circular semitransparente ───────────────────────
            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Color(0.08, 0.08, 0.10, 0.60);
            DrawFilledCircle(gl, cx, cy, Center - 1);
            gl.Disable(OpenGL.GL_BLEND);

            // ── Proyectar ejes según orientación actual de la cámara ──
            ProjectAxis(camera, 1, 0, 0, out double xPx, out double xPy);
            ProjectAxis(camera, 0, 1, 0, out double yPx, out double yPy);
            ProjectAxis(camera, 0, 0, 1, out double zPx, out double zPy);

            gl.LineWidth(2.0f);

            // Dibujar ejes negativos primero (quedan "detrás")
            DrawEje(gl, cx, cy, -(int)xPx, -(int)xPy, 0.40, 0.10, 0.10, false);
            DrawEje(gl, cx, cy, -(int)yPx, -(int)yPy, 0.10, 0.40, 0.10, false);
            DrawEje(gl, cx, cy, -(int)zPx, -(int)zPy, 0.10, 0.20, 0.50, false);

            // Dibujar ejes positivos encima
            DrawEje(gl, cx, cy, (int)xPx, (int)xPy, 0.90, 0.20, 0.20, true);
            DrawEje(gl, cx, cy, (int)yPx, (int)yPy, 0.20, 0.90, 0.20, true);
            DrawEje(gl, cx, cy, (int)zPx, (int)zPy, 0.20, 0.55, 1.00, true);

            gl.LineWidth(1.0f);

            // ── Restaurar estado ──────────────────────────────────────
            gl.Enable(OpenGL.GL_DEPTH_TEST);

            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.PopMatrix();
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.PopMatrix();
            gl.MatrixMode(OpenGL.GL_MODELVIEW);

            // ── Dibujar texto con GDI (OpenGL fijo no tiene texto) ────
            DibujarTextoGDI(glControl, camera, ox, oy);
        }

        // ── Proyección de un eje 3D al plano 2D del gizmo ────────────────────
        private void ProjectAxis(EditorCamera cam,
                                  double ax, double ay, double az,
                                  out double sx, out double sy)
        {
            double yaw = cam.Yaw * System.Math.PI / 180.0;
            double pitch = cam.Pitch * System.Math.PI / 180.0;

            // Vector right de la cámara
            double rx = System.Math.Cos(yaw);
            double rz = -System.Math.Sin(yaw);

            // Vector up de la cámara
            double upX = -System.Math.Sin(pitch) * System.Math.Sin(yaw);
            double upY = System.Math.Cos(pitch);
            double upZ = -System.Math.Sin(pitch) * System.Math.Cos(yaw);

            // Proyectar sobre right y up
            sx = (ax * rx + ay * 0 + az * rz) * AxisLen;
            sy = -(ax * upX + ay * upY + az * upZ) * AxisLen;
        }

        // ── Dibujar un eje (línea + círculo en la punta) ──────────────────────
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

        // ── Texto con GDI: etiquetas X Y Z + posición cámara ─────────────────
        private void DibujarTextoGDI(OpenGLControl glControl, EditorCamera camera,
                                      int ox, int oy)
        {
            if (glControl.InvokeRequired) return;

            using var gdi = glControl.CreateGraphics();

            // Fuente para etiquetas de ejes
            using var fuenteEje = new Font("Segoe UI", 7.5f, FontStyle.Bold);
            // Fuente para info de cámara
            using var fuenteInfo = new Font("Consolas", 7.5f);

            // ── Etiquetas X, Y, Z sobre el gizmo ─────────────────────
            int cx = ox + Center;
            int cy = oy + Center;

            ProjectAxis(camera, 1, 0, 0, out double xPx, out double xPy);
            ProjectAxis(camera, 0, 1, 0, out double yPx, out double yPy);
            ProjectAxis(camera, 0, 0, 1, out double zPx, out double zPy);

            DibujarLabel(gdi, fuenteEje, "X",
                cx + (int)xPx - 4, cy + (int)xPy - 6,
                Color.FromArgb(255, 80, 80));

            DibujarLabel(gdi, fuenteEje, "Y",
                cx + (int)yPx - 4, cy + (int)yPy - 6,
                Color.FromArgb(80, 220, 80));

            DibujarLabel(gdi, fuenteEje, "Z",
                cx + (int)zPx - 4, cy + (int)zPy - 6,
                Color.FromArgb(80, 160, 255));

            // ── "Persp" debajo del círculo ────────────────────────────
            int yBase = oy + GizmoSize + 4;

            DibujarLabel(gdi, fuenteEje, "Persp",
                ox + Center - 14, yBase,
                Color.FromArgb(180, 180, 180));

            // ── Info de posición y ángulos de la cámara ───────────────
            yBase += 16;

            var bgBrush = new SolidBrush(Color.FromArgb(140, 18, 18, 22));
            var txtBrush = new SolidBrush(Color.FromArgb(210, 210, 210));
            var dimBrush = new SolidBrush(Color.FromArgb(120, 120, 130));

            string[] lineas =
            {
                $"Pos  X {camera.EyeX:+0.0;-0.0;0.0}",
                $"     Y {camera.EyeY:+0.0;-0.0;0.0}",
                $"     Z {camera.EyeZ:+0.0;-0.0;0.0}",
                $"Yaw    {camera.Yaw:F1}°",
                $"Pitch  {camera.Pitch:F1}°",
                $"Dist   {camera.Distance:F1}",
            };

            // Fondo del bloque de texto
            int panelW = 100;
            int panelH = lineas.Length * 13 + 6;
            int panelX = ox + Center - panelW / 2;

            gdi.FillRectangle(bgBrush,
                new Rectangle(panelX - 2, yBase - 2, panelW + 4, panelH));

            for (int i = 0; i < lineas.Length; i++)
            {
                // Primera palabra en gris tenue, resto en blanco
                var partes = lineas[i].Split(' ', 2);
                gdi.DrawString(partes[0],
                    fuenteInfo, dimBrush,
                    panelX, yBase + i * 13);

                if (partes.Length > 1)
                    gdi.DrawString(partes[1],
                        fuenteInfo, txtBrush,
                        panelX + 32, yBase + i * 13);
            }

            bgBrush.Dispose();
            txtBrush.Dispose();
            dimBrush.Dispose();
        }

        private void DibujarLabel(Graphics g, Font font, string texto,
                                   int x, int y, Color color)
        {
            // Sombra para legibilidad
            using var sombra = new SolidBrush(Color.FromArgb(160, 0, 0, 0));
            g.DrawString(texto, font, sombra, x + 1, y + 1);
            using var brush = new SolidBrush(color);
            g.DrawString(texto, font, brush, x, y);
        }

        // ── Círculo relleno en OpenGL ─────────────────────────────────────────
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
