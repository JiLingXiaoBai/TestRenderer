
namespace TestRenderer
{
    public partial class Form1 : Form
    {
        ObjLoader objLoader = new ObjLoader();
        Canvas canvas = new Canvas();
        Bitmap bitmap = new Bitmap(Canvas.canvas_width, Canvas.canvas_height);
        float[] zbuffer = new float[Canvas.canvas_width * Canvas.canvas_height];
        Vector3 light_dir = new Vector3(0, 0, 1);

        Matrix4x4 m_scale;
        Matrix4x4 m_rotationX;
        Matrix4x4 m_rotationY;
        Matrix4x4 m_rotationZ;
        Matrix4x4 m_rotation;
        Matrix4x4 m_view;
        Matrix4x4 m_orthoProjection;
        Matrix4x4 m_perspectiveProjection;
        string lightingType = "IsFlatLit";
        bool isReady = false;

        float near_dis = 500;
        float far_dis = 1000;
        float near_width = 500;
        float near_height = 500;
        Vector3? cameraPos = new Vector3();
        List<Triangle> triangles;
        Vector3[][][]? lineVertNDC_pos;

        public Form1()
        {
            InitializeComponent();

            m_scale = new Matrix4x4();
            m_scale[1, 1] = ScaleValue.Value * ScaleValueX.Value / 100f;
            m_scale[2, 2] = ScaleValue.Value * ScaleValueY.Value / 100f;
            m_scale[3, 3] = ScaleValue.Value * ScaleValueZ.Value / 100f;
            m_scale[4, 4] = 1;

            m_rotationX = new Matrix4x4();
            SetRotateMatrix(Axis.X);
            m_rotationY = new Matrix4x4();
            SetRotateMatrix(Axis.Y);
            m_rotationZ = new Matrix4x4();
            SetRotateMatrix(Axis.Z);
            m_rotation = new Matrix4x4();
            m_rotation = m_rotationX.Mul(m_rotationY).Mul(m_rotationZ);

            m_view = new Matrix4x4();
            m_view[1, 1] = 1;
            m_view[2, 2] = 1;
            m_view[3, 3] = -1;
            m_view[4, 3] = 800;
            m_view[4, 4] = 1;

            m_orthoProjection = new Matrix4x4();
            m_orthoProjection[1, 1] = 2 / near_width;
            m_orthoProjection[2, 2] = 2 / near_height;
            m_orthoProjection[3, 3] = 2 / (near_dis - far_dis);
            m_orthoProjection[4, 3] = (near_dis + far_dis) / (far_dis - near_dis);
            m_orthoProjection[4, 4] = 1;

            m_perspectiveProjection = new Matrix4x4();
            m_perspectiveProjection[1, 1] = 2 * near_dis / near_width;
            m_perspectiveProjection[2, 2] = 2 * near_dis / near_height;
            m_perspectiveProjection[3, 3] = (near_dis + far_dis) / (near_dis - far_dis);
            m_perspectiveProjection[3, 4] = 1;
            m_perspectiveProjection[4, 3] = 2 * near_dis * far_dis / (far_dis - near_dis);

            triangles = new List<Triangle>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Title = "选择模型";
            dialog.Filter = "(*.obj)|*.obj";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = dialog.FileName;
                if (fileName == null)
                    return;
                objLoader.LoadObjFile(fileName);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (objLoader.mesh != null)
            {
                if (!isReady)
                {
                    lineVertNDC_pos = new Vector3[objLoader.triangleCount][][];
                    for (int i = 0; i < objLoader.triangleCount; i++)
                    {
                        lineVertNDC_pos[i] = new Vector3[3][];
                        for (int j = 0; j < 3; j++)
                        {
                            lineVertNDC_pos[i][j] = new Vector3[2];
                        }
                    }
                }

                //MVP
                m_rotation = m_rotationX.Mul(m_rotationY).Mul(m_rotationZ);
                Matrix4x4 M = m_scale.Mul(m_rotation);
                Matrix4x4 MV = M.Mul(m_view);
                Matrix4x4 MVP;
                if (IsOrtho.Checked)
                    MVP = MV.Mul(m_orthoProjection);
                else
                    MVP = MV.Mul(m_perspectiveProjection);

                triangles.Clear();

                for (int i = 0; i < objLoader.triangleCount; i++)
                {
                    ObjLoader.Surface s = objLoader.mesh.m_Surfaces[i];
                    Vertex[] tempVertices = new Vertex[3];
                    for (int j = 0; j < 3; j++)
                    {
                        Vector3 v0 = objLoader.mesh.m_Vertex[s.Vert[j]];
                        Vector3 v1 = objLoader.mesh.m_Vertex[s.Vert[(j + 1) % 3]];

                        lineVertNDC_pos[i][j][0] = (new Vector4(v0) * MVP).transTo3D;
                        lineVertNDC_pos[i][j][1] = (new Vector4(v1) * MVP).transTo3D;

                        Vertex vert = new Vertex();
                        vert.texture_uv = objLoader.mesh.m_Texture[s.Tex[j]];
                        vert.model_pos = objLoader.mesh.m_Vertex[s.Vert[j]];
                        vert.world_pos = (new Vector4(vert.model_pos) * M).transTo3D;
                        vert.ndc_pos = (new Vector4(vert.model_pos) * MVP).transTo3D;
                        vert.vertex_tangent = new Vector3(0, 0, 0);
                        if (m_scale[1, 1] == m_scale[2, 2] && m_scale[1, 1] == m_scale[3, 3])
                            vert.vertex_normal = (new Vector4(objLoader.mesh.m_Normal[s.Norm[j]]) * M).transTo3D.normalized;
                        else
                            vert.vertex_normal = (new Vector4(objLoader.mesh.m_Normal[s.Norm[j]]) * (M.inverseMatrix.transposed)).transTo3D.normalized;
                        tempVertices[j] = vert;
                    }
                    triangles.Add(new Triangle(tempVertices[0], tempVertices[1], tempVertices[2]));
                }

                for (int i = 0; i < triangles.Count; i++)
                {
                    Vertex v0 = triangles[i].v0;
                    Vertex v1 = triangles[i].v1;
                    Vertex v2 = triangles[i].v2;

                    Vector3 edge1 = v1.world_pos - v0.world_pos;
                    Vector3 edge2 = v2.world_pos - v0.world_pos;
                    Vector2 deltaUV1 = v1.texture_uv - v0.texture_uv;
                    Vector2 deltaUV2 = v2.texture_uv - v0.texture_uv;

                    float f = 1.0f / (deltaUV1.x * deltaUV2.y - deltaUV1.y * deltaUV2.x);

                    Vector3 tangent = new Vector3();

                    tangent.x = f * (deltaUV2.y * edge1.x - deltaUV1.y * edge2.x);
                    tangent.y = f * (deltaUV2.y * edge1.y - deltaUV1.y * edge2.y);
                    tangent.z = f * (deltaUV2.y * edge1.z - deltaUV1.y * edge2.z);

                    triangles[i].v0.vertex_tangent += tangent;
                    triangles[i].v1.vertex_tangent += tangent;
                    triangles[i].v2.vertex_tangent += tangent;

                }

                for (int i = 0; i < triangles.Count; i++)
                {
                    Vertex v0 = triangles[i].v0;
                    Vertex v1 = triangles[i].v1;
                    Vertex v2 = triangles[i].v2;

                    triangles[i].v0.vertex_tangent = v0.vertex_tangent.normalized;
                    triangles[i].v1.vertex_tangent = v1.vertex_tangent.normalized;
                    triangles[i].v2.vertex_tangent = v2.vertex_tangent.normalized;
                }

                isReady = true;
                this.Invalidate();
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

                if (IsLine.Checked)
                {
                    for (int i = 0; i < objLoader.triangleCount; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            canvas.DrawLine(lineVertNDC_pos[i][j][0], lineVertNDC_pos[i][j][1], ref bitmap, Color.Red);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < triangles.Count; i++)
                    {
                        canvas.SetData(cameraPos, triangles[i], IsDiffuseTex.Checked, objLoader.baseTexture, objLoader.normalTexture, zbuffer, light_dir);
                        canvas.DrawTrangle(IsBaryCentric.Checked, lightingType, ref bitmap);
                    }
                }
            }

            this.pictureBox1.Image = bitmap;
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
            m_scale[1, 1] = ScaleValue.Value * ScaleValueX.Value / 100f;
            m_scale[2, 2] = ScaleValue.Value * ScaleValueY.Value / 100f;
            m_scale[3, 3] = ScaleValue.Value * ScaleValueZ.Value / 100f;
        }

        private void ScaleValueX_Scroll(object sender, EventArgs e)
        {
            m_scale[1, 1] = ScaleValue.Value * ScaleValueX.Value / 100f;
        }

        private void ScaleValueY_Scroll(object sender, EventArgs e)
        {
            m_scale[2, 2] = ScaleValue.Value * ScaleValueY.Value / 100f;
        }

        private void ScaleValueZ_Scroll(object sender, EventArgs e)
        {
            m_scale[3, 3] = ScaleValue.Value * ScaleValueZ.Value / 100f;
        }

        private void ChangeLightingType(object sender, EventArgs e)
        {
            if (lightingType.Equals(((RadioButton)sender).Name))
            {
                return;
            }
            else
            {
                Control[] controls = this.Controls.Find(lightingType, true);
                ((RadioButton)(controls[0])).Checked = false;
                lightingType = ((RadioButton)sender).Name;
            }
        }

        private void ResetAxisScale_Click(object sender, EventArgs e)
        {
            ScaleValueX.Value = 100;
            ScaleValueY.Value = 100;
            ScaleValueZ.Value = 100;
            m_scale[1, 1] = ScaleValue.Value * ScaleValueX.Value / 100f;
            m_scale[2, 2] = ScaleValue.Value * ScaleValueY.Value / 100f;
            m_scale[3, 3] = ScaleValue.Value * ScaleValueZ.Value / 100f;
        }
    }

    public enum Axis
    {
        X, Y, Z,
    }
}