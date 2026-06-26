using BimpEngine.Engine.Core;
using SharpGL;

namespace BimpEngine.Engine.Rendering
{
    public class Renderer
    {
        public bool ShowErrorsInConsole { get; set; } = true;
        public void DrawScene(OpenGLControl glControl, Scene scene)
        {
            if (glControl == null || scene == null) return;

            foreach (var obj in scene.Objetos)
            {
                if (obj == null || !obj.Enabled) continue;

                try
                {
                    obj.Draw(glControl);
                }
                catch (Exception ex)
                {
                    if (ShowErrorsInConsole)
                    {
                        Debug.Console.Error($"Error dibujando '{obj.Name}': {ex.Message}","Renderer");
                    }
                }
            }
        }
    }
}
