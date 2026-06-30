using BimpEngine.Engine.Editor;
using SharpGL;

namespace BimpEngine.Engine.Rendering
{
    public class Grid2D
    {
        public int Lines { get; set; } = 50;

        public void Draw(OpenGL gl, EditorCamera2D cam, int viewportWidth, int viewportHeight)
        {
            if (viewportHeight == 0) return;

            double aspect = (double)viewportWidth / viewportHeight;
            double halfW = cam.Zoom * aspect;
            double halfH = cam.Zoom;

            double rawStep = ChooseStep(cam.Zoom);

            double left = cam.PanX - halfW;
            double right = cam.PanX + halfW;
            double bottom = cam.PanY - halfH;
            double top = cam.PanY + halfH;

            int startX = (int)System.Math.Floor(left / rawStep);
            int endX = (int)System.Math.Ceiling(right / rawStep);
            int startY = (int)System.Math.Floor(bottom / rawStep);
            int endY = (int)System.Math.Ceiling(top / rawStep);

            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.LineWidth(1f);

            gl.Color(0.28, 0.28, 0.28);
            gl.Begin(OpenGL.GL_LINES);

            for (int x = startX; x <= endX; x++)
            {
                if (IsMajor(x)) continue;
                double wx = x * rawStep;
                gl.Vertex(wx, bottom, 0.0);
                gl.Vertex(wx, top, 0.0);
            }

            for (int y = startY; y <= endY; y++)
            {
                if (IsMajor(y)) continue;
                double wy = y * rawStep;
                gl.Vertex(left, wy, 0.0);
                gl.Vertex(right, wy, 0.0);
            }

            gl.End();

            gl.LineWidth(1.5f);
            gl.Color(0.42, 0.42, 0.42);
            gl.Begin(OpenGL.GL_LINES);

            for (int x = startX; x <= endX; x++)
            {
                if (!IsMajor(x)) continue;
                double wx = x * rawStep;
                gl.Vertex(wx, bottom, 0.0);
                gl.Vertex(wx, top, 0.0);
            }

            for (int y = startY; y <= endY; y++)
            {
                if (!IsMajor(y)) continue;
                double wy = y * rawStep;
                gl.Vertex(left, wy, 0.0);
                gl.Vertex(right, wy, 0.0);
            }

            gl.End();

            gl.LineWidth(2f);

            gl.Color(0.55, 0.18, 0.18);
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(left, 0.0, 0.0);
            gl.Vertex(right, 0.0, 0.0);
            gl.End();

            gl.Color(0.18, 0.50, 0.18);
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(0.0, bottom, 0.0);
            gl.Vertex(0.0, top, 0.0);
            gl.End();

            gl.LineWidth(1f);
        }

        private static double ChooseStep(double zoom)
        {
            double raw = zoom / 5.0;
            double exp = System.Math.Pow(10, System.Math.Floor(System.Math.Log10(raw)));
            double norm = raw / exp;

            if (norm < 2) return exp;
            if (norm < 5) return 2 * exp;
            return 5 * exp;
        }

        private static bool IsMajor(int index) => index % 10 == 0;
    }
}
