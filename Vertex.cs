
namespace TestRenderer
{
    internal class Vertex
    {
        public Vector3 model_pos;
        public Vector3 world_pos;
        public Vector3 ndc_pos;
        public Vector3 vertex_normal;
        public Vector3 vertex_tangent;
        public Vector2 texture_uv;

        public Vertex()
        {
            this.model_pos = new Vector3();
            this.world_pos = new Vector3();
            this.ndc_pos = new Vector3();
            this.vertex_normal = new Vector3();
            this.vertex_tangent = new Vector3();
            this.texture_uv = new Vector2();
        }
    }
}