using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchMark7.Renderer
{
    abstract class Shader
    {
        public Shader(Engine engine)
        {
            Engine = engine;
        }

        protected Engine Engine { get; set; }

        public void DrawPrimitive(Triangle triangle)
        {
            var a = VertexShader(new VertexShaderInput { Vertex = triangle.Vertices[0] });
            var b = VertexShader(new VertexShaderInput { Vertex = triangle.Vertices[1] });
            var c = VertexShader(new VertexShaderInput { Vertex = triangle.Vertices[2] });

            if (a == null || b == null || c == null)
            {
                return;
            }

            var viewportA = a.Position * Engine.ViewportTransform;
            var viewportB = b.Position * Engine.ViewportTransform;
            var viewportC = c.Position * Engine.ViewportTransform;

            int Y1 = (int)Math.Round(16.0f * viewportA.Y);
            int Y2 = (int)Math.Round(16.0f * viewportB.Y);
            int Y3 = (int)Math.Round(16.0f * viewportC.Y);

            int X1 = (int)Math.Round(16.0f * viewportA.X);
            int X2 = (int)Math.Round(16.0f * viewportB.X);
            int X3 = (int)Math.Round(16.0f * viewportC.X);

            int DX12 = X1 - X2;
            int DX23 = X2 - X3;
            int DX31 = X3 - X1;

            int DY12 = Y1 - Y2;
            int DY23 = Y2 - Y3;
            int DY31 = Y3 - Y1;

            int FDX12 = DX12 << 4;
            int FDX23 = DX23 << 4;
            int FDX31 = DX31 << 4;

            int FDY12 = DY12 << 4;
            int FDY23 = DY23 << 4;
            int FDY31 = DY31 << 4;

            int minx = (Math.Min(X1, Math.Min(X2, X3)) + 0xF) >> 4;
            int maxx = (Math.Max(X1, Math.Max(X2, X3)) + 0xF) >> 4;
            int miny = (Math.Min(Y1, Math.Min(Y2, Y3)) + 0xF) >> 4;
            int maxy = (Math.Max(Y1, Math.Max(Y2, Y3)) + 0xF) >> 4;

            int C1 = DY12 * X1 - DX12 * Y1;
            int C2 = DY23 * X2 - DX23 * Y2;
            int C3 = DY31 * X3 - DX31 * Y3;

            if (DY12 < 0 || (DY12 == 0 && DX12 > 0)) C1++;
            if (DY23 < 0 || (DY23 == 0 && DX23 > 0)) C2++;
            if (DY31 < 0 || (DY31 == 0 && DX31 > 0)) C3++;

            int CY1 = C1 + DX12 * (miny << 4) - DY12 * (minx << 4);
            int CY2 = C2 + DX23 * (miny << 4) - DY23 * (minx << 4);
            int CY3 = C3 + DX31 * (miny << 4) - DY31 * (minx << 4);

            for (int y = miny; y < maxy; y++)
            {
                int CX1 = CY1;
                int CX2 = CY2;
                int CX3 = CY3;

                for (int x = minx; x < maxx; x++)
                {
                    if (CX1 > 0 && CX2 > 0 && CX3 > 0)
                    {
                        float b1 = ((viewportB.Y - viewportC.Y) * (x - viewportC.X) + (viewportC.X - viewportB.X) * (y - viewportC.Y))
                                / ((viewportB.Y - viewportC.Y) * (viewportA.X - viewportC.X) + (viewportC.X - viewportB.X) * (viewportA.Y - viewportC.Y));

                        float b2 = ((viewportC.Y - viewportA.Y) * (x - viewportC.X) + (viewportA.X - viewportC.X) * (y - viewportC.Y))
                            / ((viewportB.Y - viewportC.Y) * (viewportA.X - viewportC.X) + (viewportC.X - viewportB.X) * (viewportA.Y - viewportC.Y));

                        float b3 = 1 - b1 - b2;

                        var position = new Vector4(
                                        b1 * a.Position.X + b2 * b.Position.X + b3 * c.Position.X,
                                        b1 * a.Position.Y + b2 * b.Position.Y + b3 * c.Position.Y,
                                        b1 * a.Position.Z + b2 * b.Position.Z + b3 * c.Position.Z,
                                        b1 * a.Position.W + b2 * b.Position.W + b3 * c.Position.W
                                        );

                        if (Engine.DepthBuffer.Data[y, x] > position.Z)
                        {
                            if (a.TextureCoord != null && b.TextureCoord != null && c.TextureCoord != null)
                            {
                                PixelShader(new PixelShaderInput
                                {
                                    RenderTargetX = x,
                                    RenderTargetY = y,
                                    Position = position,
                                    Normal = a.Normal,
                                    TextureCoord = new Vector2(
                                        b1 * a.TextureCoord.X + b2 * b.TextureCoord.X + b3 * c.TextureCoord.X,
                                        b1 * a.TextureCoord.Y + b2 * b.TextureCoord.Y + b3 * c.TextureCoord.Y
                                        ),
                                    Color = a.Color
                                });
                            }
                            else
                            {
                                PixelShader(new PixelShaderInput
                                {
                                    RenderTargetX = x,
                                    RenderTargetY = y,
                                    Position = position,
                                    Normal = a.Normal,
                                    Color = new Vector3(
                                        b1 * a.Color.X + b2 * b.Color.X + b3 * c.Color.X,
                                        b1 * a.Color.Y + b2 * b.Color.Y + b3 * c.Color.Y,
                                        b1 * a.Color.Z + b2 * b.Color.Z + b3 * c.Color.Z
                                        )
                                });
                            }
                        }
                    }

                    CX1 -= FDY12;
                    CX2 -= FDY23;
                    CX3 -= FDY31;
                }

                CY1 += FDX12;
                CY2 += FDX23;
                CY3 += FDX31;
            }
        }

        protected abstract VertexShaderOutput VertexShader(VertexShaderInput input);
        protected abstract void PixelShader(PixelShaderInput input);
    }
}
