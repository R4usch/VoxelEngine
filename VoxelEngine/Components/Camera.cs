using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using VoxelEngine.Core;

namespace VoxelEngine.Components
{
    public class Camera
    {

        public enum VIEW_TYPE
        {
            PERSPECTIVE,
            ORTOGRAPHIC
        }


        public float _fov = MathHelper.PiOver2;

        public float depthNear = 0.1f;
        public float depthFar  = 1000.0f;

        Vector3 _front = -Vector3.UnitZ;
        Vector3 _up = Vector3.UnitY;
        Vector3 _right = Vector3.UnitX;

        // Rotação em volta do angulo X ( Radians )
        private float _pitch;

        // Rotação em volta do angulo Y ( Radians )
        private float _yaw;

        private Vector3 _position;

        public Vector3 Up => _up;
        public Vector3 Right => _right;
        public Vector3 Front => _front;



        public Vector3 Position 
        {
            get => _position;
            set
            {
                _position = value;
                
            }
        }

        // Campo de visão, indo de 1f a 90f
        public float Fov
        {
            get => MathHelper.RadiansToDegrees(_fov);
            set
            {
                var angle = MathHelper.Clamp(value, 1f, 90f);
                _fov = MathHelper.DegreesToRadians(angle);
            }
        }

        // Aparentemente, converter degrees para radians melhora a perfomance |     Contexto = We convert from degrees to radians as soon as the property is set to improve performance.
        public float Pitch
        {
            get => MathHelper.RadiansToDegrees(_pitch);
            set
            {
                var angle = MathHelper.Clamp(value, -89f, 89f);
                _pitch = MathHelper.DegreesToRadians(angle);
                UpdateVectors();
            }
        }

        public float Yaw
        {
            get => MathHelper.RadiansToDegrees(_yaw);
            set
            {
                _yaw = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            }
        }

        public Camera(float fov) 
        {
            Fov = fov;
        }

        internal Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + _front, _up);
        }


        internal Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(_fov, 
                                                        (float)Window.game.ClientRectangle.Size.X / Window.game.ClientRectangle.Size.Y, 
                                                        depthNear, 
                                                        depthFar);
        }

        private void UpdateVectors()
        {
            // Primeiramente, a front é calculado
            
            _front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
            _front.Y = MathF.Sin(_pitch);
            _front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);

            _front = Vector3.Normalize(_front);

            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(_right, _front));

        }

        //private float UpdateVectors()
        //{

        //}

        public void ChangeCamera()
        {
            Window.currentCamera = this;
        }
    }
}
