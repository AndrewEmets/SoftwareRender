namespace SoftwareRender
{
    public class Mesh
    {
        public Vector3[] vertices;
        public int[] triangles;

        public Mesh(Vector3[] v, int[] t)
        {
            this.vertices = v;
            this.triangles = t;
        }
    }
}
