#pragma once

#include "Vectors.h"

namespace BenchMark7
{
	struct Vertex
	{
		Vector3 Position, Normal, Color;
		Vector2 TextureCoord;

		Vertex()
		{
		}

		Vertex(Vector3 position, Vector3 color)
			: Position(position), Color(color)
		{
		}

		Vertex(Vector3 position, Vector3 normal, Vector2 textureCoord)
			: Position(position), Normal(normal), TextureCoord(textureCoord)
		{
		}
	};
}