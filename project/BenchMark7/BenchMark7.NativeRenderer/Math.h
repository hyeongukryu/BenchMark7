#pragma once

namespace BenchMark7
{
	float Clamp(float value, float min, float max)
	{
		if (value < min)
		{
			return min;
		}
		if (value > max)
		{
			return max;
		}
		return value;
	}

	char BitmapClamp(float value)
	{
		value = Clamp(value, 0, 1.0f);
		value *= 255;
		return (char)value;
	}
}