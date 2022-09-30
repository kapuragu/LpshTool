using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpshTool
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (string arg in args)
            {
                if (File.Exists(arg))
                {
                    ReadFile(arg);
                }
            }
            Console.ReadLine();
        }
        static void ReadFile(string path)
        {
            using (BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open)))
            {
                LpshFile lpsh = new LpshFile();
                lpsh.Read(reader);
            }
        }
    }
}
