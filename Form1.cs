using System.Numerics;
namespace TestRenderer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        ObjLoader objLoader = new ObjLoader();
        string? fileName = null;
        Bitmap bitmap = new Bitmap(400, 400);


        private void Form1_Load(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件";
            dialog.Filter = "模型文件(*.obj)|*.obj";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                fileName = dialog.FileName;
                if (fileName == null)
                    return;
                objLoader.LoadObjFile(fileName);
            }
        }

        private void Form1_Paint(object sender, EventArgs e)
        {
            if (objLoader.mesh != null)
            {
                for (int i = 0; i < objLoader.mesh.Surfaces.Count; i++)
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
                }
            }

            this.pictureBox1.Image = bitmap;
        }

    }
}