namespace Patterns
{
    // Prototype: creates objects by cloning an existing object.
    //
    // Motivation:  
    // Complicated objects are not designed from scratch - existing design being re-iterated
    // An existing object partially or fully constructed is a Prototype
    // We make a copy (deep clone) of the prototype and customized it - cloning must be convenient
    class Prototype
    {
        public static void Run()
        {
            Console.WriteLine("Prototype");

            // shallow clone with IClonable
            var pa = new Person(new[] { "John", "Smith" }, new Address("City", "Street", 123));
            var pb = (Person)pa.Clone();
            pb.Names[0] = "Violet";
            pb.Address.House = 321;

            Console.WriteLine(pa);
            Console.WriteLine(pb);

            // clone with copy constructor
            var h1 = new Hero(new[] { "Light", "Yagami" }, new Power("Desu Noto"));
            var h2 = new Hero(h1);
            h2.Names[0] = "Soichiro";
            h2.Power.Source = "Justice";

            Console.WriteLine(h1);
            Console.WriteLine(h2);

            // own interface for deep copy
            var ta = new Thing("Thing");
            var tb = ta.DeepCopy();
            tb.Name = "SuperThing";

            Console.WriteLine(ta.Name);
            Console.WriteLine(tb.Name);
        }

        //

        // IClonable - shallow clone is not quite right.

        //

        // Person - represents person.
        class Person : ICloneable
        {
            public List<string> Names = new();
            public Address Address;

            public Person(string[] names, Address address)
            {
                // Names.Add(name);
                foreach (var name in names)
                {
                    Names.Add(name);
                }
                Address = address;
            }

            public object Clone()
            {
                // shallow clone
                return new Person(Names.ToArray(), (Address)Address.Clone());
            }

            public override string ToString()
            {
                return $"Names: {string.Join(", ", Names.ToArray())} | " +
                $"Address: {Address.City} - {Address.Street} - {Address.House}";
            }
        }

        // Address - represents person address.
        class Address : ICloneable
        {
            public string City;
            public string Street;
            public int House;

            public Address(string city, string street, int house)
            {
                City = city;
                Street = street;
                House = house;
            }

            public object Clone()
            {
                return new Address(City, Street, House);
            }
        }

        //

        // Copy Constructor - a special type of extensible constructor.

        //

        // Hero - represents hero.
        class Hero
        {
            public List<string> Names = new();
            public Power Power;

            public Hero(string[] names, Power power)
            {
                foreach (var name in names)
                {
                    Names.Add(name);
                }
                Power = power;
            }

            // copy constructor
            public Hero(Hero other)
            {
                foreach (var name in other.Names)
                {
                    Names.Add(name);
                }
                Power = new Power(other.Power);
            }

            public override string ToString()
            {
                return $"Names: {string.Join(", ", Names.ToArray())} | " +
                $"Power: {Power.Source}";
            }
        }

        // Power - represents hero power.
        class Power
        {
            public string Source;

            public Power(string power)
            {
                Source = power;
            }

            // copy constructor
            public Power(Power other)
            {
                Source = other.Source;
            }
        }

        //

        // Own Prototype Interface

        //

        //

        interface IPrototype<T>
        {
            T DeepCopy();
        }

        class Thing : IPrototype<Thing>
        {
            public string Name;
            public Thing(string name)
            {
                Name = name;
            }

            public Thing DeepCopy()
            {
                return new Thing(Name);
            }
        }
    }
}