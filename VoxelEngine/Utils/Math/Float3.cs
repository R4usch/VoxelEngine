using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngine.Utils.Math
{
    public struct Float3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Float3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return $"{X}, {Y}, {Z}";

        }
    }
    public struct Int3
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Int3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return $"{X}, {Y}, {Z}";

        }
    }
}
