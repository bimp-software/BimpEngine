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

        public static void Save(Scene scene, string filePath)
        {
            var all = new List<Objetos>();
            foreach (var obj in scene.Objetos)
                CollectAll(obj, all);

            var data = new SceneData { Name = scene.Name };

            foreach (var obj in all)
            {
                bool is2D = obj is PrimitiveObject2D;

                var dto = new ObjectData
                {
                    Id = obj.Id,
                    Name = obj.Name,
                    PrimitiveType = is2D
                        ? ((PrimitiveObject2D)obj).PrimitiveType2D.ToString()
                        : obj.PrimitiveType.ToString(),
                    Is2D = is2D,
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

        public static Scene Load(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<SceneData>(json, JsonOpts)
                       ?? throw new InvalidOperationException("Archivo de escena inválido.");

            var scene = new Scene { Name = data.Name };

            var map = new Dictionary<Guid, Objetos>();
            foreach (var dto in data.Objects)
            {
                var obj = CreateObject(dto);
                map[dto.Id] = obj;
            }

            foreach (var dto in data.Objects)
            {
                if (dto.ParentId.HasValue && map.TryGetValue(dto.ParentId.Value, out var parent))
                {
                    var child = map[dto.Id];
                    parent.AddChild(child);
                }
            }

            foreach (var dto in data.Objects)
            {
                if (dto.ParentId == null)
                    scene.Objetos.Add(map[dto.Id]);
            }

            return scene;
        }

        private static void CollectAll(Objetos obj, List<Objetos> list)
        {
            list.Add(obj);
            foreach (var child in obj.Children)
                CollectAll(child, list);
        }

        private static Objetos CreateObject(ObjectData dto)
        {
            Objetos obj;

            if (dto.Is2D)
            {
                System.Enum.TryParse<PrimitiveType2D>(dto.PrimitiveType, out var type2D);
                obj = PrimitiveFactory2D.Create(type2D);
            }
            else
            {
                System.Enum.TryParse<PrimitiveType>(dto.PrimitiveType, out var type);

                obj = type switch
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
            }

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

        private static void SetId(Objetos obj, Guid id)
        {
            var field = typeof(Objetos)
                .GetField("<Id>k__BackingField",
                          System.Reflection.BindingFlags.Instance |
                          System.Reflection.BindingFlags.NonPublic);
            field?.SetValue(obj, id);
        }

        public static string EmptySceneJson(string name)
        {
            var data = new SceneData { Name = name };
            return JsonSerializer.Serialize(data, JsonOpts);
        }
    }
}