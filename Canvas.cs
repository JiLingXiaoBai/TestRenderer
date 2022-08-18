using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRenderer
{
    internal class Canvas
    {
        private void TempSwap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        //Bresenham算法
        public void DrawLine(int x0, int y0, int x1, int y1, Bitmap bitmap, Color color)
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
    }
}
