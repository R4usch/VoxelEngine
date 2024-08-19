using OpenTK.Mathematics;

namespace VoxelEngine.Objects.Primordial
{
    public class Transform
    {
        public Vector3 position = Vector3.Zero;
        public Quaternion rotation = new Quaternion(0, 0, 0);
        public Vector3 scale = Vector3.One;

        public Transform() : this(Vector3.Zero, new Quaternion(), Vector3.One) { }
        public Transform(Vector3 position) : this(position, new Quaternion(), Vector3.One) { }
        public Transform(Vector3 position, Quaternion rotation) : this(position, rotation, Vector3.One) { }
        public Transform(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }
    }
}
