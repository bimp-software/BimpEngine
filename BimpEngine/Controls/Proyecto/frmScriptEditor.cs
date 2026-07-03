using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BimpEngine.Controls.Proyecto
{
    public partial class frmScriptEditor : Form
    {
        private readonly string _path;
        private TextBox _editor;
        public event Action<string>? OnSaved;

        public frmScriptEditor(string path)
        {
            _path = path;

            Text = "Editando: " + Path.GetFileName(path);
            Size = new Size(720, 560);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(30, 30, 30);
            ForeColor = Color.White;

            _editor = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.Both,
                WordWrap = false,
                AcceptsTab = true,
                Font = new Font("Consolas", 11f),
                BackColor = Color.FromArgb(24, 24, 24),
                ForeColor = Color.FromArgb(220, 220, 220),
                BorderStyle = BorderStyle.None,
                Text = File.Exists(path) ? File.ReadAllText(path) : ""
            };

            var btnGuardar = new Button
            {
                Text = "Guardar (Ctrl+S)",
                Dock = DockStyle.Bottom,
                Height = 34,
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += (s, e) => Guardar();

            Controls.Add(_editor);
            Controls.Add(btnGuardar);

            KeyPreview = true;
            KeyDown += (s, e) =>
            {
                if (e.Control && e.KeyCode == Keys.S) { Guardar(); e.Handled = true; }
            };
        }

        private void Guardar()
        {
            File.WriteAllText(_path, _editor.Text);
            OnSaved?.Invoke(_path);
        }
    }
}