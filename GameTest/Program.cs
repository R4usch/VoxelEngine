using GameTest.Maps;
using VoxelEngine.Objects.Primordial;
using VoxelEngine.Scenes;
class Program
{
    public static void Main()
    {
        VoxelEngine.Core.Window window = new VoxelEngine.Core.Window(1024, 768, "GameTest");

        World scene = new World();

        window.Run();


    }
}