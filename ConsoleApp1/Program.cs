
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
            0.5f, 0.5f, 0f,     // Front face begin
            0.5f, -0.5f, 0f,
            -0.5f, -0.5f, 0.0f, // Front face end

            -0.5f, 0.5f, 0.0f,  // Triangle 2
            0.5f, 0.5f, 0.0f,
            -0.5f,-0.5f,0.0f    // Triangle 2
        ];

        var gameWindow = new GameWindow(new GameWindowSettings(), new NativeWindowSettings
        {
            Size = new Vector2i(800, 800),
            Title = "OpenGL in C# with OpenTK"
        });

        Console.WriteLine("Marca  : " + GL.GetString(StringName.Vendor));
        Console.WriteLine("Modelo : " + GL.GetString(StringName.Renderer));
        Console.WriteLine("Versão : " + GL.GetString(StringName.Version));

        GLFWBindingsContext bindingsContext = new GLFWBindingsContext();
        GL.LoadBindings(bindingsContext);


        gameWindow.Load += () =>
        {
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0.0f, 0.15f, 0.25f, 1.0f); // Cor de fundo

            // VAO = Um objeto que armazena os ponteiros das vertices que estão no VBO ( eu acho )
            // Bind vao (VERTEX ARRAY OBJECT)
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            // VBO = Local na memoria onde sera armazenado as vertices
            // Bind VBO (VERTEX BUFFER OBJECT)
            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices.ToArray(), BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, 0, 0);
        };

        gameWindow.RenderFrame += (sender) =>
        {
            // Processa eventos e entradas
            if (gameWindow.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.V))
            {
                gameWindow.Close();
            };

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.BindVertexArray(VAO);
            // Preparo do viewport


            // Renderiza a partir daqui


            // 0 = Onde vai começar a ler as vertices
            // 3 = E a quantidade de vertices
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length / 3);  // 2 triangles * 3 vertices
            

            gameWindow.SwapBuffers();
        };


        gameWindow.Run();
    }
}