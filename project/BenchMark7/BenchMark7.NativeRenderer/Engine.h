#pragma once

#include "Buffers.h"
#include "Model.h"
#include "Camera.h"
#include "Matrix.h"
#include "Model.h"
#include "Shader.h"

namespace BenchMark7
{
	class Engine
	{
	public:

		std::shared_ptr<Model> Model;
		Camera Camera;

		int Width, Height;

		Buffer1 DepthBuffer, SpecularPowerBuffer, SpecularIntensityBuffer;
		Buffer3 PositionBuffer, NormalBuffer, AlbedoBuffer, BackBuffer;

		std::vector<std::shared_ptr<Shader>> ModelShaders;
		std::vector<std::shared_ptr<Shader>> ScreenShaders;
		
		std::function<void()> Debug;

		Matrix ViewportTransform;
		
		Engine(int width, int height);

		void ClearRenderTargets();
		void Render(std::shared_ptr<BenchMark7::Model> model, BenchMark7::Camera camera);

	private:
		void Render();
	};
}
