using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.OpenGL;

namespace VoxelEngine.Core
{
    internal class Shader
    {
        internal int Handle;

        int VertexShader;
        int FragmentShader;
        internal Shader(ShaderSettings settings)
        {
            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, settings.vertex);

            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, settings.fragment);

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
            LinkProgram();

            // Após linkar o programa, podemos deletar e limpar os shaders
            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(VertexShader);
            GL.DeleteShader(FragmentShader);
        }

        internal void LinkProgram()
        {
            GL.LinkProgram(Handle);
        }

        internal void Use()
        {
            GL.UseProgram(Handle);

            // Check for linking errors
            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int)All.True)
            {
                // We can use `GL.GetProgramInfoLog(program)` to get information about the error.
                throw new Exception($"Error occurred whilst linking Program({Handle})");
            }
        }
    }

    public class ShaderSettings
    {
        public string vertex;
        public string fragment;
        public ShaderSettings(string _vertex, string _fragment)
        {
            vertex = _vertex;
            fragment = _fragment;
        }
    }
}
