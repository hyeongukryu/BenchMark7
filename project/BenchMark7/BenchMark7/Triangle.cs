using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchMark7
{
    public class Triangle
    {
        public Triangle(Vertex a, Vertex b, Vertex c)
        {
            Vertices = new[] { a, b, c };
        }

        public Vertex[] Vertices { get; set; }
    }
}
