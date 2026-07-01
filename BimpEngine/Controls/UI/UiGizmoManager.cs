using BimpEngine.Engine.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Controls.UI
{
    public class UiGizmoManager
    {
        private const int HandleSize = 6; 
        private string _activeHandle = "";
        private PointF _startDragPos;
        private RectangleF _startBounds;

        public void DibujarGizmo(Graphics g, RectangleF bounds)
        {
            using (Pen pen = new Pen(Color.FromArgb(0, 120, 255), 2))
            {
                g.DrawRectangle(pen, bounds.X, bounds.Y, bounds.Width, bounds.Height);
            }

            using (Brush brush = new SolidBrush(Color.White))
            using (Pen pen = new Pen(Color.FromArgb(0, 120, 255), 1))
            {
                g.FillRectangle(brush, bounds.X - HandleSize / 2, bounds.Y - HandleSize / 2, HandleSize, HandleSize);
                g.DrawRectangle(pen, bounds.X - HandleSize / 2, bounds.Y - HandleSize / 2, HandleSize, HandleSize);

                g.FillRectangle(brush, bounds.Right - HandleSize / 2, bounds.Bottom - HandleSize / 2, HandleSize, HandleSize);
                g.DrawRectangle(pen, bounds.Right - HandleSize / 2, bounds.Bottom - HandleSize / 2, HandleSize, HandleSize);

            }
        }

        public bool ProcesarMouseDown(Point mousePos, RectangleF bounds)
        {
            // Verificar si el clic dio en la manivela inferior derecha para cambiar tamaño
            RectangleF bottomRightHandle = new RectangleF(bounds.Right - HandleSize / 2, bounds.Bottom - HandleSize / 2, HandleSize, HandleSize);

            if (bottomRightHandle.Contains(mousePos))
            {
                _activeHandle = "BottomRight";
                _startDragPos = mousePos;
                _startBounds = bounds;
                return true;
            }

            // Verificar si dio dentro del cuerpo para moverlo
            if (bounds.Contains(mousePos))
            {
                _activeHandle = "Move";
                _startDragPos = mousePos;
                _startBounds = bounds;
                return true;
            }

            _activeHandle = "";
            return false;
        }

        public void ProcesarMouseMove(Point mousePos, RectTransform rect)
        {
            if (string.IsNullOrEmpty(_activeHandle)) return;

            float deltaX = mousePos.X - _startDragPos.X;
            float deltaY = mousePos.Y - _startDragPos.Y;

            if (_activeHandle == "BottomRight")
            {
                rect.Width = Math.Max(10, _startBounds.Width + deltaX);
                rect.Height = Math.Max(10, _startBounds.Height + deltaY);
            }
            else if (_activeHandle == "Move")
            {
                rect.Transform.X = _startBounds.X + deltaX;
                rect.Transform.Y = _startBounds.Y + deltaY;
            }
        }

        public void ProcesarMouseUp()
        {
            _activeHandle = "";
        }

    }
}
