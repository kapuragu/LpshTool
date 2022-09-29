using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpshTool
{
    public class MetadataString
    {
        public uint Hash;
        public string String;
        public void Read(BinaryReader reader)
        {
            Hash = reader.ReadUInt32();
            var offset = reader.ReadUInt32();
            var continueOffset = reader.BaseStream.Position;
            reader.BaseStream.Position = offset;
            String = reader.ReadCString();
            reader.BaseStream.Position = continueOffset;
        }
    }
}
