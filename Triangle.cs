
namespace TestRenderer
{
    internal class Triangle
    {
        public Vertex v0;
        public Vertex v1;
        public Vertex v2;

        public Triangle()
        {
            this.v0 = new Vertex();
            this.v1 = new Vertex();
            this.v2 = new Vertex();
        }

        public Triangle(Vertex v0, Vertex v1, Vertex v2)
        {
            this.v0 = v0;
            this.v1 = v1;
            this.v2 = v2;
        }
    }
}