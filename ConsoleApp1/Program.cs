
using OpenTK.Graphics;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using VoxelEngine.Core;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;


class Program
{
    static int VAO; // VERTEX ARRAY OBJECT
    static int VBO; // VERTEX BUFFER OBJECT
    static void Main(string[] args)
    {

        float[] vertices = 
        [
                
                0.5f, 0.5f, 0.0f,
                -0.5f, -0.5f, 0.0f,
                0.5f, -0.5f, 0.0f
        ];

        float[] vertices2 = 
        [
                
                -0.5f, 0.5f, 0.0f,
                -0.5f, -0.5f, 0.0f,
                0.5f, -0.5f, 0.0f
        ];

        var gameWindow = new GameWindow(new GameWindowSettings(), new NativeWindowSettings
        {
            Size = new Vector2i(800, 600),
            Title = "OpenGL in C# with OpenTK"
        });

        Console.WriteLine("Marca  : " + GL.GetString(StringName.Vendor));
        Console.WriteLine("Modelo : " + GL.GetString(StringName.Renderer));
        Console.WriteLine("Versão : " + GL.GetString(StringName.Version));

        GLFWBindingsContext bindingsContext = new GLFWBindingsContext();
        GL.LoadBindings(bindingsContext);


        gameWindow.Load += () =>
        {

            // Bind vao (VERTEX ARRAY OBJECT)
            GL.ClearColor(0.0f, 0.15f, 0.25f, 1.0f); // Cor de fundo
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);


            // Bind VBO (VERTEX BUFFER OBJECT)
            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices.ToArray(), BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, 0,0);
        };

        gameWindow.RenderFrame += (sender) =>
        {
            // Processa eventos e entradas
            if (gameWindow.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.V))
            {
                gameWindow.Close();
            };

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Preparo do viewport


            // Renderiza a partir daqui

            GL.BindVertexArray(VAO);

            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
            

            gameWindow.SwapBuffers();
        };


        gameWindow.Run();
    }
}