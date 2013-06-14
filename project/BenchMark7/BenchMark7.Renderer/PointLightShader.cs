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

        public Vector4 LightPosition { get; set; }
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

            var L = Vector4.Normalize(LightPosition - input.Position);
            var N = new Vector4(Engine.NormalBuffer.Data[y, x], 0);
            var dot = Vector4.Dot(N, L);
            var eye = Vector4.Normalize(new Vector4(-Engine.PositionBuffer.Data[y, x], 0));
            var H = Vector4.Normalize(eye + L);

            if (dot < 0)
                dot = 0;

            Engine.BackBuffer.Data[y, x] += Engine.AlbedoBuffer.Data[y, x] * (Intensity * dot
                + (float)Math.Pow(Math.Max(0, Vector4.Dot(N, H)), Engine.SpecularPowerBuffer.Data[y, x])
                * Engine.SpecularIntensityBuffer.Data[y, x]);
        }
    }
}
