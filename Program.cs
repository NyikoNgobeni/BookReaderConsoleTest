using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace BookReaderConsoleTest
{
    class Program
    {
        //For the application to be cloud based, it have to be build in .Net Core which is a cross platform and can run to multiple platform like Linux, MacOS, Windows and can be deployable in the cloud like Azure
        //.Net core support cloud, it need an API developed in .Net core to have all the searches and can intergrate either Entity Framework or MongoDB to read from Database
        // Its possible to change the application to be Async all the way to improve performance task
        //An application which is built in .Net Core is very good when it comes to performance and is fast to load

        public static void Main(string[] args)
        {
            Console.WriteLine("Counting words from the file...");
            DateTime startAt = DateTime.Now;
            TrieNode root = new(null, '?');
            Dictionary<DataReader, Thread> readers = new();

            if (args.Length == 0)
            {
                args = new string[] { "war-and-peace.txt"};
            }

            if (args.Length > 0)
            {
                //for an application to be more efficient in terms of performance, For Loop is much faster than foreach 
                foreach (string path in args)
                {
                    DataReader newReader = new(path, ref root);
                    Thread new_thread = new(newReader.ThreadRun);
                    readers.Add(newReader, new_thread);
                    new_thread.Start();
                }
            }

            foreach (Thread t in readers.Values) t.Join();

            DateTime stopAt = DateTime.Now;
            Console.WriteLine("Duration it takes to processed is {0} secs", new TimeSpan(stopAt.Ticks - startAt.Ticks).TotalSeconds);
            Console.WriteLine();
            Console.WriteLine("Top 50 commonly found words:");


           
            //Listing top 50 Words in count
            List<TrieNode> top50_nodes = new() { root, root, root, root, root, root, root, root, root, root , root, root, root, root, root, root, root, root, root, root , root, root, root, root, root, root, root, root, root, root , root, root, root, root, root, root, root, root, root, root , root, root, root, root, root, root, root, root, root, root };
            int distinctWordCount = 0;
            int totalWordCount = 0;
            root.GetTopCounts(ref top50_nodes, ref distinctWordCount, ref totalWordCount);
            top50_nodes.Reverse();

            //for an application to be more efficient in terms of performance, For Loop is much faster than foreach 
            foreach (TrieNode node in top50_nodes)
            {
                Console.WriteLine("{0} - {1} times", node.ToString(), node._wordCount);
            }

            Console.WriteLine();
            Console.WriteLine("{0} words counted", totalWordCount);
            Console.WriteLine("{0} distinct words found", distinctWordCount);
            Console.WriteLine();
            Console.WriteLine("done.");
        }
    } 
}