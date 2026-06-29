using BimpEngine.Engine.Scripting;
using BimpEngine.Engine.Scripting.Enum;
using BimpEngine.Engine.Scripting.Nodes;
using BimpEngine.Engine.UI;
using BimpEngine.Engine.World;
using System;
using System.Windows.Forms;

namespace BimpEngine.Controls.Inspector.Componentes.Blueprint
{
    public class frmBlueprintEditor : Form
    {
        private readonly string _filePath;

        public event Action? OnGraphChanged;

        private readonly NodeCanvas _canvas;
        private readonly NodeGraph _graph;
        private FlowLayoutPanel _listaVariables;

        public frmBlueprintEditor(NodeGraph graph, string objectName,string filePath, List<Objetos>? objetosEscena = null)
        {
            _graph = graph;
            _filePath = filePath;

            Text = $"Script — {objectName}";
            Size = new Size(1200, 700);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(22, 22, 25);
            ForeColor = Color.White;
            MinimumSize = new Size(700, 450);

            // ── Toolbar ───────────────────────────────────────────────────
            var toolbar = new ToolStrip
            {
                BackColor = Color.FromArgb(30, 30, 33),
                ForeColor = Color.White,
                GripStyle = ToolStripGripStyle.Hidden,
                RenderMode = ToolStripRenderMode.Professional,
                Dock = DockStyle.Top
            };

            var btnSave = new ToolStripButton("💾  Guardar") { ForeColor = Color.LightGreen };
            btnSave.Click += (s, e) => GuardarGrafo();

            var btnClear = new ToolStripButton("🗑  Limpiar todo") { ForeColor = Color.Salmon };
            btnClear.Click += (s, e) =>
            {
                if (MessageBox.Show("¿Eliminar todos los nodos?", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _graph.Nodes.Clear();
                    _graph.Connections.Clear();
                    _canvas.Invalidate();
                    OnGraphChanged?.Invoke();
                }
            };

            var btnHelp = new ToolStripButton("Ayuda") { ForeColor = Color.White };
            btnHelp.Click += (s, e) => MostrarAyuda();

            toolbar.Items.AddRange(new ToolStripItem[]
            {
                btnSave,
                new ToolStripSeparator(),
                btnClear,
                new ToolStripSeparator(),
                btnHelp,
                new ToolStripSeparator(),
                new ToolStripLabel("Clic derecho para agregar nodos")
                { ForeColor = Color.FromArgb(120, 120, 130) }
            });

            // ── Panel izquierdo de variables ──────────────────────────────
            var panelVariables = new Panel
            {
                Width = 220,
                Dock = DockStyle.Left,
                BackColor = Color.FromArgb(28, 28, 32)
            };

            var lblTitulo = new Label
            {
                Text = "  VARIABLES",
                Dock = DockStyle.Top,
                Height = 28,
                BackColor = Color.FromArgb(35, 35, 40),
                ForeColor = Color.FromArgb(180, 180, 190),
                Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };

            var btnAgregarVar = new Button
            {
                Text = "+ Nueva Variable",
                Dock = DockStyle.Top,
                Height = 28,
                BackColor = Color.FromArgb(40, 40, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5f),
                Cursor = Cursors.Hand
            };
            btnAgregarVar.FlatAppearance.BorderSize = 0;
            btnAgregarVar.Click += (s, e) => AgregarVariable();

            _listaVariables = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.FromArgb(28, 28, 32),
                Padding = new Padding(4)
            };

            panelVariables.Controls.Add(_listaVariables);
            panelVariables.Controls.Add(btnAgregarVar);
            panelVariables.Controls.Add(lblTitulo);

            // ── Canvas ────────────────────────────────────────────────────
            _canvas = new NodeCanvas { Dock = DockStyle.Fill };
            _canvas.SetGraph(_graph);
            _canvas.ObjetosEscena = objetosEscena ?? new List<Objetos>();

            var hint = new Label
            {
                Text = "Clic derecho → Agregar nodo   |   Alt+Arrastrar → Desplazar   |   Rueda → Zoom",
                Dock = DockStyle.Bottom,
                Height = 22,
                BackColor = Color.FromArgb(20, 20, 23),
                ForeColor = Color.FromArgb(100, 100, 110),
                Font = new Font("Segoe UI", 8f),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Controls.Add(_canvas);
            Controls.Add(panelVariables);
            Controls.Add(toolbar);
            Controls.Add(hint);

            RefrescarVariables();
        }

        private void AgregarVariable()
        {
            var v = new GraphVariable
            {
                Nombre = $"Variable{_graph.Variables.Count + 1}",
                Tipo = VariableType.Float,
                ValorDefecto = 0.0
            };
            _graph.Variables.Add(v);
            RefrescarVariables();
        }

        private void RefrescarVariables()
        {
            _listaVariables.Controls.Clear();

            foreach (var v in _graph.Variables)
            {
                var fila = new Panel
                {
                    Width = _listaVariables.ClientSize.Width - 12,
                    Height = 60,
                    BackColor = Color.FromArgb(35, 35, 42),
                    Margin = new Padding(0, 2, 0, 2)
                };

                // Bolita de color del tipo
                var bolita = new Panel
                {
                    Size = new Size(10, 10),
                    Location = new Point(8, 10),
                    BackColor = v.Color
                };

                // Nombre editable
                var tbNombre = new TextBox
                {
                    Text = v.Nombre,
                    Location = new Point(24, 6),
                    Width = fila.Width - 52,
                    BackColor = Color.FromArgb(45, 45, 52),
                    ForeColor = Color.White,
                    BorderStyle = BorderStyle.None,
                    Font = new Font("Segoe UI", 9f, FontStyle.Bold)
                };
                tbNombre.LostFocus += (s, e) => v.Nombre = tbNombre.Text;

                if (v.Tipo == VariableType.Float || v.Tipo == VariableType.String || v.Tipo == VariableType.Bool)
                {
                    fila.Height = 84; // expandir fila

                    var tbValor = new TextBox
                    {
                        Text = v.ValorDefecto?.ToString() ?? "",
                        Location = new Point(24, 56),
                        Width = fila.Width - 52,
                        BackColor = Color.FromArgb(55, 55, 65),
                        ForeColor = Color.FromArgb(180, 220, 180),
                        BorderStyle = BorderStyle.None,
                        Font = new Font("Segoe UI", 8.5f),
                        PlaceholderText = "Valor por defecto..."
                    };
                    tbValor.LostFocus += (s, e) =>
                    {
                        v.ValorDefecto = v.Tipo switch
                        {
                            VariableType.Float => double.TryParse(tbValor.Text, out var d) ? d : 0.0,
                            VariableType.Bool => tbValor.Text.ToLower() == "true",
                            _ => tbValor.Text
                        };
                    };
                    fila.Controls.Add(tbValor);
                }

                // ComboBox con todos los tipos
                var cmbTipo = new ComboBox
                {
                    Location = new Point(24, 30),
                    Width = fila.Width - 52,
                    BackColor = Color.FromArgb(45, 45, 52),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 8f),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };

                foreach (VariableType tipo in Enum.GetValues<VariableType>())
                    cmbTipo.Items.Add(new VariableTipoItem(tipo));

                cmbTipo.SelectedIndex = (int)v.Tipo;
                cmbTipo.SelectedIndexChanged += (s, e) =>
                {
                    if (cmbTipo.SelectedItem is VariableTipoItem item)
                    {
                        v.Tipo = item.Tipo;
                        v.ValorDefecto = item.Tipo switch
                        {
                            VariableType.Float => 0.0,
                            VariableType.Bool => false,
                            VariableType.String => "",
                            _ => null
                        };
                        bolita.BackColor = v.Color;
                    }
                };

                // Botón eliminar
                var btnDel = new Button
                {
                    Text = "✕",
                    Location = new Point(fila.Width - 26, 6),
                    Size = new Size(20, 20),
                    BackColor = Color.FromArgb(100, 40, 40),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnDel.FlatAppearance.BorderSize = 0;
                btnDel.Click += (s, e) => { _graph.Variables.Remove(v); RefrescarVariables(); };

                // Arrastrar al canvas para crear nodo Get/Set
                tbNombre.MouseMove += (s, e) =>
                {
                    if (e.Button == MouseButtons.Left)
                        fila.DoDragDrop(v, DragDropEffects.Copy);
                };

                fila.Controls.AddRange(new Control[] { bolita, tbNombre, cmbTipo, btnDel });
                _listaVariables.Controls.Add(fila);
            }
        }

        private void MostrarAyuda()
        {
            MessageBox.Show(
                "EDITOR DE SCRIPTS\n\n" +
                "▶ Clic derecho en el fondo  → Agregar nodos\n" +
                "▶ Clic derecho en un nodo   → Eliminar / Duplicar\n" +
                "▶ Arrastrar nodo            → Moverlo\n" +
                "▶ Arrastrar desde un puerto → Crear conexión\n" +
                "▶ Rueda del ratón           → Zoom\n" +
                "▶ Alt + Arrastrar           → Desplazar vista\n\n" +
                "VARIABLES\n" +
                "▶ Clic en + Nueva Variable     → Crear variable\n" +
                "▶ Arrastrar variable al canvas → Crear nodo Get/Set\n\n" +
                "TIPOS DE VARIABLE\n" +
                "▶ Número, Booleano, Texto, Vector3\n" +
                "▶ Objeto, Escena, Transform, Cámara, Canvas UI\n" +
                "▶ Lista Números, Lista Objetos\n\n" +
                "COLORES DE PUERTO\n" +
                "▶ Blanco  = Flujo (Exec)\n" +
                "▶ Verde   = Número\n" +
                "▶ Rojo    = Booleano\n" +
                "▶ Naranja = Texto\n" +
                "▶ Azul    = Vector3\n" +
                "▶ Morado  = Objeto/Referencia",
                "Ayuda", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Helper para mostrar nombre bonito en el ComboBox
        private class VariableTipoItem
        {
            public VariableType Tipo { get; }
            public VariableTipoItem(VariableType tipo) => Tipo = tipo;
            public override string ToString() => new GraphVariable { Tipo = Tipo }.NombreTipo;
        }

        private void GuardarGrafo()
        {
            try
            {
                var opciones = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
                string json = System.Text.Json.JsonSerializer.Serialize(_graph, opciones);
                System.IO.File.WriteAllText(_filePath, json);

                // Pequeño feedback visual en la barra de título
                string tituloOriginal = Text;
                Text = "✓ Guardado correctamente";
                var t = new System.Windows.Forms.Timer { Interval = 1500 };
                t.Tick += (s, e) => { Text = tituloOriginal; t.Stop(); t.Dispose(); };
                t.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}