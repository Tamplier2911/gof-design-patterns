using System.Text.Json;

namespace Prototype
{
    // Prototype: creates objects by cloning an existing object.
    //
    // Motivation:  
    // Complicated objects are not designed from scratch - existing design being re-iterated
    // An existing object partially or fully constructed is a Prototype
    // We make a copy (deep clone) of the prototype and customized it - cloning must be convenient
    class Main
    {
        public static void Run()
        {
            Console.WriteLine("\nPrototype");

            // shallow clone with ICloneable
            var pa = new PersonOne(new List<string> { "John", "Smith" }, new AddressOne("City", "Street", 123));
            var pb = (PersonOne)pa.Clone();
            pb.Names[0] = "Violet";
            pb.Address.House = 321;

            Console.WriteLine(pa);
            Console.WriteLine(pb);

            // clone with Copy Constructor
            var h1 = new PersonTwo(new List<string> { "John", "Smith" }, new AddressTwo("City", "Street", 123));
            var h2 = new PersonTwo(h1);
            h2.Names[0] = "Violet";
            h2.Address.House = 321;

            Console.WriteLine(h1);
            Console.WriteLine(h2);

            // prototypal inheritance
            var j = new Employee(new List<string> { "John", "Smith" }, new AddressThree("City", "Street", 123), 777);
            var e = j.DeepCopy(); // without type specification
            e.Names[0] = "Violet";
            var p = j.DeepCopy<PersonThree>(); // with type specification
            p.Address!.House = 321;

            Console.WriteLine(j);
            Console.WriteLine(e);
            Console.WriteLine(p);

            // copy through serialization
            var cs1 = new PersonFour(new List<string> { "John", "Smith" }, new AddressFour("City", "Street", 123));
            var cs2 = cs1.DeepCopyJSON<PersonFour>();
            cs2.Names[0] = "Violet";
            var cs3 = cs2.DeepCopyJSON<PersonFour>();
            cs3.Address.House = 321;

            Console.WriteLine(cs1);
            Console.WriteLine(cs2);
            Console.WriteLine(cs3);
        }
    }

    //

    // ICloneable - shallow clone.

    //

    public class PersonOne : ICloneable
    {
        public List<string> Names = new();
        public AddressOne Address;

        public PersonOne(List<string> names, AddressOne address)
        {
            Names = new List<string>(names);
            Address = address;
        }

        public object Clone()
        {
            // shallow clone
            return new PersonOne(Names, (AddressOne)Address.Clone());
        }

        public override string ToString()
        {
            return $"Names: {string.Join(", ", Names.ToArray())} | " +
            $"Address: {Address.City} - {Address.Street} - {Address.House} | ";
        }
    }

    public class AddressOne : ICloneable
    {
        public string City;
        public string Street;
        public int House;

        public AddressOne(string city, string street, int house)
        {
            City = city;
            Street = street;
            House = house;
        }

        public object Clone()
        {
            return new AddressOne(City, Street, House);
        }
    }

    //

    // Copy Constructor - a special type of extensible constructor.

    //

    public class PersonTwo
    {
        public List<string> Names = new();
        public AddressTwo Address;

        public PersonTwo(List<string> names, AddressTwo address)
        {
            Names = new List<string>(names);
            Address = address;
        }

        public PersonTwo(PersonTwo other)
        {
            Names = new List<string>(other.Names);
            Address = new AddressTwo(other.Address);
        }

        public override string ToString()
        {
            return $"Names: {string.Join(", ", Names.ToArray())} | " +
            $"Address: {Address.City} - {Address.Street} - {Address.House} | ";
        }
    }

    public class AddressTwo
    {
        public string City;
        public string Street;
        public int House;

        public AddressTwo(string city, string street, int house)
        {
            City = city;
            Street = street;
            House = house;
        }

        public AddressTwo(AddressTwo other)
        {
            City = other.City;
            Street = other.Street;
            House = other.House;
        }
    }

    //

    // Prototype Inheritance 

    //

    public interface IDeepCopyable<T> where T : new() // start from blank
    {
        void CopyTo(T target); // copy internal state into target
        public T DeepCopy() // default interface member
        {
            T t = new T();
            CopyTo(t);
            return t;
        }
    }

    public static class ExtensionMethods
    {
        public static T DeepCopy<T>(this IDeepCopyable<T> item) where T : new() // with type specification
        {
            return item.DeepCopy();
        }

        public static T DeepCopy<T>(this T person) where T : PersonThree, new() // without type specification
        {
            return ((IDeepCopyable<T>)person).DeepCopy();
        }
    }

    public class PersonThree : IDeepCopyable<PersonThree>
    {
        public List<string> Names = new();
        public AddressThree? Address;

        public PersonThree() { }

        public PersonThree(List<string> names, AddressThree address)
        {
            Names = new List<string>(names);
            Address = address;
        }

        public void CopyTo(PersonThree t)
        {
            t.Names = new List<string>(Names);
            t.Address = Address!.DeepCopy(); // call on extension method
            // t.Address = ((IDeepCopyable<Address>)Address!).DeepCopy(); // call on interface
        }

        public override string ToString()
        {
            return $"Names: {string.Join(", ", Names.ToArray())} | " +
            $"Address: {Address!.City} - {Address.Street} - {Address.House} | ";
        }
    }

    // AddressThree - represents person address.
    public class AddressThree : IDeepCopyable<AddressThree>
    {
        public string? City;
        public string? Street;
        public int House;

        public AddressThree() { }

        public AddressThree(string city, string street, int house)
        {
            City = city;
            Street = street;
            House = house;
        }

        public void CopyTo(AddressThree t)
        {
            t.City = City;
            t.Street = Street;
            t.House = House;
        }
    }

    public class Employee : PersonThree, IDeepCopyable<Employee>
    {
        public int Salary;

        public Employee() { }

        public Employee(List<string> names, AddressThree address, int salary) : base(names, address)
        {
            Salary = salary;
        }

        public void CopyTo(Employee t)
        {
            t.Salary = Salary;
            base.CopyTo(t); // copy rest from base class
        }

        public override string ToString()
        {
            return $"{base.ToString()}" + $"Salary: {Salary}";
        }
    }

    //

    // Copy through serialization

    //

    public static class ExtensionMethods2
    {
        public static T DeepCopyJSON<T>(this T self)
        {
            return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize<T>(self))!;
        }
    }

    class PersonFour
    {
        public List<string> Names { get; set; }
        public AddressFour Address { get; set; }

        public PersonFour(List<string> names, AddressFour address)
        {
            Names = new List<string>(names);
            Address = address;
        }

        public override string ToString()
        {
            return $"Names: {string.Join(", ", Names.ToArray())} | " +
            $"Address: {Address!.City} - {Address.Street} - {Address.House} | ";
        }
    }

    class AddressFour
    {
        public string City { get; set; }
        public string Street { get; set; }
        public int House { get; set; }

        public AddressFour(string city, string street, int house)
        {
            City = city;
            Street = street;
            House = house;
        }
    }
}