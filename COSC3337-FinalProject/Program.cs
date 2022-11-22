using System;
using System.Linq;

namespace COSC3337_FinalProject
{
    class Program
    {
        static void Main(string[] args)
        {
            BookRecords records = new BookRecords("records");
            records.SaveChanges();
            Console.ReadKey();

        }
    }
}
