using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Project
{
    public class RecentProject
    {
        public string Name { get; set; } = "";
        public string Path { get; set; } = ""; 
        public DateTime LastOpened { get; set; } = DateTime.UtcNow;
    }
}
