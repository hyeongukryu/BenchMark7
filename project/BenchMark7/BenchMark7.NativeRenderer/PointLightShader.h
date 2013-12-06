#pragma once

#include "Shader.h"
#include "Buffers.h"
#include <memory>

namespace BenchMark7
{
	class PointLightShader : public Shader
	{
	public:

		Vector4 LightPosition;
		float Intensity;

		PointLightShader(std::shared_ptr<Engine> engine) : Shader(engine)
		{
		}
	
	protected:

		VertexShaderOutput VertexShader(const VertexShaderInput &input) const override
		{
			VertexShaderOutput output;
			
			output.Position = Vector4(input.Vertex.Position, 1);
			output.Color = input.Vertex.Color;

			return output;
		}

		void PixelShader(const PixelShaderInput &input) const override
		{
			int x = input.RenderTargetX,
				y = input.RenderTargetY;

			Vector4 L = (LightPosition * engine_->Camera.Transform - input.Position).Nomalize();
			Vector4 N = Vector4(engine_->NormalBuffer.Data[y][x], 0);
			Vector4 eye = Vector4(-engine_->PositionBuffer.Data[y][x], 0).Nomalize();
			Vector4 H = (eye + L).Nomalize();

			Vector3 &backBuffer = engine_->BackBuffer.Data[y][x];
			Vector3 albedo = engine_->AlbedoBuffer.Data[y][x];

			backBuffer += albedo * Intensity *
				std::max(0.0f, Vector4::Dot(N, L)) * (1.0f / L.LengthSquared());

			float NdotH = Vector4::Dot(N, H);
			
			float power = engine_->SpecularPowerBuffer.Data[y][x];
			float intensity = engine_->SpecularIntensityBuffer.Data[y][x];

			backBuffer += albedo * std::pow(NdotH, power) * intensity;
			backBuffer += Vector3(1, 1, 1) * std::pow(NdotH, power + 10) * intensity;
		}
	};
}
