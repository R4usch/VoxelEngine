using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using VoxelEngine.Scene;

namespace VoxelEngine.Core
{
  

    public class Window : GameWindow
    {
        
        int VAO; // Vertex Array Object
        int VBO; // Vertex Buffer Object

        float[] vertices;

        ShaderSettings shaderSettings;
        Shader shader;

        new protected Action<FrameEventArgs> RenderFrame;

        public Window(int width, int height, string title, ShaderSettings _shaderSettings, float[] _vertices) : base (GameWindowSettings.Default, new NativeWindowSettings
        {
            Size = new Vector2i(width, height),
            Title = title
        })
        {
            vertices = _vertices;
            shaderSettings = _shaderSettings;
            base.Run();
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            // Criar instancia de shader
            shader = new Shader(shaderSettings);
            shader.Use();

            
            GL.ClearColor(0.0f, 0.15f, 0.25f, 1.0f); // Cor de limpeza da tela
            GL.Enable(EnableCap.DepthTest); // Habilita profundida na tela

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



            
            
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            List<Scene.GameObject> gameObjects = Scene.Scene.getCurrentScene().GetGameObjects();

            foreach(GameObject obj in gameObjects)
            {
                obj.Update((float)args.Time);
            }
        }   

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); // Limpa o buffer de cores e profundidade

            // Carregar cada GameObject


            
            
            // Troca o buffer do front-end pelo back-end
            base.SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
        }

        public override void Close()
        {
            base.Close();
        }
    }
}
