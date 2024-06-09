using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using VoxelEngine.Core;
using VoxelEngine.Mathf;
using System.Drawing;
using MyGame;
using Microsoft.VisualBasic;
using ImGuiNET;


namespace VoxelEngine.Components.Chunks
{
    public enum BlockType
    {
        DEFAULT,
        AIR
    }

    public class Voxel2
    {
        int VBO;
        int VAO;
        int EBO;
        float[] vertices;

        public uint[] indices;

        public BlockType blockType;

        public Voxel2(BlockType _blockType)
        {
            blockType = _blockType;
            if (blockType == BlockType.AIR) return;

            vertices = VoxelConstants.GetVoxelColored(Randomic.NextFloat(0,1), Randomic.NextFloat(0, 1), Randomic.NextFloat(0, 1));

            // VBO = Local na memoria onde sera armazenado as vertices
            // Bind VBO (VERTEX BUFFER OBJECT)
            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            // Copia os dados da vertex para o buffer de memoria
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // VAO = Local onde é armazenado os ids das vertices
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            // Habilita os atributos de posição vertex na localização 0  // Stride é quantas casas vai ter que se mover para achar o próximo valor da proxima vertex
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            //Habilita os atributos de cor na vertex na localização 1  // Offset é para onde ele começará a ler. Como a posição fica de 0 a 3, ele começará do 3
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
        }

        public void Render(Matrix4 matrix)
        {
            if (blockType == BlockType.AIR) return;

            // Alocar VAO e VBO
            GL.BindVertexArray(VAO);
            GL.BindVertexArray(VBO);
            // Alocar element buffer object
            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);


            // Gerar matrix
            Window.shader.SetMatrix4("model", matrix);

            GL.DrawElements(PrimitiveType.Triangles, VoxelConstants.VOXEL_CUBE_INDICES.Length, DrawElementsType.UnsignedInt, 0);
        }
    }

    public class Chunk
    {
        public bool[] _faces = new bool[6] { true, true, true, true, true, true };


        public static int CHUNK_SIZE = 12;

        public Voxel2[,,] voxels = new Voxel2[CHUNK_SIZE, Settings.CHUNK_MAX_HEIGHT, CHUNK_SIZE];
        public Voxel2[,,] visible_voxels = new Voxel2[CHUNK_SIZE, Settings.CHUNK_MAX_HEIGHT, CHUNK_SIZE];

        bool GENERATING_CHUNK = true;
        

        internal Matrix4 GetModelMatrix(Vector3 position)
        {
            Matrix4 model = Matrix4.CreateTranslation(position)
                            * Matrix4.CreateScale(1)
                            * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(0))
                            * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(0))
                            * Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(0));

            return model;
        }

        public Chunk() 
        {
            Console.WriteLine("Generating chunk");
            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                for (int y = 0;  y < Settings.CHUNK_MAX_HEIGHT; y++)
                {
                    for (int z = 0; z < CHUNK_SIZE; z++)
                    {
                        if (y <= 75)
                        {
                            voxels[x, y, z] = new Voxel2(BlockType.DEFAULT);
                        }
                        else // AIR
                        {
                            voxels[x, y, z] = new Voxel2(BlockType.AIR);
                        }
                    }
                }
            }
            Console.WriteLine("Chunk generated");

            UpdateChunk();
            GENERATING_CHUNK = false;
        }

        private bool IsWithinBounds(int x, int y, int z)
        {
            return ((x >= 0) && (x < CHUNK_SIZE)) && ((y >= 0) && (y < Settings.CHUNK_MAX_HEIGHT)) && ((z >= 0) && (z < CHUNK_SIZE)) ;
        }

        private bool IsAir(int x, int y, int z)
        {
            return IsWithinBounds(x, y, z) ? voxels[x, y, z].blockType == BlockType.AIR : true;
        }

        public void UpdateChunk()
        {
            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                for (int y = 0; y < Settings.CHUNK_MAX_HEIGHT; y++)
                {
                    for (int z = 0; z < CHUNK_SIZE; z++)
                    {
                        Voxel2 block = voxels[x, y, z];

                        if (block.blockType == BlockType.AIR)
                        {
                            // DO NOTHING
                        }
                        else
                        { 
                            bool[] _faces = new bool[6] { false, false, false, false, false, false };

                            _faces[0] = IsAir(x, y, z + 1);
                            _faces[1] = IsAir(x, y, z - 1);
                            _faces[2] = IsAir(x, y + 1, z);
                            _faces[3] = IsAir(x, y - 1, z);
                            _faces[4] = IsAir(x + 1 , y, z);
                            _faces[5] = IsAir(x - 1, y, z);

                            block.indices = VoxelConstants.GetVoxelIndices(_faces);
                        }
                    }
                }
            }
        }
        public void Render()
        {
            if (GENERATING_CHUNK) return;
            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                for (int y = 0; y < Settings.CHUNK_MAX_HEIGHT; y++)
                {
                    for (int z = 0; z < CHUNK_SIZE; z++)
                    {
                        Voxel2 block = voxels[x, y, z];

                        if(block.blockType == BlockType.AIR)
                        {
                            // DO NOTHING
                        }
                        else
                        {

                            block.Render(GetModelMatrix(new Vector3(x, y, z)));
                        }
                    }
                }
            }
        }
    }
}
