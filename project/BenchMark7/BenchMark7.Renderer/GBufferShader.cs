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
            var camera = Engine.Camera;

            Vector4 position = new Vector4(input.Vertex.Position, 1);
            Vector4 normal = new Vector4(input.Vertex.Normal, 0);

            position *= camera.World;
            position *= camera.View;
            position *= camera.Projection;

            return new VertexShaderOutput
            {
                Position = position,
                Normal = normal,
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
                    input.TextureCoord.X, input.TextureCoord.Y);
            }

            float depth = 0;

            Vector3 normal = null;
            
            Engine.DepthBuffer.Data[y, x] = depth;
            Engine.NormalBuffer.Data[y, x] = normal;
            Engine.AlbedoBuffer.Data[y, x] = albedo;
            Engine.SpecularIntensityBuffer.Data[y, x] = 10;
            Engine.SpecularPowerBuffer.Data[y, x] = 10;
        }
    }
}
