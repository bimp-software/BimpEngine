using SharpGL;
using SharpGL.SceneGraph.Assets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Rendering
{
    public class MeshRenderer
    {
        public Material Material { get; set; } = new Material();

        public void Draw(OpenGL gl, Mesh mesh)
        {
            if (mesh == null)
                return;

            gl.Color(
                Material.Color.R / 255.0,
                Material.Color.G / 255.0,
                Material.Color.B / 255.0
            );

            gl.Begin(OpenGL.GL_TRIANGLES);

            foreach (int index in mesh.Triangles)
            {
                var v = mesh.Vertices[index];
                gl.Vertex(v.vector.X, v.vector.Y, v.vector.Z);
            }

            gl.End();
        }
    }
}
