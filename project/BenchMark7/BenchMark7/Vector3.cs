using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchMark7
{
    public class Vector3
    {
        public Vector3()
        {
        }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public static Vector3 operator *(Vector3 v, float f)
        {
            return new Vector3
            {
                X = v.X * f,
                Y = v.Y * f,
                Z = v.Z * f
            };
        }

        public static Vector3 operator *(float f, Vector3 v)
        {
            return new Vector3
            {
                X = v.X * f,
                Y = v.Y * f,
                Z = v.Z * f
            };
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3
            {
                X = v1.X + v2.X,
                Y = v1.Y + v2.Y,
                Z = v1.Z + v2.Z
            };
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3
            {
                X = v1.X - v2.X,
                Y = v1.Y - v2.Y,
                Z = v1.Z - v2.Z
            };
        }

        public static Vector3 operator -(Vector3 v)
        {
            return new Vector3
            {
                X = -v.X,
                Y = -v.Y,
                Z = -v.Z
            };
        }

        public static float Dot(Vector3 v1, Vector3 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        public static Vector3 Cross(Vector3 a, Vector3 b)
        {
            return new Vector3
            {
                X = a.Y * b.Z - a.Z * b.Y,
                Y = a.Z * b.X - a.X * b.Z,
                Z = a.X * b.Y - a.Y * b.X
            };
        }

        public static Vector3 Normalize(Vector3 v)
        {
            float l = (float)Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);
            return new Vector3
            {
                X = v.X / l,
                Y = v.Y / l,
                Z = v.Z / l
            };
        }
    }
}
