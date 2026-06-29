using BimpEngine.Engine.Scripting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Controls.Inspector.Componentes.Blueprint
{
    public class frmBlueprintEditor : Form
    {
        public event Action? OnGraphChanged;

        private readonly NodeCanvas _canvas;
        private readonly NodeGraph _graph;
        private readonly Label _lblHint;
        private readonly ToolStrip _toolbar;

        public frmBlueprintEditor(NodeGraph graph, string objectName)
        {
            _graph = graph;

            Text = $"Script — {objectName}";
            Size = new Size(1100, 700);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(22, 22, 25);
            ForeColor = Color.White;
            MinimumSize = new Size(700, 450);

            // ── Toolbar ───────────────────────────────────────────────────
            _toolbar = new ToolStrip
            {
                BackColor = Color.FromArgb(30, 30, 33),
                ForeColor = Color.White,
                GripStyle = ToolStripGripStyle.Hidden,
                RenderMode = ToolStripRenderMode.Professional
            };

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

            var btnHelp = new ToolStripButton("❓  Ayuda") { ForeColor = Color.White };
            btnHelp.Click += (s, e) => MessageBox.Show(
                "CONTROLES DEL EDITOR DE SCRIPTS\n\n" +
                "▶ Clic derecho en el fondo  → Menú para agregar nodos\n" +
                "▶ Clic derecho en un nodo   → Eliminar / Duplicar\n" +
                "▶ Arrastrar nodo            → Moverlo\n" +
                "▶ Arrastrar desde un puerto → Crear conexión\n" +
                "▶ Rueda del ratón           → Zoom\n" +
                "▶ Alt + Arrastrar           → Desplazar vista\n\n" +
                "TIPOS DE PUERTO (color)\n" +
                "▶ Blanco   = Exec (flujo de ejecución)\n" +
                "▶ Verde    = Número flotante\n" +
                "▶ Rojo     = Booleano (Verdad/Falso)\n" +
                "▶ Azul     = Vector3\n" +
                "▶ Naranja  = Texto",
                "Ayuda — Editor de Scripts",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            var lblSep = new ToolStripLabel("  |  ") { ForeColor = Color.FromArgb(60, 60, 65) };
            var lblHint = new ToolStripLabel("Clic derecho para agregar nodos")
            { ForeColor = Color.FromArgb(120, 120, 130) };

            _toolbar.Items.AddRange(new ToolStripItem[] { btnClear, lblSep, btnHelp, new ToolStripSeparator(), lblHint });

            // ── Canvas ────────────────────────────────────────────────────
            _canvas = new NodeCanvas { Dock = DockStyle.Fill };
            _canvas.SetGraph(_graph);

            // Hint label overlaid bottom-right
            _lblHint = new Label
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
            Controls.Add(_toolbar);
            Controls.Add(_lblHint);
        }
    }
}
