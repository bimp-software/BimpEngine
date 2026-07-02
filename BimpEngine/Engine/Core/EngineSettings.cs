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

        public List<InputBinding> InputBindings { get; set; } = new();

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
                    Current.CargarAtajosPorDefecto();
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

        public void CargarAtajosPorDefecto()
        {
            InputBindings = new List<InputBinding>
            {
                // Editor
                new InputBinding("editor_move", "Editor", "Mover objeto", "W"),
                new InputBinding("editor_rotate", "Editor", "Rotar objeto", "E"),
                new InputBinding("editor_scale", "Editor", "Escalar objeto", "R"),
                new InputBinding("editor_delete", "Editor", "Eliminar objeto", "Delete"),
                new InputBinding("editor_duplicate", "Editor", "Duplicar objeto", "D"),
                new InputBinding("editor_play", "Editor", "Probar juego", "F5"),
                new InputBinding("editor_stop", "Editor", "Detener juego", "Escape"),

                // Jugador
                new InputBinding("player_forward", "Jugador", "Avanzar", "W"),
                new InputBinding("player_back", "Jugador", "Retroceder", "S"),
                new InputBinding("player_left", "Jugador", "Mover izquierda", "A"),
                new InputBinding("player_right", "Jugador", "Mover derecha", "D"),
                new InputBinding("player_jump", "Jugador", "Saltar", "Space"),
                new InputBinding("player_run", "Jugador", "Correr", "ShiftKey"),
                new InputBinding("player_interact", "Jugador", "Interactuar", "E"),

                // Flechas para estudiantes
                new InputBinding("player_arrow_up", "Jugador", "Avanzar con flecha", "Up"),
                new InputBinding("player_arrow_down", "Jugador", "Retroceder con flecha", "Down"),
                new InputBinding("player_arrow_left", "Jugador", "Izquierda con flecha", "Left"),
                new InputBinding("player_arrow_right", "Jugador", "Derecha con flecha", "Right")
            };
        }

        public string GetKey(string id)
        {
            return InputBindings.FirstOrDefault(x => x.Id == id)?.Tecla ?? "";
        }

        public void SetKey(string id, string tecla)
        {
            var binding = InputBindings.FirstOrDefault(x => x.Id == id);

            if (binding != null)
                binding.Tecla = tecla;
        }
    }
}
