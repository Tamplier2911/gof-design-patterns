namespace Bridge
{
    // Bridge: decouples an abstraction from its implementation so that the two can vary independently.
    //
    // Motivation:
    // Prevents a 'cartesian product' complexity explosion AxBxC scenario.
    // shape: Circle, Square ...
    // type:  Raster, Vector ...
    // types: shape * type = RasterCircle, VectorCircle, RasterSquare, VectorSquare
    class Main
    {
        public static void Run()
        {
            Console.WriteLine("\nBridge\n");

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

    // Shape - represents abstraction.
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

    // IRenderer - represents implementor.
    public interface IRenderer
    {
        public void RenderCircle(float radius);
        public void RenderSquare(int side);
    }

    // -- Refined Abstraction

    // Circle - represents refined abstraction, inherits from abstraction.
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

    // Square - represents refined abstraction, inherits from abstraction.
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

    // RasterRenderer - represents concrete implementor, inherits from implementor.
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

    // VectorRenderer - represents concrete implementor, inherits from implementor.
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