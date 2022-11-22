using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace COSC3337_FinalProject
{
    class BookRecords
    {
        public const int tableSize = 1000;

        private string recLoc;
        private string isbnIndexLoc;
        private string titleIndexLoc;
        private string availLoc;

        //Indicies
        FileIndex isbnIndex = new FileIndex();
        FileIndex titleIndex = new FileIndex();

        //Record file stream
        private FileStream recStream; //stores records by index

        //Avail index List
        private List<int> availList = new List<int>();

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

        public void SaveChanges()
        {
            isbnIndex.SaveIndexToFile(isbnIndexLoc);
            titleIndex.SaveIndexToFile(titleIndexLoc);
            SaveAvailList();
        }

        public void Add(Record record)
        {
            int index = 0; 
            if (availList.Count > 0)
            {
                index = availList[0];
                availList.RemoveAt(0);
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
            recStream.Seek(Record.recordSize * index, SeekOrigin.Begin);
            byte[] data = Encoding.ASCII.GetBytes(record.ToString());
            recStream.Write(data, 0, Record.recordSize);

            //Add indicies
            titleIndex.AddIndex(new Index(record.title.ToString(), index));
            isbnIndex.AddIndex(new Index(record.isbn.ToString(), index));
        }

        public bool RecordExists(int index)
        {
            Record rec;
            return GetRecord(index, out rec);
        }

        public bool GetRecord(int index, out Record rec)
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
                return true;
            }

            return false;
        }

        public void LoadAvailList()
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

        public void SaveAvailList()
        {
            string text = "";
            foreach (int i in availList)
            {
                text += i + '\n';
            }
            File.WriteAllText(availLoc, text);
        }
    }
}
