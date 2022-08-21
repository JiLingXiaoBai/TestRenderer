﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRenderer
{
    internal class ObjLoader
    {

        public class Surface 
        { 
            public int[] Vert = new int[3];
            public int[] Tex = new int[3];
            public int[] Norm = new int[3];
        };

        public class Model
        {
            //代表顶点。格式为V X Y Z，V后面的X Y Z表示三个顶点坐标。浮点型
            public List<Vector3> Vertex = new List<Vector3>();
            //表示纹理坐标。格式为VT TU TV。浮点型
            public List<Vector2> Texture = new List<Vector2>();
            //法向量。每个三角形的三个顶点都要指定一个法向量。格式为 NX NY NZ。浮点型
            public List<Vector3> Normal = new List<Vector3>();
            //面。面后面跟着的整型值分别是属于这个面的顶点、纹理坐标、法向量的索引。
            public List<Surface> Surfaces = new List<Surface>();
            //面的格式为：f Vertex1/Texture1/Normal1 Vertex2/Texture2/Normal2 Vertex3/Texture3/Normal3
        }

        public Model mesh = new Model();
        private string? fileName = null;
        public Bitmap? baseTexture = null;

        public void LoadObjFile(String fileName)
        {
            StreamReader objReader = new StreamReader(fileName);
            string texLineTem;
            this.fileName = fileName;
            while (objReader.Peek() != -1)
            {
                texLineTem = objReader.ReadLine();

                if (texLineTem.Length < 2) continue;

                if (texLineTem.IndexOf("v") == 0)
                {
                    if (texLineTem.IndexOf("t") == 1)//vt 0.581151 0.979929 纹理
                    {
                        string[] tempArray = texLineTem.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                        Vector2 vt = new Vector2();
                        vt.x = float.Parse(tempArray[1]);
                        vt.y = float.Parse(tempArray[2]);
                        mesh.Texture.Add(vt);
                    }
                    else if (texLineTem.IndexOf("n") == 1)//vn 0.637005 -0.0421857 0.769705 法向量
                    {
                        string[] tempArray = texLineTem.Split(new char[] { '/', ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
                        Vector3 vn = new Vector3();
                        vn.x = float.Parse(tempArray[1]);
                        vn.y = float.Parse(tempArray[2]);
                        if (tempArray[3] == "\\")
                        {
                            texLineTem = objReader.ReadLine();
                            vn.z = float.Parse(texLineTem);
                        }
                        else vn.z = float.Parse(tempArray[3]);

                        mesh.Normal.Add(vn);
                    }
                    else
                    {//v -53.0413 158.84 -135.806 点
                        string[] tempArray = texLineTem.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                        Vector3 v = new Vector3();
                        v.x = float.Parse(tempArray[1]);
                        v.y = float.Parse(tempArray[2]);
                        v.z = float.Parse(tempArray[3]);
                        mesh.Vertex.Add(v);
                    }
                }
                else if (texLineTem.IndexOf("f") == 0)
                {
                    //f 2443//2656 2442//2656 2444//2656 面
                    string[] tempArray = texLineTem.Split(new char[] { '/', ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
                    Surface surface = new Surface();
                    int i = 2;
                    int k = 1;
                    while (i >= 0)
                    {
                        if (mesh.Vertex.Count() != 0)
                        {
                            surface.Vert[i] = int.Parse(tempArray[k]) - 1;
                            k++;
                        }
                        if (mesh.Texture.Count() != 0)
                        {
                            surface.Tex[i] = int.Parse(tempArray[k]) - 1;
                            k++;
                        }
                        if (mesh.Normal.Count() != 0)
                        {
                            surface.Norm[i] = int.Parse(tempArray[k]) - 1;
                            k++;
                        }
                        i--;
                    }
                    mesh.Surfaces.Add(surface);
                }
            }

            baseTexture = GetBaseTexture();
        }

        public Bitmap? GetBaseTexture()
        {
            if (fileName == null)
                return null;
            char[] chars = { '.', 'o', 'b', 'j' };
            string textureName = fileName.TrimEnd(chars) + "_diffuse.png";

            /*FileStream stream = new FileStream(textureName, FileMode.Open);
            return Bitmap.FromStream(stream) as Bitmap;*/
            return Bitmap.FromFile(textureName) as Bitmap;
            
        }
    }
}
