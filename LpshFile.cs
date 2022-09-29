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
        GrHeader header;
        List<GrMetadataEntry> MetadataEntries;
        LightProbeSHCoefficients Coefficients;
        public void Read(BinaryReader reader)
        {
            header.Read(reader);

            reader.BaseStream.Position = header.MetadataEntriesOffset;

            MetadataEntries = new List<GrMetadataEntry>();
            GrMetadataEntry firstEntry = new GrMetadataEntry();
            firstEntry.Read(reader);
            MetadataEntries.Add(firstEntry);
            var nextEntryOffset = firstEntry.NextEntryOffset;
            while (nextEntryOffset != 0)
            {
                GrMetadataEntry nextEntry = new GrMetadataEntry();
                nextEntry.Read(reader);
                MetadataEntries.Add(nextEntry);
                nextEntryOffset = nextEntry.NextEntryOffset;
            }

            var LightProbeSHCoefficientsMetadata = MetadataEntries.First();
            uint NumDiv = 0;
            uint NumLightProbes = 0;
            foreach (GrMetadataEntryParameter param in LightProbeSHCoefficientsMetadata.Params)
            {
                if (param.Name.String== "numDiv")
                {
                    NumDiv = param.ValueUint;
                }
                else if (param.Name.String== "numLightProbes")
                {
                    NumLightProbes = param.ValueUint;
                }
            }

            reader.BaseStream.Position = LightProbeSHCoefficientsMetadata.DataOffset;

            Coefficients = new LightProbeSHCoefficients();
            Coefficients.Read(reader, NumDiv, NumLightProbes);

        }
    }
}
