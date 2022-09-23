
namespace TestRenderer
{
    internal class IntVector2
    {
        public int x, y;
        public IntVector2() { }

        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    default: throw new IndexOutOfRangeException("Invalid IntVector2 index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    default: throw new IndexOutOfRangeException("Invalid IntVector2 index!");
                }
            }
        }

        public IntVector2(int x, int y)
        {
            this.x = x; this.y = y;
        }

        public IntVector2(IntVector2 v)
        {
            x = v.x; y = v.y;
        }

        public static IntVector2 operator +(IntVector2 v1, IntVector2 v2)
        {
            return new IntVector2(v1.x + v2.x, v1.y + v2.y);
        }

        public static IntVector2 operator -(IntVector2 v1, IntVector2 v2)
        {
            return new IntVector2(v1.x - v2.x, v1.y - v2.y);
        }

        public static IntVector2 operator -(IntVector2 v)
        {
            return new IntVector2(-v.x, -v.y);
        }

        public static IntVector2 operator *(IntVector2 v, int d)
        {
            return new IntVector2(v.x * d, v.y * d);
        }

        public static IntVector2 operator *(int d, IntVector2 v)
        {
            return new IntVector2(v.x * d, v.y * d);
        }

        public static IntVector2 operator *(float d, IntVector2 v)
        {
            return new IntVector2(Convert.ToInt16(v.x * d), Convert.ToInt16(v.y * d));
        }

        public static IntVector2 operator *(IntVector2 v, float d)
        {
            return new IntVector2(Convert.ToInt16(v.x * d), Convert.ToInt16(v.y * d));
        }

        public static IntVector2 operator /(IntVector2 v, float d)
        {
            return new IntVector2(Convert.ToInt16(v.x / d), Convert.ToInt16(v.y / d));
        }

        public static IntVector2 operator /(IntVector2 v, int d)
        {
            return new IntVector2(Convert.ToInt16((float)v.x / d), Convert.ToInt16((float)v.y / d));
        }
    }
}
