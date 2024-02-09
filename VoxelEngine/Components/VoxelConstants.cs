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

        public static float[] VOXEL_CUBE_VERTICES_2 =
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

        public static uint[] VOXEL_CUBE_INDICES =
        {
            // Front
            0, 1, 3, // first triangle
            1, 2, 3, // second triangle
            
            // Back
            4, 5, 7, // first triangle
            5, 6, 7, // second triangle

            // Top
            7, 3, 4,
            4, 0, 3,

            // Right
            4, 5, 1,
            4, 0 ,1,

            // Left
            6, 7, 2,
            7, 3, 2,

            // Bottom
            2, 6, 5,
            2, 1, 5

        };


        [Obsolete("Este método é descontinuado. Ele tem 36 vertices")]
        public static readonly float[] VOXEL_CUBE_VERTICES =  
            [
                // Face da frente
                // Posição          //Cores       
                    0.5f,  0.5f, 0f,   1.0f, 0.0f, 0.0f,  // Topo  direita   // Face frente begin
                    0.5f, -0.5f, 0f,   0.0f, 1.0f, 0.0f,  // Baixo direita
                -0.5f, -0.5f, 0f,   0.0f, 0.0f, 1.0f,  // Baixo esquerda // Triangle 1 End

                -0.5f, 0.5f, 0.0f,  0.0f, 1.0f, 0.0f,  // Topo esquerda  // Triangle 2 Begin
                    0.5f, 0.5f, 0.0f,  1.0f, 0.0f, 0.0f,  // Topo direita
                -0.5f,-0.5f, 0.0f,  0.0f, 0.0f, 1.0f,   // Baixo esquerda // Face frente end

                    0.5f,  0.5f, -1f,   1.0f, 0.0f, 0.0f,  // Topo  direita  // Face back begin
                    0.5f, -0.5f, -1f,   0.0f, 1.0f, 0.0f,  // Baixo direita
                -0.5f, -0.5f, -1f,   0.0f, 0.0f, 1.0f,  // Baixo esquerda // Triangle 1 End

                -0.5f, 0.5f, -1f,  0.0f, 1.0f, 0.0f,  // Topo esquerda    // Triangle 2 Begin
                    0.5f, 0.5f, -1f,  1.0f, 0.0f, 0.0f,  // Topo direita
                -0.5f,-0.5f, -1f,  0.0f, 0.0f, 1.0f,   // Baixo esquerda   // Face b

                -0.5f, 0.5f, -0f,  0.0f, 1.0f, 0.0f,  // Topo esquerda    // Triangle 2 Begin
                    0.5f, 0.5f, -1f,  1.0f, 0.0f, 0.0f,  // Topo direita
                -0.5f, 0.5f, -1f,  0.0f, 0.0f, 1.0f,   // Baixo esquerda   // Face back end

                    0.5f, 0.5f, -0f,  0.0f, 1.0f, 0.0f,  // Topo esquerda    // Triangle 2 Begin
                    0.5f, 0.5f, -1f,  1.0f, 0.0f, 0.0f,  // Topo direita
                -0.5f, 0.5f, -0f,  0.0f, 0.0f, 1.0f,   // Baixo esquerda   // Face back end

                -0.5f, -0.5f, -0f,  1.0f, 0.0f, 0.0f,  // Topo esquerda    // Triangle 2 Begin
                    0.5f, -0.5f, -1f,  0.0f, 1.0f, 0.0f,  // Topo direita
                -0.5f, -0.5f, -1f,  0.0f, 0.0f, 1.0f,   // Baixo esquerda   // Face back end

                    0.5f, -0.5f, -0f,  1.0f, 0.0f, 0.0f,  // Topo esquerda    // Triangle 2 Begin
                    0.5f, -0.5f, -1f,  0.0f, 1.0f, 0.0f,  // Topo direita
                -0.5f, -0.5f, -0f,  0.0f, 0.0f, 1.0f,   // Baixo esquerda   // Face back end

                -0.5f, -0.5f, -0f,  1.0f, 0.0f, 0.0f,  // Topo esquerda    // Triangle 2 Begin
                    -0.5f, 0.5f, -1f,  0.0f, 1.0f, 0.0f,  // Topo direita
                -0.5f, -0.5f, -1f,  0.0f, 0.0f, 1.0f,   // Baixo esquerda   // Face back end

                -0.5f, -0.5f, -0f,  1.0f, 0.0f, 0.0f,  // Topo esquerda    // Triangle 2 Begin
                -0.5f, 0.5f, -0f,   0.0f, 1.0f, 0.0f,  // Topo direita
                -0.5f, 0.5f, -1f,   0.0f, 0.0f, 1.0f,   // Baixo esquerda   // Face back
                                                    
                -0.5f, -0.5f, -1f,  1.0f, 0.0f, 0.0f,  // Topo esquerda    // Triangle 2 Begin
                    -0.5f, 0.5f, -1f,  0.0f, 1.0f, 0.0f,  // Topo direita
                -0.5f, -0.5f, -1f,  0.0f, 0.0f, 1.0f,   // Baixo esquerda   // Face back end

                0.5f, -0.5f, -0f,  1.0f, 0.0f, 0.0f,  // Topo esquerda    // Triangle 2 Begin
                0.5f, 0.5f, -0f,  0.0f, 1.0f, 0.0f,  // Topo direita
                0.5f, 0.5f, -1f,  0.0f, 0.0f, 1.0f,   // Baixo esquerda   // Face back end

                0.5f, -0.5f, -0f, 1.0f, 0.0f, 0.0f,  // Topo esquerda    // Triangle 2 Begin
                0.5f, 0.5f, -1f,  0.0f, 1.0f, 0.0f,  // Topo direita
                0.5f, -0.5f, -1f,  0.0f, 0.0f, 1.0f,   // Baixo esquerda   // Face back end
            ];
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
