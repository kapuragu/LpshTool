using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpshTool
{
    public enum GR_FILE_TYPE : uint
    {
	    GR_FILE_TYPE_ATSH = 1,
	    GR_FILE_TYPE_OBRB = 2,
	    GR_FILE_TYPE_HTRE_OBR = 3,
	    GR_FILE_TYPE_LPSH_TRE2_HTRE = 4,
    };
    public class GrHeader
    {
        public GR_FILE_TYPE Type;
        public uint MetadataEntriesOffset;
        public uint FileSize;
        public MetadataString Name = new MetadataString();
        public void Read(BinaryReader reader)
        {
            Console.WriteLine($"@{reader.BaseStream.Position} GrHeader:");
            Type = (GR_FILE_TYPE)reader.ReadUInt32(); Console.WriteLine($"type={Type}");
            MetadataEntriesOffset = reader.ReadUInt32(); Console.WriteLine($"MetadataEntriesOffset={MetadataEntriesOffset}");
            FileSize = reader.ReadUInt32(); Console.WriteLine($"FileSize={FileSize}");
            
            Name.Hash = reader.ReadUInt32(); var offset = reader.ReadUInt32();
            var continueOffset = reader.BaseStream.Position;
            reader.BaseStream.Position = offset; Name.String = reader.ReadCString();
            reader.BaseStream.Position = continueOffset;
            
            Console.WriteLine($"Name={Name.Hash}|{Name.String}");
        }
    }
}
