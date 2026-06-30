using BimpEngine.Engine.Project.Enum;
using System.Text.Json.Serialization;

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
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ProjectMode Mode { get; set; } = ProjectMode.Mode3D;
    }
}
