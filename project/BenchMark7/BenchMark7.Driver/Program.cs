using BenchMark7.Renderer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BenchMark7.Driver
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch watch = Stopwatch.StartNew();

            Console.WriteLine("path");
            string path = @"C:\Users\HyeongUk\Documents\GitHub\BenchMark7\models\mark42\Mark 42.obj";
            string content = File.ReadAllText(path);

            Console.WriteLine("texturePath");
            string texturePath = @"C:\Users\HyeongUk\Documents\GitHub\BenchMark7\models\mark42\maps\42.png";
            Bitmap textureBitmap = new Bitmap(texturePath);
            BitmapData textureBitmapData = textureBitmap.LockBits(
                new Rectangle(0, 0, textureBitmap.Width, textureBitmap.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            int textureBytes = Math.Abs(textureBitmapData.Stride) * textureBitmap.Height;
            byte[] textureRgbValues = new byte[textureBytes];
            Marshal.Copy(textureBitmapData.Scan0, textureRgbValues, 0, textureBytes);
            Buffer3 texture = new Buffer3(textureBitmap.Width, textureBitmap.Height);

            for (int i = 0; i < textureBitmap.Height; i++)
            {
                for (int j = 0; j < textureBitmap.Width; j++)
                {
                    texture.Data[i, j].X = textureRgbValues[i * textureBitmapData.Stride + j * 3 + 0] / 255.0f;
                    texture.Data[i, j].Y = textureRgbValues[i * textureBitmapData.Stride + j * 3 + 1] / 255.0f;
                    texture.Data[i, j].Z = textureRgbValues[i * textureBitmapData.Stride + j * 3 + 2] / 255.0f;
                }
            }

            const int width = 1366;
            const int height = 768;

            Engine engine = new Engine(width, height);
            
            Model model = Model.FromString(content);
            model.Texture = texture;
            
            Camera camera = new Camera();
            camera.World = Matrix.CreateScale(600);
            camera.View = Matrix.CreateLook(
                new Vector3(0, 520, 0),
                new Vector3(400, 520, -1024),
                new Vector3(1, 0, 0));
            camera.Projection = Matrix.CreateOrthogonalProjection(1280, 720, 1024, 0);

            engine.Render(model, camera);

            Bitmap backBufferbitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            BitmapData backBufferbitmapData = backBufferbitmap.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            object buffer = engine.BackBuffer;

            int backBufferBytes = Math.Abs(backBufferbitmapData.Stride) * height;
            byte[] rgbValues = new byte[backBufferBytes];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (buffer is Buffer3)
                    {
                        rgbValues[i * backBufferbitmapData.Stride + j * 3 + 0] = MathHelper.BitmapClamp((buffer as Buffer3).Data[i, j].X);
                        rgbValues[i * backBufferbitmapData.Stride + j * 3 + 1] = MathHelper.BitmapClamp((buffer as Buffer3).Data[i, j].Y);
                        rgbValues[i * backBufferbitmapData.Stride + j * 3 + 2] = MathHelper.BitmapClamp((buffer as Buffer3).Data[i, j].Z);
                    }
                    else
                    {
                        rgbValues[i * backBufferbitmapData.Stride + j * 3 + 0] = MathHelper.BitmapClamp((buffer as Buffer1).Data[i, j]);
                        rgbValues[i * backBufferbitmapData.Stride + j * 3 + 1] = MathHelper.BitmapClamp((buffer as Buffer1).Data[i, j]);
                        rgbValues[i * backBufferbitmapData.Stride + j * 3 + 2] = MathHelper.BitmapClamp((buffer as Buffer1).Data[i, j]);
                    }
                }
            }

            Marshal.Copy(rgbValues, 0, backBufferbitmapData.Scan0, backBufferBytes);
            backBufferbitmap.UnlockBits(backBufferbitmapData);

            backBufferbitmap.Save("result.png");

            Console.WriteLine(watch.ElapsedMilliseconds);
            Console.ReadLine();
        }
    }
}
