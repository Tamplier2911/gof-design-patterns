namespace Singleton
{
    // Singleton: restricts object creation for a class to only one instance.
    //
    // Motivation:  
    // For some components it only makes sense to have single instance in the system. (Repository, Factory)
    // When constructor call is expensive and we want to restrict it to a single call, provide every consumer with same instance.
    // Prevent client from making any addion copies.
    class Main
    {
        public static void Run()
        {
            Console.WriteLine("\nSingleton\n");

            // singleton
            var sdb = SingletonDatabase.Instance;
            const string _city = "Tokyo";
            Console.WriteLine($"City: {_city} | Population {sdb.GetCityPopulation(_city)}");

            // monostate
            var ms1 = new Monostate();
            var ms2 = new Monostate();
            var ms3 = new Monostate();
            ms1.CityName = "London";
            ms1.Population = "8,908,081";
            Console.WriteLine(ms1 + " | " + ms2 + " | " + ms3);

            // per thread singleton
            var t1 = Task.Factory.StartNew(() =>
            {
                var pts1 = PerThreadSingleton.Instance; // one log
                var pts2 = PerThreadSingleton.Instance;
            });
            var t2 = Task.Factory.StartNew(() =>
            {
                var pts1 = PerThreadSingleton.Instance; // second log
                var pts2 = PerThreadSingleton.Instance;
            });
            Task.WaitAll(t1, t2);

            // ambient context
            var b = new Building();
            using (new BuildingContext(3000))
            {
                // 3k
                var w1 = new Wall(new Point(0, 0), new Point(5000, 0));
                b.AddWall(w1);

                using (new BuildingContext(5000))
                {
                    // 5k
                    var w3 = new Wall(new Point(0, 5000), new Point(5000, 5000));
                    var w4 = new Wall(new Point(5000, 0), new Point(5000, 5000));
                    b.AddWall(w3);
                    b.AddWall(w4);
                }

                // 3k
                var w2 = new Wall(new Point(0, 0), new Point(0, 5000));
                b.AddWall(w2);
            }
            Console.WriteLine(b);
        }
    }

    // -- Singleton - ensures instance is only one

    public interface IDatabase
    {
        string GetCityPopulation(string name);
    }

    public class SingletonDatabase : IDatabase
    {
        private Dictionary<string, string> db;
        // db instance created only once in lazy manner
        private static Lazy<SingletonDatabase> instance = new Lazy<SingletonDatabase>(() => new SingletonDatabase());
        // then we just accessing instance
        public static SingletonDatabase Instance => instance.Value;

        // private constructor prevents multiple instance creations
        private SingletonDatabase()
        {
            Console.WriteLine("Initializing Database");
            // mock db data loading
            db = new Dictionary<string, string>() {
                { "Beijing", "21,542,000" },
                { "Tokyo", "13,929,286" },
                { "Kinshasa", "12,691,000" },
                { "Moscow", "12,506,468" },
                { "Jakarta", "10,075,310" },
                { "Seoul", "9,838,892" },
                { "Cairo", "9,848,576" },
                { "London", "8,908,081" },
                { "Tehran", "8,693,706" },
                { "Baghdad", "6,719,500" },
            };
        }

        public string GetCityPopulation(string name)
        {
            return db[name];
        }
    }

    // -- Monostate - creates multiple instances that shares common state

    public class Monostate
    {
        // since fields are static, every new instance will share same state
        private static string cityName, population = cityName = population = "";

        // set/get monostate
        public string CityName
        {
            get => cityName;
            set => cityName = value;
        }

        // set/get monostate
        public string Population
        {
            get => population;
            set => population = value;
        }

        public override string ToString()
        {
            return $"City Name: {this.CityName} | Population: {this.Population}";
        }
    }

    // -- PerThread Singleton - restricts to have more than one instance of object per thread
    public class PerThreadSingleton
    {
        private static ThreadLocal<PerThreadSingleton> threadInstance = new ThreadLocal<PerThreadSingleton>(() => new PerThreadSingleton());
        public static PerThreadSingleton Instance => threadInstance.Value!;
        private PerThreadSingleton()
        {
            Console.WriteLine($"Initializing Per Thread Singleton in thread: {Thread.CurrentThread.ManagedThreadId}");
        }
    }

    // -- Ambient Context

    public sealed class BuildingContext : IDisposable
    {
        private static Stack<BuildingContext> stack = new Stack<BuildingContext>();
        public static BuildingContext Current => stack.Peek();
        public int WallHeight;

        static BuildingContext()
        {
            stack.Push(new BuildingContext(0));
        }

        public BuildingContext(int wallHeight)
        {
            WallHeight = wallHeight;
            stack.Push(this);
        }


        public void Dispose()
        {
            if (stack.Count > 1)
            {
                stack.Pop();
            }
        }
    }

    public class Building
    {
        private List<Wall> Walls = new List<Wall>();

        public void AddWall(Wall w)
        {
            Walls.Add(w);
        }

        public override string ToString()
        {
            var result = "";
            foreach (var w in Walls)
            {
                result += $"Start: {w.Start.x},{w.Start.y} End: {w.End.x},{w.End.y} Height: {w.Height}\n";
            }
            return result;
        }
    }

    public class Wall
    {
        internal Point Start, End;
        internal int Height;

        public Wall(Point start, Point end)
        {
            Start = start;
            End = end;
            Height = BuildingContext.Current.WallHeight;
        }
    }

    public class Point
    {
        internal int x, y;
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}