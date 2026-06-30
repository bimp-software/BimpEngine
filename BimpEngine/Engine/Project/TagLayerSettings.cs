using System.Text.Json;

namespace BimpEngine.Engine.Project
{
    public class TagLayerSettings
    {
        public static readonly IReadOnlyList<string> BuiltinTags = new[] 
        {
            "Untagged",
            "Player",
            "Enemy",
            "Respawn",
            "Finish",
            "EditorOnly",
            "MainCamera",
            "GameController"
        };

        public static readonly IReadOnlyList<LayerEntry> BuiltinLayers = new[]
        {
            new LayerEntry(0,  "Default"),
            new LayerEntry(1,  "TransparentFX"),
            new LayerEntry(2,  "Ignore Raycast"),
            new LayerEntry(3,  "Water"),
            new LayerEntry(4,  "UI"),
        };

        public List<string> CustomTags { get; set; } = new();

        public List<LayerEntry> CustomLayers { get; set; } = new();

        public IEnumerable<string> AllTags
        {
            get
            {
                foreach (var t in BuiltinTags) yield return t;
                foreach (var t in CustomTags) yield return t;
            }
        }

        public IEnumerable<LayerEntry> AllLayers
        {
            get
            {
                foreach (var l in BuiltinLayers) yield return l;
                foreach (var l in CustomLayers) yield return l;
            }
        }

        private static string FilePath(string projectFolder) =>
            Path.Combine(projectFolder, "ProjectSettings", "TagsAndLayers.json");

        public static TagLayerSettings Load(string projectFolder)
        {
            string path = FilePath(projectFolder);
            if (!File.Exists(path)) return new TagLayerSettings();

            try
            {
                var json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<TagLayerSettings>(json)
                       ?? new TagLayerSettings();
            }
            catch { return new TagLayerSettings(); }
        }

        public void Save(string projectFolder)
        {
            string path = FilePath(projectFolder);
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, JsonSerializer.Serialize(this,
                new JsonSerializerOptions { WriteIndented = true }));
        }

        private static string GlobalFile =>
            Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData),
                "BimpEngine", "DefaultTagsAndLayers.json");

        public static TagLayerSettings LoadGlobal()
        {
            if (!File.Exists(GlobalFile)) return new TagLayerSettings();
            try
            {
                var json = File.ReadAllText(GlobalFile);
                return JsonSerializer.Deserialize<TagLayerSettings>(json)
                       ?? new TagLayerSettings();
            }
            catch { return new TagLayerSettings(); }
        }

        public void SaveGlobal()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(GlobalFile)!);
            File.WriteAllText(GlobalFile, JsonSerializer.Serialize(this,
                new JsonSerializerOptions { WriteIndented = true }));
        }

        public bool AddTag(string tag)
        {
            tag = tag.Trim();
            if (string.IsNullOrEmpty(tag)) return false;
            if (IsBuiltinTag(tag)) return false;
            if (CustomTags.Contains(tag)) return false;
            CustomTags.Add(tag);
            return true;
        }

        public bool RemoveTag(string tag)
        {
            if (IsBuiltinTag(tag)) return false;
            return CustomTags.Remove(tag);
        }

        public bool AddLayer(string name)
        {
            name = name.Trim();
            if (string.IsNullOrEmpty(name)) return false;

            int nextIndex = NextFreeLayerIndex();
            if (nextIndex < 0) return false;   // máximo 32 layers

            CustomLayers.Add(new LayerEntry(nextIndex, name));
            return true;
        }

        public bool RemoveLayer(int index)
        {
            if (index < 5) return false;        // builtin protegidos
            int removed = CustomLayers.RemoveAll(l => l.Index == index);
            return removed > 0;
        }

        private bool IsBuiltinTag(string tag)
        {
            foreach (var t in BuiltinTags)
                if (t == tag) return true;
            return false;
        }

        private int NextFreeLayerIndex()
        {
            var used = new HashSet<int>();
            foreach (var l in BuiltinLayers) used.Add(l.Index);
            foreach (var l in CustomLayers) used.Add(l.Index);
            for (int i = 5; i < 32; i++)
                if (!used.Contains(i)) return i;
            return -1;
        }
    }
}
