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

        public abstract string GetName();
        public abstract void AddLine(Line line);

        // Draw - operates on extrinsic state, which can vary (position, color, etc..).
        public abstract void Draw(string color, float x, float y);
    }

    // -- Concreate Flyweights

    // SquareFigure - represents concreate flyweight implementation.
    public class SquareFigure : Figure
    {
        const string _square = "square";
        public SquareFigure() : base(_square)
        {
            AddLine(new Line(0, 0, 5, 0));
            AddLine(new Line(0, 5, 5, 5));
            AddLine(new Line(0, 0, 0, 5));
            AddLine(new Line(5, 0, 5, 5));
        }

        public override string GetName() => name;
        public override void AddLine(Line line) => lines.Add(line);
        public override void Draw(string color, float x, float y)
        {
            Console.WriteLine($"Drawing {color} {name} at position x:{x} y:{y}.");
        }
    }

    // TriangleFigure - represents concreate flyweight implementation.
    public class TriangleFigure : Figure
    {
        const string _triangle = "triangle";
        public TriangleFigure() : base(_triangle)
        {
            AddLine(new Line(0, 0, 0, 5));
            AddLine(new Line(0, 0, 5, 0));
            AddLine(new Line(0, 5, 5, 0));
        }

        public override string GetName() => name;
        public override void AddLine(Line line) => lines.Add(line);
        public override void Draw(string color, float x, float y)
        {
            Console.WriteLine($"Drawing {color} {name} at position x:{x} y:{y}.");
        }
    }

    // CustomFigure - represent concreate implementation of flyweight.
    public class CustomFigure : Figure
    {
        public CustomFigure(string name) : base(name) { }

        public override string GetName() => name;
        public override void AddLine(Line line) => lines.Add(line);
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
            var square = new SquareFigure();
            figures[square.GetName()] = square;

            var triangle = new TriangleFigure();
            figures[triangle.GetName()] = triangle;
        }

        // GetFigure - retrieves figure from state or creates new custom figure.
        public Figure GetFigure(string name)
        {
            // return figure if exists
            if (figures.ContainsKey(name))
            {
                Console.WriteLine($"reused {name} figure");
                return figures[name];
            }

            // create new custom figure
            var fg = new CustomFigure(name);
            figures[fg.GetName()] = fg;
            Console.WriteLine($"created new {name} figure");

            return figures[fg.GetName()];
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
