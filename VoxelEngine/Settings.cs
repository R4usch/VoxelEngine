using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public static class Settings
    {
        public struct ChunkSize
        {
            int[] pos = new int[3];

            public readonly int x;
            public readonly int y;
            public readonly int z;

            public ChunkSize(int x, int y, int z)
            {
                pos = [x, y, z];

                this.x = x;
                this.y = y;
                this.z = z;
            }

            public int this[int index]
            {
                get 
                {
                    return pos[index];
                }
            }
        }

        public static readonly ChunkSize CHUNK_SIZE = new ChunkSize(12, 255, 12);
    }
}
