using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngine.Scenes
{
    public class GameObject
    {
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
