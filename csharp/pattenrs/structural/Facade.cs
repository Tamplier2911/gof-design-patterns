namespace Facade
{
    // Facade: provides a simplified interface to a large body of code.
    //
    // Motivation:
    // provide simple API for complicated underlying systems
    // allows to define one point of interaction between the client and complex system
    // reduce the number of dependencies between the client and a complex system
    class Main
    {
        public static void Run()
        {
            Console.WriteLine("\nFacade");

            // Facade - IDE

            // init facade
            var ide = new IDEFacade(new TextEditor(), new Compiler(), new Runtime(), new IDEConsole());
            // init client
            var client = new Developer(ide);
            // create app
            client.CreateApplication("Hello, world!");

            // Facade - Magic Square

            // init magic square generator
            var sg = new MagicSquareGenerator(new Generator(), new Splitter(), new Verifier());
            // generate magic square
            // var ms = sg.Generate(3);
        }
    }

    // -- Complex Underlying Systems

    // ITextEditor - represents text editor interface.
    public interface ITextEditor
    {
        public string WriteText(string text);
        public string SaveText();
    }

    // TextEditor - represents text editor.
    public class TextEditor : ITextEditor
    {
        private List<string> Stack = new List<string>();
        private string Current = "";

        public string WriteText(string text)
        {
            return Current += text;
        }

        public string SaveText()
        {
            Stack.Add(Current);
            Current = "";
            return Stack.Last();
        }
    }

    // ICompiler - represents Compiler interface.
    public interface ICompiler
    {
        public string Compile(string code);
    }

    // Compiler - represents code compiler.
    public class Compiler : ICompiler
    {
        public string Compile(string code)
        {
            return $"compiled({code})";
        }
    }

    // IRuntime - represents Runtime interface.
    public interface IRuntime
    {
        public string Execute(string bin);
    }

    // Runtime - represents runtime.
    public class Runtime : IRuntime
    {
        public string Execute(string bin)
        {
            return $"executed({bin})";
        }
    }

    // IConsole - represents console interface.
    public interface IConsole
    {
        public void Output(string result);
    }

    // IDEConsole - represents console.
    public class IDEConsole : IConsole
    {
        public void Output(string result)
        {
            Console.WriteLine($"output({result})");
        }
    }

    // -- Facade

    // IDE - represents IDE interface.
    public interface IDE
    {
        public void Run(string code);
    }

    // IDEFacade - represents IDE, which lavarage complex underlying systems to provide simple API.
    public class IDEFacade : IDE
    {
        protected ITextEditor editor;
        protected ICompiler compiler;
        protected IRuntime runtime;
        protected IConsole console;

        public IDEFacade(ITextEditor editor, ICompiler compiler, IRuntime runtime, IConsole console)
        {
            this.editor = editor;
            this.compiler = compiler;
            this.runtime = runtime;
            this.console = console;
        }

        public void Run(string appCode)
        {
            // write and save app code
            editor.WriteText(appCode);
            var code = editor.SaveText();

            // compile app code
            var bin = compiler.Compile(code);

            // execute binaries
            var result = runtime.Execute(bin);

            // output result
            console.Output(result);
        }
    }

    // -- Client

    // Developer - represent client
    public class Developer
    {
        private IDE tool;
        public Developer(IDE tool)
        {
            this.tool = tool;
        }

        public void CreateApplication(string appCode)
        {
            tool.Run(appCode);
        }
    }

    // -- Magic Square Example

    // -- Complex Underlying Systems

    // IGenerator - represents slice generator interface.
    public interface IGenerator
    {
        public List<int> Generate(int count);
    }

    // Generator - generates random slice of integers.
    public class Generator : IGenerator
    {
        private static readonly Random random = new Random();

        public List<int> Generate(int count)
        {
            return Enumerable.Range(0, count)
              .Select(_ => random.Next(1, 6))
              .ToList();
        }
    }

    // ISplitter - represents splitter interface.
    public interface ISplitter
    {
        public List<List<int>> Split(List<List<int>> array);
    }

    // Splitter - splits square matrix into individual parallel and diagonal pieces.
    public class Splitter : ISplitter
    {
        public List<List<int>> Split(List<List<int>> array)
        {
            var result = new List<List<int>>();

            var rowCount = array.Count;
            var colCount = array[0].Count;

            // get the rows
            for (int r = 0; r < rowCount; ++r)
            {
                var theRow = new List<int>();
                for (int c = 0; c < colCount; ++c)
                    theRow.Add(array[r][c]);
                result.Add(theRow);
            }

            // get the columns
            for (int c = 0; c < colCount; ++c)
            {
                var theCol = new List<int>();
                for (int r = 0; r < rowCount; ++r)
                    theCol.Add(array[r][c]);
                result.Add(theCol);
            }

            // now the diagonals
            var diag1 = new List<int>();
            var diag2 = new List<int>();
            for (int c = 0; c < colCount; ++c)
            {
                for (int r = 0; r < rowCount; ++r)
                {
                    if (c == r)
                        diag1.Add(array[r][c]);
                    var r2 = rowCount - r - 1;
                    if (c == r2)
                        diag2.Add(array[r][c]);
                }
            }

            result.Add(diag1);
            result.Add(diag2);

            return result;
        }
    }

    // IVerifier - represents verifier interface.
    public interface IVerifier
    {
        public bool Verify(List<List<int>> array);
    }

    // Verifier - verifies that it is indeed a maginc square.
    public class Verifier : IVerifier
    {
        public bool Verify(List<List<int>> array)
        {
            if (!array.Any()) return false;

            var expected = array.First().Sum();

            return array.All(t => t.Sum() == expected);
        }
    }

    // -- Facade

    // MagicSquareGenerator - represents facade, that leverage underlying systems to explode simple API.
    public class MagicSquareGenerator
    {
        private IGenerator generator;
        private ISplitter splitter;
        private IVerifier verifier;

        public MagicSquareGenerator(IGenerator generator, ISplitter splitter, IVerifier verifier)
        {
            this.generator = generator;
            this.splitter = splitter;
            this.verifier = verifier;
        }

        public List<List<int>> Generate(int size)
        {
            // generate data
            List<List<int>> lists = new List<List<int>>();
            for (int i = 0; i < size; i++)
            {
                lists.Add(generator.Generate(size));
            }

            // split data
            var split = splitter.Split(lists);

            // verify data
            if (!verifier.Verify(split))
            {
                return Generate(size);
            }

            return lists;
        }
    }
}