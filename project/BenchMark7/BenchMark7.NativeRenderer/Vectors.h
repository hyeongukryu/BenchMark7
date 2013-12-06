#pragma once

#include <algorithm>

namespace BenchMark7
{

	struct Vector2
	{
		float X, Y;

		Vector2() : X(0), Y(0)
		{
		}

		Vector2(float x, float y) : X(x), Y(y)
		{
		}
	};

	struct Vector3
	{
		float X, Y, Z;

		Vector3() : X(0), Y(0), Z(0)
		{
		}

		Vector3(float x, float y, float z) : X(x), Y(y), Z(z)
		{
		}

		Vector3 operator +(const Vector3 &other) const
		{
			return Vector3(X + other.X, Y + other.Y, Z + other.Z);
		}

		Vector3 operator -(const Vector3 &other) const
		{
			return Vector3(X - other.X, Y - other.Y, Z - other.Z);
		}

		Vector3 operator -() const
		{
			return Vector3(-X, -Y, -Z);
		}

		Vector3 operator *(float f) const
		{
			return Vector3(X * f, Y * f, Z * f);
		}

		Vector3 operator /(float f) const
		{
			return Vector3(X / f, Y / f, Z / f);
		}

		Vector3& operator +=(const Vector3 &other)
		{
			X += other.X;
			Y += other.Y;
			Z += other.Z;

			return *this;
		}

		float LengthSquared() const
		{
			return X * X + Y * Y + Z * Z;
		}

		float Length() const
		{
			return sqrt(LengthSquared());
		}

		Vector3 Normalize() const
		{
			float length = Length();
			return Vector3(X / length, Y / length, Z / length);
		}

		static float Dot(const Vector3 &a, const Vector3 &b)
		{
			return a.X * b.X + a.Y + b.Y + a.Z + b.Z;
		}

		static Vector3 Cross(const Vector3 &a, const Vector3 &b)
		{
			return Vector3(
				a.Y * b.Z - a.Z * b.Y,
				a.Z * b.X - a.X * b.Z,
				a.X * b.Y - a.Y * b.X
				);
		}
	};

	inline Vector3 operator *(float f, const Vector3 &v)
	{
		return Vector3(f * v.X, f * v.Y, f * v.Z);
	}

	struct Vector4
	{
		float X, Y, Z, W;

		Vector4() : X(0), Y(0), Z(0), W(0)
		{
		}

		Vector4(float x, float y, float z, float w)
			: X(x), Y(y), Z(z), W(w)
		{
		}

		Vector4(Vector3 v3, float w)
			: X(v3.X), Y(v3.Y), Z(v3.Z), W(w)
		{
		}

		Vector4 operator /(float f) const
		{
			return Vector4(X / f, Y / f, Z / f, W / f);
		}

		float LengthSquared() const
		{
			return X * X + Y * Y + Z * Z + W * W;
		}

		float Length() const 
		{
			return std::sqrt(LengthSquared());
		}

		Vector4 Nomalize() const
		{
			float l = Length();
			return Vector4(X / l, Y / l, Z / l, W / l);
		}

		Vector4 operator +(const Vector4 &o) const
		{
			return Vector4(X + o.X, Y + o.Y, Z + o.Z, W + o.W);
		}

		Vector4 operator -(const Vector4 &o) const
		{
			return Vector4(X - o.X, Y - o.Y, Z - o.Z, W - o.W);
		}

		Vector4 operator -() const
		{
			return Vector4(-X, -Y, -Z, -W);
		}

		static float Dot(const Vector4 &a, const Vector4 &b)
		{
			return a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;
		}
	};

}