using Assimp;
using BimpEngine.Engine.Math;
using BimpEngine.Engine.Rendering;

namespace BimpEngine.Engine.Assets
{
    public static class ModelImporter
    {
        private static readonly string[] ExtensionesSoportadas =
        {
            ".fbx", ".obj", ".dae", ".gltf", ".glb", ".3ds", ".blend", ".stl"
        };

        public static bool EsSoportado(string path)
        {
            string ext = Path.GetExtension(path).ToLowerInvariant();
            return ExtensionesSoportadas.Contains(ext);
        }

        public static ImportedModel Import(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"No se encontró el archivo: {path}");

            using var context = new AssimpContext();

            var scene = context.ImportFile(path,
                PostProcessSteps.Triangulate |
                PostProcessSteps.GenerateSmoothNormals |
                PostProcessSteps.JoinIdenticalVertices |
                PostProcessSteps.ImproveCacheLocality |
                PostProcessSteps.ValidateDataStructure |
                PostProcessSteps.FlipUVs);

            if (scene == null || !scene.HasMeshes)
                throw new InvalidDataException($"El archivo no contiene mallas válidas: {path}");

            var resultado = new ImportedModel { SourcePath = path };
            var baseDir = Path.GetDirectoryName(path) ?? "";

            foreach (var mat in scene.Materials)
            {
                var importedMat = new ImportedMaterial
                {
                    Name = string.IsNullOrWhiteSpace(mat.Name) ? "Material" : mat.Name,
                    Shininess = mat.HasShininess ? mat.Shininess : 32f
                };

                if (mat.HasColorDiffuse)
                {
                    var c = mat.ColorDiffuse;
                    importedMat.DiffuseColor = System.Drawing.Color.FromArgb(
                        (int)(System.Math.Clamp(c.A, 0, 1) * 255),
                        (int)(System.Math.Clamp(c.R, 0, 1) * 255),
                        (int)(System.Math.Clamp(c.G, 0, 1) * 255),
                        (int)(System.Math.Clamp(c.B, 0, 1) * 255));
                }

                if (mat.HasTextureDiffuse)
                {
                    string texFile = mat.TextureDiffuse.FilePath;
                    string resolved = ResolverRutaTextura(texFile, baseDir);
                    if (resolved != null)
                        importedMat.TexturePath = resolved;
                }

                resultado.Materials.Add(importedMat);
            }

            if (resultado.Materials.Count == 0)
                resultado.Materials.Add(new ImportedMaterial());

            var meshFinal = new Rendering.Mesh
            {
                Name = Path.GetFileNameWithoutExtension(path),
                IsImported = true
            };

            var subMeshesPorMaterial = new Dictionary<int, SubMesh>();

            void ProcesarNodo(Node node, Assimp.Matrix4x4 parentTransform)
            {
                var transform = node.Transform * parentTransform;

                foreach (int meshIndex in node.MeshIndices)
                {
                    var aMesh = scene.Meshes[meshIndex];
                    int materialIndex = aMesh.MaterialIndex;

                    if (!subMeshesPorMaterial.TryGetValue(materialIndex, out var subMesh))
                    {
                        subMesh = new SubMesh { MaterialIndex = materialIndex };
                        subMeshesPorMaterial[materialIndex] = subMesh;
                        meshFinal.SubMeshes.Add(subMesh);
                    }

                    int baseIndex = meshFinal.Vertices.Count;

                    for (int i = 0; i < aMesh.VertexCount; i++)
                    {
                        var pos = aMesh.Vertices[i];
                        var p = TransformPoint(transform, pos);

                        var vertex = new Vertex(p.X, p.Y, p.Z);

                        if (aMesh.HasNormals)
                        {
                            var n = aMesh.Normals[i];
                            vertex.normal = new Vector3(n.X, n.Y, n.Z);
                        }

                        if (aMesh.HasTextureCoords(0))
                        {
                            var uv = aMesh.TextureCoordinateChannels[0][i];
                            vertex.U = uv.X;
                            vertex.V = uv.Y;
                        }

                        meshFinal.Vertices.Add(vertex);
                    }

                    foreach (var face in aMesh.Faces)
                    {
                        if (face.IndexCount != 3) continue;

                        int i0 = baseIndex + face.Indices[0];
                        int i1 = baseIndex + face.Indices[1];
                        int i2 = baseIndex + face.Indices[2];

                        meshFinal.Triangles.Add(i0);
                        meshFinal.Triangles.Add(i1);
                        meshFinal.Triangles.Add(i2);

                        subMesh.Triangles.Add(i0);
                        subMesh.Triangles.Add(i1);
                        subMesh.Triangles.Add(i2);
                    }
                }

                foreach (var child in node.Children)
                    ProcesarNodo(child, transform);
            }

            ProcesarNodo(scene.RootNode, Assimp.Matrix4x4.Identity);

            resultado.Mesh = meshFinal;
            return resultado;
        }

        private static (double X, double Y, double Z) TransformPoint(Assimp.Matrix4x4 m, Assimp.Vector3D v)
        {
            double x = m.A1 * v.X + m.A2 * v.Y + m.A3 * v.Z + m.A4;
            double y = m.B1 * v.X + m.B2 * v.Y + m.B3 * v.Z + m.B4;
            double z = m.C1 * v.X + m.C2 * v.Y + m.C3 * v.Z + m.C4;
            return (x, y, z);
        }

        private static string? ResolverRutaTextura(string texFile, string baseDir)
        {
            if (string.IsNullOrWhiteSpace(texFile))
                return null;

            if (texFile.StartsWith("*"))
                return null;

            string limpio = texFile.Replace('\\', Path.DirectorySeparatorChar)
                                    .Replace('/', Path.DirectorySeparatorChar);

            string candidato = Path.Combine(baseDir, limpio);
            if (File.Exists(candidato))
                return Path.GetFullPath(candidato);

            string soloNombre = Path.GetFileName(limpio);
            string candidato2 = Path.Combine(baseDir, soloNombre);
            if (File.Exists(candidato2))
                return Path.GetFullPath(candidato2);

            foreach (var sub in new[] { "textures", "Textures", "tex", "Tex" })
            {
                string candidato3 = Path.Combine(baseDir, sub, soloNombre);
                if (File.Exists(candidato3))
                    return Path.GetFullPath(candidato3);
            }

            return null;
        }
    }
}
