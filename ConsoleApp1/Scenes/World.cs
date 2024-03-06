using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ImGuiNET;
using VoxelEngine.Components;
using OpenTK.Windowing.Common;
using VoxelEngine.Mathf;
using SimplexNoise;

namespace MyGame.Scenes
{
    public class World : VoxelEngine.Scenes.Scene
    {
        List<VoxelEngine.Components.Voxel> voxels = new List<VoxelEngine.Components.Voxel>();
        List<Map.Chunk> chunks = new List<Map.Chunk>();

        Camera _camera;

        int _voxels = 64;
        float freq = 1;
        int columns = 16;
        float amp = 1;
        float GRID_SIZE = 16;
        int octaves = 4;
        float persistance = 12;
        public override void onLoad()
        {
            base.onLoad();

            GenerateCubes();

            _camera = new Camera(45f);
            _camera.Position = new Vector3();
            _camera.ChangeCamera();
            _camera.Yaw = -90;
            _camera.Pitch = -90;
            _camera.Position = new Vector3(0, 90, 0);

            VoxelEngine.Core.Window.game.CursorState = CursorState.Grabbed;
        }

        void GenerateCubes()
        {
            //int seed = Randomic.NextInt(0, 999999);

            //for (int i = voxels.Count - 1; i >= 0; i--)
            //{
            //    Voxel v = voxels[i];
            //    voxels.RemoveAt(i);
            //    v.Destroy();
            //}


            FastNoiseLite noise = new FastNoiseLite();
            noise.SetSeed(Randomic.NextInt(0, 999999));
            //noise.SetFrequency(freq);
            noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);

            for(int col = 0; col < columns; col++)
            {
                for(int i = 0; i < _voxels; i++)
                {

                    float n = -(noise.GetNoise(i, col));
                    Console.WriteLine(n);

                    int y = Randomic.Next(n, -1, 32);

                    Voxel _voxel = new Voxel(new Color4(n, 
                                                        n, 
                                                        n, 
                                                        1f));
                    _voxel.Position = new Vector3(i, y, col);

                    voxels.Add(_voxel);
                }

            }
        }

        void LoadDebug()
        {

            ImGui.Begin("Noise");
            ImGui.SliderFloat("Frequencia", ref freq, 0, 100);
            ImGui.SliderInt("Colunas", ref columns, 0, 100);


            //if (ImGui.Button("Opa"))
            //{
            //    //Perlin perlin = new Perlin();

            //    //Console.WriteLine(perlin.perlin(1, 1, 1));
            //}

            if (ImGui.Button("Generate"))
            {
                GenerateCubes();
            }

            if (ImGui.Button("Print Camera Position"))
            {
                Console.WriteLine(_camera.Position.ToString());
            }

            if (ImGui.Button("Clear"))
            {
                foreach (Voxel voxel1 in voxels)
                {
                    voxel1.Destroy();
                }
            }

            ImGui.End();


        }

        float speed = 30f;
        private Vector2 _lastPos;
        private bool _firstMove = true;
        private float sensitivy = 0.15f;

        public override void Render(double deltaTime)
        {
            base.Render(deltaTime);
            LoadDebug();
        }

        public override void Update(double deltaTime)
        {
            //LoadDebug();
            base.Update(deltaTime);
            //Console.WriteLine(freq);

            if (VoxelEngine.Core.Window.game.IsKeyDown(Keys.L))
            {
                _camera.Pitch = 0;
                _camera.Yaw = 0;
                _camera.Position = Vector3.Zero;
            }
            if (VoxelEngine.Core.Window.game.IsKeyDown(Keys.K)){
                _camera.Pitch = -90;
                _camera.Yaw = -90;
                _camera.Position = new Vector3(0, 90, 0);
            }

            if (VoxelEngine.Core.Window.game.IsKeyDown(Keys.V))
            {
                VoxelEngine.Core.Window.game.CursorState = CursorState.Grabbed;
            }
            if (VoxelEngine.Core.Window.game.IsKeyDown(Keys.C))
            {
                VoxelEngine.Core.Window.game.CursorState = CursorState.Normal;
            }

            if (VoxelEngine.Core.Window.game.IsKeyDown(Keys.A))
            {
                _camera.Position -= _camera.Right * speed * (float)deltaTime;
            }
            if (VoxelEngine.Core.Window.game.IsKeyDown(Keys.D))
            {
                _camera.Position += _camera.Right * speed * (float)deltaTime;
            }
            if (VoxelEngine.Core.Window.game.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * speed * (float)deltaTime;
            }
            if (VoxelEngine.Core.Window.game.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * speed * (float)deltaTime;
            }
            if (VoxelEngine.Core.Window.game.IsKeyDown(Keys.LeftShift))
            {
                _camera.Position = new Vector3(_camera.Position.X, _camera.Position.Y + (speed * (float)deltaTime), _camera.Position.Z);
            }
            if (VoxelEngine.Core.Window.game.IsKeyDown(Keys.LeftControl))
            {
                _camera.Position = new Vector3(_camera.Position.X, _camera.Position.Y - (speed * (float)deltaTime), _camera.Position.Z);
            }

            var mouse = VoxelEngine.Core.Window.game.MouseState;

            if (_firstMove)
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            { 
                if(VoxelEngine.Core.Window.game.CursorState == OpenTK.Windowing.Common.CursorState.Grabbed)
                {
                    var deltaX = mouse.X - _lastPos.X;
                    var deltaY = mouse.Y - _lastPos.Y;
                    _lastPos = new Vector2(mouse.X, mouse.Y);

                    _camera.Yaw += deltaX * sensitivy;
                    _camera.Pitch -= deltaY * sensitivy;

                }
            }
        }
    }
}
