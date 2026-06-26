using SharpGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.UI
{
    public class ButtonElement : UIElement
    {
        public string Text { get; set; } = "Button";

        public Color NormalColor { get; set; } = Color.FromArgb(70, 70, 70);
        public Color HoverColor { get; set; } = Color.FromArgb(95, 95, 95);
        public Color PressedColor { get; set; } = Color.FromArgb(45, 45, 45);
        public Color TextColor { get; set; } = Color.White;

        public event Action OnClick;

        private bool isHover;
        private bool isPressed;

        public override void Draw(OpenGL gl)
        {
            Color current = NormalColor;

            if (isPressed)
                current = PressedColor;
            else if (isHover)
                current = HoverColor;

            DrawRect(gl, current);
            DrawText(gl);
        }

        private void DrawRect(OpenGL gl, Color color)
        {
            var r = RectTransform;

            gl.Color(
                color.R / 255.0,
                color.G / 255.0,
                color.B / 255.0,
                color.A / 255.0
            );

            gl.Begin(OpenGL.GL_QUADS);
            gl.Vertex(r.Transform.X, r.Transform.Y);
            gl.Vertex(r.Transform.X + r.Width, r.Transform.Y);
            gl.Vertex(r.Transform.X + r.Width, r.Transform.Y + r.Height);
            gl.Vertex(r.Transform.X, r.Transform.Y + r.Height);
            gl.End();
        }

        private void DrawText(OpenGL gl)
        {
            gl.DrawText(
                (int)(RectTransform.Transform.X + 10),
                (int)(RectTransform.Transform.Y + 24),
                TextColor.R / 255f,
                TextColor.G / 255f,
                TextColor.B / 255f,
                "Arial",
                12f,
                Text
            );
        }

        public override void OnMouseMove(Point mouse)
        {
            isHover = Contains(mouse);
        }

        public override void OnMouseDown(Point mouse)
        {
            if (Contains(mouse))
                isPressed = true;
        }

        public override void OnMouseUp(Point mouse)
        {
            if (isPressed && Contains(mouse))
                OnClick?.Invoke();

            isPressed = false;
        }
    }
}
