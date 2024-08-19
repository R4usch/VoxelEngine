using VoxelEngine.Utils.Math;

namespace VoxelEngine.Voxels
{
    public class Constants
    {
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


        public static readonly uint[] QUAD_INDICES =
        {
            0, 1, 3,   // first triangle
            1, 2, 3    // second triangle
        };

        // 0.5f,  0.5f, 0f, R, G, B,  // top right     0
        // 0.5f, -0.5f, 0f, R, G, B,  // bottom right  1
        //-0.5f, -0.5f, 0f, R, G, B,  // bottom left   2
        //-0.5f,  0.5f, 0f, R, G, B,  // top left      3  

        public static float[] CreateQuadOnPosition(Float3 topRight, Float3 bottomRight, Float3 bottomLeft, Float3 topLeft)
        {
            float R = 1f;
            float G = 1f;
            float B = 1f;

            float[] retorno = {
                // Front vertices
                 topRight.X + 0.5f,  topRight.Y + 0.5f, topRight.Z, R, G, B,  // top right     0
                 bottomRight.X + 0.5f, bottomRight.Y - 0.5f, bottomRight.Z, R, G, B,  // bottom right  1
                 bottomLeft.X - 0.5f, bottomLeft.Y - 0.5f, bottomLeft.Z, R, G, B,  // bottom left   2
                 topLeft.X - 0.5f,  topLeft.Y + 0.5f, topLeft.Z, R, G, B,  // top left      3            
            };

            return retorno;
        }

        public static float[] CreateQuadFixed()
        {
            float R = 1f;
            float G = 1f;
            float B = 1f;

            float[] retorno = {
                 // Front vertices
                0.5f,  0.5f, 0f, R, G, B,  // top right     0
                 0.5f, -0.5f, 0f, R, G, B,  // bottom right  1
                -0.5f, -0.5f, 0f, R, G, B,  // bottom left   2
                -0.5f,  0.5f, 0f, R, G, B,  // top left      3          
            };

            return retorno;
        }

        public static float[] CreateQuad(Float3 topRight, Float3 bottomRight, Float3 bottomLeft, Float3 topLeft)
        {
            float R = 1f;
            float G = 1f;
            float B = 1f;

            float[] retorno = {
                 // Front vertices
                 topRight.X,    topRight.Y,    topRight.Z, R, G, B,          // top right     0
                 bottomRight.X, bottomRight.Y, bottomRight.Z, R, G, B,  // bottom right  1
                 bottomLeft.X,  bottomLeft.Y,  bottomLeft.Z, R, G, B,     // bottom left   2
                 topLeft.X,     topLeft.Y,     topLeft.Z, R, G, B,             // top left      3            
            };

            return retorno;
        }

        public static float[] GetVoxelColored(float R, float G, float B)
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
