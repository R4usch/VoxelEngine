
using OpenTK.Graphics;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using VoxelEngine.Core;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;


public class Shader
{
    int Handle;

    public Shader(string vertexPath, string fragmentPath)
    {
        string VertexShaderSource = File.ReadAllText(vertexPath);

        string FragmentShaderSource = File.ReadAllText(fragmentPath);

        int VertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(VertexShader, VertexShaderSource);

        int FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(FragmentShader, FragmentShaderSource);

        GL.CompileShader(VertexShader);

        GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int success);
        if (success == 0)
        {
            string infoLog = GL.GetShaderInfoLog(VertexShader);
            Console.WriteLine(infoLog);
        }

        GL.CompileShader(FragmentShader);

        GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out int success2);
        if (success2 == 0)
        {
            string infoLog = GL.GetShaderInfoLog(FragmentShader);
            Console.WriteLine(infoLog);
        }

        Handle = GL.CreateProgram();

        GL.AttachShader(Handle, VertexShader);
        GL.AttachShader(Handle, FragmentShader);

        GL.LinkProgram(Handle);

        GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int success3);
        if (success3 == 0)
        {
            string infoLog = GL.GetProgramInfoLog(Handle);
            Console.WriteLine(infoLog);
        }
    }
    public void Use()
    {
        GL.UseProgram(Handle);
    }
    private bool disposedValue = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            GL.DeleteProgram(Handle);

            disposedValue = true;
        }
    }

    ~Shader()
    {
        if (disposedValue == false)
        {
            Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
        }
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
class Program
{
    static int VAO; // VERTEX ARRAY OBJECT
    static int VBO; // VERTEX BUFFER OBJECT

    static int CBO; // COLOR BUFFER OBJECT

    static Shader shader;

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
            // Carregar shaders

            shader = new Shader("..\\VoxelEngine\\Shaders\\vertex.glsl",
                "..\\VoxelEngine\\Shaders\\fragment.glsl");


            // Defaults
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

            // Copia os dados da vertex para o buffer de memoria
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices.ToArray(), BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            shader.Use();
        };

        gameWindow.Resize += (sender) =>
        {
            GL.Viewport(0, 0, sender.Width, sender.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();


            // Perspectiva de camera
            // Campo de visão ( FOV / FIELD OF VIEW )
            float fieldOfView = 2.0f;

            // Aspect ratio eu acho
            float aspectRatio = sender.Width / sender.Height;

            // Distancia de perto
            float depthNear = 1.0f;

            // Distancia de longe
            float depthFar = 100.0f;

            Matrix4 matrix = Matrix4.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, depthNear, depthFar);
            GL.LoadMatrix(ref matrix);
            
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