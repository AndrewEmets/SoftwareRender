using System;
using System.CodeDom;

namespace SoftwareRender
{
    public struct Vector2
    {
        public float x;
        public float y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public struct Pixel
    {
        public Vector2Int coordinates;
        public Vector2 lerpValue;
    }

    public struct Vector2Int
    {
        public int x, y;
        
        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static explicit operator Vector2Int(Vector3 a)
        {
            return new Vector2Int((int) a.x, (int) a.y);
        }
    }

    public struct Vector3 : IEquatable<Vector3>
    {
        public float x, y, z, w;

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = 1;
        }

        public float this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    case 3: return w;
                    default:
                        throw new ArgumentException("Wrong vector index");
                }
            }
            set
            {
                switch (i)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    case 3: w = value; break;
                    default:
                        throw new ArgumentException("Wrong vector index");
                }
            }
        }

        public void Scale(Vector3 scaleVec)
        {
            for (var i = 0; i < 4; i++)            
                this[i] *= scaleVec[i];            
        }

        public override bool Equals(object obj)
        {
            return obj is Vector3 && Equals((Vector3) obj);
        }

        public bool Equals(Vector3 other)
        {
            for (var i = 0; i < 4; i++)
            {
                if (this[i] != other[i])
                    return false;
            }
            return true;
        }
        public override string ToString()
        {
            return $"{x:f2};{y:f2};{z:f2}";
        }

        public string ToString(string format)
        {
            return string.Format("{0};{1};{2}", x.ToString(format), y.ToString(format), z.ToString(format));
        }

        public static readonly Vector3 Zero = new Vector3(0,0,0);
        public static readonly Vector3 One = new Vector3(1, 1, 1);

        public static Vector3 operator + (Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3 operator - (Vector3 a, Vector3 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3 operator * (Vector3 a, float b)
        {
            return new Vector3(a.x * b, a.y * b, a.z * b);
        }

        public static Vector3 Cross(Vector3 a, Vector3 b)
        {
            var result = new Vector3(
                a.y*b.z - a.z*b.y,
                a.z*b.x - a.x*b.z,
                a.x*b.y - a.y*b.x
                );
            return result;
        }

        public float Magnitude()
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }

        public Vector3 Normalized()
        {
            var f = 1f / Magnitude();
            var result = new Vector3(x*f,y*f,z*f);
            return result;
        }
        public static Vector3 Normalize(Vector3 vector3)
        {
            return vector3.Normalized();
        }
        public static float Dot(Vector3 a, Vector3 b)
        {
            var result = 0f;
            for (int i = 0; i < 3; i++)
            {
                result += a[i] * b[i];
            }
            return result;
        }
    }
}
