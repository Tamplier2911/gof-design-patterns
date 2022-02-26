namespace Solid
{
    ///
    /// <summary>
    /// Class <c>LiskovSubstitution</c> represents Liskov Substitution principle usecase.
    /// "Let (x) be a property provable about objects x of type T.
    /// Then (y) should be true for objects y of type S where S is a subtype of T."
    /// </summary>
    ///
    class LiskovSubstitution
    {
        static public int GetArea(Rectangle r) => r.Width * r.Height;

        public static void Run()
        {
            Console.WriteLine("\nLiskov Substitution\n");

            Rectangle rec = new Rectangle(5, 5);
            Console.WriteLine(rec);
            Console.WriteLine(GetArea(rec));

            Rectangle sqr = new Square();
            sqr.Width = 5; // set only width
            Console.WriteLine(sqr);
            Console.WriteLine(GetArea(sqr));
        }
    }

    /// <summary>Class <c>Rectangle</c> represents basic rectangle.</summary>
    class Rectangle
    {

        // using 'virtual' keyword to utilize polymorphism
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }

        public Rectangle() { }

        public Rectangle(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
        }
    }

    /// <summary>Class <c>Square</c> inherit from Reactangle.</summary>
    class Square : Rectangle
    {

        // using 'new' keyword would violate Liskov substitution principle
        // using 'override' keyword instead helps us to satisfy this principle

        public override int Width
        {
            set { base.Width = base.Height = value; }
        }

        public override int Height
        {
            set { base.Height = base.Width = value; }
        }
    }
}