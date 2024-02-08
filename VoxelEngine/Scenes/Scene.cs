using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngine.Scenes
{
    public abstract class Scene
    {
        static Scene currentScene;
        public static Scene getCurrentScene()
        {
            return currentScene;
        }

        public static void loadScene(Scene _scene)
        {
            currentScene = _scene;
        }

        internal class ObjectManager
        {
            public List<GameObject> _gameObjects = new List<GameObject>();


            internal Mutex voxelMutex = new Mutex();
            public List<VoxelEngine.Components.Voxel> _voxelObjects = new List<VoxelEngine.Components.Voxel>();
            public bool PushGameObject(GameObject _gameObject)
            {
                _gameObjects.Add(_gameObject);
                return true;
            }

            public bool DestroyGameObject(GameObject _gameObject)
            {
                return _gameObjects.Remove(_gameObject);  
            }

            public bool PushVoxel(VoxelEngine.Components.Voxel _voxel)
            {

                voxelMutex.WaitOne();
                try
                {
                    _voxelObjects.Add(_voxel);
                }
                finally
                {
                    voxelMutex.ReleaseMutex();
                }

                return true;
            }

            public bool DestroyVoxel(VoxelEngine.Components.Voxel _voxel)
            {
                _voxelObjects.Add(_voxel);
                return true;
            }
        }

        internal ObjectManager objectManager = new ObjectManager();


        public int ID = new Random().Next(1, 999);
        

        public List<GameObject> GetGameObjects()
        {
            return objectManager._gameObjects;
        }

        public List<VoxelEngine.Components.Voxel> GetVoxels()
        {
            objectManager.voxelMutex.WaitOne();
            try
            {

                //Console.WriteLine(objectManager._voxelObjects.Count);
                return objectManager._voxelObjects;
                
            }
            finally
            {
                objectManager.voxelMutex.ReleaseMutex();
            }
        }

        public Scene()
        {
            this.onLoad();
        }

        public virtual void onLoad() 
        {

        }
        public virtual void Update(double deltaTime)
        {
            
        }
        
   

        
    }
}
