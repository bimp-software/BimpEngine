using BimpEngine.Controls.Consola;
using BimpEngine.Controls.Escena;
using BimpEngine.Controls.Herencia;
using BimpEngine.Controls.Inspector;
using BimpEngine.Controls.Proyecto;
using BimpEngine.Engine.Debug;
using BimpEngine.Engine.Editor;
using BimpEngine.Engine.Editor.Layouts;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
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

            editorViews = new Dictionary<string, EditorView>()
            {
                {
                    "Hierarchy",
                    new EditorView(
                        "Hierarchy",
                        hierarchy)
                },

                {
                    "Scene",
                    new EditorView(
                        "Scene",
                        sceneView)
                },

                {
                    "Inspector",
                    new EditorView(
                        "Inspector",
                        inspector)
                },

                {
                    "Console",
                    new EditorView(
                        "Console",
                        console)
                },

                {
                    "Project",
                    new EditorView(
                        "Project",
                        proyecto)
                }
            };

            layoutManager.SetLayout(new DefaultLayout(),editorViews);
        }
    }
}
