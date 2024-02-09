using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VoxelEngine.Core
{
    internal class Shader
    {
        internal int Handle;

        int VertexShader;
        int FragmentShader;

        Dictionary<string, int> _uniformLocations;

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

            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

            _uniformLocations = new Dictionary<string, int>();

            // Loop over all the uniforms,
            for (var i = 0; i < numberOfUniforms; i++)
            {
                // get the name of this uniform,
                var key = GL.GetActiveUniform(Handle, i, out _, out _);

                // get the location,
                var location = GL.GetUniformLocation(Handle, key);

                // and then add it to the dictionary.
                _uniformLocations.Add(key, location);
            }
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
