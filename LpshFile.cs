using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpshTool
{
    public class LpshFile
    {
        public GrHeader header = new GrHeader();
        public List<GrMetadataEntry> MetadataEntries = new List<GrMetadataEntry>();
        public LightProbeSHCoefficients Coefficients = new LightProbeSHCoefficients();
        public void Read(BinaryReader reader)
        {
            header.Read(reader);

            reader.BaseStream.Position = header.MetadataEntriesOffset;

            uint entryStartPos = (uint)reader.BaseStream.Position;
            GrMetadataEntry firstEntry = new GrMetadataEntry();
            firstEntry.Read(reader);
            MetadataEntries.Add(firstEntry);
            var nextEntryOffset = firstEntry.NextEntryOffset;
            Console.WriteLine($"nextEntryOffset:{nextEntryOffset}");
            while (nextEntryOffset != 0)
            {
                entryStartPos += (uint)nextEntryOffset;
                reader.BaseStream.Position = entryStartPos;
                GrMetadataEntry nextEntry = new GrMetadataEntry();
                nextEntry.Read(reader);
                MetadataEntries.Add(nextEntry);
                nextEntryOffset = nextEntry.NextEntryOffset;
                Console.WriteLine($"nextEntryOffset:{nextEntryOffset}");
            }

            var LightProbeSHCoefficientsMetadata = MetadataEntries.First();
            uint NumDiv = 0;
            uint NumLightProbes = 0;
            foreach (GrMetadataEntryParameter param in LightProbeSHCoefficientsMetadata.Params)
            {
                Console.WriteLine($"{param.Name.String}");
                if (param.Name.String.Equals("numLightProbes"))
                {
                    NumLightProbes = param.ValueUint;
                }
                else if (param.Name.String.Equals("numDiv"))
                {
                    NumDiv = param.ValueUint;
                }
            }

            reader.BaseStream.Position = header.MetadataEntriesOffset + LightProbeSHCoefficientsMetadata.DataOffset;

            Coefficients.Read(reader, NumDiv, NumLightProbes);

        }
    }
}
