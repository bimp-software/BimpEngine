namespace BimpEngine.Engine.Project
{
    public static class TagLayerManager
    {
        private static TagLayerSettings _current = new();
        private static string? _projectFolder;

        public static TagLayerSettings Current => _current;

        public static void Load(string projectFolder)
        {
            _projectFolder = projectFolder;

            // Fusionar defaults globales con los del proyecto
            var global = TagLayerSettings.LoadGlobal();
            var project = TagLayerSettings.Load(projectFolder);

            // Los custom tags globales que no estén en el proyecto se agregan
            foreach (var t in global.CustomTags)
                if (!project.CustomTags.Contains(t))
                    project.CustomTags.Add(t);

            foreach (var l in global.CustomLayers)
                if (!project.CustomLayers.Exists(x => x.Index == l.Index))
                    project.CustomLayers.Add(l);

            _current = project;
        }

        public static void Save()
        {
            if (_projectFolder == null) return;
            _current.Save(_projectFolder);
        }

        public static void SaveAsGlobal() => _current.SaveGlobal();

        public static void Reset()
        {
            _current = new TagLayerSettings();
            _projectFolder = null;
        }
    }
}
