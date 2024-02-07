using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngine.Scene
{
    public class Scene
    {
        static Scene currentScene = new Scene();
        internal class GameObjectManager
        {
            public List<GameObject> _gameObjects = new List<GameObject>();
            public bool Push(GameObject _gameObject)
            {
                _gameObjects.Add(_gameObject);
                return true;
            }

            public bool Destroy(GameObject _gameObject)
            {
                return _gameObjects.Remove(_gameObject);
                
            }
        }

        internal GameObjectManager gameObjectManager = new GameObjectManager();


        public static Scene getCurrentScene()
        {
            return currentScene;
        }

        public static void loadScene()
        {

        }
        

        public List<GameObject> GetGameObjects()
        {
            return gameObjectManager._gameObjects;
        }

        public Scene()
        {

        }

        
    }
}
