using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngine.Components
{
    public static class VoxelConstants
    {
        public static float[] VOXEL_CUBE_VERTICES =  
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
            
    }
}
