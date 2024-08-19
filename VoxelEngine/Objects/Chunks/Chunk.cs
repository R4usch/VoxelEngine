
using VoxelEngine.Objects.Primordial;
using VoxelEngine.Utils.Math;

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

        static int ChunkSize = 3;
        static int ChunkHeight = 3;

        TestBlockTypeStruct[,,] voxels = new TestBlockTypeStruct[ChunkSize, ChunkSize, ChunkSize];

        static float offSetZ = 0f;
        static float offSetY = 0f;

        public Chunk()
        {
            for(int x = 0; x < ChunkSize; x++)
            {
                for(int y = 0; y < ChunkSize; y ++)
                {
                    for(int z = 0; z < ChunkSize; z++)
                    {
                        bool debug = false;
                        if((x % 4 == 0 && x != 0))
                        {
                            voxels[x,y,z] = new TestBlockTypeStruct(TestBlockTypeStruct.BlockType.AIR);
                        }
                        else
                        {
                            voxels[x, y, z] = new TestBlockTypeStruct(TestBlockTypeStruct.BlockType.GRASS);

                            if (debug)
                            {
                                Mesh mesh = new Mesh(VoxelEngine.Voxels.Constants.GetVoxelColored(1f, 1f, 1f), VoxelEngine.Voxels.Constants.VOXEL_CUBE_INDICES);
                                mesh.transform.position.X = x;
                                mesh.transform.position.Y = y + offSetY;
                                mesh.transform.position.Z = z;
                            }
                        }
                    }
                }
            }
            GreedyMesh();
        }

        bool isBlockAt(int x, int y, int z)
        {
            return voxels[x, y, z].type != TestBlockTypeStruct.BlockType.AIR;
        }

        void GreedyMesh()
        {
            
        }

        public override void Update()
        {

        }
    }
}
