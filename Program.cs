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
            bool isClone = false;
            int CloneTime = -1;
            int CloneOrigIndex = -1;
            foreach (string arg in args)
            {
                Console.WriteLine(arg);

                if (isClone)
                {
                    if (CloneTime<0)
                    {
                        int.TryParse(arg, out CloneTime);
                        Console.WriteLine($"Clone time: {CloneTime}");
                        continue;
                    }
                    else if (CloneOrigIndex<0)
                    {
                        int.TryParse(arg, out CloneOrigIndex);
                        Console.WriteLine($"Clone original index: {CloneOrigIndex}");
                        continue;
                    }
                }

                if (arg.ToLower().Equals("clone"))
                {
                    isClone = true;
                    Console.WriteLine("Start clone");
                    continue;
                }

                if (File.Exists(arg))
                {
                    string path = arg;
                    LpshFile lpsh = ReadFile(path);
                    if (isClone& CloneTime>0)
                        lpsh.FixGuantanamo((uint)CloneTime, CloneOrigIndex);
                    string newPath = Path.GetFileNameWithoutExtension(path) + "_out.lpsh";
                    WriteFile(lpsh, newPath);
                }
            }
            Console.ReadLine();
        }
        static LpshFile ReadFile(string path)
        {
            using (BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open)))
            {
                LpshFile lpsh = new LpshFile();
                lpsh.Read(reader);
                return lpsh;
            }
        }
        static void WriteFile(LpshFile lpsh, string path)
        {
            using (BinaryWriter writer = new BinaryWriter(new FileStream(path, FileMode.Create)))
            {
                lpsh.Write(writer);
            }
        }
    }
}
