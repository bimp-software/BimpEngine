using BimpEngine.Engine.Project;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BimpEngine.Vista
{
    public partial class frmNuevoProyecto : Form
    {
        public string ProjectFolder { get; private set; } = "";

        public frmNuevoProyecto()
        {
            txtUbicacion.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BimpProjects");
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            string name = txtNombre.Text.Trim();
            string loc = txtUbicacion.Text.Trim();
            lblPreview.Text = (name.Length > 0 && loc.Length > 0) ? Path.Combine(loc, name) : "";
            pnlError.Visible = false;
            lblError.Text = "";
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            string name = txtNombre.Text.Trim();
            string loc = txtUbicacion.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                pnlError.Visible = true;
                lblError.Text = "El nombre del proyecto no puede estar vacío."; return;
            }

            if (string.IsNullOrEmpty(loc))
            {
                pnlError.Visible = true;
                lblError.Text = "Selecciona una ubicación para el proyecto."; return;
            }

            foreach (char c in Path.GetInvalidFileNameChars())
                if (name.Contains(c))
                {
                    pnlError.Visible = true;
                    lblError.Text = $"El nombre contiene un carácter no válido: '{c}'"; return;
                }

            try
            {
                ProjectFolder = ProjectManager.CreateProject(loc, name);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                pnlError.Visible = true;
                lblError.Text = ex.Message;
            }
        }

        private void btnExaminar_Click(object sender, EventArgs e)
        {
            using var dlg = new FolderBrowserDialog
            {
                Description = "Selecciona la carpeta donde crear el proyecto",
                UseDescriptionForTitle = true,
                SelectedPath = txtUbicacion.Text
            };
            if (dlg.ShowDialog(this) == DialogResult.OK) txtUbicacion.Text = dlg.SelectedPath;
        }
    }
}