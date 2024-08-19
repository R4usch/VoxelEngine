using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using VoxelEngine.Core;
using VoxelEngine.Scenes;
namespace VoxelEngine.Objects.Primordial
{
    public class Mesh
    {

        public Transform transform = new();

        public int indicesCount = 0;

        int VBO;
        int VAO;
        int EBO;

        public Mesh(float[] vertices, uint[] indices)
        {
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

            // Habilita os atributos de cor na vertex na localização 1  // Offset é para onde ele começará a ler. Como a posição fica de 0 a 3, ele começará do 3
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // Element Buffer Object
            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            //GL.BindVertexArray(0);  // Unbind the VAO

            indicesCount = indices.Length;

            Scene.currentScene.meshes.Add(this);

        }

        internal Matrix4 GetModelMatrix()
        {
            Matrix4 model = Matrix4.CreateTranslation(transform.position)
                            * Matrix4.CreateScale(transform.scale)
                            * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(transform.rotation.X))
                            * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(transform.rotation.Y))
                            * Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(transform.rotation.Z));

            return model;
        }

        internal void Render()
        {
            //Console.WriteLine("Desenhando");
            //Console.WriteLine("Renderizando mesh");
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
    }
}
