
namespace TestRenderer
{
    internal class Canvas
    {
        public const int canvas_height = 400;
        public const int canvas_width = 400;

        static float intensity = 0;
        static Color color = Color.White;

        static Vector2[]? texture_uv;
        static Vector3[]? vertex_normal;
        static Vector3[]? world_pos;
        static Vector3[]? ndc_pos;
        static bool useDiffuseTex;
        static Bitmap? diffuseTex;
        static float[] zbuffer;
        static Vector3 light_dir;

        private static void TempSwap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }


        //Bresenham算法
        public static void DrawLine(Vector3 v0, Vector3 v1, ref Bitmap bitmap, Color color)
        {

            int x0 = Convert.ToInt32(v0.x * canvas_width / 2 + canvas_width / 2);
            int y0 = Convert.ToInt32(v0.y * canvas_height / 2 + canvas_height / 2);
            int x1 = Convert.ToInt32(v1.x * canvas_width / 2 + canvas_width / 2);
            int y1 = Convert.ToInt32(v1.y * canvas_height / 2 + canvas_height / 2);

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
                if (x > 0 && x < canvas_width && y > 0 && y < canvas_height)
                {
                    if (steep)
                    {
                        bitmap.SetPixel(y, canvas_height - x, color);
                    }
                    else
                    {
                        bitmap.SetPixel(x, canvas_height - y, color);
                    }
                }

                error += derror;

                if (error > dx)
                {
                    y += (y1 > y0 ? 1 : -1);
                    error -= dx * 2;
                }
            }
        }

        public static Vector3 Barycentric(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
        {
            Vector3[] s = new Vector3[2];
            for (int i = 0; i < 2; i++)
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

        //FlatLit
        public static void DrawTriangle1F(ref Bitmap resBitmap)
        {
            int x, y;
            x = Convert.ToInt32(ndc_pos[0].x * canvas_width / 2 + canvas_width / 2);
            y = Convert.ToInt32(ndc_pos[0].y * canvas_height / 2 + canvas_height / 2);
            IntVector2 t0 = new IntVector2(x, y);
            x = Convert.ToInt32(ndc_pos[1].x * canvas_width / 2 + canvas_width / 2);
            y = Convert.ToInt32(ndc_pos[1].y * canvas_height / 2 + canvas_height / 2);
            IntVector2 t1 = new IntVector2(x, y);
            x = Convert.ToInt32(ndc_pos[2].x * canvas_width / 2 + canvas_width / 2);
            y = Convert.ToInt32(ndc_pos[2].y * canvas_height / 2 + canvas_height / 2);
            IntVector2 t2 = new IntVector2(x, y);

            float[] z = new float[3];

            for (int i = 0; i < 3; i++)
            {
                z[i] = ndc_pos[i].z;
            }

            //三角形的三个点y值相同，面积为0
            if (t0.y == t1.y && t1.y == t2.y) return;
            //根据y值大小对坐标进行排序
            if (t0.y > t1.y)
            {
                TempSwap(ref t0, ref t1);
                TempSwap(ref texture_uv[0], ref texture_uv[1]);
                TempSwap(ref z[0], ref z[1]);
            }
            if (t0.y > t2.y)
            {
                TempSwap(ref t0, ref t2);
                TempSwap(ref texture_uv[0], ref texture_uv[2]);
                TempSwap(ref z[0], ref z[2]);
            }
            if (t1.y > t2.y)
            {
                TempSwap(ref t1, ref t2);
                TempSwap(ref texture_uv[1], ref texture_uv[2]);
                TempSwap(ref z[1], ref z[2]);
            }


            int total_height = t2.y - t0.y;
            //以高度差作为循环控制变量，此时不需要考虑斜率，因为着色完后每行都会被填充
            for (int i = 0; i < total_height; i++)
            {
                bool second_half = i > t1.y - t0.y || t1.y == t0.y;

                int segment_height = second_half ? t2.y - t1.y : t1.y - t0.y;
                float alpha = (float)i / total_height;
                float beta = (float)(i - (second_half ? t1.y - t0.y : 0)) / segment_height;
                //计算A，B两点的坐标
                IntVector2 A = t0 + (t2 - t0) * alpha;
                IntVector2 B = second_half ? t1 + (t2 - t1) * beta : t0 + (t1 - t0) * beta;

                Vector2 uvA = texture_uv[0] + (texture_uv[2] - texture_uv[0]) * alpha;
                Vector2 uvB = second_half ? texture_uv[1] + (texture_uv[2] - texture_uv[1]) * beta : texture_uv[0] + (texture_uv[1] - texture_uv[0]) * beta;

                float zA = z[0] + (z[2] - z[0]) * alpha;
                float zB = second_half ? z[1] + (z[2] - z[1]) * beta : z[0] + (z[1] - z[0]) * beta;

                if (A.x > B.x)
                {
                    TempSwap(ref A, ref B);
                    TempSwap(ref uvA, ref uvB);
                    TempSwap(ref zA, ref zB);
                }

                for (int j = A.x; j <= B.x; j++)
                {
                    float phi = B.x == A.x ? 1f : (float)(j - A.x) / (float)(B.x - A.x);
                    IntVector2 P = A + (B - A) * phi;
                    float zP = zA + (zB - zA) * phi;

                    if (P.x >= 0 && P.x < canvas_width && P.y > 0 && P.y < canvas_height)
                    {
                        if (zbuffer[P.x + P.y * canvas_width] < zP)
                        {
                            zbuffer[P.x + P.y * canvas_width] = zP;
                            if (useDiffuseTex && diffuseTex != null)
                            {
                                Vector2 uvP = uvA + (uvB - uvA) * phi;
                                Color texColor = diffuseTex.GetPixel((int)Math.Floor(uvP.x * diffuseTex.Width), (int)Math.Floor((1 - uvP.y) * diffuseTex.Height));
                                Color resColor = Color.FromArgb(color.R * texColor.R / 255, color.G * texColor.G / 255, color.B * texColor.B / 255);
                                resBitmap.SetPixel((int)P.x, (int)(canvas_height - P.y), resColor);
                            }
                            else
                            {
                                resBitmap.SetPixel((int)P.x, (int)(canvas_height - P.y), color);
                            }
                        }
                    }
                }
            }
        }
        //FlatLit
        public static void DrawTriangle2F(ref Bitmap bitmap)
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
                screen_pos[i].x = (ndc_pos[i].x + 1) * canvas_width / 2;
                screen_pos[i].y = (ndc_pos[i].y + 1) * canvas_height / 2;
                screen_pos[i].z = ndc_pos[i].z;
                
                for (int j = 0; j < 2; j++)
                {
                    bboxmin[j] = Math.Max(0f, Math.Min(bboxmin[j], screen_pos[i][j]));
                    bboxmax[j] = Math.Min(clamp[j], Math.Max(bboxmax[j], screen_pos[i][j]));
                }
            }
   
            Vector3 p = new Vector3();
            Vector2 uvP = new Vector2();
            for (p.x = (int)Math.Ceiling(bboxmin.x); p.x <= (int)Math.Floor(bboxmax.x); p.x++)
            {
                for(p.y = (int)Math.Ceiling(bboxmin.y); p.y <= (int)Math.Floor(bboxmax.y); p.y++)
                {
                    Vector3 bc_screen = Barycentric(screen_pos[0], screen_pos[1], screen_pos[2], p);

                    if (bc_screen.x < 0f || bc_screen.y < 0f || bc_screen.z < 0f) continue;
                    
                    p.z = screen_pos[0].z * bc_screen.x + screen_pos[1].z * bc_screen.y + screen_pos[2].z * bc_screen.z;
                            
                    if (zbuffer[(int)(p.x + p.y * canvas_width)] < p.z)
                    {
                        zbuffer[(int)(p.x + p.y * canvas_width)] = p.z;

                        uvP = texture_uv[0] * bc_screen.x + texture_uv[1] * bc_screen.y + texture_uv[2] * bc_screen.z;
                        
                        if (useDiffuseTex && diffuseTex != null)
                        {
                            Color texColor = diffuseTex.GetPixel((int)(uvP.x * diffuseTex.Width), (int)((1 - uvP.y) * diffuseTex.Height));
                            Color resColor = Color.FromArgb(color.R * texColor.R / 255, color.G * texColor.G / 255, color.B * texColor.B / 255);
                            if (p.y == 0) return;
                            bitmap.SetPixel(Convert.ToInt16(p.x), Convert.ToInt16(canvas_height - p.y), resColor);
                        }
                        else
                        {
                            if (p.y == 0) return;
                            bitmap.SetPixel(Convert.ToInt16(p.x), Convert.ToInt16(canvas_height - p.y), color);
                        }
                        
                    }
                }
            }
        }
        //VertexLit
        public static void DrawTriangle1V(ref Bitmap resBitmap)
        {
            int x, y;
            x = Convert.ToInt32(ndc_pos[0].x * canvas_width / 2 + canvas_width / 2);
            y = Convert.ToInt32(ndc_pos[0].y * canvas_height / 2 + canvas_height / 2);
            IntVector2 t0 = new IntVector2(x, y);
            x = Convert.ToInt32(ndc_pos[1].x * canvas_width / 2 + canvas_width / 2);
            y = Convert.ToInt32(ndc_pos[1].y * canvas_height / 2 + canvas_height / 2);
            IntVector2 t1 = new IntVector2(x, y);
            x = Convert.ToInt32(ndc_pos[2].x * canvas_width / 2 + canvas_width / 2);
            y = Convert.ToInt32(ndc_pos[2].y * canvas_height / 2 + canvas_height / 2);
            IntVector2 t2 = new IntVector2(x, y);

            float[] z = new float[3];
            float[] ity = new float[3];

            for(int i = 0; i < 3; i++)
            {
                z[i] = ndc_pos[i].z;
                ity[i] = Vector3.DotProduct(vertex_normal[i].normalized, light_dir);
            }

            //三角形的三个点y值相同，面积为0
            if (t0.y == t1.y && t1.y == t2.y) return;
            //根据y值大小对坐标进行排序
            if (t0.y > t1.y)
            {
                TempSwap(ref t0, ref t1);
                TempSwap(ref texture_uv[0], ref texture_uv[1]);
                TempSwap(ref z[0], ref z[1]);
                TempSwap(ref ity[0], ref ity[1]);
            }
            if (t0.y > t2.y)
            {
                TempSwap(ref t0, ref t2);
                TempSwap(ref texture_uv[0], ref texture_uv[2]);
                TempSwap(ref z[0], ref z[2]);
                TempSwap(ref ity[0], ref ity[2]);
            }
            if (t1.y > t2.y)
            {
                TempSwap(ref t1, ref t2);
                TempSwap(ref texture_uv[1], ref texture_uv[2]);
                TempSwap(ref z[1], ref z[2]);
                TempSwap(ref ity[1], ref ity[2]);
            }


            int total_height = t2.y - t0.y;
            //以高度差作为循环控制变量，此时不需要考虑斜率，因为着色完后每行都会被填充
            for (int i = 0; i < total_height; i++)
            {
                bool second_half = i > t1.y - t0.y || t1.y == t0.y;

                int segment_height = second_half ? t2.y - t1.y : t1.y - t0.y;
                float alpha = (float)i / total_height;
                float beta = (float)(i - (second_half ? t1.y - t0.y : 0)) / segment_height;
                //计算A，B两点的坐标
                IntVector2 A = t0 + (t2 - t0) * alpha;
                IntVector2 B = second_half ? t1 + (t2 - t1) * beta : t0 + (t1 - t0) * beta;

                Vector2 uvA = texture_uv[0] + (texture_uv[2] - texture_uv[0]) * alpha;
                Vector2 uvB = second_half ? texture_uv[1] + (texture_uv[2] - texture_uv[1]) * beta : texture_uv[0] + (texture_uv[1] - texture_uv[0]) * beta;

                float zA = z[0] + (z[2] - z[0]) * alpha;
                float zB = second_half ? z[1] + (z[2] - z[1]) * beta : z[0] + (z[1] - z[0]) * beta;

                float ityA = ity[0] + (ity[2] - ity[0]) * alpha;
                float ityB = second_half ? ity[1] + (ity[2] - ity[1]) * beta : ity[0] + (ity[1] - ity[0]) * beta;

                if (A.x > B.x)
                {
                    TempSwap(ref A, ref B);
                    TempSwap(ref uvA, ref uvB);
                    TempSwap(ref zA, ref zB);
                    TempSwap(ref ityA, ref ityB);
                }

                for (int j = A.x; j <= B.x; j++)
                {
                    float phi = B.x == A.x ? 1f : (float)(j - A.x) / (float)(B.x - A.x);
                    IntVector2 P = A + (B - A) * phi;
                    float zP = zA + (zB - zA) * phi;
                    float ityP = ityA + (ityB - ityA) * phi;

                    if (P.x >= 0 && P.x < canvas_width && P.y > 0 && P.y < canvas_height)
                    {
                        int gray = Convert.ToInt32(Math.Abs(ityP) * 255);
                        color = Color.FromArgb(gray, gray, gray);

                        if (zbuffer[P.x + P.y * canvas_width] < zP)
                        {
                            zbuffer[P.x + P.y * canvas_width] = zP;
                            if (useDiffuseTex && diffuseTex != null)
                            {
                                Vector2 uvP = uvA + (uvB - uvA) * phi;
                                Color texColor = diffuseTex.GetPixel((int)Math.Floor(uvP.x * diffuseTex.Width), (int)Math.Floor((1 - uvP.y) * diffuseTex.Height));
                                Color resColor = Color.FromArgb(color.R * texColor.R / 255, color.G * texColor.G / 255, color.B * texColor.B / 255);
                                resBitmap.SetPixel((int)P.x, (int)(canvas_height - P.y), resColor);
                            }
                            else
                            {
                                resBitmap.SetPixel((int)P.x, (int)(canvas_height - P.y), color);
                            }
                        }
                    }
                }
            }
        }
        //VertexLit
        public static void DrawTriangle2V(ref Bitmap bitmap)
        {
            Vector2 bboxmin = new Vector2(float.MaxValue, float.MaxValue);
            Vector2 bboxmax = new Vector2(float.MinValue, float.MinValue);
            Vector2 clamp = new Vector2(bitmap.Width - 1, bitmap.Height - 1);
            Vector3[] screen_pos = new Vector3[3];
            float[] ity = new float[3];
            
            for (int i = 0; i < 3; i++)
            {
                //bboxmin.x = Math.Max(0f, Math.Min(bboxmin.x, vertexPos[i].x));
                //bboxmin.y = Math.Max(0f, Math.Min(bboxmin.y, vertexPos[i].y));
                //bboxmax.x = Math.Min(clamp.x, Math.Max(bboxmax.x, vertexPos[i].x));
                //bboxmax.y = Math.Min(clamp.y, Math.Max(bboxmax.y, vertexPos[i].y));
                screen_pos[i] = new Vector3();
                screen_pos[i].x = (ndc_pos[i].x + 1) * canvas_width / 2;
                screen_pos[i].y = (ndc_pos[i].y + 1) * canvas_height / 2;
                screen_pos[i].z = ndc_pos[i].z;
                ity[i] = Vector3.DotProduct(vertex_normal[i].normalized, light_dir);
                for (int j = 0; j < 2; j++)
                {
                    bboxmin[j] = Math.Max(0f, Math.Min(bboxmin[j], screen_pos[i][j]));
                    bboxmax[j] = Math.Min(clamp[j], Math.Max(bboxmax[j], screen_pos[i][j]));
                }
            }

            Vector3 p = new Vector3();
            Vector2 uvP = new Vector2();
            for (p.x = (int)Math.Ceiling(bboxmin.x); p.x <= (int)Math.Floor(bboxmax.x); p.x++)
            {
                for (p.y = (int)Math.Ceiling(bboxmin.y); p.y <= (int)Math.Floor(bboxmax.y); p.y++)
                {
                    Vector3 bc_screen = Barycentric(screen_pos[0], screen_pos[1], screen_pos[2], p);
                    if (bc_screen.x < 0f || bc_screen.y < 0f || bc_screen.z < 0f) continue;
                    
                    p.z = screen_pos[0].z * bc_screen.x + screen_pos[1].z * bc_screen.y + screen_pos[2].z * bc_screen.z;
                    
                    if (zbuffer[(int)(p.x + p.y * canvas_width)] < p.z)
                    {
                        zbuffer[(int)(p.x + p.y * canvas_width)] = p.z;

                        uvP = texture_uv[0] * bc_screen.x + texture_uv[1] * bc_screen.y + texture_uv[2] * bc_screen.z;
          
                        intensity = ity[0] * bc_screen.x + ity[1] * bc_screen.y + ity[2] * bc_screen.z;
                        //if (intensity < 0f) return;
                        int gray = Convert.ToInt32(Math.Abs(intensity) * 255);
                        color = Color.FromArgb(gray, gray, gray);

                        if (useDiffuseTex && diffuseTex != null)
                        {
                            Color texColor = diffuseTex.GetPixel((int)(uvP.x * diffuseTex.Width), (int)((1 - uvP.y) * diffuseTex.Height));
                            Color resColor = Color.FromArgb(color.R * texColor.R / 255, color.G * texColor.G / 255, color.B * texColor.B / 255);
                            if (p.y == 0) return;
                            bitmap.SetPixel(Convert.ToInt16(p.x), Convert.ToInt16(canvas_height - p.y), resColor);
                        }
                        else
                        {
                            if (p.y == 0) return;
                            bitmap.SetPixel(Convert.ToInt16(p.x), Convert.ToInt16(canvas_height - p.y), color);
                        }

                    }
                }
            }
        }
        public static void DrawTrangle(bool useBaryCentric, string lightingType, ref Bitmap bitmap)
        {
            Vector3 n = Vector3.CrossProduct(world_pos[2] - world_pos[0], world_pos[1] - world_pos[0]);
            intensity = Vector3.DotProduct(n.normalized, light_dir);
            //背面剔除
            if (intensity < 0) return;
            switch (lightingType)
            {
                case "IsFlatLit":
                    
                    int gray = Convert.ToInt32(intensity * 255);
                    color = Color.FromArgb(gray, gray, gray);
                    if (!useBaryCentric)
                    {
                        DrawTriangle1F(ref bitmap);
                    }
                    else
                    {
                        DrawTriangle2F(ref bitmap);
                    }
                    break;
                case "IsVertexLit":
                    if (!useBaryCentric)
                    {
                        DrawTriangle1V(ref bitmap);
                    }
                    else
                    {
                        DrawTriangle2V(ref bitmap);
                    }
                    break;
                case "IsPixelLit":
                    break;
                default:
                    return;
            }
        }

        public static void SetData(Vector3[] world_pos, Vector3[] ndc_pos, Vector3[] vertex_normal, Vector2[] texture_uv, bool useDiffuseTex, Bitmap? diffuseTex, float[] zbuffer, Vector3 light_dir)
        {
            Canvas.world_pos = world_pos;
            Canvas.ndc_pos = ndc_pos;
            Canvas.vertex_normal = vertex_normal;
            Canvas.texture_uv = texture_uv;
            Canvas.diffuseTex = diffuseTex;
            Canvas.zbuffer = zbuffer;
            Canvas.light_dir = light_dir;
            Canvas.useDiffuseTex = useDiffuseTex;
        }
    }
}
