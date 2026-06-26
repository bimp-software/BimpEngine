using BimpEngine.Engine.World;
using SharpGL;
using System;

namespace BimpEngine.Engine.Editor.Gizmos
{
    public class GizmoManager
    {
        public GizmoMode Mode { get; set; } = GizmoMode.Move;

        public GizmoAxis HoverAxis { get; private set; } = GizmoAxis.None;
        public GizmoAxis ActiveAxis { get; private set; } = GizmoAxis.None;

        private const double Size = 3.0;
        private const double HoverDistance = 10.0;

        public void Draw(OpenGLControl glControl, Objetos selectedObject)
        {
            if (selectedObject == null)
                return;

            switch (Mode)
            {
                case GizmoMode.Move:
                    DrawMove(glControl, selectedObject);
                    break;

                case GizmoMode.Rotate:
                    DrawRotate(glControl, selectedObject);
                    break;

                case GizmoMode.Scale:
                    DrawScale(glControl, selectedObject);
                    break;
            }
        }

        private void DrawMove(OpenGLControl glControl, Objetos selectedObject)
        {
            var gl = glControl.OpenGL;
            var pos = selectedObject.Transform.Position;

            gl.PushMatrix();
            gl.Translate(pos.X, pos.Y, pos.Z);

            DrawArrow(gl, GizmoAxis.X);
            DrawArrow(gl, GizmoAxis.Y);
            DrawArrow(gl, GizmoAxis.Z);

            gl.PopMatrix();
        }

        private void DrawArrow(OpenGL gl, GizmoAxis axis)
        {
            SetAxisColor(gl, axis);

            gl.LineWidth(4);

            gl.Begin(OpenGL.GL_LINES);

            gl.Vertex(0, 0, 0);

            if (axis == GizmoAxis.X)
                gl.Vertex(Size, 0, 0);

            if (axis == GizmoAxis.Y)
                gl.Vertex(0, Size, 0);

            if (axis == GizmoAxis.Z)
                gl.Vertex(0, 0, Size);

            gl.End();

            DrawArrowCone(gl, axis);
        }

        private void DrawArrowCone(OpenGL gl, GizmoAxis axis)
        {
            gl.PushMatrix();

            if (axis == GizmoAxis.X)
            {
                gl.Translate(Size, 0, 0);
                gl.Rotate(0, 90, 0);
            }

            if (axis == GizmoAxis.Y)
            {
                gl.Translate(0, Size, 0);
                gl.Rotate(-90, 0, 0);
            }

            if (axis == GizmoAxis.Z)
            {
                gl.Translate(0, 0, Size);
            }

            double radius = 0.18;
            double height = 0.45;
            int segments = 24;

            gl.Begin(OpenGL.GL_TRIANGLES);

            for (int i = 0; i < segments; i++)
            {
                double a1 = i * System.Math.PI * 2.0 / segments;
                double a2 = (i + 1) * System.Math.PI * 2.0 / segments;

                double x1 = System.Math.Cos(a1) * radius;
                double y1 = System.Math.Sin(a1) * radius;

                double x2 = System.Math.Cos(a2) * radius;
                double y2 = System.Math.Sin(a2) * radius;

                gl.Vertex(0, 0, height);
                gl.Vertex(x1, y1, 0);
                gl.Vertex(x2, y2, 0);
            }

            gl.End();

            gl.PopMatrix();
        }

        private void DrawRotate(OpenGLControl glControl, Objetos selectedObject)
        {
            var gl = glControl.OpenGL;
            var pos = selectedObject.Transform.Position;

            gl.PushMatrix();
            gl.Translate(pos.X, pos.Y, pos.Z);
            gl.LineWidth(3);

            DrawCircle(gl, GizmoAxis.X);
            DrawCircle(gl, GizmoAxis.Y);
            DrawCircle(gl, GizmoAxis.Z);

            gl.PopMatrix();
        }

        private void DrawScale(OpenGLControl glControl, Objetos selectedObject)
        {
            var gl = glControl.OpenGL;
            var pos = selectedObject.Transform.Position;

            gl.PushMatrix();
            gl.Translate(pos.X, pos.Y, pos.Z);
            gl.LineWidth(4);

            gl.Begin(OpenGL.GL_LINES);

            DrawAxis(gl, GizmoAxis.X, 1, 0, 0);
            DrawAxis(gl, GizmoAxis.Y, 0, 1, 0);
            DrawAxis(gl, GizmoAxis.Z, 0, 0, 1);

            gl.End();

            DrawScaleBox(gl, Size, 0, 0, GizmoAxis.X);
            DrawScaleBox(gl, 0, Size, 0, GizmoAxis.Y);
            DrawScaleBox(gl, 0, 0, Size, GizmoAxis.Z);

            gl.PopMatrix();
        }

        private void DrawAxis(OpenGL gl, GizmoAxis axis, double x, double y, double z)
        {
            SetAxisColor(gl, axis);

            gl.Vertex(0, 0, 0);
            gl.Vertex(x * Size, y * Size, z * Size);
        }

        private void DrawCircle(OpenGL gl, GizmoAxis axis)
        {
            SetAxisColor(gl, axis);

            double radius = 2.5;

            gl.Begin(OpenGL.GL_LINE_LOOP);

            for (int i = 0; i < 80; i++)
            {
                double angle = i * System.Math.PI * 2.0 / 80.0;

                if (axis == GizmoAxis.X)
                    gl.Vertex(0, System.Math.Cos(angle) * radius, System.Math.Sin(angle) * radius);

                if (axis == GizmoAxis.Y)
                    gl.Vertex(System.Math.Cos(angle) * radius, 0, System.Math.Sin(angle) * radius);

                if (axis == GizmoAxis.Z)
                    gl.Vertex(System.Math.Cos(angle) * radius, System.Math.Sin(angle) * radius, 0);
            }

            gl.End();
        }

        private void DrawScaleBox(OpenGL gl, double x, double y, double z, GizmoAxis axis)
        {
            gl.PushMatrix();
            gl.Translate(x, y, z);

            SetAxisColor(gl, axis);

            double s = 0.18;

            gl.Begin(OpenGL.GL_QUADS);

            gl.Vertex(-s, -s, 0);
            gl.Vertex(s, -s, 0);
            gl.Vertex(s, s, 0);
            gl.Vertex(-s, s, 0);

            gl.End();

            gl.PopMatrix();
        }

        private void SetAxisColor(OpenGL gl, GizmoAxis axis)
        {
            if (HoverAxis == axis || ActiveAxis == axis)
            {
                gl.Color(1.0, 1.0, 0.0);
                return;
            }

            if (axis == GizmoAxis.X)
                gl.Color(1.0, 0.0, 0.0);
            else if (axis == GizmoAxis.Y)
                gl.Color(0.0, 1.0, 0.0);
            else if (axis == GizmoAxis.Z)
                gl.Color(0.0, 0.0, 1.0);
            else
                gl.Color(1.0, 1.0, 1.0);
        }

        public void UpdateHover(OpenGLControl glControl, Objetos selectedObject, Point mouse)
        {
            if (selectedObject == null)
            {
                HoverAxis = GizmoAxis.None;
                return;
            }

            var pos = selectedObject.Transform.Position;

            PointF center = WorldToScreen(glControl, pos.X, pos.Y, pos.Z);

            PointF xEnd = WorldToScreen(glControl, pos.X + Size, pos.Y, pos.Z);
            PointF yEnd = WorldToScreen(glControl, pos.X, pos.Y + Size, pos.Z);
            PointF zEnd = WorldToScreen(glControl, pos.X, pos.Y, pos.Z + Size);

            double dx = DistanceToLine(mouse, center, xEnd);
            double dy = DistanceToLine(mouse, center, yEnd);
            double dz = DistanceToLine(mouse, center, zEnd);

            HoverAxis = GizmoAxis.None;
            double min = HoverDistance;

            if (dx < min)
            {
                min = dx;
                HoverAxis = GizmoAxis.X;
            }

            if (dy < min)
            {
                min = dy;
                HoverAxis = GizmoAxis.Y;
            }

            if (dz < min)
            {
                HoverAxis = GizmoAxis.Z;
            }
        }

        public void BeginDrag()
        {
            ActiveAxis = HoverAxis;
        }

        public void EndDrag()
        {
            ActiveAxis = GizmoAxis.None;
        }

        public void Drag(Objetos selectedObject, int deltaX, int deltaY)
        {
            if (selectedObject == null || ActiveAxis == GizmoAxis.None)
                return;

            if (Mode == GizmoMode.Move)
                DragMove(selectedObject, deltaX, deltaY);

            if (Mode == GizmoMode.Rotate)
                DragRotate(selectedObject, deltaX, deltaY);

            if (Mode == GizmoMode.Scale)
                DragScale(selectedObject, deltaX, deltaY);
        }

        private void DragMove(Objetos selectedObject, int deltaX, int deltaY)
        {
            double speed = 0.03;

            if (ActiveAxis == GizmoAxis.X)
                selectedObject.Transform.Position.X += deltaX * speed;

            if (ActiveAxis == GizmoAxis.Y)
                selectedObject.Transform.Position.Y -= deltaY * speed;

            if (ActiveAxis == GizmoAxis.Z)
                selectedObject.Transform.Position.Z += deltaX * speed;
        }

        private void DragRotate(Objetos selectedObject, int deltaX, int deltaY)
        {
            double rotateSpeed = 0.5;

            if (ActiveAxis == GizmoAxis.X)
                selectedObject.Transform.Rotation.X += deltaY * rotateSpeed;

            if (ActiveAxis == GizmoAxis.Y)
                selectedObject.Transform.Rotation.Y += deltaX * rotateSpeed;

            if (ActiveAxis == GizmoAxis.Z)
                selectedObject.Transform.Rotation.Z += deltaX * rotateSpeed;
        }

        private void DragScale(Objetos selectedObject, int deltaX, int deltaY)
        {
            double scaleSpeed = 0.02;

            if (ActiveAxis == GizmoAxis.X)
                selectedObject.Transform.Scale.X += deltaX * scaleSpeed;

            if (ActiveAxis == GizmoAxis.Y)
                selectedObject.Transform.Scale.Y -= deltaY * scaleSpeed;

            if (ActiveAxis == GizmoAxis.Z)
                selectedObject.Transform.Scale.Z += deltaX * scaleSpeed;

            selectedObject.Transform.Scale.X = System.Math.Max(0.1, selectedObject.Transform.Scale.X);
            selectedObject.Transform.Scale.Y = System.Math.Max(0.1, selectedObject.Transform.Scale.Y);
            selectedObject.Transform.Scale.Z = System.Math.Max(0.1, selectedObject.Transform.Scale.Z);
        }

        private PointF WorldToScreen(OpenGLControl glControl, double x, double y, double z)
        {
            var gl = glControl.OpenGL;

            double[] model = new double[16];
            double[] projection = new double[16];
            int[] viewport = new int[4];

            gl.GetDouble(OpenGL.GL_MODELVIEW_MATRIX, model);
            gl.GetDouble(OpenGL.GL_PROJECTION_MATRIX, projection);
            gl.GetInteger(OpenGL.GL_VIEWPORT, viewport);

            double[] screenX = new double[1];
            double[] screenY = new double[1];
            double[] screenZ = new double[1];

            gl.Project(
                x, y, z,
                model,
                projection,
                viewport,
                screenX,
                screenY,
                screenZ
            );

            return new PointF(
                (float)screenX[0],
                (float)(glControl.Height - screenY[0])
            );
        }

        private double DistanceToLine(Point p, PointF a, PointF b)
        {
            double px = p.X;
            double py = p.Y;

            double ax = a.X;
            double ay = a.Y;
            double bx = b.X;
            double by = b.Y;

            double dx = bx - ax;
            double dy = by - ay;

            if (dx == 0 && dy == 0)
                return System.Math.Sqrt(System.Math.Pow(px - ax, 2) + System.Math.Pow(py - ay, 2));

            double t = ((px - ax) * dx + (py - ay) * dy) / (dx * dx + dy * dy);
            t = System.Math.Max(0, System.Math.Min(1, t));

            double closestX = ax + t * dx;
            double closestY = ay + t * dy;

            return System.Math.Sqrt(System.Math.Pow(px - closestX, 2) + System.Math.Pow(py - closestY, 2));
        }
    }
}
