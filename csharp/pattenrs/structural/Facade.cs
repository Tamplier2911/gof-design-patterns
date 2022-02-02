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
            Console.WriteLine("\nFacade\n");

            // Facade 

            // init facade
            var ide = new IDEFacade(new TextEditor(), new Compiler(), new Runtime(), new CommandLine());
            // init client
            var client = new Developer(ide);
            // create application
            client.CreateApplication("Hello, world!");
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
            if (Stack.Count > 0)
            {
                return Current = Stack.Last() + text;
            }
            return Current = text;
        }

        public string SaveText()
        {
            Stack.Add(Current);
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
        public string Execute(string compiledCode);
    }


    // Runtime - represents runtime.
    public class Runtime : IRuntime
    {
        public string Execute(string compiledCode)
        {
            return $"executed({compiledCode})";
        }
    }

    // ICommandLine - represents command line interface.
    public interface ICommandLine
    {
        public void Output(string executionResult);
    }

    // CommandLine - represents command line.
    public class CommandLine : ICommandLine
    {
        public void Output(string executionResult)
        {
            Console.WriteLine($"output({executionResult})");
        }
    }

    // -- Facade

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
        protected ICommandLine cmd;

        public IDEFacade(ITextEditor editor, ICompiler compiler, IRuntime runtime, ICommandLine cmd)
        {
            this.editor = editor;
            this.compiler = compiler;
            this.runtime = runtime;
            this.cmd = cmd;
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
            cmd.Output(result);
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
}