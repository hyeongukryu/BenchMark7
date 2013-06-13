using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BenchMark7.Renderer
{
    class ShadowShader : Shader
    {
        public ShadowShader(Engine engine)
            : base(engine)
        {
        }

        protected override VertexShaderOutput VertexShader(VertexShaderInput input)
        {
            throw new NotImplementedException();
        }

        protected override void PixelShader(PixelShaderInput input)
        {
            throw new NotImplementedException();
        }
    }
}
