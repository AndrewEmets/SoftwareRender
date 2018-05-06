using System;
using System.Text;

namespace SoftwareRender
{
    public struct Matrix4X4 : IEquatable<Matrix4X4>
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

        public Matrix4X4(float[,] m)
        {
            if (m.GetLength(0) != 4 || m.GetLength(1) != 4)
                throw  new ArgumentException();

            mat = m;
        }

        public static Matrix4X4 operator *(Matrix4X4 a, Matrix4X4 b)
        {
            var result = Zero;

            for (var i = 0; i < 4; i++)
            for (var j = 0; j < 4; j++)
            {
                var c = 0f;
                for (var r = 0; r < 4; r++)
                    c += a[i, r] * b[r, j];
                result[i, j] = c;
            }

            return result;
        }

        public static Vector3 operator *(Matrix4X4 a, Vector3 b)
        {
            var result = Vector3.Zero;
            for (var i = 0; i < 4; i++)
            {
                var c = 0f;
                for (var j = 0; j < 4; j++)
                {
                    c += a[i, j] * b[j];
                }
                result[i] = c;
            }

            return result;
        }

        public static readonly Matrix4X4 Zero;
        public static readonly Matrix4X4 One;
        static Matrix4X4()
        {
            Zero = new Matrix4X4 {mat = new float[4, 4]};

            One = new Matrix4X4 {mat = new float[4, 4]};
            for (var i = 0; i < 4; i++)
                One.mat[i, i] = 1;
        }

        public static Matrix4X4 Translation(Vector3 offset)
        {
            var matrix = Matrix4X4.One;

            for (var i = 0; i < 3; i++)
                matrix[i, 3] = offset[i];

            return matrix;
        }

        public static Matrix4X4 Scale(Vector3 scale)
        {
            var matrix = new Matrix4X4 {
                mat = new float[4, 4] };

            for (var i = 0; i < 3; i++)
                matrix[i, i] = scale[i];

            matrix[3, 3] = 1;

            return matrix;
        }

        public static Matrix4X4 RotationX(float angle)
        {
            var cos = (float) Math.Cos(angle);
            var sin = (float) Math.Sin(angle);
            var result = new Matrix4X4(new[,]
            {
                {1, 0, 0, 0},
                {0, cos, -sin, 0},
                {0, sin, cos, 0},
                {0, 0, 0, 1}
            });
            return result;
        }

        public static Matrix4X4 RotationY(float angle)
        {
            var cos = (float) Math.Cos(angle);
            var sin = (float) Math.Sin(angle);

            var result = new Matrix4X4(new[,]
            {
                {cos, 0, sin, 0},
                {0, 1, 0, 0},
                {-sin, 0, cos, 0},
                {0, 0, 0, 1}
            });
            return result;
        }

        public static Matrix4X4 RotationZ(float angle)
        {
            var cos = (float)Math.Cos(angle);
            var sin = (float)Math.Sin(angle);

            var result = new Matrix4X4(new[,]
            {
                {cos, -sin, 0, 0},
                {sin, cos, 0, 0},
                {0, 0, 1, 0},
                {0, 0, 0, 1},
            });
            return result;
        }
        public static Matrix4X4 Rotation(Vector3 eulerAngles)
        {
            var x = RotationX(eulerAngles.x);
            var y = RotationY(eulerAngles.y);
            var z = RotationZ(eulerAngles.z);
            return x * y * z;
        }

        public bool Equals(Matrix4X4 other)
        {
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    if (this[i, j] != other[i, j])
                        return false;
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is Matrix4X4 && Equals((Matrix4X4) obj);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    sb.Append(this[i, j].ToString("F2") + " ");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}