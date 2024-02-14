using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ImGuiNET;
using VoxelEngine.Components;
using OpenTK.Windowing.Common;
using VoxelEngine.Mathf;

namespace MyGame.Scenes
{
    public class World : VoxelEngine.Scenes.Scene
    {
        List<VoxelEngine.Components.Voxel> voxels = new List<VoxelEngine.Components.Voxel>();


        Camera _camera;

        int _voxels = 64;
        float freq = 1;
        float amp = 1;
        float GRID_SIZE = 16;
        int octaves = 4;
        float persistance = 12;
        public override void onLoad()
        {
            base.onLoad();

            GenerateCubes();

            _camera = new Camera(45f);
            _camera.ChangeCamera();

            //VoxelEngine.Core.Window.game.CursorState = CursorState.Grabbed;
        }

        void GenerateCubes()
        {
            foreach(Voxel voxel1 in voxels)
            {
                voxel1.Destroy();
            }
            
    

            for (int i = 0; i < _voxels; i++)
            {
                for (int ii = 0; ii <= 16; ii++)
                {
                    // Bloco do topo
                    Color4 color = new Color4(VoxelEngine.Mathf.Random.NextFloat(0, 1), VoxelEngine.Mathf.Random.NextFloat(0, 1),
                                                                                        VoxelEngine.Mathf.Random.NextFloat(0, 1),
                                                                                        1f);

                    VoxelEngine.Components.Voxel voxel = new VoxelEngine.Components.Voxel(this, color);

                    
                    //y = OpenTK.Mathematics.MathHelper.Sin(ii);
                    voxel.Position = new Vector3(ii, -1, -(i));
                    voxels.Add(voxel);

                    // Desenha para baixo

                    //if (y > 0)
                    //{
                    //    for(int _y = 0; _y < y; _y++)
                    //    {

                    //        VoxelEngine.Components.Voxel _voxel = new VoxelEngine.Components.Voxel(this, color);
                    //        _voxel.Position = new Vector3(ii, _y, -(i));
                    //        voxels.Add(_voxel);
                    //    }
                    //}

                }
            }
        }

        void LoadDebug()
        {

            ImGui.Begin("Perlin Noise");
            ImGui.SliderFloat("Grid_Size", ref GRID_SIZE, 0, 1000);
            ImGui.SliderFloat("Amplitude", ref amp, 0, 100);
            ImGui.SliderFloat("Frequencia", ref freq, 0, 100);
            ImGui.SliderInt("Octaves", ref octaves, 0, 100);
            ImGui.SliderFloat("Persistance", ref persistance, 0, 100);
            //if (ImGui.Button("Opa"))
            //{
            //    //Perlin perlin = new Perlin();

            //    //Console.WriteLine(perlin.perlin(1, 1, 1));
            //}

            if (ImGui.Button("Generate"))
            {
                GenerateCubes();
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

        float speed = 5f;
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
            if (VoxelEngine.Core.Window.game.IsKeyDown(Keys.K))
            {
                _camera.Fov += 40f * (float)deltaTime;
            }
            if (VoxelEngine.Core.Window.game.IsKeyDown(Keys.J))
            {
                _camera.Fov -= 40f * (float)deltaTime;
            }
            if (VoxelEngine.Core.Window.game.IsKeyDown(Keys.A))
            {
                _camera.Position = new Vector3(_camera.Position.X + -(speed * (float)deltaTime), _camera.Position.Y, _camera.Position.Z);
            }
            if (VoxelEngine.Core.Window.game.IsKeyDown(Keys.D))
            {
                _camera.Position = new Vector3(_camera.Position.X + speed * (float)deltaTime, _camera.Position.Y, _camera.Position.Z);
            }
            if (VoxelEngine.Core.Window.game.IsKeyDown(Keys.W))
            {
                _camera.Position = new Vector3(_camera.Position.X, _camera.Position.Y, _camera.Position.Z + -(speed * (float)deltaTime));
            }
            if (VoxelEngine.Core.Window.game.IsKeyDown(Keys.S))
            {
                _camera.Position = new Vector3(_camera.Position.X, _camera.Position.Y, _camera.Position.Z + (speed * (float)deltaTime));
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
