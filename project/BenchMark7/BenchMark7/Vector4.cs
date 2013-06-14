using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchMark7
{
    public class Vector4
    {
        public Vector4()
        {
        }

        public Vector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Vector4(Vector3 vector3, float w)
        {
            X = vector3.X;
            Y = vector3.Y;
            Z = vector3.Z;
            W = w;
        }

        public static Vector4 operator /(Vector4 v, float f)
        {
            return new Vector4
            {
                X = v.X / f,
                Y = v.Y / f,
                Z = v.Z / f,
                W = v.W / f
            };
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public static Vector4 Normalize(Vector4 v)
        {
            float l = (float)Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z + v.W * v.W);
            return new Vector4
            {
                X = v.X / l,
                Y = v.Y / l,
                Z = v.Z / l,
                W = v.W / l
            };
        }

        public static Vector4 operator +(Vector4 v1, Vector4 v2)
        {
            return new Vector4
            {
                X = v1.X + v2.X,
                Y = v1.Y + v2.Y,
                Z = v1.Z + v2.Z,
                W = v1.W + v2.W
            };
        }

        public static Vector4 operator -(Vector4 v1, Vector4 v2)
        {
            return new Vector4
            {
                X = v1.X - v2.X,
                Y = v1.Y - v2.Y,
                Z = v1.Z - v2.Z,
                W = v1.W - v2.W
            };
        }

        public static Vector4 operator -(Vector4 v)
        {
            return new Vector4
            {
                X = -v.X,
                Y = -v.Y,
                Z = -v.Z,
                W = -v.W
            };
        }

        public static float Dot(Vector4 v1, Vector4 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z + v1.W * v2.W;
        }
    }
}
