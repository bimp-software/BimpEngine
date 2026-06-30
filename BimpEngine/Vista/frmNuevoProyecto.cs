using BimpEngine.Engine.Project;
using BimpEngine.Engine.Project.Enum;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BimpEngine.Vista
{
    /// <summary>
    /// Formulario para crear un nuevo proyecto.
    /// Ahora incluye selector de modo (2D / 3D).
    /// </summary>
    public partial class frmNuevoProyecto : Form
    {
        public string ProjectFolder { get; private set; } = "";

        // ── Controles del selector de modo ──────────────────────────────
        private Panel pnlModo;
        private Button btn2D;
        private Button btn3D;
        private Label lblModoDesc;
        private ProjectMode _modoSeleccionado = ProjectMode.Mode3D;

        public frmNuevoProyecto()
        {
            InitializeComponent();
            InicializarSelectorModo();
            txtUbicacion.Text = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "BimpProjects");
            UpdatePreview();
        }

        // ── Selector 2D / 3D ────────────────────────────────────────────

        private void InicializarSelectorModo()
        {
            // Panel contenedor
            pnlModo = new Panel
            {
                Height = 80,
                Dock = DockStyle.None,
                Location = new Point(12, /* ajusta según tu diseño */ 120),
                Width = this.ClientSize.Width - 24
            };

            // Botón 3D
            btn3D = new Button
            {
                Text = "🎮  3D",
                Width = 120,
                Height = 60,
                Location = new Point(0, 0),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 204),   // azul = seleccionado por defecto
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn3D.Click += (s, e) => SeleccionarModo(ProjectMode.Mode3D);

            // Botón 2D
            btn2D = new Button
            {
                Text = "✏️  2D",
                Width = 120,
                Height = 60,
                Location = new Point(130, 0),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(50, 50, 55),    // oscuro = no seleccionado
                ForeColor = Color.FromArgb(180, 180, 180),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn2D.Click += (s, e) => SeleccionarModo(ProjectMode.Mode2D);

            // Descripción del modo
            lblModoDesc = new Label
            {
                Text = "Perspectiva 3D con cámara orbital. Ideal para juegos de acción, plataformas 3D y simulaciones.",
                AutoSize = false,
                Width = pnlModo.Width - 270,
                Height = 60,
                Location = new Point(265, 0),
                ForeColor = Color.FromArgb(180, 180, 190),
                Font = new Font("Segoe UI", 8.5f),
                TextAlign = ContentAlignment.MiddleLeft
            };

            pnlModo.Controls.AddRange(new Control[] { btn3D, btn2D, lblModoDesc });

            // Agregar panel al formulario (antes del panel de error o al final)
            // NOTA: ajusta el índice o usa this.Controls.Add si tu Designer no lo expone
            this.Controls.Add(pnlModo);
            pnlModo.BringToFront();
        }

        private void SeleccionarModo(ProjectMode modo)
        {
            _modoSeleccionado = modo;

            bool es3D = modo == ProjectMode.Mode3D;

            btn3D.BackColor = es3D
                ? Color.FromArgb(0, 122, 204)
                : Color.FromArgb(50, 50, 55);
            btn3D.ForeColor = es3D ? Color.White : Color.FromArgb(180, 180, 180);

            btn2D.BackColor = !es3D
                ? Color.FromArgb(0, 122, 204)
                : Color.FromArgb(50, 50, 55);
            btn2D.ForeColor = !es3D ? Color.White : Color.FromArgb(180, 180, 180);

            lblModoDesc.Text = es3D
                ? "Perspectiva 3D con cámara orbital. Ideal para juegos de acción, plataformas 3D y simulaciones."
                : "Vista ortográfica 2D con zoom y pan. Ideal para plataformas 2D, top-down y pixel art.";
        }

        // ── Lógica existente ────────────────────────────────────────────

        private void UpdatePreview()
        {
            string name = txtNombre.Text.Trim();
            string loc = txtUbicacion.Text.Trim();
            lblPreview.Text = (name.Length > 0 && loc.Length > 0)
                ? Path.Combine(loc, name)
                : "";
            pnlError.Visible = false;
            lblError.Text = "";
        }

        private void btnSalir_Click(object sender, EventArgs e) => Close();

        private void btnCrear_Click(object sender, EventArgs e)
        {
            string name = txtNombre.Text.Trim();
            string loc = txtUbicacion.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                pnlError.Visible = true;
                lblError.Text = "El nombre del proyecto no puede estar vacío.";
                return;
            }

            if (string.IsNullOrEmpty(loc))
            {
                pnlError.Visible = true;
                lblError.Text = "Selecciona una ubicación para el proyecto.";
                return;
            }

            foreach (char c in Path.GetInvalidFileNameChars())
                if (name.Contains(c))
                {
                    pnlError.Visible = true;
                    lblError.Text = $"El nombre contiene un carácter no válido: '{c}'";
                    return;
                }

            try
            {
                // Pasamos el modo al crear el proyecto
                ProjectFolder = ProjectManager.CreateProject(loc, name, _modoSeleccionado);
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
            if (dlg.ShowDialog(this) == DialogResult.OK)
                txtUbicacion.Text = dlg.SelectedPath;
        }
    }
}