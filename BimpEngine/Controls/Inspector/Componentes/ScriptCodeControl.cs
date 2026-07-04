using BimpEngine.Engine.Core.Interface;
using BimpEngine.Engine.Scripting;
using BimpEngine.Engine.Scripting.Engines;
using BimpEngine.Engine.Scripting.Enum;
using BimpEngine.Engine.Scripting.Interface;
using BimpEngine.Engine.World;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BimpEngine.Controls.Inspector.Componentes
{
    public partial class ScriptCodeControl : UserControl, IInspectorComponent
    {
        private Objetos? _objeto;
        private ScriptComponent? _comp;
        private DateTime _ultimaEscrituraConocida;

        private Label _lblArchivo;
        private Label _lblLenguaje;
        private Button _btnSeleccionar;
        private Button _btnEditar;
        private Button _btnQuitar;
        private FlowLayoutPanel _panelVariables;

        private Label _titulo;
        private Panel _panelInfo;
        private FlowLayoutPanel _panelBotones;

        public event Action<Objetos>? OnObjectModified;
        public event Action<ScriptCodeControl>? OnRemoveRequested;
        public event Action<string>? OnEditRequested;

        public ScriptComponent? ComponenteVinculado => _comp;

        public ScriptCodeControl()
        {
            AutoSize = false;
            BackColor = Color.FromArgb(45, 45, 45);
            AllowDrop = true;

            _titulo = new Label
            {
                Text = "Script",
                Dock = DockStyle.Top,
                Height = 22,
                ForeColor = Color.FromArgb(200, 200, 200),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Padding = new Padding(6, 4, 0, 0)
            };

            _panelInfo = new Panel { Dock = DockStyle.Top, Height = 36, Padding = new Padding(6, 0, 6, 0) };
            _lblArchivo = new Label { Text = "Sin script asignado", Dock = DockStyle.Top, ForeColor = Color.Gray, Font = new Font("Segoe UI", 8.5f) };
            _lblLenguaje = new Label { Text = "", Dock = DockStyle.Top, ForeColor = Color.FromArgb(120, 180, 255), Font = new Font("Segoe UI", 8f) };
            _panelInfo.Controls.Add(_lblLenguaje);
            _panelInfo.Controls.Add(_lblArchivo);

            _panelBotones = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 30, FlowDirection = FlowDirection.LeftToRight };

            _btnSeleccionar = BotonPequeño("Seleccionar...");
            _btnSeleccionar.Click += (s, e) => SeleccionarArchivo();

            _btnEditar = BotonPequeño("Editar");
            _btnEditar.Click += (s, e) => { if (_comp != null) OnEditRequested?.Invoke(_comp.ScriptPath); };

            _btnQuitar = BotonPequeño("Quitar");
            _btnQuitar.Click += (s, e) => Quitar();

            _panelBotones.Controls.Add(_btnSeleccionar);
            _panelBotones.Controls.Add(_btnEditar);
            _panelBotones.Controls.Add(_btnQuitar);

            _panelVariables = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(6, 2, 6, 6)
            };

            Controls.Add(_panelVariables);
            Controls.Add(_panelBotones);
            Controls.Add(_panelInfo);
            Controls.Add(_titulo);

            ActualizarAltura();

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
                Engine.Scripting.ScriptRuntime.DestroyComponent(_comp);
            }

            _comp.ScriptPath = path;
            _comp.ScriptName = Path.GetFileNameWithoutExtension(path);
            _comp.Language = lenguaje;
            _comp.Code = File.ReadAllText(path);

            DetectarVariables();
            ActualizarUI();
            OnObjectModified?.Invoke(_objeto);
        }

        private void DetectarVariables()
        {
            if (_comp == null || _objeto == null) return;

            IScriptEngine motorTemporal = _comp.Language switch
            {
                Language.CSharp => new CSharpScriptEngine(),
                Language.Lua => new LuaScriptEngine(),
                Language.Python => new PythonScriptEngine(),
                _ => new CSharpScriptEngine()
            };

            try
            {
                var contexto = new ScriptContext { Owner = _objeto, Scene = null, DeltaTime = 0 };
                motorTemporal.Load(_comp.Code, contexto);

                var detectadas = motorTemporal.GetExposedVariables().ToList();

                foreach (var det in detectadas)
                {
                    var existente = _comp.Variables.FirstOrDefault(v => v.Name == det.Name);
                    if (existente == null)
                        _comp.Variables.Add(det);
                    else
                        existente.Type = det.Type;
                }

                _comp.Variables.RemoveAll(v => !detectadas.Any(d => d.Name == v.Name));
            }
            catch
            {
                // si el script tiene errores de sintaxis, se reportará al presionar Play
            }
            finally
            {
                motorTemporal.OnDestroy();
            }
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
                _panelVariables.Controls.Clear();
                ActualizarAltura();
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

            ActualizarVariablesUI();
        }

        private void ActualizarVariablesUI()
        {
            _panelVariables.Controls.Clear();
            if (_comp != null)
            {
                foreach (var variable in _comp.Variables)
                    _panelVariables.Controls.Add(CrearFilaVariable(variable));
            }

            ActualizarAltura();
        }

        private void ActualizarAltura()
        {
            if (_titulo == null || _panelInfo == null || _panelBotones == null || _panelVariables == null)
                return;

            Height = _titulo.Height + _panelInfo.Height + _panelBotones.Height + _panelVariables.PreferredSize.Height + 4;
        }

        private Control CrearFilaVariable(ScriptVariable variable)
        {
            var fila = new Panel { Height = 26, Width = 260 };

            var lbl = new Label
            {
                Text = variable.Name,
                Location = new Point(0, 4),
                Width = 100,
                ForeColor = Color.FromArgb(210, 210, 210),
                Font = new Font("Segoe UI", 8.5f)
            };
            fila.Controls.Add(lbl);

            Control editor = variable.Type switch
            {
                VariableType.Bool => CrearEditorBool(variable),
                VariableType.Vector3 => CrearEditorVector3(variable),
                _ => CrearEditorTexto(variable)
            };

            editor.Location = new Point(104, 1);
            fila.Controls.Add(editor);

            return fila;
        }

        private Control CrearEditorTexto(ScriptVariable variable)
        {
            var tb = new TextBox { Text = variable.ValueRaw, Width = 150 };
            tb.Leave += (s, e) => GuardarValor(variable, tb.Text);
            return tb;
        }

        private Control CrearEditorBool(ScriptVariable variable)
        {
            var cb = new CheckBox { Checked = variable.ValueRaw == "True" || variable.ValueRaw == "true" };
            cb.CheckedChanged += (s, e) => GuardarValor(variable, cb.Checked.ToString());
            return cb;
        }

        private Control CrearEditorVector3(ScriptVariable variable)
        {
            var partes = variable.ValueRaw.Split(';');
            string x = partes.Length > 0 ? partes[0] : "0";
            string y = partes.Length > 1 ? partes[1] : "0";
            string z = partes.Length > 2 ? partes[2] : "0";

            var panel = new Panel { Width = 150, Height = 22 };
            var tbX = new TextBox { Text = x, Width = 46, Location = new Point(0, 0) };
            var tbY = new TextBox { Text = y, Width = 46, Location = new Point(52, 0) };
            var tbZ = new TextBox { Text = z, Width = 46, Location = new Point(104, 0) };

            void Actualizar(object? s, EventArgs e) => GuardarValor(variable, $"{tbX.Text};{tbY.Text};{tbZ.Text}");
            tbX.Leave += Actualizar;
            tbY.Leave += Actualizar;
            tbZ.Leave += Actualizar;

            panel.Controls.Add(tbX);
            panel.Controls.Add(tbY);
            panel.Controls.Add(tbZ);
            return panel;
        }

        private void GuardarValor(ScriptVariable variable, string nuevoValorRaw)
        {
            variable.ValueRaw = nuevoValorRaw;

            // Si el juego está corriendo (Play), aplica el cambio en caliente
            if (_comp?.Engine != null)
            {
                var valor = ScriptVariableUtils.ParseValor(nuevoValorRaw, variable.Type);
                if (valor != null)
                    _comp.Engine.SetVariable(variable.Name, valor);
            }

            if (_objeto != null)
                OnObjectModified?.Invoke(_objeto);
        }

        public void SetObject(Objetos obj)
        {
            _objeto = obj;
            ActualizarUI();
        }

        public void Refresh(Objetos obj)
        {
            if (_comp == null || string.IsNullOrEmpty(_comp.ScriptPath) || !File.Exists(_comp.ScriptPath))
                return;

            var ultimaEscritura = File.GetLastWriteTimeUtc(_comp.ScriptPath);
            if (ultimaEscritura == _ultimaEscrituraConocida)
                return;

            _ultimaEscrituraConocida = ultimaEscritura;
            _comp.Code = File.ReadAllText(_comp.ScriptPath);
            DetectarVariables();
            ActualizarUI();
        }

        public void VincularExistente(ScriptComponent comp)
        {
            _comp = comp;
            ActualizarUI();
        }
    }
}