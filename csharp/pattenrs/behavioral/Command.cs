namespace Command
{
    ///
    /// <summary>
    /// Class <c>Main</c> represents Command pattern usecase.
    /// Command: creates objects that encapsulate actions and parameters.
    /// Motivation:
    /// Helps to pass as parameters certain actions that are called in response to other actions. 
    /// When we need to ensure the execution of the request queue, as well as their possible cancellation.
    /// When we want to support logging of changes as a result of requests.
    /// </summary>
    ///
    class Main
    {
        public static void Run()
        {
            Console.WriteLine("\nCommand");

            // init user
            var user = new User("User", "example@email.com");

            // init receiver
            var receiver = new BankAccount(user);

            // init commands
            var deposit = new Deposit(receiver, 1000);
            var withdraw = new Withdraw(receiver, 500);

            // init invoker
            var invoker = new Terminal();

            Console.WriteLine($"Balance before deposit: {receiver.Balance}");

            // invoke commands
            invoker.SetCommand(deposit);
            invoker.Run();
            Console.WriteLine($"Balance after deposit: {receiver.Balance}");

            invoker.SetCommand(withdraw);
            invoker.Run();
            Console.WriteLine($"Balance after withdraw: {receiver.Balance}");

            invoker.Cancel();
            Console.WriteLine($"Balance after withdraw undo: {receiver.Balance}");
        }
    }

    // -- Command Abstraction

    /// <summary>Class <c>Operation</c> represents command abstraction.</summary>
    public abstract class Operation
    {
        public abstract void Execute();
        public abstract void Undo();
    }

    // -- Concreate Command

    /// <summary>Class <c>Deposit</c> represents concreate command.</summary>
    public class Deposit : Operation
    {
        private readonly BankAccount account;
        private readonly int amount;
        private bool isComplete;

        public Deposit(BankAccount account, int amount)
        {
            this.account = account;
            this.amount = amount;
            this.isComplete = false;
        }

        public override void Execute()
        {
            account.Balance += amount;
            isComplete = true;
        }
        public override void Undo()
        {
            if (isComplete && account.Balance - amount >= 0)
            {
                account.Balance -= amount;
                isComplete = false;
            }
        }
    }

    /// <summary>Class <c>Withdraw</c> represents concreate command.</summary>
    public class Withdraw : Operation
    {
        private readonly BankAccount account;
        private readonly int amount;
        private bool isComplete;
        public Withdraw(BankAccount account, int amount)
        {
            this.account = account;
            this.amount = amount;
            this.isComplete = false;
        }

        public override void Execute()
        {
            if (account.Balance - amount >= 0)
            {
                account.Balance -= amount;
                isComplete = true;
            }
        }
        public override void Undo()
        {
            if (isComplete)
            {
                account.Balance += amount;
                isComplete = false;
            }
        }
    }

    // -- Receiver

    /// <summary>Class <c>BankAccount</c> represents receiver.</summary>
    public class BankAccount
    {
        public User owner;
        public int Balance { get; set; }

        public BankAccount(User user)
        {
            owner = user;
            Balance = 0;
        }
    }

    // -- Invoker

    /// <summary>Class <c>Terminal</c> represents invoker.</summary>
    public class Terminal
    {
        private Operation? operation;

        public void SetCommand(Operation operation)
        {
            this.operation = operation;
        }

        public void Run()
        {
            if (operation != null)
            {
                operation.Execute();
            }
        }

        public void Cancel()
        {
            if (operation != null)
            {
                operation.Undo();
            }
        }
    }

    // -- Auxiliary classes

    /// <summary>Class <c>User</c> represents user entity.</summary>
    public class User
    {
        private string name, email;
        public User(string name, string email)
        {
            this.name = name; this.email = email;
        }
    }
}
