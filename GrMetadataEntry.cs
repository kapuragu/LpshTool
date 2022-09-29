using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpshTool
{
    public class GrMetadataEntry
    {
        public MetadataString Name;
        public uint Unknown; // Only nonzero in HTRE Type==4 files and OBR Type==3 files
        public uint DataOffset; // 0 means no data
        public uint DataSize; // 0 means no data
        public uint EditParamOffset; //Assert(EditParamOffset == 0);             // Only nonzero for configrationIds entry in HTRE Type==3 files
        public uint ConfigrationIdsOffset; //Assert(ConfigrationIdsOffset == 0); // exe searches for this entry, Only nonzero for editParam entry in HTRE Type==3 files
        public int PreviousEntryOffset; // 0 means no previous entries
        public int NextEntryOffset; // 0 means no more entries
        public uint EntryParametersOffset; // 0 means no params
        public List<GrMetadataEntryParameter> Params;
        public void Read(BinaryReader reader)
        {
            var startpos = reader.BaseStream.Position;

            Name.Read(reader);
            Unknown = reader.ReadUInt32();
            DataOffset = reader.ReadUInt32();
            DataSize = reader.ReadUInt32();
            EditParamOffset = reader.ReadUInt32();
            ConfigrationIdsOffset = reader.ReadUInt32();
            PreviousEntryOffset = reader.ReadInt32();
            NextEntryOffset = reader.ReadInt32();
            EntryParametersOffset = reader.ReadUInt32();
            reader.BaseStream.Position += 8;

            if (EntryParametersOffset != 0)
            {
                reader.BaseStream.Position = startpos + EntryParametersOffset;
                uint paramStartPos = (uint)reader.BaseStream.Position;

                GrMetadataEntryParameter Param = new GrMetadataEntryParameter();
                Param.Read(reader);
                Params = new List<GrMetadataEntryParameter>() { Param };
                var nextParamOffset = Param.NextParamOffset;
                while (nextParamOffset != 0)
                {
                    paramStartPos += (uint)Param.NextParamOffset;
                    reader.BaseStream.Position = paramStartPos;

                    GrMetadataEntryParameter nextParam = new GrMetadataEntryParameter();
                    nextParam.Read(reader);

                    nextParamOffset = nextParam.NextParamOffset;
                }
            }
        }
    }
}
