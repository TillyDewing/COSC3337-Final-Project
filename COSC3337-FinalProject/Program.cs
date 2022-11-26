using System;
using System.Linq;

namespace COSC3337_FinalProject
{
    class Program
    {
        static BookRecords records = new BookRecords("records");

        static void Main(string[] args) //Console interface driver class
        {
            
            Console.WriteLine("Welcome Tilly's Library System:");
            Console.WriteLine("---------------------------------------------------------------");

            bool run = true;
            while (run)
            {
                Console.WriteLine("Add (A) , Delete (D), Update (U), or Search (S) a record? (press Q to quit)");
                ConsoleKey key = Console.ReadKey().Key;
                Console.WriteLine();
                if (key == ConsoleKey.A)
                {
                    Add();
                }
                else if (key == ConsoleKey.D)
                {
                    Delete();
                }
                else if (key == ConsoleKey.U)
                {
                    Update();
                }
                else if (key == ConsoleKey.S)
                {
                    Search();
                }
                else if (key == ConsoleKey.Q)
                {
                    run = false;
                }
            }

        }

        private static void Add()
        {
            Console.WriteLine("Enter the Title of the book:");
            string title = Console.ReadLine();
            Console.WriteLine("Enter the Author of the book:");
            string author = Console.ReadLine();
            Console.WriteLine("Enter the ISBN # of the book:");
            string isbn = Console.ReadLine();
            Record record = new Record(author, title, isbn);

            records.Add(record);

        }

        //Interface functions
        private static void Delete()
        {
            Console.WriteLine("Search for record to delete....");
            int index = Search();
            Console.WriteLine("Deleting Record at: " + index);
            records.Delete(index);
        }

        private static void Update()
        {
            Console.WriteLine("Search for record to update....");
            int index = Search();

            Console.WriteLine("Enter new values:");

            Console.WriteLine("Enter the Title of the book:");
            string title = Console.ReadLine();
            Console.WriteLine("Enter the Author of the book:");
            string author = Console.ReadLine();
            Console.WriteLine("Enter the ISBN # of the book:");
            string isbn = Console.ReadLine();
            Record record = new Record(author, title, isbn);

            records.Update(index, record);
        }

        private static int Search()
        {
            Console.WriteLine("Search by ISBN (I) or Title (T)?");
            if (Console.ReadKey().Key == ConsoleKey.I)
            {
                Console.WriteLine();
                Console.WriteLine("Enter ISBN #: ");
                string isbn = Console.ReadLine();
                Record record;
                int index = records.SearchByISBN(isbn, out record);
                if (index > -1)
                {
                    Console.WriteLine("Found Record at: " + index);
                    Console.WriteLine(record.PrintRecord());
                }
                else
                {
                    Console.WriteLine("Record Not Found!");
                }
                return index;
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Enter Title: ");
                string title = Console.ReadLine();
                 
                Record record;
                int index = records.SearchByTitle(title, out record);

                if (index > -1)
                {
                    Console.WriteLine("Found Record at: " + index);
                    Console.WriteLine(record.PrintRecord());
                }
                else
                {
                    Console.WriteLine("Record Not Found!");
                }
                return index;
            }
        }
    }
}
