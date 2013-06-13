using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BenchMark7.Renderer
{
    class PointLightShader : Shader
    {
        public PointLightShader(Engine engine)
            : base(engine)
        {
        }

        protected override VertexShaderOutput VertexShader(VertexShaderInput input)
        {
            return null;
        }

        protected override void PixelShader(PixelShaderInput input)
        {
        }
    }
}
