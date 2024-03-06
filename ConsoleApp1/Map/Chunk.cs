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

        public List<VoxelEngine.Components.Voxel> _voxels = new List<VoxelEngine.Components.Voxel>();

        public Voxel[,] array = new vox[CHUNK_SIZE, CHUNK_SIZE];


        public Chunk(int x, int y, FastNoiseLite noise)
        {
            // X
            for(int _x = x-CHUNK_SIZE/2; _x <= (x+ CHUNK_SIZE / 2); _x++)
            {
                for(int _y = y- CHUNK_SIZE / 2; _y <= (y+ CHUNK_SIZE / 2); _y++)
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

                    Color4 color = (((_x == x && _y == y) || (_x == x-8 || _y == y-8) || (_x == x + 8 || _y == y + 8))) && World.debug_lines ? new (1,1,1,1) : color_before;

                    Voxel voxel = new Voxel(color);
                    Vector3 pos = new Vector3(_x, Yfixed, _y);
                    voxel.Position = pos;

                    //array[_x, _y] = voxel;
                    
                    _voxels.Add(voxel);
                }
         
            }
        }

        public void Destroy()
        {
            for(int i = 0; i < _voxels.Count; i++)
            {
                Voxel voxel = _voxels[i];
                voxel.Destroy();
            }

            for (int i = 0; i < _voxels.Count; i++)
            {
                Voxel voxel = _voxels[i];
                _voxels.Remove(voxel);
            }
        }
    }
}
