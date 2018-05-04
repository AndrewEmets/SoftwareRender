using System;

namespace SoftwareRender
{
    public struct Vector3
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
            for (int i = 0; i < 4; i++)            
                this[i] *= scaleVec[i];            
        }

        public override string ToString()
        {
            return string.Format("{0};{1};{2}", x, y, z);
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

    }
}
