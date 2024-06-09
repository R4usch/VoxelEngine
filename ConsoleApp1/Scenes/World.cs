using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ImGuiNET;
using VoxelEngine.Components;
using OpenTK.Windowing.Common;
using VoxelEngine.Mathf;
using SimplexNoise;
using VoxelEngine.Components.Chunks;
using OpenTK.Graphics.OpenGL;

namespace MyGame.Scenes
{
    public class World : VoxelEngine.Scenes.Scene
    {
        List<VoxelEngine.Components.Voxel> voxels = new List<VoxelEngine.Components.Voxel>();
        List<Map.Chunk> chunks = new List<Map.Chunk>();

        Camera _camera;

        float freq = 1;
        static int chunks_quantity = 1;
        public static bool debug_lines = false;
        public static bool colored_voxels = true;
        public static bool invert_x = false;

        public static bool wireframe = false;

        int cameraMode = 2;

        //VoxelEngine.Components.Chunk.Voxel _voxel;

        VoxelEngine.Components.Chunks.Chunk chunk;

        public override void onLoad()
        {
            base.onLoad();

            _camera = new Camera(45f);

            UpdateCamera();
            _camera.ChangeCamera();

            chunk = new Chunk();

            //VoxelEngine.Core.Window.game.CursorState = CursorState.Grabbed;
        }

        //void GenerateCubes()
        //{
        //    Console.WriteLine("Gerando cubos");
        //    for(int i = 0; i < chunks.Count; i++)
        //    {
        //        chunks[i].Destroy();
        //    }

        //    for(int i = 0; i < chunks.Count; i++)
        //    {
        //        chunks.Remove(chunks[i]);
        //    }

        //    FastNoiseLite noise = new FastNoiseLite();
        //    noise.SetSeed(Randomic.NextInt(1111111, 9999999));
        //    noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);

        //    for(int i = 0; i < chunks_quantity; i++)
        //    {
        //        for(int ii = 0; ii < chunks_quantity; ii++)
        //        {
        //            MyGame.Map.Chunk _chunk = new MyGame.Map.Chunk((invert_x ? -1 : 1) * (i * MyGame.Map.Chunk.CHUNK_SIZE), ii * MyGame.Map.Chunk.CHUNK_SIZE, noise);
        //            chunks.Add(_chunk);

        //        }
        //    }
        //}

        void LoadDebug()
        {
            ImGui.Begin("Geral");

            if(ImGui.Checkbox("Wireframe", ref wireframe))
            {
                Console.WriteLine("Oba");
                if (wireframe)
                {
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                }
                else
                {
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                }
            }

            if (ImGui.Button("Print Camera Position"))
            {
                Console.WriteLine(_camera.Position.ToString());
                Console.WriteLine(_camera.Yaw.ToString());
                Console.WriteLine(_camera.Pitch.ToString());
            }
            ImGui.End();


            ImGui.Begin("Greedy Meshing");


            ImGui.End();
        }

        float speed = 30f;
        private Vector2 _lastPos;
        private bool _firstMove = true;
        private float sensitivy = 0.15f;

        public override void Render(double deltaTime)
        {
            base.Render(deltaTime);
            chunk.Render();
            LoadDebug();
        }

        void UpdateCamera()
        {
            switch (cameraMode)
            {
                case 0:
                    _camera.Yaw = -90;
                    _camera.Pitch = -90;
                    _camera.Position = new Vector3(0, 90, 0);
                    break;
                case 1:
                    _camera.Yaw = 0;
                    _camera.Pitch = 0;
                    _camera.Position = Vector3.Zero;
                    break;
                case 2:
                    _camera.Yaw = 45f;
                    _camera.Pitch = -24f;
                    _camera.Position = new Vector3(-8f, 8f, -8f);
                    break;
            }
        }

        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);

            // Cameras
            if (VoxelEngine.Core.Window.game.IsKeyDown(Keys.J))
            {
                cameraMode = 2;
                UpdateCamera();
            }
            if (VoxelEngine.Core.Window.game.IsKeyDown(Keys.L))
            {
                cameraMode = 1;
                UpdateCamera();
            }
            if (VoxelEngine.Core.Window.game.IsKeyDown(Keys.K)){
                cameraMode = 0;
                UpdateCamera();
            }

            // Mouse
            if (VoxelEngine.Core.Window.game.IsKeyDown(Keys.V))
            {
                VoxelEngine.Core.Window.game.CursorState = CursorState.Grabbed;
            }
            if (VoxelEngine.Core.Window.game.IsKeyDown(Keys.C))
            {
                VoxelEngine.Core.Window.game.CursorState = CursorState.Normal;
            }

            // Movement
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
                if(cameraMode == 0)
                {
                    _camera.Position += _camera.Up * speed * (float)deltaTime;

                }
                else
                {
                    _camera.Position += _camera.Front * speed * (float)deltaTime;

                }
            }
            if (VoxelEngine.Core.Window.game.IsKeyDown(Keys.S))
            {
                if (cameraMode == 0)
                {
                    _camera.Position -= _camera.Up * speed * (float)deltaTime;

                }
                else
                {
                    _camera.Position -= _camera.Front * speed * (float)deltaTime;

                }
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
