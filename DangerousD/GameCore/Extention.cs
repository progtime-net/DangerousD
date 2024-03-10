using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangerousD.GameCore
{
    public static class Extention
    { 
        public static int GetCoordinateDefinedRandomNumber(int input, int seed = 0)
        {
            const long randMax = 4294967296;
            input = (214013 + seed) * input + 2531011 + seed;
            input ^= input >> 15;
            return (int)(input % randMax);
        }
    }
}
