namespace SoftwareRender
{
    public class Transform
    {
        private Vector3 position;
        public Vector3 Position
        {
            get { return position; }
        }

        private Vector3 scale;
        public Vector3 Scale
        {
            get { return scale; }
        }

        private Vector3 rotation;
        public Vector3 Rotation
        {
            get { return rotation; }
        }

        public readonly Matrix4X4 Matrix;

        public Transform(Vector3 position, Vector3 scale, Vector3 rotation)
        {
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;

            var rotm = Matrix4X4.Rotation(rotation);
            var scalem = Matrix4X4.Scale(scale);
            var translm = Matrix4X4.Translation(position);
            Matrix = rotm * scalem * translm;
        }

        public Transform()
        {
            scale = Vector3.One;
            Matrix = Matrix4X4.One;
        }
    }

    public class Mesh
    {
        public Transform transform = new Transform();

        public Vector3[] vertices;
        public int[] triangles;

        public Vector2[] uvs;
        public int[] uvTriangles;

        public Mesh(Vector3[] v, int[] t)
        {
            this.vertices = v;
            this.triangles = t;
        }
    }
}
