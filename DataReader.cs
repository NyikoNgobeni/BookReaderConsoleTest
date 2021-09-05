using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReaderConsoleTest
{
    public class DataReader: IDataReader
    {
        static readonly int count = 1;
        private readonly TrieNode _root;
        private readonly string _path;

        public DataReader(string path, ref TrieNode root)
        {
            _root = root;
            _path = path;
        }

        //Reading data from file using Stream Reader and File Stream
        public void ThreadRun()
        {
            for (int i = 0; i < count; i++) 
            {
                using FileStream fileStream = new(_path, FileMode.Open, FileAccess.Read);
                using StreamReader streamReader = new(fileStream);
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    string[] words = line.Split(null);
                    foreach (string word in words)
                    {
                        _root.AddWord(word.Trim());
                    }
                }
            }
        }
    }
}
