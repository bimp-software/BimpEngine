using SharpGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.UI
{
    public class TextElement : UIElement
    {
        public string Text { get; set; } = "Text";
        public Color Color { get; set; } = Color.White;
        public string FontName { get; set; } = "Arial";
        public float FontSize { get; set; } = 14f;

        public override void Draw(OpenGL gl)
        {
            gl.DrawText(
                (int)RectTransform.Transform.X,
                (int)RectTransform.Transform.Y,
                Color.R / 255f,
                Color.G / 255f,
                Color.B / 255f,
                FontName,
                FontSize,
                Text
            );
        }
    }
}
