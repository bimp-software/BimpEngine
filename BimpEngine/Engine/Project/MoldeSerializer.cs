using BimpEngine.Engine.Entities;
using BimpEngine.Engine.Entities.Primitive;
using BimpEngine.Engine.Math;
using BimpEngine.Engine.World;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BimpEngine.Engine.Project
{
    public static class MoldeSerializer
    {
        public const string MoldeExtension = ".bmold";

        private static readonly JsonSerializerOptions JsonOpts = new()
        {
            WriteIndented = true
        };

        public static void Crear(Objetos objeto, string filePath)
        {
            var all = new List<Objetos>();
            CollectAll(objeto, all);

            var data = new MoldeData
            {
                Name = objeto.Name,
                Objects = all.Select(ToObjectData).ToList()
            };

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            File.WriteAllText(filePath, JsonSerializer.Serialize(data, JsonOpts));

            objeto.Molde = new MoldeLink
            {
                MoldeFilePath = filePath,
                MoldeId = data.MoldeId
            };
        }

        public static Objetos Instanciar(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<MoldeData>(json, JsonOpts)
                       ?? throw new InvalidOperationException("Archivo de Molde inválido.");

            var map = new Dictionary<Guid, Objetos>();
            foreach (var dto in data.Objects)
                map[dto.Id] = FromObjectData(dto);

            Objetos? raiz = null;
            foreach (var dto in data.Objects)
            {
                var obj = map[dto.Id];

                ReasignarId(obj);

                obj.Molde = new MoldeLink { MoldeFilePath = filePath, MoldeId = data.MoldeId };

                if (dto.ParentId.HasValue && map.TryGetValue(dto.ParentId.Value, out var parent))
                    parent.AddChild(obj);
                else
                    raiz = obj;
            }

            return raiz ?? throw new InvalidOperationException("El Molde no tiene un objeto raíz válido.");
        }

        public static void AplicarCambios(Objetos instancia)
        {
            if (instancia.Molde?.MoldeFilePath == null)
                throw new InvalidOperationException("Este objeto no es una instancia de ningún Molde.");

            var all = new List<Objetos>();
            CollectAll(instancia, all);

            var data = new MoldeData
            {
                MoldeId = instancia.Molde.MoldeId ?? Guid.NewGuid(),
                Name = instancia.Name,
                Objects = all.Select(ToObjectData).ToList()
            };

            File.WriteAllText(instancia.Molde.MoldeFilePath,
                JsonSerializer.Serialize(data, JsonOpts));
        }

        public static Objetos Revertir(Objetos instancia)
        {
            if (instancia.Molde?.MoldeFilePath == null)
                throw new InvalidOperationException("Este objeto no es una instancia de ningún Molde.");

            return Instanciar(instancia.Molde.MoldeFilePath);
        }

        private static void CollectAll(Objetos obj, List<Objetos> list)
        {
            list.Add(obj);
            foreach (var child in obj.Children)
                CollectAll(child, list);
        }

        private static ObjectData ToObjectData(Objetos obj)
        {
            bool is2D = obj is PrimitiveObject2D;

            return new ObjectData
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
                Position = new Vec3Data(obj.Transform.Position.X, obj.Transform.Position.Y, obj.Transform.Position.Z),
                Rotation = new Vec3Data(obj.Transform.Rotation.X, obj.Transform.Rotation.Y, obj.Transform.Rotation.Z),
                Scale = new Vec3Data(obj.Transform.Scale.X, obj.Transform.Scale.Y, obj.Transform.Scale.Z),
                ParentId = obj.Parent?.Id,
                ChildrenIds = obj.Children.Select(c => c.Id).ToList()
            };
        }

        private static Objetos FromObjectData(ObjectData dto)
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

            SetId(obj, dto.Id);

            return obj;
        }

        private static void SetId(Objetos obj, Guid id)
        {
            var field = typeof(Objetos).GetField("<Id>k__BackingField",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            field?.SetValue(obj, id);
        }

        private static void ReasignarId(Objetos obj) => SetId(obj, Guid.NewGuid());
    }
}
