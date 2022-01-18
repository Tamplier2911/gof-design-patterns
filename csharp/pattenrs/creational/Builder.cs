using System.Text;

namespace Builder
{
    // Builder: when piecewise object construction is complicated provides an API for doing it succinctly.
    //
    // Motivation:
    // Some objects are simple and can be created within a single constructor call.
    // Some isn't, having object with a dozen constructor arguments is not productive.
    // Builder provides API for constructing objects step by step.
    class Main
    {
        public static void Run()
        {
            Console.WriteLine("\nBuilder\n");

            // regular builder with fluent interface
            var cb = new CodeBuilder("Person").
                AddField("Name", "string").
                AddField("Age", "int");
            Console.WriteLine(cb);

            // fluent builder interface and inheritance (using recursive generics)
            var person = Person.New.
                Called("Tommy").
                WorkAs("Creator").
                Build();
            Console.WriteLine(person);

            // stepwise builder - sequential builder (one call at a time)
            var car = CarBuilder.
                Create().               // returns ISpecifyCarType
                OfType(CarType.Sedan).  // returns ISpecifyWheelSize 
                WithWheels(15).         // returns IBuildCar
                Build();                // returns Car
            Console.WriteLine(car);

            // functional builder - using extension methods
            var cat = new CatBuilder().
                Called("Tom").           // cat builder method 
                Likes("to chase Jerry"). // extension method
                Build();
            Console.WriteLine(cat);

            // faceted builder - combine multiple builders into a builder(facade)
            var eb = new EmployeeBuilder(); // provides api for each builder

            var employee = eb.
                Lives. // address builder
                    OnStreet("501 N VIRGIL").
                    InCity("Los Angeles").
                    WithPostalCode("90004-2315").
                Works. // job builder
                    InCompany("Hufflepuff").
                    OnPosition("Puffmaker").
                Build(); // return employee object
            Console.WriteLine(employee);
        }
    }

    //

    // Basic Builder with fluent interface.

    //

    class CodeBuilder
    {
        private string ClassName;
        private List<string> Fields = new List<string>();
        private const string AccessMod = "public";

        public CodeBuilder(string className)
        {
            ClassName = className;
        }

        public CodeBuilder AddField(string fieldName, string fieldType)
        {
            Fields.Add($"\n  {AccessMod} {fieldType} {fieldName};");
            return this;
        }

        public string ImplementToString()
        {
            var Code = $"{AccessMod} class {ClassName}";

            Code += "\n{";
            foreach (var field in Fields)
            {
                Code += field;
            }
            Code += "\n}";

            return Code;
        }

        public override string ToString()
        {
            return ImplementToString();
        }
    }

    //

    // Fluent Interface and Inheritance - handled with recursive generics

    // 

    abstract class PersonBuilder
    {
        protected Person p = new Person();

        public Person Build()
        {
            return p;
        }
    }

    class PersonInfoBuilder<SELF> : PersonBuilder
    where SELF : PersonInfoBuilder<SELF> // recursive generics
    {
        public SELF Called(string name)
        {
            p.Name = name;
            return (SELF)this;
        }
    }

    class PersonJobBuilder<SELF> : PersonInfoBuilder<PersonJobBuilder<SELF>>
    where SELF : PersonJobBuilder<SELF> // recursive generics
    {
        public SELF WorkAs(string position)
        {
            p.Position = position;
            return (SELF)this;
        }
    }

    class Person
    {
        public string? Name;
        public string? Position;

        // Builder - represents builder class which inherit from itself.
        public class Builder : PersonJobBuilder<Builder> { }

        // New - create instance of builder.
        public static Builder New => new Builder();

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name} | {nameof(Position)}: {Position}";
        }
    }

    //

    // Stepwise Builder

    //

    public enum CarType { Sedan, Crossover }

    interface ISpecifyCarType
    {
        ISpecifyWheelSize OfType(CarType type);
    }

    interface ISpecifyWheelSize
    {
        IBuildCar WithWheels(int size);
    }

    interface IBuildCar
    {
        public Car Build();
    }

    class CarBuilder
    {
        private class Implement : ISpecifyCarType, ISpecifyWheelSize, IBuildCar
        {
            private Car c = new Car();

            public ISpecifyWheelSize OfType(CarType type)
            {
                c.Type = type;
                return this;
            }

            public IBuildCar WithWheels(int size)
            {
                switch (c.Type)
                {
                    case CarType.Sedan when size < 15 || size > 17:
                    case CarType.Crossover when size < 17 || size > 20:
                        throw new ArgumentException("invalid input data");
                }
                c.WheelSize = size;
                return this;
            }

            public Car Build()
            {
                return c;
            }
        }

        public static ISpecifyCarType Create()
        {
            return new Implement();
        }
    }

    class Car
    {
        public CarType Type;
        public int WheelSize;

        public override string ToString()
        {
            return $"{nameof(Type)}: {Type} | {nameof(WheelSize)}: {WheelSize}";
        }
    }

    //

    // Functional Builder

    //

    public abstract class FunctionalBuilder<TSubject, TSelf>
    where TSubject : new()
    where TSelf : FunctionalBuilder<TSubject, TSelf>
    {
        private readonly List<Func<TSubject, TSubject>> actions = new List<Func<TSubject, TSubject>>();

        public TSelf Do(Action<TSubject> action) => AddAction(action);

        public TSubject Build() => actions.Aggregate(new TSubject(), (subject, func) => func(subject));

        private TSelf AddAction(Action<TSubject> action)
        {
            actions.Add(c => { action(c); return c; });
            return (TSelf)this;
        }
    }

    public sealed class CatBuilder : FunctionalBuilder<Cat, CatBuilder>
    {
        public CatBuilder Called(string name) => Do(c => c.Name = name);
    }

    public static class CatBuilderExtansions
    {
        public static CatBuilder Likes(this CatBuilder builder, string hobby) => builder.Do(c => c.Hobby = hobby);
    }

    public class Cat
    {
        public string? Name, Hobby;

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name} | {nameof(Hobby)}: {Hobby}";
        }
    }

    //

    // Faceted Builder 

    //

    public class EmployeeBuilder
    {
        // !all builders use same reference
        protected Employee employee = new Employee();

        public EmployeeAddressBuilder Lives => new EmployeeAddressBuilder(employee);
        public EmployeeJobBuilder Works => new EmployeeJobBuilder(employee);
        public Employee Build() => employee;
    }

    public class EmployeeAddressBuilder : EmployeeBuilder
    {
        public EmployeeAddressBuilder(Employee e)
        {
            // pass reference of person from EmployeeBuilder(facade)
            employee = e;
        }

        public EmployeeAddressBuilder OnStreet(string street)
        {
            employee.StreetAddress = street;
            return this;
        }

        public EmployeeAddressBuilder InCity(string city)
        {
            employee.City = city;
            return this;
        }

        public EmployeeAddressBuilder WithPostalCode(string code)
        {
            employee.PostCode = code;
            return this;
        }
    }

    // EmployeeJobBuilder - builds employee job information.
    public class EmployeeJobBuilder : EmployeeBuilder
    {
        public EmployeeJobBuilder(Employee e)
        {
            // pass reference of person from EmployeeBuilder(facade)
            employee = e;
        }

        public EmployeeJobBuilder InCompany(string company)
        {
            employee.CompanyName = company;
            return this;
        }

        public EmployeeJobBuilder OnPosition(string position)
        {
            employee.Position = position;
            return this;
        }
    }

    public class Employee
    {
        // address
        public string? StreetAddress, PostCode, City;

        // employment
        public string? CompanyName, Position;

        public override string ToString()
        {
            return $"{nameof(StreetAddress)} {StreetAddress} | {nameof(City)} {City} | {nameof(PostCode)} {PostCode} | " +
            $"{nameof(CompanyName)} {CompanyName} | {nameof(Position)} {Position}";
        }
    }
}