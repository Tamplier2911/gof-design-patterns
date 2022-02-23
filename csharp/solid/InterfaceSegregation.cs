namespace Solid
{
    ///
    /// <summary>
    /// Class <c>InterfaceSegregation</c> represents interface segregation principle usecase.
    /// 1. specific interfaces are better than general purpose ones
    /// 2. classes should not implement methods they don't need
    /// </summary>
    ///
    class InterfaceSegregation
    {
        public static void Run()
        {
            Console.WriteLine("\nInterface Segregation\n");

            Document doc = new Document("File");

            // general purpose interface
            MultiFunctionDocumentPrinter mfdp = new MultiFunctionDocumentPrinter();
            mfdp.Print(doc);
            mfdp.Scan(doc);

            StandardPrinter sp = new StandardPrinter();
            sp.Print(doc);
            // sp.Scan(doc); // will fail

            // specific interface
            DocumentPrinter dp = new DocumentPrinter();
            dp.Print(doc);

            MultiFunctionDocumentPrinterEnhanced mfdpe = new MultiFunctionDocumentPrinterEnhanced();
            mfdpe.Print(doc);
            mfdpe.Scan(doc);

            // utilize delegation
            DocumentScanner ds = new DocumentScanner();
            DocumentFax df = new DocumentFax();

            MultiFunctionDocumentPrinterDelegated mfdpd = new MultiFunctionDocumentPrinterDelegated(dp, ds, df);
            mfdpd.Print(doc);
            mfdpd.Scan(doc);
        }
    }

    /// <summary>Class <c>Document</c> document entity.</summary>
    class Document
    {
        public string Name;

        public Document(string name)
        {
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
        }
    }

    /// <summary>Interface <c>IMachine</c> represents general purpose interface.</summary>
    interface IMachine<T>
    {
        void Print(T t);
        void Scan(T t);
        void Fax(T t);
    }

    /// <summary>Class <c>MultiFunctionDocumentPrinter</c> capable to perform multiple operations on documents.</summary>
    class MultiFunctionDocumentPrinter : IMachine<Document>
    {
        public void Print(Document d)
        {
            Console.WriteLine($"Print: {d.Name}");
        }
        public void Scan(Document d)
        {
            Console.WriteLine($"Scan: {d.Name}");
        }
        public void Fax(Document d)
        {
            Console.WriteLine($"Fax: {d.Name}");
        }
    }

    /// <summary>Class <c>StandardPrinter</c> capable to perform printing operations on documents.</summary>
    class StandardPrinter : IMachine<Document>
    {
        public void Print(Document d)
        {
            Console.WriteLine($"Printed: {d.Name}");
        }

        // StandardPrinter have to implement methods that useless for its purpose in order to implement IMachine interface.
        public void Scan(Document d)
        {
            throw new NotImplementedException();
        }
        public void Fax(Document d)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>Interface <c>IPrinter</c> represents specific interface.</summary>
    interface IPrinter<T>
    {
        void Print(T t);
    }

    /// <summary>Interface <c>IScanner</c> represents specific interface.</summary>
    interface IScanner<T>
    {
        void Scan(T t);
    }

    /// <summary>Interface <c>IFax</c> represents specific interface.</summary>
    interface IFax<T>
    {
        void Fax(T t);
    }

    /// <summary>Interface <c>IMultiFunctionPrinter</c> represents combinator interface.</summary>
    interface IMultiFunctionPrinter<T> : IPrinter<T>, IScanner<T>, IFax<T> { }

    /// <summary>Class <c>DocumentPrinter</c> capable to perform printing operations on documents.</summary>
    class DocumentPrinter : IPrinter<Document>
    {
        public void Print(Document d)
        {
            Console.WriteLine($"Print: {d.Name}");
        }
    }

    /// <summary>Class <c>DocumentScanner</c> capable to perform scanning operations on documents.</summary>
    class DocumentScanner : IScanner<Document>
    {
        public void Scan(Document d)
        {
            Console.WriteLine($"Scan: {d.Name}");
        }
    }

    /// <summary>Class <c>DocumentFax</c> capable to perform fax operations on documents.</summary>
    class DocumentFax : IFax<Document>
    {
        public void Fax(Document d)
        {
            Console.WriteLine($"Fax: {d.Name}");
        }
    }

    /// <summary>Class <c>MultiFunctionDocumentPrinterEnhanced</c> capable to perform multiple operations on documents.</summary>
    class MultiFunctionDocumentPrinterEnhanced : IMultiFunctionPrinter<Document>
    {
        public void Print(Document d)
        {
            Console.WriteLine($"Print: {d.Name}");
        }
        public void Scan(Document d)
        {
            Console.WriteLine($"Scan: {d.Name}");
        }
        public void Fax(Document d)
        {
            Console.WriteLine($"Fax: {d.Name}");
        }
    }

    /// <summary>Class <c>MultiFunctionDocumentPrinterDelegated</c> capable to delegate certain functionalities to other classes (Decorator).</summary>
    class MultiFunctionDocumentPrinterDelegated : IMultiFunctionPrinter<Document>
    {
        private DocumentPrinter printer;
        private DocumentScanner scanner;
        private DocumentFax fax;

        public MultiFunctionDocumentPrinterDelegated(DocumentPrinter printer, DocumentScanner scanner, DocumentFax fax)
        {
            this.printer = printer ?? throw new ArgumentNullException(paramName: nameof(printer));
            this.scanner = scanner ?? throw new ArgumentNullException(paramName: nameof(scanner));
            this.fax = fax ?? throw new ArgumentNullException(paramName: nameof(fax));
        }

        public void Print(Document d)
        {
            printer.Print(d);
        }

        public void Scan(Document d)
        {
            scanner.Scan(d);
        }

        public void Fax(Document d)
        {
            fax.Fax(d);
        }
    }
}