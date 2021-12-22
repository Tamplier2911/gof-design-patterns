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
    // - can be hierarchy of factories (Abstract Factory)
    class Factories
    {
        public static void Run()
        {
            Console.WriteLine("Factories");

            // factory method
            var p1 = PointFactoryMethod.NewCartisianPoint(5.0, 5.0);
            var p2 = PointFactoryMethod.NewPolarPoint(1.0, Math.PI / 2);

            // factory
            var p3 = PointFactory.NewCartisianPoint(5.0, 5.0);
            var p4 = PointFactory.NewPolarPoint(1.0, Math.PI / 2);

            // inner factory
            var p5 = PointInnerFactory.Factory.NewCartisianPoint(5.0, 5.0);
            var p6 = PointInnerFactory.Factory.NewPolarPoint(1.0, Math.PI / 2);

            // abstract factory
            var hdm = new HotDrinkMachine();
            var coffee = hdm.MakeDrink(HotDrinkMachine.AvailableDrink.Coffee, "cappuccino");
            var tea = hdm.MakeDrink(HotDrinkMachine.AvailableDrink.Tea, "green tea");
            coffee.Consume();
            tea.Consume();

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

    // PointFactoryMethod - represents point (using factory method).
    class PointFactoryMethod
    {
        private double x, y;

        // constructors usually 'private' with factory methods
        private PointFactoryMethod(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        // - names of factory methods are unique
        // - same argument types, different discripting names

        // NewCartisianPoint - is a factory method, which is a wrapper around constructor.
        public static PointFactoryMethod NewCartisianPoint(double x, double y)
        {
            return new PointFactoryMethod(x, y);
        }

        // NewPolarPoint - is factory method, which is a wrapper around constructor.
        public static PointFactoryMethod NewPolarPoint(double rho, double theta)
        {
            return new PointFactoryMethod(rho * Math.Cos(theta), rho * Math.Sin(theta));
        }
    }

    // 

    // Factory

    // Motivation: separation of concerns (object construction and object behaviour)

    // 

    // Point - represents point.
    class Point
    {
        private double x, y;

        internal Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }

    // PointFactory - represents point construction factory.
    class PointFactory
    {
        // NewCartisianPoint - is a factory method, which is a wrapper around constructor.
        public static Point NewCartisianPoint(double x, double y)
        {
            return new Point(x, y);
        }

        // NewPolarPoint - is factory method, which is a wrapper around constructor.
        public static Point NewPolarPoint(double rho, double theta)
        {
            return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
        }
    }

    //

    // Inner Factory

    // Motivation: if class constraint should be private, it may contain inner factory

    //

    // PointInnerFactory - represents point with Inner Factory.
    class PointInnerFactory
    {
        private double x, y;

        private PointInnerFactory(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        // Factory - implements point Inner Factory.
        public static class Factory
        {
            public static PointInnerFactory NewCartisianPoint(double x, double y)
            {
                return new PointInnerFactory(x, y);
            }

            public static PointInnerFactory NewPolarPoint(double rho, double theta)
            {
                return new PointInnerFactory(rho * Math.Cos(theta), rho * Math.Sin(theta));
            }
        }
    }

    // 

    // Abstract Factory - returns abstract classes or interfaces

    // Motivation: group object factories that have a common theme.

    //

    // IHotDrink - represents hot drink.
    public interface IHotDrink
    {
        void Consume();
    }

    // Tea - represents tea.
    internal class Tea : IHotDrink
    {
        private string Kind;
        internal Tea(string kind) { Kind = kind; }
        public void Consume() { Console.WriteLine($"consuming drink: {Kind}"); }
    }

    // Coffee - represents coffee.
    internal class Coffee : IHotDrink
    {
        private string Kind;
        internal Coffee(string kind) { Kind = kind; }
        public void Consume() { Console.WriteLine($"consuming drink: {Kind}"); }
    }

    // IHotDrinkFactory - represents hot drink abstract factory.
    interface IHotDrinkFactory
    {
        IHotDrink Prepare(string kind);
    }

    // TeaFactory - represent tea factory.
    internal class TeaFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(string kind)
        {
            return new Tea(kind);
        }
    }

    // CoffeeFactory - represent coffee factory.
    internal class CoffeeFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(string kind)
        {
            return new Coffee(kind);
        }
    }

    // HotDrinkMachine - hot drink abstract factory.
    public class HotDrinkMachine
    {
        public enum AvailableDrink { Coffee, Tea }
        private Dictionary<AvailableDrink, IHotDrinkFactory> factories = new();

        public HotDrinkMachine()
        {
            foreach (AvailableDrink drink in Enum.GetValues(typeof(AvailableDrink)))
            {
                var factory = (IHotDrinkFactory)Activator.CreateInstance(
                    Type.GetType("Patterns." + Enum.GetName(typeof(AvailableDrink), drink) + "Factory")!
                )!;
                factories.Add(drink, factory);
            }
        }

        public IHotDrink MakeDrink(AvailableDrink drink, string kind)
        {
            return factories[drink].Prepare(kind);
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