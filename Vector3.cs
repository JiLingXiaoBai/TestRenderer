using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRenderer
{
    internal class Vector3
    {
        public const float kEpsilon = 0.00001f;

        public float x, y, z;

        public float this[int index]
        {
            get {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    default: throw new IndexOutOfRangeException("Invalid Vector3 index!");
                }
            }
            set {
                switch (index)
                {
                    case 0: x=value; break;
                    case 1: y=value; break;
                    case 2: z=value; break;
                    default: throw new IndexOutOfRangeException("Invalid Vector3 index!");
                }
            }
        }

        public Vector3() { }

        public Vector3(float x, float y, float z)
        {
            this.x = x; this.y = y; this.z = z;
        }

        public Vector3(Vector3 v)
        {
            x = v.x; y = v.y; z = v.z;
        }

        //三维向量叉积
        public static Vector3 CrossProduct(Vector3 v1, Vector3 v2)
        {
            return new Vector3(
                v1.y * v2.z - v1.z * v2.y,
                v1.z * v2.x - v1.x * v2.z,
                v1.x * v2.y - v1.y * v2.x);
        }

        public static float DotProduct(Vector3 v1, Vector3 v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }

        public static Vector3 operator + (Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Vector3 operator - (Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static Vector3 operator -(Vector3 v)
        {
            return new Vector3(-v.x, -v.y, -v.z);
        }

        public static Vector3 operator * (float d, Vector3 v)
        {
            return new Vector3(d * v.x, d * v.y, d * v.z);
        }

        public static Vector3 operator * (Vector3 v, float d)
        {
            return new Vector3(d * v.x, d * v.y, d * v.z);
        }

        public static Vector3 operator / (Vector3 v, float d)
        {
            return new Vector3(v.x / d, v.y / d, v.z / d);
        }

        public static float Magnitude(Vector3 v)
        {
            return (float)Math.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
        }

        public float magnitude
        {
            get { return (float)Math.Sqrt(x * x + y * y + z * z); }
        }

        public static Vector3 Normalize(Vector3 v)
        {
            float mag = Magnitude(v);
            if (mag > kEpsilon)
                return v / mag;
            else
                return new Vector3(0, 0, 0);
        }

        public Vector3 normalized { 
            get { return Normalize(this); }
        }
    }
}
