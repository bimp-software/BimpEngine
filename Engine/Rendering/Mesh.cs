using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Rendering
{
    public class Mesh
    {
        public List<Vertex> Vertices { get; set; } = new List<Vertex>();
        public List<int> Triangles { get; set; } = new List<int>();
    }
}
