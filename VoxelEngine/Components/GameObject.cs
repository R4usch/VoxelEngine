using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelEngine.Scenes;

namespace VoxelEngine.Components
{
    public abstract class GameObject
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public GameObject()
        {
            Scene.getCurrentScene().objectManager.PushGameObject(this);

        }

        public void Destroy()
        {
            
        }

        public void Update(float deltaTime)
        {

        }
        
        

    }
}
