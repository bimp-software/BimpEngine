using BimpEngine.Engine.Core.Interface;
using BimpEngine.Engine.Scripting;
using BimpEngine.Engine.Scripting.Nodes;
using BimpEngine.Engine.World;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BimpEngine.Controls.Inspector.Componentes.Blueprint
{
    /// <summary>
    /// Inspector component that opens the visual scripting editor for an object.
    /// Shows a compact summary in the inspector; clicking "Editar Script" opens
    /// a full-screen floating editor window.
    /// </summary>
    public class BlueprintControl : UserControl, IInspectorComponent
    {
        private Objetos? _objeto;
        private NodeGraph _graph = new();
        private Label _lblInfo = new();
        private Button _btnEdit = new();
        private Button _btnRun = new();
        private frmBlueprintEditor? _editorWin;

        public BlueprintControl()
        {
            BackColor = Color.FromArgb(38, 38, 42);
            Height = 80;
            Dock = DockStyle.Top;
            Padding = new Padding(8);

            _lblInfo.Text = "Sin nodos  |  0 conexiones";
            _lblInfo.ForeColor = Color.FromArgb(160, 160, 165);
            _lblInfo.Font = new Font("Segoe UI", 8.5f);
            _lblInfo.Location = new Point(8, 8);
            _lblInfo.AutoSize = true;

            _btnEdit.Text = "✏  Editar Script";
            _btnEdit.Location = new Point(8, 32);
            _btnEdit.Size = new Size(130, 30);
            _btnEdit.BackColor = Color.RoyalBlue;
            _btnEdit.ForeColor = Color.White;
            _btnEdit.FlatStyle = FlatStyle.Flat;
            _btnEdit.FlatAppearance.BorderSize = 0;
            _btnEdit.Font = new Font("Segoe UI", 9f);
            _btnEdit.Click += BtnEdit_Click;

            _btnRun.Text = "▶  Probar";
            _btnRun.Location = new Point(146, 32);
            _btnRun.Size = new Size(90, 30);
            _btnRun.BackColor = Color.FromArgb(50, 130, 70);
            _btnRun.ForeColor = Color.White;
            _btnRun.FlatStyle = FlatStyle.Flat;
            _btnRun.FlatAppearance.BorderSize = 0;
            _btnRun.Font = new Font("Segoe UI", 9f);
            _btnRun.Click += BtnRun_Click;

            Controls.AddRange(new Control[] { _lblInfo, _btnEdit, _btnRun });
        }

        public void SetObject(Objetos obj)
        {
            _objeto = obj;
            UpdateInfo();
        }

        public void Refresh(Objetos obj)
        {
            _objeto = obj;
            UpdateInfo();
        }

        private void UpdateInfo()
        {
            int nodes = _graph.Nodes.Count;
            int conns = _graph.Connections.Count;
            _lblInfo.Text = $"{nodes} nodos  |  {conns} conexiones";
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (_editorWin == null || _editorWin.IsDisposed)
            {
                string nombreObjeto = _objeto?.Name ?? "Objeto";

                string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", $"{nombreObjeto}.json");

                string? directorio = System.IO.Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directorio))
                {
                    System.IO.Directory.CreateDirectory(directorio);
                }

                List<Objetos>? objetosEscena = null;

                _editorWin = new frmBlueprintEditor(_graph, nombreObjeto, filePath, objetosEscena);
                _editorWin.OnGraphChanged += () => { UpdateInfo(); };
                _editorWin.Show(FindForm());
            }
            else
            {
                _editorWin.BringToFront();
            }
        }

        private void BtnRun_Click(object? sender, EventArgs e)
        {
            if (_objeto == null) return;
            var ctx = new ScriptContext { Owner = _objeto };
            var eval = new GraphEvaluator(_graph);
            eval.FireEvent("Al Iniciar", ctx);
            UpdateInfo();
        }
    }
}