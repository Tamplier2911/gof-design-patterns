namespace Solid
{
    ///
    /// <summary>
    /// Class <c>OpenClosed</c> represents Open-Closed principle usecase.
    /// 1. classes, modules, functions should be open for extension, but closed for modification
    /// 2. such an entity can allow its behaviour to be extended without modifying its source code
    /// </summary>
    ///
    class OpenClosed
    {
        public static void Run()
        {
            Console.WriteLine("\nOpen Closed\n");

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

    /// <summary>Class <c>Color</c> represents available colors.</summary>
    public enum Color
    {
        Red, Green, Blue
    }

    /// <summary>Class <c>Size</c>  represents available sizes</summary>
    public enum Size
    {
        Small, Medium, Large
    }

    /// <summary>Class <c>Product</c> represents product entity.</summary>
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

    /// <summary>Class <c>ProductFilter</c> concerned about filtering products by certain criteria.</summary>
    public class ProductFiler
    {
        // Coming back to modify filter class every time business rules change would violate Open-Closed principle.

        /// <summary>Method <c>FilterBySize</c> filter products by size.</summary>
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

        /// <summary>Method <c>FilterByColor</c> filter products by color.</summary>
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

        /// <summary>Method <c>FilterByColor</c> filter products by size and color...</summary>
    }

    /// <summary>Interface <c>ISpecification</c> defines specification interface.</summary>
    public interface ISpecification<T>
    {
        bool IsSatisfied(T t);
    }

    /// <summary>Interface <c>IFilter</c> defines filter interface.</summary>
    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }

    /// <summary>Class <c>ColorSpecification</c> implements specification interface.</summary>
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

    /// <summary>Class <c>SizeSpecification</c> implements specification interface.</summary>
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

    /// <summary>Class <c>AndSpecification</c> implements specification interface as well implements combinator of specifications.</summary>
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

    /// <summary>Class <c>ProductFilterEnhanced</c> concerned about filtering products by certain criteria utilizing open-closed principle.</summary>
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