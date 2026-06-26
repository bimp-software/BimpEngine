using BimpEngine.Engine.Core;
using BimpEngine.Engine.Editor;
using BimpEngine.Engine.Editor.Gizmos;
using BimpEngine.Engine.Rendering;
using BimpEngine.Engine.World;
using SharpGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
            Invalidate();
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
                Invalidate();
                return;
            }

            if (_panningCamera)
            {
                int dx = e.X - _lastCameraMouse.X;
                int dy = e.Y - _lastCameraMouse.Y;

                _camera.Pan(dx, dy);

                _lastCameraMouse = e.Location;
                Invalidate();
                return;
            }

            if (_draggingGizmo && _selection.SelectedObject != null)
            {
                int dx = e.X - _lastMouse.X;
                int dy = e.Y - _lastMouse.Y;

                _gizmo.Drag(_selection.SelectedObject, dx, dy);

                _lastMouse = e.Location;

                OnObjectChanged?.Invoke(_selection.SelectedObject);
                Invalidate();
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

            Invalidate();
        }

        private void glControl_MouseWheel(object sender, MouseEventArgs e)
        {
            _camera.Zoom(e.Delta);
            Invalidate();
        }
        #endregion

        #region Optional Helpers

        public void SetSelection(Objetos obj, int index)
        {
            _selection.Select(obj, index);
            OnObjectSelected?.Invoke(obj);
            Invalidate();
        }

        public Objetos GetSelectedObject()
        {
            return _selection.SelectedObject;
        }

        #endregion

    }
}
