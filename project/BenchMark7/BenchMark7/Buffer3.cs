using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchMark7
{
    public class Buffer3
    {
        public Buffer3(int width, int height)
        {
            Width = width;
            Height = height;
            Data = new Vector3[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Data[i, j] = new Vector3();
                }
            }
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public Vector3[,] Data { get; set; }

        public void Clear(Vector3 value)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Data[i, j] = value;
                }
            }
        }

        public Vector3 Sample(float u, float v)
        {
            u = -u;
            
            u = u * Height - 0.5f;
            v = v * Width - 0.5f;

            int x = (int)Math.Floor(u);
            int y = (int)Math.Floor(v);
            float u_ratio = u - x;
            float v_ratio = v - y;
            float u_opposite = 1 - u_ratio;
            float v_opposite = 1 - v_ratio;
            int x1 = x + 1;
            int y1 = y + 1;

            while (x < 0) x += Height;
            while (y < 0) y += Width;
            while (x >= Height) x -= Height;
            while (y >= Width) y -= Width;

            while (x1 < 0) x1 += Height;
            while (y1 < 0) y1 += Width;
            while (x1 >= Height) x1 -= Height;
            while (y1 >= Width) y1 -= Width;

            Vector3 result = new Vector3(
                (Data[x, y].X * u_opposite + Data[x1, y].X * u_ratio) * v_opposite +
                (Data[x, y1].X * u_opposite + Data[x1, y1].X * u_ratio) * v_ratio,
                (Data[x, y].Y * u_opposite + Data[x1, y].Y * u_ratio) * v_opposite +
                (Data[x, y1].Y * u_opposite + Data[x1, y1].Y * u_ratio) * v_ratio,
                (Data[x, y].Z * u_opposite + Data[x1, y].Z * u_ratio) * v_opposite +
                (Data[x, y1].Z * u_opposite + Data[x1, y1].Z * u_ratio) * v_ratio
                );

            return result;
        }
    }
}
