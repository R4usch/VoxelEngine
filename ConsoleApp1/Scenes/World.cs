using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VoxelEngine.Components;

namespace MyGame.Scenes
{
    public class World : VoxelEngine.Scenes.Scene
    {
        List<VoxelEngine.Components.Voxel> voxels = new List<VoxelEngine.Components.Voxel>();


        Camera _camera;

        public override void onLoad()
        {
            base.onLoad();

            int _voxels = 500;
            for (int i = 0; i < _voxels; i++)
            {
                VoxelEngine.Components.Voxel voxel = new VoxelEngine.Components.Voxel(this, new Color4(VoxelEngine.Utils.Math.Random.NextFloat(0, 1), 
                                                                                                        VoxelEngine.Utils.Math.Random.NextFloat(0, 1),
                                                                                                        VoxelEngine.Utils.Math.Random.NextFloat(0, 1),
                                                                                                        1f));
                float x = 0;
                voxel.Position = new Vector3(x, -1, -(i));
                voxels.Add(voxel);
            }

            _camera = new Camera(45f);
            _camera.ChangeCamera();

            //VoxelEngine.Core.Window.game.CursorState = CursorState.Grabbed;
        }

        float speed = 5f;
        private Vector2 _lastPos;
        private bool _firstMove = true;
        private float sensitivy = 0.15f;
        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);

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

            var mouse = VoxelEngine.Core.Window.game.MouseState;

            if (_firstMove)
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
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
