using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Scenes
{
    public class World : VoxelEngine.Scenes.Scene
    {

        public override void onLoad()
        {
            base.onLoad();

            VoxelEngine.Components.Voxel voxel = new VoxelEngine.Components.Voxel(VoxelEngine.Components.VoxelConstants.VOXEL_CUBE_VERTICES, this);
            voxel.Position = new Vector3(-1, 0, 0);

            VoxelEngine.Components.Voxel voxel2 = new VoxelEngine.Components.Voxel(VoxelEngine.Components.VoxelConstants.VOXEL_CUBE_VERTICES, this);
            voxel2.Position = new Vector3(0, 0, 0);
        }

        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);

            if(VoxelEngine.Core.Window.game.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.K)){
                Hell hell = new Hell();
                
            }


        }
    }
}
