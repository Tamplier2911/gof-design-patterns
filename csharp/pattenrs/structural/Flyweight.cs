namespace Flyweight
{
    // Flyweight: reduces the cost of creating and manipulating a large number of similar objects.
    //
    // Motivation:
    // avoid redundancy when storing data (app using large amount of similar objects which turns into significant memory allocation)
    // inner mutable state can be extracted from the class, which helps to replate inner state with a group of small shared objects
    //
    class Main
    {
        public static void Run()
        {
            Console.WriteLine("\nFlyweight\n");

            // extrinsic state
            const string _blueColor = "blue";
            const string _redColor = "red";
            float x = 0;
            float y = 0;

            // init flyweight factory
            var ff = new FigureFactory();

            // get figures
            var sq1 = ff.GetFigure("square");
            var sq2 = ff.GetFigure("square");

            var tr1 = ff.GetFigure("triangle");
            var tr2 = ff.GetFigure("triangle");

            var pt1 = ff.GetFigure("portrait");
            var pt2 = ff.GetFigure("portrait");

            var rb1 = ff.GetFigure("rainbow");
            var rb2 = ff.GetFigure("rainbow");

            // perform actions (operations) passing extrinsic state
            sq1.Draw(_blueColor, ++x, ++y);
            sq2.Draw(_redColor, ++x, ++y);

            tr1.Draw(_blueColor, ++x, ++y);
            tr2.Draw(_redColor, ++x, ++y);

            pt1.Draw(_blueColor, ++x, ++y);
            pt2.Draw(_redColor, ++x, ++y);

            rb1.Draw(_blueColor, ++x, ++y);
            rb2.Draw(_redColor, ++x, ++y);

            // see all unique figures which being reused
            Console.WriteLine(ff);
        }
    }

    // -- Flyweight Abstraction

    // Figure - represents flyweight abstraction.
    public abstract class Figure
    {
        // lines and name - represents intrinsic state, which are specific for each figure.
        protected string name;
        protected List<Line> lines = new List<Line>();
        public Figure(string name) { this.name = name; }

        // Draw - operates on extrinsic state, which can vary (position, color, etc..).
        public abstract void Draw(string color, float x, float y);
        public abstract void AddLine(Line line);
    }

    // -- Concreate Flyweights

    // SquareFigure - represents concreate implementation of flyweight.
    public class SquareFigure : Figure
    {
        public SquareFigure(string name) : base(name) { }

        public override void AddLine(Line line)
        {
            lines.Add(line);
        }

        public override void Draw(string color, float x, float y)
        {
            Console.WriteLine($"Drawing {color} {name} at position x:{x} y:{y}.");
        }
    }

    // TriangleFigure - represents concreate implementation of flyweight.
    public class TriangleFigure : Figure
    {
        public TriangleFigure(string name) : base(name) { }

        public override void AddLine(Line line)
        {
            lines.Add(line);
        }

        public override void Draw(string color, float x, float y)
        {
            Console.WriteLine($"Drawing {color} {name} at position x:{x} y:{y}.");
        }
    }

    // CustomFigure - represent concreate implementation of flyweight.
    public class CustomFigure : Figure
    {
        public CustomFigure(string name) : base(name) { }

        public override void AddLine(Line line)
        {
            lines.Add(line);
        }

        public override void Draw(string color, float x, float y)
        {
            Console.WriteLine($"Drawing {color} {name} at position x:{x} y:{y}.");
        }
    }

    // -- Flyweight Factory

    // FigureFactory - represents flyweight factory.
    public class FigureFactory
    {
        private Dictionary<string, Figure> figures = new Dictionary<string, Figure>();

        // Constructor - initializes default factory state.
        public FigureFactory()
        {
            var square = new SquareFigure("square");
            square.AddLine(new Line(0, 0, 5, 0));
            square.AddLine(new Line(0, 5, 5, 5));
            square.AddLine(new Line(0, 0, 0, 5));
            square.AddLine(new Line(5, 0, 5, 5));
            figures["square"] = square;

            var triangle = new TriangleFigure("triangle");
            triangle.AddLine(new Line(0, 0, 0, 5));
            triangle.AddLine(new Line(0, 0, 5, 0));
            triangle.AddLine(new Line(0, 5, 5, 0));
            figures["triangle"] = triangle;
        }

        // GetFigure - retrieves figure from state if exists, else creates new custom figure.
        public Figure GetFigure(string figure)
        {
            // return figure if exists
            if (figures.ContainsKey(figure))
            {
                Console.WriteLine($"reused {figure} figure");
                return figures[figure];
            }

            // create new custom figure
            var fg = new CustomFigure(figure);
            figures[figure] = fg;
            Console.WriteLine($"created new {figure} figure");

            return figures[figure];
        }

        public override string ToString()
        {
            return string.Join("\n", figures.Keys);
        }
    }

    // -- Auxiliary Classes

    // Line - represents part of figure.
    public class Line
    {
        private float x1, y1, x2, y2;
        public Line(float x1, float y1, float x2, float y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }
    }
}