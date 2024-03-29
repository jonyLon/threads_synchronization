using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace threads_synchronization
{
    internal class Program
    {


        class Stat 
        {
            public string l { get; set; }
            public Stat(string l)
            {
                this.l = l;
            }
            public static int Words { get; set; }
            public static int Lines { get; set; }
            public static int Punctuation { get; set; }

            public void UpdateFields()
            {
                lock (this)
                {
                    Words += l.Split(" ").Count();
                    Lines += l.Split("\n").Count();
                    Punctuation += l.Count(char.IsPunctuation);
                }
            } 
        }





        static void Main(string[] args)
        {
            string directoryPath = @"C:\Users\user\Desktop\ШАГ\lines";
            string[] files = Directory.GetFiles(directoryPath, "*.txt");
            Thread[] threads = new Thread[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                string fileContent = File.ReadAllText(files[i]);
                Stat s = new Stat(fileContent);
                threads[i] = new Thread(s.UpdateFields);
                threads[i].Start();
            }
            for (int i = 0; i < threads.Length; ++i)
                threads[i].Join();
            Console.WriteLine("Words: {0}, Lines: {1}, Punctuation: {2}\n\n", Stat.Words, Stat.Lines, Stat.Punctuation);
        }
    }
}