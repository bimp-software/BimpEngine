using SharpGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Editor
{
    public class EditorCamera2D
    {
        public double PanX { get; set; } = 0;
        public double PanY { get; set; } = 0;

        public double Zoom { get; set; } = 8.0;

        public double ZoomMin { get; set; } = 0.5;
        public double ZoomMax { get; set; } = 500.0;
        public double ZoomSpeed { get; set; } = 0.12;
        public double PanSpeed { get; set; } = 0.005;

        public void Apply(OpenGL gl, int viewportWidth, int viewportHeight)
        {
            if (viewportHeight == 0) viewportHeight = 1;
            double aspect = (double)viewportWidth / viewportHeight;

            double halfH = Zoom;
            double halfW = halfH * aspect;

            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            gl.Ortho(
                PanX - halfW, PanX + halfW,   
                PanY - halfH, PanY + halfH,   
                -1000.0, 1000.0              
            );

            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();
        }

        public void DoZoom(int delta)
        {
            double factor = delta > 0
                ? 1.0 - ZoomSpeed
                : 1.0 + ZoomSpeed;

            Zoom = System.Math.Clamp(Zoom * factor, ZoomMin, ZoomMax);
        }

        public void DoPan(int dx, int dy, int viewportWidth, int viewportHeight)
        {
            if (viewportHeight == 0) return;
            double aspect = (double)viewportWidth / viewportHeight;

            double worldPerPixelX = (Zoom * 2 * aspect) / viewportWidth;
            double worldPerPixelY = (Zoom * 2) / viewportHeight;

            PanX -= dx * worldPerPixelX;
            PanY += dy * worldPerPixelY; 
        }

        public (double wx, double wy) ScreenToWorld(int sx, int sy, int viewportWidth, int viewportHeight)
        {
            double aspect = viewportWidth == 0 ? 1 : (double)viewportWidth / viewportHeight;
            double halfW = Zoom * aspect;
            double halfH = Zoom;

            double wx = PanX + ((sx / (double)viewportWidth) * 2.0 - 1.0) * halfW;
            double wy = PanY - ((sy / (double)viewportHeight) * 2.0 - 1.0) * halfH;
            return (wx, wy);
        }
    }
}
