using BimpEngine.Engine.Core;
using BimpEngine.Engine.Editor;
using BimpEngine.Engine.Editor.Gizmos;
using BimpEngine.Engine.Rendering;
using BimpEngine.Engine.World;
using SharpGL;

namespace BimpEngine.Controls.Escena
{
    public partial class EscenaControl : UserControl
    {
        #region Core Engine
        private EditorCamera _camera = new EditorCamera();
        private Grid _grid = new Grid();

        private Scene _scene = new Scene();
        private Renderer _renderer = new Renderer();

        private SelectionManager _selection = new SelectionManager();
        private GizmoManager _gizmo = new GizmoManager();

        #endregion

        #region Input State
        private bool _rotatingCamera;
        private bool _panningCamera;
        private bool _draggingGizmo;

        private Point _lastMouse;
        private Point _lastCameraMouse;
        #endregion

        #region Events
        public event Action<Objetos> OnObjectSelected;
        public event Action<Objetos> OnObjectChanged;
        #endregion

        #region Constructor
        public EscenaControl()
        {
            InitializeComponent();

            glControl.MouseWheel += glControl_MouseWheel;
        }
        #endregion

        #region Public API
        public void SetScene(Scene scene)
        {
            _scene = scene;
        }

        public Scene GetScene()
        {
            return _scene;
        }

        public void SelectObject(Objetos obj, int index)
        {
            _selection.Select(obj, index);
            OnObjectSelected?.Invoke(obj);
            glControl.Invalidate();
        }
        #endregion

        #region OpenGL Events
        private void glControl_OpenGLInitialized(object sender, EventArgs e)
        {
            var gl = glControl.OpenGL;

            gl.ClearColor(0.18f, 0.18f, 0.18f, 1f);
            gl.Enable(OpenGL.GL_DEPTH_TEST);
        }

        private void glControl_Resized(object sender, EventArgs e)
        {
            var gl = glControl.OpenGL;

            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();

            gl.Perspective(
                60f,
                glControl.Width / (float)glControl.Height,
                0.1f,
                100f
            );

            gl.Viewport(0, 0, glControl.Width, glControl.Height);
        }

        private void glControl_OpenGLDraw(object sender, RenderEventArgs args)
        {
            var gl = glControl.OpenGL;

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();

            _camera.Apply(gl);

            _grid.Draw(glControl, _camera.TargetX, _camera.TargetZ);

            _renderer.DrawScene(glControl, _scene);

            _gizmo.Draw(glControl, _selection.SelectedObject);

            gl.Flush();
        }

        #endregion

        #region Mouse Input 
        private void glControl_MouseDown(object sender, MouseEventArgs e)
        {
            glControl.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _gizmo.UpdateHover(glControl, _selection.SelectedObject, e.Location);
                _gizmo.BeginDrag();

                if (_gizmo.ActiveAxis != GizmoAxis.None)
                {
                    _draggingGizmo = true;
                    _lastMouse = e.Location;
                    return;
                }

                // Ray picking por color
                int index = PickObject(e.Location);
                if (index >= 0 && index < _scene.Objetos.Count)
                {
                    var obj = _scene.Objetos[index];
                    _selection.Select(obj, index);
                    OnObjectSelected?.Invoke(obj);
                }
                else
                {
                    _selection.Select(null, -1);
                    OnObjectSelected?.Invoke(null);
                }

                Invalidate();
            }

            if (e.Button == MouseButtons.Right)
            {
                _rotatingCamera = true;
                _lastCameraMouse = e.Location;
            }

            if (e.Button == MouseButtons.Middle)
            {
                _panningCamera = true;
                _lastCameraMouse = e.Location;
            }
        }

        private void glControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (_rotatingCamera)
            {
                int dx = e.X - _lastCameraMouse.X;
                int dy = e.Y - _lastCameraMouse.Y;

                _camera.Rotate(dx, dy);

                _lastCameraMouse = e.Location;
                glControl.Invalidate();
                return;
            }

            if (_panningCamera)
            {
                int dx = e.X - _lastCameraMouse.X;
                int dy = e.Y - _lastCameraMouse.Y;

                _camera.Pan(dx, dy);

                _lastCameraMouse = e.Location;
                glControl.Invalidate();
                return;
            }

            if (_draggingGizmo && _selection.SelectedObject != null)
            {
                int dx = e.X - _lastMouse.X;
                int dy = e.Y - _lastMouse.Y;

                _gizmo.Drag(_selection.SelectedObject, dx, dy);

                _lastMouse = e.Location;

                OnObjectChanged?.Invoke(_selection.SelectedObject);
                glControl.Invalidate();
                return;
            }

            _gizmo.UpdateHover(glControl, _selection.SelectedObject, e.Location);
        }

        private void glControl_MouseUp(object sender, MouseEventArgs e)
        {
            _rotatingCamera = false;
            _panningCamera = false;

            _draggingGizmo = false;

            _gizmo.EndDrag();

            glControl.Invalidate();
        }

        private void glControl_MouseWheel(object sender, MouseEventArgs e)
        {
            _camera.Zoom(e.Delta);
            glControl.Invalidate();
        }
        #endregion

        #region Optional Helpers

        public void SetSelection(Objetos obj, int index)
        {
            _selection.Select(obj, index);
            OnObjectSelected?.Invoke(obj);
            glControl.Invalidate();
        }

        public Objetos GetSelectedObject()
        {
            return _selection.SelectedObject;
        }

        public void RefrescarEscena()
        {
            glControl.Invalidate();
        }

        private int PickObject(Point mousePos)
        {
            var gl = glControl.OpenGL;

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();
            _camera.Apply(gl);

            // Dibujar cada objeto con un color único basado en su índice
            for (int i = 0; i < _scene.Objetos.Count; i++)
            {
                var obj = _scene.Objetos[i];

                // Convertir índice a color RGB (índice+1 para evitar el negro del fondo)
                int id = i + 1;
                float r = ((id >> 16) & 0xFF) / 255.0f;
                float g = ((id >> 8) & 0xFF) / 255.0f;
                float b = ((id) & 0xFF) / 255.0f;

                gl.PushMatrix();

                gl.Translate(obj.Transform.Position.X, obj.Transform.Position.Y, obj.Transform.Position.Z);
                gl.Rotate(obj.Transform.Rotation.X, 1, 0, 0);
                gl.Rotate(obj.Transform.Rotation.Y, 0, 1, 0);
                gl.Rotate(obj.Transform.Rotation.Z, 0, 0, 1);
                gl.Scale(obj.Transform.Scale.X, obj.Transform.Scale.Y, obj.Transform.Scale.Z);

                gl.Color(r, g, b);
                gl.Begin(OpenGL.GL_TRIANGLES);
                if (obj.MeshFilter?.Mesh != null)
                {
                    foreach (int idx in obj.MeshFilter.Mesh.Triangles)
                    {
                        var v = obj.MeshFilter.Mesh.Vertices[idx];
                        gl.Vertex(v.vector.X, v.vector.Y, v.vector.Z);
                    }
                }
                gl.End();

                gl.PopMatrix();
            }

            gl.Flush();

            // Leer el pixel bajo el cursor (Y invertido en OpenGL)
            int x = mousePos.X;
            int y = glControl.Height - mousePos.Y;

            byte[] pixel = new byte[3];
            gl.ReadPixels(x, y, 1, 1, OpenGL.GL_RGB, OpenGL.GL_UNSIGNED_BYTE, pixel);

            int pickedId = (pixel[0] << 16) | (pixel[1] << 8) | pixel[2];

            // Redibujar la escena normal inmediatamente
            glControl.Invalidate();

            return pickedId - 1; // -1 si fondo (pickedId == 0)
        }
        #endregion

    }
}
