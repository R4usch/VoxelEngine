
using System;
using System.ComponentModel.Design;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.X86;
using VoxelEngine.Objects.Primordial;
using VoxelEngine.Utils.Math;
using VoxelEngine.Utils.Threading;
using static System.Reflection.Metadata.BlobBuilder;

namespace VoxelEngine.Objects.Chunks
{
    public class Chunk : GameObject
    {
        public struct TestBlockTypeStruct
        {
            public enum BlockType
            {
                AIR,
                GRASS,
            }

            public TestBlockTypeStruct(BlockType type)
            {
                this.type = type;
            }

            public BlockType type;
        }

        static readonly int CHUNK_SIZE = 10;

        TestBlockTypeStruct[,,] voxels = new TestBlockTypeStruct[CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE];

        static float offSetX = -CHUNK_SIZE / 2;
        static float offSetY = -CHUNK_SIZE / 2;
        static float offSetZ = -80f;


        public Mesh[,,] debugMesh = new Mesh[CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE];
        public List<Mesh> greedyMeshes = new List<Mesh>();

        public int start_direction = 0;
        public bool break_mesh = false;

        public Chunk()
        {
            // Definimos aqui as posições do chunk
            //chunkPosX = this.transform.position.X; chunkPosY = this.transform.position.Y; chunkPosZ = this.transform.position.Z;


            for(int x = 0; x < CHUNK_SIZE; x++)
            {
                for(int y = 0; y < CHUNK_SIZE; y ++)
                {
                    for(int z = 0; z < CHUNK_SIZE; z++)
                    {
                        bool debug = true;

                        if (x % 2 == 0 && x != 0) // x % 2 == 0 && x != 0
                        {
                            voxels[x, y, z] = new TestBlockTypeStruct(TestBlockTypeStruct.BlockType.AIR);
                        }
                        else
                        {
                            voxels[x, y, z] = new TestBlockTypeStruct(TestBlockTypeStruct.BlockType.GRASS);
                        }

                        if (debug)
                        {

                            if(voxels[x, y, z].type != TestBlockTypeStruct.BlockType.AIR)
                            {
                                Mesh mesh = new Mesh(VoxelEngine.Voxels.Constants.GetVoxelColored(1f, 1f, 1f), VoxelEngine.Voxels.Constants.VOXEL_CUBE_INDICES);
                                mesh.transform.position.X = x + offSetX;
                                mesh.transform.position.Y = y + offSetY;
                                mesh.transform.position.Z = z + offSetZ;
                                //mesh.CreateVertices(VoxelEngine.Voxels.Constants.GetVoxelColored(0f, 0f, 1f));
                                debugMesh[x, y, z] = mesh;
                            }

                        }
                        
                    }
                }
            }

            //GreedyMesh();
        }

        bool isBlockAt(int x, int y, int z)
        {
            if (x < 0 || x >= CHUNK_SIZE) return false;
            if (y < 0 || y >= CHUNK_SIZE) return false;
            if (z < 0 || z >= CHUNK_SIZE) return false;

            if (voxels[x, y, z].type == TestBlockTypeStruct.BlockType.AIR) return false;

            return true;
        }

        public void GreedyMesh()
        {
            
            int time = 1000;
            int initialTime = 0;


            for (int direction = start_direction; direction < 3; ++direction)
            {

                int direction2 = (direction + 1) % 3;
                int direction3 = (direction + 2) % 3;

                int[] direction_verificada = new int[3];
                int[] direction_a_ser_verificada = new int[3];

                bool[] mascara = new bool[CHUNK_SIZE * CHUNK_SIZE];
                direction_a_ser_verificada[direction] = 1;

                for (direction_verificada[direction] = -1; direction_verificada[direction] < CHUNK_SIZE;)
                {
                    int n = 0;

                    for (direction_verificada[direction3] = 0; direction_verificada[direction3] < CHUNK_SIZE; ++direction_verificada[direction3])
                    {
                        for (direction_verificada[direction2] = 0; direction_verificada[direction2] < CHUNK_SIZE; ++direction_verificada[direction2])
                        {
                            Int3 pos = new Int3(direction_verificada[0], direction_verificada[1], direction_verificada[2]);
                            Int3 nextPos = new Int3(direction_a_ser_verificada[0], direction_a_ser_verificada[1], direction_a_ser_verificada[2]);

                            // Caso a posição seja 0, ele joga true, por que já está na ponta
                            bool blockCurrent = isBlockAt(pos.X, pos.Y, pos.Z);
                            // Caso a posição do próximo bloco for menor que o chunk, use isBlockAt, caso contrario, é true por que está na ponta
                            bool blockNext    = isBlockAt(pos.X + nextPos.X, pos.Y + nextPos.Y, pos.Z + nextPos.Z);

                            mascara[n++] = blockCurrent != blockNext;
                        }
                    }
                    ++direction_verificada[direction];

                    n = 0;

                    // Passando por todos os blocos daquele pedaço novamente
                    for(int local_direction3 = 0; local_direction3 < CHUNK_SIZE; ++local_direction3)
                    {
                        for (int local_direction2 = 0; local_direction2 < CHUNK_SIZE;)
                        {
                            if (mascara[n])
                            {
                                // Calculando largura
                                int largura = 0;
                                for (largura = 1; local_direction2 + largura < CHUNK_SIZE && mascara[n + largura]; largura++) { }

                                // Calculando altura
                                bool done = false;
                                int altura = 0;
                                for (altura = 1; local_direction3 + altura < CHUNK_SIZE; altura++)
                                {
                                    // Check each block next to this quad
                                    for (int k = 0; k < largura; ++k)
                                    {
                                        // If there's a hole in the mask, exit
                                        if (!mascara[n + k + altura * CHUNK_SIZE])
                                        {
                                            done = true;
                                            break;
                                        }
                                    }
                                    if (done)
                                        break;
                                }

                                direction_verificada[direction2] = local_direction2;
                                direction_verificada[direction3] = local_direction3;

                                // Orientação da 
                                int[] du = new int[3];
                                du[direction2] = largura;

                                int[] dv = new int[3];
                                dv[direction3] = altura;

                                Float3 topLeft     = new Float3(direction_verificada[0] - 0.5f, 
                                                                direction_verificada[1] - 0.5f, 
                                                                direction_verificada[2] - 0.5f);
                                Float3 topRight    = new Float3(direction_verificada[0] + du[0] - 0.5f, 
                                                                direction_verificada[1] + du[1] - 0.5f, 
                                                                direction_verificada[2] + du[2] - 0.5f);
                                Float3 bottomLeft  = new Float3(direction_verificada[0] + dv[0] - 0.5f, 
                                                                direction_verificada[1] + dv[1] - 0.5f, 
                                                                direction_verificada[2] + dv[2] - 0.5f);
                                Float3 bottomRight = new Float3(direction_verificada[0] + du[0] + dv[0] - 0.5f, 
                                                                direction_verificada[1] + du[1] + dv[1] - 0.5f, 
                                                                direction_verificada[2] + du[2] + dv[2] - 0.5f);

                                float[] vertices = Voxels.Constants.AppendQuad(
                                        topLeft,
                                        topRight, 
                                        bottomLeft, 
                                        bottomRight
                                );

                                Mesh mesh = new Mesh(vertices, Voxels.Constants.QUAD_INDICES);

                                mesh.transform.position.X = offSetX;
                                mesh.transform.position.Y = offSetY;
                                mesh.transform.position.Z = offSetZ;

                                //totalMeshes += 1;
                                greedyMeshes.Add( mesh );

                                for (int l = 0; l < altura; ++l)
                                    for (int k = 0; k < largura; ++k)
                                        mascara[n + k + l * CHUNK_SIZE] = false;

                                local_direction2 += largura;
                                n += largura;
                            }
                            else
                            {
                                local_direction2++;
                                n++;
                            }
                        }
    
                    }
                }

                if(break_mesh) break;
            }
            
        }

        public override void Update()
        {

        }
    }
}
