using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngine.Scene
{
    public class GameObject
    {
        public GameObject() 
        {
            Scene.getCurrentScene().gameObjectManager.Push(this);

        }

        public void Destroy()
        {
            
        }

        public void Update(float deltaTime)
        {

        }

    }
}
