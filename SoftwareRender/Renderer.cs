using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SoftwareRender
{
    public struct Matrix4x4
    {
        private float[,] mat;

        public float this[int i, int j]
        {
            get
            {
                return mat[i, j];
            }
            set
            {
                mat[i, j] = value;
            }
        }

        public static Matrix4x4 Translation(Vector3 offset)
        {
            var matrix = new Matrix4x4 {
                mat = new float[4, 4] };

            for (int i = 0; i < 3; i++)
                matrix[i, i] = 1;

            for (int i = 0; i < 2; i++)
                matrix[i, 3] = offset[i];

            return matrix;
        }

        public static Matrix4x4 Scale(Vector3 scale)
        {
            var matrix = new Matrix4x4 {
                mat = new float[4, 4] };

            for (int i = 0; i < 2; i++)
                matrix[i, i] = scale[i];

            matrix[3, 3] = 1;

            return matrix;
        }

        /*public static Matrix4x4 Rotation(Vector3 eulerAngles)
        {
            var matrix = new Matrix4x4
            {
                mat = new float[4, 4]
            };
        }*/
    }

    public class Renderer
    {
        private Vector3[,] colorBuffer;
        private float[,] depthBuffer;

        private int width, height;

        public Renderer(int screenWidth, int screenHeight)
        {
            colorBuffer = new Vector3[screenWidth, screenHeight];
            //depthBuffer = new float[screenWidth, screenHeight];

            width = screenWidth;
            height = screenHeight;
        }

        public void ClearColorBuffer()
        {
            for (int y = 0; y < colorBuffer.GetLength(1); y++)
                for (int x = 0; x < colorBuffer.GetLength(0); x++)
                    colorBuffer[x, y] = Vector3.Zero;
        }        

        public void Render(Mesh mesh)
        {
            for (int i = 0; i < mesh.triangles.Length / 3; i++)
            {
                var vert0Index = mesh.triangles[i * 3 + 0];
                var vert0 = mesh.vertices[vert0Index];

                var vert1Index = mesh.triangles[i * 3 + 1];
                var vert1 = mesh.vertices[vert1Index];

                var vert2 = mesh.vertices[mesh.triangles[i * 3 + 2]];
                ProcessVertex(ref vert0);
                ProcessVertex(ref vert1);
                ProcessVertex(ref vert2);

                DrawLine(vert0, vert1);
                DrawLine(vert1, vert2);
                DrawLine(vert2, vert0);
            }
        }

        private Vector3 scaleVec = new Vector3(1, -1, 1);
        private void ProcessVertex(ref Vector3 vert)
        {
            vert.Scale(scaleVec);
            vert.Scale(new Vector3(500, 500, 500));
            vert += new Vector3(width / 2f, height /2f, 0);            
        }

        private void Swap(ref int x0, ref int x1)
        {
            int t = x0;
            x0 = x1;
            x1 = t;
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
                for (int x = 0; x < width; x++)
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