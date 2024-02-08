using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using VoxelEngine.Core;

namespace VoxelEngine.Components
{


    public class Voxel
    {
        int VAO; // Vertex Array Object
        int VBO; // Vertex Buffer Object
        float[] vertices;

        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }

        Stopwatch timer;

        public Voxel(float[] _vertices) : this(_vertices, Scenes.Scene.getCurrentScene())
        {
  
        }

        public Voxel(float[] _vertices, Scenes.Scene _scene) 
        {
            timer = new Stopwatch(); // Cria uma nova classe do StopWatch
            timer.Start(); // Inicia o stopwatch

            vertices = _vertices;

            // VAO = Local onde é armazenado os ids das vertices
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            // VBO = Local na memoria onde sera armazenado as vertices
            // Bind VBO (VERTEX BUFFER OBJECT)
            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);

            // Copia os dados da vertex para o buffer de memoria
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices.ToArray(), BufferUsageHint.StaticDraw);

            // Habilita os atributos de posição vertex na localização 0  // Stride é quantas casas vai ter que se mover para achar o próximo valor da proxima vertex
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Habilita os atributos de cor na vertex na localização 1  // Offset é para onde ele começará a ler. Como a posição fica de 0 a 3, ele começará do 3
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            _scene.objectManager.PushVoxel(this);
        }

        public void Translate()
        {

        }

        internal Matrix4 GetModelMatrix()
        {
            Matrix4 model = Matrix4.CreateTranslation(Position)
                            * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(Rotation.X))
                            * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(Rotation.Y))
                            * Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(Rotation.Z));
            return model;

        }

        public void Render(double deltaTime)
        {

            Window.shader.SetMatrix4("model", GetModelMatrix());


            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length / 3);
        }
    }
}
