using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchMark7
{
    public class Vertex
    {
        public Vertex(Vector3 position, Vector3 color)
        {
            Position = position;
            Color = color;
        }

        public Vertex(Vector3 position, Vector3 normal, Vector2 textureCoord)
        {
            Position = position;
            Normal = normal;
            TextureCoord = textureCoord;
        }

        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
        public Vector2 TextureCoord { get; set; }
        public Vector3 Color { get; set; }
    }
}
