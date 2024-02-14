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
        int EBO; // Elements Buffer Object
        float[] vertices;
        uint[]  indices = VoxelConstants.VOXEL_CUBE_INDICES;

        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }

        public Vector3 Scale = new Vector3(1, 1, 1);


        public Voxel() : this(Scenes.Scene.getCurrentScene(), new Color4(1,1,1,1)){}

        public Voxel(Color4 color) : this(Scenes.Scene.getCurrentScene(), color){}

        public Voxel(Scenes.Scene _scene) : this(_scene, new Color4(1, 1, 1, 1)){}

        public Voxel(Scenes.Scene _scene, Color4 color) 
        {
            _scene.objectManager.PushVoxel(this);

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

            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);



        }



        internal Matrix4 GetModelMatrix()
        {


            Matrix4 model = Matrix4.CreateTranslation(Position)
                            * Matrix4.CreateScale(Scale)
                            * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(Rotation.X))
                            * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(Rotation.Y))
                            * Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(Rotation.Z));
                            
            return model;
            

        }

        internal void Render(double deltaTime)
        {
            // Adiciona as vertexs desse objeto para ser trabalhado
            GL.BindVertexArray(VBO);


            Window.shader.SetMatrix4("model", GetModelMatrix());

            
            GL.DrawElements(PrimitiveType.Triangles, VoxelConstants.VOXEL_CUBE_INDICES.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}
