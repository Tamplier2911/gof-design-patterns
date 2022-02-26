namespace Facade
{
    ///
    /// <summary>
    /// Class <c>Main</c> represents Facade pattern usecase.
    /// Facade: provides a simplified interface to a large body of code.
    /// Motivation:
    /// provide simple API for complicated underlying systems
    /// allows to define one point of interaction between the client and complex system
    /// reduce the number of dependencies between the client and a complex system
    /// </summary>
    ///
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

    /// <summary>Interface <c>ITextEditor</c> describes text editor.</summary>
    public interface ITextEditor
    {
        public string WriteText(string text);
        public string SaveText();
    }

    /// <summary>Class <c>TextEditor</c> represents text editor.</summary>
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

    /// <summary>Interface <c>ICompiler</c> describes compiler.</summary>
    public interface ICompiler
    {
        public string Compile(string code);
    }

    /// <summary>Class <c>Compiler</c> represents code compiler.</summary>
    public class Compiler : ICompiler
    {
        public string Compile(string code)
        {
            return $"compiled({code})";
        }
    }

    /// <summary>Interface <c>IRuntime</c> describes runtime.</summary>
    public interface IRuntime
    {
        public string Execute(string bin);
    }

    /// <summary>Class <c>Runtime</c> represents runtime.</summary>
    public class Runtime : IRuntime
    {
        public string Execute(string bin)
        {
            return $"executed({bin})";
        }
    }

    /// <summary>Interface <c>IConsole</c> describes console.</summary>
    public interface IConsole
    {
        public void Output(string result);
    }

    /// <summary>Class <c>IDEConsole</c> represents console.</summary>
    public class IDEConsole : IConsole
    {
        public void Output(string result)
        {
            Console.WriteLine($"output({result})");
        }
    }

    // -- Facade

    /// <summary>Interface <c>IDE</c> describes IDE.</summary>
    public interface IDE
    {
        public void Run(string code);
    }

    /// <summary>Class <c>IDEFacade</c> represents IDE, which lavarage complex underlying systems to provide simple API.</summary>
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

    /// <summary>Class <c>Developer</c> represents client.</summary>
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

    /// <summary>Interface <c>IGenerator</c> describes magic square generator.</summary>
    public interface IGenerator
    {
        public List<int> Generate(int count);
    }

    /// <summary>Class <c>Generator</c> represents magic square generator.</summary>
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

    /// <summary>Interface <c>ISplitter</c> describes splitter.</summary>
    public interface ISplitter
    {
        public List<List<int>> Split(List<List<int>> array);
    }

    /// <summary>Class <c>Splitter</c> represents splitter.</summary>
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

    /// <summary>Interface <c>IVerifier</c> describes verifier.</summary>
    public interface IVerifier
    {
        public bool Verify(List<List<int>> array);
    }

    /// <summary>Class <c>Verifier</c> represents verifier.</summary>
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

    /// <summary>Class <c>MagicSquareGenerator</c> represents facade, that leverage underlying systems to explode simple API.</summary>
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