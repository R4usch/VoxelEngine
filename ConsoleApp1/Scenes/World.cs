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
        List<VoxelEngine.Components.Voxel> voxels = new List<VoxelEngine.Components.Voxel>();
        

        public override void onLoad()
        {
            base.onLoad();

            VoxelEngine.Components.Voxel voxel = new VoxelEngine.Components.Voxel(this);

            voxels.Add(voxel);
        }

        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);

            float angle = 40f * (float)deltaTime;

            voxels[0].Rotation = new Vector3(voxels[0].Rotation.X + angle, voxels[0].Rotation.Y + angle , voxels[0].Rotation.Z + angle);

            //voxels[0]._currentAngle = voxels[0]._currentAngle + 80f * (float)deltaTime;
            //voxels[0]._currentAngle %= 360;

        }
    }
}
