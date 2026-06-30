using BimpEngine.Engine.Project.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace BimpEngine.Engine.Project
{
    public static class ProjectManager
    {
        public const string ProjectExtension = ".bimp";
        public const string SceneExtension = ".bscene";

        private static readonly string AppDataFolder =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                         "BimpEngine");
        private static readonly string RecentFile =
            Path.Combine(AppDataFolder, "recent_projects.json");

        public static string? CurrentProjectFolder { get; private set; }
        public static ProjectInfo? CurrentProject { get; private set; }
        public static bool HasOpenProject => CurrentProject != null && CurrentProjectFolder != null;

        public static ProjectMode CurrentMode =>
            CurrentProject?.Mode ?? ProjectMode.Mode3D;

        public static List<RecentProject> GetRecentProjects()
        {
            if (!File.Exists(RecentFile)) return new List<RecentProject>();
            try
            {
                var json = File.ReadAllText(RecentFile);
                return JsonSerializer.Deserialize<List<RecentProject>>(json)
                       ?? new List<RecentProject>();
            }
            catch { return new List<RecentProject>(); }
        }

        private static void AddToRecent(string folder, string name)
        {
            Directory.CreateDirectory(AppDataFolder);
            var list = GetRecentProjects();
            list.RemoveAll(r => r.Path == folder);
            list.Insert(0, new RecentProject { Name = name, Path = folder, LastOpened = DateTime.UtcNow });
            if (list.Count > 10) list = list.Take(10).ToList();
            File.WriteAllText(RecentFile, JsonSerializer.Serialize(list,
                new JsonSerializerOptions { WriteIndented = true }));
        }

        public static string CreateProject(
            string parentFolder,
            string projectName,
            ProjectMode mode = ProjectMode.Mode3D)
        {
            string folder = Path.Combine(parentFolder, projectName);
            if (Directory.Exists(folder))
                throw new InvalidOperationException(
                    $"Ya existe una carpeta llamada '{projectName}' en esa ubicación.");

            Directory.CreateDirectory(folder);
            Directory.CreateDirectory(Path.Combine(folder, "Scenes"));
            Directory.CreateDirectory(Path.Combine(folder, "Assets"));
            Directory.CreateDirectory(Path.Combine(folder, "Exports"));

            var info = new ProjectInfo
            {
                Name = projectName,
                Author = Environment.UserName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                MainScene = "Scenes/Main.bscene",
                Mode = mode
            };
            WriteProjectFile(folder, info);

            string scenePath = Path.Combine(folder, "Scenes", "Main.bscene");
            File.WriteAllText(scenePath, SceneSerializer.EmptySceneJson("Main"));

            CurrentProjectFolder = folder;
            CurrentProject = info;

            AddToRecent(folder, projectName);

            Directory.CreateDirectory(Path.Combine(folder, "ProjectSettings"));

            var globalDefaults = TagLayerSettings.LoadGlobal();
            globalDefaults.Save(folder);
            TagLayerManager.Load(folder);

            return folder;
        }

        public static ProjectInfo OpenProject(string folder)
        {
            string bimpFile = FindBimpFile(folder)
                ?? throw new FileNotFoundException(
                    $"No se encontró un archivo .bimp en:\n{folder}");

            var json = File.ReadAllText(bimpFile);
            var info = JsonSerializer.Deserialize<ProjectInfo>(json)
                       ?? throw new InvalidOperationException(
                           "El archivo .bimp está corrupto o vacío.");

            CurrentProjectFolder = folder;
            CurrentProject = info;

            AddToRecent(folder, info.Name);
            TagLayerManager.Load(folder);
            return info;
        }

        public static void SaveProjectInfo()
        {
            if (CurrentProject == null || CurrentProjectFolder == null) return;
            CurrentProject.UpdatedAt = DateTime.UtcNow;
            WriteProjectFile(CurrentProjectFolder, CurrentProject);
        }

        public static string GetMainScenePath()
        {
            if (CurrentProjectFolder == null || CurrentProject == null)
                throw new InvalidOperationException("No hay un proyecto abierto.");

            return Path.Combine(CurrentProjectFolder,
                CurrentProject.MainScene.Replace('/', Path.DirectorySeparatorChar));
        }

        public static string GetScenesFolder()
        {
            if (CurrentProjectFolder == null)
                throw new InvalidOperationException("No hay un proyecto abierto.");
            return Path.Combine(CurrentProjectFolder, "Scenes");
        }

        private static void WriteProjectFile(string folder, ProjectInfo info)
        {
            string bimpFile = Path.Combine(folder, info.Name + ProjectExtension);
            foreach (var old in Directory.GetFiles(folder, "*" + ProjectExtension))
                if (Path.GetFileName(old) != Path.GetFileName(bimpFile))
                    File.Delete(old);

            File.WriteAllText(bimpFile, JsonSerializer.Serialize(info,
                new JsonSerializerOptions { WriteIndented = true }));
        }

        public static string? FindBimpFile(string folder)
        {
            var files = Directory.GetFiles(folder, "*" + ProjectExtension);
            return files.Length > 0 ? files[0] : null;
        }

        public static void Close()
        {
            CurrentProjectFolder = null;
            CurrentProject = null;
            TagLayerManager.Reset();
        }
    }
}