
namespace TestRenderer
{
    internal class Vector2
    {
        public float x, y;
        public Vector2() { }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    default: throw new IndexOutOfRangeException("Invalid Vector2 index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    default: throw new IndexOutOfRangeException("Invalid Vector2 index!");
                }
            }
        }

        public Vector2(float x, float y)
        {
            this.x = x; this.y = y; 
        }

        public Vector2(Vector2 v)
        {
            x = v.x; y = v.y;
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x - v2.x, v1.y - v2.y);
        }

        public static Vector2 operator -(Vector2 v)
        {
            return new Vector2(-v.x, -v.y);
        }

        public static Vector2 operator *(float d, Vector2 v)
        {
            return new Vector2(d * v.x, d * v.y);
        }

        public static Vector2 operator *(Vector2 v, float d)
        {
            return new Vector2(d * v.x, d * v.y);
        }

        public static Vector2 operator /(Vector2 v, float d)
        {
            return new Vector2(v.x / d, v.y / d);
        }
    }
}
