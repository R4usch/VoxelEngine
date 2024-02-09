using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace VoxelEngine.Components
{
    public class Camera
    {
        public static Camera instance;

        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }

        public float Fov;
        public Camera() 
        {
            if(instance == null)
            {
                instance = this;
            }
        }
    }
}
