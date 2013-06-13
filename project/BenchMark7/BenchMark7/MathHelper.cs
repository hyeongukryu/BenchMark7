using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchMark7
{
    public static class MathHelper
    {
        public static float Clamp(float value, float min, float max)
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
        public static byte BitmapClamp(float value)
        {
            value = MathHelper.Clamp(value, 0, 1.0f);
            value *= 255;
            return (byte)value;
        }
    }
}
