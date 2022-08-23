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
        

        public Matrix4x4 Mul(Matrix4x4 m)
        {
            Matrix4x4 newM = new Matrix4x4();
            /*
            newM[1, 1] = this[1, 1] * m[1, 1] + this[1, 2] * m[2, 1] + this[1, 3] * m[3, 1] + this[1, 4] * m[4, 1];
            newM[1, 2] = this[1, 1] * m[1, 2] + this[1, 2] * m[2, 2] + this[1, 3] * m[3, 2] + this[1, 4] * m[4, 2];
            newM[1, 3] = this[1, 1] * m[1, 3] + this[1, 2] * m[2, 3] + this[1, 3] * m[3, 3] + this[1, 4] * m[4, 3];
            newM[1, 4] = this[1, 1] * m[1, 4] + this[1, 2] * m[2, 4] + this[1, 3] * m[3, 4] + this[1, 4] * m[4, 4];
            newM[2, 1] = this[2, 1] * m[1, 1] + this[2, 2] * m[2, 1] + this[2, 3] * m[3, 1] + this[2, 4] * m[4, 1];
            newM[2, 2] = this[2, 1] * m[1, 2] + this[2, 2] * m[2, 2] + this[2, 3] * m[3, 2] + this[2, 4] * m[4, 2];
            newM[2, 3] = this[2, 1] * m[1, 3] + this[2, 2] * m[2, 3] + this[2, 3] * m[3, 3] + this[2, 4] * m[4, 3];
            newM[2, 4] = this[2, 1] * m[1, 4] + this[2, 2] * m[2, 4] + this[2, 3] * m[3, 4] + this[2, 4] * m[4, 4];
            newM[3, 1] = this[3, 1] * m[1, 1] + this[3, 2] * m[2, 1] + this[3, 3] * m[3, 1] + this[3, 4] * m[4, 1];
            newM[3, 2] = this[3, 1] * m[1, 2] + this[3, 2] * m[2, 2] + this[3, 3] * m[3, 2] + this[3, 4] * m[4, 2];
            newM[3, 3] = this[3, 1] * m[1, 3] + this[3, 2] * m[2, 3] + this[3, 3] * m[3, 3] + this[3, 4] * m[4, 3];
            newM[3, 4] = this[3, 1] * m[1, 4] + this[3, 2] * m[2, 4] + this[3, 3] * m[3, 4] + this[3, 4] * m[4, 4];
            newM[4, 1] = this[4, 1] * m[1, 1] + this[4, 2] * m[2, 1] + this[4, 3] * m[3, 1] + this[4, 4] * m[4, 1];
            newM[4, 2] = this[4, 1] * m[1, 2] + this[4, 2] * m[2, 2] + this[4, 3] * m[3, 2] + this[4, 4] * m[4, 2];
            newM[4, 3] = this[4, 1] * m[1, 3] + this[4, 2] * m[2, 3] + this[4, 3] * m[3, 3] + this[4, 4] * m[4, 3];
            newM[4, 4] = this[4, 1] * m[1, 4] + this[4, 2] * m[2, 4] + this[4, 3] * m[3, 4] + this[4, 4] * m[4, 4];
            */
            for (int w = 1; w <= 4; w++)
            {
                for (int h = 1; h <= 4; h++)
                {
                    for (int n = 1; n <= 4; n++)
                    {
                        newM[w, h] += this[w, n] * m[n, h];
                    }
                }
            }
            return newM;
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
