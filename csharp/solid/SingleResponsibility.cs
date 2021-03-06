namespace Solid
{
    ///
    /// <summary>
    /// Class <c>SingleResponsibility</c> represents Single Responsibility principle usecase.
    /// 1. module, class or function should have responsibility over a single part of that program's functionality and it should encapsulate that part
    /// 2. all of that module, class or function's services should be narrowly aligned with that responsibility
    /// 3. separation of concerns - different classes handling different independent problems/tasks
    /// </summary>
    ///
    class SingleResponsibility
    {
        public static void Run()
        {
            Console.WriteLine("\nSingle Responsibility\n");

            // define path
            const string _path = "./solid/files/journal_entries.txt";

            // init journal instance
            var j = new Journal();
            // append entries
            j.AddEntry("journal entry one");
            j.AddEntry("journal entry two");

            // will call toString
            Console.WriteLine("Journal Content: \n" + j);

            // init local repo instance
            var r = new LocalRepository(_path);
            // save journal entries to a file
            r.SaveTextToFile(j.ToString());
            // load journal entries from a file
            string fileContent = r.LoadTextFromFile();

            Console.WriteLine("File Content: \n" + fileContent);

            // clear content of a file
            r.Clear();
        }
    }

    /// <summary>Class <c>Journal</c> class is only concerned about managing entries.</summary>
    class Journal
    {
        private readonly List<string> entries = new List<string>();
        private static int count = 0;

        public int AddEntry(string entry)
        {
            entries.Add($"{++count}: {entry}");
            return count;
        }

        public void RemoveEntry(int index)
        {
            entries.RemoveAt(index);
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, entries);
        }

        /// <summary>Method <c>StoreEntry</c> would implement persistency, 
        /// which would add more responsibility...</summary>
    }

    /// <summary>Class <c>LocalRepository</c> concerned only storing saving or retrieving text data to/from local files.</summary>
    class LocalRepository
    {
        private string Path;
        public LocalRepository(string path)
        {
            Path = path ?? throw new ArgumentNullException(paramName: nameof(path));
        }

        public void SaveTextToFile(string contents, bool overwrite = false)
        {
            // overwrite content
            if (overwrite || !File.Exists(Path))
            {
                File.WriteAllText(Path, contents);
                return;
            }

            // append contents
            File.AppendAllText(Path, contents + "\n");
        }

        public string LoadTextFromFile()
        {
            // load content from a file
            return File.ReadAllText(Path);
        }

        public void Clear()
        {
            File.WriteAllText(Path, "");
        }
    }
}