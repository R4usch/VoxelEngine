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


namespace VoxelEngine.Components.Chunks
{
    // Novo Voxel Otimizado.
    // Capaz de sumir com faces para melhor desempenho
    public class Voxel
    {
        int VAO; // Vertex Array Object
        int VBO; // Vertex Buffer Object
        int EBO; // Elements Buffer Object
                                           //Xpositive Xnegative Ypositive Ynegative Znegative Zpositive
        public bool[] faces = new bool[6] {  true,     true,     true,     true,     true,     true };

        uint[] indices = VoxelConstants.VOXEL_CUBE_INDICES;
        float[] vertices;
        public Vector3 position { get; set; }

        public Vector2 chunk_position { get; set; }

        public Vector3 scale = new(1f, 1f, 1f);
        public Vector3 rotation { get; set; }


        public Voxel(Color4 color)
        {
            vertices = VoxelConstants.GetVoxelColored(color.R, color.G, color.B);

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

            //Cores. Atualmente desabilitado
            //Habilita os atributos de cor na vertex na localização 1  // Offset é para onde ele começará a ler. Como a posição fica de 0 a 3, ele começará do 3
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            UpdateIndices();
        }
        internal Matrix4 GetModelMatrix()
        {
            Matrix4 model = Matrix4.CreateTranslation(position)
                            * Matrix4.CreateScale(scale)
                            * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(rotation.X))
                            * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(rotation.Y))
                            * Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(rotation.Z));

            return model;
        }

        public void UpdateIndices()
        {
            indices = VoxelConstants.GetVoxelIndices(faces);

            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
        }

        public void Render()
        {
            // Adiciona as vertexs desse objeto para ser trabalhado
            GL.BindVertexArray(VBO);
            GL.BindVertexArray(VAO);

            Window.shader.SetMatrix4("model", GetModelMatrix());


            GL.DrawElements(PrimitiveType.Triangles, VoxelConstants.VOXEL_CUBE_INDICES.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void VerifyFaces()
        {

        }
    }
    public class Chunk
    {

        public static int CHUNK_SIZE = 16;

        int VBO;

        public Voxel[,] voxels = new Voxel[CHUNK_SIZE, CHUNK_SIZE];

        public Chunk(int chunkCenterX, int chunkCenterY) 
        {
            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                for (int y = 0; y < CHUNK_SIZE; y++)
                {
                    //Console.WriteLine("Criando voxels");
                    int _x = x + chunkCenterX / 2;
                    int _z = y + chunkCenterY / 2;

                    Voxel voxel = new Voxel(new Color4(Randomic.NextFloat(0f,1f), Randomic.NextFloat(0f, 1f), Randomic.NextFloat(0f, 1f), 1f))
                    {
                        position = new(_x, -1, _z)
                    };


                    voxels[x,y] = voxel;
                }
            }
        }



        public void Render()
        {
            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                for (int y = 0; y < CHUNK_SIZE; y++)
                {
                    Voxel v = voxels[x, y];
                    if (v != null)
                    {
                        v.Render();
                    }
                }
            }
        }
    }
}
