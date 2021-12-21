namespace Solid
{
    // L - Liskov Substitution (ability to substitute base-type with sub-type)
    // "Let (x) be a property provable about objects x of type T. Then (y) should be true for objects y of type S where S is a subtype of T."
    class LiskovSubstitution
    {
        static public int GetArea(Rectangle r) => r.Width * r.Height;

        public static void Run()
        {
            Rectangle rec = new Rectangle(5, 5);
            Console.WriteLine(rec);
            Console.WriteLine(GetArea(rec));

            Rectangle sqr = new Square();
            sqr.Width = 5; // set only width
            Console.WriteLine(sqr);
            Console.WriteLine(GetArea(sqr));
        }
    }

    // Rectangle represents basic rectangle.
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

    // Square inherit from Reactangle.
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