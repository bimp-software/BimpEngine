using BimpEngine.Engine.Project;
using BimpEngine.Vista;

namespace BimpEngine
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            BimpEngine.Engine.Core.EngineSettings.Load();

            string? projectFolder = null;
            using (var launcher = new frmLauncher())
            {
                if (launcher.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return; // user closed launcher without selecting a project

                projectFolder = launcher.SelectedProjectFolder;
            }

            if (projectFolder == null) return;

            // 2. Open the selected project
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

            // 3. Open the editor and load the project's main scene
            var editor = new frmBimpEngine();
            editor.CargarProyecto();

            editor.FormClosing += (s, e) =>
            {
                // FormClosing already handled inside frmBimpEngine (unsaved changes prompt)
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