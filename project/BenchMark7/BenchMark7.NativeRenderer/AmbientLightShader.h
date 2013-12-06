#pragma once

#include "Shader.h"
#include <memory>

namespace BenchMark7
{
	class AmbientLightShader : public Shader
	{
	public:

		float Intensity;

		AmbientLightShader(std::shared_ptr<Engine> engine) : Shader(engine)
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
			
			engine_->BackBuffer.Data[y][x] +=
				Intensity * engine_->AlbedoBuffer.Data[y][x];
		}
	};
}
