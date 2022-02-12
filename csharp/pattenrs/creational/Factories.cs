using System.Threading.Tasks;
using System.Text;

namespace Factories
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
    // - can be hierarchy of factories (Abstract Factory)
    class Main
    {
        public static void Run()
        {
            Console.WriteLine("\nFactories");

            // factory method
            var p1 = PointOne.NewCartesianPoint(5.0, 5.0);
            var p2 = PointOne.NewPolarPoint(1.0, Math.PI / 2);

            // factory
            var p3 = PointTwoFactory.NewCartesianPoint(5.0, 5.0);
            var p4 = PointTwoFactory.NewPolarPoint(1.0, Math.PI / 2);

            // inner factory
            var p5 = PointThree.Factory.NewCartesianPoint(5.0, 5.0);
            var p6 = PointThree.Factory.NewPolarPoint(1.0, Math.PI / 2);

            // abstract factory
            var hdm = new HotDrinkMachine();
            // var drink = hdm.MakeDrink();
            // drink.Consume();

            // async factory method
            // var n = await Note.CreateAsync();
            // Note.CreateAsync().Wait();

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

    // 

    // Factory Method

    //

    class PointOne
    {
        private double x, y;

        // constructors usually 'private' with factory methods
        private PointOne(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        // - names of factory methods are unique
        // - same argument types, different descriptive names

        // NewCartesianPoint - is a factory method, which is a wrapper around constructor.
        public static PointOne NewCartesianPoint(double x, double y)
        {
            return new PointOne(x, y);
        }

        // NewPolarPoint - is factory method, which is a wrapper around constructor.
        public static PointOne NewPolarPoint(double rho, double theta)
        {
            return new PointOne(rho * Math.Cos(theta), rho * Math.Sin(theta));
        }
    }

    // 

    // Factory - separation of concerns (object construction and object behaviour)

    // 

    class PointTwo
    {
        private double x, y;

        internal PointTwo(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class PointTwoFactory
    {
        public static PointTwo NewCartesianPoint(double x, double y)
        {
            return new PointTwo(x, y);
        }

        public static PointTwo NewPolarPoint(double rho, double theta)
        {
            return new PointTwo(rho * Math.Cos(theta), rho * Math.Sin(theta));
        }
    }

    //

    // Inner Factory - if class constraint should be private, it may contain inner factory

    //

    class PointThree
    {
        private double x, y;

        private PointThree(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        // Factory - implements point Inner Factory.
        public static class Factory
        {
            public static PointThree NewCartesianPoint(double x, double y)
            {
                return new PointThree(x, y);
            }

            public static PointThree NewPolarPoint(double rho, double theta)
            {
                return new PointThree(rho * Math.Cos(theta), rho * Math.Sin(theta));
            }
        }
    }

    // 

    // Abstract Factory - returns abstract classes or interfaces, group object factories that have a common theme.

    //

    public interface IHotDrink
    {
        void Consume();
    }

    internal class Tea : IHotDrink
    {
        private string Kind;
        internal Tea(string kind) { Kind = kind; }
        public void Consume() { Console.WriteLine($"consuming drink: {Kind}"); }
    }

    internal class Coffee : IHotDrink
    {
        private string Kind;
        internal Coffee(string kind) { Kind = kind; }
        public void Consume() { Console.WriteLine($"consuming drink: {Kind}"); }
    }

    interface IHotDrinkFactory
    {
        IHotDrink Prepare(string kind);
    }

    internal class TeaFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(string kind)
        {
            return new Tea(kind);
        }
    }

    internal class CoffeeFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(string kind)
        {
            return new Coffee(kind);
        }
    }

    public class HotDrinkMachine
    {
        private List<(string, IHotDrinkFactory)> factories = new();

        public HotDrinkMachine()
        {
            foreach (var type in typeof(HotDrinkMachine).Assembly.GetTypes())
            {
                // if type is assignable and not interface by it self
                if (typeof(IHotDrinkFactory).IsAssignableFrom(type) && !type.IsInterface)
                {
                    factories.Add((type.Name.Replace("Factory", string.Empty), (IHotDrinkFactory)Activator.CreateInstance(type)!));
                }
            }
        }

        public IHotDrink MakeDrink()
        {
            Console.WriteLine("Available Drinks");
            for (int i = 0; i < factories.Count; i++)
            {
                Console.WriteLine($"{i}: {factories[i].Item1}");
            }

            while (true)
            {
                string s;
                // input is not null, integer, greater than 0 and less than max available drinks
                if ((s = Console.ReadLine()!) != null && int.TryParse(s, out int i) && i >= 0 && i < factories.Count)
                {
                    return factories[i].Item2.Prepare(factories[i].Item1);
                }
                Console.WriteLine("Incorrect input, please try again!");
            }
        }
    }

    // 

    // Asynchronous Factory Method - cannot perform asynchronous action inside of the constructor.

    //  

    class Note
    {
        private Note()
        {
            // cannot perform async actions in constructor
            // but async action is required for initialization
        }

        private async Task<Note> InitAsync()
        {
            // perform async action
            await Task.Delay(1000);
            return this;
        }

        public static Task<Note> CreateAsync()
        {
            // create instance of note
            var n = new Note();

            // perform task creation
            return n.InitAsync();
        }
    }

    // 

    // Factory: Object Tracking & Bulk Replacement

    // 

    interface ITheme
    {
        string TextColor { get; }
        string BGColor { get; }
    }

    class LightTheme : ITheme
    {
        public string TextColor => "#222";
        public string BGColor => "#fff";
    }

    class DarkTheme : ITheme
    {
        public string TextColor => "#ffe";
        public string BGColor => "#444";
    }

    enum ThemeColor
    {
        LightTheme, DarkTheme
    }

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

    class Ref<T> where T : class
    {
        public T Value;
        public Ref(T value)
        {
            Value = value;
        }
    }

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