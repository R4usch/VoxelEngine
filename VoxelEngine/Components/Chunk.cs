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
using static VoxelEngine.Components.Chunks.Chunk;


namespace VoxelEngine.Components.Chunks
{
    public enum BlockType
    {
        DEFAULT,
        AIR
    }

    public class Voxel
    {
        int VBO;
        int VAO;
        int EBO;
        float[] vertices;

        public float color;

        float[] colors = new float[3] { 0f, 0.5f, 1f };

        public uint[] indices;

        public BlockType blockType;

        public void UpdateTemporary(float r, float g, float b)
        {
            vertices = VoxelConstants.GetVoxelColored(r, g, b);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            // Copia os dados da vertex para o buffer de memoria
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        }

        public Voxel(BlockType _blockType)
        {
            blockType = _blockType;
            if (blockType == BlockType.AIR) return;

            int random = Randomic.NextInt(0, 2);
            color = colors[random];

            vertices = VoxelConstants.GetVoxelColored(color, color, color);

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

            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, VoxelConstants.VOXEL_CUBE_INDICES.Length * sizeof(uint), VoxelConstants.VOXEL_CUBE_INDICES, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(0);  // Unbind the VAO
        }

        public void Render(Matrix4 matrix)
        {
            if (blockType == BlockType.AIR) return;

            // Alocar VAO e VBO
            GL.BindVertexArray(VAO);

            // Gerar matrix
            Window.shader.SetMatrix4("model", matrix);

            GL.DrawElements(PrimitiveType.Triangles, VoxelConstants.VOXEL_CUBE_INDICES.Length, DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(0);  // Unbind the VAO
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
  

        public static int CHUNK_SIZE = 12;

        public Voxel[,,] voxels         = new Voxel[CHUNK_SIZE, Settings.CHUNK_MAX_HEIGHT, CHUNK_SIZE];
        public Voxel[,,] visible_voxels = new Voxel[CHUNK_SIZE, Settings.CHUNK_MAX_HEIGHT, CHUNK_SIZE];
        public Mesh[,,]  meshes         =  new Mesh[CHUNK_SIZE, Settings.CHUNK_MAX_HEIGHT, CHUNK_SIZE];

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
            return ((x >= 0) && (x < CHUNK_SIZE)) && ((y >= 0) && (y < Settings.CHUNK_MAX_HEIGHT)) && ((z >= 0) && (z < CHUNK_SIZE)) ;
        }

        private bool IsAir(int x, int y, int z)
        {
            return IsWithinBounds(x, y, z) ? voxels[x, y, z].blockType == BlockType.AIR : true;
        }

        public void UpdateChunk()
        {
            for (int y = 0; y < Settings.CHUNK_MAX_HEIGHT; y++)
            {
                for (int z = 0; z < CHUNK_SIZE; z++)
                {
                    for (int x = 0; x < CHUNK_SIZE; x++)
                    {
                        Voxel block = voxels[x, y, z];

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
                            _faces[4] = IsAir(x + 1, y, z);
                            _faces[5] = IsAir(x - 1, y, z);


                            foreach (var face in _faces)
                            {
                                if(face)
                                {
                                    block.indices = VoxelConstants.GetVoxelIndices(_faces);
                                    visible_voxels[x, y, z] = block;
                                    
                                }
                            }


                        }
                    }
                }
            }

            GenerateGreedyMesh();
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
            // Axis Z

            for(int y = 0; y < Settings.CHUNK_MAX_HEIGHT; y++)
            {
                for(int z = 0; z < CHUNK_SIZE; z++)
                {
                    for(int x = 0; x < CHUNK_SIZE; x++)
                    {
                        if (visible_voxels[x, y, z] == null) continue;
                        GreedyMesh greedyMesh = new GreedyMesh();
                        Voxel current = visible_voxels[x, y, z];
                        greedyMesh.Add(x, y, z, current.color);
                        _meshes.Add(greedyMesh);

                        int maxX = 0;
                        for(int _x = x + 1; _x < CHUNK_SIZE; _x++)
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

                        //for(int _z = z + 1; _z < CHUNK_SIZE; _z++)
                        //{

                        //}
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


        public void Render()
        {
            if (GENERATING_CHUNK) return;


            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                for (int y = 0; y < Settings.CHUNK_MAX_HEIGHT; y++)
                {
                    for (int z = 0; z < CHUNK_SIZE; z++)
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
