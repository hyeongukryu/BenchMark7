#pragma once

#include "Shader.h"
#include <memory>

namespace BenchMark7
{
	class GBufferShader : public Shader
	{
	public:

		float SpecularIntensity, SpecularPower;

		GBufferShader(std::shared_ptr<Engine> engine) : Shader(engine)
		{
		}

	protected:

		VertexShaderOutput VertexShader(const VertexShaderInput &input) const override
		{
			auto& camera = engine_->Camera;
			return VertexShaderOutput(
				Vector4(input.Vertex.Position, 1) * camera.Transform,
				(Vector4(input.Vertex.Normal, 0) * camera.World *
				Matrix(1, 0, 0, 0,
				0, cos(0.5236), sin(0.5236), 0,
				0, -sin(0.5236), cos(0.5236), 0,
				0, 0, 0, 0)).Nomalize(),
				input.Vertex.TextureCoord,
				input.Vertex.Color
				);
		}

		void PixelShader(const PixelShaderInput &input) const override
		{
			int x = input.RenderTargetX,
				y = input.RenderTargetY;
			
			engine_->PositionBuffer.Data[y][x] = Vector3(input.Position.X, input.Position.Y, input.Position.Z);
			engine_->DepthBuffer.Data[y][x] = input.Position.Z;
			engine_->NormalBuffer.Data[y][x] = Vector3(input.Normal.X, input.Normal.Y, input.Normal.Z);
			engine_->AlbedoBuffer.Data[y][x] = engine_->Model->Texture.Sample(input.TextureCoord.Y, input.TextureCoord.X);
			engine_->SpecularIntensityBuffer.Data[y][x] = SpecularIntensity;
			engine_->SpecularPowerBuffer.Data[y][x] = SpecularPower;
		}
	};
}
