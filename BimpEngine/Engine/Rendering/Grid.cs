using SharpGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Rendering
{
    public class Grid
    {
        public int VisibleRange { get; set; } = 40;
        public double Step { get; set; } = 1.0;
        public int MajorLineEvery { get; set; } = 10;

        public void Draw(OpenGLControl control, double cameraX, double cameraZ)
        {
            OpenGL gl = control.OpenGL;

            int centerX = (int)System.Math.Floor(cameraX / Step);
            int centerZ = (int)System.Math.Floor(cameraZ / Step);

            int startX = centerX - VisibleRange;
            int endX = centerX + VisibleRange;
            int startZ = centerZ - VisibleRange;
            int endZ = centerZ + VisibleRange;

            gl.Disable(OpenGL.GL_TEXTURE_2D);

            DrawMinorLines(gl, startX, endX, startZ, endZ);
            DrawMajorLines(gl, startX, endX, startZ, endZ);
        }

        private void DrawMinorLines(OpenGL gl, int startX, int endX, int startZ, int endZ)
        {
            gl.LineWidth(1f);
            gl.Color(0.28, 0.28, 0.28);

            gl.Begin(OpenGL.GL_LINES);

            for (int x = startX; x <= endX; x++)
            {
                if (IsMajorLine(x))
                    continue;

                double worldX = x * Step;

                gl.Vertex(worldX, 0, startZ * Step);
                gl.Vertex(worldX, 0, endZ * Step);
            }

            for (int z = startZ; z <= endZ; z++)
            {
                if (IsMajorLine(z))
                    continue;

                double worldZ = z * Step;

                gl.Vertex(startX * Step, 0, worldZ);
                gl.Vertex(endX * Step, 0, worldZ);
            }

            gl.End();
        }

        private void DrawMajorLines(OpenGL gl, int startX, int endX, int startZ, int endZ)
        {
            gl.LineWidth(1.5f);
            gl.Color(0.42, 0.42, 0.42);

            gl.Begin(OpenGL.GL_LINES);

            for (int x = startX; x <= endX; x++)
            {
                if (!IsMajorLine(x))
                    continue;

                double worldX = x * Step;

                gl.Vertex(worldX, 0, startZ * Step);
                gl.Vertex(worldX, 0, endZ * Step);
            }

            for (int z = startZ; z <= endZ; z++)
            {
                if (!IsMajorLine(z))
                    continue;

                double worldZ = z * Step;

                gl.Vertex(startX * Step, 0, worldZ);
                gl.Vertex(endX * Step, 0, worldZ);
            }

            gl.End();
        }

        private bool IsMajorLine(int value)
        {
            return value % MajorLineEvery == 0;
        }
    }
}
