using BimpEngine.Controls.Consola;
using BimpEngine.Controls.Escena;
using BimpEngine.Controls.Herencia;
using BimpEngine.Controls.Inspector;
using BimpEngine.Controls.Proyecto;
using BimpEngine.Engine.Editor;
using BimpEngine.Engine.Editor.Layouts;
using BimpEngine.Engine.Entities;
using BimpEngine.Engine.Entities.Primitive;
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

        public frmBimpEngine()
        {
            InitializeComponent();
            InitializeUser();

            InicializarControler();
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
            };

            inspector.OnChildCreated += (hijo, padre) =>
            {
                hierarchy.AddObject(hijo, padre);
                sceneView.RefrescarEscena();
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
    }
}
