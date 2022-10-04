using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace LpshTool
{
    public class CoefficientSet
    {
                            // Band   Basis
        ShCoefficients c00 = new ShCoefficients(); //  0       0

        ShCoefficients c10 = new ShCoefficients(); //  1      -1
        ShCoefficients c11 = new ShCoefficients(); //  1       0 
        ShCoefficients c12 = new ShCoefficients(); //  1       1

        ShCoefficients c20 = new ShCoefficients(); //  2      -2
        ShCoefficients c21 = new ShCoefficients(); //  2      -1
        ShCoefficients c22 = new ShCoefficients(); //  2       0
        ShCoefficients c23 = new ShCoefficients(); //  2       1
        ShCoefficients c24 = new ShCoefficients(); //  2       2
        public void Read(BinaryReader reader)
        {
            c00.Read(reader);

            c10.Read(reader);
            c11.Read(reader);
            c12.Read(reader);

            c20.Read(reader);
            c21.Read(reader);
            c22.Read(reader);
            c23.Read(reader);
            c24.Read(reader);
        }

        public void Write(BinaryWriter writer)
        {
            c00.Write(writer);

            c10.Write(writer);
            c11.Write(writer);
            c12.Write(writer);

            c20.Write(writer);
            c21.Write(writer);
            c22.Write(writer);
            c23.Write(writer);
            c24.Write(writer);
        }
    }
}
