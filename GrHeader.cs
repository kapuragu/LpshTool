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
        public uint MetadataEntriesOffset;
        public uint FileSize;
        public MetadataString Name;
        public void Read(BinaryReader reader)
        {
            GR_FILE_TYPE type = (GR_FILE_TYPE)reader.ReadUInt32();
            MetadataEntriesOffset = reader.ReadUInt32();
            FileSize = reader.ReadUInt32();
            Name.Read(reader);
        }
    }
}
