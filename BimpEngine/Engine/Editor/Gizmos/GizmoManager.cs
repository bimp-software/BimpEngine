using BimpEngine.Engine.World;
using SharpGL;

namespace BimpEngine.Engine.Editor.Gizmos
{
    public class GizmoManager
    {
        public bool Is2D { get; set; } = false;
        public GizmoMode Mode { get; set; } = GizmoMode.Move;
        public GizmoAxis HoverAxis { get; private set; } = GizmoAxis.None;
        public GizmoAxis ActiveAxis { get; private set; } = GizmoAxis.None;
        private const double HoverDistance = 14.0;

        private double GetGizmoSize(Objetos obj)
        {
            double maxScale = System.Math.Max(
                obj.Transform.Scale.X, 
                System.Math.Max(obj.Transform.Scale.Y, obj.Transform.Scale.Z));

            return System.Math.Max(3.0, maxScale + 1.5);
        }

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

        private void DrawMove(OpenGLControl glControl, Objetos obj)
        {
            var gl = glControl.OpenGL;
            var pos = obj.Transform.Position;
            var Size = GetGizmoSize(obj);

            gl.PushMatrix();
            gl.Translate(pos.X, pos.Y, pos.Z);

            DrawArrow(gl, GizmoAxis.X, Size);
            DrawArrow(gl, GizmoAxis.Y, Size);
            if (!Is2D) DrawArrow(gl, GizmoAxis.Z, Size);

            gl.PopMatrix();
        }

        private void DrawArrow(OpenGL gl, GizmoAxis axis, double Size)
        {
            SetAxisColor(gl, axis);
            gl.LineWidth(4);

            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(0, 0, 0);
            if (axis == GizmoAxis.X) gl.Vertex(Size, 0, 0);
            if (axis == GizmoAxis.Y) gl.Vertex(0, Size, 0);
            if (axis == GizmoAxis.Z) gl.Vertex(0, 0, Size);
            gl.End();

            DrawArrowCone(gl, axis, Size);
        }

        private void DrawArrowCone(OpenGL gl, GizmoAxis axis, double Size)
        {
            gl.PushMatrix();

            if (axis == GizmoAxis.X) { gl.Translate(Size, 0, 0); gl.Rotate(0, 90, 0); }
            if (axis == GizmoAxis.Y) { gl.Translate(0, Size, 0); gl.Rotate(-90, 0, 0); }
            if (axis == GizmoAxis.Z) { gl.Translate(0, 0, Size); }

            double radius = 0.18;
            double height = 0.45;
            int segments = 24;

            gl.Begin(OpenGL.GL_TRIANGLES);
            for (int i = 0; i < segments; i++)
            {
                double a1 = i * System.Math.PI * 2.0 / segments;
                double a2 = (i + 1) * System.Math.PI * 2.0 / segments;

                gl.Vertex(0, 0, height);
                gl.Vertex(System.Math.Cos(a1) * radius, System.Math.Sin(a1) * radius, 0);
                gl.Vertex(System.Math.Cos(a2) * radius, System.Math.Sin(a2) * radius, 0);
            }
            gl.End();

            gl.PopMatrix();
        }

        private void DrawRotate(OpenGLControl glControl, Objetos obj)
        {
            var gl = glControl.OpenGL;
            var pos = obj.Transform.Position;

            gl.PushMatrix();
            gl.Translate(pos.X, pos.Y, pos.Z);
            gl.LineWidth(3);

            if (!Is2D)
            {
                DrawCircle(gl, GizmoAxis.X);
                DrawCircle(gl, GizmoAxis.Y);
            }
            DrawCircle(gl, GizmoAxis.Z);

            gl.PopMatrix();
        }

        private void DrawScale(OpenGLControl glControl, Objetos obj)
        {
            var gl = glControl.OpenGL;
            var pos = obj.Transform.Position;
            double Size = GetGizmoSize(obj);

            gl.PushMatrix();
            gl.Translate(pos.X, pos.Y, pos.Z);
            gl.LineWidth(4);

            gl.Begin(OpenGL.GL_LINES);
            DrawAxis(gl, GizmoAxis.X, 1, 0, 0, Size);
            DrawAxis(gl, GizmoAxis.Y, 0, 1, 0, Size);
            if (!Is2D) DrawAxis(gl, GizmoAxis.Z, 0, 0, 1, Size);
            gl.End();

            DrawScaleBox(gl, Size, 0, 0, GizmoAxis.X);
            DrawScaleBox(gl, 0, Size, 0, GizmoAxis.Y);
            if (!Is2D) DrawScaleBox(gl, 0, 0, Size, GizmoAxis.Z);

            gl.PopMatrix();
        }

        private void DrawAxis(OpenGL gl, GizmoAxis axis, double x, double y, double z, double Size)
        {
            SetAxisColor(gl, axis);
            gl.Vertex(0, 0, 0);
            gl.Vertex(x * Size, y * Size, z * Size);
        }

        private void DrawCircle(OpenGL gl, GizmoAxis axis)
        {
            SetAxisColor(gl, axis);

            bool resaltado = (HoverAxis == axis || ActiveAxis == axis);
            gl.LineWidth(resaltado ? 5 : 2);

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
            if (HoverAxis == axis || ActiveAxis == axis) { gl.Color(1.0, 1.0, 0.0); return; }
            if (axis == GizmoAxis.X) gl.Color(1.0, 0.0, 0.0);
            else if (axis == GizmoAxis.Y) gl.Color(0.0, 1.0, 0.0);
            else if (axis == GizmoAxis.Z) gl.Color(0.0, 0.0, 1.0);
            else gl.Color(1.0, 1.0, 1.0);
        }

        public void UpdateHover(OpenGLControl glControl, Objetos selectedObject, Point mouse)
        {
            if (selectedObject == null)
            {
                HoverAxis = GizmoAxis.None;
                return;
            }

            if (Mode == GizmoMode.Rotate)
                UpdateHoverRotate(glControl, selectedObject, mouse);
            else
                UpdateHoverLinear(glControl, selectedObject, mouse);
        }

        private void UpdateHoverLinear(OpenGLControl glControl, Objetos selectedObject, Point mouse)
        {
            var pos = selectedObject.Transform.Position;
            double Size = GetGizmoSize(selectedObject);

            PointF center = WorldToScreen(glControl, pos.X, pos.Y, pos.Z);
            PointF xEnd = WorldToScreen(glControl, pos.X + Size, pos.Y, pos.Z);
            PointF yEnd = WorldToScreen(glControl, pos.X, pos.Y + Size, pos.Z);
            PointF zEnd = WorldToScreen(glControl, pos.X, pos.Y, pos.Z + Size);

            double dx = DistanceToLine(mouse, center, xEnd);
            double dy = DistanceToLine(mouse, center, yEnd);
            double dz = DistanceToLine(mouse, center, zEnd);

            HoverAxis = GizmoAxis.None;
            double min = HoverDistance;

            if (dx < min) { min = dx; HoverAxis = GizmoAxis.X; }
            if (dy < min) { min = dy; HoverAxis = GizmoAxis.Y; }
            if (dz < min) { HoverAxis = GizmoAxis.Z; }
        }

        private void UpdateHoverRotate(OpenGLControl glControl, Objetos selectedObject, Point mouse)
        {
            var pos = selectedObject.Transform.Position;
            double radius = 2.5;
            const int segments = 80;
            const double toleranceScreen = 12.0;

            HoverAxis = GizmoAxis.None;
            double minDist = toleranceScreen;

            foreach (var axis in new[] { GizmoAxis.X, GizmoAxis.Y, GizmoAxis.Z })
            {
                PointF? prevPoint = null;

                for (int i = 0; i <= segments; i++)
                {
                    double angle = i * System.Math.PI * 2.0 / segments;
                    double wx = pos.X, wy = pos.Y, wz = pos.Z;

                    if (axis == GizmoAxis.X)
                    {
                        wy += System.Math.Cos(angle) * radius;
                        wz += System.Math.Sin(angle) * radius;
                    }
                    else if (axis == GizmoAxis.Y)
                    {
                        wx += System.Math.Cos(angle) * radius;
                        wz += System.Math.Sin(angle) * radius;
                    }
                    else // Z
                    {
                        wx += System.Math.Cos(angle) * radius;
                        wy += System.Math.Sin(angle) * radius;
                    }

                    PointF screenPoint = WorldToScreen(glControl, wx, wy, wz);

                    if (prevPoint.HasValue)
                    {
                        double dist = DistanceToLine(mouse, prevPoint.Value, screenPoint);
                        if (dist < minDist)
                        {
                            minDist = dist;
                            HoverAxis = axis;
                        }
                    }

                    prevPoint = screenPoint;
                }
            }
        }

        public void BeginDrag()
        {
            if (Is2D && Mode == GizmoMode.Rotate)
            {
                ActiveAxis = GizmoAxis.Z;
                return;
            }
            ActiveAxis = HoverAxis;
        }
        public void EndDrag() => ActiveAxis = GizmoAxis.None;

        public void Drag(Objetos selectedObject, int deltaX, int deltaY)
        {
            if (selectedObject == null || ActiveAxis == GizmoAxis.None) return;

            if (Is2D && ActiveAxis == GizmoAxis.Z && Mode != GizmoMode.Rotate) return;

            if (Mode == GizmoMode.Move) DragMove(selectedObject, deltaX, deltaY);
            if (Mode == GizmoMode.Rotate) DragRotate(selectedObject, deltaX, deltaY);
            if (Mode == GizmoMode.Scale) DragScale(selectedObject, deltaX, deltaY);
        }

        private void DragMove(Objetos obj, int dx, int dy)
        {
            double speed = 0.03;
            if (ActiveAxis == GizmoAxis.X) obj.Transform.Position.X += dx * speed;
            if (ActiveAxis == GizmoAxis.Y) obj.Transform.Position.Y -= dy * speed;
            if (ActiveAxis == GizmoAxis.Z) obj.Transform.Position.Z -= dx * speed;
        }

        private void DragRotate(Objetos selectedObject, int deltaX, int deltaY)
        {
            double rotateSpeed = 0.5;

            if (Is2D)
            {
                selectedObject.Transform.Rotation.Z += deltaX * rotateSpeed;
                return;
            }

            double delta = System.Math.Abs(deltaX) > System.Math.Abs(deltaY) ? deltaX : deltaY;

            if (ActiveAxis == GizmoAxis.X)
                selectedObject.Transform.Rotation.X += deltaY * rotateSpeed;

            if (ActiveAxis == GizmoAxis.Y)
                selectedObject.Transform.Rotation.Y += deltaX * rotateSpeed;

            if (ActiveAxis == GizmoAxis.Z)
                selectedObject.Transform.Rotation.Z += delta * rotateSpeed;
        }

        private void DragScale(Objetos obj, int dx, int dy)
        {
            double speed = 0.02;
            if (ActiveAxis == GizmoAxis.X) obj.Transform.Scale.X += dx * speed;
            if (ActiveAxis == GizmoAxis.Y) obj.Transform.Scale.Y -= dy * speed;
            if (ActiveAxis == GizmoAxis.Z) obj.Transform.Scale.Z += dx * speed;

            obj.Transform.Scale.X = System.Math.Max(0.1, obj.Transform.Scale.X);
            obj.Transform.Scale.Y = System.Math.Max(0.1, obj.Transform.Scale.Y);
            obj.Transform.Scale.Z = System.Math.Max(0.1, obj.Transform.Scale.Z);
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

            double[] sx = new double[1];
            double[] sy = new double[1];
            double[] sz = new double[1];

            gl.Project(x, y, z, model, projection, viewport, sx, sy, sz);

            return new PointF((float)sx[0], (float)(glControl.Height - sy[0]));
        }

        private double DistanceToLine(Point p, PointF a, PointF b)
        {
            double dx = b.X - a.X;
            double dy = b.Y - a.Y;

            if (dx == 0 && dy == 0)
                return System.Math.Sqrt(System.Math.Pow(p.X - a.X, 2) + System.Math.Pow(p.Y - a.Y, 2));

            double t = System.Math.Clamp(
                ((p.X - a.X) * dx + (p.Y - a.Y) * dy) / (dx * dx + dy * dy),
                0, 1);

            return System.Math.Sqrt(
                System.Math.Pow(p.X - (a.X + t * dx), 2) +
                System.Math.Pow(p.Y - (a.Y + t * dy), 2));
        }
    }
}
