using VoxelEngine.Core;
using VoxelEngine.Scenes;
using MyGame.Scenes;

class Program
{
    static void Main()
    {

        ShaderSettings shaderSettings = new ShaderSettings(File.ReadAllText("Shaders\\vertex.glsl"),
                File.ReadAllText("Shaders\\fragment.glsl"));

        VoxelEngine.Core.Window window = new VoxelEngine.Core.Window(800, 800, "Voxel Engine!", shaderSettings);

        World _scene = new World();

        Scene.loadScene(_scene);

        window.Run();

    }
}