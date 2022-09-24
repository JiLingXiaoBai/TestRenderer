
namespace TestRenderer
{
    internal class Canvas
    {
        public const int canvas_height = 400;
        public const int canvas_width = 400;

        float gloss = 80f;
        Color color = Color.White;
        Vertex[]? vertices;
        bool useDiffuseTex;
        Bitmap? diffuseTex;
        Bitmap? normalTex;
        float[]? zbuffer;
        Vector3? light_dir;
        Vector3? camPos;

        Matrix4x4? reNormalizedTBN;

        public Canvas()
        {

        }

        private static void TempSwap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }


        //Bresenham算法
        public void DrawLine(Vector3 v0, Vector3 v1, ref Bitmap bitmap, Color color)
        {

            int x0 = Convert.ToInt32(v0.x * canvas_width / 2 + canvas_width / 2);
            int y0 = Convert.ToInt32(v0.y * canvas_height / 2 + canvas_height / 2);
            int x1 = Convert.ToInt32(v1.x * canvas_width / 2 + canvas_width / 2);
            int y1 = Convert.ToInt32(v1.y * canvas_height / 2 + canvas_height / 2);

            bool steep = false;
            if (Math.Abs(x0 - x1) < Math.Abs(y0 - y1))
            {
                TempSwap(ref x0, ref y0);
                TempSwap(ref x1, ref y1);
                steep = true;
            }

            if (x0 > x1)
            {
                TempSwap(ref x0, ref x1);
                TempSwap(ref y0, ref y1);
            }

            int dx = x1 - x0;
            int dy = y1 - y0;

            int derror = Math.Abs(dy) * 2;
            int error = 0;
            int y = y0;

            for (int x = x0; x <= x1; x++)
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

        public Vector3 Barycentric(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
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
        public void DrawTriangle1F(ref Bitmap resBitmap)
        {
            int x, y;
            x = Convert.ToInt32(vertices[0].ndc_pos.x * canvas_width / 2 + canvas_width / 2);
            y = Convert.ToInt32(vertices[0].ndc_pos.y * canvas_height / 2 + canvas_height / 2);
            IntVector2 t0 = new IntVector2(x, y);
            x = Convert.ToInt32(vertices[1].ndc_pos.x * canvas_width / 2 + canvas_width / 2);
            y = Convert.ToInt32(vertices[1].ndc_pos.y * canvas_height / 2 + canvas_height / 2);
            IntVector2 t1 = new IntVector2(x, y);
            x = Convert.ToInt32(vertices[2].ndc_pos.x * canvas_width / 2 + canvas_width / 2);
            y = Convert.ToInt32(vertices[2].ndc_pos.y * canvas_height / 2 + canvas_height / 2);
            IntVector2 t2 = new IntVector2(x, y);

            float[] z = new float[3];

            for (int i = 0; i < 3; i++)
            {
                z[i] = vertices[i].ndc_pos.z;
            }

            //三角形的三个点y值相同，面积为0
            if (t0.y == t1.y && t1.y == t2.y) return;
            //根据y值大小对坐标进行排序
            if (t0.y > t1.y)
            {
                TempSwap(ref t0, ref t1);
                TempSwap(ref vertices[0], ref vertices[1]);
                TempSwap(ref z[0], ref z[1]);
            }
            if (t0.y > t2.y)
            {
                TempSwap(ref t0, ref t2);
                TempSwap(ref vertices[0], ref vertices[2]);
                TempSwap(ref z[0], ref z[2]);
            }
            if (t1.y > t2.y)
            {
                TempSwap(ref t1, ref t2);
                TempSwap(ref vertices[1], ref vertices[2]);
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

                Vector2 uvA = vertices[0].texture_uv + (vertices[2].texture_uv - vertices[0].texture_uv) * alpha;
                Vector2 uvB = second_half ? vertices[1].texture_uv + (vertices[2].texture_uv - vertices[1].texture_uv) * beta : vertices[0].texture_uv + (vertices[1].texture_uv - vertices[0].texture_uv) * beta;

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
                                Color resColor = ColorProduct(color, texColor);
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
        public void DrawTriangle2F(ref Bitmap bitmap)
        {
            Vector2 bboxmin = new Vector2(float.MaxValue, float.MaxValue);
            Vector2 bboxmax = new Vector2(float.MinValue, float.MinValue);
            Vector2 clamp = new Vector2(bitmap.Width - 1, bitmap.Height - 1);
            Vector3[] screen_pos = new Vector3[3];
            for (int i = 0; i < 3; i++)
            {
                //bboxmin.x = Math.Max(0f, Math.Min(bboxmin.x, vertexPos[i].x));
                //bboxmin.y = Math.Max(0f, Math.Min(bboxmin.y, vertexPos[i].y));
                //bboxmax.x = Math.Min(clamp.x, Math.Max(bboxmax.x, vertexPos[i].x));
                //bboxmax.y = Math.Min(clamp.y, Math.Max(bboxmax.y, vertexPos[i].y));
                screen_pos[i] = new Vector3();
                screen_pos[i].x = (vertices[i].ndc_pos.x + 1) * canvas_width / 2;
                screen_pos[i].y = (vertices[i].ndc_pos.y + 1) * canvas_height / 2;
                screen_pos[i].z = vertices[i].ndc_pos.z;

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

                        uvP = vertices[0].texture_uv * bc_screen.x + vertices[1].texture_uv * bc_screen.y + vertices[2].texture_uv * bc_screen.z;

                        if (useDiffuseTex && diffuseTex != null)
                        {
                            Color texColor = diffuseTex.GetPixel((int)(uvP.x * diffuseTex.Width), (int)((1 - uvP.y) * diffuseTex.Height));
                            Color resColor = ColorProduct(color, texColor);
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
        public void DrawTriangle1V(ref Bitmap resBitmap)
        {
            int x, y;
            x = Convert.ToInt32(vertices[0].ndc_pos.x * canvas_width / 2 + canvas_width / 2);
            y = Convert.ToInt32(vertices[0].ndc_pos.y * canvas_height / 2 + canvas_height / 2);
            IntVector2 t0 = new IntVector2(x, y);
            x = Convert.ToInt32(vertices[1].ndc_pos.x * canvas_width / 2 + canvas_width / 2);
            y = Convert.ToInt32(vertices[1].ndc_pos.y * canvas_height / 2 + canvas_height / 2);
            IntVector2 t1 = new IntVector2(x, y);
            x = Convert.ToInt32(vertices[2].ndc_pos.x * canvas_width / 2 + canvas_width / 2);
            y = Convert.ToInt32(vertices[2].ndc_pos.y * canvas_height / 2 + canvas_height / 2);
            IntVector2 t2 = new IntVector2(x, y);

            float[] z = new float[3];
            float[] ity_diffuse = new float[3];
            float[] ity_specular = new float[3];

            for (int i = 0; i < 3; i++)
            {
                z[i] = vertices[i].ndc_pos.z;
                ity_diffuse[i] = Vector3.DotProduct(vertices[i].vertex_normal.normalized, light_dir);

                Vector3 view_dir = (vertices[i].world_pos - camPos).normalized;
                Vector3 h = (light_dir + view_dir).normalized;
                float dot_nh = Math.Max(0, Vector3.DotProduct(vertices[i].vertex_normal.normalized, h));
                ity_specular[i] = (float)Math.Pow(dot_nh, gloss);
            }

            //三角形的三个点y值相同，面积为0
            if (t0.y == t1.y && t1.y == t2.y) return;
            //根据y值大小对坐标进行排序
            if (t0.y > t1.y)
            {
                TempSwap(ref t0, ref t1);
                TempSwap(ref vertices[0], ref vertices[1]);
                TempSwap(ref z[0], ref z[1]);
                TempSwap(ref ity_diffuse[0], ref ity_diffuse[1]);
                TempSwap(ref ity_specular[0], ref ity_specular[1]);
            }
            if (t0.y > t2.y)
            {
                TempSwap(ref t0, ref t2);
                TempSwap(ref vertices[0], ref vertices[2]);
                TempSwap(ref z[0], ref z[2]);
                TempSwap(ref ity_diffuse[0], ref ity_diffuse[2]);
                TempSwap(ref ity_specular[0], ref ity_specular[2]);
            }
            if (t1.y > t2.y)
            {
                TempSwap(ref t1, ref t2);
                TempSwap(ref vertices[1], ref vertices[2]);
                TempSwap(ref z[1], ref z[2]);
                TempSwap(ref ity_diffuse[1], ref ity_diffuse[2]);
                TempSwap(ref ity_specular[1], ref ity_specular[2]);
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

                Vector2 uvA = vertices[0].texture_uv + (vertices[2].texture_uv - vertices[0].texture_uv) * alpha;
                Vector2 uvB = second_half ? vertices[1].texture_uv + (vertices[2].texture_uv - vertices[1].texture_uv) * beta : vertices[0].texture_uv + (vertices[1].texture_uv - vertices[0].texture_uv) * beta;

                float zA = z[0] + (z[2] - z[0]) * alpha;
                float zB = second_half ? z[1] + (z[2] - z[1]) * beta : z[0] + (z[1] - z[0]) * beta;

                float ity_diffuseA = ity_diffuse[0] + (ity_diffuse[2] - ity_diffuse[0]) * alpha;
                float ity_diffuseB = second_half ? ity_diffuse[1] + (ity_diffuse[2] - ity_diffuse[1]) * beta : ity_diffuse[0] + (ity_diffuse[1] - ity_diffuse[0]) * beta;

                float ity_specularA = ity_specular[0] + (ity_specular[2] - ity_specular[0]) * alpha;
                float ity_specularB = second_half ? ity_specular[1] + (ity_specular[2] - ity_specular[1]) * beta : ity_specular[0] + (ity_specular[1] - ity_specular[0]) * beta;

                if (A.x > B.x)
                {
                    TempSwap(ref A, ref B);
                    TempSwap(ref uvA, ref uvB);
                    TempSwap(ref zA, ref zB);
                    TempSwap(ref ity_diffuseA, ref ity_diffuseB);
                    TempSwap(ref ity_specularA, ref ity_specularB);
                }

                for (int j = A.x; j <= B.x; j++)
                {
                    float phi = B.x == A.x ? 1f : (float)(j - A.x) / (float)(B.x - A.x);
                    IntVector2 P = A + (B - A) * phi;
                    float zP = zA + (zB - zA) * phi;
                    float ity_diffuseP = ity_diffuseA + (ity_diffuseB - ity_diffuseA) * phi;
                    float ity_specularP = ity_specularA + (ity_specularB - ity_specularA) * phi;

                    if (P.x >= 0 && P.x < canvas_width && P.y > 0 && P.y < canvas_height)
                    {
                        float intensity = (ity_diffuseP + ity_specularP) * 0.5f + 0.5f;
                        int res = Math.Clamp(Convert.ToInt32(intensity * 255), 0, 255);
                        color = Color.FromArgb(res, res, res);

                        if (zbuffer[P.x + P.y * canvas_width] < zP)
                        {
                            zbuffer[P.x + P.y * canvas_width] = zP;
                            if (useDiffuseTex && diffuseTex != null)
                            {
                                Vector2 uvP = uvA + (uvB - uvA) * phi;
                                Color texColor = diffuseTex.GetPixel((int)Math.Floor(uvP.x * diffuseTex.Width), (int)Math.Floor((1 - uvP.y) * diffuseTex.Height));
                                Color resColor = ColorProduct(color, texColor);
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
        public void DrawTriangle2V(ref Bitmap bitmap)
        {
            Vector2 bboxmin = new Vector2(float.MaxValue, float.MaxValue);
            Vector2 bboxmax = new Vector2(float.MinValue, float.MinValue);
            Vector2 clamp = new Vector2(bitmap.Width - 1, bitmap.Height - 1);
            Vector3[] screen_pos = new Vector3[3];
            float[] ity_diffuse = new float[3];
            float[] ity_specular = new float[3];
            for (int i = 0; i < 3; i++)
            {
                //bboxmin.x = Math.Max(0f, Math.Min(bboxmin.x, vertexPos[i].x));
                //bboxmin.y = Math.Max(0f, Math.Min(bboxmin.y, vertexPos[i].y));
                //bboxmax.x = Math.Min(clamp.x, Math.Max(bboxmax.x, vertexPos[i].x));
                //bboxmax.y = Math.Min(clamp.y, Math.Max(bboxmax.y, vertexPos[i].y));
                screen_pos[i] = new Vector3();
                screen_pos[i].x = (vertices[i].ndc_pos.x + 1) * canvas_width / 2;
                screen_pos[i].y = (vertices[i].ndc_pos.y + 1) * canvas_height / 2;
                screen_pos[i].z = vertices[i].ndc_pos.z;
                for (int j = 0; j < 2; j++)
                {
                    bboxmin[j] = Math.Max(0f, Math.Min(bboxmin[j], screen_pos[i][j]));
                    bboxmax[j] = Math.Min(clamp[j], Math.Max(bboxmax[j], screen_pos[i][j]));
                }

                ity_diffuse[i] = Vector3.DotProduct(vertices[i].vertex_normal.normalized, light_dir);
                Vector3 view_dir = (vertices[i].world_pos - camPos).normalized;
                Vector3 h = (light_dir + view_dir).normalized;
                float dot_nh = Math.Max(0, Vector3.DotProduct(vertices[i].vertex_normal.normalized, h));
                ity_specular[i] = (float)Math.Pow(dot_nh, gloss);

            }

            Vector3 p = new Vector3();
            Vector2 uvP;
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

                        uvP = vertices[0].texture_uv * bc_screen.x + vertices[1].texture_uv * bc_screen.y + vertices[2].texture_uv * bc_screen.z;

                        float ity_diffuseP = ity_diffuse[0] * bc_screen.x + ity_diffuse[1] * bc_screen.y + ity_diffuse[2] * bc_screen.z;
                        float ity_specularP = ity_specular[0] * bc_screen.x + ity_specular[1] * bc_screen.y + ity_specular[2] * bc_screen.z;

                        float intensity = (ity_diffuseP + ity_specularP) * 0.5f + 0.5f;
                        int res = Math.Clamp(Convert.ToInt32(intensity * 255), 0, 255);
                        color = Color.FromArgb(res, res, res);

                        if (useDiffuseTex && diffuseTex != null)
                        {
                            Color texColor = diffuseTex.GetPixel((int)(uvP.x * diffuseTex.Width), (int)((1 - uvP.y) * diffuseTex.Height));
                            Color resColor = ColorProduct(color, texColor);
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

        public void DrawTriangle1P(ref Bitmap resBitmap)
        {

            int x, y;
            x = Convert.ToInt32(vertices[0].ndc_pos.x * canvas_width / 2 + canvas_width / 2);
            y = Convert.ToInt32(vertices[0].ndc_pos.y * canvas_height / 2 + canvas_height / 2);
            IntVector2 t0 = new IntVector2(x, y);
            x = Convert.ToInt32(vertices[1].ndc_pos.x * canvas_width / 2 + canvas_width / 2);
            y = Convert.ToInt32(vertices[1].ndc_pos.y * canvas_height / 2 + canvas_height / 2);
            IntVector2 t1 = new IntVector2(x, y);
            x = Convert.ToInt32(vertices[2].ndc_pos.x * canvas_width / 2 + canvas_width / 2);
            y = Convert.ToInt32(vertices[2].ndc_pos.y * canvas_height / 2 + canvas_height / 2);
            IntVector2 t2 = new IntVector2(x, y);

            float[] z = new float[3];
            Vector3[] h = new Vector3[3];
            for (int i = 0; i < 3; i++)
            {
                z[i] = vertices[i].ndc_pos.z;
                Vector3 view_dir = (vertices[i].world_pos - camPos).normalized;
                h[i] = (light_dir + view_dir).normalized;
            }

            //三角形的三个点y值相同，面积为0
            if (t0.y == t1.y && t1.y == t2.y) return;
            //根据y值大小对坐标进行排序
            if (t0.y > t1.y)
            {
                TempSwap(ref t0, ref t1);
                TempSwap(ref vertices[0], ref vertices[1]);
                TempSwap(ref z[0], ref z[1]);
                TempSwap(ref h[0], ref h[1]);
            }
            if (t0.y > t2.y)
            {
                TempSwap(ref t0, ref t2);
                TempSwap(ref vertices[0], ref vertices[2]);
                TempSwap(ref z[0], ref z[2]);
                TempSwap(ref h[0], ref h[2]);
            }
            if (t1.y > t2.y)
            {
                TempSwap(ref t1, ref t2);
                TempSwap(ref vertices[1], ref vertices[2]);
                TempSwap(ref z[1], ref z[2]);
                TempSwap(ref h[1], ref h[2]);
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

                Vector2 uvA = vertices[0].texture_uv + (vertices[2].texture_uv - vertices[0].texture_uv) * alpha;
                Vector2 uvB = second_half ? vertices[1].texture_uv + (vertices[2].texture_uv - vertices[1].texture_uv) * beta : vertices[0].texture_uv + (vertices[1].texture_uv - vertices[0].texture_uv) * beta;

                float zA = z[0] + (z[2] - z[0]) * alpha;
                float zB = second_half ? z[1] + (z[2] - z[1]) * beta : z[0] + (z[1] - z[0]) * beta;

                Vector3 hA = h[0] + (h[2] - h[0]) * alpha;
                Vector3 hB = second_half ? h[1] + (h[2] - h[1]) * beta : h[0] + (h[1] - h[0]) * beta;

                Vector3 tbn_normalA = vertices[0].vertex_normal + (vertices[2].vertex_normal - vertices[0].vertex_normal) * alpha;
                Vector3 tbn_normalB = second_half ? vertices[1].vertex_normal + (vertices[2].vertex_normal - vertices[1].vertex_normal) * beta : vertices[0].vertex_normal + (vertices[1].vertex_normal - vertices[0].vertex_normal) * beta;

                Vector3 temp_tangentA = vertices[0].vertex_tangent + (vertices[2].vertex_tangent - vertices[0].vertex_tangent) * alpha;
                Vector3 temp_tangentB = second_half ? vertices[1].vertex_tangent + (vertices[2].vertex_tangent - vertices[1].vertex_tangent) * beta : vertices[0].vertex_tangent + (vertices[1].vertex_tangent - vertices[0].vertex_tangent) * beta;


                if (A.x > B.x)
                {
                    TempSwap(ref A, ref B);
                    TempSwap(ref uvA, ref uvB);
                    TempSwap(ref zA, ref zB);
                    TempSwap(ref hA, ref hB);
                    TempSwap(ref tbn_normalA, ref tbn_normalB);
                    TempSwap(ref temp_tangentA, ref temp_tangentB);
                }

                for (int j = A.x; j <= B.x; j++)
                {
                    float phi = B.x == A.x ? 1f : (float)(j - A.x) / (float)(B.x - A.x);
                    IntVector2 P = A + (B - A) * phi;
                    float zP = zA + (zB - zA) * phi;

                    // float ity_diffuseP = ity_diffuseA + (ity_diffuseB - ity_diffuseA) * phi;
                    // float ity_specularP = ity_specularA + (ity_specularB - ity_specularA) * phi;

                    if (P.x >= 0 && P.x < canvas_width && P.y > 0 && P.y < canvas_height)
                    {

                        if (zbuffer[P.x + P.y * canvas_width] < zP)
                        {
                            zbuffer[P.x + P.y * canvas_width] = zP;
                            if (useDiffuseTex && diffuseTex != null)
                            {
                                Vector2 uvP = uvA + (uvB - uvA) * phi;

                                Vector3 tbn_normalP = (tbn_normalA + (tbn_normalB - tbn_normalA) * phi).normalized;
                                Vector3 temp_tangentP = (temp_tangentA + (temp_tangentB - temp_tangentA) * phi).normalized;
                                Vector3 tbn_tangentP = (temp_tangentP - Vector3.DotProduct(temp_tangentP, tbn_normalP) * tbn_normalP).normalized;
                                Vector3 tbn_bitangentP = Vector3.CrossProduct(tbn_tangentP, tbn_normalP).normalized;
                                Matrix4x4 TBN = new Matrix4x4();
                                TBN[1, 1] = tbn_tangentP.x; TBN[1, 2] = tbn_tangentP.y; TBN[1, 3] = tbn_tangentP.z;
                                TBN[2, 1] = tbn_bitangentP.x; TBN[2, 2] = tbn_bitangentP.y; TBN[2, 3] = tbn_bitangentP.z;
                                TBN[3, 1] = tbn_normalP.x; TBN[3, 2] = tbn_normalP.y; TBN[3, 3] = tbn_normalP.z;
                                TBN[4, 4] = 1;

                                reNormalizedTBN = TBN.reNormalized.transposed;

                                Color normalColor = normalTex.GetPixel((int)(uvP.x * normalTex.Width), (int)((1 - uvP.y) * normalTex.Height));
                                Vector3 tangentNormal = new Vector3(normalColor.R / 255f, normalColor.G / 255f, normalColor.B / 255f);
                                Vector3 normal = (new Vector4(tangentNormal.x * 2.0f - 1.0f, tangentNormal.y * 2.0f - 1.0f, tangentNormal.z * 2.0f - 1.0f, 1) * reNormalizedTBN).transTo3D.normalized;
                                Vector3 hP = hA + (hB - hA) * phi;
                                float dot_nh = Math.Max(0, Vector3.DotProduct(normal, hP));

                                float ity_diffuseP = Vector3.DotProduct(normal, light_dir);
                                float ity_specularP = (float)Math.Pow(dot_nh, gloss);

                                float intensity = (ity_diffuseP + ity_specularP) * 0.5f + 0.5f;
                                int res = Math.Clamp(Convert.ToInt32(intensity * 255), 0, 255);
                                color = Color.FromArgb(res, res, res);

                                Color texColor = diffuseTex.GetPixel((int)Math.Floor(uvP.x * diffuseTex.Width), (int)Math.Floor((1 - uvP.y) * diffuseTex.Height));
                                Color resColor = ColorProduct(color, texColor);
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

        public void DrawTriangle2P(ref Bitmap bitmap)
        {
            Vector2 bboxmin = new Vector2(float.MaxValue, float.MaxValue);
            Vector2 bboxmax = new Vector2(float.MinValue, float.MinValue);
            Vector2 clamp = new Vector2(bitmap.Width - 1, bitmap.Height - 1);
            Vector3[] screen_pos = new Vector3[3];
            Vector3[] h = new Vector3[3];
            for (int i = 0; i < 3; i++)
            {
                //bboxmin.x = Math.Max(0f, Math.Min(bboxmin.x, vertexPos[i].x));
                //bboxmin.y = Math.Max(0f, Math.Min(bboxmin.y, vertexPos[i].y));
                //bboxmax.x = Math.Min(clamp.x, Math.Max(bboxmax.x, vertexPos[i].x));
                //bboxmax.y = Math.Min(clamp.y, Math.Max(bboxmax.y, vertexPos[i].y));
                screen_pos[i] = new Vector3();
                screen_pos[i].x = (vertices[i].ndc_pos.x + 1) * canvas_width / 2;
                screen_pos[i].y = (vertices[i].ndc_pos.y + 1) * canvas_height / 2;
                screen_pos[i].z = vertices[i].ndc_pos.z;
                for (int j = 0; j < 2; j++)
                {
                    bboxmin[j] = Math.Max(0f, Math.Min(bboxmin[j], screen_pos[i][j]));
                    bboxmax[j] = Math.Min(clamp[j], Math.Max(bboxmax[j], screen_pos[i][j]));
                }
                Vector3 view_dir = (vertices[i].world_pos - camPos).normalized;
                h[i] = (light_dir + view_dir).normalized;
            }

            Vector3 p = new Vector3();
            Vector2 uvP;
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

                        uvP = vertices[0].texture_uv * bc_screen.x + vertices[1].texture_uv * bc_screen.y + vertices[2].texture_uv * bc_screen.z;

                        Vector3 tbn_normal = (vertices[0].vertex_normal * bc_screen.x + vertices[1].vertex_normal * bc_screen.y + vertices[2].vertex_normal * bc_screen.z).normalized;
                        Vector3 temp_tangent = (vertices[0].vertex_tangent * bc_screen.x + vertices[1].vertex_tangent * bc_screen.y + vertices[2].vertex_tangent * bc_screen.z).normalized;
                        Vector3 tbn_tangent = (temp_tangent - Vector3.DotProduct(temp_tangent, tbn_normal) * tbn_normal).normalized;
                        Vector3 tbn_bitangent = Vector3.CrossProduct(tbn_tangent, tbn_normal);
                        Matrix4x4 TBN = new Matrix4x4();
                        TBN[1, 1] = tbn_tangent.x; TBN[1, 2] = tbn_tangent.y; TBN[1, 3] = tbn_tangent.z;
                        TBN[2, 1] = tbn_bitangent.x; TBN[2, 2] = tbn_bitangent.y; TBN[2, 3] = tbn_bitangent.z;
                        TBN[3, 1] = tbn_normal.x; TBN[3, 2] = tbn_normal.y; TBN[3, 3] = tbn_normal.z;
                        TBN[4, 4] = 1;

                        reNormalizedTBN = TBN.reNormalized.transposed;

                        Color normalColor = normalTex.GetPixel((int)(uvP.x * normalTex.Width), (int)((1 - uvP.y) * normalTex.Height));
                        Vector3 tangentNormal = new Vector3(normalColor.R / 255f, normalColor.G / 255f, normalColor.B / 255f);
                        Vector3 normal = (new Vector4(tangentNormal.x * 2.0f - 1.0f, tangentNormal.y * 2.0f - 1.0f, tangentNormal.z * 2.0f - 1.0f, 1) * reNormalizedTBN).transTo3D.normalized;

                        Vector3 bc_h = h[0] * bc_screen.x + h[1] * bc_screen.y + h[2] * bc_screen.z;
                        float dot_nh = Math.Max(0, Vector3.DotProduct(normal, bc_h));

                        float ity_diffuseP = Vector3.DotProduct(normal, light_dir);
                        float ity_specularP = (float)Math.Pow(dot_nh, gloss);

                        float intensity = (ity_diffuseP + ity_specularP) * 0.5f + 0.5f;
                        int res = Math.Clamp(Convert.ToInt32(intensity * 255), 0, 255);
                        color = Color.FromArgb(res, res, res);

                        if (useDiffuseTex && diffuseTex != null)
                        {
                            Color texColor = diffuseTex.GetPixel((int)(uvP.x * diffuseTex.Width), (int)((1 - uvP.y) * diffuseTex.Height));
                            Color resColor = ColorProduct(color, texColor);
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

        public void DrawTrangle(bool useBaryCentric, string lightingType, ref Bitmap bitmap)
        {
            Vector3 n = Vector3.CrossProduct(vertices[2].world_pos - vertices[0].world_pos, vertices[1].world_pos - vertices[0].world_pos);
            float diffuse = Vector3.DotProduct(n.normalized, light_dir);
            //背面剔除
            if (diffuse < 0) return;
            switch (lightingType)
            {
                case "IsFlatLit":
                    Vector3 view_dir = ((vertices[0].world_pos * 1 / 3 + vertices[1].world_pos * 1 / 3 + vertices[2].world_pos * 1 / 3) - camPos).normalized; //由于z轴反转，view_dir也反转
                    Vector3 h = (light_dir + view_dir).normalized;
                    float dot_nh = Math.Max(0, Vector3.DotProduct(n.normalized, h));
                    float specular = (float)Math.Pow(dot_nh, gloss);
                    float intensity = (diffuse + specular) * 0.8f + 0.2f;
                    int res = Math.Clamp(Convert.ToInt32(intensity * 255), 0, 255);
                    color = Color.FromArgb(res, res, res);
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
                    if (!useBaryCentric)
                    {
                        DrawTriangle1P(ref bitmap);
                    }
                    else
                    {
                        DrawTriangle2P(ref bitmap);
                    }
                    break;
                default:
                    return;
            }
        }

        public void SetData(Vector3 camPos, Triangle triangle, bool useDiffuseTex, Bitmap? diffuseTex, Bitmap? normalTex, float[] zbuffer, Vector3 light_dir)
        {
            this.vertices = new Vertex[3];
            this.camPos = camPos;
            this.vertices[0] = triangle.v0;
            this.vertices[1] = triangle.v1;
            this.vertices[2] = triangle.v2;
            this.diffuseTex = diffuseTex;
            this.normalTex = normalTex;
            this.zbuffer = zbuffer;
            this.light_dir = light_dir;
            this.useDiffuseTex = useDiffuseTex;
        }

        private Color ColorProduct(Color A, Color B)
        {
            return Color.FromArgb(A.R * B.R / 255, A.G * B.G / 255, A.B * B.B / 255);
        }
    }
}
