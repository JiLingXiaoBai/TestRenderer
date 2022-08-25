using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRenderer
{
    internal class Vector4
    {
        public float x, y, z, w;

        public Vector4() { }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    case 3: return w;
                    default: throw new IndexOutOfRangeException("Invalid Vector4 index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    case 3: w = value; break;
                    default: throw new IndexOutOfRangeException("Invalid Vector4 index!");
                }
            }
        }

        public Vector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vector4(Vector4 v)
        {
            x = v.x; y = v.y; z = v.z; w = v.w;
        }

        public Vector4(Vector3 v)
        {
            x = v.x; y = v.y; z = v.z; w = 1;
        }

        //四维行向量和矩阵相乘
        public static Vector4 operator *(Vector4 v, Matrix4x4 m)
        {
            Vector4 newV = new Vector4();
            newV.x = v.x * m[1, 1] + v.y * m[2, 1] + v.z * m[3, 1] + v.w * m[4, 1];
            newV.y = v.x * m[1, 2] + v.y * m[2, 2] + v.z * m[3, 2] + v.w * m[4, 2];
            newV.z = v.x * m[1, 3] + v.y * m[2, 3] + v.z * m[3, 3] + v.w * m[4, 3];
            newV.w = v.x * m[1, 4] + v.y * m[2, 4] + v.z * m[3, 4] + v.w * m[4, 4];
            return newV;
        }

        public Vector3 transTo3D
        {
            get {
                if(Math.Abs(w)>float.Epsilon)
                    return new Vector3(x / w, y / w, z / w); 
                else
                    return new Vector3(0, 0, 0);
            }
        }

        public static bool operator ==(Vector4 a, Vector4 b)
        {
            return Math.Abs(a.x - b.x) < float.Epsilon
                && Math.Abs(a.y - b.y) < float.Epsilon
                && Math.Abs(a.z - b.z) < float.Epsilon
                && Math.Abs(a.w - b.w) < float.Epsilon;
        }
        public static bool operator !=(Vector4 a, Vector4 b)
        {
            return !(a == b);
        }
    }
}
