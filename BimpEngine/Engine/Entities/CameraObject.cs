using BimpEngine.Engine.World;
using SharpGL;

namespace BimpEngine.Engine.Entities
{
    public class CameraObject : Objetos
    {
        public double FieldOfView { get; set; } = 60;
        public double NearClip { get; set; } = 0.1;
        public double FarClip { get; set; } = 1000;

        // Largo visual del frustum en el editor (no afecta el renderizado real)
        public double GizmoLength { get; set; } = 4;

        public CameraObject()
        {
            Name = "Cámara";
        }

        public override void Draw(OpenGLControl glControl)
        {
            var gl = glControl.OpenGL;

            gl.PushMatrix();

            gl.Translate(
                Transform.Position.X,
                Transform.Position.Y,
                Transform.Position.Z);

            gl.Rotate(Transform.Rotation.X, 1, 0, 0);
            gl.Rotate(Transform.Rotation.Y, 0, 1, 0);
            gl.Rotate(Transform.Rotation.Z, 0, 0, 1);

            DrawCameraIcon(gl);
            DrawFrustum(gl);

            gl.PopMatrix();
        }

        private void DrawFrustum(OpenGL gl)
        {
            const double aspecto = 16.0 / 9.0;
            double fovRad = FieldOfView * System.Math.PI / 180.0;

            double distCercana = 0.3;
            double distLejana = GizmoLength;

            double altoLejano = System.Math.Tan(fovRad / 2.0) * distLejana;
            double anchoLejano = altoLejano * aspecto;

            double altoCercano = System.Math.Tan(fovRad / 2.0) * distCercana;
            double anchoCercano = altoCercano * aspecto;

            var flTL = (-anchoLejano, altoLejano, -distLejana);
            var flTR = (anchoLejano, altoLejano, -distLejana);
            var flBL = (-anchoLejano, -altoLejano, -distLejana);
            var flBR = (anchoLejano, -altoLejano, -distLejana);

            var ncTL = (-anchoCercano, altoCercano, -distCercana);
            var ncTR = (anchoCercano, altoCercano, -distCercana);
            var ncBL = (-anchoCercano, -altoCercano, -distCercana);
            var ncBR = (anchoCercano, -altoCercano, -distCercana);

            gl.LineWidth(IsSelected ? 2.5f : 1.2f);
            gl.Color(1.0, 0.95, 0.3);

            gl.Begin(OpenGL.GL_LINES);

            // desde el ojo de la cámara hacia las 4 esquinas del plano lejano
            Linea(gl, (0, 0, 0), flTL);
            Linea(gl, (0, 0, 0), flTR);
            Linea(gl, (0, 0, 0), flBL);
            Linea(gl, (0, 0, 0), flBR);

            // rectángulo lejano
            Linea(gl, flTL, flTR);
            Linea(gl, flTR, flBR);
            Linea(gl, flBR, flBL);
            Linea(gl, flBL, flTL);

            // rectángulo cercano
            Linea(gl, ncTL, ncTR);
            Linea(gl, ncTR, ncBR);
            Linea(gl, ncBR, ncBL);
            Linea(gl, ncBL, ncTL);

            gl.End();

            // línea de dirección (hacia dónde apunta)
            gl.LineWidth(1.5f);
            gl.Color(0.3, 0.9, 1.0);
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(0, 0, 0);
            gl.Vertex(0, 0, -distLejana * 1.5);
            gl.End();
        }

        private static void Linea(OpenGL gl, (double x, double y, double z) a, (double x, double y, double z) b)
        {
            gl.Vertex(a.x, a.y, a.z);
            gl.Vertex(b.x, b.y, b.z);
        }

        private void DrawCameraIcon(OpenGL gl)
        {
            gl.LineWidth(2);
            gl.Color(1.0, 1.0, 0.0);

            gl.Begin(OpenGL.GL_LINES);

            // cuerpo
            gl.Vertex(-0.5, -0.3, 0);
            gl.Vertex(0.5, -0.3, 0);

            gl.Vertex(0.5, -0.3, 0);
            gl.Vertex(0.5, 0.3, 0);

            gl.Vertex(0.5, 0.3, 0);
            gl.Vertex(-0.5, 0.3, 0);

            gl.Vertex(-0.5, 0.3, 0);
            gl.Vertex(-0.5, -0.3, 0);

            // lente
            gl.Vertex(0.5, 0.2, 0);
            gl.Vertex(1.0, 0.5, 0);

            gl.Vertex(0.5, -0.2, 0);
            gl.Vertex(1.0, -0.5, 0);

            gl.Vertex(1.0, 0.5, 0);
            gl.Vertex(1.0, -0.5, 0);

            // dirección
            gl.Vertex(0, 0, 0);
            gl.Vertex(0, 0, -2);

            gl.End();
        }
    }
}