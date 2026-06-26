using SharpGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.UI
{
    public class ImageElement : UIElement
    {
        public Color Color { get; set; } = Color.White;

        public override void Draw(OpenGL gl)
        {
            var r = RectTransform;

            gl.Color(
                Color.R / 255.0,
                Color.G / 255.0,
                Color.B / 255.0,
                Color.A / 255.0
            );

            gl.Begin(OpenGL.GL_QUADS);

            gl.Vertex(r.Transform.X, r.Transform.Y);
            gl.Vertex(r.Transform.X + r.Width, r.Transform.Y);
            gl.Vertex(r.Transform.X + r.Width, r.Transform.Y + r.Height);
            gl.Vertex(r.Transform.X, r.Transform.Y + r.Height);

            gl.End();
        }
    }
}
