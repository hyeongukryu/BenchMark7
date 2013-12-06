#pragma once

#include "Vectors.h"
#include <vector>

namespace BenchMark7
{
	struct Buffer1
	{
		int Width, Height;
		std::vector<std::vector<float>> Data;

		Buffer1(int width, int height)
			: Width(width), Height(height)
		{
			Clear(0);
		}

		void Clear(float value)
		{
			Data = std::vector<std::vector<float>>(
				Height, std::vector<float>(Width, value));
		}
	};

	struct Buffer3
	{
		int Width, Height;
		std::vector<std::vector<Vector3>> Data;

		Buffer3()
			: Width(0), Height(0)
		{
		}

		Buffer3(int width, int height)
			: Width(width), Height(height)
		{
			Clear(Vector3());
		}

		void Clear(Vector3 value)
		{
			Data = std::vector<std::vector<Vector3>>(
				Height, std::vector<Vector3>(Width, value));
		}

		Vector3 Sample(float u, float v) const
		{
			u = -u;
			
			u = u * Height - 0.5f;
			v = v * Width - 0.5f;

			int x = (int)floor(u);
			int y = (int)floor(v);
			float u_ratio = u - x;
			float v_ratio = v - y;
			float u_opposite = 1 - u_ratio;
			float v_opposite = 1 - v_ratio;
			int x1 = x + 1;
			int y1 = y + 1;

			while (x < 0)			x += Height;
			while (y < 0)			y += Width;
			while (x >= Height)		x -= Height;
			while (y >= Width)		y -= Width;

			while (x1 < 0)			x1 += Height;
			while (y1 < 0)			y1 += Width;
			while (x1 >= Height)	x1 -= Height;
			while (y1 >= Height)	y1 -= Width;

			return Vector3(
				(Data[x][y].X * u_opposite + Data[x1][y].X * u_ratio) * v_opposite +
                (Data[x][y1].X * u_opposite + Data[x1][y1].X * u_ratio) * v_ratio,
                (Data[x][y].Y * u_opposite + Data[x1][y].Y * u_ratio) * v_opposite +
                (Data[x][y1].Y * u_opposite + Data[x1][y1].Y * u_ratio) * v_ratio,
                (Data[x][y].Z * u_opposite + Data[x1][y].Z * u_ratio) * v_opposite +
                (Data[x][y1].Z * u_opposite + Data[x1][y1].Z * u_ratio) * v_ratio
				);
		}
	};

}