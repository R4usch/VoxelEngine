using VoxelEngine.Core;
using VoxelEngine.Scenes;
using MyGame.Scenes;

class Program
{
    static void Main(string[] args)
    {

        ShaderSettings shaderSettings = new ShaderSettings(File.ReadAllText("Shaders\\vertex.glsl"),
                File.ReadAllText("Shaders\\fragment.glsl"));

        VoxelEngine.Core.Window window = new VoxelEngine.Core.Window(800, 800, "Voxel Engine!", shaderSettings);

        Scene _scene = new World();

        Scene.loadScene(_scene);

        window.Run();
    }
}