namespace Proxy
{
    // Proxy: provides a placeholder for another object to control access, reduce cost, and reduce complexity.
    //
    // Motivation:
    // controll access to expensive resource - virtual proxy
    // restrict access to the object depending on the policies of calling object - protection proxy
    // count references to an object or ensure thread-safe work with a real object - smart references
    // lazy loading, caching, logging
    //
    class Main
    {
        public static void Run()
        {
            Console.WriteLine("\nProxy\n");

            // init subject
            var book = new Book("Plague", new List<string>() {
                "The unusual events described in this chronicle occurred in 194- at Oran." ,
                "Everyone agreed that, considering their somewhat extraordinary character, they were out of place there.",
                "For its ordinariness is what strikes one first about the town of Oran, which is merely a large "+
                "French port on the Algerian coast, headquarters of the Prefect of a French Department.",
                "The town itself, let us admit, is ugly. It has a smug, placid air and you need time to discover "+
                "what it is that makes it different from so many business centers in other parts of the world." ,
                "How to conjure up a picture, for instance, of a town without pigeons, without any trees or gardens, "+
                "where you never hear the beat of wings or the rustle of leaves, a thoroughly negative place, in short?",
            });

            // init client
            var u = new User("Albert", true);

            // use subject by the client
            u.ReadBook(book);

            // init logger proxy
            var lbp = new LoggerBookProxy(book); // logs current page and date
            // init caching proxy
            var cbp = new CacheBookProxy(lbp);   // caching pages
            // init protection proxy
            var bpp = new PreviewBookProxy(cbp); // in preview mode only 3 pages are readable

            // use subject through proxies by a client
            u.ReadBook(bpp);

        }
    }

    // -- Subject

    // IBook - represents abstraction (subject interface).
    public interface IBook
    {
        public string GetBookTitle();
        public string ReadPage(int page);
    }

    // -- Real Subject

    // Book - represents real subject.
    public class Book : IBook
    {
        private string title;
        private List<string> pages;
        public Book(string title, List<string> pages)
        {
            this.title = title;
            this.pages = pages;
        }

        public string GetBookTitle() => title;
        public string ReadPage(int page)
        {
            if (page > pages.Count - 1) return "";
            return pages[page];
        }

    }

    // -- Proxy

    // LoggerBookProxy - represents logger proxy.
    public class LoggerBookProxy : IBook
    {
        private IBook book;
        public LoggerBookProxy(IBook book) { this.book = book; }

        public string GetBookTitle()
        {
            // log title
            Console.WriteLine($"{DateTime.Now} - book title: {book.GetBookTitle()}");
            return book.GetBookTitle();
        }

        public string ReadPage(int page)
        {
            // log page
            Console.WriteLine($"{DateTime.Now} - reading book page {page}");
            return book.ReadPage(page);
        }
    }

    // CacheBookProxy - represents virtual proxy.
    public class CacheBookProxy : IBook
    {
        private IBook book;
        private Dictionary<int, string> cache;
        public CacheBookProxy(IBook book)
        {
            this.book = book;
            this.cache = new Dictionary<int, string>();
        }

        public string GetBookTitle()
        {
            return book.GetBookTitle();
        }

        public string ReadPage(int page)
        {
            // get page from cache
            if (cache.ContainsKey(page))
            {
                Console.WriteLine("Retrieved page form cache.");
                return cache[page];
            }
            // cache page
            Console.WriteLine("Saved page to cache.");
            cache[page] = book.ReadPage(page);

            return cache[page];
        }
    }

    // PreviewBookProxy - represent protection proxy.
    public class PreviewBookProxy : IBook
    {
        private IBook book;
        public PreviewBookProxy(IBook book) { this.book = book; }

        public string GetBookTitle()
        {
            return book.GetBookTitle();
        }

        public string ReadPage(int page)
        {
            // in preview mode can only access 3 book pages
            if (page > 2) return "";
            return book.ReadPage(page);
        }
    }

    // -- Client

    // User - represents user (a client that going to use proxy).
    public class User
    {
        private string name;
        private bool reader;
        public User(string name, bool reader)
        {
            this.name = name;
            this.reader = reader;
        }
        public string GetName() => name;
        public bool IsReader() => reader;
        public void ReadBook(IBook book)
        {
            for (int i = 0; ; i++)
            {
                var page = book.ReadPage(i);
                if (page == "") break;
                Console.WriteLine($"Reading: {page}");
            }
        }
    }
}