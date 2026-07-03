using BimpEngine.Controls.Escena;
using BimpEngine.Engine.Core;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BimpEngine.Controls.Preferencias
{
    public class frmPreferences : Form
    {
        private Panel _panelLateral;
        private Panel _panelContenido;
        private Button _btnGuardar;
        private Button _btnCancelar;
        private FlowLayoutPanel _flpAtajos;

        public frmPreferences()
        {
            Text = "Preferencias del Editor — BimpEngine";
            Size = new Size(750, 500);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.FromArgb(22, 22, 25);
            ForeColor = Color.White;

            InicializarComponentes();
            CargarCategoriaGeneral();
        }

        private void InicializarComponentes()
        {
            _panelLateral = new Panel
            {
                Width = 180,
                Dock = DockStyle.Left,
                BackColor = Color.FromArgb(28, 28, 32),
                Padding = new Padding(5)
            };

            var lblTitulo = new Label
            {
                Text = "PREFERENCIAS",
                Dock = DockStyle.Top,
                Height = 35,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(140, 140, 150),
                TextAlign = ContentAlignment.MiddleCenter
            };

            _panelLateral.Controls.Add(lblTitulo);

            AgregarBotonCategoria("⚙️ General", CargarCategoriaGeneral);
            AgregarBotonCategoria("⌨️ Atajos de Teclado", CargarCategoriaAtajos);

            Panel panelDerecho = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(22, 22, 25)
            };

            _panelContenido = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(22, 22, 25),
                Padding = new Padding(20)
            };

            var panelInferior = new Panel
            {
                Height = 55,
                Dock = DockStyle.Bottom,
                BackColor = Color.FromArgb(28, 28, 32),
                Padding = new Padding(10)
            };

            var panelBotones = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                Width = 210,
                FlowDirection = FlowDirection.LeftToRight
            };

            _btnGuardar = new Button
            {
                Text = "Guardar",
                Size = new Size(90, 32),
                BackColor = Color.FromArgb(45, 110, 45),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            _btnGuardar.FlatAppearance.BorderSize = 0;
            _btnGuardar.Click += (s, e) =>
            {
                EngineSettings.Current.Save();
                DialogResult = DialogResult.OK;
                Close();
            };

            _btnCancelar = new Button
            {
                Text = "Cancelar",
                Size = new Size(90, 32),
                BackColor = Color.FromArgb(50, 50, 55),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            _btnCancelar.FlatAppearance.BorderSize = 0;
            _btnCancelar.Click += (s, e) =>
            {
                EngineSettings.Load();
                Close();
            };

            panelBotones.Controls.Add(_btnGuardar);
            panelBotones.Controls.Add(_btnCancelar);

            panelInferior.Controls.Add(panelBotones);

            panelDerecho.Controls.Add(_panelContenido);
            panelDerecho.Controls.Add(panelInferior);

            Controls.Add(panelDerecho);
            Controls.Add(_panelLateral);
        }

        private void AgregarBotonCategoria(string texto, Action metodoClick)
        {
            var btn = new Button
            {
                Text = "  " + texto,
                Dock = DockStyle.Top,
                Height = 32,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(200, 200, 205),
                BackColor = Color.FromArgb(28, 28, 32),
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 40, 45);
            btn.Click += (s, e) => metodoClick();

            _panelLateral.Controls.Add(btn);
            btn.BringToFront();
        }

        private void CargarCategoriaGeneral()
        {
            _panelContenido.Controls.Clear();

            var lblSeccion = new Label
            {
                Text = "Configuración General del Editor",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 20)
            };

            var lblIdioma = new Label
            {
                Text = "Idioma del Motor:",
                ForeColor = Color.Gainsboro,
                Location = new Point(24, 70),
                AutoSize = true
            };

            var cmbIdioma = new ComboBox
            {
                Location = new Point(160, 66),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(40, 40, 45),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            cmbIdioma.Items.AddRange(new string[] { "es-ES", "en-US" });
            cmbIdioma.SelectedItem = EngineSettings.Current.Idioma;
            cmbIdioma.SelectedIndexChanged += (s, e) =>
            {
                if (cmbIdioma.SelectedItem != null)
                    EngineSettings.Current.Idioma = cmbIdioma.SelectedItem.ToString();
            };

            var chkCargar = new CheckBox
            {
                Text = "Cargar automáticamente el último proyecto al iniciar",
                ForeColor = Color.Gainsboro,
                Location = new Point(24, 110),
                AutoSize = true,
                Checked = EngineSettings.Current.CargarUltimoProyectoAlIniciar,
                FlatStyle = FlatStyle.Flat
            };

            chkCargar.CheckedChanged += (s, e) =>
                EngineSettings.Current.CargarUltimoProyectoAlIniciar = chkCargar.Checked;

            _panelContenido.Controls.Add(lblSeccion);
            _panelContenido.Controls.Add(lblIdioma);
            _panelContenido.Controls.Add(cmbIdioma);
            _panelContenido.Controls.Add(chkCargar);
        }

        private void CargarCategoriaAtajos()
        {
            _panelContenido.Controls.Clear();

            var lblSeccion = new Label
            {
                Text = "Atajos de Teclado del Editor",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 45
            };

            _flpAtajos = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.FromArgb(22, 22, 25),
                Padding = new Padding(0, 10, 0, 0)
            };

            _panelContenido.Controls.Add(_flpAtajos);
            _panelContenido.Controls.Add(lblSeccion);

            bool encontroAtajos = false;

            foreach (var binding in EngineSettings.Current.InputBindings)
            {
                if (binding.Categoria != "Editor")
                    continue;

                encontroAtajos = true;
                AgregarFilaAtajo(binding.Descripcion, binding.Tecla, nuevaTecla =>
                {
                    binding.Tecla = nuevaTecla;
                });
            }

            if (!encontroAtajos)
            {
                AgregarFilaAtajo("Mover objeto", "W", tecla => { });
                AgregarFilaAtajo("Rotar objeto", "E", tecla => { });
                AgregarFilaAtajo("Escalar objeto", "R", tecla => { });
            }
        }

        private void AgregarFilaAtajo(string accion, string tecla, Action<string> alCambiar)
        {
            var row = new Panel
            {
                Width = 500,
                Height = 42,
                BackColor = Color.FromArgb(34, 34, 38),
                Margin = new Padding(0, 0, 0, 8)
            };

            var lbl = new Label
            {
                Text = accion,
                ForeColor = Color.White,
                Location = new Point(12, 12),
                Width = 280,
                AutoSize = false
            };

            var btnTecla = new Button
            {
                Text = tecla,
                Location = new Point(340, 7),
                Size = new Size(120, 28),
                BackColor = Color.FromArgb(45, 45, 52),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TabStop = true
            };

            btnTecla.FlatAppearance.BorderSize = 0;

            btnTecla.Click += (s, e) =>
            {
                btnTecla.Text = "Presiona tecla...";
                btnTecla.Focus();
            };

            btnTecla.KeyDown += (s, e) =>
            {
                string nuevaTecla = e.KeyCode.ToString();
                btnTecla.Text = nuevaTecla;
                alCambiar?.Invoke(nuevaTecla);
                e.SuppressKeyPress = true;
            };

            row.Controls.Add(lbl);
            row.Controls.Add(btnTecla);
            _flpAtajos.Controls.Add(row);
        }
    }
}