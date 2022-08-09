using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRenderer
{
    internal class Vector4
    {
        public float x, y, z, w;

        public Vector4() { }

        public Vector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vector4(Vector4 v)
        {
            x = v.x; y = v.y; z = v.z; w = v.w;
        }
    }
}
