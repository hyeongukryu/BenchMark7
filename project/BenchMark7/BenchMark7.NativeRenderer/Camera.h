#pragma once

#include "Matrix.h"

namespace BenchMark7
{
	struct Camera
	{
		Matrix World, View, Projection, Transform;

		Camera() :
			World(Matrix::CreateIdentity()),
			View(Matrix::CreateIdentity()),
			Projection(Matrix::CreateIdentity())
		{
		}

		void Freeze()
		{
			Transform = World * View * Projection;
		}
	};
}