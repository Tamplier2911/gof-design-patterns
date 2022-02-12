namespace Solid
{
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