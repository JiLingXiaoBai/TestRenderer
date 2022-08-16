using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRenderer
{
    internal class ObjLoader
    {
        public class Vertex 
        {
            public float X;
            public float Y;
            public float Z;
        };

        public class Texture 
        {
            public float TU;
            public float TV;
        };

        public class Normal
        {
            public float NX;
            public float NY;
            public float NZ;
        };

        public class Surface 
        { 
            public int[] Vert = new int[3];
            public int[] Tex = new int[3];
            public int[] Norm = new int[3];
        };

        public class Model
        {
            //代表顶点。格式为V X Y Z，V后面的X Y Z表示三个顶点坐标。浮点型
            public List<Vertex> V = new List<Vertex>();
            //表示纹理坐标。格式为VT TU TV。浮点型
            public List<Texture> T = new List<Texture>();
            //法向量。每个三角形的三个顶点都要指定一个法向量。格式为 NX NY NZ。浮点型
            public List<Normal> N = new List<Normal>();
            //面。面后面跟着的整型值分别是属于这个面的顶点、纹理坐标、法向量的索引。
            public List<Surface> Surfaces = new List<Surface>();
            //面的格式为：f Vertex1/Texture1/Normal1 Vertex2/Texture2/Normal2 Vertex3/Texture3/Normal3
        }

        public Model mesh = new Model();

        public void LoadObjFile(String fileName)
        {
            StreamReader objReader = new StreamReader(fileName);
            
            string texLineTem;

            while (objReader.Peek() != -1)
            {
                texLineTem = objReader.ReadLine();

                if (texLineTem.Length < 2) continue;

                if (texLineTem.IndexOf("v") == 0)
                {
                    if (texLineTem.IndexOf("t") == 1)//vt 0.581151 0.979929 纹理
                    {
                        string[] tempArray = texLineTem.Split(' ');
                        Texture vt = new Texture();
                        vt.TU = float.Parse(tempArray[1]);
                        vt.TV = float.Parse(tempArray[2]);
                        mesh.T.Add(vt);
                    }
                    else if (texLineTem.IndexOf("n") == 1)//vn 0.637005 -0.0421857 0.769705 法向量
                    {
                        string[] tempArray = texLineTem.Split(new char[] { '/', ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
                        Normal vn = new Normal();
                        vn.NX = float.Parse(tempArray[1]);
                        vn.NY = float.Parse(tempArray[2]);
                        if (tempArray[3] == "\\")
                        {
                            texLineTem = objReader.ReadLine();
                            vn.NZ = float.Parse(texLineTem);
                        }
                        else vn.NZ = float.Parse(tempArray[3]);

                        mesh.N.Add(vn);
                    }
                    else
                    {//v -53.0413 158.84 -135.806 点
                        string[] tempArray = texLineTem.Split(' ');
                        Vertex v = new Vertex();
                        v.X = float.Parse(tempArray[1]);
                        v.Y = float.Parse(tempArray[2]);
                        v.Z = float.Parse(tempArray[3]);
                        mesh.V.Add(v);
                    }
                }
                else if (texLineTem.IndexOf("f") == 0)
                {
                    //f 2443//2656 2442//2656 2444//2656 面
                    string[] tempArray = texLineTem.Split(new char[] { '/', ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
                    Surface surface = new Surface();
                    int i = 0;
                    int k = 1;
                    while (i < 3)
                    {
                        if (mesh.V.Count() != 0)
                        {
                            surface.Vert[i] = int.Parse(tempArray[k]) - 1;
                            k++;
                        }
                        if (mesh.T.Count() != 0)
                        {
                            surface.Tex[i] = int.Parse(tempArray[k]) - 1;
                            k++;
                        }
                        if (mesh.N.Count() != 0)
                        {
                            surface.Norm[i] = int.Parse(tempArray[k]) - 1;
                            k++;
                        }
                        i++;
                    }
                    mesh.Surfaces.Add(surface);
                }
            }
        }

        

    }
}
