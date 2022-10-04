using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpshTool
{
    public class ShCoefficients
    {
        public Half r;
        public Half g;
        public Half b;
        public Half skyVisibility;
        public void Read(BinaryReader reader)
        {
            r = Half.ToHalf(reader.ReadUInt16());
            g = Half.ToHalf(reader.ReadUInt16());
            b = Half.ToHalf(reader.ReadUInt16());
            skyVisibility = Half.ToHalf(reader.ReadUInt16());
            //Console.WriteLine($"({r},{g},{b},{skyVisibility})");
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Half.GetBytes(r));
            writer.Write(Half.GetBytes(g));
            writer.Write(Half.GetBytes(b));
            writer.Write(Half.GetBytes(skyVisibility));
        }
    }
}
