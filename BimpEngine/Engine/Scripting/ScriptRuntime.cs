using BimpEngine.Engine.Scripting.Engines;
using BimpEngine.Engine.Scripting.Enum;
using BimpEngine.Engine.World;
using System;

namespace BimpEngine.Engine.Scripting
{
    public static class ScriptRuntime
    {
        public static void Init(Objetos objeto, ScriptComponent comp, Core.Scene scene)
        {
            comp.Engine = comp.Language switch
            {
                Language.CSharp => new CSharpScriptEngine(),
                Language.Lua => new LuaScriptEngine(),
                Language.Python => new PythonScriptEngine(),
                _ => new CSharpScriptEngine()
            };

            var context = new ScriptContext
            {
                Owner = objeto,
                Scene = scene,
                DeltaTime = Core.Time.DeltaTime
            };

            try
            {
                comp.Engine.Load(comp.Code, context);
                comp.Engine.Start();
                comp.Started = true;
            }
            catch (Exception ex)
            {
                Debug.Console.LogError($"Error al iniciar script '{comp.ScriptName}': {ex.Message}", objeto.Name);
            }
        }

        public static void UpdateAll(Core.Scene scene)
        {
            foreach (var objeto in scene.Objetos)
            {
                if (!objeto.Enabled) continue;

                foreach (var comp in objeto.Scripts)
                {
                    if (comp.Engine == null)
                    {
                        Init(objeto, comp, scene);
                        continue;
                    }

                    try
                    {
                        comp.Engine.Update(Core.Time.DeltaTime);
                    }
                    catch (Exception ex)
                    {
                        Debug.Console.LogError($"Error en Update de '{comp.ScriptName}': {ex.Message}", objeto.Name);
                    }
                }
            }
        }

        public static void DestroyComponent(ScriptComponent comp)
        {
            if (comp.Engine == null) return;

            try { comp.Engine.OnDestroy(); }
            catch (Exception ex) { Debug.Console.LogError($"Error en OnDestroy de '{comp.ScriptName}': {ex.Message}"); }

            comp.Started = false;
            comp.Engine = null;
        }
    }
}
