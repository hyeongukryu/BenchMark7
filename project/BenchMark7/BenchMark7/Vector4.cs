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
    }
}
