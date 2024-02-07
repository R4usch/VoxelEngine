
using OpenTK.Graphics;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;

using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using VoxelEngine.Core;

public class Shader
{
    public int Handle;

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
class Program2
{
    static int VAO; // VERTEX ARRAY OBJECT
    static int VBO; // VERTEX BUFFER OBJECT

    static int CBO; // COLOR BUFFER OBJECT

    static Shader shader;

    static Stopwatch timer; // Objeto stopwatch. Faz a contagem de tempo desde a inicialização do projeto
    public static float[] quad =  
    [
        // Face da frente
        // Posição          //Cores       
            0.5f,  0.5f, 0f,   1.0f, 0.0f, 0.0f,  // Topo  direita   // Face frente begin
            0.5f, -0.5f, 0f,   0.0f, 1.0f, 0.0f,  // Baixo direita
        -0.5f, -0.5f, 0f,   0.0f, 0.0f, 1.0f,  // Baixo esquerda // Triangle 1 End

        -0.5f, 0.5f, 0.0f,  0.0f, 1.0f, 0.0f,  // Topo esquerda  // Triangle 2 Begin
            0.5f, 0.5f, 0.0f,  1.0f, 0.0f, 0.0f,  // Topo direita
        -0.5f,-0.5f, 0.0f,  0.0f, 0.0f, 1.0f,   // Baixo esquerda // Face frente end

            0.5f,  0.5f, -1f,   1.0f, 0.0f, 0.0f,  // Topo  direita  // Face back begin
            0.5f, -0.5f, -1f,   0.0f, 1.0f, 0.0f,  // Baixo direita
        -0.5f, -0.5f, -1f,   0.0f, 0.0f, 1.0f,  // Baixo esquerda // Triangle 1 End

        -0.5f, 0.5f, -1f,  0.0f, 1.0f, 0.0f,  // Topo esquerda    // Triangle 2 Begin
            0.5f, 0.5f, -1f,  1.0f, 0.0f, 0.0f,  // Topo direita
        -0.5f,-0.5f, -1f,  0.0f, 0.0f, 1.0f,   // Baixo esquerda   // Face b

        -0.5f, 0.5f, -0f,  0.0f, 1.0f, 0.0f,  // Topo esquerda    // Triangle 2 Begin
            0.5f, 0.5f, -1f,  1.0f, 0.0f, 0.0f,  // Topo direita
        -0.5f, 0.5f, -1f,  0.0f, 0.0f, 1.0f,   // Baixo esquerda   // Face back end

            0.5f, 0.5f, -0f,  0.0f, 1.0f, 0.0f,  // Topo esquerda    // Triangle 2 Begin
            0.5f, 0.5f, -1f,  1.0f, 0.0f, 0.0f,  // Topo direita
        -0.5f, 0.5f, -0f,  0.0f, 0.0f, 1.0f,   // Baixo esquerda   // Face back end

        -0.5f, -0.5f, -0f,  1.0f, 0.0f, 0.0f,  // Topo esquerda    // Triangle 2 Begin
            0.5f, -0.5f, -1f,  0.0f, 1.0f, 0.0f,  // Topo direita
        -0.5f, -0.5f, -1f,  0.0f, 0.0f, 1.0f,   // Baixo esquerda   // Face back end

            0.5f, -0.5f, -0f,  1.0f, 0.0f, 0.0f,  // Topo esquerda    // Triangle 2 Begin
            0.5f, -0.5f, -1f,  0.0f, 1.0f, 0.0f,  // Topo direita
        -0.5f, -0.5f, -0f,  0.0f, 0.0f, 1.0f,   // Baixo esquerda   // Face back end

        -0.5f, -0.5f, -0f,  1.0f, 0.0f, 0.0f,  // Topo esquerda    // Triangle 2 Begin
            -0.5f, 0.5f, -1f,  0.0f, 1.0f, 0.0f,  // Topo direita
        -0.5f, -0.5f, -1f,  0.0f, 0.0f, 1.0f,   // Baixo esquerda   // Face back end

        -0.5f, -0.5f, -0f,  1.0f, 0.0f, 0.0f,  // Topo esquerda    // Triangle 2 Begin
        -0.5f, 0.5f, -0f,   0.0f, 1.0f, 0.0f,  // Topo direita
        -0.5f, 0.5f, -1f,   0.0f, 0.0f, 1.0f,   // Baixo esquerda   // Face back
                                                    
        -0.5f, -0.5f, -1f,  1.0f, 0.0f, 0.0f,  // Topo esquerda    // Triangle 2 Begin
            -0.5f, 0.5f, -1f,  0.0f, 1.0f, 0.0f,  // Topo direita
        -0.5f, -0.5f, -1f,  0.0f, 0.0f, 1.0f,   // Baixo esquerda   // Face back end

        0.5f, -0.5f, -0f,  1.0f, 0.0f, 0.0f,  // Topo esquerda    // Triangle 2 Begin
        0.5f, 0.5f, -0f,  0.0f, 1.0f, 0.0f,  // Topo direita
        0.5f, 0.5f, -1f,  0.0f, 0.0f, 1.0f,   // Baixo esquerda   // Face back end

        0.5f, -0.5f, -0f, 1.0f, 0.0f, 0.0f,  // Topo esquerda    // Triangle 2 Begin
        0.5f, 0.5f, -1f,  0.0f, 1.0f, 0.0f,  // Topo direita
        0.5f, -0.5f, -1f,  0.0f, 0.0f, 1.0f,   // Baixo esquerda   // Face back end
    ];
    static void Main1(string[] args)
    {
        float[] vertices = quad;

        timer = new Stopwatch(); // Cria uma nova classe do StopWatch
        timer.Start(); // Inicia o stopwatch
        
        var gameWindow = new GameWindow(GameWindowSettings.Default, new NativeWindowSettings
        {
            Size = new Vector2i(600, 600),
            Title = "OpenGL in C# with OpenTK"
        });

        Console.WriteLine("Marca  : " + GL.GetString(StringName.Vendor));
        Console.WriteLine("Modelo : " + GL.GetString(StringName.Renderer));
        Console.WriteLine("Versão : " + GL.GetString(StringName.Version));

        GLFWBindingsContext bindingsContext = new GLFWBindingsContext();
        GL.LoadBindings(bindingsContext);

        Matrix4 model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-55.0f));

        //Matrix4 view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);

        Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 800 / 800, 0.1f, 100.0f);

        //
        Vector3 Position = new Vector3(0.0f, 0.0f, 3.0f);

        Vector3 cameraTarget = Vector3.Zero;
        Vector3 cameraDirection = Vector3.Normalize(Position - cameraTarget);

        Vector3 up = Vector3.UnitY;
        Vector3 cameraRight = Vector3.Normalize(Vector3.Cross(up, cameraDirection));

        Vector3 cameraUp = Vector3.Cross(cameraDirection, cameraRight);

        Matrix4 view = Matrix4.LookAt(new Vector3(0.0f, 0.0f, 3.0f),
             new Vector3(0.0f, 0.0f, 0.0f),
             new Vector3(0.0f, 1.0f, 0.0f));

  
        gameWindow.Load += () =>
        {
            // Carregar shaders

            shader = new Shader("..\\..\\..\\..\\VoxelEngine\\Shaders\\vertex.glsl",
                "..\\..\\..\\..\\VoxelEngine\\Shaders\\fragment.glsl");
            shader.Use();

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

            // Habilita os atributos de posição vertex na localização 0  // Stride é quantas casas vai ter que se mover para achar o próximo valor da proxima vertex
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Habilita os atributos de cor na vertex na localização 1  // Offset é para onde ele começará a ler. Como a posição fica de 0 a 3, ele começará do 3
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // Habilita o uso de shaders

            // 

            //Matrix4 rotation = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(45f));
            //Matrix4 scale = Matrix4.CreateScale(0.5f, 0.5f, 0.5f);
            //Matrix4 trans = rotation * scale;

            int location = GL.GetUniformLocation(shader.Handle, "model");
            GL.UniformMatrix4(location, true, ref model);

            location = GL.GetUniformLocation(shader.Handle, "view");
            GL.UniformMatrix4(location, true, ref view);

            location = GL.GetUniformLocation(shader.Handle, "projection");
            GL.UniformMatrix4(location, true, ref projection);


            //shader.SetMatrix4("model", model);
            //shader.SetMatrix4("view", view);
            //shader.SetMatrix4("projection", projection);

            //CursorState = CursorState.Normal;

            gameWindow.CursorState = CursorState.Grabbed;
        };

        gameWindow.Resize += (sender) =>
        {
            GL.Viewport(0, 0, sender.Width, sender.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            
        };


        Vector3 position = new Vector3(0.0f, 0.0f, 3.0f);
        Vector3 front = new Vector3(0.0f, 0.0f, -1.0f);
        Vector3 _up = new Vector3(0.0f, 1.0f, 0.0f);

        bool _firstMove = true;
        Vector2 lastPos = Vector2.Zero;

        float yaw = 0;
        float pitch = 0;
        float sensitivity = 0.2f;
        

        gameWindow.UpdateFrame += (e) =>
        {
            float speed = 1.5f;

            if (gameWindow.IsKeyDown(Keys.W))
            {
                position += front * speed * (float)e.Time; //Forward 
            }

            if (gameWindow.IsKeyDown(Keys.S))
            {
                position -= front * speed * (float)e.Time; //Backwards
            }

            if (gameWindow.IsKeyDown(Keys.A))
            {
                position -= Vector3.Normalize(Vector3.Cross(front, _up)) * speed * (float)e.Time; //Left
            }

            if (gameWindow.IsKeyDown(Keys.D))
            {
                position += Vector3.Normalize(Vector3.Cross(front,_up)) * speed * (float)e.Time; //Right
            }

            if (gameWindow.IsKeyDown(Keys.Space))
            {
                position += _up * speed * (float)e.Time; //Up 
            }

            if (gameWindow.IsKeyDown(Keys.LeftShift))
            {
                position -= _up * speed * (float)e.Time; //Down
            }


            view = Matrix4.LookAt(position, position + front, _up);

            var mouse = gameWindow.MousePosition;
            var mouseState = gameWindow.CursorState;

            Console.WriteLine(mouseState);

            if (_firstMove) 
            {
                lastPos = new Vector2(mouse.X, mouse.Y);    
                _firstMove = false;
            }
            else if (mouseState == CursorState.Grabbed)
            {
                float deltaX = mouse.X - lastPos.X;
                float deltaY = mouse.Y - lastPos.Y;
                lastPos = new Vector2(mouse.X, mouse.Y);

                yaw += deltaX * sensitivity;
                pitch -= deltaY * sensitivity;

                if (pitch > 89.0f)
                {
                    pitch = 89.0f;
                }
                else if (pitch < -89.0f)
                {
                    pitch = -89.0f;
                }
                else
                {
                    pitch -= deltaX * sensitivity;
                }

                front.X = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(yaw));
                front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(pitch));
                front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(yaw));
                front = Vector3.Normalize(front);
            }
            
        };

        gameWindow.RenderFrame += (sender) =>
        {
            // Processa eventos e entradas
            if (gameWindow.IsKeyDown(Keys.V)) 
            {
                gameWindow.Close();
            };

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); // Limpa as cores da tela e o buffer de profundidade

            // Renderiza a partir daqui

            // Rotaciona o objeto
            model = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(timer.Elapsed.TotalSeconds * 40)) * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(timer.Elapsed.TotalSeconds * 80)); ;

            //Console.WriteLine(timer.Elapsed.TotalSeconds);
            int location = GL.GetUniformLocation(shader.Handle, "model");
            GL.UniformMatrix4(location, true, ref model);

            location = GL.GetUniformLocation(shader.Handle, "view");
            GL.UniformMatrix4(location, true, ref view);

            location = GL.GetUniformLocation(shader.Handle, "projection");
            GL.UniformMatrix4(location, true, ref projection);

            // 0 = Onde vai começar a ler as vertices
            // 3 = E a quantidade de vertices

            //GL.BindVertexArray(VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length / 3); // Pega o total de vertices e divide por 3,
                                                                            // resultando na quantidade de triangulos desenhados

            // Troca o frame no back-end para o front-end
            gameWindow.SwapBuffers();
        };


        gameWindow.Run();
    }
}

class Program
{
    static void Main(string[] args)
    {

        ShaderSettings shaderSettings = new ShaderSettings(File.ReadAllText("Shaders\\vertex.glsl"),
                File.ReadAllText("Shaders\\fragment.glsl"));

        VoxelEngine.Core.Window window = new VoxelEngine.Core.Window(800, 800, "Voxel Engine!", shaderSettings, Program2.quad);

        

    }
}