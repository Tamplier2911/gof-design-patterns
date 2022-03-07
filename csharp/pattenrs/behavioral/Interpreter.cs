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

    public class ExpressionProcessor
    {
        public Dictionary<char, int> Variables = new Dictionary<char, int>();

        /*
            public int Calculate(string expression)
            {
                // todo
                // "-" and "+"
                // "1" and "111

                // Expressions use integral values (e.g., "13" ), single-letter variables defined in Variables, as well as + and - operators only
                // There is no need to support braces or any other operations
                // If a variable is not found in "Variables" (or if we encounter a variable with >1 letter, e.g. ab), the evaluator returns 0 (zero)
                // In case of any parsing failure, evaluator returns 0
                // Calculate("1+2+3") should return 6
                // Calculate("1+2+xy") should return 0
                // Calculate("10-2-x") when x=3 is in "Variables" should return 5
            }
        */
    }
}
