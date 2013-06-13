using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchMark7
{
    public class Matrix
    {
        public float[,] Data { get; set; }

        public Matrix()
        {
            Data = new float[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Data[i, j] = 0;
                }
            }
        }

        public static Matrix CreateIdentity()
        {
            return new Matrix
            {
                Data = new float[,] {
                    {1, 0, 0, 0},
                    {0, 1, 0, 0},
                    {0, 0, 1, 0},
                    {0, 0, 0, 1}
                }
            };
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            Matrix result = new Matrix();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int p = 0; p < 4; p++)
                    {
                        result.Data[i, j] +=
                            m1.Data[i, p] * m2.Data[p, j];
                    }
                }
            }
            return result;
        }

        public static Vector4 operator *(Vector4 v, Matrix m)
        {
            return new Vector4
            {
                X = v.X * m.Data[0, 0] + v.Y * m.Data[1, 0] + v.Z * m.Data[2, 0] + v.W * m.Data[3, 0],
                Y = v.X * m.Data[0, 1] + v.Y * m.Data[1, 1] + v.Z * m.Data[2, 1] + v.W * m.Data[3, 1],
                Z = v.X * m.Data[0, 2] + v.Y * m.Data[1, 2] + v.Z * m.Data[2, 2] + v.W * m.Data[3, 2],
                W = v.X * m.Data[0, 3] + v.Y * m.Data[1, 3] + v.Z * m.Data[2, 3] + v.W * m.Data[3, 3]
            };
        }

        public static Matrix CreateScale(float scale)
        {
            return new Matrix
            {
                Data = new float[,] {
                    {scale, 0, 0, 0},
                    {0, scale, 0, 0},
                    {0, 0, scale, 0},
                    {0, 0, 0, 1}
                }
            };
        }

        public static Matrix CreateLook(Vector3 target, Vector3 camera, Vector3 up)
        {
            var n = Vector3.Normalize(target - camera);
            var v = Vector3.Normalize(Vector3.Cross(up, n));
            var u = Vector3.Cross(n, v);

            return new Matrix
            {
                Data = new float[,] {
                    {v.X, u.X, n.X, 0},
                    {v.Y, u.Y, n.Y, 0},
                    {v.Z, u.Z, n.Z, 0},
                    {-Vector3.Dot(camera, v), -Vector3.Dot(camera, u), -Vector3.Dot(camera, n), 1}
                }
            };
        }

        public static Matrix CreateOrthogonalProjection(float width, float height, float far, float near)
        {
            return new Matrix
            {
                Data = new float[,] {
                    {2.0f / width, 0, 0, 0},
                    {0, 2.0f / height, 0, 0},
                    {0, 0, 2.0f / (far - near), 0},
                    {0, 0, -1, 1}
                }
            };
        }

        public static Matrix CreateViewport(float width, float height, float maxZ, float minZ)
        {
            return new Matrix
            {
                Data = new float[,] {
                    {width / 2.0f, 0, 0, 0},
                    {0, -height / 2.0f, 0, 0},
                    {0, 0, maxZ - minZ, 0},
                    {width / 2.0f, height / 2.0f, minZ, 1}
                }
            };
        }
    }
}
