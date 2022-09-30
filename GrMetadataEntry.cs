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
        public MetadataString Name = new MetadataString();
        public uint DataOffset; // 0 means no data
        public int NextEntryOffset; // 0 means no more entries
        public List<GrMetadataEntryParameter> Params = new List<GrMetadataEntryParameter>();
        public void Read(BinaryReader reader)
        {
            Console.WriteLine($"GrMetadataEntry");
            var startpos = reader.BaseStream.Position;

            Name.Read(reader); Console.WriteLine($"Name={Name.Hash}|{Name.String}");
            uint unknown = reader.ReadUInt32(); if (unknown != 1) { throw new Exception($"@{reader.BaseStream.Position} unknown not 1!!!"); }
            DataOffset = reader.ReadUInt32(); Console.WriteLine($"DataOffset={DataOffset}");
            uint DataSize = reader.ReadUInt32(); Console.WriteLine($"DataSize={DataSize}");
            uint EditParamOffset = reader.ReadUInt32(); Console.WriteLine($"EditParamOffset={EditParamOffset}");
            uint ConfigrationIdsOffset = reader.ReadUInt32(); Console.WriteLine($"ConfigrationIdsOffset={ConfigrationIdsOffset}");
            int PreviousEntryOffset = reader.ReadInt32(); Console.WriteLine($"PreviousEntryOffset={PreviousEntryOffset}");
            NextEntryOffset = reader.ReadInt32(); Console.WriteLine($"NextEntryOffset={NextEntryOffset}");
            uint EntryParametersOffset = reader.ReadUInt32(); Console.WriteLine($"EntryParametersOffset={EntryParametersOffset}");
            reader.BaseStream.Position += 8;

            if (EntryParametersOffset != 0)
            {
                reader.BaseStream.Position = startpos + EntryParametersOffset;
                uint paramStartPos = (uint)reader.BaseStream.Position;

                GrMetadataEntryParameter Param = new GrMetadataEntryParameter();
                Param.Read(reader);
                Params.Add(Param);
                var nextParamOffset = Param.NextParamOffset;
                while (nextParamOffset != 0)
                {
                    paramStartPos += (uint)Param.NextParamOffset;
                    reader.BaseStream.Position = paramStartPos;

                    GrMetadataEntryParameter nextParam = new GrMetadataEntryParameter();
                    nextParam.Read(reader);

                    Params.Add(nextParam);

                    nextParamOffset = nextParam.NextParamOffset;
                }
            }
        }
    }
}
