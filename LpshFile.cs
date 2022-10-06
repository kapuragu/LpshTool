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
        public void Write(BinaryWriter writer)
        {
            //header:
            writer.Write((uint)header.Type);
            writer.Write(32);
            writer.WriteZeroes(4);//TODO for filesize goto 0x8
            writer.Write(981817376);
            writer.Write(80);

            writer.AlignStream(16);

            //metadata:
            writer.Write(0);//empty string hash
            writer.Write(0);//empty string offset
            writer.Write(1);//unknown
            uint offsetToDataOffset = (uint)writer.BaseStream.Position; writer.WriteZeroes(4);
            uint offsetToDataSize = (uint)writer.BaseStream.Position; writer.WriteZeroes(4);
            writer.Write(0);//EditParamOffset
            writer.Write(0);//ConfigrationIdsOffset
            writer.Write(0);//PreviousEntryOffset
            writer.Write(0);//NextEntryOffset
            writer.Write(80);//EntryParametersOffset

            writer.AlignStream(16);

            writer.WriteCString("LightProbeSHCoefficients");
            writer.AlignStream(16);


            //metadata params:
            GrMetadataEntry entry = MetadataEntries.First();
            int i = 0;
            foreach (GrMetadataEntryParameter param in entry.Params)
            {
                i++;
                writer.Write((short)param.Type);
                bool isLast = (i == entry.Params.Count);
                if (!isLast)
                    writer.Write((short)16);
                else
                    writer.Write((short)0);
                writer.Write(param.Name.Hash);
                int offsetToString = 12 + (entry.Params.Count-1) * 16;
                writer.Write(offsetToString);
                if (param.Type == ENTRY_PARAM_TYPE.GR_ENTRY_PARAM_TYPE_UINT)
                {
                    if (param.Name.String.Equals("numDiv"))
                        param.ValueUint = (uint)Coefficients.Times.Count;
                    else if (param.Name.String.Equals("numLightProbes"))
                        param.ValueUint = (uint)Coefficients.Probes.Count;
                    writer.Write(param.ValueUint);
                }
                else if (param.Type == ENTRY_PARAM_TYPE.ENTRY_PARAM_TYPE_FLOAT)
                    writer.Write(param.ValueFloat);
            }
            foreach (GrMetadataEntryParameter param in entry.Params)
            {
                writer.WriteCString(param.Name.String);
                writer.AlignStream(16);
            }

            //coefficients

            //coeffcients: times
            uint dataOffset = (uint)writer.BaseStream.Position;
            foreach (uint time in Coefficients.Times)
            {
                writer.Write(time);
            }
            writer.AlignStream(16);

            //coefficients: probe metadata
            uint[] offsetToNameStringOffsets = new uint[Coefficients.Probes.Count];
            uint[] offsetToProbeDataOffsets = new uint[Coefficients.Probes.Count];
            uint[] nameStringOffsets = new uint[Coefficients.Probes.Count];
            uint[] probeDataOffsets = new uint[Coefficients.Probes.Count];
            uint[] probeOffsets = new uint[Coefficients.Probes.Count];
            i = 0;
            foreach (Probe probe in Coefficients.Probes)
            {
                uint offsetToNameStringOffset = (uint)writer.BaseStream.Position; writer.WriteZeroes(4);
                uint offsetToProbeDataOffset = (uint)writer.BaseStream.Position; writer.WriteZeroes(4);
                offsetToNameStringOffsets[i] = offsetToNameStringOffset;
                offsetToProbeDataOffsets[i] = offsetToProbeDataOffset; 
                uint flags = 0;
                if (probe.enable24hSH)
                    flags += 0b1;
                if (probe.enableWeatherSH)
                    flags += 0b10;
                if (probe.enableRelatedLightSH)
                    flags += 0b100;
                if (probe.enableOcclusionMode)
                    flags += 0b1000;
                writer.Write(flags);
                i++;
            }
            writer.AlignStream(16);

            //coefficients: string names

            i = 0;
            foreach (Probe probe in Coefficients.Probes)
            {
                uint offsetToNameString = (uint)writer.BaseStream.Position;
                nameStringOffsets[i] = offsetToNameString;
                writer.WriteCString(probe.Name);
                i++;
            }
            writer.AlignStream(16);

            //coefficients: probes

            i = 0;
            foreach (Probe probe in Coefficients.Probes)
            {
                uint offsetToData = (uint)writer.BaseStream.Position;
                probeDataOffsets[i] = (offsetToData);
                foreach (CoefficientSet set in probe.Weather0)
                    set.Write(writer);
                if (probe.enableWeatherSH)
                    foreach (CoefficientSet set in probe.Weather1)
                        set.Write(writer);
                if (probe.enableRelatedLightSH)
                    foreach (CoefficientSet set in probe.RelatedLight)
                        set.Write(writer);
                if (probe.enableOcclusionMode)
                    probe.Occlusion.Write(writer);
                i++;
            }
            writer.AlignStream(16);
            uint fileSize = (uint)writer.BaseStream.Position;
            uint dataSize = fileSize - dataOffset;

            //END:

            //END header:
            writer.BaseStream.Position = 8;
            writer.Write(fileSize);

            //END coefficients:
            writer.BaseStream.Position = offsetToDataOffset;
            writer.Write(dataOffset-32);
            writer.BaseStream.Position = offsetToDataSize;
            writer.Write(dataSize);

            for (i = 0; i < Coefficients.Probes.Count; i++)
            {
                writer.BaseStream.Position = offsetToNameStringOffsets[i];
                writer.Write(nameStringOffsets[i]);
                writer.BaseStream.Position = offsetToProbeDataOffsets[i];
                writer.Write(probeDataOffsets[i]);
            }
        }
        public void FixGuantanamo(uint time, int origIndex)
        {
            //uint time = 84240;//23:24:00
            //uint time = 70440;//19:34:00
            time %= 86400;//24:00:00
            int newTimeIndex = Coefficients.Times.Count;
            if (!Coefficients.Times.Contains(time))
            {
                for (int i = 0; i < Coefficients.Times.Count; i++)
                {
                    if (time <= Coefficients.Times[i])
                    {
                        newTimeIndex = i;
                        break;
                    }
                }
                if (origIndex < 0)
                {
                    origIndex = newTimeIndex;
                    if (newTimeIndex > Coefficients.Times.IndexOf(Coefficients.Times.First()) + 1 & newTimeIndex < Coefficients.Times.Count)
                        if (Math.Abs(time - Coefficients.Times[newTimeIndex - 1]) < Math.Abs(Coefficients.Times[newTimeIndex] - time))
                        {
                            origIndex = newTimeIndex;
                        }
                    if (newTimeIndex == Coefficients.Times.Count)
                        if (Math.Abs(Coefficients.Times[newTimeIndex - 1] - time) > Math.Abs(86400 - time))
                        {
                            origIndex = 0;
                        }
                }
                Coefficients.Times.Insert(newTimeIndex, time);
                foreach (Probe probe in Coefficients.Probes)
                {
                    if (probe.enable24hSH)
                    {
                        CoefficientSet set = probe.Weather0[origIndex];
                        probe.Weather0.Insert(newTimeIndex, set);
                        if (probe.enableWeatherSH)
                        {
                            set = probe.Weather1[origIndex];
                            probe.Weather1.Insert(newTimeIndex, set);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Time already exists! Replacing...");
                int newIndex = Coefficients.Times.IndexOf(time);
                foreach (Probe probe in Coefficients.Probes)
                {
                    if (probe.enable24hSH)
                    {
                        CoefficientSet set = probe.Weather0[origIndex];
                        probe.Weather0[newIndex] = set;
                        if (probe.enableWeatherSH)
                        {
                            set = probe.Weather1[origIndex];
                            probe.Weather1[newIndex] = set;
                        }
                    }
                }
                return;
            }
        }
    }
}
