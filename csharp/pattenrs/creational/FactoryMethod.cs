using System.Threading.Tasks;
using System.Text;

namespace Patterns
{
    // Factories: are components responsible solely for the wholesale (not piecewise) creation of objects.
    //
    // Motivation:
    // Object creation logic becomes too convoluted.
    // Constructor is not descriptive:
    // - name mandated by name of constructing type
    // - cannot overload with same sets of arguments with different names
    // - can turn into 'optional parameter hell'
    // Object creation (non-piecewise, unlike Builder) can be outsourced to:
    // - separate function (Factory Method)
    // - separate class (Factory)
    // - ca be hierarchy of factories (Abstract Factory)
    class FactoryMethod
    {
        public static void Run()
        {
            Console.WriteLine("Factory Method");

            // regular way
            var p0 = new Point(5, 5, CoordinateSystemType.Cortesian);

            // using factory method
            var p1 = PointFM.NewCartisianPoint(5.0, 5.0);
            var p2 = PointFM.NewPolarPoint(1.0, Math.PI / 2);

            // using async factory method
            // var n = await Note.CreateAsync();
            // Note.CreateAsync().Wait();

            // using factory
            var p3 = PointFactory.NewCartisianPoint(5.0, 5.0);
            var p4 = PointFactory.NewPolarPoint(1.0, Math.PI / 2);

            // object tracking
            var ttf = new TrackingThemeFactory();
            var t0 = ttf.CreateTheme(ThemeColor.LightTheme);
            var t1 = ttf.CreateTheme(ThemeColor.DarkTheme);
            Console.WriteLine(ttf.Info());                    // list all created objects

            // bulk replacement
            var rtf = new ReplaceableThemeFactory();
            var t2 = rtf.CreateTheme(ThemeColor.LightTheme);
            var t3 = rtf.CreateTheme(ThemeColor.DarkTheme);
            Console.WriteLine(rtf.Info());                    // list all created objects
            rtf.ReplaceThemes(ThemeColor.LightTheme);         // replaces all themes with light theme
            Console.WriteLine(rtf.Info());                    // list all created objects
        }
    }

    // CoordinateSystemType - represents coordinate system type enum.
    public enum CoordinateSystemType
    {
        Cortesian,
        Polar
    }

    // Point - represents point of coordinate (regular way).
    class Point
    {
        private double x, y;

        public Point(double x, double y, CoordinateSystemType s = CoordinateSystemType.Cortesian)
        {
            switch (s)
            {
                case CoordinateSystemType.Cortesian:
                    this.x = x; // rho
                    this.y = y; // theta
                    break;
                case CoordinateSystemType.Polar:
                    x = x * Math.Cos(y);
                    y = x * Math.Sin(y);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(s), s, null);
            }
        }
    }

    // 

    // Factory Method

    //

    // PointFM - represents point of coordinate (using factory method).
    class PointFM
    {
        private double x, y;

        // constructor is now private
        private PointFM(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        // - names of factory methods are unique
        // - same argument types, different discripting names

        // NewCartisianPoint - is a factory method, which is a wrapper around constructor.
        public static PointFM NewCartisianPoint(double x, double y)
        {
            return new PointFM(x, y);
        }

        // NewPolarPoint - is factory method, which is a wrapper around constructor.
        public static PointFM NewPolarPoint(double rho, double theta)
        {
            return new PointFM(rho * Math.Cos(theta), rho * Math.Sin(theta));
        }
    }

    // 

    // Asynchronous Factory Method

    // Motivation: cannot perform asynchronous action inside of the constructor.

    //  

    // Note - represents random note.
    class Note
    {
        private Note()
        {
            // cannot perform async actions in constructor
            // but async action is required for initialization
        }

        // InitAsync - getting task description asynchronously.
        private async Task<Note> InitAsync()
        {
            // perform async action
            await Task.Delay(1000);
            return this;
        }

        // CreateAsync - is async factory method, a wrapper around constructor which performs async call.
        public static Task<Note> CreateAsync()
        {
            // create instance of note
            var n = new Note();

            // perform task creation
            return n.InitAsync();
        }
    }

    // 

    // Factory

    // Motivation: separation of concerns (object construction and object behaviour)

    // 

    // PointFactory - represents point of coordinate construction factory.
    class PointFactory
    {
        // NewCartisianPoint - is a factory method, which is a wrapper around constructor.
        public static Point NewCartisianPoint(double x, double y)
        {
            return new Point(x, y, CoordinateSystemType.Cortesian);
        }

        // NewPolarPoint - is factory method, which is a wrapper around constructor.
        public static Point NewPolarPoint(double rho, double theta)
        {
            return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta), CoordinateSystemType.Polar);
        }
    }

    // 

    // Factory: Object Tracking & Bulk Replacement

    // 

    // ITheme - represents theme interface.
    interface ITheme
    {
        string TextColor { get; }
        string BGColor { get; }
    }

    // LightTheme - represents light theme.
    class LightTheme : ITheme
    {
        public string TextColor => "#222";
        public string BGColor => "#fff";
    }

    // DarkTheme - represents dark theme.
    class DarkTheme : ITheme
    {
        public string TextColor => "#ffe";
        public string BGColor => "#444";
    }

    // ThemeColor - represets possible theme colors.
    enum ThemeColor
    {
        LightTheme, DarkTheme
    }

    // TrackThemeFactory - saving a weak reference to objects created inside a factory in order to track them.
    class TrackingThemeFactory
    {
        private readonly List<WeakReference<ITheme>> refs = new();

        public ITheme CreateTheme(ThemeColor tm)
        {
            switch (tm)
            {
                case ThemeColor.DarkTheme:
                    var dt = new DarkTheme();
                    refs.Add(new WeakReference<ITheme>(dt)); // new(dt)
                    return dt;
                case ThemeColor.LightTheme:
                    var lt = new LightTheme();
                    refs.Add(new WeakReference<ITheme>(lt)); // new(lt)
                    return lt;
                default:
                    throw new ArgumentException(nameof(tm));
            }
        }

        public string Info()
        {
            var sb = new StringBuilder();

            foreach (var r in refs)
            {
                if (r.TryGetTarget(out var theme))
                {
                    var origin = theme is DarkTheme ? "dark" : theme is LightTheme ? "light" : "unknown";
                    sb.Append($"Origin: {origin} | Text: {theme.TextColor} | BG: {theme.BGColor}\n");
                }
            }

            return sb.ToString();
        }
    }


    // Ref - wrapper which allow bulk replacements.
    class Ref<T> where T : class
    {
        public T Value;
        public Ref(T value)
        {
            Value = value;
        }
    }

    // ReplaceableThemeFactory - saving a weak reference to objects created inside a factory in order to track them,
    // wraps objects in a Ref in order to have ability for bulk replacement.
    class ReplaceableThemeFactory
    {
        private readonly List<WeakReference<Ref<ITheme>>> refs = new();

        private ITheme CreateITheme(ThemeColor tm)
        {
            switch (tm)
            {
                case ThemeColor.DarkTheme:
                    return new DarkTheme();
                case ThemeColor.LightTheme:
                    return new LightTheme();
                default:
                    throw new ArgumentException(nameof(tm));
            }
        }

        public Ref<ITheme> CreateTheme(ThemeColor tm)
        {
            var r = new Ref<ITheme>(CreateITheme(tm));
            refs.Add(new WeakReference<Ref<ITheme>>(r)); // new(r)
            return r;
        }

        public void ReplaceThemes(ThemeColor tm)
        {
            foreach (var r in refs)
            {
                if (r.TryGetTarget(out var reference))
                {
                    reference.Value = CreateITheme(tm);
                }
            }
        }

        public string Info()
        {
            var sb = new StringBuilder();

            foreach (var r in refs)
            {
                if (r.TryGetTarget(out var reference))
                {
                    var origin = reference.Value is DarkTheme ? "dark" : reference.Value is LightTheme ? "light" : "unknown";
                    sb.Append($"Origin: {origin} | Text: {reference.Value.TextColor} | BG: {reference.Value.BGColor}\n");
                }
            }

            return sb.ToString();
        }
    }
}