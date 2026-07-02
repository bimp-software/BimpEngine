using BimpEngine.Engine.Core;
using BimpEngine.Engine.Project;
using BimpEngine.Vista;

namespace BimpEngine
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            EngineSettings.Load();

            ApplicationConfiguration.Initialize();

            string? projectFolder = null;
            using (var launcher = new frmLauncher())
            {
                if (launcher.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return; 

                projectFolder = launcher.SelectedProjectFolder;
            }

            if (projectFolder == null) return;

            try
            {
                ProjectManager.OpenProject(projectFolder);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    $"No se pudo abrir el proyecto:\n{ex.Message}",
                    "Error", System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }

            var editor = new frmBimpEngine();
            editor.CargarProyecto();

            editor.FormClosing += (s, e) =>
            {
                Application.Exit();
                Application.ExitThread();
            };
            editor.FormClosed += (s, e) =>
            {
                Application.Exit();
                Application.ExitThread();
            };

            editor.ShowDialog();
            Application.Run();
        }
    }
}