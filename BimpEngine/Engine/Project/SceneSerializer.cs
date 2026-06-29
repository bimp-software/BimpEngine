using BimpEngine.Engine.Core;
using BimpEngine.Engine.Entities;
using BimpEngine.Engine.Entities.Primitive;
using BimpEngine.Engine.Math;
using BimpEngine.Engine.World;
using System.Text.Json;

namespace BimpEngine.Engine.Project
{
    public static class SceneSerializer
    {
        private static readonly JsonSerializerOptions JsonOpts = new()
        {
            WriteIndented = true
        };

        // ── Save ─────────────────────────────────────────────────────────
        public static void Save(Scene scene, string filePath)
        {
            // Flatten: collect root objects and ALL their descendants
            var all = new List<Objetos>();
            foreach (var obj in scene.Objetos)
                CollectAll(obj, all);

            var data = new SceneData { Name = scene.Name };

            foreach (var obj in all)
            {
                var dto = new ObjectData
                {
                    Id = obj.Id,
                    Name = obj.Name,
                    PrimitiveType = obj.PrimitiveType.ToString(),
                    Enabled = obj.Enabled,
                    Tag = obj.Tag,
                    Layer = obj.Layer,
                    BlueprintMode = obj.MeshRenderer?.Material?.BlueprintMode ?? false,
                    Position = new Vec3Data(obj.Transform.Position.X,
                                                 obj.Transform.Position.Y,
                                                 obj.Transform.Position.Z),
                    Rotation = new Vec3Data(obj.Transform.Rotation.X,
                                                 obj.Transform.Rotation.Y,
                                                 obj.Transform.Rotation.Z),
                    Scale = new Vec3Data(obj.Transform.Scale.X,
                                                 obj.Transform.Scale.Y,
                                                 obj.Transform.Scale.Z),
                    ParentId = obj.Parent?.Id,
                    ChildrenIds = obj.Children.Select(c => c.Id).ToList()
                };
                data.Objects.Add(dto);
            }

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            File.WriteAllText(filePath, JsonSerializer.Serialize(data, JsonOpts));
        }

        // ── Load ─────────────────────────────────────────────────────────
        public static Scene Load(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<SceneData>(json, JsonOpts)
                       ?? throw new InvalidOperationException("Archivo de escena inválido.");

            var scene = new Scene { Name = data.Name };

            // First pass: instantiate all objects indexed by Id
            var map = new Dictionary<Guid, Objetos>();
            foreach (var dto in data.Objects)
            {
                var obj = CreateObject(dto);
                map[dto.Id] = obj;
            }

            // Second pass: rebuild parent-child relationships
            foreach (var dto in data.Objects)
            {
                if (dto.ParentId.HasValue && map.TryGetValue(dto.ParentId.Value, out var parent))
                {
                    var child = map[dto.Id];
                    // Use internal AddChild but DON'T add to scene root
                    parent.AddChild(child);
                }
            }

            // Third pass: add only root objects to the scene
            foreach (var dto in data.Objects)
            {
                if (dto.ParentId == null)
                    scene.Objetos.Add(map[dto.Id]);
            }

            return scene;
        }

        // ── Helpers ───────────────────────────────────────────────────────
        private static void CollectAll(Objetos obj, List<Objetos> list)
        {
            list.Add(obj);
            foreach (var child in obj.Children)
                CollectAll(child, list);
        }

        private static Objetos CreateObject(ObjectData dto)
        {
            Enum.TryParse<PrimitiveType>(dto.PrimitiveType, out var type);

            Objetos obj = type switch
            {
                PrimitiveType.Cube => new Cube(),
                PrimitiveType.Sphere => new Sphere(),
                PrimitiveType.Cylinder => new Cylinder(),
                PrimitiveType.Plane => new Plane(),
                PrimitiveType.Cone => new Cone(),
                PrimitiveType.Circle => new Circle(),
                PrimitiveType.Triangle => new Triangle(),
                _ => new Cube()
            };

            // Restore identity fields using reflection-safe setters
            SetId(obj, dto.Id);
            obj.Name = dto.Name;
            obj.Enabled = dto.Enabled;
            obj.Tag = dto.Tag;
            obj.Layer = dto.Layer;

            obj.Transform.Position.X = dto.Position.X;
            obj.Transform.Position.Y = dto.Position.Y;
            obj.Transform.Position.Z = dto.Position.Z;
            obj.Transform.Rotation.X = dto.Rotation.X;
            obj.Transform.Rotation.Y = dto.Rotation.Y;
            obj.Transform.Rotation.Z = dto.Rotation.Z;
            obj.Transform.Scale.X = dto.Scale.X;
            obj.Transform.Scale.Y = dto.Scale.Y;
            obj.Transform.Scale.Z = dto.Scale.Z;

            obj.MeshRenderer.Material.BlueprintMode = dto.BlueprintMode;

            return obj;
        }

        /// <summary>
        /// The Id property has a private setter generated by { get; } = Guid.NewGuid().
        /// We use reflection to restore the saved Id.
        /// </summary>
        private static void SetId(Objetos obj, Guid id)
        {
            var field = typeof(Objetos)
                .GetField("<Id>k__BackingField",
                          System.Reflection.BindingFlags.Instance |
                          System.Reflection.BindingFlags.NonPublic);
            field?.SetValue(obj, id);
        }

        // ── Empty scene template ──────────────────────────────────────────
        public static string EmptySceneJson(string name)
        {
            var data = new SceneData { Name = name };
            return JsonSerializer.Serialize(data, JsonOpts);
        }
    }
}
