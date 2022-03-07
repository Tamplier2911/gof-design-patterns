namespace Behavioral
{
    ///
    /// <summary>
    /// Class <c>BehavioralPatterns</c> invokes all Behavioral patterns for testing purposes.
    /// Behavioral patterns: are all different, and have no certain theme
    /// </summary>
    ///
    class BehavioralPatterns
    {
        public static void Run()
        {
            Console.WriteLine("\nBehavioral");

            ChainOfResponsibility.Main.Run();
            Command.Main.Run();
            Interpreter.Main.Run();
        }
    }
}
