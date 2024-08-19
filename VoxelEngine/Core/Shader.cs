using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace VoxelEngine.Core
{
    internal class Shader
    {
        internal int Handle;

        int VertexShader;
        int FragmentShader;

        Dictionary<string, int> _uniformLocations;

        internal Shader(string vertexPath, string fragmentPath)
        {
            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, vertexPath);

            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, fragmentPath);

            CompileShader();

        }

        internal void CompileShader()
        {
            // Compila o vertex Shader
            GL.CompileShader(VertexShader);

            // Verifica se foi compilado com sucesso
            GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(VertexShader);
                Console.WriteLine(infoLog);
            }

            // Compila o fragment shader
            GL.CompileShader(FragmentShader);

            // Verifica se foi compilado com sucesso
            GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out int success2);
            if (success2 == 0)
            {
                string infoLog = GL.GetShaderInfoLog(FragmentShader);
                Console.WriteLine(infoLog);
            }

            // Cria o programa de shader
            Handle = GL.CreateProgram();

            // Adiciona os shaders ao program
            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);

            // Linka o programa ao OpenGL
            GL.LinkProgram(Handle);

            // Verifica por errors
            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int success3);
            if (success3 == 0)
            {
                string infoLog = GL.GetProgramInfoLog(Handle);
                Console.WriteLine(infoLog);
            }

            // Após linkar o programa, podemos deletar e limpar os shaders
            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(VertexShader);
            GL.DeleteShader(FragmentShader);

            // Acessa o shader e pega a quantidade total de todas as variaveis uniformes
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);
            _uniformLocations = new Dictionary<string, int>();

            // Loop por todas as variáveis uniformes
            for (var i = 0; i < numberOfUniforms; i++)
            {
                // Retorna o nome da variável uniforme
                var key = GL.GetActiveUniform(Handle, i, out _, out _);

                // Retorna a localização da variável usando o nome
                var location = GL.GetUniformLocation(Handle, key);

                // Adiciona a variável uniforme ao dicionário
                _uniformLocations.Add(key, location);
            }
        }


        internal void Use()
        {
            GL.UseProgram(Handle);

            // Check for linking errors
            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int)All.True)
            {
                // We can use `GL.GetProgramInfoLog(program)` to get information about the error.
                string log = GL.GetProgramInfoLog(Handle);
                Console.WriteLine(log);
                throw new Exception($"Error occurred whilst linking Program({Handle})");
            }
        }

        internal void SetMatrix4(string name, Matrix4 matrix)
        {
            GL.UseProgram(Handle);

            if (!_uniformLocations.ContainsKey(name)) return;
            GL.UniformMatrix4(_uniformLocations[name], true, ref matrix);
        }
    }
}
