using OpenTK.Windowing;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using VoxelEngine.Objects.Primordial;
using VoxelEngine.Scenes;


namespace VoxelEngine.Core
{
    public class Window : GameWindow
    {
        internal static Window window;
        internal Shader shader;

        ImGUI.ImGUIController _imGUIcontroller;

        public Window(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings
        {
            // Setando o tamanho da tela
            ClientSize = new Vector2i(width, height),
            // Setando o titulo da tela
            Title = title,
        })
        {
            window = this;

 
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            _imGUIcontroller = new ImGUI.ImGUIController(ClientSize.X, ClientSize.Y);

            shader = new Shader(File.ReadAllText(".\\Shaders\\vertex.glsl"), File.ReadAllText(".\\Shaders\\fragment.glsl"));
            shader.Use();

            if (Camera.currentCamera != null)
            {
                shader.SetMatrix4("view", Camera.currentCamera.GetViewMatrix());

            }

            // Define a cor de limpeza do fundo
            GL.ClearColor(0.0f, 0.15f, 0.25f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            if (Scene.currentScene != null)
            {
                Scene.currentScene.onLoad();
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if(Scenes.Scene.currentScene != null)
            {
                Scenes.Scene.currentScene.UpdateObjects();
                Scenes.Scene.currentScene.Update();
            }
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            _imGUIcontroller.Update(this, (float)args.Time);

            // Limpa o buffer de cores e profundidade
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (Camera.currentCamera != null)
            {
                // Atualiza a matriz de projeção, com a projeção da camera atual
                shader.SetMatrix4("projection", Camera.currentCamera.GetProjectionMatrix());

                // Atualiza a matriz de view, com a projeção da camera atual
                shader.SetMatrix4("view", Camera.currentCamera.GetViewMatrix());
            }
            if (Scenes.Scene.currentScene != null)
            {
                Scenes.Scene.currentScene.onRender();
                Scenes.Scene.currentScene.Render();
            }
            _imGUIcontroller.Render();

            // Troca o buff do front-end pelo do back-end 
            base.SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);

            GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadIdentity();

            _imGUIcontroller.WindowResized(ClientSize.X, ClientSize.Y);
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
