using BimpEngine.Engine.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Entities.Primitive
{
    public class Square : PrimitiveObject2D
    {
        public Square() : base(
            PrimitiveType2D.Square,
            "Square",
            PrimitiveMeshFactory2D.CreateSquare())
        { }
    }

    public class Rectangle2D : PrimitiveObject2D
    {
        public Rectangle2D() : base(
            PrimitiveType2D.Rectangle,
            "Rectangle",
            PrimitiveMeshFactory2D.CreateRectangle())
        { }
    }

    public class Circle2D : PrimitiveObject2D
    {
        public Circle2D() : base(
            PrimitiveType2D.Circle,
            "Circle",
            PrimitiveMeshFactory2D.CreateCircle())
        { }
    }

    public class Triangle2D : PrimitiveObject2D
    {
        public Triangle2D() : base(
            PrimitiveType2D.Triangle,
            "Triangle",
            PrimitiveMeshFactory2D.CreateTriangle())
        { }
    }

    public class Diamond2D : PrimitiveObject2D
    {
        public Diamond2D() : base(
            PrimitiveType2D.Diamond,
            "Diamond",
            PrimitiveMeshFactory2D.CreateDiamond())
        { }
    }

    public class Pentagon2D : PrimitiveObject2D
    {
        public Pentagon2D() : base(
            PrimitiveType2D.Pentagon,
            "Pentagon",
            PrimitiveMeshFactory2D.CreatePolygon(5))
        { }
    }

    public class Hexagon2D : PrimitiveObject2D
    {
        public Hexagon2D() : base(
            PrimitiveType2D.Hexagon,
            "Hexagon",
            PrimitiveMeshFactory2D.CreatePolygon(6))
        { }
    }

    public class Star2D : PrimitiveObject2D
    {
        public Star2D() : base(
            PrimitiveType2D.Star,
            "Star",
            PrimitiveMeshFactory2D.CreateStar())
        { }
    }

    public class Arrow2D : PrimitiveObject2D
    {
        public Arrow2D() : base(
            PrimitiveType2D.Arrow,
            "Arrow",
            PrimitiveMeshFactory2D.CreateArrow())
        { }
    }

    public class Line2D : PrimitiveObject2D
    {
        public Line2D() : base(
            PrimitiveType2D.Line,
            "Line",
            PrimitiveMeshFactory2D.CreateLine())
        { }
    }
}
