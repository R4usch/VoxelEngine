using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelEngine.Objects.Primordial;
using VoxelEngine.Scenes;
using OpenTK.Graphics.OpenGL;
using ImGuiNET;
using VoxelEngine.Objects.Chunks;

namespace GameTest.Maps
{
    internal class World : Scene
    {
        float Yaw = 0f;
        float Pitch = 0f;
        float Fov = 45f;

        bool wireframe = false;

        bool cameraMode = false;
        public void DebugInterface()
        {
            ImGui.Begin("Geral");

            ImGui.SliderFloat("Camera FoV", ref Fov, 0f, 90f);

            ImGui.SliderFloat("Camera Yaw", ref Yaw, 0f, 90f);
            ImGui.SliderFloat("Camera Pitch", ref Pitch, 0f, 90f);


            if (ImGui.Checkbox("Wireframe", ref wireframe))
            {
                if (wireframe)
                {
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                }
                else
                {
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                }
            }

            if(ImGui.Checkbox("Ortographic", ref cameraMode))
            {
                if (cameraMode)
                {
                    Camera.currentCamera.mode = Camera.CameraMode.ORTOGRAPHIC;
                }
                else
                {
                    Camera.currentCamera.mode = Camera.CameraMode.PERSPECTIVE;
                }
            }

            ImGui.End();
        }

        public override void onLoad()
        {
            Camera camera = new Camera(Fov);
            Chunk chunk = new Chunk();

            Camera.currentCamera.mode = cameraMode ? Camera.CameraMode.ORTOGRAPHIC : Camera.CameraMode.PERSPECTIVE;
        }

        public override void Update()
        {
            Camera.currentCamera.Fov = Fov;
            //Camera.currentCamera.Yaw = Yaw;
            //Camera.currentCamera.Pitch = Pitch;
        }

        public override void onRender()
        {
            base.onRender();
            DebugInterface();
        }

    }
}
