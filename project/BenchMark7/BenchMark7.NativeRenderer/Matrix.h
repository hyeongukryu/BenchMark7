#pragma once

#include "Vectors.h"

namespace BenchMark7
{
	struct Matrix
	{
		float Data[4][4];

		Matrix()
		{
			Data[0][0] = Data[0][1] = Data[0][2] = Data[0][3] =
				Data[1][0] = Data[1][1] = Data[1][2] = Data[1][3] =
				Data[2][0] = Data[2][1] = Data[2][2] = Data[2][3] = 
				Data[3][0] = Data[3][1] = Data[3][2] = Data[3][3] = 0;
		}

		Matrix(float d00, float d01, float d02, float d03,
			float d10, float d11, float d12, float d13,
			float d20, float d21, float d22, float d23,
			float d30, float d31, float d32, float d33)
		{
			Data[0][0] = d00;
			Data[0][1] = d01;
			Data[0][2] = d02;
			Data[0][3] = d03;
			Data[1][0] = d10;
			Data[1][1] = d11;
			Data[1][2] = d12;
			Data[1][3] = d13;
			Data[2][0] = d20;
			Data[2][1] = d21;
			Data[2][2] = d22;
			Data[2][3] = d23;
			Data[3][0] = d30;
			Data[3][1] = d31;
			Data[3][2] = d32;
			Data[3][3] = d33;
		}

		Matrix operator *(const Matrix &m) const
		{
			Matrix result;

			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					for (int p = 0; p < 4; p++)
					{
						result.Data[i][j] += Data[i][p] * m.Data[p][j];
					}
				}
			}

			return result;
		}

		static Matrix CreateIdentity()
		{
			return Matrix(
				1, 0, 0, 0,
				0, 1, 0, 0,
				0, 0, 1, 0,
				0, 0, 0, 1
				);
		}

		static Matrix CreateScale(float scale)
		{
			return Matrix(
				scale, 0, 0, 0,
				0, scale, 0, 0,
				0, 0, scale, 0,
				0, 0, 0, 1
				);
		}

		static Matrix CreateLook(
			const Vector3 &target, const Vector3 &camera, const Vector3 &up)
		{
			Vector3 n = (target - camera).Normalize();
			Vector3 v = Vector3::Cross(up, n).Normalize();
			Vector3 u = Vector3::Cross(n, v);

			return Matrix(
				v.X, u.X, n.X, 0,
				v.Y, u.Y, n.Y, 0,
				v.Z, u.Z, n.Z, 0,
				-Vector3::Dot(camera, v), -Vector3::Dot(camera, u), -Vector3::Dot(camera, n), 1
				);
		}

		static Matrix CreateOrthogonalProjection(
			float width, float height, float farPlane, float nearPlane)
		{
			return Matrix(
				2.0f / width, 0, 0, 0,
				0, 2.0f / height, 0, 0,
				0, 0, 2.0f / (farPlane - nearPlane), 0,
				0, 0, -1, 1
				);
		}

		static Matrix CreateViewport(
			float width, float height, float maxZ, float minZ)
		{
			return Matrix(
				width / 2.0f, 0, 0, 0,
				0, -height / 2.0f, 0, 0,
				0, 0, maxZ - minZ, 0,
				width / 2.0f, height / 2.0f, minZ, 1
				);
		}
	};

	inline Vector4 operator *(const Vector4 &v, const Matrix &m)
	{
		return Vector4(
			v.X * m.Data[0][0] + v.Y * m.Data[1][0] + v.Z * m.Data[2][0] + v.W * m.Data[3][0],
			v.X * m.Data[0][1] + v.Y * m.Data[1][1] + v.Z * m.Data[2][1] + v.W * m.Data[3][1],
			v.X * m.Data[0][2] + v.Y * m.Data[1][2] + v.Z * m.Data[2][2] + v.W * m.Data[3][2],
			v.X * m.Data[0][3] + v.Y * m.Data[1][3] + v.Z * m.Data[2][3] + v.W * m.Data[3][3]
		);
	}
}
