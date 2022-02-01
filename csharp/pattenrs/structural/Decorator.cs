using System.Text;

namespace Decorator
{
    // Decorator: dynamically adds/overrides behaviour in an existing method of an object.
    //
    // Motivation:
    // we want to augment the object with additional functionality but we don't want to alter existing code / opening class (OCP)
    // we want to keep new functionality separete (SRP)
    // we need to interact with existing structures
    // -- inheritance (if class is not sealed)
    // -- build decorator (else)
    class Main
    {
        public static void Run()
        {
            Console.WriteLine("\nDecorator\n");

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

    // Pizza - represents an abstraction (component).
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

    // ItalianPizza - represents concreate implementation of (component) Pizza.
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

    // BulgarianPizza - represents concreate implementation of (component) Pizza.
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

    // PizzaDecorator - represents an abstraction for decorator.
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

    // TomatoPizza - represents concreate implementation of (decorator) PizzaDecorator.
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

    // CheesePizza - represents concreate implementation of (decorator) PizzaDecorator.
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