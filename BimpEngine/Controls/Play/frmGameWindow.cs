using BimpEngine.Engine.Core;
using BimpEngine.Engine.Input;
using BimpEngine.Engine.Math;
using BimpEngine.Engine.Rendering;
using BimpEngine.Engine.Scripting;
using BimpEngine.Engine.World;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BimpEngine.Controls.Play
{
    public class frmGameWindow : Form
    {
        private readonly Scene _scene;
        private readonly SharpGL.OpenGLControl _glControl;
        private readonly Renderer _renderer = new Renderer();
        private readonly Dictionary<Guid, (Vector3 pos, Vector3 rot, Vector3 scale)> _snapshot = new();
        private bool _running;

        public event Action? OnStopped;

        public frmGameWindow(Scene scene)
        {
            _scene = scene;

            Text = "BimpEngine — Play";
            Size = new Size(1024, 640);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.Black;
            KeyPreview = true;

            _glControl = new SharpGL.OpenGLControl
            {
                Dock = DockStyle.Fill,
                DrawFPS = true,
                OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1,
                RenderContextType = SharpGL.RenderContextType.DIBSection,
                RenderTrigger = SharpGL.RenderTrigger.TimerBased,
                TabStop = true
            };

            ((System.ComponentModel.ISupportInitialize)_glControl).BeginInit();

            _glControl.OpenGLInitialized += GlControl_OpenGLInitialized;
            _glControl.OpenGLDraw += GlControl_OpenGLDraw;
            _glControl.Resized += (s, e) => AplicarProyeccion();
            _glControl.KeyDown += (s, e) => InputManager.KeyDown(e.KeyCode);
            _glControl.KeyUp += (s, e) => InputManager.KeyUp(e.KeyCode);

            Controls.Add(_glControl);

            ((System.ComponentModel.ISupportInitialize)_glControl).EndInit();

            FormClosing += (s, e) => Detener();
            Shown += (s, e) => { _glControl.Focus(); Iniciar(); };
        }

        private void Iniciar()
        {
            try
            {
                GuardarSnapshot();
                InputManager.Clear();
                Time.Start();
                _running = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo iniciar el modo Play:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        public void Detener()
        {
            if (!_running) return;
            _running = false;

            foreach (var obj in _scene.Objetos)
                foreach (var comp in obj.Scripts)
                    ScriptRuntime.DestroyComponent(comp);

            RestaurarSnapshot();
            InputManager.Clear();
            OnStopped?.Invoke();
        }

        private void GuardarSnapshot()
        {
            _snapshot.Clear();
            foreach (var obj in _scene.Objetos)
                GuardarSnapshotRecursivo(obj);
        }

        private void GuardarSnapshotRecursivo(Objetos obj)
        {
            _snapshot[obj.Id] = (
                new Vector3(obj.Transform.Position.X, obj.Transform.Position.Y, obj.Transform.Position.Z),
                new Vector3(obj.Transform.Rotation.X, obj.Transform.Rotation.Y, obj.Transform.Rotation.Z),
                new Vector3(obj.Transform.Scale.X, obj.Transform.Scale.Y, obj.Transform.Scale.Z));

            foreach (var hijo in obj.Children)
                GuardarSnapshotRecursivo(hijo);
        }

        private void RestaurarSnapshot()
        {
            foreach (var obj in _scene.Objetos)
                RestaurarSnapshotRecursivo(obj);
        }

        private void RestaurarSnapshotRecursivo(Objetos obj)
        {
            if (_snapshot.TryGetValue(obj.Id, out var t))
            {
                obj.Transform.Position = t.pos;
                obj.Transform.Rotation = t.rot;
                obj.Transform.Scale = t.scale;
            }
            foreach (var hijo in obj.Children)
                RestaurarSnapshotRecursivo(hijo);
        }

        private void GlControl_OpenGLInitialized(object sender, EventArgs e)
        {
            var gl = _glControl.OpenGL;
            gl.ClearColor(0f, 0f, 0f, 1f);
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            AplicarProyeccion();
        }

        private void AplicarProyeccion()
        {
            if (_glControl.OpenGL == null) return;
            var gl = _glControl.OpenGL;
            int w = _glControl.Width;
            int h = _glControl.Height == 0 ? 1 : _glControl.Height;

            gl.Viewport(0, 0, w, h);
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();

            float fov = (float)(_scene.MainCamera?.FieldOfView ?? 60);
            float near = (float)(_scene.MainCamera?.NearClip ?? 0.1);
            float far = (float)(_scene.MainCamera?.FarClip ?? 1000);

            gl.Perspective(fov, w / (float)h, near, far);
        }

        private void GlControl_OpenGLDraw(object sender, RenderEventArgs args)
        {
            if (!_running) return;

            try
            {
                Time.Update();
                _scene.Update();
            }
            catch (Exception ex)
            {
                Engine.Debug.Console.LogError("Error en el loop de juego: " + ex.Message, "Play");
                _running = false;
                return;
            }

            var gl = _glControl.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();
            AplicarCamara(gl);

            try
            {
                _renderer.DrawScene(_glControl, _scene);
            }
            catch (Exception ex)
            {
                Engine.Debug.Console.LogError("Error al renderizar: " + ex.Message, "Play");
            }

            gl.Flush();
        }

        private void AplicarCamara(OpenGL gl)
        {
            var cam = _scene.MainCamera;

            if (cam == null)
            {
                // Sin cámara en la escena: vista de respaldo alejada del origen
                gl.Translate(0, -2, -10);
                return;
            }

            gl.Rotate(-cam.Transform.Rotation.Z, 0, 0, 1);
            gl.Rotate(-cam.Transform.Rotation.Y, 0, 1, 0);
            gl.Rotate(-cam.Transform.Rotation.X, 1, 0, 0);
            gl.Translate(-cam.Transform.Position.X, -cam.Transform.Position.Y, -cam.Transform.Position.Z);
        }
    }
}