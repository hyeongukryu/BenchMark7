using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BenchMark7.Renderer
{
    class AmbientLightShader : Shader
    {
        public AmbientLightShader(Engine engine)
            : base(engine)
        {
        }

        public float Intensity { get; set; }

        protected override VertexShaderOutput VertexShader(VertexShaderInput input)
        {
            Vector4 position = new Vector4(input.Vertex.Position, 1);

            return new VertexShaderOutput
            {
                Position = position,
                Color = input.Vertex.Color
            };
        }

        protected override void PixelShader(PixelShaderInput input)
        {
            int x = input.RenderTargetX,
                y = input.RenderTargetY;

            Engine.BackBuffer.Data[y, x] +=
                Intensity * Engine.AlbedoBuffer.Data[y, x];
        }
    }
}
