using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngine.Utils
{
    public class Math
    {
        public static class Random 
        {
            //public static float PerlinNoise(float x, float y)
            //{

            //}
            public static float NextFloat(float min, float max)
            {
                System.Random random = new System.Random();
                double val = (random.NextDouble() * (max - min) + min);
                return (float)val;
            }
        
        }

    }
}
