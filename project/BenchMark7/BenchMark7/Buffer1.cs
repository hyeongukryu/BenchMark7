using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchMark7
{
    public class Buffer1
    {
        public Buffer1(int width, int height)
        {
            Width = width;
            Height = height;
            Data = new float[height, width];
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public float[,] Data { get; set; }

        public void Clear(float value)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Data[i, j] = value;
                }
            }
        }

        public float Sample(float u, float v)
        {
            u = v * Width - 0.5f;
            u = v * Height - 0.5f;
            int x = (int)Math.Floor(u);
            int y = (int)Math.Floor(v);
            float u_ratio = u - x;
            float v_ratio = v - y;
            float u_opposite = 1 - u_ratio;
            float v_opposite = 1 - v_ratio;
            float result = (Data[x, y] * u_opposite + Data[x + 1, y] * u_ratio) * v_opposite +
                (Data[x, y + 1] * u_opposite + Data[x + 1, y + 1] * u_ratio) * v_ratio;
            return result;
        }
    }
}
