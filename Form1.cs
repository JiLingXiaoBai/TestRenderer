using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

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
        LightingType lightingType;
        bool isReady = false;

        float near_dis = 50;
        float far_dis = 550;
        float near_width = 500;
        float near_height = 500;


        public Form1()
        {
            InitializeComponent();
            m_scale = new Matrix4x4();
            m_scale[1, 1] = ScaleValue.Value;
            m_scale[2, 2] = ScaleValue.Value;
            m_scale[3, 3] = ScaleValue.Value;
            m_scale[4, 4] = 1;

            m_rotationX = new Matrix4x4();
            SetRotateMatrix(Axis.X);
            m_rotationY = new Matrix4x4();
            SetRotateMatrix(Axis.Y);
            m_rotationZ = new Matrix4x4();
            SetRotateMatrix(Axis.Z);

            m_view = new Matrix4x4();
            m_view[1, 1] = 1;
            m_view[2, 2] = 1;
            m_view[3, 3] = 1;
            m_view[4, 3] = 200;//z轴偏移200，x和y不变,变换后三角形在坐标系顶点为正值，所以200取正值
            m_view[4, 4] = 1;

            m_orthoProjection = new Matrix4x4();
            m_orthoProjection[1, 1] = 2 / near_width;
            m_orthoProjection[2, 2] = 2 / near_height;
            m_orthoProjection[3, 3] = 2 / (near_dis - far_dis); 
            m_orthoProjection[4, 3] = (near_dis + far_dis) / (near_dis - far_dis);
            m_orthoProjection[4, 4] = 1;

            m_perspectiveProjection = new Matrix4x4();
            m_perspectiveProjection[1, 1] = 2 * near_dis / near_width;
            m_perspectiveProjection[2, 2] = 2 * near_dis / near_height;
            m_perspectiveProjection[3, 3] = (near_dis + far_dis) / (near_dis - far_dis);
            m_perspectiveProjection[3, 4] = -1;
            m_perspectiveProjection[4, 3] = 2 * near_dis * far_dis / (near_dis - far_dis);

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
            if (isReady)
            {
                for (int i = 0; i < Canvas.canvas_width * Canvas.canvas_height; i++)
                {
                    zbuffer[i] = float.MinValue;
                }
                bitmap = new Bitmap(Canvas.canvas_width, Canvas.canvas_height);

                if (IsLine.Checked) //线框模式
                {
                    for(int i = 0; i < 3; i++)
                    {
                        Canvas.DrawLine(lineVertNDC_pos[i, 0], lineVertNDC_pos[i, 1], ref bitmap, Color.Red);
                    }
                }
                else
                {
                    float intensity = 0;
                    Color color = Color.White;
                    switch (lightingType)
                    {
                        case LightingType.Flat:
                            Vector3 n = Vector3.CrossProduct(world_pos[2] - world_pos[0], world_pos[1] - world_pos[0]);
                            intensity = Vector3.DotProduct(n.normalized, light_dir);
                            break;
                        case LightingType.Vertex:
                            break;
                        case LightingType.Pixel:
                            break;
                        default: 
                            return;
                    }

                    //背面剔除
                    if (intensity > 0)
                    {
                        int gray = Convert.ToInt32(intensity * 255);
                        color = Color.FromArgb(gray, gray, gray);
                    }

                    Canvas.DrawTrangle(ndc_pos, texture_uv, IsDiffuseTex.Checked, objLoader.baseTexture, IsZBuffer.Checked, zbuffer, color, ref bitmap);
                }
            }
            
            this.pictureBox1.Image = bitmap;
        }

        Vector3[,] lineVertNDC_pos = new Vector3[3, 2];
        Vector2[] texture_uv = new Vector2[3];
        Vector3[] model_pos = new Vector3[3];
        Vector3[] world_pos = new Vector3[3];
        Vector3[] ndc_pos = new Vector3[3];
        
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (objLoader.mesh != null)
            {
                //MVP 矩阵
                Matrix4x4 M = m_scale * (m_rotationZ * (m_rotationY * m_rotationX));
                Matrix4x4 MV = m_view * M;
                Matrix4x4 MVP;
                if (IsOrtho.Checked)
                    MVP = m_orthoProjection * MV;
                else
                    MVP = m_perspectiveProjection * MV;

                for (int i = 0; i < objLoader.mesh.Surfaces.Count; i++)
                {
                    ObjLoader.Surface s = objLoader.mesh.Surfaces[i];
                    
                    for (int j = 0; j < 3; j++)
                    {
                        if (IsLine.Checked)
                        {
                            Vector3 v0 = objLoader.mesh.Vertex[s.Vert[j]];
                            Vector3 v1 = objLoader.mesh.Vertex[s.Vert[(j + 1) % 3]];
                            
                            lineVertNDC_pos[j, 0] = (new Vector4(v0) * MVP).transTo3D;
                            lineVertNDC_pos[j, 1] = (new Vector4(v1) * MVP).transTo3D;
                        }
                        else
                        {
                            texture_uv[j] = objLoader.mesh.Texture[s.Tex[j]];
                            model_pos[j] = objLoader.mesh.Vertex[s.Vert[j]];
                            world_pos[j] = (new Vector4(model_pos[j]) * M).transTo3D;
                            ndc_pos[j] = (new Vector4(model_pos[j]) * MVP).transTo3D;
                        }   
                    }

                }
                isReady = true;
                this.Invalidate();
            }
        }

        private void SetRotateMatrix(Axis axis)
        {
            double angle;
            switch (axis)
            {
                case Axis.X:
                    angle = RotateX.Value / 180.0 * Math.PI;
                    m_rotationX[1, 1] = 1;
                    m_rotationX[2, 2] = (float)Math.Cos(angle);
                    m_rotationX[2, 3] = (float)Math.Sin(angle);
                    m_rotationX[3, 2] = (float)-Math.Sin(angle);
                    m_rotationX[3, 3] = (float)Math.Cos(angle);
                    m_rotationX[4, 4] = 1;
                    break;
                case Axis.Y:
                    angle = RotateY.Value / 180.0 * Math.PI;
                    m_rotationY[1, 1] = (float)Math.Cos(angle);
                    m_rotationY[1, 3] = (float)Math.Sin(angle);
                    m_rotationY[2, 2] = 1;
                    m_rotationY[3, 1] = (float)-Math.Sin(angle);
                    m_rotationY[3, 3] = (float)Math.Cos(angle);
                    m_rotationY[4, 4] = 1;
                    break;
                case Axis.Z:
                    angle = RotateZ.Value / 180.0 * Math.PI;
                    m_rotationZ[1, 1] = (float)Math.Cos(angle);
                    m_rotationZ[1, 2] = (float)Math.Sin(angle);
                    m_rotationZ[2, 1] = (float)-Math.Sin(angle);
                    m_rotationZ[2, 2] = (float)Math.Cos(angle);
                    m_rotationZ[3, 3] = 1;
                    m_rotationZ[4, 4] = 1;
                    break;
            }
        }

        private void RotateX_Scroll(object sender, EventArgs e)
        {
            SetRotateMatrix(Axis.X);
        }

        private void RotateY_Scroll(object sender, EventArgs e)
        {
            SetRotateMatrix(Axis.Y);
        }

        private void RotateZ_Scroll(object sender, EventArgs e)
        {
            SetRotateMatrix(Axis.Z);
        }

        private void ScaleValue_Scroll(object sender, EventArgs e)
        {
            for (int i = 1; i <= 3; i++)
            {
                m_scale[i, i] = ScaleValue.Value;
            }
        }

        private void ChangeLightingType(object sender, EventArgs e)
        {
            for(int i = 0; i < lightingSelect.Items.Count; i++)
            {
                if(lightingSelect.GetItemCheckState(i) == CheckState.Checked)
                {
                    switch (i)
                    {
                        case 0:
                            lightingType = LightingType.Flat; break;
                        case 1:
                            lightingType = LightingType.Vertex; break;
                        case 2:
                            lightingType = LightingType.Pixel; break;
                        default:
                            return;
                    }
                }
            }
        }
    }

    public enum LightingType
    {
        Flat,
        Vertex,
        Pixel
    }

    public enum Axis
    {
        X,Y,Z,
    }
}