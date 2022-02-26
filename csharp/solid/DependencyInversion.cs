namespace Solid
{
    ///
    /// <summary>
    /// Class <c>DependencyInversion</c> represents dependency inversion principle usecase.
    /// 1. high-level modules should not import anything from low-level modules, both should depend on abstractions
    /// 2. abstractions should not depend on details, details (concrete implementations) should depend on abstractions
    /// </summary>
    ///
    class DependencyInversion
    {
        public static void Run()
        {
            Console.WriteLine("\nDependency Inversion\n");

            Person t = new Person("Tom");
            Robot j = new Robot("Jerry");

            // since both Person and Robot types implement IAssignee interface chores can be created with both this types
            // type Chore depends on interface which does not depend on details
            Chore ct = new Chore("Go to the moon!", t);
            Chore cj = new Chore("Go to the beach!", j);
            ct.CompleteChore();

            Console.WriteLine(ct);
            Console.WriteLine(cj);
        }
    }

    /// <summary>Class <c>Chore</c> represents chore entity.</summary>
    class Chore
    {
        private string ChoreDescription;
        // if we used Person type directly instead using interface DI would be violated
        private IAssignable Assignee;
        private bool Done = false;

        public Chore(string choreDescription, IAssignable assignee)
        {
            ChoreDescription = choreDescription ?? throw new ArgumentNullException(paramName: nameof(choreDescription));
            Assignee = assignee ?? throw new ArgumentNullException(paramName: nameof(assignee));
        }

        public void CompleteChore()
        {
            Done = Assignee.Work();
        }

        public override string ToString()
        {
            return $"--\nChore: {ChoreDescription}\nAssignee: {Assignee.Name}\nDone: {Done}\n--";
        }
    }

    /// <summary>Interface <c>IAssignable</c> represents assignable interface.</summary>
    interface IAssignable
    {
        string Name { get; set; }
        bool Work();
    }

    /// <summary>Class <c>Person</c> represents person entity.</summary>
    class Person : IAssignable
    {
        public string Name { get; set; }

        public Person(string name)
        {
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
        }

        public bool Work()
        {
            return true;
        }
    }

    /// <summary>Robot <c>Person</c> represents robot entity.</summary>
    class Robot : IAssignable
    {
        public string Name { get; set; }

        public Robot(string name)
        {
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
        }

        public bool Work()
        {
            return true;
        }
    }
}
