using MyGame.Scenes;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelEngine.Components;
using VoxelEngine.Mathf;

namespace MyGame.Map
{
    public class Chunk
    {
        public static int CHUNK_SIZE = 16;

        //public List<VoxelEngine.Components.Voxel> _voxels = new List<VoxelEngine.Components.Voxel>();

        public Voxel[,] voxels = new Voxel[CHUNK_SIZE, CHUNK_SIZE];


        public Chunk(int chunkCenterX, int chunkCenterY, FastNoiseLite noise)
        {
            // X
            for(int _x = chunkCenterX-CHUNK_SIZE/2; _x < (chunkCenterX+ CHUNK_SIZE / 2); _x++)
            {
                for(int _y = chunkCenterY- CHUNK_SIZE / 2; _y < (chunkCenterY+ CHUNK_SIZE / 2); _y++)
                {
                    float vY = noise.GetNoise(_x, _y);
                    float Yfixed = Randomic.Next(vY, -1, 15);

                    Color4 color_before = new(vY,vY, vY, 1);

                    if (World.colored_voxels)
                    {
                        if(Yfixed < 0) // Mar
                        {
                            Console.WriteLine("Colorindo mar " + Yfixed);
                            color_before = new Color4(0f, 0f, 1f, 1);
                        }
                        else
                        {
                            color_before = new Color4(0f, 1f, 0f, 1);
                        }
                    }

                    Color4 color = (((_x == chunkCenterX && _y == chunkCenterY) || (_x == chunkCenterX-8 || _y == chunkCenterY-8) 
                        || (_x == chunkCenterX + 8 || _y == chunkCenterY + 8))) && World.debug_lines ? new (1,1,1,1) : color_before;

                    Voxel voxel = new Voxel(color);
                    Vector3 pos = new Vector3(_x, Yfixed, _y);
                    voxel.Position = pos;

                    Console.WriteLine("X : " + _x + "|CHUNK X : " + (_x + CHUNK_SIZE / 2) + "|Y : " + _y + "|CHUNK Y : " + (_y + CHUNK_SIZE / 2)
                        +"|CENTER X: " + chunkCenterX + "|CENTER Y: " + chunkCenterY);

                    if(_x + CHUNK_SIZE / 2 >= 16 || _y + CHUNK_SIZE / 2 >= 16)
                    {
                        Console.WriteLine("Posição errada ");
                        _x = chunkCenterX + CHUNK_SIZE / 2;
                        break;
                    }
                    else
                    {

                        voxels[_x + CHUNK_SIZE / 2, _y + CHUNK_SIZE / 2] = voxel;
                    }

                    
                    //_voxels.Add(voxel);
                }
         
            }
        }

        public void Destroy()
        {
            for (int x = 0; x < voxels.GetLength(0); x++)
            {
                for (int y = 0; y < voxels.GetLength(1); y++)
                {
                    Voxel v = voxels[x, y];
                    if (v != null)
                    {
                        v.Destroy(); // Chame o método Destroy() do Voxel
                        voxels[x, y] = null; // Atribua null à posição do array
                    }
                }
            }

            //for(int i = 0; i < _voxels.Count; i++)
            //{
            //    Voxel voxel = _voxels[i];
            //    voxel.Destroy();
            //}

            //for (int i = 0; i < _voxels.Count; i++)
            //{
            //    Voxel voxel = _voxels[i];
            //    _voxels.Remove(voxel);
            //}
        }
    }
}
