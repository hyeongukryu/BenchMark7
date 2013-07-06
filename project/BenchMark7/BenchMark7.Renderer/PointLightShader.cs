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
            var eye = Vector4.Normalize(new Vector4(-Engine.PositionBuffer.Data[y, x], 0));
            var H = Vector4.Normalize(eye + L);
                        
            Engine.BackBuffer.Data[y, x] += Engine.AlbedoBuffer.Data[y, x] * Intensity *
                Math.Max(0, Vector4.Dot(N, L)) * (1.0f / Vector4.Dot(L, L));
            
            var NdotH = Vector4.Dot(N, H);

            var power = Engine.SpecularPowerBuffer.Data[y, x];
            var intensity = Engine.SpecularIntensityBuffer.Data[y, x];

            Engine.BackBuffer.Data[y, x] += Engine.AlbedoBuffer.Data[y, x] *
                (float)Math.Pow(NdotH, power) * intensity;
              
            Engine.BackBuffer.Data[y, x] += new Vector3(1, 1, 1) *
                (float)Math.Pow(NdotH, power + 10) * intensity;
        }
    }
}
