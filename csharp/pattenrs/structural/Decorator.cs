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

            var bp = new BulgarianPizza("Banica", 16);
            var bpc = new CheesePizza(bp, 3);
            Console.WriteLine($"{bpc.Name} - ${bpc.Price}");

            var ip = new ItalianPizza("Margherita", 20);
            var ipc = new CheesePizza(ip, 3);
            var ipct = new TomatoPizza(ipc, 2);
            Console.WriteLine($"{ipct.Name} - ${ipct.Price}");
        }
    }

    // -- Component

    // Pizza - represents an abstraction (component).
    public abstract class Pizza
    {
        public string Origin { get; private set; }
        public string Name { get; private set; }
        public int Price { get; private set; }
        public Pizza(string origin, string name, int price)
        {
            Origin = origin;
            Name = name;
            Price = price;
        }
        public abstract int GetPrice();
    }

    // -- Concreate Component 

    // ItalianPizza - represents concreate implementation of (component) Pizza.
    public class ItalianPizza : Pizza
    {
        private const string Italian = "Italian";
        public ItalianPizza(string name, int price) : base(Italian, name, price) { }

        public override int GetPrice()
        {
            return Price;
        }
    }

    // BulgarianPizza - represents concreate implementation of (component) Pizza.
    public class BulgarianPizza : Pizza
    {
        private const string Bulgarian = "Bulgarian";
        public BulgarianPizza(string name, int price) : base(Bulgarian, name, price) { }

        public override int GetPrice()
        {
            return Price;
        }
    }

    // -- Decorator

    // PizzaDecorator - represents an abstraction for decorator.
    public abstract class PizzaDecorator : Pizza
    {
        protected Pizza Pizza; // reference to component SetComponent
        public PizzaDecorator(Pizza pizza, string name, int price) : base(pizza.Origin, name, price)
        {
            Pizza = pizza;
        }
    }

    // -- Concreate Decorator

    // TomatoPizza - represents concreate implementation of (decorator) PizzaDecorator.
    public class TomatoPizza : PizzaDecorator
    {
        private const string Cheese = "Tomatoes";
        public TomatoPizza(Pizza pizza, int price) : base(pizza, pizza.Name + $", with {Cheese}", pizza.Price + price) { }

        public override int GetPrice()
        {
            return Price;
        }
    }

    // CheesePizza - represents concreate implementation of (decorator) PizzaDecorator.
    public class CheesePizza : PizzaDecorator
    {
        private const string Cheese = "Cheese";
        public CheesePizza(Pizza pizza, int price) : base(pizza, pizza.Name + $", with {Cheese}", pizza.Price + price) { }

        public override int GetPrice()
        {
            return Price;
        }
    }
}