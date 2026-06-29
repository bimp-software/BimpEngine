using BimpEngine.Engine.Project;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BimpEngine.Vista
{

    public partial class frmLauncher : Form
    {
        public string? SelectedProjectFolder { get; private set; }

        public frmLauncher()
        {
            InitializeComponent();
            LoadRecent();
        }

        private void LoadRecent()
        {
            lvRecent.Items.Clear();
            foreach (var r in ProjectManager.GetRecentProjects())
            {
                bool exists = Directory.Exists(r.Path);
                var item = new ListViewItem(r.Name);
                item.SubItems.Add(r.Path);
                item.SubItems.Add(r.LastOpened.ToLocalTime().ToString("dd/MM/yyyy HH:mm"));
                item.Tag = r.Path;
                item.ForeColor = exists ? Color.White : Color.FromArgb(120, 120, 125);
                if (!exists) item.ToolTipText = "Carpeta no encontrada";
                lvRecent.Items.Add(item);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            using var dlg = new frmNuevoProyecto();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                SelectedProjectFolder = dlg.ProjectFolder;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            using var dlg = new FolderBrowserDialog
            {
                Description = "Selecciona la carpeta del proyecto BimpEngine",
                UseDescriptionForTitle = true
            };
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            string folder = dlg.SelectedPath;
            if (ProjectManager.FindBimpFile(folder) == null)
            {
                MessageBox.Show("La carpeta seleccionada no contiene un archivo .bimp.\n" +
                                "Asegúrate de seleccionar la raíz del proyecto.",
                                "Proyecto no válido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SelectedProjectFolder = folder;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            if (lvRecent.SelectedItems.Count == 0) return;
            string folder = (string)lvRecent.SelectedItems[0].Tag!;
            if (!Directory.Exists(folder))
            {
                MessageBox.Show("La carpeta ya no existe en la ruta indicada.",
                                "Carpeta no encontrada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SelectedProjectFolder = folder;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void lvRecent_DoubleClick(object sender, EventArgs e) => btnAbrir_Click(sender, e);

    }
}