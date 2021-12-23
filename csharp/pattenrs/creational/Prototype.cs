namespace Patterns
{
    // Prototype: creates objects by cloning an existing object.
    //
    // Motivation:  
    // Complicated objects are not designed from scratch
    // - existing design being re-iterated
    // An existing object partially or fully constructed is a Prototype
    // We make a copy (deep clone) of the prototype and customized it
    // Cloning must be convenient
    class Prototype
    {
        public static void Run()
        {
            Console.WriteLine("Prototype");


        }
    }
}