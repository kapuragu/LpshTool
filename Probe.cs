using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpshTool
{
    public class Probe
    {
        public string Name;
        public bool enable24hSH;
        public bool enableWeatherSH;
        public bool enableRelatedLightSH;
        public bool enableOcclusionMode;
        public List<CoefficientSet> Weather0;
        public List<CoefficientSet> Weather1;
        public List<CoefficientSet> RelatedLight;
        public CoefficientSet Occlusion;
        public void Read(BinaryReader reader, uint NumDiv)
        {
            Console.WriteLine($"@{reader.BaseStream.Position}");
            int NameStringOffset = reader.ReadInt32(); 
            var continuePos = reader.BaseStream.Position;
            reader.BaseStream.Position = NameStringOffset;
            Name = reader.ReadCString(); Console.WriteLine($"{Name}");
            reader.BaseStream.Position = continuePos;

            int DataOffset = reader.ReadInt32();

            uint flags = reader.ReadUInt32();
            enable24hSH = (flags & 0b1) != 0;
            enableWeatherSH = (flags & 0b10) != 0;
            enableRelatedLightSH = (flags & 0b100) != 0;
            enableOcclusionMode = (flags & 0b1000) != 0;

            uint NextStartPos = (uint)reader.BaseStream.Position;

            reader.BaseStream.Position = DataOffset;

            Weather0 = new List<CoefficientSet>();

            if (enable24hSH)
            {
                for (int i = 0; i < NumDiv; i++)
                {
                    var coeff = new CoefficientSet();
                    coeff.Read(reader);
                    Weather0.Add(coeff);
                }
            }
            else
            {
                var coeff = new CoefficientSet();
                coeff.Read(reader);
                Weather0.Add(coeff);
            }

            Weather1 = new List<CoefficientSet>();

            if (enableWeatherSH)
            {
                if (enable24hSH)
                {
                    for (int i = 0; i < NumDiv; i++)
                    {
                        var coeff = new CoefficientSet();
                        coeff.Read(reader);
                        Weather1.Add(coeff);
                    }
                }
                else
                {
                    var coeff = new CoefficientSet();
                    coeff.Read(reader);
                    Weather1.Add(coeff);
                }
            }

            RelatedLight = new List<CoefficientSet>();

            if (enableRelatedLightSH)
            {
                for (int i = 0; i < 2; i++)
                {
                    var coeff = new CoefficientSet();
                    coeff.Read(reader);
                    RelatedLight.Add(coeff);
                }
            }

            if (enableOcclusionMode)
            {
                Occlusion = new CoefficientSet();
                Occlusion.Read(reader);
            }

            reader.BaseStream.Position = NextStartPos;
        }
    }
}
