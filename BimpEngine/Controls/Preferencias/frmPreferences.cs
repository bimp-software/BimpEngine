using BimpEngine.Engine.Core;

namespace BimpEngine.Controls.Preferencias
{
    public class frmPreferences : Form
    {
        private Panel _panelLateral;
        private Panel _panelContenido;
        private Button _btnGuardar;
        private Button _btnCancelar;

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
            // ── Panel Lateral (Categorías) ────────────────────────────────
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

            // Botones de categorías
            AgregarBotonCategoria("⚙️ General", CargarCategoriaGeneral);
            AgregarBotonCategoria("⌨️ Atajos de Teclado", CargarCategoriaAtajos);

            // ── Panel Inferior (Botones de Acción) ─────────────────────────
            var panelInferior = new Panel
            {
                Height = 50,
                Dock = DockStyle.Bottom,
                BackColor = Color.FromArgb(28, 28, 32)
            };

            _btnGuardar = new Button
            {
                Text = "Guardar",
                Size = new Size(90, 30),
                Location = new Point(540, 10),
                BackColor = Color.FromArgb(45, 110, 45),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            _btnGuardar.FlatAppearance.BorderSize = 0;
            _btnGuardar.Click += (s, e) => { EngineSettings.Current.Save(); DialogResult = DialogResult.OK; Close(); };

            _btnCancelar = new Button
            {
                Text = "Cancelar",
                Size = new Size(90, 30),
                Location = new Point(640, 10),
                BackColor = Color.FromArgb(50, 50, 55),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            _btnCancelar.FlatAppearance.BorderSize = 0;
            _btnCancelar.Click += (s, e) => { EngineSettings.Load(); Close(); }; // Recarga el JSON original para descartar cambios

            panelInferior.Controls.AddRange(new Control[] { _btnGuardar, _btnCancelar });

            // ── Panel de Contenido Dinámico ───────────────────────────────
            _panelContenido = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(22, 22, 25),
                Padding = new Padding(20)
            };

            Controls.Add(_panelContenido);
            Controls.Add(_panelLateral);
            Controls.Add(panelInferior);
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
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 40, 45);
            btn.Click += (s, e) => metodoClick();
            _panelLateral.Controls.Add(btn);
            btn.BringToFront(); // Mantiene el orden de inserción de arriba a abajo
        }

        // ── CATEGORÍA: GENERAL ─────────────────────────────────────────────
        private void CargarCategoriaGeneral()
        {
            _panelContenido.Controls.Clear();

            var lblSeccion = new Label
            {
                Text = "Configuración General del Editor",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            // Dropdown Idioma
            var lblIdioma = new Label { Text = "Idioma del Motor:", Location = new Point(24, 70), AutoSize = true };
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
            cmbIdioma.SelectedIndexChanged += (s, e) => EngineSettings.Current.Idioma = cmbIdioma.SelectedItem.ToString();

            // Checkbox Cargar Último Proyecto
            var chkCargar = new CheckBox
            {
                Text = "Cargar automáticamente el último proyecto al iniciar",
                Location = new Point(24, 110),
                AutoSize = true,
                Checked = EngineSettings.Current.CargarUltimoProyectoAlIniciar,
                FlatStyle = FlatStyle.Flat
            };
            chkCargar.CheckedChanged += (s, e) => EngineSettings.Current.CargarUltimoProyectoAlIniciar = chkCargar.Checked;

            _panelContenido.Controls.AddRange(new Control[] { lblSeccion, lblIdioma, cmbIdioma, chkCargar });
        }

        // ── CATEGORÍA: ATAJOS DE TECLADO ──────────────────────────────────
        private void CargarCategoriaAtajos()
        {
            _panelContenido.Controls.Clear();

            var lblSeccion = new Label
            {
                Text = "Atajos de Teclado del Editor",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            // Contenedor scrolleable para la lista de teclas
            var listaControles = new FlowLayoutPanel
            {
                Location = new Point(20, 60),
                Size = new Size(500, 320),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            // Generamos dinámicamente una fila por cada atajo guardado en EngineSettings
            foreach (var binding in EngineSettings.Current.InputBindings)
            {
                // Solo listamos los que pertenecen al Editor (para no mezclar con cosas de gameplay por ahora)
                if (binding.Categoria != "Editor") continue;

                var fila = new Panel { Size = new Size(460, 40), Padding = new Padding(5) };

                var lblAccion = new Label
                {
                    Text = binding.Descripcion,
                    Location = new Point(5, 10),
                    Width = 200,
                    ForeColor = Color.Gainsboro
                };

                var btnTecla = new Button
                {
                    Text = binding.Tecla,
                    Location = new Point(220, 5),
                    Size = new Size(120, 25),
                    BackColor = Color.FromArgb(45, 45, 52),
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };

                // Evento para cambiar la tecla capturando el teclado directamente
                btnTecla.Click += (s, e) =>
                {
                    btnTecla.Text = "Presione una tecla...";
                    btnTecla.Focus();
                };

                btnTecla.KeyDown += (s, ke) =>
                {
                    // Guardamos la tecla física presionada
                    binding.Tecla = ke.KeyCode.ToString();
                    btnTecla.Text = binding.Tecla;
                    ke.SuppressKeyPress = true; // Evita el sonido del sistema
                };

                fila.Controls.AddRange(new Control[] { lblAccion, btnTecla });
                listaControles.Controls.Add(fila);
            }

            _panelContenido.Controls.AddRange(new Control[] { lblSeccion, listaControles });
        }
    }
}
