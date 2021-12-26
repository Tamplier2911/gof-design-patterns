namespace Patterns
{
    // Creational patterns: deal with creation (construction of objects)
    // explicit (constructor) vs implicit (DI, reflection, etc...)
    // wholesale (single statement) vs piecewise (step-by-step)
    class CreationalPatterns
    {
        public static void Run()
        {
            Builder.Main.Run();
            Factories.Main.Run();
            Prototype.Main.Run();
        }
    }

    // Structural patterns: concerned about structure (e.g class members)
    // wrappers that mimic underlying class' interface
    // stress the importance of good API design
    class StructuralPatterns
    {
        public static void Run()
        {

        }
    }

    // Behavioral patterns: all different no certain theme
    class BehavioralPatterns
    {
        public static void Run()
        {

        }
    }
}
