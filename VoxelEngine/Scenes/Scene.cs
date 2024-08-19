using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelEngine.Objects.Primordial;

namespace VoxelEngine.Scenes
{
    // Responsável pelas entidades e meshes do mapa
    public abstract class Scene
    {
        public static Scene currentScene;

        // Array de meshes para serem renderizadas
        internal List<Mesh> meshes = new List<Mesh>();

        // Lista de GameObjects para serem atualizados
        internal List<GameObject> gameObjects = new List<GameObject>();

        public Scene()
        {
            currentScene = this;
        }

        public virtual void onLoad() { }

        internal virtual void UpdateObjects()
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update();
            }
        }

        public virtual void Update() { }

        public virtual void onRender() { }

        internal void Render()
        {
            // Renderiza cada mesh da array
            foreach (Mesh mesh in meshes)
            {
                mesh.Render();
            }

        }

    }
}
