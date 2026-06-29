using BimpEngine.Engine.Entities;
using BimpEngine.Engine.World;
using System;
using System.Diagnostics;
using System.Text;

namespace BimpEngine.Controls.Inspector
{
    public partial class frmAddComponent : Form
    {
        public event Action<string> OnComponentSelected;

        private TextBox tbBuscar;
        private ListBox lstComponentes;
        private Objetos _objeto;

        // Registro de componentes: nombre → compatibilidad por tipo
        private readonly List<ComponenteInfo> _todos = new List<ComponenteInfo>
        {
            new ComponenteInfo("Mueblería",          "Diseño de muebles con cajones, puertas y repisas",  typeof(PrimitiveObject)),
            new ComponenteInfo("Script (Blueprint)",  "Editor visual de nodos — lógica sin código",        null),
            new ComponenteInfo("Cámara",             "Controla la vista de la cámara en la escena",       typeof(CameraObject)),
            new ComponenteInfo("Luz",                "Agrega una fuente de luz al objeto",                null),
            new ComponenteInfo("Colisionador",       "Define el área de colisión del objeto",             null),
            new ComponenteInfo("RigidBody",          "Aplica física y gravedad al objeto",                null),
            new ComponenteInfo("Audio",              "Reproduce sonidos desde este objeto",               null),
        };

        public frmAddComponent(Objetos objeto)
        {
            _objeto = objeto;

            // Estilo del form
            this.Text = "Agregar Componente";
            this.Size = new Size(300, 380);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.StartPosition = FormStartPosition.Manual;
            this.BackColor = Color.FromArgb(45, 45, 45);
            this.ForeColor = Color.White;
            this.ShowInTaskbar = false;
            this.TopMost = true;

            BuildUI();
            CargarComponentes("");
        }

        private void BuildUI()
        {
            // Buscador
            var lblBuscar = new Label
            {
                Text = "Buscar componente...",
                ForeColor = Color.FromArgb(150, 150, 150),
                Font = new Font("Segoe UI", 8.5f),
                Dock = DockStyle.Top,
                Height = 20,
                Padding = new Padding(6, 4, 0, 0)
            };

            tbBuscar = new TextBox
            {
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10f),
                Height = 30
            };
            tbBuscar.TextChanged += (s, e) => CargarComponentes(tbBuscar.Text);

            // Lista de componentes
            lstComponentes = new ListBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 9.5f),
                ItemHeight = 40
            };
            lstComponentes.DrawMode = DrawMode.OwnerDrawFixed;
            lstComponentes.DrawItem += LstComponentes_DrawItem;
            lstComponentes.MouseDoubleClick += (s, e) => SeleccionarComponente();
            lstComponentes.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) SeleccionarComponente();
                if (e.KeyCode == Keys.Escape) this.Close();
            };

            // Botón seleccionar
            var btnSeleccionar = new Button
            {
                Text = "Agregar",
                Dock = DockStyle.Bottom,
                Height = 35,
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f),
                Cursor = Cursors.Hand
            };
            btnSeleccionar.FlatAppearance.BorderSize = 0;
            btnSeleccionar.Click += (s, e) => SeleccionarComponente();

            Controls.Add(lstComponentes);
            Controls.Add(tbBuscar);
            Controls.Add(lblBuscar);
            Controls.Add(btnSeleccionar);

            // Focus al buscador al abrir
            this.Shown += (s, e) => tbBuscar.Focus();

            // Cerrar al perder foco
            this.Deactivate += (s, e) => this.Close();
        }

        private void LstComponentes_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            var item = (ComponenteInfo)lstComponentes.Items[e.Index];
            bool seleccionado = (e.State & DrawItemState.Selected) != 0;

            e.Graphics.FillRectangle(
                new SolidBrush(seleccionado ? Color.FromArgb(0, 100, 180) : Color.FromArgb(50, 50, 50)),
                e.Bounds);

            // Icono (círculo de color)
            var iconRect = new Rectangle(e.Bounds.X + 8, e.Bounds.Y + 10, 20, 20);
            e.Graphics.FillEllipse(new SolidBrush(item.Color), iconRect);

            // Nombre
            e.Graphics.DrawString(
                item.Nombre,
                new Font("Segoe UI", 9f, FontStyle.Bold),
                Brushes.White,
                new PointF(e.Bounds.X + 36, e.Bounds.Y + 6));

            // Descripción
            e.Graphics.DrawString(
                item.Descripcion,
                new Font("Segoe UI", 7.5f),
                new SolidBrush(Color.FromArgb(160, 160, 160)),
                new PointF(e.Bounds.X + 36, e.Bounds.Y + 22));
        }

        private void CargarComponentes(string filtro)
        {
            lstComponentes.Items.Clear();

            var compatibles = _todos.Where(c =>
                // Filtro de texto
                (string.IsNullOrEmpty(filtro) || c.Nombre.ToLower().Contains(filtro.ToLower())) &&
                // Filtro de compatibilidad
                (c.TipoCompatible == null || (_objeto != null && c.TipoCompatible.IsInstanceOfType(_objeto)))
            );

            foreach (var c in compatibles)
                lstComponentes.Items.Add(c);

            if (lstComponentes.Items.Count > 0)
                lstComponentes.SelectedIndex = 0;
        }

        private void SeleccionarComponente()
        {
            if (lstComponentes.SelectedItem is ComponenteInfo info)
            {
                OnComponentSelected?.Invoke(info.Nombre);
                this.Close();
            }
        }
    }

    public class ComponenteInfo
    {
        public string Nombre { get; }
        public string Descripcion { get; }
        public Type TipoCompatible { get; }
        public Color Color { get; }

        private static readonly Color[] Colores = {
            Color.FromArgb(80, 160, 255),
            Color.FromArgb(80, 200, 120),
            Color.FromArgb(255, 180, 60),
            Color.FromArgb(200, 80, 80),
            Color.FromArgb(160, 100, 220),
        };

        private static int _colorIndex = 0;

        public ComponenteInfo(string nombre, string descripcion, Type tipoCompatible)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            TipoCompatible = tipoCompatible;
            Color = Colores[_colorIndex++ % Colores.Length];
        }
    }
}