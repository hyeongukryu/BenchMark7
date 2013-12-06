#include "stdafx.h"
#include "Engine.h"

namespace BenchMark7
{
	Engine::Engine(int width, int height) :
		Width(width), Height(height),
		AlbedoBuffer(Width, Height),
		PositionBuffer(Width, Height),
		DepthBuffer(Width, Height),
		NormalBuffer(Width, Height),
		SpecularIntensityBuffer(Width, Height),
		SpecularPowerBuffer(Width, Height),
		BackBuffer(Width, Height)
	{
		ViewportTransform = Matrix::CreateViewport(width, height, 1, 0);
	}

	void Engine::ClearRenderTargets()
	{
		DepthBuffer.Clear(std::numeric_limits<float>::max());
	}

	void Engine::Render(std::shared_ptr<BenchMark7::Model> model, BenchMark7::Camera camera)
	{
		Model = model;
		Camera = camera;

		Render();
	}

	void Engine::Render()
	{
		ClearRenderTargets();
		Camera.Freeze();

		// �� ������
		for (auto &shader : ModelShaders)
		{
			for (auto &triangle : Model->Triangles)
			{
				shader->DrawPrimitive(triangle);
			}
		}

		if (Debug != nullptr)
		{
			Debug();
		}

		// ȭ�� ��ü�� ���� �簢������ ���� ó��
		Camera.World = Camera.View = Camera.Projection = Matrix::CreateIdentity();

		Triangle full[] = {
			Triangle(
				Vertex(Vector3(-1, 1, 0), Vector3(1, 1, 1)),
				Vertex(Vector3(-1, -1, 0), Vector3(1, 1, 1)),
				Vertex(Vector3(1, 1, 0), Vector3(1, 1, 1))
			),
			Triangle(
				Vertex(Vector3(1, 1, 0), Vector3(1, 1, 1)),
				Vertex(Vector3(-1, -1, 0), Vector3(1, 1, 1)),
				Vertex(Vector3(1, -1, 0), Vector3(1, 1, 1))
			),
		};

		for (auto &shader : ScreenShaders)
		{
			for (auto &triangle : full)
			{
				shader->DrawPrimitive(triangle);
			}
		}
	}
}
