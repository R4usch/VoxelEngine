using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngine.Mathf
{
    public static class Random
    {
        static System.Random random = new System.Random();
        public static float NextFloat(float min, float max)
        {

            double val = random.NextDouble() * (max - min) + min;
            return (float)val;
        }

        

    }
}
