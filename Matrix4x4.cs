
using System.Runtime.CompilerServices;

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

        private Vector4 GetColumn(int index)
        {
            switch (index)
            {
                case 0: return new Vector4(pts[0, 0], pts[1, 0], pts[2, 0], pts[3, 0]);
                case 1: return new Vector4(pts[0, 1], pts[1, 1], pts[2, 1], pts[3, 1]);
                case 2: return new Vector4(pts[0, 2], pts[1, 2], pts[2, 2], pts[3, 2]);
                case 3: return new Vector4(pts[0, 3], pts[1, 3], pts[2, 3], pts[3, 3]);
                default:
                    throw new IndexOutOfRangeException("Invalid column index!");
            }
        }

        public static bool operator ==(Matrix4x4 lhs, Matrix4x4 rhs)
        {
            // Returns false in the presence of NaN values.
            return lhs.GetColumn(0) == rhs.GetColumn(0)
                && lhs.GetColumn(1) == rhs.GetColumn(1)
                && lhs.GetColumn(2) == rhs.GetColumn(2)
                && lhs.GetColumn(3) == rhs.GetColumn(3);
        }
        public static bool operator !=(Matrix4x4 lhs, Matrix4x4 rhs)
        {
            // Returns true in the presence of NaN values.
            return !(lhs == rhs);
        }

        private static bool IsOrtho(Matrix4x4 m)
        {
            return m.Mul(m.transposed) == identity;
        }

        public bool isOrtho
        {
            get
            {
                return IsOrtho(this);
            }
        }


        public Matrix4x4(float[,] mpts)
        {
            pts = mpts;
        }

        /// <summary>
        /// 求矩阵的逆矩阵
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public Matrix4x4 inverseMatrix
        {
            get{
                if (isOrtho)
                    return transposed;

                //计算矩阵行列式的值
                float dDeterminant = Determinant(pts, 4);
                if (Math.Abs(dDeterminant) < float.Epsilon)
                {
                    throw new Exception("矩阵不可逆");
                }

                //制作一个伴随矩阵大小的矩阵
                float[,] result = AdjointMatrix(pts);

                //矩阵的每项除以矩阵行列式的值，即为所求
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        result[i, j] = result[i, j] / dDeterminant;
                    }
                }

                return new Matrix4x4(result);
            }
            
        }

        /// <summary>
        /// 递归计算行列式的值
        /// </summary>
        /// <param name="matrix">矩阵</param>
        /// <returns></returns>
        private float Determinant(float[,] matrix, int n)
        {
            if (n == 1)
            {
                return matrix[0, 0];
            }

            //对第一行使用“加边法”递归计算行列式的值
            float dSum = 0f;
            int dSign = 1;
            float[,] matrixTemp = new float[4, 4];
            for (int i = 0; i < n; i++)
            {
                BlockCofactor(matrix, ref matrixTemp, 0, i, n);
         

                dSum += matrix[0, i] * dSign * Determinant(matrixTemp, n-1);
                dSign = -dSign;
            }

            return dSum;
        }

        private static void BlockCofactor(float[,] matrix, ref float[,] temp, int p, int q, int n)
        {
            int i = 0;
            int j = 0;

            for (int row = 0; row < n; row++)
            {
                for (int col = 0; col < n; col++)
                {
                    if (row != p && col != q)
                    {
                        temp[i, j++] = matrix[row, col];
                        if (j == (n - 1))
                        {
                            j = 0;
                            i++;
                        }
                    }
                }
            }
        }



        /// <summary>
        /// 计算方阵的伴随矩阵
        /// </summary>
        /// <param name="matrix">方阵</param>
        /// <returns></returns>
        private float[,] AdjointMatrix(float[,] matrix)
        {
            int N = matrix.GetLength(0);
            //制作一个伴随矩阵大小的矩阵
            float[,] result = new float[N, N];            
            float[,] temp = new float[N, N];
            //生成伴随矩阵
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    BlockCofactor(matrix, ref temp, i, j, N);
                    result[j, i] = ((i + j) % 2 == 0 ? 1 : -1) * Determinant(temp, N - 1);  
                }
            }

            return result;
        }


        /// 三维矩阵重正交化后扩展至四维矩阵
        public Matrix4x4 ReNormalized(Matrix4x4 matrix)
        {
            Vector3 a1 = new Vector3(matrix[1, 1], matrix[1, 2], matrix[1, 3]);
            Vector3 a2 = new Vector3(matrix[2, 1], matrix[2, 2], matrix[2, 3]);
            Vector3 a3 = new Vector3(matrix[3, 1], matrix[3, 2], matrix[3, 3]);

            Vector3 b1 = a1;
            Vector3 e1 = b1 / b1.magnitude;
            Vector3 b2 = a2 - Vector3.DotProduct(a2, e1) * e1;
            Vector3 e2 = b2 / b2.magnitude;
            Vector3 b3 = a3 - Vector3.DotProduct(a3, e2) * e2 - Vector3.DotProduct(a3, e1) * e1;
            Vector3 e3 = b3 / b3.magnitude;

            Matrix4x4 res = new Matrix4x4();
            res[1, 1] = e1.x; res[1, 2] = e1.y; res[1, 3] = e1.z;
            res[2, 1] = e2.x; res[2, 2] = e2.y; res[2, 3] = e2.z;
            res[3, 1] = e3.x; res[3, 2] = e3.y; res[3, 3] = e3.z;
            res[4, 4] = 1;
            return res;
        }

        public Matrix4x4 reNormalized
        {
            get { return ReNormalized(this); }
        }
    }
}
