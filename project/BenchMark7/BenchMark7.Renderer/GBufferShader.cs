using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BenchMark7.Renderer
{
    class GBufferShader : Shader
    {
        public GBufferShader(Engine engine)
            : base(engine)
        {
        }

        protected override VertexShaderOutput VertexShader(VertexShaderInput input)
        {                        
            return new VertexShaderOutput
            {
                Position = new Vector4(input.Vertex.Position, 1) * Engine.Camera.Transform,
                Normal = new Vector4(input.Vertex.Normal ?? new Vector3(), 0),
                TextureCoord = input.Vertex.TextureCoord,
                Color = input.Vertex.Color
            };
        }

        protected override void PixelShader(PixelShaderInput input)
        {
            int x = input.RenderTargetX,
                y = input.RenderTargetY;

            Vector3 albedo = input.Color;
            if (input.TextureCoord != null)
            {
                albedo = Engine.Model.Texture.Sample(
                    input.TextureCoord.Y, input.TextureCoord.X);
            }

            Engine.PositionBuffer.Data[y, x] = new Vector3(input.Position.X, input.Position.Y, input.Position.Z);
            Engine.DepthBuffer.Data[y, x] = input.Position.Z;
            Engine.NormalBuffer.Data[y, x] = new Vector3(input.Normal.X, input.Normal.Y, input.Normal.Z);
            Engine.AlbedoBuffer.Data[y, x] = albedo;
            Engine.SpecularIntensityBuffer.Data[y, x] = 15f;
            Engine.SpecularPowerBuffer.Data[y, x] = 15f;
        }
    }
}
