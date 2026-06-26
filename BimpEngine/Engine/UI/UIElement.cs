using SharpGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.UI
{
    public abstract class UIElement
    {
        public string Name { get; set; } = "UI Element";
        public bool Enabled { get; set; } = true;
        public RectTransform RectTransform { get; set; } = new RectTransform();
        public abstract void Draw(OpenGL gl);

        public virtual void OnMouseMove(Point mouse) { }
        public virtual void OnMouseDown(Point mouse) { }
        public virtual void OnMouseUp(Point mouse) { }

        public bool Contains(Point mouse)
        {
            return mouse.X >= RectTransform.Transform.X &&
                   mouse.X <= RectTransform.Transform.X + RectTransform.Width &&
                   mouse.Y >= RectTransform.Transform.Y &&
                   mouse.Y <= RectTransform.Transform.Y + RectTransform.Height;
        }
    }
}
