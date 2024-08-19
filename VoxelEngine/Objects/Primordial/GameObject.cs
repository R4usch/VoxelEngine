using OpenTK.Mathematics;

namespace VoxelEngine.Objects.Primordial
{
    public abstract  class GameObject
    {
        public Transform transform = new();

        internal GameObject()
        {
            Scenes.Scene.currentScene.gameObjects.Add(this);
        }

        public abstract void Update();

        public void Destroy()
        {

        }

    }
}
