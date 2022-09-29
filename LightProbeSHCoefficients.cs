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
        public List<uint> Times;
        public void Read(BinaryReader reader, uint NumDiv, uint NumLightProbes)
        {
            Times = new List<uint>();
            for (int i = 0; i < NumDiv; i++)
            {
                Times.Add(reader.ReadUInt32());
            }

            reader.AlignStream(16);


        }
    }
}
