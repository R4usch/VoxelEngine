using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using System.Reflection;
using VoxelEngine.Components;
using ImGuiNET;
using VoxelEngine.ImGUI;

namespace VoxelEngine.Core
{


    public class Window : GameWindow
    {
        public static Window game;

        internal static Camera currentCamera;

        internal static Shader shader;
        ShaderSettings shaderSettings;

        int frameCount = 0;


        internal static Matrix4 viewMatrix;
        internal static Matrix4 projectionMatrix;

        ImGUI.ImGUIController _imGUIcontroller;

        static internal bool loaded = false;

        public Window(int width, int height, string title, ShaderSettings _shaderSettings) : base (GameWindowSettings.Default, new NativeWindowSettings
        {
            Size = new Vector2i(width, height),
            Title = title
        })
        {
            shaderSettings = _shaderSettings;
            game = this;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            _imGUIcontroller = new ImGUI.ImGUIController(ClientSize.X, ClientSize.Y);

            Console.WriteLine("OpenGL Version : " + GL.GetString(StringName.Version));

            // Criar instancia de shader
            shader = new Shader(shaderSettings);
            shader.Use();

            if(currentCamera != null ) 
            {

                viewMatrix = currentCamera.GetViewMatrix();
                shader.SetMatrix4("view", viewMatrix);
                
            }

            GL.ClearColor(0.0f, 0.15f, 0.25f, 1.0f); // Cor de limpeza da tela
            GL.Enable(EnableCap.DepthTest); // Habilita profundida na tela

            loaded = true;

            // Carrega a cena
            if(Scenes.Scene.getCurrentScene() != null)
            {
                Scenes.Scene.getCurrentScene().onLoad();
            }

        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if (Scenes.Scene.getCurrentScene() == null) return;

            // Atualiza a cena atual
            Scenes.Scene.getCurrentScene().Update(args.Time);

            // Atualiza os objetos da cena atual
            List<GameObject> gameObjects = Scenes.Scene.getCurrentScene().GetGameObjects();
            foreach(GameObject obj in gameObjects)
            {
                obj.Update((float)args.Time);
            }
        }   

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            frameCount++;
            
            _imGUIcontroller.Update(this, (float)args.Time);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); // Limpa o buffer de cores e profundidade

            // Se não exister cena nenhuma, não renderize nada
            if (Scenes.Scene.getCurrentScene() == null){base.SwapBuffers();return;};

            // Renderiza os voxels da cena atual
            List<Components.Voxel> voxels = Scenes.Scene.getCurrentScene().GetVoxels();
            foreach(Components.Voxel obj in voxels)
            {
                obj.Render(args.Time);
            }

            // TO DO : Renderização de GameObjects

            // Atualiza a matriz de projeção com a projeção da camera
            if(currentCamera != null) 
            {
                projectionMatrix = currentCamera.GetProjectionMatrix();
                shader.SetMatrix4("projection", projectionMatrix);


                viewMatrix = currentCamera.GetViewMatrix();
                shader.SetMatrix4("view", viewMatrix);
            }

            Scenes.Scene.getCurrentScene().Render(args.Time);

            _imGUIcontroller.Render();

            // Troca o buffer do front-end pelo back-end
            base.SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            //Console.WriteLine(Size.X + " / " + )
            GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            Console.WriteLine(Window.game.ClientRectangle.Size.X);

            if(currentCamera != null)
            {
                projectionMatrix = currentCamera.GetProjectionMatrix();

            }

            _imGUIcontroller.WindowResized(ClientSize.X, ClientSize.Y);
        }
        
        public override void Run()
        {
            Console.WriteLine("Run chamado");
  
            base.Run();
        }

        public override void Close()
        {
            base.Close();
        }

        protected override void OnTextInput(TextInputEventArgs e)
        {
            base.OnTextInput(e);


            _imGUIcontroller.PressChar((char)e.Unicode);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            _imGUIcontroller.MouseScroll(e.Offset);
        }
    }
}
