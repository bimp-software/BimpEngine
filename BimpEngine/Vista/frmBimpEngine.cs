using BimpEngine.Controls.Consola;
using BimpEngine.Controls.Escena;
using BimpEngine.Controls.Herencia;
using BimpEngine.Controls.Inspector;
using BimpEngine.Controls.Inspector.Componentes.Blueprint;
using BimpEngine.Controls.Proyecto;
using BimpEngine.Engine.Core;
using BimpEngine.Engine.Editor;
using BimpEngine.Engine.Editor.Layouts;
using BimpEngine.Engine.Entities;
using BimpEngine.Engine.Entities.Primitive;
using BimpEngine.Engine.Project;
using BimpEngine.Engine.Scripting;
using BimpEngine.Engine.World;
using Windows.System;

namespace BimpEngine.Vista
{
    public partial class frmBimpEngine : Form
    {
        #region Variables
        private TableLayoutPanel editorLayout;

        private Dictionary<string, EditorView> editorViews;

        private LayoutManager layoutManager;

        private HerenciaControl hierarchy;
        private EscenaControl sceneView;
        private InspectorControl inspector;
        private ConsolaControl console;
        private ProyectoControl proyecto;
        #endregion

        private bool _hasUnsavedChanges = false;
        private string? _currentScenePath = null;

        public frmBimpEngine()
        {
            InitializeComponent();
            InitializeUser();
            InicializarControler();
            //InicializarMenuArchivo();
            ActualizarTitulo();
        }

        #region Cuenta del usuario
        public void InitializeUser()
        {
            lblNombreUsuario.Text = Environment.MachineName;
            CargarFotoUsuarioOficial();
        }

        private async void CargarFotoUsuarioOficial()
        {
            try
            {
                // 1. Obtener la lista de usuarios activos en la sesión actual de Windows
                var usuarios = await Windows.System.User.FindAllAsync();
                var usuarioActual = usuarios.FirstOrDefault();

                if (usuarioActual != null)
                {
                    // 2. Solicitamos la imagen de la cuenta (tamaño grande para mejor calidad)
                    var resultadoImagen = await usuarioActual.GetPictureAsync(UserPictureSize.Size64x64);

                    if (resultadoImagen != null)
                    {
                        // 3. Abrimos el flujo de la API de Windows y lo pasamos al PictureBox
                        using (var streamWindows = await resultadoImagen.OpenReadAsync())
                        using (Stream streamNet = streamWindows.AsStreamForRead())
                        {
                            pbFotoUsuario.SizeMode = PictureBoxSizeMode.Zoom;
                            pbFotoUsuario.Image = Image.FromStream(streamNet);
                        }
                        return;
                    }
                }

                MessageBox.Show("No se encontró una foto de perfil configurada en tu cuenta de Windows.");
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("El sistema bloqueó el acceso. Asegúrate de que tu aplicación tiene permisos para ver la información de la cuenta en la configuración de privacidad de Windows.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al leer la cuenta: {ex.Message}");
            }
        }
        #endregion

        private void InicializarControler()
        {
            layoutManager = new LayoutManager(pnlContenedor);

            hierarchy = new HerenciaControl();
            sceneView = new EscenaControl();
            inspector = new InspectorControl();
            console = new ConsolaControl();
            proyecto = new ProyectoControl();

            sceneView.OnObjectSelected += (obj) =>
            {
                inspector.ShowObject(obj);
                hierarchy.SelectObject(obj);
            };

            sceneView.OnObjectChanged += (obj) =>
            {
                inspector.RefreshObject(obj);
                hierarchy.RefreshObject(obj);
            };

            hierarchy.OnObjectSelected += (obj, index) =>
            {
                sceneView.SetSelection(obj, index);
                inspector.ShowObject(obj);
            };

            inspector.OnCreatePrimitive += (tipo) =>
            {
                var obj = PrimitiveFactory.Create(tipo);
                sceneView.GetScene().Add(obj);
                hierarchy.AddObject(obj);
                sceneView.SetSelection(obj, sceneView.GetScene().Objetos.Count - 1);
                inspector.ShowObject(obj);
            };

            inspector.OnObjectModified += (obj) =>
            {
                hierarchy.RefreshObject(obj);
                sceneView.RefrescarEscena();
                MarcarCambio();
            };

            inspector.OnChildCreated += (hijo, padre) =>
            {
                hierarchy.AddObject(hijo, padre);
                sceneView.RefrescarEscena();
                MarcarCambio();
            };

            hierarchy.OnObjectReparented += (hijo, nuevoPadre) =>
            {
                hijo.Parent?.RemoveChild(hijo);
                sceneView.GetScene().Objetos.Remove(hijo);

                if (nuevoPadre != null)
                    nuevoPadre.AddChild(hijo);
                else
                    sceneView.GetScene().Add(hijo);

                sceneView.RefrescarEscena();
                MarcarCambio();
            };

            hierarchy.OnObjectDeleted += (obj) =>
            {
                obj.Parent?.RemoveChild(obj);
                sceneView.GetScene().Remove(obj);
                inspector.RemoveObjectComponents(obj);
                inspector.ShowObject(null);
                sceneView.RefrescarEscena();
                MarcarCambio();
            };

            hierarchy.OnObjectDuplicated += (obj) =>
            {
                var clon = obj.Clone();
                clon.Name = sceneView.GetScene().GenerarNombre(obj.Name);
                sceneView.GetScene().Add(clon);
                hierarchy.AddObject(clon, clon.Parent);
                sceneView.SetSelection(clon, sceneView.GetScene().Objetos.Count - 1);
                inspector.ShowObject(clon);
                MarcarCambio(); // ← agregar
            };

            proyecto.OnOpenBlueprint += (path) =>
            {
                NodeGraph graph;

                if (System.IO.File.Exists(path))
                {
                    try
                    {
                        string json = System.IO.File.ReadAllText(path);
                        graph = System.Text.Json.JsonSerializer.Deserialize<NodeGraph>(json) ?? new NodeGraph();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al cargar el Blueprint: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        graph = new NodeGraph();
                    }
                }
                else
                {
                    graph = new NodeGraph();
                }

                string nombreBlueprint = System.IO.Path.GetFileNameWithoutExtension(path);
                var editor = new frmBlueprintEditor(graph, nombreBlueprint, path, sceneView.GetScene().Objetos);
                editor.FormClosing += (s, e) =>
                {
                    try
                    {
                        var opciones = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
                        string jsonGuardar = System.Text.Json.JsonSerializer.Serialize(graph, opciones);

                        System.IO.File.WriteAllText(path, jsonGuardar);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"No se pudo guardar el archivo: {ex.Message}", "Error de Guardado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                editor.Show(this);
            };

            hierarchy.OnCrearMolde += (obj) => CrearMoldeDesdeObjeto(obj);
            hierarchy.OnAplicarCambiosAlMolde += (obj) =>
            {
                try
                {
                    Engine.Project.MoldeSerializer.AplicarCambios(obj);
                    MessageBox.Show($"Cambios aplicados al Molde de '{obj.Name}'.",
                        "Molde actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            proyecto.OnOpenScene += (scenePath) =>
            {
                if (ConfirmarDescartarCambios())
                    CargarEscenaDesdeArchivo(scenePath);
            };
            proyecto.OnInstanciarMolde += (path) => InstanciarMoldeEnEscena(path);

            editorViews = new Dictionary<string, EditorView>()
            {
                { "Hierarchy", new EditorView("Hierarchy", hierarchy) },
                { "Scene",     new EditorView("Scene",     sceneView) },
                { "Inspector", new EditorView("Inspector", inspector) },
                { "Console",   new EditorView("Console",   console)  },
                { "Project",   new EditorView("Project",   proyecto) }
            };

            var layout = LayoutManager.GetLayoutByName(EngineSettings.Current.LayoutDefecto);
            layoutManager.SetLayout(layout, editorViews);
        }

        private void tsmCubo_Click(object sender, EventArgs e)
        {
            var cubo = new Cube();
            sceneView.GetScene().Add(cubo);
            hierarchy.AddObject(cubo);
            sceneView.SetSelection(cubo, sceneView.GetScene().Objetos.Count - 1);
            inspector.ShowObject(cubo);
        }

        private void tsmTriangulo_Click(object sender, EventArgs e)
        {
            var triangulo = new Triangle();
            sceneView.GetScene().Add(triangulo);
            hierarchy.AddObject(triangulo);
            sceneView.SetSelection(triangulo, sceneView.GetScene().Objetos.Count - 1);
            inspector.ShowObject(triangulo);
        }

        private void tsmCilindro_Click(object sender, EventArgs e)
        {
            var cilindro = new Cylinder();
            sceneView.GetScene().Add(cilindro);
            hierarchy.AddObject(cilindro);
            sceneView.SetSelection(cilindro, sceneView.GetScene().Objetos.Count - 1);
            inspector.ShowObject(cilindro);
        }

        #region Project System

        //private void InicializarMenuArchivo()
        //{
        //    archivoToolStripMenuItem.DropDownItems.Clear();

        //    var tsmGuardar = new ToolStripMenuItem("Guardar Escena\tCtrl+S");
        //    tsmGuardar.ShortcutKeys = Keys.Control | Keys.S;
        //    tsmGuardar.Click += (s, e) => GuardarEscena();

        //    var tsmGuardarComo = new ToolStripMenuItem("Guardar Escena como…");
        //    tsmGuardarComo.Click += (s, e) => GuardarEscenaComo();

        //    var tsmSep1 = new ToolStripSeparator();

        //    var tsmAbrirEscena = new ToolStripMenuItem("Abrir Escena…");
        //    tsmAbrirEscena.Click += (s, e) => AbrirEscena();

        //    var tsmSep2 = new ToolStripSeparator();

        //    var tsmNuevaEscena = new ToolStripMenuItem("Nueva Escena");
        //    tsmNuevaEscena.Click += (s, e) => NuevaEscena();

        //    var tsmSep3 = new ToolStripSeparator();

        //    var tsmIrLauncher = new ToolStripMenuItem("Volver al Gestor de Proyectos…");
        //    tsmIrLauncher.Click += (s, e) => VolverAlLauncher();

        //    var tsmSalir = new ToolStripMenuItem("Salir");
        //    tsmSalir.Click += (s, e) => Close();

        //    archivoToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
        //    {
        //        tsmGuardar, tsmGuardarComo, tsmSep1,
        //        tsmAbrirEscena, tsmSep2,
        //        tsmNuevaEscena, tsmSep3,
        //        tsmIrLauncher, tsmSalir
        //    });
        //}

        // ── Called by Program.cs after opening a project ──────────────────

        public void CargarProyecto()
        {
            if (!ProjectManager.HasOpenProject) return;

            proyecto.CargarProyecto(ProjectManager.CurrentProjectFolder!);

            string scenePath = ProjectManager.GetMainScenePath();
            if (File.Exists(scenePath))
                CargarEscenaDesdeArchivo(scenePath);

            ActualizarTitulo();
        }

        private void AplicarModoProyecto()
        {
            var modo = Engine.Project.ProjectManager.CurrentMode;
            sceneView.SetMode(modo);

            // Cambiar el layout por defecto según el modo
            // (opcional: podés asignar un layout 2D diferente si lo tenés)
            // layoutManager.SetLayout(modo == ProjectMode.Mode2D
            //     ? new Layout2D()
            //     : new DefaultLayout(), editorViews);

            // Actualizar badge en la barra de título
            ActualizarTitulo();
        }

        private void GuardarEscena()
        {
            if (!ProjectManager.HasOpenProject)
            { GuardarEscenaComo(); return; }

            if (_currentScenePath == null)
            {
                GuardarEscenaComo();
                return;
            }

            try
            {
                SceneSerializer.Save(sceneView.GetScene(), _currentScenePath);
                ProjectManager.SaveProjectInfo();
                _hasUnsavedChanges = false;
                ActualizarTitulo();
                proyecto.RefrescarArbol();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GuardarEscenaComo()
        {
            if (!ProjectManager.HasOpenProject)
            {
                MessageBox.Show("No hay un proyecto abierto. Crea o abre un proyecto primero.",
                    "Sin proyecto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var dlg = new SaveFileDialog
            {
                Title = "Guardar Escena como",
                InitialDirectory = ProjectManager.GetScenesFolder(),
                Filter = "Escena BimpEngine (*.bscene)|*.bscene",
                DefaultExt = "bscene",
                FileName = sceneView.GetScene().Name
            };
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                sceneView.GetScene().Name = Path.GetFileNameWithoutExtension(dlg.FileName);

                SceneSerializer.Save(sceneView.GetScene(), dlg.FileName);
                _currentScenePath = dlg.FileName;
                _hasUnsavedChanges = false;
                ActualizarTitulo();
                proyecto.RefrescarArbol();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AbrirEscena()
        {
            if (!ConfirmarDescartarCambios()) return;

            string startDir = ProjectManager.HasOpenProject
                ? ProjectManager.GetScenesFolder()
                : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            using var dlg = new OpenFileDialog
            {
                Title = "Abrir Escena",
                InitialDirectory = startDir,
                Filter = "Escena BimpEngine (*.bscene)|*.bscene"
            };
            if (dlg.ShowDialog(this) != DialogResult.OK) return;
            CargarEscenaDesdeArchivo(dlg.FileName);
        }

        private void CargarEscenaDesdeArchivo(string path)
        {
            try
            {
                var scene = SceneSerializer.Load(path);
                sceneView.SetScene(scene);
                hierarchy.Clear();
                foreach (var obj in scene.Objetos)
                    hierarchy.AddObject(obj, null, includeChildren: true);

                inspector.ShowObject(null);

                // Re-aplicar el modo del proyecto (2D/3D) a la nueva escena cargada
                sceneView.SetMode(ProjectManager.CurrentMode);

                sceneView.RefrescarEscena();

                _currentScenePath = path;
                _hasUnsavedChanges = false;
                ActualizarTitulo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la escena:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NuevaEscena()
        {
            if (!ConfirmarDescartarCambios()) return;

            var scene = new Engine.Core.Scene { Name = "Scene" };
            sceneView.SetScene(scene);
            sceneView.SetMode(ProjectManager.CurrentMode); // respeta 2D/3D del proyecto
            hierarchy.Clear();
            inspector.ShowObject(null);
            sceneView.RefrescarEscena();

            _currentScenePath = null;   // sin archivo todavía → al Guardar pedirá ubicación
            _hasUnsavedChanges = true;  // una escena nueva ya cuenta como "sin guardar"
            ActualizarTitulo();
        }

        private void VolverAlLauncher()
        {
            if (!ConfirmarDescartarCambios()) return;
            ProjectManager.Close();
            var launcher = new frmLauncher();
            Hide();
            if (launcher.ShowDialog() == DialogResult.OK && launcher.SelectedProjectFolder != null)
            {
                ProjectManager.OpenProject(launcher.SelectedProjectFolder);
                CargarProyecto();
                Show();
            }
            else
            {
                Close();
            }
        }

        private void ActualizarTitulo()
        {
            string proyecto = ProjectManager.CurrentProject?.Name ?? "Sin Proyecto";
            string escena = _currentScenePath != null
                ? Path.GetFileNameWithoutExtension(_currentScenePath)
                : sceneView?.GetScene()?.Name ?? "Untitled";
            string cambios = _hasUnsavedChanges ? " *" : "";
            string modo = ProjectManager.CurrentMode == Engine.Project.Enum.ProjectMode.Mode2D
                ? " [2D]" : " [3D]";

            Text = $"BimpEngine — {proyecto}{modo} — {escena}{cambios}";
        }

        private bool ConfirmarDescartarCambios()
        {
            if (!_hasUnsavedChanges) return true;
            var r = MessageBox.Show("Hay cambios sin guardar. ¿Deseas guardar antes de continuar?",
                "Cambios sin guardar",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (r == DialogResult.Cancel) return false;
            if (r == DialogResult.Yes) GuardarEscena();
            return true;
        }

        public void MarcarCambio()
        {
            if (!_hasUnsavedChanges)
            {
                _hasUnsavedChanges = true;
                ActualizarTitulo();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!ConfirmarDescartarCambios())
                e.Cancel = true;
            base.OnFormClosing(e);
        }

        #endregion

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            GuardarEscena();
        }

        private void btnGuardarComo_Click(object sender, EventArgs e)
        {
            GuardarEscenaComo();
        }

        #region 2D
        private void tsmiCuadrado_Click(object sender, EventArgs e)
        {
            var obj = PrimitiveFactory2D.Create(PrimitiveType2D.Square);
            sceneView.GetScene().Add(obj);
            hierarchy.AddObject(obj);
        }

        #endregion

        private void CrearMoldeDesdeObjeto(Objetos objeto)
        {
            if (!ProjectManager.HasOpenProject)
            {
                MessageBox.Show("Abre un proyecto antes de crear un Molde.",
                    "Sin proyecto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string moldesFolder = Path.Combine(ProjectManager.CurrentProjectFolder!, "Moldes");
            Directory.CreateDirectory(moldesFolder);

            using var dlg = new SaveFileDialog
            {
                Title = "Crear Molde",
                InitialDirectory = moldesFolder,
                Filter = "Molde BimpEngine (*.bmold)|*.bmold",
                DefaultExt = "bmold",
                FileName = objeto.Name
            };
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                Engine.Project.MoldeSerializer.Crear(objeto, dlg.FileName);
                hierarchy.RefreshObject(objeto); // repinta el nodo en azul
                proyecto.RefrescarArbol();
                MarcarCambio();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear el Molde:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InstanciarMoldeEnEscena(string moldeFilePath)
        {
            try
            {
                var instancia = Engine.Project.MoldeSerializer.Instanciar(moldeFilePath);
                sceneView.GetScene().Add(instancia);
                hierarchy.AddObject(instancia, null, includeChildren: true);
                sceneView.SetSelection(instancia, sceneView.GetScene().Objetos.Count - 1);
                sceneView.RefrescarEscena();
                MarcarCambio();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al instanciar el Molde:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsmiConfiguracionProyecto_Click(object sender, EventArgs e)
        {
            using var frm = new frmTagsAndLayers();
            frm.ShowDialog(this);

            inspector?.RefrescarTagsYLayers();
        }
    }
}