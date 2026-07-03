using BimpEngine.Engine.Core;
using BimpEngine.Engine.Project;
using BimpEngine.Vista;
using System.Windows.Forms;

namespace BimpEngine
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (s, e) => MostrarError(e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                MostrarError(e.ExceptionObject as Exception ?? new Exception("Error desconocido"));

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
                MessageBox.Show(
                    $"No se pudo abrir el proyecto:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private static void MostrarError(Exception ex)
        {
            MessageBox.Show(
                $"Ocurrió un error inesperado:\n\n{ex.Message}\n\n{ex.StackTrace}",
                "Error no controlado", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}