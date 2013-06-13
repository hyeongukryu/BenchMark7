using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchMark7.Renderer
{
    public class Engine
    {
        public Model Model { get; set; }
        public Camera Camera { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public Buffer1 DepthBuffer { get; set; }

        public Buffer3 PositionBuffer { get; set; }
        public Buffer3 NormalBuffer { get; set; }

        public Buffer3 DiffuseBuffer { get; set; }
        public Buffer1 SpecularPowerBuffer { get; set; }
        public Buffer1 SpecularIntensityBuffer { get; set; }

        public Buffer3 BackBuffer { get; set; }
        public Buffer3 ShadowBuffer { get; set; }

        internal GBufferShader GBufferShader { get; set; }
        internal AmbientLightShader AmbientLightShader { get; set; }
        internal PointLightShader PointLightShader { get; set; }
        

        public Engine(int width, int height)
        {
            Width = width;
            Height = height;

            InitializeBuffers();
            InitializeShaders();
        }

        private void InitializeBuffers()
        {
            DepthBuffer = new Buffer1(Width, Height);
            PositionBuffer = new Buffer3(Width, Height);
            NormalBuffer = new Buffer3(Width, Height);
            DiffuseBuffer = new Buffer3(Width, Height);
            SpecularPowerBuffer = new Buffer1(Width, Height);
            SpecularIntensityBuffer = new Buffer1(Width, Height);
            BackBuffer = new Buffer3(Width, Height);
            ShadowBuffer = new Buffer3(Width, Height);
        }

        private void InitializeShaders()
        {
            GBufferShader = new GBufferShader(this);
            AmbientLightShader = new AmbientLightShader(this);
            PointLightShader = new PointLightShader(this);
        }

        public void ClearRenderTargets()
        {
            DepthBuffer.Clear(float.MinValue);
        }

        private void Render()
        {
            ClearRenderTargets();

            // 바닥 렌더링

            // 모델 렌더링
            foreach (var triangle in Model.Triangles)
            {
                GBufferShader.DrawPrimitive(triangle);
            }

            // 화면 전체를 덮는 사각형으로 조명 처리
            AmbientLightShader.DrawPrimitive(null);
            PointLightShader.DrawPrimitive(null);
        }

        public void Render(Model model, Camera camera)
        {
            Model = model;
            Camera = camera;

            Render();
        }        
    }
}
