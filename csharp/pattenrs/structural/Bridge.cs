namespace Bridge
{
    ///
    /// <summary>
    /// Class <c>Main</c> represents Bridge pattern usecase.
    /// Bridge: decouples an abstraction from its implementation so that the two can vary independently.
    /// Motivation:
    /// Prevents a 'cartesian product' complexity explosion AxBxC scenario.
    /// shape: Circle, Square ...
    /// type:  Raster, Vector ...
    /// types: shape * type = RasterCircle, VectorCircle, RasterSquare, VectorSquare
    /// </summary>
    ///
    class Main
    {
        public static void Run()
        {
            Console.WriteLine("\nBridge");

            // initialize concreate implementors
            var rrn = new RasterRenderer();
            var vrn = new VectorRenderer();

            // initialize refined abstractions
            var c = new Circle(rrn, 5.0f);
            var s = new Square(vrn, 5);

            // run refined abstraction method that utilize concreate implementors logic
            c.Render();
            s.Render();
        }
    }

    // -- Abstraction

    /// <summary>Class <c>Shape</c> represents abstraction.</summary>
    public abstract class Shape
    {
        protected IRenderer renderer;

        // constructor requires implementor
        public Shape(IRenderer renderer)
        {
            this.renderer = renderer;
        }

        public abstract void Render();
    }

    // /\
    //
    // Bridge
    //
    // \/

    // -- Implementor

    /// <summary>Interface <c>IRenderer</c> describes implementor.</summary>
    public interface IRenderer
    {
        public void RenderCircle(float radius);
        public void RenderSquare(int side);
    }

    // -- Refined Abstraction

    /// <summary>Class <c>Circle</c> represents refined abstraction, inherits from abstraction.</summary>
    public class Circle : Shape
    {
        private float radius;

        public Circle(IRenderer renderer, float radius) : base(renderer)
        {
            this.radius = radius;
        }

        public override void Render()
        {
            // utilizes implementor
            renderer.RenderCircle(radius);
        }
    }

    /// <summary>Class <c>Square</c> represents refined abstraction, inherits from abstraction.</summary>
    public class Square : Shape
    {
        private int side;

        public Square(IRenderer renderer, int side) : base(renderer)
        {
            this.side = side;
        }

        public override void Render()
        {
            // utilizes implementor
            renderer.RenderSquare(side);
        }
    }

    // -- Concreate Implementor

    /// <summary>Class <c>RasterRenderer</c> represents concrete implementor, inherits from implementor.</summary>
    public class RasterRenderer : IRenderer
    {
        public RasterRenderer() { }

        public void RenderCircle(float radius)
        {
            Console.WriteLine($"rendering circle as raster: {radius}");
        }

        public void RenderSquare(int side)
        {
            Console.WriteLine($"rendering square as raster: {side}");
        }
    }

    /// <summary>Class <c>VectorRenderer</c> represents concrete implementor, inherits from implementor.</summary>
    public class VectorRenderer : IRenderer
    {
        public VectorRenderer() { }

        public void RenderCircle(float radius)
        {
            Console.WriteLine($"rendering circle as vector: {radius}");
        }

        public void RenderSquare(int side)
        {
            Console.WriteLine($"rendering square as vector: {side}");
        }
    }
}