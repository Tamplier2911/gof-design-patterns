namespace ChainOfResponsibility
{
    ///
    /// <summary>
    /// Class <c>Main</c> represents Chain of Responsibility pattern usecase.
    /// Chain of Responsibility: delegates commands to a chain of processing objects.
    /// Motivation:
    /// Helps to avoids hard-wiring the sender of a request to the receiver.
    /// Helps to have more than one object capable to handle particular request.
    /// Makes it possible to transfer a request to one of several objects without specifying exact one.
    /// Makes it possible to dynamically construct range of handler objects.
    /// </summary>
    ///
    class Main
    {
        public static void Run()
        {
            Console.WriteLine("\nChain of Responsibility");

            // init client
            var c = new Client(9305);

            // init handlers
            var thousands = new ThousandsHandler();
            var hundreds = new HundredsHandler();
            var tens = new TensHandler();
            var ones = new OnesHandler();

            // wire handlers
            thousands.
                SetNext(hundreds).
                SetNext(tens).
                SetNext(ones);

            // handle client request
            thousands.Handle(c);

            Console.WriteLine($"Client cash: {c.GetCash()}");
        }
    }

    // -- Handler Abstraction

    /// <summary>Interface <c>IHandler</c> describes handler abstraction interface.</summary>
    public interface IHandler
    {
        public IHandler SetNext(IHandler handler);
        public void Handle(IClient client);
    }

    /// <summary>Class <c>Handler</c> represents abstraction.</summary>
    public abstract class Handler : IHandler
    {
        protected IHandler? next;
        public IHandler SetNext(IHandler handler)
        {
            next = handler;
            return handler;
        }

        public virtual void Handle(IClient client)
        {
            if (this.next != null)
            {
                this.next.Handle(client);
            }
        }
    }

    // -- Concreate Handlers

    /// <summary>Class <c>ThousandsHandler</c> represents concreate handler.</summary>
    public class ThousandsHandler : Handler
    {
        public override void Handle(IClient client)
        {
            var cash = client.GetCash();
            if (InBounds(cash))
            {
                var dif = cash % 1000;
                var thousands = cash - dif;
                Console.WriteLine($"Withdraw {thousands / 1000} thousand");
                cash = client.SetCash(dif);
            }
            if (cash == 0) return;
            if (base.next != null)
            {
                base.next.Handle(client);
            }
        }

        private bool InBounds(int cash)
        {
            var bounds = Math.Floor(Math.Log10(cash)) + 1;
            return bounds > 3 && bounds < 7;
        }
    }

    /// <summary>Class <c>HundredsHandler</c> represents concreate handler.</summary>
    public class HundredsHandler : Handler
    {
        public override void Handle(IClient client)
        {
            var cash = client.GetCash();
            if (InBounds(cash))
            {
                var dif = cash % 100;
                var hundreds = cash - dif;
                Console.WriteLine($"Withdraw {hundreds / 100} hundred");
                cash = client.SetCash(dif);
            }
            if (cash == 0) return;
            if (base.next != null)
            {
                base.next.Handle(client);
            }
        }

        private bool InBounds(int cash)
        {
            var bounds = Math.Floor(Math.Log10(cash)) + 1;
            return bounds > 2 && bounds < 4;
        }
    }

    /// <summary>Class <c>TensHandler</c> represents concreate handler.</summary>
    public class TensHandler : Handler
    {
        public override void Handle(IClient client)
        {
            var cash = client.GetCash();
            if (InBounds(cash))
            {
                var dif = cash % 10;
                var tens = cash - dif;
                Console.WriteLine($"Withdraw {tens / 10} tens");
                cash = client.SetCash(dif);
            }
            if (cash == 0) return;
            if (base.next != null)
            {
                base.next.Handle(client);
            }
        }

        private bool InBounds(int cash)
        {
            var bounds = Math.Floor(Math.Log10(cash)) + 1;
            return bounds > 1 && bounds < 3;
        }
    }


    /// <summary>Class <c>OnesHandler</c> represents concreate handler.</summary>
    public class OnesHandler : Handler
    {
        public override void Handle(IClient client)
        {
            var cash = client.GetCash();
            if (InBounds(cash) && cash != 0)
            {
                Console.WriteLine($"Withdraw {cash} ones");
                cash = client.SetCash(0);
            }
            if (cash == 0) return;
            if (base.next != null)
            {
                base.next.Handle(client);
            }
        }

        private bool InBounds(int cash)
        {
            var bounds = Math.Floor(Math.Log10(cash)) + 1;
            return bounds > 0 && bounds < 2;
        }
    }

    // -- Client

    /// <summary>Interface <c>Client</c> describes client abstraction.</summary>
    public interface IClient
    {
        public int GetCash();
        public int SetCash(int cash);
    }

    /// <summary>Class <c>Client</c> represents client.</summary>
    public class Client : IClient
    {
        private int cash;
        public Client(int cash)
        {
            this.cash = cash;
        }

        public int GetCash() { return this.cash; }
        public int SetCash(int cash) { return this.cash = cash; }
    }
}
