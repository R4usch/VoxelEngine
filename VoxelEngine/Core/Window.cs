using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using VoxelEngine.Scenes;
using System.Reflection;

namespace VoxelEngine.Core
{
  

    public class Window : GameWindow
    {
        public static Window game;
        int VAO; // Vertex Array Object
        int VBO; // Vertex Buffer Object

        float[] vertices;

        ShaderSettings shaderSettings;
        internal static Shader shader;

        int frameCount = 0;

        public Window(int width, int height, string title, ShaderSettings _shaderSettings, float[] _vertices) : base (GameWindowSettings.Default, new NativeWindowSettings
        {
            Size = new Vector2i(width, height),
            Title = title
        })
        {
            vertices = _vertices;
            shaderSettings = _shaderSettings;
            game = this;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            // Criar instancia de shader
            shader = new Shader(shaderSettings);
            shader.Use();

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 800 / 800, 0.1f, 100.0f);
            shader.SetMatrix4("projection", projection);

            Matrix4 view = Matrix4.LookAt(new Vector3(0.0f, 0.0f, 3.0f),
                 new Vector3(0.0f, 0.0f, 0.0f),
                 new Vector3(0.0f, 1.0f, 0.0f));
            shader.SetMatrix4("view", view);

            GL.ClearColor(0.0f, 0.15f, 0.25f, 1.0f); // Cor de limpeza da tela
            GL.Enable(EnableCap.DepthTest); // Habilita profundida na tela

            
            
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if (Scenes.Scene.getCurrentScene() == null) return;

            List<Scenes.GameObject> gameObjects = Scenes.Scene.getCurrentScene().GetGameObjects();

            
            Scenes.Scene.getCurrentScene().Update(args.Time);

            foreach(GameObject obj in gameObjects)
            {
                obj.Update((float)args.Time);
            }
        }   

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            frameCount++;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); // Limpa o buffer de cores e profundidade

            if (Scenes.Scene.getCurrentScene() == null)
            {
                base.SwapBuffers();
                    return;
            };


            // View e Projeção

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 800 / 800, 0.1f, 100.0f);
            shader.SetMatrix4("projection", projection);

            Matrix4 view = Matrix4.LookAt(new Vector3(0.0f, 0.0f, 3.0f),
                 new Vector3(0.0f, 0.0f, 0.0f),
                 new Vector3(0.0f, 1.0f, 0.0f));
            shader.SetMatrix4("view", view);


            // Renderização de Voxels
            List<Components.Voxel> voxels = Scenes.Scene.getCurrentScene().GetVoxels();

            foreach(Components.Voxel obj in voxels)
            {
                obj.Render(args.Time);
            }

            // Renderização de GameObjects


            

            
            
            // Troca o buffer do front-end pelo back-end
            base.SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
        }
        
        public override void Run()
        {
            base.Run();
        }

        public override void Close()
        {
            base.Close();
        }
    }
}
