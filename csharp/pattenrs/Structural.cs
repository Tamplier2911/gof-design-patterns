namespace Structural
{
    // Structural patterns: concerned about structure (e.g class members)
    // wrappers that mimic underlying class' interface
    // stress the importance of good API design
    class StructuralPatterns
    {
        public static void Run()
        {
            Console.WriteLine("\nStructural");

            Adapter.Main.Run();
            Bridge.Main.Run();
            Composite.Main.Run();
            Decorator.Main.Run();
            Facade.Main.Run();
            Flyweight.Main.Run();
            Proxy.Main.Run();
        }
    }
}