using System.Numerics;
namespace TestRenderer
{
    public partial class Form1 : Form
    {
        

        ObjLoader objLoader = new ObjLoader();
        
        Bitmap bitmap = new Bitmap(Canvas.canvas_width, Canvas.canvas_height);
        float[] zbuffer = new float[Canvas.canvas_width * Canvas.canvas_height];

        Vector3 light_dir = new Vector3(0, 0, 1);

        Matrix4x4 m_scale;//缩放矩阵
        Matrix4x4 m_rotationX;//绕X轴旋转矩阵
        Matrix4x4 m_rotationY;//绕Y轴旋转矩阵
        Matrix4x4 m_rotationZ;//绕Z轴旋转矩阵
        Matrix4x4 m_view;//将空间坐标变换为摄像机坐标矩阵,即平移矩阵
        Matrix4x4 m_orthoProjection;//正交投影矩阵
        Matrix4x4 m_perspectiveProjection;//透视投影矩阵

        public Form1()
        {
            InitializeComponent();
            m_scale = new Matrix4x4();
            m_scale[1, 1] = 200;
            m_scale[2, 2] = 200;
            m_scale[3, 3] = 200;
            m_scale[4, 4] = 1;

            m_rotationX = new Matrix4x4();
            m_rotationY = new Matrix4x4();
            m_rotationZ = new Matrix4x4();

            m_view = new Matrix4x4();
            m_view[1, 1] = 1;
            m_view[2, 2] = 1;
            m_view[3, 3] = 1;
            m_view[4, 3] = 200;//z轴偏移200，x和y不变,变换后三角形在坐标系顶点为正值，所以200取正值
            m_view[4, 4] = 1;

            m_orthoProjection = new Matrix4x4();
            /*m_orthoProjection[1, 1] = 1;
            m_orthoProjection[2, 2] = 1;
            m_orthoProjection[3, 3] = 1;
            m_orthoProjection[3, 4] = 1.0f / 200;// 1/d, d为焦距
            m_orthoProjection[4, 4] = 1;*/

            m_perspectiveProjection = new Matrix4x4();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件";
            dialog.Filter = "模型文件(*.obj)|*.obj";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = dialog.FileName;
                if (fileName == null)
                    return;
                objLoader.LoadObjFile(fileName);
            }
        }

        private void Form1_Paint(object sender, EventArgs e)
        {
            if (objLoader.mesh != null)
            {

                for(int i = 0; i < Canvas.canvas_width * Canvas.canvas_height; i++)
                {
                    zbuffer[i] = float.MinValue;
                }

                /*for (int i = 0; i < objLoader.mesh.Surfaces.Count; i++)
                {
                    ObjLoader.Surface s = objLoader.mesh.Surfaces[i];
                    for (int j = 0; j < 3; j++)
                    {
                        Vector3 v0 = objLoader.mesh.Vertex[s.Vert[j]];
                        Vector3 v1 = objLoader.mesh.Vertex[s.Vert[(j + 1) % 3]];

                        int x0 = (int)(v0.x + 1) * 1 + 150;
                        int y0 = (int)(v0.y + 1) * 1 + 150;
                        int x1 = (int)(v1.x + 1) * 1 + 150;
                        int y1 = (int)(v1.y + 1) * 1 + 150;
                        Canvas.DrawLine(x0, y0, x1, y1, ref bitmap, Color.Red);
                    }
                }*/

                for (int i = 0; i < objLoader.mesh.Surfaces.Count; i++)
                {
                    ObjLoader.Surface s = objLoader.mesh.Surfaces[i];
                    IntVector2[] screen_coords = new IntVector2[3];
                    Vector2[] texture_uv = new Vector2[3];
                    Vector3[] world_coords = new Vector3[3];

                    for (int j = 0; j < 3; j++)
                    {
                        Vector3 v = objLoader.mesh.Vertex[s.Vert[j]];
                        screen_coords[j] = new IntVector2(Convert.ToInt16((v.x * 0.8f + 1) * Canvas.canvas_width/2), Convert.ToInt16((v.y * 0.8f + 1) * Canvas.canvas_height / 2));
                        world_coords[j] = v;
                        texture_uv[j] = objLoader.mesh.Texture[s.Tex[j]];
                    }

                    Vector3 n = Vector3.CrossProduct(world_coords[2] - world_coords[0], world_coords[1] - world_coords[0]);
                
                    float intensity = Vector3.DotProduct(n.normalized, light_dir);
                    //背面剔除
                    if(intensity > 0)
                    {
                        int gray = Convert.ToInt32(intensity * 255);
                        Color color = Color.FromArgb(gray, gray, gray);
                        if (objLoader.baseTexture != null)
                            Canvas.DrawTriangle1(screen_coords[0], screen_coords[1], screen_coords[2], texture_uv[0], texture_uv[1], texture_uv[2], objLoader.baseTexture, ref bitmap, color);
                    }
                }

                /*for (int i = 0; i < objLoader.mesh.Surfaces.Count; i++)
                {
                    ObjLoader.Surface s = objLoader.mesh.Surfaces[i];
                    Vector2[] texture_uv = new Vector2[3];
                    Vector3[] world_coords = new Vector3[3];

                    for (int j = 0; j < 3; j++)
                    {
                        texture_uv[j] = objLoader.mesh.Texture[s.Tex[j]];
                        world_coords[j] = objLoader.mesh.Vertex[s.Vert[j]];
                    }

                    Vector3 n = Vector3.CrossProduct(world_coords[2] - world_coords[0], world_coords[1] - world_coords[0]);

                    float intensity = Vector3.DotProduct(n.normalized, light_dir);
                    //背面剔除
                    if (intensity > 0)
                    {
                        int gray = Convert.ToInt16(intensity * 255);
                        Color color = Color.FromArgb(gray, gray, gray);

                        if(objLoader.baseTexture != null)
                            Canvas.DrawTriangle2(world_coords, texture_uv, zbuffer, objLoader.baseTexture, ref bitmap, color);
                    }
                } */  
            }
            
            this.pictureBox1.Image = bitmap;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //MVP 矩阵

            this.Invalidate();
        }

        private void RotateX_Scroll(object sender, EventArgs e)
        {
            double angle = RotateX.Value / 180.0 * Math.PI;
            m_rotationX[1, 1] = 1;
            m_rotationX[2, 2] = (float)Math.Cos(angle);
            m_rotationX[2, 3] = (float)Math.Sin(angle);
            m_rotationX[3, 2] = (float)-Math.Sin(angle);
            m_rotationX[3, 3] = (float)Math.Cos(angle);
            m_rotationX[4, 4] = 1;
        }

        private void RotateY_Scroll(object sender, EventArgs e)
        {
            double angle = RotateY.Value / 180.0 * Math.PI;
            m_rotationY[1, 1] = (float)Math.Cos(angle);
            m_rotationY[1, 3] = (float)Math.Sin(angle);
            m_rotationY[2, 2] = 1;
            m_rotationY[3, 1] = (float)-Math.Sin(angle);
            m_rotationY[3, 3] = (float)Math.Cos(angle);
            m_rotationY[4, 4] = 1;
        }

        private void RotateZ_Scroll(object sender, EventArgs e)
        {
            double angle = RotateZ.Value / 180.0 * Math.PI;
            m_rotationZ[1, 1] = (float)Math.Cos(angle);
            m_rotationZ[1, 2] = (float)Math.Sin(angle);
            m_rotationZ[2, 1] = (float)-Math.Sin(angle);
            m_rotationZ[2, 2] = (float)Math.Cos(angle);
            m_rotationZ[3, 3] = 1;
            m_rotationZ[4, 4] = 1;
        }

        private void ScaleValue_Scroll(object sender, EventArgs e)
        {
            //m_view[4, 3] = (sender as TrackBar).Value;
        }

    }
}