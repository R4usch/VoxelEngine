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
using VoxelEngine.Utils;

namespace VoxelEngine.Components.Chunks
{
    public enum BlockType
    {
        DEFAULT,
        AIR
    }

    public class Voxel
    {
        //int VBO;
        //int VAO;
        //int EBO;
        //float[] vertices;

        public float color;

        float[] colors = new float[3] { 0f, 0.5f, 1f };

        //public uint[] indices;

        public BlockType blockType;


        public Voxel(BlockType _blockType)
        {
            blockType = _blockType;
            if (blockType == BlockType.AIR) return;

            int random = Randomic.NextInt(0, 2);
            color = colors[random];

            //vertices = VoxelConstants.GetVoxelColored(color, color, color);

            //// VBO = Local na memoria onde sera armazenado as vertices
            //// Bind VBO (VERTEX BUFFER OBJECT)
            //VBO = GL.GenBuffer();
            //GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            //// Copia os dados da vertex para o buffer de memoria
            //GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            //// VAO = Local onde é armazenado os ids das vertices
            //VAO = GL.GenVertexArray();
            //GL.BindVertexArray(VAO);

            //// Habilita os atributos de posição vertex na localização 0  // Stride é quantas casas vai ter que se mover para achar o próximo valor da proxima vertex
            //GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            //GL.EnableVertexAttribArray(0);

            ////Habilita os atributos de cor na vertex na localização 1  // Offset é para onde ele começará a ler. Como a posição fica de 0 a 3, ele começará do 3
            //GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            //GL.EnableVertexAttribArray(1);

            //EBO = GL.GenBuffer();
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            //GL.BufferData(BufferTarget.ElementArrayBuffer, VoxelConstants.VOXEL_CUBE_INDICES.Length * sizeof(uint), VoxelConstants.VOXEL_CUBE_INDICES, BufferUsageHint.StaticDraw);

            //GL.BindVertexArray(0);  // Unbind the VAO
        }

        public void Render(Matrix4 matrix)
        {
            if (blockType == BlockType.AIR) return;

            //// Alocar VAO e VBO
            //GL.BindVertexArray(VAO);

            //// Gerar matrix
            //Window.shader.SetMatrix4("model", matrix);

            //GL.DrawElements(PrimitiveType.Triangles, VoxelConstants.VOXEL_CUBE_INDICES.Length, DrawElementsType.UnsignedInt, 0);

            //GL.BindVertexArray(0);  // Unbind the VAO
        }
    }

    public class Mesh
    {
        int VBO;
        int VAO;
        int EBO;
        MeshData data;
        public Mesh(MeshData _data) 
        {
            data = _data;
            // VBO = Local na memoria onde sera armazenado as vertices
            // Bind VBO (VERTEX BUFFER OBJECT)
            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            // Copia os dados da vertex para o buffer de memoria
            GL.BufferData(BufferTarget.ArrayBuffer, data.vertices.Length * sizeof(float), data.vertices, BufferUsageHint.StaticDraw);

            // VAO = Local onde é armazenado os ids das vertices
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            // Habilita os atributos de posição vertex na localização 0  // Stride é quantas casas vai ter que se mover para achar o próximo valor da proxima vertex
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            //Habilita os atributos de cor na vertex na localização 1  // Offset é para onde ele começará a ler. Como a posição fica de 0 a 3, ele começará do 3
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, VoxelConstants.VOXEL_CUBE_INDICES.Length * sizeof(uint), VoxelConstants.VOXEL_CUBE_INDICES, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(0);  // Unbind the VAO
        
        }

        public void Render(Matrix4 matrix)
        {
            GL.BindVertexArray(VAO);

            // Gerar matrix
            Window.shader.SetMatrix4("model", matrix);

            GL.DrawElements(PrimitiveType.Triangles, VoxelConstants.VOXEL_CUBE_INDICES.Length, DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(0);  // Unbind the VAO
        }

        public class MeshData
        {
            public uint[] indices;
            public float[] vertices;
        }
    }

    public class Chunk
    {
  

        static readonly Settings.ChunkSize CHUNK_SIZE = Settings.CHUNK_SIZE;

        Voxel[,,] voxels         = new Voxel[CHUNK_SIZE.x, CHUNK_SIZE.y, CHUNK_SIZE.z];
        Voxel[,,] visible_voxels = new Voxel[CHUNK_SIZE.x, CHUNK_SIZE.y, CHUNK_SIZE.z];
        Mesh[,,]  meshes         =  new Mesh[CHUNK_SIZE.x, CHUNK_SIZE.y, CHUNK_SIZE.z];

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
            for (int x = 0; x < CHUNK_SIZE.x; x++)
            {
                for (int y = 0;  y < CHUNK_SIZE.y; y++)
                {
                    for (int z = 0; z < CHUNK_SIZE.z; z++)
                    {
                        if (y <= 75)
                        {
                            voxels[x, y, z] = new Voxel(BlockType.DEFAULT);
                        }
                        else // AIR
                        {
                            voxels[x, y, z] = new Voxel(BlockType.AIR);
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
            return ((x >= 0) && (x < CHUNK_SIZE.x)) && ((y >= 0) && (y < CHUNK_SIZE.y)) && ((z >= 0) && (z < CHUNK_SIZE.z)) ;
        }

        private Voxel BlockAt(int x, int y, int z)
        {
            return IsWithinBounds(x, y, z) ? voxels[x, y, z] : new Voxel(BlockType.AIR);
        }

        private bool IsAir(int x, int y, int z)
        {
            return IsWithinBounds(x, y, z) ? voxels[x, y, z].blockType == BlockType.AIR : true;
        }

        public void UpdateChunk()
        {

            GenerateGreedyMesh2();

            GENERATING_CHUNK = false;
        }

        public class GreedyMesh
        {
            public List<VoxelCoordinate> _voxels = new List<VoxelCoordinate>();
            

            public void Add(int x, int y, int z, float color)
            {
                _voxels.Add(new VoxelCoordinate(x,y,z, color));
            }

            public Mesh.MeshData Generate()
            {
                Mesh.MeshData data = new Mesh.MeshData();

                VoxelCoordinate voxelStart = _voxels[0];
                VoxelCoordinate voxelEnd = _voxels[_voxels.Count - 1];


                data.indices = VoxelConstants.VOXEL_CUBE_INDICES;
                data.vertices = VoxelConstants.CombineVoxel(voxelStart.pos, voxelEnd.pos, _voxels[0].initialColor, _voxels[0].initialColor, _voxels[0].initialColor);

                return data;
            }


            public class VoxelCoordinate
            {
                public Vector3 pos;
                public float initialColor;

                public VoxelCoordinate(int x, int y, int z, float _color)
                {
                    this.pos = new Vector3(x, y, z);
                    this.initialColor = _color; 
                }
            }
        }


        void GenerateGreedyMesh()
        {
            List<GreedyMesh> _meshes = new List<GreedyMesh>();
            for(int y = 0; y < CHUNK_SIZE.y; y++)
            {
                for(int z = 0; z < CHUNK_SIZE.z; z++)
                {
                    for(int x = 0; x < CHUNK_SIZE.x; x++)
                    {
                        if (visible_voxels[x, y, z] == null) continue;
                        GreedyMesh greedyMesh = new GreedyMesh();
                        Voxel current = visible_voxels[x, y, z];
                        greedyMesh.Add(x, y, z, current.color);
                        _meshes.Add(greedyMesh);

                        int maxX = 0;
                        for(int _x = x + 1; _x < CHUNK_SIZE.x; _x++)
                        {
                            if (visible_voxels[_x, y, z] == null) break;

                            if (visible_voxels[_x, y, z].color == current.color)
                            {
                                maxX++;
                                greedyMesh.Add(_x, y, z, current.color);
                            }
                            else
                            {
                                break;
                            }
                        }
                        x += maxX;
                    }
                }
            }

            Console.WriteLine("Meshes generated : " + _meshes.Count);
            
            foreach(GreedyMesh greedyMesh in _meshes)
            {
                Mesh mesh = new Mesh(greedyMesh.Generate());
                var pos = greedyMesh._voxels[0];
                meshes[(int)pos.pos.X, (int)pos.pos.Y, (int)pos.pos.Z] = mesh;
            }

        }

        void GenerateGreedyMesh2()
        {
            for(int axis = 0; axis < 3; ++axis)
            {
                Console.WriteLine("Generating mesh on axis " + GetAxisName(axis));

                int axis_perpendicular = (axis + 1) % 3;
                int axis_perpendicular_2 = (axis + 2) % 3;

                int main_axis_limit = CHUNK_SIZE[axis];
                int axis_perpendicular_limit = CHUNK_SIZE[axis_perpendicular];
                int axis_perpendicular_2_limit = CHUNK_SIZE[axis_perpendicular_2];

                int[] chunkItr = new int[3];
                int[] axisMask = new int[3];
                int[] deltaAxis = new int[3];

                axisMask[axis] = 1;

                bool[] mask = new bool[CHUNK_SIZE[axis_perpendicular] * CHUNK_SIZE[axis_perpendicular_2]];

                for (chunkItr[axis] = -1; chunkItr[axis] < main_axis_limit;){
                    int n = 0;

                    for (chunkItr[axis_perpendicular_2] = 0; chunkItr[axis_perpendicular_2] < axis_perpendicular_2_limit; ++chunkItr[axis_perpendicular_2])
                    {
                        for (chunkItr[axis_perpendicular] = 0; chunkItr[axis_perpendicular] < axis_perpendicular_limit; ++chunkItr[axis_perpendicular])
                        {
                            var currentBlock = BlockAt(chunkItr[0], chunkItr[1], chunkItr[2]);
                            var compareBlock = BlockAt(chunkItr[0] + axisMask[0], chunkItr[1] + axisMask[1], chunkItr[2] + axisMask[2]);

                            bool currentBlockOpaque = currentBlock.blockType != BlockType.AIR;
                            bool compareBlockOpaque = compareBlock.blockType != BlockType.AIR;

                            if(currentBlockOpaque == compareBlockOpaque)
                            {
                                mask[n++] = false;
                            }
                            else if (currentBlockOpaque)
                            {
                                mask[n++] = true;
                            }
                            else
                            {
                                mask[n++] = true;
                            }
                        }
                    }

                    ++chunkItr[axis];
                    Debug.PrintLine(chunkItr[axis], main_axis_limit, mask.Length);
                    n = 0;

                    // Gerar malha vindo da mascara
                    for(int j = 0; j < axis_perpendicular_2_limit; ++j)
                    {
                        for (int i = 0; i < axis_perpendicular_limit;)
                        {
                            if (mask[n])
                            {
                                chunkItr[axis_perpendicular] = i;
                                chunkItr[axis_perpendicular_2] = j;

                                // Gera o tamanho width da mesh
                                int width;
                                for(width = 1; i + width < axis_perpendicular_limit && mask[n + width]; width++){}

                                // Gera o tamanho height da mesh
                                int height;
                                bool done = false;

                                for(height = 1; j + height < axis_perpendicular_2_limit; height++)
                                {
                                    for(int k = 0; k < width; k++)
                                    {
                                        // Se encontrar um buraco na mascará, ele ira terminar este loop
                                        if (!mask[n + k + height * axis_perpendicular_limit])
                                        {
                                            done = true;
                                            break;
                                        }
                                    }

                                    if (done) break;
                                }

                                deltaAxis[axis_perpendicular] = width;
                                deltaAxis[axis_perpendicular_2] = height;

                                var du = new int[3];
                                du[axis_perpendicular] = width;

                                var dv = new int[3];
                                dv[axis_perpendicular_2] = height;

                                VoxelConstants.Vertice top_left =     new(chunkItr[0] + deltaAxis[0], chunkItr[1] + deltaAxis[1], chunkItr[2] + deltaAxis[2]);
                                VoxelConstants.Vertice top_right =    new(chunkItr[0] + deltaAxis[0] + du[0], chunkItr[1] + deltaAxis[1] + du[1], chunkItr[2] + deltaAxis[2] + du[2]);
                                VoxelConstants.Vertice bottom_left =  new(chunkItr[0] + deltaAxis[0] + dv[0], chunkItr[1] + deltaAxis[1] + dv[1], chunkItr[2] + deltaAxis[2] + dv[2]);
                                VoxelConstants.Vertice bottom_right = new(chunkItr[0] + deltaAxis[0] + dv[0] + du[0], chunkItr[1] +deltaAxis[1] + dv[1] + du[1], chunkItr[2] +deltaAxis[2] + dv[2] + du[2]); 

                                Color4 color = new Color4(1f, 1f, 1f, 1f);

                                Console.WriteLine("Chunk : " + chunkItr[0] + "|" + chunkItr[1] + "|" + chunkItr[2]);

                                Mesh.MeshData data = new Mesh.MeshData();
                                data.vertices = VoxelConstants.CreateQuad(top_left, top_right, bottom_left, bottom_right, color);
                                data.indices = VoxelConstants.QUAD_INDICES;

                                VoxelEngine.Utils.Debug.PrintLine(chunkItr[0], chunkItr[1], chunkItr[2], main_axis_limit);

                                meshes[chunkItr[0] == 12 ? chunkItr[0] - 1 : chunkItr[0], 
                                    chunkItr[1] == 255 ? chunkItr[1] -1 : chunkItr[1], 
                                    chunkItr[2] == 12 ? chunkItr[2] - 1 : chunkItr[2]] = (new Mesh(data));

                                deltaAxis = [0,0,0];

                                for (int l = 0; l < height; ++l)
                                    for (int k = 0; k < width; ++k)
                                    {
                                        Debug.PrintLine(n + k + l * main_axis_limit);
                                        mask[n + k + l * main_axis_limit] = false;

                                    }
                                        

                                i += width;
                                n += height;
                            }
                            else
                            {
                                i ++;
                                n ++;
                            }
                        }
                    }
                }
            }
        }

        string GetAxisName(int axis)
        {
            switch(axis) 
            {
                case 0:
                    return "X";
                case 1:
                    return "Y";
                case 2:
                    return "Z";
            }
            return "NONE";
        }

        public void Render()
        {
            if (GENERATING_CHUNK) return;


            for (int x = 0; x < CHUNK_SIZE.x; x++)
            {
                for (int y = 0; y < CHUNK_SIZE.y; y++)
                {
                    for (int z = 0; z < CHUNK_SIZE.z; z++)
                    {
                        if (meshes[x, y, z] != null)
                        {
                            meshes[x, y, z].Render(GetModelMatrix(new Vector3(x, y, z)));
                        }
                    }
                }
            }
        }
    }
}
