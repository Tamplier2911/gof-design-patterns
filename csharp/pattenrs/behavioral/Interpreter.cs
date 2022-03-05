namespace Interpreter
{
    ///
    /// <summary>
    /// Class <c>Main</c> represents Interpreter pattern usecase.
    /// Interpreter: implements a specialized language.
    /// Motivation:
    /// Interpreter processing text input, turning it into sequence of lexical tokens and then parsing those sequences.
    /// (compilers, interpreters, numeric expressions, regular expressions) are implementations of interpreter pattern.
    /// </summary>
    ///
    class Main
    {
        public static void Run()
        {
            Console.WriteLine("\nInterpreter");

            // init context
            var ctx = new Context();

            // set context variables
            ctx.SetVariable("x", 2);
            ctx.SetVariable("y", 4);
            ctx.SetVariable("z", 8);

            // init expression
            var expression = new SubstractExpression(
                new AddExpression(
                    new NumberExpression("y"), // 4 + 8 = 12
                    new NumberExpression("z")
                ),
                new NumberExpression("x") // 12 - 2 = 10
            );

            // interpret expression
            var result = expression.Interpret(ctx);

            Console.WriteLine($"Expression result: {result}");
        }
    }

    // -- Context

    /// <summary>Class <c>Context</c> represents interpretation context.</summary>
    public class Context
    {
        private Dictionary<string, int> variables;
        public Context()
        {
            variables = new Dictionary<string, int>();
        }

        public void SetVariable(string name, int value)
        {
            variables[name] = value;
        }

        public int GetVariable(string name)
        {
            if (variables.ContainsKey(name))
            {
                return variables[name];
            }
            return 0;
        }
    }

    // -- Abstract Expression

    /// <summary>Interface <c>IExpression</c> describes expression.</summary>
    public interface IExpression
    {
        public int Interpret(Context ctx);
    }

    // -- Concreate Expression

    /// <summary>Class <c>NumberExpression</c> represents concreate expression.</summary>
    public class NumberExpression : IExpression
    {
        private string name;
        public NumberExpression(string name)
        {
            this.name = name;
        }
        public int Interpret(Context ctx)
        {
            return ctx.GetVariable(name);
        }
    }

    /// <summary>Class <c>AddExpression</c> represents concreate expression.</summary>
    public class AddExpression : IExpression
    {
        private IExpression left;
        private IExpression right;
        public AddExpression(IExpression left, IExpression right)
        {
            this.left = left;
            this.right = right;
        }
        public int Interpret(Context ctx)
        {
            return left.Interpret(ctx) + right.Interpret(ctx);
        }
    }

    /// <summary>Class <c>SubstractExpression</c> represents concreate expression.</summary>
    public class SubstractExpression : IExpression
    {
        private IExpression left;
        private IExpression right;
        public SubstractExpression(IExpression left, IExpression right)
        {
            this.left = left;
            this.right = right;
        }
        public int Interpret(Context ctx)
        {
            return left.Interpret(ctx) - right.Interpret(ctx);
        }
    }
}
