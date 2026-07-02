using BimpEngine.Engine.Core;
using BimpEngine.Engine.Editor;
using BimpEngine.Engine.Editor.Gizmos;
using BimpEngine.Engine.Input;
using BimpEngine.Engine.Physics;
using BimpEngine.Engine.Project;
using BimpEngine.Engine.Project.Enum;
using BimpEngine.Engine.Rendering;
using BimpEngine.Engine.World;
using OpenTK.Graphics.OpenGL;
using SharpGL;

namespace BimpEngine.Controls.Escena
{
    public partial class EscenaControl : UserControl
    {
        #region Core Engine
        private ProjectMode _mode = ProjectMode.Mode3D;

        private EditorCamera _camera3D = new EditorCamera();
        private EditorCamera2D _camera2D = new EditorCamera2D();

        private Grid _grid3D = new Grid();
        private Grid2D _grid2D = new Grid2D();

        private Scene _scene = new Scene();
        private Renderer _renderer = new Renderer();
        private CollisionDrawer _collisionDrawer = new CollisionDrawer();
        private CollisionRenderer _collisionRenderer = new CollisionRenderer();

        private SelectionManager _selection = new SelectionManager();
        private GizmoManager _gizmo = new GizmoManager();
        private OrientationGizmo _orientationGizmo = new OrientationGizmo();

        public EditorCamera GetEditorCamera() => _camera3D;
        #endregion

        #region Input State
        private bool _rotatingCamera;
        private bool _panningCamera;
        private bool _draggingGizmo;

        private Point _lastMouse;
        private Point _lastCameraMouse;
        #endregion

        #region Toolbar de modo
        private Button _btnMove;
        private Button _btnRotate;
        private Button _btnScale;

        private static readonly Color ColorActivo = Color.FromArgb(0, 122, 204);
        private static readonly Color ColorInactivo = Color.FromArgb(55, 55, 58);
        #endregion

        #region Events
        public event Action<Objetos> OnObjectSelected;
        public event Action<Objetos> OnObjectChanged;
        public event Action<string> OnModelDropped;
        public event Action<EditorCamera> OnCameraChanged;
        #endregion

        #region Constructor
        public EscenaControl()
        {
            InitializeComponent();
            glControl.MouseWheel += glControl_MouseWheel;

            ConstruirToolbarModo();
            ActualizarBotonesModo();

            glControl.KeyDown += glControl_KeyDown;
            glControl.PreviewKeyDown += (s, e) =>
            {
                e.IsInputKey = true;
            };
        }
        #endregion

        #region Modo 2D / 3D

        public void SetMode(ProjectMode mode)
        {
            _mode = mode;
            _gizmo.Is2D = (_mode == ProjectMode.Mode2D);

            _camera3D = new EditorCamera();
            _camera2D = new EditorCamera2D();


            AplicarProyeccion();
            glControl.Invalidate();
        }

        public ProjectMode CurrentMode => _mode;

        #endregion

        #region Public API
        public void SetScene(Scene scene) => _scene = scene;
        public Scene GetScene() => _scene;

        public void SetSelection(Objetos obj, int index)
        {
            _selection.Select(obj, index);
            OnObjectSelected?.Invoke(obj);
            glControl.Invalidate();
        }

        public Objetos GetSelectedObject() => _selection.SelectedObject;

        public void RefrescarEscena() => glControl.Invalidate();
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
            AplicarProyeccion();
        }

        private void glControl_OpenGLDraw(object sender, RenderEventArgs args)
        {
            var gl = glControl.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            if (_mode == ProjectMode.Mode3D)
            {
                Draw3D(gl);
            }
            else
            {
                Draw2D(gl);
            }
            _orientationGizmo.Draw(glControl, _camera3D);
            OnCameraChanged?.Invoke(_camera3D);
            gl.Flush();
        }

        private void Draw3D(OpenGL gl)
        {
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();
            _camera3D.Apply(gl);

            _grid3D.Draw(glControl, _camera3D.TargetX, _camera3D.TargetZ);
            _renderer.DrawScene(glControl, _scene);
            _collisionDrawer.Draw(glControl, _selection.SelectedObject);
            _collisionRenderer.Draw(glControl, _selection.SelectedObject);
            _gizmo.Draw(glControl, _selection.SelectedObject);
        }

        private void Draw2D(OpenGL gl)
        {
            _camera2D.Apply(gl, glControl.Width, glControl.Height);

            _grid2D.Draw(gl, _camera2D, glControl.Width, glControl.Height);
            _renderer.DrawScene(glControl, _scene);
            _collisionDrawer.Draw(glControl, _selection.SelectedObject);
            _collisionRenderer.Draw(glControl, _selection.SelectedObject);
            _gizmo.Draw(glControl, _selection.SelectedObject);
        }

        private void AplicarProyeccion()
        {
            var gl = glControl.OpenGL;
            int w = glControl.Width;
            int h = glControl.Height == 0 ? 1 : glControl.Height;

            gl.Viewport(0, 0, w, h);

            if (_mode == ProjectMode.Mode3D)
            {
                gl.MatrixMode(OpenGL.GL_PROJECTION);
                gl.LoadIdentity();
                gl.Perspective(60f, w / (float)h, 0.1f, 100f);
            }
            else
            {
                _camera2D.Apply(gl, w, h);
            }
            gl.Viewport(0, 0, glControl.Width, glControl.Height);
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
                if (_mode == ProjectMode.Mode3D)
                    _rotatingCamera = true;
                else
                    _panningCamera = true;

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
            if (_rotatingCamera && _mode == ProjectMode.Mode3D)
            {
                int dx = e.X - _lastCameraMouse.X;
                int dy = e.Y - _lastCameraMouse.Y;
                _camera3D.Rotate(dx, dy);
                _lastCameraMouse = e.Location;
                glControl.Invalidate();
                return;
            }

            if (_panningCamera)
            {
                int dx = e.X - _lastCameraMouse.X;
                int dy = e.Y - _lastCameraMouse.Y;

                if (_mode == ProjectMode.Mode3D)
                    _camera3D.Pan(dx, dy);
                else
                    _camera2D.DoPan(dx, dy, glControl.Width, glControl.Height);

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
            if (_mode == ProjectMode.Mode3D)
                _camera3D.Zoom(e.Delta);
            else
                _camera2D.DoZoom(e.Delta);

            glControl.Invalidate();
        }

        #endregion

        #region Picking

        private int PickObject(Point mousePos)
        {
            var gl = glControl.OpenGL;

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();

            if (_mode == ProjectMode.Mode3D)
                _camera3D.Apply(gl);

            for (int i = 0; i < _scene.Objetos.Count; i++)
            {
                var obj = _scene.Objetos[i];

                int id = i + 1;
                float r = ((id >> 16) & 0xFF) / 255.0f;
                float g = ((id >> 8) & 0xFF) / 255.0f;
                float b = (id & 0xFF) / 255.0f;

                gl.PushMatrix();
                gl.Translate(obj.Transform.Position.X, obj.Transform.Position.Y, obj.Transform.Position.Z);
                gl.Rotate(obj.Transform.Rotation.X, 1, 0, 0);
                gl.Rotate(obj.Transform.Rotation.Y, 0, 1, 0);
                gl.Rotate(obj.Transform.Rotation.Z, 0, 0, 1);
                gl.Scale(obj.Transform.Scale.X, obj.Transform.Scale.Y, obj.Transform.Scale.Z);

                gl.Color(r, g, b);
                gl.Begin(OpenGL.GL_TRIANGLES);
                if (obj.MeshFilter?.Mesh != null)
                    foreach (int idx in obj.MeshFilter.Mesh.Triangles)
                    {
                        var v = obj.MeshFilter.Mesh.Vertices[idx];
                        gl.Vertex(v.vector.X, v.vector.Y, v.vector.Z);
                    }
                gl.End();
                gl.PopMatrix();
            }

            gl.Flush();

            int x = mousePos.X;
            int y = glControl.Height - mousePos.Y;
            byte[] pixel = new byte[3];
            gl.ReadPixels(x, y, 1, 1, OpenGL.GL_RGB, OpenGL.GL_UNSIGNED_BYTE, pixel);

            int pickedId = (pixel[0] << 16) | (pixel[1] << 8) | pixel[2];
            glControl.Invalidate();
            return pickedId - 1;
        }

        #endregion

        private void ConstruirToolbarModo()
        {
            _btnMove = CrearBotonModo("Mover (W)", 8);
            _btnRotate = CrearBotonModo("Rotar (E)", 96);
            _btnScale = CrearBotonModo("Escalar (R)", 184);

            _btnMove.Click += (s, e) => SetGizmoMode(GizmoMode.Move);
            _btnRotate.Click += (s, e) => SetGizmoMode(GizmoMode.Rotate);
            _btnScale.Click += (s, e) => SetGizmoMode(GizmoMode.Scale);

            panel4.Controls.Add(_btnMove);
            panel4.Controls.Add(_btnRotate);
            panel4.Controls.Add(_btnScale);
        }

        private Button CrearBotonModo(string texto, int x)
        {
            return new Button
            {
                Text = texto,
                Location = new Point(x, 4),
                Size = new Size(84, 26),
                FlatStyle = FlatStyle.Flat,
                BackColor = ColorInactivo,
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                TabStop = false
            };
        }

        private void ActualizarBotonesModo()
        {
            _btnMove.BackColor = _gizmo.Mode == GizmoMode.Move ? ColorActivo : ColorInactivo;
            _btnRotate.BackColor = _gizmo.Mode == GizmoMode.Rotate ? ColorActivo : ColorInactivo;
            _btnScale.BackColor = _gizmo.Mode == GizmoMode.Scale ? ColorActivo : ColorInactivo;
        }

        public void SetGizmoMode(GizmoMode mode)
        {
            _gizmo.Mode = mode;
            ActualizarBotonesModo();
            ActualizarCursor();
            glControl.Invalidate();
        }

        private void glControl_KeyDown(object sender, KeyEventArgs e)
        {
            InputManager.KeyDown(e.KeyCode);

            if (e.KeyCode.ToString() == EngineSettings.Current.GetKey("editor_move"))
                SetGizmoMode(GizmoMode.Move);

            if (e.KeyCode.ToString() == EngineSettings.Current.GetKey("editor_rotate"))
                SetGizmoMode(GizmoMode.Rotate);

            if (e.KeyCode.ToString() == EngineSettings.Current.GetKey("editor_scale"))
                SetGizmoMode(GizmoMode.Scale);
        }

        private void ActualizarCursor()
        {
            if (_rotatingCamera)
            {
                glControl.Cursor = Cursors.Hand;
                return;
            }
            if (_panningCamera)
            {
                glControl.Cursor = Cursors.SizeAll;
                return;
            }

            bool sobreEje = _gizmo.ActiveAxis != GizmoAxis.None || _gizmo.HoverAxis != GizmoAxis.None;

            if (sobreEje)
            {
                glControl.Cursor = _gizmo.Mode switch
                {
                    GizmoMode.Move => Cursors.SizeAll,
                    GizmoMode.Rotate => Cursors.Hand,
                    GizmoMode.Scale => Cursors.SizeNWSE,
                    _ => Cursors.Default
                };
                return;
            }

            glControl.Cursor = Cursors.Default;
        }

        private void glControl_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(DataFormats.StringFormat) == true)
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void glControl_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data?.GetData(DataFormats.StringFormat) is string path)
                OnModelDropped?.Invoke(path);
        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        private void glControl_KeyUp(object sender, KeyEventArgs e)
        {
            InputManager.KeyUp(e.KeyCode);
        }
    }
}