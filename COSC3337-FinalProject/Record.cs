using System;
using System.Collections.Generic;
using System.Text;

namespace COSC3337_FinalProject
{
    public class Record //50 chars long
    {
        public const int recordSize = 50;
        public char[] author = new char[16];
        public char[] title = new char[24];
        public char[] isbn = new char[10];

        public Record(string author, string title, string isbn) //Create new record
        {
            this.author = author.ToCharArray(0, 16);
            this.title = title.ToCharArray(0, 24);
            this.isbn = isbn.ToCharArray(0, 10);
        }

        public Record(string record) //Create record from string (from file)
        {
            if (record.Length == recordSize)
            {
                this.author = record.ToCharArray(0, 16);
                title = record.ToCharArray(16, 24);
                isbn = record.ToCharArray(50, 10);
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
