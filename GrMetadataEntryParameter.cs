using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpshTool
{
    public enum ENTRY_PARAM_TYPE : ushort
    {
	    GR_ENTRY_PARAM_TYPE_UINT = 0,
	    ENTRY_PARAM_TYPE_FLOAT = 2,
    };
    public class GrMetadataEntryParameter
    {
        public ENTRY_PARAM_TYPE Type = new ENTRY_PARAM_TYPE();
        public short NextParamOffset; //sizeof(GrMetadataEntryParameter)
        public MetadataString Name = new MetadataString();
        public uint ValueUint;
        public float ValueFloat;
        public void Read(BinaryReader reader)
        {
            Type = (ENTRY_PARAM_TYPE)reader.ReadUInt16();
            NextParamOffset = reader.ReadInt16();
            Name.Read(reader); Console.WriteLine($"Name={Name.Hash}|{Name.String}");
            switch (Type)
            {
                case ENTRY_PARAM_TYPE.GR_ENTRY_PARAM_TYPE_UINT:
                    ValueUint = reader.ReadUInt32(); Console.WriteLine($"Value={ValueUint}");
                    break;
                case ENTRY_PARAM_TYPE.ENTRY_PARAM_TYPE_FLOAT:
                    ValueFloat = reader.ReadSingle(); Console.WriteLine($"Value={ValueFloat}");
                    break;
                default:
                    Console.Write("Unknown parameter type.");
                    break;
            }
        }
    }
}
