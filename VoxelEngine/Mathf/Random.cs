using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngine.Mathf
{
    public static class Randomic
    {
        static System.Random random = new System.Random();

        public static int Next(float value, int min, int max)
        {
            double val = value * (max - min) + min;
            return (int)Math.Round(val);
        }

        public static float NextFloat(float min, float max)
        {

            double val = random.NextDouble() * (max - min) + min;
            return (float)val;
        }

        public static int NextInt(int min, int max)
        {
            return random.Next() * (max - min) + min;
        }

    }
}
