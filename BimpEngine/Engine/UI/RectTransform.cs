using BimpEngine.Engine.Math;
using System.Drawing;

namespace BimpEngine.Engine.UI
{
    public class RectTransform
    {
        public Vector2 Transform { get; set; } = new Vector2();
        public double Width { get; set; } = 100;
        public double Height { get; set; } = 100;

        public PointF AnchorMin { get; set; } = new PointF(0.5f,0.5f);
        public PointF AnchorMax { get; set; } = new PointF(0.5f, 0.5f);
        public PointF PointF { get; set; } = new PointF(0.5f, 0.5f);

        public RectangleF GetBounds()
        {
            return new RectangleF((float)Transform.X, (float)Transform.Y, (float)Width, (float)Height);
        }
    }
}
