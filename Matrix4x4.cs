using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRenderer
{
    internal class Matrix4x4
    {
        private float[,] pts;
        public Matrix4x4()
        {
            pts = new float[4, 4];
        }

        public float this[int i, int j] 
        {
            get { return pts[i - 1, j - 1]; }
            set { pts[i - 1, j - 1] = value; }
        }
        
        //两个矩阵相乘
        public static Matrix4x4 operator * (Matrix4x4 m1, Matrix4x4 m2)
        {
            Matrix4x4 newM = new Matrix4x4();

            for(int w = 1; w <=4; w++)
            {
                for(int h = 1; h <=4; h++)
                {
                    for(int n = 1; n <=4; n++)
                    {
                        newM[w, h] += m1[w, n] * m2[n, h];
                    }
                }
            }

            return newM;
        }

        //四维行向量和矩阵相乘
        public static Vector4 operator * (Vector4 v, Matrix4x4 m)
        {
            Vector4 newV = new Vector4();
            newV.x = v.x * m[1, 1] + v.y * m[2, 1] + v.z * m[3, 1] + v.w * m[4, 1];
            newV.y = v.x * m[1, 2] + v.y * m[2, 2] + v.z * m[3, 2] + v.w * m[4, 2];
            newV.z = v.x * m[1, 3] + v.y * m[2, 3] + v.z * m[3, 3] + v.w * m[4, 3];
            newV.w = v.x * m[1, 4] + v.y * m[2, 4] + v.z * m[3, 4] + v.w * m[4, 4];
            return newV;
        }

        public static Matrix4x4 Transpose(Matrix4x4 m)
        {
            Matrix4x4 transposeMatrix = new Matrix4x4();
            for(int i = 1; i <= 4; i++)
            {
                for(int j = 1; j <=4; j++)
                {
                    transposeMatrix[i, j] = m[j, i];
                }
            }
            return transposeMatrix;
        }

        public Matrix4x4 transposed
        {
            get { return Transpose(this); }
        }

        static Matrix4x4 IdentityMatrix4x4
        {
            get {
                Matrix4x4 matrix = new Matrix4x4();
                matrix[1, 1] = 1;
                matrix[2, 2] = 1;
                matrix[3, 3] = 1;
                matrix[4, 4] = 1;
                return matrix;
            }
        }

        public static Matrix4x4 identity => IdentityMatrix4x4 as Matrix4x4;
    }
}
