namespace Adapter
{
    // Adapter: allows classes with incompatible interfaces to work together by wrapping its own interface around that of an already existing class.
    //
    // Motivation:
    // Every type cannot conform every possible interface.
    // Adapter is a construct which dapats an existing interface X to conform to the required interface Y.
    class Main
    {
        public static void Run()
        {
            Console.WriteLine("\nAdapter\n");

            // initialize client
            var pr = new ImagePrinter();

            // initialize target
            var rm = new RasterImage();
            rm.AddPoint(new Point(2, 1));
            rm.AddPoint(new Point(7, 1));
            rm.AddPoint(new Point(4, 2));
            rm.AddPoint(new Point(5, 2));
            rm.AddPoint(new Point(4, 3));
            rm.AddPoint(new Point(5, 3));

            // use target by client
            pr.PrintImage(rm);

            // initialize adaptee
            var vm = new VectorImage();
            vm.AddLine(new Line(0, 0, 10, 0));
            vm.AddLine(new Line(0, 0, 0, 10));
            vm.AddLine(new Line(0, 10, 10, 10));
            vm.AddLine(new Line(10, 0, 10, 10));

            // init cache
            var pc = new PointsCache();

            // initialize adapter
            var via = new VectorToRasterAdapter(vm, pc);
            var viaCached1 = new VectorToRasterAdapter(vm, pc);
            var viaCached2 = new VectorToRasterAdapter(vm, pc);

            // use adaptee by client
            pr.PrintImage(via);
        }
    }

    // -- Adapter

    public class VectorToRasterAdapter : IPrintable
    {
        private IPointsCache Cache;
        private List<Point> Points = new List<Point>();

        public VectorToRasterAdapter(VectorImage vm, IPointsCache pc)
        {
            Cache = pc;
            foreach (var line in vm.GetLines())
            {
                this.AddLine(line);
            }
        }

        public List<Point> GetPoints()
        {
            return Points;
        }

        private void AddLine(Line l)
        {
            var sum = Cache.GetSum(l);

            // try to get points from cache
            if (Cache.Found(sum))
            {
                Console.WriteLine("got points from cache");
                Points = Cache.Retrieve(sum);
                return;
            }

            Console.WriteLine("converting lines to points");

            // if both x and y grows, then line is diagonal
            if (l.x1 != l.x2 && l.y1 != l.y2)
            {
                for (int i = l.x1, j = l.y1; i < l.x2 && j < l.y2; i++, j++)
                {
                    Points.Add(new Point(i, j));
                }

                for (int i = l.x2, j = l.y2; i > l.x1 && j < l.y1; i--, j--)
                {
                    Points.Add(new Point(i, j));
                }
            }

            // if y doesn't change, then line is - horizontal
            if (l.y1 == l.y2)
            {
                for (int i = l.x1; i <= l.x2; i++)
                {
                    Points.Add(new Point(i, l.y1));
                }
            }

            // if x doesen't change, then line is - vertical
            if (l.x1 == l.x2)
            {
                for (int i = l.y1; i <= l.y2; i++)
                {
                    Points.Add(new Point(l.x1, i));
                }
            }

            // store points to cache
            Cache.Store(sum, Points);
        }
    }

    // -- Adaptee

    // VectorImage - represents vector image, does not implements IPrintable.
    public class VectorImage
    {
        private List<Line> Lines = new List<Line>();

        public void AddLine(Line l)
        {
            Lines.Add(l);
        }

        public List<Line> GetLines()
        {
            return Lines;
        }
    }

    // -- Target

    // RasterImage - represents raster image, implements IPrintable interface.
    public class RasterImage : IPrintable
    {
        private List<Point> Points = new List<Point>();

        public void AddPoint(Point p)
        {
            Points.Add(p);
        }

        public List<Point> GetPoints()
        {
            return Points;
        }
    }

    // -- Client

    // IPrintable - describes interface required to print image.
    public interface IPrintable
    {
        public List<Point> GetPoints();
    }

    // ImagePrinter - used to print images.
    public class ImagePrinter
    {
        public void PrintImage(IPrintable rm)
        {
            // get image points
            var points = rm.GetPoints();

            // get max height and max width of the image
            int maxH = 0; // y
            int maxW = 0; // x
            foreach (var p in points)
            {
                if (p.y > maxH)
                {
                    maxH = p.y;
                }

                if (p.x > maxW)
                {
                    maxW = p.x;
                }
            }

            // create a matrix based on max height and max width values
            string[,] mx = new string[maxH + 1, maxW + 1];
            for (int y = 0; y < mx.GetLength(0); y++)
            {
                for (int x = 0; x < mx.GetLength(1); x++)
                {
                    // fill matrix with whitespaces
                    mx[y, x] = " ";
                }
            }

            // range over each point
            foreach (var p in points)
            {
                // represent point on matrix
                mx[p.y, p.x] = ".";
            }

            var result = "";
            for (int y = 0; y < mx.GetLength(0); y++)
            {
                for (int x = 0; x < mx.GetLength(1); x++)
                {
                    result += mx[y, x];
                    if (x == mx.GetLength(1) - 1)
                    {
                        result += "\n";
                    }
                }
            }

            Console.WriteLine(result);
        }
    }

    // -- Cache

    // IPointsCache - represents points cache interface.
    public interface IPointsCache
    {
        public int GetSum(object o);
        public bool Found(int sum);
        public List<Point> Retrieve(int sum);
        public void Store(int sum, List<Point> points);
    }

    // PointsCache - represents points cache.
    public class PointsCache : IPointsCache
    {
        private Dictionary<int, List<Point>> Cache = new Dictionary<int, List<Point>>();

        public PointsCache() { }

        public int GetSum(object o)
        {
            var p = (Line)o;
            return ((p.x1 * 397) ^ p.y1) + ((p.x2 * 397) ^ p.y2);
        }

        public bool Found(int sum)
        {
            List<Point>? value;
            return Cache.TryGetValue(sum, out value);
        }

        public List<Point> Retrieve(int sum)
        {
            return Cache[sum];
        }

        public void Store(int sum, List<Point> points)
        {
            Cache[sum] = points;
        }
    }

    // -- Auxillary classes

    // Point - represents point.
    public class Point
    {
        public int x, y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    // Line - represents line.
    public class Line
    {
        public int x1, y1, x2, y2 = 0;

        public Line(int x1, int y1, int x2, int y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }
    }
}
