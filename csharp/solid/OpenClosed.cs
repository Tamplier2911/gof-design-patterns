namespace Solid
{
    // O - Open-Closed (open for extension, closed for modification)
    // 1. classes, modules, functions should be open for extension, but closed for modification
    // 2. such an entity can allow its behaviour to be extended without modifying its source code
    class OpenClosed
    {
        public static void Run()
        {
            // create products
            var candy = new Product("Candy", Color.Red, Size.Small);
            var dress = new Product("Dress", Color.Red, Size.Medium);
            var book = new Product("Book", Color.Green, Size.Medium);

            // create array of products
            Product[] products = { candy, dress, book };

            var pf = new ProductFiler();
            Console.WriteLine("Filter Products (Violating Open-Closed Principle):");
            foreach (var p in pf.FilterByColor(products, Color.Red))
            {
                Console.WriteLine($"Name: {p.Name}, Color: {p.Color}, Size: {p.Size}");
            }
            foreach (var p in pf.FilterBySize(products, Size.Medium))
            {
                Console.WriteLine($"Name: {p.Name}, Color: {p.Color}, Size: {p.Size}");
            }

            var pfe = new ProductFilterEnhanced();
            Console.WriteLine("Filter Products (Utilizing Open-Closed Principle):");
            foreach (var p in pfe.Filter(products, new ColorSpecification(Color.Red)))
            {
                Console.WriteLine($"Name: {p.Name}, Color: {p.Color}, Size: {p.Size}");
            }
            foreach (var p in pfe.Filter(products, new SizeSpecification(Size.Medium)))
            {
                Console.WriteLine($"Name: {p.Name}, Color: {p.Color}, Size: {p.Size}");
            }
            foreach (var p in pfe.Filter(
                products,
                new AndSpecification<Product>(
                    new ColorSpecification(Color.Red),
                    new SizeSpecification(Size.Small))
                ))
            {
                Console.WriteLine($"Name: {p.Name}, Color: {p.Color}, Size: {p.Size}");
            }
        }
    }

    // Color enum - represents available colors.
    public enum Color
    {
        Red, Green, Blue
    }

    // Size enum - represents available sizes.
    public enum Size
    {
        Small, Medium, Large
    }

    // Product - represents product model.
    public class Product
    {
        public string Name;
        public Color Color;
        public Size Size;

        public Product(string name, Color color, Size size)
        {
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            Color = color;
            Size = size;
        }
    }

    // ProductFilter - concerned about filtering products by certain criteria.
    public class ProductFiler
    {
        // Coming back to modify filter class every time business rules change would violate Open-Closed principle.

        // Filter products by size.
        public IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
        {
            foreach (var p in products)
            {
                if (p.Size == size)
                {
                    yield return p;
                }
            }
        }

        // Filter products by color.
        public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color)
        {
            foreach (var p in products)
            {
                if (p.Color == color)
                {
                    yield return p;
                }
            }
        }

        // Filter products by size and color...
    }

    // Specification pattern to the resque.

    // ISpecification - defines specification interface.
    public interface ISpecification<T>
    {
        bool IsSatisfied(T t);
    }

    // IFilter - defines filter interface.
    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }

    // ColorSpecification - implements specification interface.
    public class ColorSpecification : ISpecification<Product>
    {
        private Color color;
        public ColorSpecification(Color color)
        {
            this.color = color;
        }
        public bool IsSatisfied(Product p)
        {
            return color == p.Color;
        }
    }

    // SizeSpecification - implements specification interface.
    public class SizeSpecification : ISpecification<Product>
    {
        private Size size;
        public SizeSpecification(Size size)
        {
            this.size = size;
        }
        public bool IsSatisfied(Product p)
        {
            return size == p.Size;
        }
    }

    // AndSpecification - implements specification interface as well implements combinator of specifications.
    public class AndSpecification<T> : ISpecification<T>
    {
        private ISpecification<T> first, second;

        public AndSpecification(ISpecification<T> first, ISpecification<T> second)
        {
            this.first = first ?? throw new ArgumentNullException(paramName: nameof(first));
            this.second = second ?? throw new ArgumentNullException(paramName: nameof(second));
        }

        public bool IsSatisfied(T t)
        {
            return first!.IsSatisfied(t) && second!.IsSatisfied(t);
        }
    }


    // ProductFilterEnhanced - concerned about filtering products by certain criteria utilizing open-closed principle.
    public class ProductFilterEnhanced : IFilter<Product>
    {
        public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
        {
            foreach (var i in items)
            {
                if (spec.IsSatisfied(i))
                {
                    yield return i;
                }
            }
        }
    }
}