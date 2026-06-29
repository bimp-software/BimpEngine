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
        public List<string> ProyectosRecientes { get; set; } = new List<string>();
        public string LayoutDefecto { get; set; } = "DefaultLayoutData";

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
                System.Windows.Forms.MessageBox.Show($"No se pudo guardar la configuración del motor: {ex.Message}");
            }
        }
    }
}
