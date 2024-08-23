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
using VoxelEngine.Core;

namespace GameTest.Maps
{
    internal class World : Scene
    {
        float Yaw = 0f;
        float Pitch = 0f;
        float Fov = 45f;

        bool wireframe = false;

        bool cameraMode = false;

        float rotation = 0;
        float rotation2 = 0;
        int direction = 0;
        bool break_mesh = false;

        Chunk current;

        public void DebugInterface()
        {
            ImGui.Begin("Geral");

            ImGui.Text($"Total Triangles : {Mesh.TotalTriangles}");
            ImGui.Text($"FPS : {Window.FPS}");


            ImGui.SliderFloat("Camera FoV", ref Fov, 0f, 90f);

            //ImGui.SliderFloat("Camera Yaw", ref Yaw, 0f, 90f);
            //ImGui.SliderFloat("Camera Pitch", ref Pitch, 0f, 90f);

            if(ImGui.SliderFloat("Debug Mesh", ref rotation, 0f, 90f))
            {
                foreach(var mesh in current.debugMesh)
                {

                    mesh.transform.rotation.X = rotation;
                    mesh.transform.rotation.Y = rotation;
                    //mesh.transform.rotation.Z = rotation;
                }
            }

            if (ImGui.SliderFloat("Minha Mesh", ref rotation2, 0f, 90f))
            {
                foreach (var mesh in current.greedyMeshes)
                {
                    mesh.transform.rotation.X = rotation2;
                    mesh.transform.rotation.Y = rotation2;
                    //mesh.transform.rotation.Z = rotation;
                }
            }

            if(ImGui.InputInt("Direction Start", ref direction))
            {
                current.start_direction = direction;
            }

            if(ImGui.Checkbox("Break Meshe", ref break_mesh))
            {
                current.break_mesh = break_mesh;
            }

            if (ImGui.Button("Destroy Meshes"))
            {
                foreach(var mesh in current.debugMesh)
                {
                    if (mesh == null) continue;
                    mesh.Destroy();
                }
  
                //foreach (var mesh in current.greedyMeshes)
                //{
                //    mesh.Destroy();
                //}
            }

            if (ImGui.Button("Greedy Mesh"))
            {
                current.GreedyMesh();
            }

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
            current = new Chunk();

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
