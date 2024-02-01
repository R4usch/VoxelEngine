
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

    static int CBO; // COLOR BUFFER OBJECT

    static void Main(string[] args)
    {

        float[] quad =  
        [
            0.5f, 0.5f, 0f,     // Triangle 1 Begin
            0.5f, -0.5f, 0f,
            -0.5f, -0.5f, 0f,   // Triangle 1 End

            -0.5f, 0.5f, 0.0f,  // Triangle 2 Begin
            0.5f, 0.5f, 0.0f,
            -0.5f,-0.5f,0.0f    // Triangle 2 End
        ];

        float[] cube =
        [
            0.5f, 0.5f, 0f,     // Front Face Begin
            0.5f, -0.5f, 0f,
            -0.5f, -0.5f, 0f,   
            -0.5f, 0.5f, 0.0f,  
            0.5f, 0.5f, 0.0f,
            -0.5f,-0.5f,0.0f    // Front Face End 
        ];

        float[] colors =
        [
            0.583f,  0.771f,  0.014f,
            0.583f,  0.771f,  0.014f,
            0.583f,  0.771f,  0.014f,
            0.583f,  0.771f,  0.014f,
            0.583f,  0.771f,  0.014f,
            0.583f,  0.771f,  0.014f,
        ];

        float[] vertices = quad;



        var gameWindow = new GameWindow(new GameWindowSettings(), new NativeWindowSettings
        {
            Size = new Vector2i(600, 600),
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

            // Color Buffer
            //CBO = GL.GenBuffer();
            //GL.BindBuffer(BufferTarget.ArrayBuffer, CBO);
            //GL.VertexAttribPointer(
            //    1,                                // attribute. No particular reason for 1, but must match the layout in the shader.
            //    3,                                // size
            //    VertexAttribPointerType.Float,    // type
            //    false,                            // normalized?
            //    0,                                // stride
            //    0                                 // array buffer offset
            //);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, 0, 0);
        };

        gameWindow.Resize += (sender) =>
        {
            GL.Viewport(0, 0, sender.Width, sender.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            // Campo de visão ( FOV / FIELD OF VIEW )
            float fieldOfView = 2.0f;
            Console.WriteLine(sender.Width);

            Console.WriteLine(sender.Height);
            // Aspect ratio eu acho
            float aspectRatio = sender.Width / sender.Height;

            // Distancia de perto
            float depthNear = 1.0f;

            // Distancia de longe
            float depthFar = 100.0f;

            Matrix4 matrix = Matrix4.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, depthNear, depthFar);
            GL.LoadMatrix(ref matrix);
            //GL.Frustum()
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