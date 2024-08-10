using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngine.Components
{
    public static class VoxelConstants
    {
        public struct Vertice
        {
            public int x;
            public int y;
            public int z;

            public Vertice(int x, int y, int z) 
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
        }

        public static readonly float[] VOXEL_CUBE_VERTICES_2 =
        {
            // Front vertices
             0.5f,  0.5f, 0.5f,   // top right     0
             0.5f, -0.5f, 0.5f,   // bottom right  1
            -0.5f, -0.5f, 0.5f,   // bottom left   2
            -0.5f,  0.5f, 0.5f,   // top left      3
                                 
            // Back vertices
            
             0.5f,  0.5f, -0.5f,  // top right     4
             0.5f, -0.5f, -0.5f,  // bottom right  5
            -0.5f, -0.5f, -0.5f,  // bottom left   6
            -0.5f,  0.5f, -0.5f   // top left      7
        };

        public static readonly uint[] VOXEL_CUBE_INDICES =
        {
            // Right
            0, 1, 3, // first triangle
            1, 2, 3, // second triangle
            
            // Left
            4, 5, 7, // first triangle
            5, 6, 7, // second triangle

            // Top
            7, 3, 4,
            4, 0, 3,

            // Bottom
            2, 6, 5,
            2, 1, 5,

            // Back
            4, 5, 1,
            4, 0 ,1,

            // Front
            6, 7, 2,
            7, 3, 2,


        };

        public static readonly uint[] QUAD_INDICES = [
            0, 1, 3,
            1, 2, 3,

        ];

        static uint[] front =
        {
            0, 1, 3,
            1, 2, 3
        };

        static uint[] back =
        {
            4, 5, 7,
            5, 6, 7,
        };
        
        static uint[] top =
        {
            7, 3, 4,
            4, 0, 3,
        };

        static uint[] right =
        {
            4, 5, 1,
            4, 0 ,1,
        };

        static uint[] left =
        {
            6, 7, 2,
            7, 3, 2,
        };

        static uint[] bottom =
        {
            2, 6, 5,
            2, 1, 5
        };

        public static uint[] GetVoxelIndices(bool[] faces)
        {

            List<uint> retorno = new List<uint>();

            for(int i = 1; i <= faces.Length; i++)
            {
                if (faces[i - 1])
                {
                    for (int ii = 0; ii < 6; ii++)
                    { 
                        retorno.Add(VOXEL_CUBE_INDICES[(i-1) * 6 + ii]);
                    }
                }
            }



            return retorno.ToArray();
        }

        public static float[] CreateQuad(Vertice top_left, Vertice top_right, Vertice bottom_left, Vertice bottom_right, Color4 color)
        {
            //float[] retorno = {
            //    // Front vertices
            //     0.5f,  0.5f, 0.5f, color.R, color.G, color.B,  // top right     0
            //     0.5f, -0.5f, 0.5f, color.R, color.G, color.B,  // bottom right  1
            //    -0.5f, -0.5f, 0.5f, color.R, color.G, color.B,  // bottom left   2
            //    -0.5f,  0.5f, 0.5f, color.R, color.G, color.B,  // top left      3

            //    // Back vertices

            //     0.5f,  0.5f, -0.5f,color.R, color.G, color.B,  // top right     4
            //     0.5f, -0.5f, -0.5f,color.R, color.G, color.B,  // bottom right  5
            //    -0.5f, -0.5f, -0.5f,color.R, color.G, color.B,  // bottom left   6
            //    -0.5f,  0.5f, -0.5f,color.R, color.G, color.B,   // top left      7
            //};

            float[] retorno = {
                // Front vertices
                 top_right.x, top_right.y, top_right.z, color.R, color.G, color.B,  // top right     0
                 bottom_right.x, bottom_right.y, bottom_right.z, color.R, color.G, color.B,  // bottom right  1
                 bottom_left.x, bottom_left.y, bottom_left.z, color.R, color.G, color.B,  // bottom left   2
                 top_left.x, top_left.y, top_left.z, color.R, color.G, color.B,  // top left      3
            };

            return retorno;
        }

        public static float[] CombineVoxel(Vector3 firstVoxelPos, Vector3 secondVoxelPos, float R, float G, float B)
        {
            float x = secondVoxelPos.X - firstVoxelPos.X;
            float y = secondVoxelPos.Y - firstVoxelPos.Y;

            float[] retorno = {
                // Front vertices
                 x+0.5f,  0.5f, 0.5f, R, G, B,  // top right     0
                 x+0.5f, -0.5f, 0.5f, R, G, B,  // bottom right  1
                -0.5f, -0.5f, 0.5f, R, G, B,  // bottom left   2
                -0.5f,  0.5f, 0.5f, R, G, B,  // top left      3

                // Back vertices

                 x+0.5f,  0.5f, -0.5f,R, G, B,  // top right     4
                 x+0.5f, -0.5f, -0.5f,R, G, B,  // bottom right  5
                -0.5f, -0.5f, -0.5f,R, G, B,  // bottom left   6
                -0.5f,  0.5f, -0.5f,R, G, B,   // top left      7
            };

            return retorno;
        }

        public static float[] GetVoxelColored(float R, float G, float  B)
        {
            
             float[] retorno = {
                // Front vertices
                 0.5f,  0.5f, 0.5f, R, G, B,  // top right     0
                 0.5f, -0.5f, 0.5f, R, G, B,  // bottom right  1
                -0.5f, -0.5f, 0.5f, R, G, B,  // bottom left   2
                -0.5f,  0.5f, 0.5f, R, G, B,  // top left      3
                                 
                // Back vertices
            
                 0.5f,  0.5f, -0.5f,R, G, B,  // top right     4
                 0.5f, -0.5f, -0.5f,R, G, B,  // bottom right  5
                -0.5f, -0.5f, -0.5f,R, G, B,  // bottom left   6
                -0.5f,  0.5f, -0.5f,R, G, B,   // top left      7
            };

            return retorno;
        }
    }
}
