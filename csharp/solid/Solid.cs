namespace Solid
{
    ///
    /// <summary>Class <c>SolidPrinciples</c> invokes all SOLID principles for testing purposes.</summary>
    ///
    class SolidPrinciples
    {
        public static void Run()
        {
            Console.WriteLine("Solid");

            SingleResponsibility.Run();
            OpenClosed.Run();
            LiskovSubstitution.Run();
            InterfaceSegregation.Run();
            DependencyInversion.Run();
        }
    }
}