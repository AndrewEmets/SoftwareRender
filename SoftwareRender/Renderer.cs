using System;
using System.Drawing;

namespace SoftwareRender
{
    public class Renderer
    {
        private Vector3[,] colorBuffer;
        private float[,] depthBuffer;

        private int width, height;

        public Vector3 offset = Vector3.Zero;

        public Renderer(int screenWidth, int screenHeight)
        {
            colorBuffer = new Vector3[screenWidth, screenHeight];
            depthBuffer = new float[screenWidth, screenHeight];

            width = screenWidth;
            height = screenHeight;
        }

        public void ClearColorBuffer()
        {
            for (int y = 0; y < colorBuffer.GetLength(1); y++)
                for (int x = 0; x < colorBuffer.GetLength(0); x++)
                    colorBuffer[x, y] = Vector3.Zero;
        }
        public void ClearDepthBuffer()
        {
            for (int y = 0; y < colorBuffer.GetLength(1); y++)
                for (int x = 0; x < colorBuffer.GetLength(0); x++)
                    depthBuffer[x, y] = 0f;
        }

        public void Render(Mesh mesh)
        {
            ClearDepthBuffer();
            ClearColorBuffer();

            for (int i = 0; i < mesh.triangles.Length / 3; i++)
            {
                var vert0 = mesh.vertices[mesh.triangles[i * 3 + 0]];
                var vert1 = mesh.vertices[mesh.triangles[i * 3 + 1]];
                var vert2 = mesh.vertices[mesh.triangles[i * 3 + 2]];

                ProcessVertex(ref vert0, mesh.transform.Matrix);
                ProcessVertex(ref vert1, mesh.transform.Matrix);
                ProcessVertex(ref vert2, mesh.transform.Matrix);
                
                var normal = Vector3.Cross(vert1 - vert0, vert1 - vert2).Normalized();
                var d = Vector3.Dot(normal, lightPos);
                              
                if (d > 0)
                    DrawTriangle(vert0, vert1, vert2, d);

                /*
                DrawLine(vert0, vert1);
                DrawLine(vert1, vert2);
                DrawLine(vert2, vert0);*/
            }
        }

        private Vector3 scaleVec = new Vector3(1, -1, 1);
        private void ProcessVertex(ref Vector3 vert, Matrix4X4 modelMatrix)
        {
            vert = modelMatrix * vert;
            var min = Math.Min(width, height) * .5f;
            vert = Matrix4X4.Scale(new Vector3(min, -min, min)) * vert;
            var translate = Matrix4X4.Translation(new Vector3(width / 2f, height / 2f, 0));
            vert = translate * vert;

        }
        
        private void Swap<T>(ref T a, ref T b) where T : struct
        {
            var temp = a;
            a = b;
            b = temp;
        }

        private Vector3 lightPos = Vector3.Normalize(new Vector3(0,0,1));
        private void DrawTriangle(Vector3 v0, Vector3 v1, Vector3 v2, float d)
        {
            var v0i = (Vector2Int) v0;
            var v1i = (Vector2Int) v1;
            var v2i = (Vector2Int) v2;

            if (v0i.y > v1i.y) Swap(ref v0i, ref v1i);
            if (v0i.y > v2i.y) Swap(ref v0i, ref v2i);
            if (v1i.y > v2i.y) Swap(ref v1i, ref v2i);

            for (int y = v0i.y; y <= v1i.y; y++)
            {
                var t1 = 1f;
                if (v1i.y != v0i.y)
                    t1 = (float) (y - v0i.y) / (v1i.y - v0i.y);

                var t2 = 1f;
                if (v2i.y != v0i.y)
                    t2 = (float) (y - v0i.y) / (v2i.y - v0i.y);

                int x01 = (int) ((v1i.x - v0i.x) * t1 + v0i.x);
                int x02 = (int) ((v2i.x - v0i.x) * t2 + v0i.x);

                float z01 = (v1.z - v0.z) * t1 + v0.z;
                float z02 = (v2.z - v0.z) * t2 + v0.z;

                if (x02 < x01)
                {
                    Swap(ref x01, ref x02);
                    Swap(ref z01, ref z02);
                }

                DrawLine(x01, x02, y, d, z01, z02);
            }
            
            for (int y = v1i.y; y <= v2i.y; y++)
            {
                var t1 = 1f;
                if (v2i.y != v1i.y)
                    t1 = (float) (y - v1i.y) / (v2i.y - v1i.y);

                var t2 = 1f;
                if (v2i.y != v0i.y)
                    t2 = (float) (y - v0i.y) / (v2i.y - v0i.y);

                int x01 = (int) ((v2i.x - v1i.x) * t1 + v1i.x);
                int x02 = (int) ((v2i.x - v0i.x) * t2 + v0i.x);

                float z01 = (v2.z - v1.z) * t1 + v1.z;
                float z02 = (v2.z - v0.z) * t2 + v0.z;

                if (x02 < x01)
                {
                    Swap(ref x01, ref x02);
                    Swap(ref z01, ref z02);
                }
                
                DrawLine(x01, x02, y, d, z01, z02);
            }
        }
        private void DrawLine(int x0, int x1, int y, float d, float z0, float z1)
        {
            d = Math.Min(d, 1f);
            for (int x = x0; x <= x1; x += 1)
            {
                if (x >= 0 && x < width && y >= 0 && y < height)
                {
                    float t = (float)(x - x0) / (x1 - x0);
                    float z = z0 + (z1 - z0) * t;
                    //if (depthBuffer[x, y] < z)
                    {
                        colorBuffer[x, y] = new Vector3(d, d, d);
                        depthBuffer[x, y] = z;
                    }
                }
            }
        }

        private void DrawLine(Vector3 v0, Vector3 v1)
        {
            DrawLine((int)v0.x, (int)v0.y, (int)v1.x, (int)v1.y);
        }
        private void DrawLine(int x0, int y0, int x1, int y1)
        {
            var steep = false;

            if (Math.Abs(x0 - x1) < Math.Abs(y0 - y1))
            {
                steep = true;
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }

            if (x0  > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            var dX = x1 - x0;
            var dY = y1 - y0;
            var dx2 = 2 * dX;
            //float dError = Math.Abs((float)dY / dX);
            int dError2 = Math.Abs(dY*2);
            
            int error2 = 0;
            int yDir = y1 > y0 ? 1 : -1;
            int y = y0;

            for (int x = x0; x <= x1; x++)
            {
                if (!steep)
                {
                    if (x >= 0 && x < width && y >= 0 && y < height)
                        colorBuffer[x, y] = new Vector3(1, 1, 1);
                    else
                        break;
                }
                else
                {
                    if (x >= 0 && y < width && y >= 0 && x < height)
                        colorBuffer[y, x] = new Vector3(1, 1, 1);
                        
                }

                error2 += dError2;

                if (error2 >= 1)
                {
                    y += yDir;
                    error2 -= dx2;
                }
            }
        }

        private Bitmap bitmap;
        public Bitmap GetBitmap()
        {
            var width = colorBuffer.GetLength(0);
            var height = colorBuffer.GetLength(1);

            if (bitmap == null)
                bitmap = new Bitmap(width, height);


            for (int y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var c = colorBuffer[x, y] * 255;
                    var color = Color.FromArgb(255, (int)c.x, (int)c.y, (int)c.z);
                    
                    bitmap.SetPixel(x, y, color);
                }
            }

            return bitmap;
        }
    }
}