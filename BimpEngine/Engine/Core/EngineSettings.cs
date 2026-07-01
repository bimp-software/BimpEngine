using BimpEngine.Engine.Input;
using BimpEngine.Engine.Project;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BimpEngine.Engine.Core
{
    public class EngineSettings
    {
        public string Idioma { get; set; } = "es-ES";
        public string Tema { get; set; } = "Dark";
        public bool CargarUltimoProyectoAlIniciar { get; set; } = true;
        public List<RecentProject> ProyectosRecientes { get; set; } = new();

        public string LayoutDefecto { get; set; } = "Default";

        public List<KeyBinding> InputBindings { get; set; } = new List<KeyBinding>();

        public bool MostrarGrilla { get; set; } = true;
        public float TamañoCeldaGrilla { get; set; } = 1.0f;
        public bool SnapEnabled { get; set; } = false;
        public float SnapTraslacion { get; set; } = 0.25f;
        public float SnapRotacion { get; set; } = 15.0f;
        public float SnapEscala { get; set; } = 0.1f;

        public float SensibilidadCamara { get; set; } = 0.5f;
        public float VelocidadCamara { get; set; } = 10.0f;
        public float CampoDivision { get; set; } = 60.0f;

        public EditorKeybinds Atajos { get; set; } = new();

        public bool MostrarMensajes { get; set; } = true;
        public bool MostrarAdvertencias { get; set; } = true;
        public bool MostrarErrores { get; set; } = true;
        public bool LimpiarConsolaAlPlay { get; set; } = true;

        public bool AutoguardadoHabilitado { get; set; } = true;
        public int IntervaloAutoguardadoMinutos { get; set; } = 5;

        private static readonly string FolderPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "BimpSoftware",
            "BimpEngine"
        );

        private static readonly string FilePath = Path.Combine(FolderPath, "editor_settings.json");

        public static EngineSettings Current { get; private set; } = new EngineSettings();

        public static void Load()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    string json = File.ReadAllText(FilePath);
                    Current = JsonSerializer.Deserialize<EngineSettings>(json) ?? new EngineSettings();
                }
                else
                {
                    // Si no existe, creamos el archivo con los valores por defecto
                    Current = new EngineSettings();
                    Current.EstablecerControlesPorDefecto();
                    Current.Save();
                }
            }
            catch
            {
                // Si algo falla (archivo corrupto, etc.), respaldamos con los valores base
                Current = new EngineSettings();
            }
        }

        // Guardar la configuración actual en AppData
        public void Save()
        {
            try
            {
                // Asegurar que la ruta en AppData/Roaming/BimpSoftware/BimpEngine exista
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }

                var opciones = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(this, opciones);
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo guardar la configuración del motor: {ex.Message}");
            }
        }

        public void EstablecerControlesPorDefecto()
        {
            InputBindings = new List<KeyBinding>
            {
                new KeyBinding { Accion = "Mover_Adante", Descripcion = "Mover hacia adelante", Categoria = "Juego", Tecla = Keys.W.ToString() },
                new KeyBinding { Accion = "Mover_Atras", Descripcion = "Mover hacia atrás", Categoria = "Juego", Tecla = Keys.S.ToString() },
                new KeyBinding { Accion = "Mover_Izquierda", Descripcion = "Mover a la izquierda", Categoria = "Juego", Tecla = Keys.A.ToString() },
                new KeyBinding { Accion = "Mover_Derecha", Descripcion = "Mover a la derecha", Categoria = "Juego", Tecla = Keys.D.ToString() },
                new KeyBinding { Accion = "Juego_Saltar", Descripcion = "Saltar", Categoria = "Juego", Tecla = Keys.Space.ToString() },
                new KeyBinding { Accion = "Juego_Disparar", Descripcion = "Acción / Disparar", Categoria = "Juego", Tecla = Keys.F.ToString() },

                // ATAJOS DEL EDITOR
                new KeyBinding { Accion = "Editor_Guardar", Descripcion = "Guardar cambios", Categoria = "Editor", Tecla = Keys.S.ToString() }, // Se evalúa junto con Ctrl
                new KeyBinding { Accion = "Editor_Compilar", Descripcion = "Compilar Scripts", Categoria = "Editor", Tecla = Keys.F5.ToString() }
            };
        }

        public bool ValidarTecla(string nombreAccion, Keys teclaPresionada)
        {
            var binding = InputBindings.Find(b => b.Accion == nombreAccion);
            if (binding != null && Enum.TryParse<Keys>(binding.Tecla, out var key))
            {
                return key == teclaPresionada;
            }
            return false;
        }
    }
}
