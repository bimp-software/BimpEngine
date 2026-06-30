using SharpGL;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;

namespace BimpEngine.Engine.Rendering
{
    public static class TextureManager
    {
        private static readonly Dictionary<string, uint> _cache = new();

        public static uint GetOrLoad(OpenGL gl, string path)
        {
            if (string.IsNullOrEmpty(path))
                return 0;
            if(_cache.TryGetValue(path, out uint existing)) return existing;

            uint id = LoadTexture(gl, path);
            _cache[path] = id;
            return id;
        }

        private static uint LoadTexture(OpenGL gl, string path)
        {
            try
            {
                using var bitmap = new Bitmap(path);
                uint[] ids = new uint[1];
                gl.GenTextures(1, ids);
                uint textureId = ids[0];

                gl.BindTexture(OpenGL.GL_TEXTURE_2D, textureId);

                var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var data = bitmap.LockBits(rect, ImageLockMode.ReadOnly,PixelFormat.Format32bppArgb);

                gl.TexImage2D(
                    OpenGL.GL_TEXTURE_2D,0,(int)OpenGL.GL_RGBA,
                    bitmap.Width, bitmap.Height, 0,
                    OpenGL.GL_BGRA, OpenGL.GL_UNSIGNED_BYTE, data.Scan0
                    );
                bitmap.UnlockBits(data);

                gl.TexParameter(OpenGL.GL_TEXTURE_2D,OpenGL.GL_TEXTURE_MIN_FILTER,OpenGL.GL_LINEAR);
                gl.TexParameter(OpenGL.GL_TEXTURE_2D,OpenGL.GL_TEXTURE_MAG_FILTER,OpenGL.GL_LINEAR);
                gl.TexParameter(OpenGL.GL_TEXTURE_2D,OpenGL.GL_TEXTURE_WRAP_S,OpenGL.GL_REPEAT);
                gl.TexParameter(OpenGL.GL_TEXTURE_2D,OpenGL.GL_TEXTURE_WRAP_T,OpenGL.GL_REPEAT);

                gl.BindTexture(OpenGL.GL_TEXTURE_2D, 0);
                return textureId;
            }
            catch
            {
                return 0;
            }
        }

        public static void ClearCache(OpenGL gl)
        {
            foreach (var id in _cache.Values)
            {
                if (id != 0)
                {
                    uint[] ids = { id };
                    gl.DeleteTextures(1, ids);
                }
            }
            _cache.Clear();
        }
    }
}
