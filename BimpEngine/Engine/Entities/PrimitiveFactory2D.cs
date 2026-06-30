using BimpEngine.Engine.Entities.Primitive;
using BimpEngine.Engine.World;

namespace BimpEngine.Engine.Entities
{
    public static class PrimitiveFactory2D
    {
        public static Objetos Create(PrimitiveType2D tipo) => tipo switch
        {
            PrimitiveType2D.Square => new Square(),
            PrimitiveType2D.Rectangle => new Rectangle2D(),
            PrimitiveType2D.Circle => new Circle2D(),
            PrimitiveType2D.Triangle => new Triangle2D(),
            PrimitiveType2D.Diamond => new Diamond2D(),
            PrimitiveType2D.Pentagon => new Pentagon2D(),
            PrimitiveType2D.Hexagon => new Hexagon2D(),
            PrimitiveType2D.Star => new Star2D(),
            PrimitiveType2D.Arrow => new Arrow2D(),
            PrimitiveType2D.Line => new Line2D(),
            _ => new Square()
        };
    }
}
