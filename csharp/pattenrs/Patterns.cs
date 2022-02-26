using Creational;
using Structural;
using Behavioral;

namespace Patterns
{
    ///
    /// <summary>Class <c>DesignPatterns</c> invokes all  patterns for testing purposes. </summary>
    ///
    class DesignPatterns
    {
        public static void Run()
        {
            Console.WriteLine("\nPatterns");
            CreationalPatterns.Run();
            StructuralPatterns.Run();
            BehavioralPatterns.Run();
        }
    }
}
