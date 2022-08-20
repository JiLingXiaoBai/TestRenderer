using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRenderer
{
    internal class Canvas
    {
        public const int canvas_height = 400;
        public const int canvas_width = 400;


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
        public static void DrawTriangle1(IntVector2 t0, IntVector2 t1, IntVector2 t2, ref Bitmap bitmap, Color color)
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
                    int x = j;
                    int y = t0.y + i;
                    if (x >= 0 && x < canvas_width && y >=0 && y < canvas_height)
                        bitmap.SetPixel(x, y, color);
                }
            }
        }

        public static Vector3 Barycentric(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
        {
            Vector3[] s = new Vector3[2];
            for(int i = 0; i < 2; i++)
            {
                s[i] = new Vector3();
                s[i][0] = C[i] - A[i];
                s[i][1] = B[i] - A[i];
                s[i][2] = A[i] - P[i];
            }

            Vector3 u = Vector3.CrossProduct(s[0], s[1]);
            if (Math.Abs(u[2]) > float.Epsilon)
            {
                return new Vector3(1f - (u.x + u.y) / u.z, u.y / u.z, u.x / u.z);
            }
            return new Vector3(-1, 1, 1);
        }

        public static void DrawTriangle2(Vector3[] vertexPos, float[] zbuffer, ref Bitmap bitmap, Color color)
        {
            Vector2 bboxmin = new Vector2(float.MaxValue,float.MaxValue);
            Vector2 bboxmax = new Vector2(float.MinValue,float.MinValue);
            Vector2 clamp = new Vector2(bitmap.Width - 1, bitmap.Height - 1);
            Vector3[] screen_pos = new Vector3[3];
            for(int i = 0; i < 3; i++)
            {
                //bboxmin.x = Math.Max(0f, Math.Min(bboxmin.x, vertexPos[i].x));
                //bboxmin.y = Math.Max(0f, Math.Min(bboxmin.y, vertexPos[i].y));
                //bboxmax.x = Math.Min(clamp.x, Math.Max(bboxmax.x, vertexPos[i].x));
                //bboxmax.y = Math.Min(clamp.y, Math.Max(bboxmax.y, vertexPos[i].y));
                screen_pos[i] = new Vector3();
                screen_pos[i].x = (vertexPos[i].x + 1) * canvas_width / 2;
                screen_pos[i].y = (vertexPos[i].y + 1) * canvas_height / 2;
                screen_pos[i].z = (vertexPos[i].z + 1) * (canvas_width + canvas_height) / 4;
                
                for (int j = 0; j < 2; j++)
                {
                    bboxmin[j] = Math.Max(0f, Math.Min(bboxmin[j], screen_pos[i][j]));
                    bboxmax[j] = Math.Min(clamp[j], Math.Max(bboxmax[j], screen_pos[i][j]));
                }
            }

            Vector3 p = new Vector3();
          
            for(p.x = (int)Math.Ceiling(bboxmin.x); p.x <= (int)Math.Floor(bboxmax.x); p.x++)
            {
                for(p.y = (int)Math.Ceiling(bboxmin.y); p.y <= (int)Math.Floor(bboxmax.y); p.y++)
                {
                    Vector3 bc_screen = Barycentric(screen_pos[0], screen_pos[1], screen_pos[2], p);
                    if (bc_screen.x < 0f || bc_screen.y < 0f || bc_screen.z < 0f) continue;
                    p.z = 0;
                    //p.z = vertexPos[0].z * bc_screen.x + vertexPos[1].z * bc_screen.y + vertexPos[2].z * bc_screen.z;
                    for (int i = 0; i < 3; i++) p.z += screen_pos[i][2] * bc_screen[i];
                    if (zbuffer[(int)(p.x + p.y * canvas_width)] < p.z)
                    {
                        zbuffer[(int)(p.x + p.y * canvas_width)] = p.z;
                        
                        bitmap.SetPixel(Convert.ToInt16(p.x), Convert.ToInt16(p.y), color);
                    }
                }
            }
        }
    }
}
