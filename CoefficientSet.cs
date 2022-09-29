using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LpshTool
{
    public class CoefficientSet
    {
        // Band   Basis
        ShCoefficients c00; //  0       0

        ShCoefficients c10; //  1      -1
        ShCoefficients c11; //  1       0 
        ShCoefficients c12; //  1       1

        ShCoefficients c20; //  2      -2
        ShCoefficients c21; //  2      -1
        ShCoefficients c22; //  2       0
        ShCoefficients c23; //  2       1
        ShCoefficients c24; //  2       2
    }
}
