using BimpEngine.Engine.World;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Entities
{
    public class CameraObject : Objetos
    {
        public double FieldOfView { get; set; } = 60;
        public double NearClip { get; set; } = 0.1;
        public double FarClip { get; set; } = 1000;

        public CameraObject()
        {
            Name = "Camera";
        }

        public override void Draw(OpenGLControl glControl)
        {
            var gl = glControl.OpenGL;

            gl.PushMatrix();

            gl.Translate(
                Transform.Position.X,
                Transform.Position.Y,
                Transform.Position.Z);

            gl.Rotate(Transform.Rotation.X, 1, 0, 0);
            gl.Rotate(Transform.Rotation.Y, 0, 1, 0);
            gl.Rotate(Transform.Rotation.Z, 0, 0, 1);

            DrawCameraIcon(gl);

            gl.PopMatrix();
        }

        private void DrawCameraIcon(OpenGL gl)
        {
            gl.LineWidth(2);
            gl.Color(1.0, 1.0, 0.0);

            gl.Begin(OpenGL.GL_LINES);

            // cuerpo
            gl.Vertex(-0.5, -0.3, 0);
            gl.Vertex(0.5, -0.3, 0);

            gl.Vertex(0.5, -0.3, 0);
            gl.Vertex(0.5, 0.3, 0);

            gl.Vertex(0.5, 0.3, 0);
            gl.Vertex(-0.5, 0.3, 0);

            gl.Vertex(-0.5, 0.3, 0);
            gl.Vertex(-0.5, -0.3, 0);

            // lente
            gl.Vertex(0.5, 0.2, 0);
            gl.Vertex(1.0, 0.5, 0);

            gl.Vertex(0.5, -0.2, 0);
            gl.Vertex(1.0, -0.5, 0);

            gl.Vertex(1.0, 0.5, 0);
            gl.Vertex(1.0, -0.5, 0);

            // dirección
            gl.Vertex(0, 0, 0);
            gl.Vertex(0, 0, -2);

            gl.End();
        }
    }
}
