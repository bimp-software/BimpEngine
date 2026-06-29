using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Project
{
    public class SceneData
    {
        public string Name { get; set; } = "Scene";
        public List<ObjectData> Objects { get; set; } = new();
    }
}
