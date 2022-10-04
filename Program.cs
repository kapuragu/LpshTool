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
            bool isCloneTimeSet = false;
            int CloneTime = 0;
            bool isCloneProbeIndexSet = false;
            int CloneProbeIndex = 0;
            foreach (string arg in args)
            {
                Console.WriteLine(arg);

                if (isClone)
                {
                    if (!isCloneTimeSet)
                    {
                        int.TryParse(arg, out CloneTime);
                        Console.WriteLine($"Clone time: {CloneTime}");
                        isCloneTimeSet = true;
                        continue;
                    }
                    else if (!isCloneProbeIndexSet)
                    {
                        int.TryParse(arg, out CloneProbeIndex);
                        Console.WriteLine($"Clone probe original index: {CloneProbeIndex}");
                        isCloneProbeIndexSet = true;
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
                    if (isClone& isCloneTimeSet& isCloneProbeIndexSet)
                        lpsh.FixGuantanamo((uint)CloneTime, CloneProbeIndex);
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
