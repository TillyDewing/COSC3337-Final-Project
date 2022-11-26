using System;
using System.Collections.Generic;
using System.Text;

namespace COSC3337_FinalProject
{
    public class Record //50 chars long
    {
        public const int recordSize = 50;
        public string author;
        public string title;
        public string isbn;

        public Record(string author, string title, string isbn) //Create new record
        {
            this.author = author.PadRight(16).Substring(0,16);
            this.title = title.PadRight(24).Substring(0, 24);
            this.isbn = isbn.PadLeft(10,'0').Substring(0, 10);
        }

        public Record(string record) //Create record from string (from file)
        {
            if (record.Length == recordSize)
            {
                author = record.PadRight(16).Substring(0, 16);
                title = record.PadRight(24).Substring(16, 24);
                isbn = record.PadRight(10).Substring(40, 10);
            }
        }

        public static bool TryParse(string str, out Record record)
        {
            try
            {
                record = new Record(str);
                return true;
            }
            catch
            {
                record = null;
                return false;
            }
        }

        public override string ToString()
        {
            return author.ToString() + title.ToString() + isbn.ToString();
        }

        public string PrintRecord()
        {
            return string.Format("ISBN: {0} Title: {1} Author: {2}", isbn.ToString(), title.ToString(), author.ToString());
        }
    }
}
