namespace Prototype
{
    // Prototype: creates objects by cloning an existing object.
    //
    // Motivation:  
    // Complicated objects are not designed from scratch - existing design being re-iterated
    // An existing object partially or fully constructed is a Prototype
    // We make a copy (deep clone) of the prototype and customized it - cloning must be convenient
    public class Main
    {
        public static void Run()
        {
            Console.WriteLine("Prototype");

            // shallow clone with ICloneable
            var pa = new PersonOne(new[] { "John", "Smith" }, new AddressOne("City", "Street", 123));
            var pb = (PersonOne)pa.Clone();
            pb.Names[0] = "Violet";
            pb.Address.House = 321;

            Console.WriteLine(pa);
            Console.WriteLine(pb);

            // clone with Copy Constructor
            var h1 = new PersonTwo(new[] { "John", "Smith" }, new AddressTwo("City", "Street", 123));
            var h2 = new PersonTwo(h1);
            h2.Names[0] = "Violet";
            h2.Address.House = 321;

            Console.WriteLine(h1);
            Console.WriteLine(h2);

            // prototypal inheritance
            var j = new Employee(new[] { "John", "Smith" }, new AddressThree("City", "Street", 123), 777);
            var e = j.DeepCopy(); // without type specification
            e.Names[0] = "Violet";
            var p = j.DeepCopy<PersonThree>(); // with type specification
            p.Address!.House = 321;

            Console.WriteLine(j);
            Console.WriteLine(e);
            Console.WriteLine(p);
        }
    }

    //

    // ICloneable - shallow clone.

    //

    public class PersonOne : ICloneable
    {
        public List<string> Names = new();
        public AddressOne Address;

        public PersonOne(string[] names, AddressOne address)
        {
            Names = new List<string>(names);
            Address = address;
        }

        public object Clone()
        {
            // shallow clone
            return new PersonOne(Names.ToArray(), (AddressOne)Address.Clone());
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

        public PersonTwo(string[] names, AddressTwo address)
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

        public PersonThree(string[] names, AddressThree address)
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

        public Employee(string[] names, AddressThree address, int salary) : base(names, address)
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
}