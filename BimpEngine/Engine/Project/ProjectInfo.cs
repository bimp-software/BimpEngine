using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Project
{
    public class ProjectInfo
    {
        public string Name { get; set; } = "New Project";
        public string Version { get; set; } = "1.0";
        public string Author { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>Relative path to the main scene file inside the project folder.</summary>
        public string MainScene { get; set; } = "Scenes/Main.bscene";
    }
}
