namespace Solid
{
    class AllPrinciple
    {
        public static void Run()
        {
            SingleResponsibility.Run();
            OpenClosed.Run();
            LiskovSubstitution.Run();
            InterfaceSegregation.Run();
            DependencyInversion.Run();
        }
    }
}