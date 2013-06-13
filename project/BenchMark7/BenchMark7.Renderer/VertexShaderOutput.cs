using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchMark7.Renderer
{
    class VertexShaderOutput
    {
        public Vector4 Position { get; set; }
        public Vector4 Normal { get; set; }
        public Vector2 TextureCoord { get; set; }
        public Vector3 Color { get; set; }
    }
}
