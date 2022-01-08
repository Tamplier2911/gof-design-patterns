namespace Solid
{
    // I - Interface Segregation 
    // 1. specific interfaces are better than general purpose ones
    // 2. classes should not implement methods they don't need
    class InterfaceSegregation
    {
        public static void Run()
        {
            Document doc = new Document("File");

            // general purpose interface
            MultiFunctionDocumentPrinter mfdp = new MultiFunctionDocumentPrinter();
            mfdp.Print(doc);
            mfdp.Scan(doc);

            StandartPrinter sp = new StandartPrinter();
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

            MultiFunctionDocumentPrinterDeligated mfdpd = new MultiFunctionDocumentPrinterDeligated(dp, ds, df);
            mfdpd.Print(doc);
            mfdpd.Scan(doc);
        }
    }

    class Document
    {
        public string Name;

        public Document(string name)
        {
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
        }
    }

    // General purpose interface.
    interface IMachine<T>
    {
        void Print(T t);
        void Scan(T t);
        void Fax(T t);
    }

    // MultiFunctionDocumentPrinter capable to perform multiple operations on documents.
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

    // StandardPrinter capable to perform printing operations on documents.
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

    // Specific interface.
    interface Printer<T>
    {
        void Print(T t);
    }

    // Specific interface.
    interface Scanner<T>
    {
        void Scan(T t);
    }

    // Specific interface.
    interface Fax<T>
    {
        void Fax(T t);
    }

    // Combinator interface.
    interface MultiFunctionPrinter<T> : Printer<T>, Scanner<T>, Fax<T> { }

    // DocumentPrinter capable to perform printing operations on documents.
    class DocumentPrinter : Printer<Document>
    {
        public void Print(Document d)
        {
            Console.WriteLine($"Print: {d.Name}");
        }
    }

    // DocumentScanner capable to perform scanning operations on documents.
    class DocumentScanner : Scanner<Document>
    {
        public void Scan(Document d)
        {
            Console.WriteLine($"Scan: {d.Name}");
        }
    }

    // DocumentFax capable to perform printing operations on documents.
    class DocumentFax : Fax<Document>
    {
        public void Fax(Document d)
        {
            Console.WriteLine($"Fax: {d.Name}");
        }
    }

    // MultiFunctionDocumentPrinterEnhanced capable to perform multiple operations on documents.
    class MultiFunctionDocumentPrinterEnhanced : MultiFunctionPrinter<Document>
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

    // MultiFunctionDocumentPrinterDelegated capable to delegate certain functionalities to other classes. (Decorator)
    class MultiFunctionDocumentPrinterDelegated : MultiFunctionPrinter<Document>
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