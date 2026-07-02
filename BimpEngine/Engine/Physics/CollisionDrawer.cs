using BimpEngine.Engine.World;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Physics
{
    public class CollisionDrawer
    {
        public void Draw(OpenGLControl glControl, Objetos obj)
        {
            if (obj == null)
                return;

            var collider = obj.GetComponent<BoxCollider>();

            if (collider == null || !collider.Enabled)
                return;

            DrawBoxCollider(glControl.OpenGL, obj, collider);
        }

        private void DrawBoxCollider(OpenGL gl, Objetos obj, BoxCollider collider)
        {
            gl.PushMatrix();

            gl.Translate(
                obj.Transform.Position.X + collider.Offset.X,
                obj.Transform.Position.Y + collider.Offset.Y,
                obj.Transform.Position.Z + collider.Offset.Z
            );

            gl.Rotate(obj.Transform.Rotation.X, 1, 0, 0);
            gl.Rotate(obj.Transform.Rotation.Y, 0, 1, 0);
            gl.Rotate(obj.Transform.Rotation.Z, 0, 0, 1);

            gl.Scale(
                obj.Transform.Scale.X * collider.Size.X,
                obj.Transform.Scale.Y * collider.Size.Y,
                obj.Transform.Scale.Z * collider.Size.Z
            );

            gl.Disable(OpenGL.GL_LIGHTING);
            gl.Color(0.0, 1.0, 0.0);
            gl.LineWidth(2);

            DrawWireCube(gl);

            gl.PopMatrix();
        }

        private void DrawWireCube(OpenGL gl)
        {
            double s = 0.5;

            double[,] v =
            {
            {-s,-s,-s}, {s,-s,-s}, {s,s,-s}, {-s,s,-s},
            {-s,-s, s}, {s,-s, s}, {s,s, s}, {-s,s, s}
        };

            int[,] edges =
            {
            {0,1},{1,2},{2,3},{3,0},
            {4,5},{5,6},{6,7},{7,4},
            {0,4},{1,5},{2,6},{3,7}
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
    }
}
