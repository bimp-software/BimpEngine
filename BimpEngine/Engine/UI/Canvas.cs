using BimpEngine.Engine.World;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.UI
{
    public class Canvas : Objetos
    {
        public List<UIElement> Elements { get; } = new List<UIElement>();

        public Canvas()
        {
            Name = "Canvas";
        }

        public void Add(UIElement element)
        {
            if (element != null)
                Elements.Add(element);
        }

        public void Draw(OpenGLControl control)
        {
            OpenGL gl = control.OpenGL;

            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.PushMatrix();
            gl.LoadIdentity();
            gl.Ortho2D(0, control.Width, control.Height, 0);

            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.PushMatrix();
            gl.LoadIdentity();

            gl.Disable(OpenGL.GL_DEPTH_TEST);

            foreach (var element in Elements)
            {
                if (element.Enabled)
                    element.Draw(gl);
            }

            gl.Enable(OpenGL.GL_DEPTH_TEST);

            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.PopMatrix();

            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.PopMatrix();

            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        public void MouseMove(Point mouse)
        {
            foreach (var element in Elements)
                element.OnMouseMove(mouse);
        }

        public void MouseDown(Point mouse)
        {
            foreach (var element in Elements)
                element.OnMouseDown(mouse);
        }

        public void MouseUp(Point mouse)
        {
            foreach (var element in Elements)
                element.OnMouseUp(mouse);
        }
    }
}
