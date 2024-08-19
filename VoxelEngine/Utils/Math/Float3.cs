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
    }
}
