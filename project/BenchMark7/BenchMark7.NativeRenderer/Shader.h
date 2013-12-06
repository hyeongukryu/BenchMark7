#pragma once

#include "Model.h"
#include "Vertex.h"

namespace BenchMark7
{
	class Engine;

	struct VertexShaderInput
	{
		VertexShaderInput()
		{
		}

		VertexShaderInput(Vertex vertex) :
			Vertex(vertex)
		{
		}

		Vertex Vertex;
	};

	struct VertexShaderOutput
	{
		VertexShaderOutput()
		{
		}

		VertexShaderOutput(
			Vector4 position, Vector4 normal,
			Vector2 textureCoord, Vector3 color)
			: Position(position), Normal(normal),
			TextureCoord(textureCoord), Color(color)
		{
		}
		
		Vector4 Position, Normal;
		Vector2 TextureCoord;
		Vector3 Color;
	};

	struct PixelShaderInput
	{
		PixelShaderInput()
		{
		}

		PixelShaderInput(int renderTargetX, int renderTargetY,
			Vector4 position, Vector4 normal, Vector2 textureCoord,
			Vector3 color)
			: RenderTargetX(renderTargetX), RenderTargetY(renderTargetY),
			Position(position), Normal(normal),
			TextureCoord(textureCoord), Color(color)
		{
		}

		int RenderTargetX, RenderTargetY;
		Vector4 Position, Normal;
		Vector2 TextureCoord;
		Vector3 Color;
	};

	class Shader
	{
	public:
		Shader(std::shared_ptr<Engine> engine)
		{
			engine_ = engine;
		}

		void DrawPrimitive(const Triangle &triangle) const;

	protected:
		std::shared_ptr<Engine> engine_;

		virtual VertexShaderOutput VertexShader(const VertexShaderInput &input) const = 0;
		virtual void PixelShader(const PixelShaderInput &input) const = 0;
	};
}

#include "Engine.h"
