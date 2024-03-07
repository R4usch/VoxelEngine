using MyGame.Scenes;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelEngine.Components;
using VoxelEngine.Mathf;
using System.Threading;

namespace MyGame.Map
{
    public class Chunk
    {
        public static int CHUNK_SIZE = 16;

        //public List<VoxelEngine.Components.Voxel> _voxels = new List<VoxelEngine.Components.Voxel>();

        public Voxel[,] voxels = new Voxel[CHUNK_SIZE, CHUNK_SIZE];


        public Chunk(int chunkCenterX, int chunkCenterY, FastNoiseLite noise)
        {
            Console.WriteLine("Chunk Center X: " + chunkCenterX + "|Chunk Center Y: " +  chunkCenterY);

            for(int x = 0; x < CHUNK_SIZE; x++)
            {
                for(int y = 0; y < CHUNK_SIZE; y++)
                {
                    //Thread.Sleep(1000);
                    int _x = x + chunkCenterX / 2;
                    int _z = y + chunkCenterY / 2;

                    float noiseValue = noise.GetNoise(_x, _z);

                    int _y = Randomic.Next(noiseValue, -1, 16);

                    Color4 color = new(noiseValue, noiseValue, noiseValue, 1f);

                    if (World.colored_voxels)
                    {
                        switch (true)
                        {
                            case bool _ when _y <= 0: // Mar
                                color = new(0f, 0f, 1f, 1f);
                                break;
                            case bool _ when _y <= 1: // Terra
                                color = new(0.69f, 0.431f, 0f, 1f);
                                break;
                            default:                  // Grama
                                color = new(0f, 1f, 0f, 1f);
                                break;
                        }
                    }

                    Voxel voxel = new Voxel(color)
                    {
                        Position = new(_x, _y, _z)
                    };

                    int xArray = x;
                    int zArray = y;

                    Console.WriteLine(xArray + "|" + zArray);
                    voxels[xArray, zArray] = voxel; // <--- Armazenar voxels 
                }
            }
        }

        public void Destroy()
        {
            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                for (int y = 0; y < CHUNK_SIZE; y++)
                {
                    Voxel v = voxels[x, y];
                    if (v != null)
                    {
                        v.Destroy(); // Chame o método Destroy() do Voxel
                        voxels[x, y] = null; // Atribua null à posição do array
                    }
                }
            }

  
        }
    }
}
