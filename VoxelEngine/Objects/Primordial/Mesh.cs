using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using VoxelEngine.Core;
using VoxelEngine.Scenes;
namespace VoxelEngine.Objects.Primordial
{
    public class Mesh
    {

        public static int TotalTriangles = 0;

        public Transform transform = new();

        public int indicesCount = 0;

        int VBO;
        int VAO;
        int EBO;

        bool changing = false;
        public bool changed = false;

        uint[] indices = null;

        public Mesh(float[] vertices, uint[] _indices)
        {
            indices = _indices;
            
            CreateVertices(vertices);

            // Habilita os atributos de posição vertex na localização 0  // Stride é quantas casas vai ter que se mover para achar o próximo valor da proxima vertex
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Habilita os atributos de cor na vertex na localização 1  // Offset é para onde ele começará a ler. Como a posição fica de 0 a 3, ele começará do 3
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // Element Buffer Object
            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
            //GL.BindVertexArray(0);  // Unbind the VAO

            indicesCount = _indices.Length;

            TotalTriangles += _indices.Length / 3;

            Scene.currentScene.meshes.Add(this);

        }

        public void CreateVertices(float[] vertices)
        {
            changing = true;

            // Vertex Array Object
            // Se VAO não existir, crie-o
            if (VAO == 0)
            {
                VAO = GL.GenVertexArray();
            }
            GL.BindVertexArray(VAO);

            // Vertex Buffer Object
            // Se VBO não existir, crie-o; caso contrário, substitua os dados no buffer
            if (VBO == 0)
            {
                VBO = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
                GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            }
            else
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
                GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, vertices.Length * sizeof(float), vertices);
            }


            changing = false;
            changed = true; // Marca que houve uma mudança
        }

        internal Matrix4 GetModelMatrix()
        {
            Matrix4 model = Matrix4.CreateScale(transform.scale)
                            * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(transform.rotation.X))
                            * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(transform.rotation.Y))
                            * Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(transform.rotation.Z))
                            * Matrix4.CreateTranslation(transform.position);

            return model;
        }

        internal void Render()
        {
            if (changing) return;

            GL.BindVertexArray(VAO);

            // Gerar matrix
            Window.window.shader.SetMatrix4("model", GetModelMatrix());

            GL.DrawElements(PrimitiveType.Triangles, indicesCount, DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(0);  // Unbind the VAO

            var error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine($"OpenGL Error: {error}");
            }
        }

        public void Destroy()
        {
            if (VAO != 0)
            {
                GL.DeleteVertexArray(VAO);
                VAO = 0;
            }
            if (VBO != 0)
            {
                GL.DeleteBuffer(VBO);
                VBO = 0;
            }
            if( EBO != 0)
            {
                GL.DeleteBuffer(EBO);
                EBO = 0;
            }
            Scene.currentScene.meshes.Remove(this);
            //GC.SuppressFinalize(this);

            TotalTriangles -= indices.Length / 3;
        }

    }
}
