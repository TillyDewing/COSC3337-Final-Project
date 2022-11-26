using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace COSC3337_FinalProject
{
    class BookRecords
    {
        public const int tableSize = 1000;

        //File locations
        private string recLoc;
        private string isbnIndexLoc;
        private string titleIndexLoc;
        private string availLoc;

        //Indicies (sorted lists)
        FileIndex isbnIndex = new FileIndex();
        FileIndex titleIndex = new FileIndex();

        //Record file stream
        private FileStream recStream; //stores records by index

        //Avail index List
        private List<int> availList = new List<int>(); //List of ints representing deleted positions

        //Last used index(for getting empty record)
        public int lastIndex = 0;

        public BookRecords(string recordName)
        {
            //Get file names
            recLoc = recordName + ".txt";
            isbnIndexLoc = recordName + "-isbnIndex.txt";
            titleIndexLoc = recordName + "-titleIndex.txt";
            availLoc = recordName + "-avail.txt";
            //Open Files
            recStream = File.Open(recLoc, FileMode.OpenOrCreate);
            //Load indicies
            titleIndex.LoadIndexFromFile(titleIndexLoc);
            isbnIndex.LoadIndexFromFile(isbnIndexLoc);
            //Load avail List
            LoadAvailList();
        }

        public void SaveChanges() //Save current index changes in memory to file
        {
            isbnIndex.SaveIndexToFile(isbnIndexLoc);
            titleIndex.SaveIndexToFile(titleIndexLoc);
            SaveAvailList();
        }

        public void Add(Record record) //Add a new record
        {
            int index = 0; 
            if (availList.Count > 0) //Check if Avail has record
            {
                index = availList[0];//Get first avail if so
                availList.RemoveAt(0);//Remove from avail list
            }
            else 
            {
                index = lastIndex; //start from last index
                while (RecordExists(index)) //Get empty index
                {
                    index++;
                }
            }

            //Write Record
            Set(index, record);

            //Add indicies
            titleIndex.AddIndex(new Index(record.title, index));
            isbnIndex.AddIndex(new Index(record.isbn, index));

            SaveChanges();
        }

        public bool Delete(int index)
        {
            Record delRec;
            if (GetRecord(index, out delRec)) //Check record exists at index
            {
                Record blankRec = new Record("EMPTY", "EMPTY", "0000000000"); //Empty Record

                //Write blank record
                Set(index, blankRec);
                //Remove index
                isbnIndex.DeleteIndex(delRec.isbn);
                titleIndex.DeleteIndex(delRec.title);
                //Add index to Avail list
                availList.Add(index);
                //save changes to index files
                SaveChanges();
                return true;
            }
            return false;
        }

        public bool Update(int index, Record record)
        {
            Record upRec;
            if (GetRecord(index, out upRec))
            {
                Set(index, record);

                //Delete old index
                isbnIndex.DeleteIndex(upRec.isbn);
                titleIndex.DeleteIndex(upRec.title);

                //Add New Index
                titleIndex.AddIndex(new Index(record.title, index));
                isbnIndex.AddIndex(new Index(record.isbn, index));

                SaveChanges();

                return true;
            }
            return false;
        }

        public int SearchByTitle(string title, out Record record) //Returns index of record and outs record
        {
            int index = titleIndex.Search(title.PadRight(24));
            if (index > -1)
            {
                if (GetRecord(index, out record)) //if record found at index
                {
                    return index;   
                }
            }
            record = null;
            return -1;
        }

        public int SearchByISBN(string isbn, out Record record) //Returns index of record and outs record
        {
            int index = isbnIndex.Search(isbn.PadLeft(10, '0'));
            if (index > -1)
            {
                if (GetRecord(index, out record)) //if record found at index
                {
                    return index;
                }
            }
            record = null;
            return -1;
        }

        private bool RecordExists(int index) //Returns true if vailid record exists at given index
        {
            Record rec;
            return GetRecord(index, out rec);
        }

        private bool GetRecord(int index, out Record rec) //Returns true if record is valid outs record
        {
            recStream.Seek((Record.recordSize * index), SeekOrigin.Begin);
            //StreamReader reader = new StreamReader(stream);

            byte[] buffer = new byte[Record.recordSize];

            int i = recStream.Read(buffer, 0, Record.recordSize);

            if (i == 0)
            {
                rec = null;
                return false;
            }
            string recStr = Encoding.ASCII.GetString(buffer);

            if (Record.TryParse(recStr, out rec))
            {
                if (rec.isbn != "0000000000") //if record is not empty
                {
                    return true;
                }
            }

            return false;
        }

        private void LoadAvailList() 
        {
            try
            {
                availList = new List<int>();
                string[] lines = File.ReadAllLines(availLoc);
                foreach (string line in lines)
                {
                    availList.Add(int.Parse(line));
                }
            }
            catch
            {
                Console.WriteLine("AVAIL list IO ERROR");
            }
            
        }

        private void SaveAvailList()
        {
            string text = "";
            foreach (int i in availList)
            {
                text += i + '\n';
            }
            File.WriteAllText(availLoc, text);
        }

        private void Set(int index, Record record) //Set record at index
        {
            recStream.Seek(Record.recordSize * index, SeekOrigin.Begin);
            byte[] data = Encoding.ASCII.GetBytes(record.ToString());
            recStream.Write(data, 0, Record.recordSize);
        }
    }
}
