using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchMark7.Renderer
{
    abstract class Shader
    {
        public Shader(Engine engine)
        {
            Engine = engine;
        }
        
        private Engine Engine { get; set; }

        public void DrawPrimitive(Triangle triangle)
        {
        }
    }
}
