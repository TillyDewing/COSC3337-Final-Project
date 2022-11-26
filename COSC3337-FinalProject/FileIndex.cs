using System;
using System.Collections.Generic;
using System.IO;

namespace COSC3337_FinalProject
{
    public class FileIndex //Sorted list of keys and index used to refrence index in master record file.
    {
        private List<Index> recordIndices = new List<Index>();

        public void LoadIndexFromFile(string fileLoc)
        {
            recordIndices = new List<Index>();
            try
            {
                string[] recs = File.ReadAllLines(fileLoc);

                foreach (string str in recs)
                {
                    Index index;
                    if (Index.TryParse(str, out index))
                    {
                        recordIndices.Add(index);
                    }
                }
            }
            catch
            {
                Console.WriteLine("Index File Error");
            }
        }

        public void SaveIndexToFile(string fileLoc)
        {
            string recordText = "";
            foreach (Index index in recordIndices)
            {
                recordText += index.ToString() + '\n';
            }
            File.WriteAllText(fileLoc, recordText);
        }

        public int Search(string key) //Returns index in records file if found (returns -1 if not found)
        {
            int low = 0, high = recordIndices.Count - 1;

            while (low <= high)
            {
                int mid = (low + high) / 2;

                switch (string.Compare(recordIndices[mid].key, key))
                {
                    case -1: //<
                        low = mid + 1;
                        break;
                    case 0: //==
                        return recordIndices[mid].index;
                    case 1: //>
                        high = mid - 1;
                        break;
                }
            }

            return -1; //Not Found
        }

        public bool AddIndex(Index index) //inserts in alphabetical order
        {
            int low = 0, high = recordIndices.Count - 1;

            while (low <= high)
            {
                int mid = (low + high) / 2;

                switch (string.Compare(recordIndices[mid].key, index.key))
                {
                    case -1: //<
                        low = mid + 1;
                        break;
                    case 0: //==
                        return false; //Already in list
                    case 1: //>
                        high = mid - 1;
                        break;
                }
            }

            recordIndices.Insert(low, index);
            return true; 
        }

        public bool DeleteIndex(string key)
        {
            int low = 0, high = recordIndices.Count - 1;

            while (low <= high)
            {
                int mid = (low + high) / 2;

                switch (string.Compare(recordIndices[mid].key, key))
                {
                    case -1: //<
                        low = mid + 1;
                        break;
                    case 0: //==
                        recordIndices.RemoveAt(mid);
                        return true;
                    case 1: //>
                        high = mid - 1;
                        break;
                }
            }

            return false; //Not Found
        }
    }

    public struct Index
    {
        public string key;
        public int index;

        //object methods:
        public Index(string key, int index)
        {
            this.key = key;
            this.index = index;
        }

        public Index(string str)
        {
            string[] split = str.Split('|');
            key = split[0];
            index = int.Parse(split[1]);
        }

        public override string ToString()
        {
            return key + '|' + index;
        }

        //Static Methods:
        public static bool TryParse(string str, out Index record)
        {
            try
            {
                record = new Index(str);
                return true;
            }
            catch
            {
                record = new Index();
                return false;
            }
        }

    }

}
