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
        ENTRY_PARAM_TYPE Type;
        public short NextParamOffset; //sizeof(GrMetadataEntryParameter)
        public MetadataString Name;
        public uint ValueUint;
        public float ValueFloat;
        public void Read(BinaryReader reader)
        {
            Type = (ENTRY_PARAM_TYPE)reader.ReadUInt16();
            NextParamOffset = reader.ReadInt16();
            Name.Read(reader);
            switch (Type)
            {
                case ENTRY_PARAM_TYPE.GR_ENTRY_PARAM_TYPE_UINT:
                    ValueUint = reader.ReadUInt32();
                    break;
                case ENTRY_PARAM_TYPE.ENTRY_PARAM_TYPE_FLOAT:
                    ValueFloat = reader.ReadSingle();
                    break;
                default:
                    Console.Write("Unknown parameter type.");
                    break;
            }
        }
    }
}
