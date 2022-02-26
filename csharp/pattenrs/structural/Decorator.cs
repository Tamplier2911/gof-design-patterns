using System.Text;

namespace Decorator
{
    ///
    /// <summary>
    /// Class <c>Main</c> represents Decorator pattern usecase.
    /// Decorator: dynamically adds/overrides behaviour in an existing method of an object.
    /// Motivation:
    /// we want to augment the object with additional functionality but we don't want to alter existing code / opening class (OCP)
    /// we want to keep new functionality separete (SRP)
    /// we need to interact with existing structures
    /// -- inheritance (if class is not sealed)
    /// -- build decorator (else)
    /// </summary>
    ///
    class Main
    {
        public static void Run()
        {
            Console.WriteLine("\nDecorator");

            // Decorator

            // create component
            var bp = new BulgarianPizza("Banica", 16);
            // decorate component
            var bpc = new CheesePizza(bp, 3);
            // review
            Console.WriteLine($"{bpc.GetName()} ${bpc.GetPrice()}");
            Console.WriteLine(bpc.GetComponent());

            // create component
            var ip = new ItalianPizza("Margherita", 20);
            // decorate component
            var ipc = new CheesePizza(ip, 4);
            var ipct = new TomatoPizza(ipc, 3);
            // review
            Console.WriteLine($"{ipct.GetName()} ${ipct.GetPrice()}");
            Console.WriteLine(ipct.GetComponent());
        }
    }

    // -- Component

    /// <summary>Class <c>IHandler</c> represents an abstraction (component).</summary>
    public abstract class Pizza
    {
        internal string Origin { get; set; }
        protected string Name { get; set; }
        protected int Price { get; set; }
        public Pizza(string origin, string name, int price)
        {
            Origin = origin;
            Name = name;
            Price = price;
        }
        public abstract string GetName();
        public abstract int GetPrice();
        public abstract string GetComponent(); // decorator test method
    }

    // -- Concreate Component 

    /// <summary>Class <c>ItalianPizza</c> represents concreate implementation of (component) Pizza.</summary>
    public class ItalianPizza : Pizza
    {
        private const string _italian = "Italian";
        public ItalianPizza(string name, int price) : base(_italian, name, price) { }

        public override string GetName()
        {
            return Name;
        }

        public override int GetPrice()
        {
            return Price;
        }

        public override string GetComponent()
        {
            return Origin;
        }
    }

    /// <summary>Class <c>BulgarianPizza</c> represents concreate implementation of (component) Pizza.</summary>
    public class BulgarianPizza : Pizza
    {
        private const string _bulgarian = "Bulgarian";
        public BulgarianPizza(string name, int price) : base(_bulgarian, name, price) { }

        public override string GetName()
        {
            return Name;
        }

        public override int GetPrice()
        {
            return Price;
        }

        public override string GetComponent()
        {
            return Origin;
        }
    }

    // -- Decorator

    /// <summary>Class <c>PizzaDecorator</c> represents an abstraction for decorator.</summary>
    public abstract class PizzaDecorator : Pizza
    {
        protected Pizza pizza; // reference to component SetComponent
        protected string name;
        protected int price;
        public PizzaDecorator(Pizza pizza, string name, int price) : base(pizza.Origin, pizza.GetName(), pizza.GetPrice())
        {
            this.pizza = pizza;
            this.name = name;
            this.price = price;
        }
    }

    // -- Concreate Decorator

    /// <summary>Class <c>TomatoPizza</c> represents concreate implementation of (decorator) PizzaDecorator.</summary>
    public class TomatoPizza : PizzaDecorator
    {
        private const string _tomatoes = "Tomatoes";
        public TomatoPizza(Pizza pizza, int price) : base(pizza, _tomatoes, price) { }

        public override string GetName()
        {
            return $"{pizza.GetName()}, with {name}";
        }

        public override int GetPrice()
        {
            return pizza.GetPrice() + price;
        }

        public override string GetComponent()
        {
            return $"{name}({pizza.GetComponent()})";
        }
    }

    /// <summary>Class <c>CheesePizza</c> represents concreate implementation of (decorator) PizzaDecorator.</summary>
    public class CheesePizza : PizzaDecorator
    {
        private const string _cheese = "Cheese";
        public CheesePizza(Pizza pizza, int price) : base(pizza, _cheese, price) { }

        public override string GetName()
        {
            return $"{pizza.GetName()}, with {name}";
        }

        public override int GetPrice()
        {
            return pizza.GetPrice() + price;
        }

        public override string GetComponent()
        {
            return $"{name}({pizza.GetComponent()})";
        }
    }
}