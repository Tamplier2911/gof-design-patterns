using System.Text;

namespace Builder
{
    ///
    /// <summary>
    /// Class <c>Main</c> represents Builder pattern usecase.
    /// Builder: when piecewise object construction is complicated provides an API for doing it succinctly.
    /// Motivation:
    /// Some objects are simple and can be created within a single constructor call.
    /// Some isn't, having object with a dozen constructor arguments is not productive.
    /// Builder provides API for constructing objects step by step.
    /// </summary>
    ///
    class Main
    {
        public static void Run()
        {
            Console.WriteLine("\nBuilder");

            // regular builder with fluent interface
            var cb = new CodeBuilder("Person").
                AddField("Name", "string").
                AddField("Age", "int");
            Console.WriteLine(cb);

            // builder with fluent interface and inheritance (using recursive generics)
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

    // -- Basic Builder with fluent interface.

    /// <summary>Class <c>CodeBuilder</c> represents builder pattern usecase.</summary>

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

    // -- Fluent Interface and Inheritance - handled with recursive generics

    /// <summary>Class <c>PersonBuilder</c> represents builder pattern usecase abstraction.</summary>
    abstract class PersonBuilder
    {
        protected Person p = new Person();

        public Person Build()
        {
            return p;
        }
    }

    /// <summary>Class <c>PersonInfoBuilder</c> represents builder pattern concreate usecase.</summary>
    class PersonInfoBuilder<SELF> : PersonBuilder
    where SELF : PersonInfoBuilder<SELF> // recursive generics
    {
        public SELF Called(string name)
        {
            p.Name = name;
            return (SELF)this;
        }
    }

    /// <summary>Class <c>PersonJobBuilder</c> represents builder pattern concreate usecase.</summary>
    class PersonJobBuilder<SELF> : PersonInfoBuilder<PersonJobBuilder<SELF>>
    where SELF : PersonJobBuilder<SELF> // recursive generics
    {
        public SELF WorkAs(string position)
        {
            p.Position = position;
            return (SELF)this;
        }
    }

    /// <summary>Class <c>Person</c> represents person entity.</summary>
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

    // -- Stepwise Builder

    /// <summary>Enum <c>CarType</c> represents car types.</summary>
    public enum CarType { Sedan, Crossover }

    /// <summary>Interface <c>ISpecifyCarType</c> describes car type specification.</summary>
    interface ISpecifyCarType
    {
        ISpecifyWheelSize OfType(CarType type);
    }

    /// <summary>Interface <c>ISpecifyWheelSize</c> describes car size specification.</summary>
    interface ISpecifyWheelSize
    {
        IBuildCar WithWheels(int size);
    }


    /// <summary>Interface <c>IBuildCar</c> describes car builder.</summary>
    interface IBuildCar
    {
        public Car Build();
    }

    /// <summary>Class <c>CarBuilder</c> represents car builder.</summary>
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

    /// <summary>Class <c>Car</c> represents car entity.</summary>
    class Car
    {
        public CarType Type;
        public int WheelSize;

        public override string ToString()
        {
            return $"{nameof(Type)}: {Type} | {nameof(WheelSize)}: {WheelSize}";
        }
    }

    // -- Functional Builder

    /// <summary>Class <c>FunctionalBuilder</c> represents functional builder abstraction.</summary>

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

    /// <summary>Class <c>CatBuilder</c> represents cat builder.</summary>
    public sealed class CatBuilder : FunctionalBuilder<Cat, CatBuilder>
    {
        public CatBuilder Called(string name) => Do(c => c.Name = name);
    }

    /// <summary>Class <c>CatBuilderExtansions</c> represents cat builder extensions.</summary>
    public static class CatBuilderExtansions
    {
        public static CatBuilder Likes(this CatBuilder builder, string hobby) => builder.Do(c => c.Hobby = hobby);
    }

    /// <summary>Class <c>Cat</c> represents cat entity.</summary>
    public class Cat
    {
        public string? Name, Hobby;

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name} | {nameof(Hobby)}: {Hobby}";
        }
    }

    // -- Faceted Builder 

    /// <summary>Class <c>EmployeeBuilder</c> represents employee builder.</summary>
    public class EmployeeBuilder
    {
        // !all builders use same reference
        protected Employee employee = new Employee();

        public EmployeeAddressBuilder Lives => new EmployeeAddressBuilder(employee);
        public EmployeeJobBuilder Works => new EmployeeJobBuilder(employee);
        public Employee Build() => employee;
    }

    /// <summary>Class <c>EmployeeAddressBuilder</c> represents employee address builder.</summary>
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

    /// <summary>Class <c>EmployeeJobBuilder</c> represents employee job builder.</summary>
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

    /// <summary>Class <c>Employee</c> represents employee entity.</summary>
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