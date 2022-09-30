using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpshTool
{
    public class LightProbeSHCoefficients
    {
        public List<uint> Times = new List<uint>();
        public List<Probe> Probes = new List<Probe>();
        public void Read(BinaryReader reader, uint NumDiv, uint NumLightProbes)
        {
            Console.WriteLine($"NumDiv: {NumDiv}");
            for (uint i = 0; i < NumDiv; i++)
            {
                var time = reader.ReadUInt32();
                Console.WriteLine($"time: {time}");
                Times.Add(time);
            }

            reader.AlignStream(16);

            for (uint i = 0; i < NumLightProbes; i++)
            {
                var probe = new Probe();
                probe.Read(reader, NumDiv);
                Probes.Add(probe);
            }
        }
    }
}
