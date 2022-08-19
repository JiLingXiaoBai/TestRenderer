using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRenderer
{
    internal class Canvas
    {
        private static void TempSwap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        private static void TempSwap(ref IntVector2 a, ref IntVector2 b)
        {
            IntVector2 temp = a;
            a = b;
            b = temp;
        }

        //Bresenham算法
        public static void DrawLine(int x0, int y0, int x1, int y1, ref Bitmap bitmap, Color color)
        {
            bool steep = false;
            if(Math.Abs(x0 - x1) < Math.Abs(y0 - y1))
            {
                TempSwap(ref x0, ref y0);
                TempSwap(ref x1, ref y1);
                steep = true;
            }

            if(x0 > x1)
            {
                TempSwap(ref x0, ref x1);
                TempSwap(ref y0, ref y1);
            }

            int dx = x1 - x0;
            int dy = y1 - y0;

            int derror = Math.Abs(dy) * 2;
            int error = 0;
            int y = y0;

            for(int x = x0; x <= x1; x++)
            {
                if (steep)
                {
                    bitmap.SetPixel(y, x, color);
                }
                else
                {
                    bitmap.SetPixel(x, y, color);
                }

                error += derror;

                if (error > dx)
                {
                    y += (y1 > y0 ? 1 : -1);
                    error -= dx * 2;
                }
            }
        }

        //二维三角形，从下往上水平画线填充三角形面
        public static void DrawTriangle(IntVector2 t0, IntVector2 t1, IntVector2 t2, ref Bitmap bitmap, Color color)
        {
            //三角形的三个点y值相同，面积为0
            if (t0.y == t1.y && t1.y == t2.y) return;
            //根据y值大小对坐标进行排序
            if (t0.y > t1.y) TempSwap(ref t0, ref t1);
            if (t0.y > t2.y) TempSwap(ref t0, ref t2);
            if (t1.y > t2.y) TempSwap(ref t1, ref t2);

            int total_height = t2.y - t0.y;
            //以高度差作为循环控制变量，此时不需要考虑斜率，因为着色完后每行都会被填充
            for(int i = 0; i < total_height; i++)
            {
                bool second_half = i > t1.y - t0.y || t1.y == t0.y;

                int segment_height = second_half ? t2.y - t1.y : t1.y - t0.y;
                float alpha = (float)i / total_height;
                float beta = (float)(i - (second_half ? t1.y - t0.y : 0)) / segment_height;
                //计算A，B两点的坐标
                IntVector2 A = t0 + (t2 - t0) * alpha;
                IntVector2 B = second_half ? t1 + (t2 - t1) * beta : t0 + (t1 - t0) * beta;

                if(A.x > B.x) TempSwap(ref A, ref B);
                for(int j = A.x; j <= B.x; j++)
                {
                    bitmap.SetPixel(j, t0.y+i, color);
                }
            }
        }
    }
}
