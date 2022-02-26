namespace Creational
{
    ///
    /// <summary>
    /// Class <c>CreationalPatterns</c> invokes all Creational patterns for testing purposes.
    /// Creational patterns: deal with creation (construction of objects)
    /// explicit (constructor) vs implicit (DI, reflection, etc...)
    /// wholesale (single statement) vs piecewise (step-by-step)
    /// </summary>
    ///
    class CreationalPatterns
    {
        public static void Run()
        {
            Console.WriteLine("\nCreational");

            Builder.Main.Run();
            Factories.Main.Run();
            Prototype.Main.Run();
            Singleton.Main.Run();
        }
    }
}
