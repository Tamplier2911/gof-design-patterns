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

            // problems of shallow clone with IClonable
            var pj = new Person(new[] { "John", "Smith" }, new Address("City", "Street", 123));
            var pv = (Person)pj.Clone();
            pv.Names[0] = "Violet";
            pv.Address.House = 321; // copied reference when cloned
            Console.WriteLine(pj);
            Console.WriteLine(pv);
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
    }
}