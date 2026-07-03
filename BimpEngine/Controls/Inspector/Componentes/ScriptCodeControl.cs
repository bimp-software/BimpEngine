using BimpEngine.Engine.Core.Interface;
using BimpEngine.Engine.Scripting;
using BimpEngine.Engine.Scripting.Enum;
using BimpEngine.Engine.World;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BimpEngine.Controls.Inspector.Componentes
{
    public partial class ScriptCodeControl : UserControl, IInspectorComponent
    {
        private Objetos? _objeto;
        private ScriptComponent? _comp;

        private Label _lblArchivo;
        private Label _lblLenguaje;
        private Button _btnSeleccionar;
        private Button _btnEditar;
        private Button _btnQuitar;

        public event Action<Objetos>? OnObjectModified;
        public event Action<ScriptCodeControl>? OnRemoveRequested;
        public event Action<string>? OnEditRequested;


        public ScriptCodeControl()
        {
            Height = 90;
            BackColor = Color.FromArgb(45, 45, 45);
            AllowDrop = true;

            var titulo = new Label
            {
                Text = "Script",
                Dock = DockStyle.Top,
                Height = 22,
                ForeColor = Color.FromArgb(200, 200, 200),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Padding = new Padding(6, 4, 0, 0)
            };

            var panelInfo = new Panel { Dock = DockStyle.Top, Height = 36, Padding = new Padding(6, 0, 6, 0) };
            _lblArchivo = new Label { Text = "Sin script asignado", Dock = DockStyle.Top, ForeColor = Color.Gray, Font = new Font("Segoe UI", 8.5f) };
            _lblLenguaje = new Label { Text = "", Dock = DockStyle.Top, ForeColor = Color.FromArgb(120, 180, 255), Font = new Font("Segoe UI", 8f) };
            panelInfo.Controls.Add(_lblLenguaje);
            panelInfo.Controls.Add(_lblArchivo);

            var panelBotones = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 30, FlowDirection = FlowDirection.LeftToRight };

            _btnSeleccionar = BotonPequeño("Seleccionar...");
            _btnSeleccionar.Click += (s, e) => SeleccionarArchivo();

            _btnEditar = BotonPequeño("Editar");
            _btnEditar.Click += (s, e) => { if (_comp != null) OnEditRequested?.Invoke(_comp.ScriptPath); };

            _btnQuitar = BotonPequeño("Quitar");
            _btnQuitar.Click += (s, e) => Quitar();

            panelBotones.Controls.Add(_btnSeleccionar);
            panelBotones.Controls.Add(_btnEditar);
            panelBotones.Controls.Add(_btnQuitar);

            Controls.Add(panelBotones);
            Controls.Add(panelInfo);
            Controls.Add(titulo);

            DragEnter += (s, e) =>
            {
                if (e.Data!.GetDataPresent(DataFormats.Text) && EsArchivoValido((string)e.Data.GetData(DataFormats.Text)!))
                    e.Effect = DragDropEffects.Copy;
            };
            DragDrop += (s, e) =>
            {
                string path = (string)e.Data!.GetData(DataFormats.Text)!;
                if (EsArchivoValido(path)) AsignarArchivo(path);
            };
        }

        private static bool EsArchivoValido(string path) =>
            File.Exists(path) && Path.GetExtension(path).ToLower() is ".cs" or ".lua" or ".py";

        private static Button BotonPequeño(string texto) => new Button
        {
            Text = texto,
            Height = 26,
            AutoSize = true,
            BackColor = Color.FromArgb(60, 60, 60),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };

        private void SeleccionarArchivo()
        {
            using var dialog = new OpenFileDialog
            {
                Title = "Seleccionar Script",
                Filter = "Scripts (*.cs;*.lua;*.py)|*.cs;*.lua;*.py|Todos los archivos|*.*",
                InitialDirectory = Engine.Project.ProjectManager.CurrentProjectFolder
            };
            if (dialog.ShowDialog() == DialogResult.OK)
                AsignarArchivo(dialog.FileName);
        }

        private void AsignarArchivo(string path)
        {
            if (_objeto == null) return;

            string ext = Path.GetExtension(path).ToLower();
            Language lenguaje = ext switch
            {
                ".cs" => Language.CSharp,
                ".lua" => Language.Lua,
                ".py" => Language.Python,
                _ => Language.CSharp
            };

            if (_comp == null)
            {
                _comp = new ScriptComponent();
                _objeto.Scripts.Add(_comp);
            }
            else
            {
                // Cambió de script → forzar reinicio del motor de ejecución
                Engine.Scripting.ScriptRuntime.DestroyComponent(_comp);
            }

            _comp.ScriptPath = path;
            _comp.ScriptName = Path.GetFileNameWithoutExtension(path);
            _comp.Language = lenguaje;
            _comp.Code = File.ReadAllText(path);

            ActualizarUI();
            OnObjectModified?.Invoke(_objeto);
        }

        private void Quitar()
        {
            if (_objeto != null && _comp != null)
            {
                Engine.Scripting.ScriptRuntime.DestroyComponent(_comp);
                _objeto.Scripts.Remove(_comp);
            }
            OnRemoveRequested?.Invoke(this);
        }

        private void ActualizarUI()
        {
            if (_comp == null)
            {
                _lblArchivo.Text = "Sin script asignado";
                _lblLenguaje.Text = "";
                return;
            }

            _lblArchivo.Text = Path.GetFileName(_comp.ScriptPath);
            _lblLenguaje.Text = _comp.Language switch
            {
                Language.CSharp => "C#",
                Language.Lua => "Lua",
                Language.Python => "Python",
                _ => ""
            };
        }

        public void SetObject(Objetos obj)
        {
            _objeto = obj;
            ActualizarUI();
        }

        public void Refresh(Objetos obj)
        {
            if (_comp != null && File.Exists(_comp.ScriptPath))
                _comp.Code = File.ReadAllText(_comp.ScriptPath); // recarga en caliente
        }

        // Usado por InspectorControl al restaurar componentes ya guardados
        public void VincularExistente(ScriptComponent comp)
        {
            _comp = comp;
            ActualizarUI();
        }
    }
}