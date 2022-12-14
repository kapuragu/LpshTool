using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpshTool
{
    public static class Extensions
    {
        public static float ParseFloatRoundtrip(string text)
        {
            if (text == "-0")
            {
                return -0f;
            }

            return float.Parse(text, CultureInfo.InvariantCulture);
        }
        public static void ReadZeroes(this BinaryReader reader, int count)
        {
            byte[] zeroes = reader.ReadBytes(count);
            foreach (byte zero in zeroes)
            {
                if (zero != 0)
                {
                    Console.WriteLine($"Padding @{reader.BaseStream.Position} isn't zero!!!");
                    //throw new Exception();
                    Console.WriteLine($"@{reader.BaseStream.Position}!=0");
                }
            }
        } //WriteZeroes
        public static void WriteZeroes(this BinaryWriter writer, int count)
        {
            byte[] array = new byte[count];

            writer.Write(array);
        } //WriteZeroes
        public static string ReadCString(this BinaryReader reader)
        {
            var chars = new List<char>();
            var @char = Convert.ToChar(reader.ReadByte());
            while (@char != '\0')
            {
                chars.Add(@char);
                @char = Convert.ToChar(reader.ReadByte());
            }

            return new string(chars.ToArray());
        }
        public static void WriteCString(this BinaryWriter writer, string iString)
        {
            char[] stringChars = iString.ToCharArray();
            foreach (var chara in stringChars)
                writer.Write(chara);
            writer.Write((byte)0);
        }
        public static void AlignStream(this BinaryReader reader, byte div)
        {
            long pos = reader.BaseStream.Position;
            if (pos % div != 0)
                reader.BaseStream.Position += div - pos % div;
        }
        public static void AlignStream(this BinaryWriter writer, byte div)
        {
            long pos = writer.BaseStream.Position;
            if (pos % div != 0)
                writer.WriteZeroes((int)(div - pos % div));
        }
    }
}
