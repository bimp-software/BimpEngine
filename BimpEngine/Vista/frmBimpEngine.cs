using BimpEngine.Controls.Consola;
using BimpEngine.Controls.Escena;
using BimpEngine.Controls.Herencia;
using BimpEngine.Controls.Inspector;
using BimpEngine.Controls.Proyecto;
using BimpEngine.Engine.Editor;
using BimpEngine.Engine.Editor.Layouts;
using BimpEngine.Engine.Entities;
using BimpEngine.Engine.Entities.Primitive;
using BimpEngine.Engine.Project;
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
                {
                    nuevoPadre.AddChild(hijo);
                }
                else
                {
                    sceneView.GetScene().Add(hijo);
                }

                sceneView.RefrescarEscena();
            };

            hierarchy.OnObjectDeleted += (obj) =>
            {
                obj.Parent?.RemoveChild(obj);
                sceneView.GetScene().Remove(obj);
                inspector.RemoveObjectComponents(obj);
                inspector.ShowObject(null);
                sceneView.RefrescarEscena();
            };

            hierarchy.OnObjectDuplicated += (obj) =>
            {
                var clon = obj.Clone();
                clon.Name = sceneView.GetScene().GenerarNombre(obj.Name);
                sceneView.GetScene().Add(clon);
                hierarchy.AddObject(clon, clon.Parent);
                sceneView.SetSelection(clon, sceneView.GetScene().Objetos.Count - 1);
                inspector.ShowObject(clon);
            };

            editorViews = new Dictionary<string, EditorView>()
            {
                { "Hierarchy", new EditorView("Hierarchy", hierarchy) },
                { "Scene",     new EditorView("Scene",     sceneView) },
                { "Inspector", new EditorView("Inspector", inspector) },
                { "Console",   new EditorView("Console",   console)  },
                { "Project",   new EditorView("Project",   proyecto) }
            };

            layoutManager.SetLayout(new DefaultLayout(), editorViews);
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

            // Load file tree in the Project panel
            proyecto.CargarProyecto(ProjectManager.CurrentProjectFolder!);
            proyecto.OnOpenScene += (scenePath) =>
            {
                if (ConfirmarDescartarCambios())
                    CargarEscenaDesdeArchivo(scenePath);
            };

            string scenePath = ProjectManager.GetMainScenePath();
            if (File.Exists(scenePath))
                CargarEscenaDesdeArchivo(scenePath);
            ActualizarTitulo();
        }

        private void GuardarEscena()
        {
            if (!ProjectManager.HasOpenProject)
            { GuardarEscenaComo(); return; }

            try
            {
                string path = ProjectManager.GetMainScenePath();
                SceneSerializer.Save(sceneView.GetScene(), path);
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
                SceneSerializer.Save(sceneView.GetScene(), dlg.FileName);
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
                sceneView.RefrescarEscena();
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
            hierarchy.Clear();
            inspector.ShowObject(null);
            sceneView.RefrescarEscena();
            _hasUnsavedChanges = false;
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
            string escena = sceneView?.GetScene()?.Name ?? "Scene";
            string cambios = _hasUnsavedChanges ? " *" : "";
            Text = $"BimpEngine — {proyecto} — {escena}{cambios}";
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

        // Mark scene as modified whenever the user changes something
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
    }
}