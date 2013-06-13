using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchMark7
{
    public class Camera
    {
        public Camera()
        {
            World = Matrix.CreateIdentity();
            View = Matrix.CreateIdentity();
            Projection = Matrix.CreateIdentity();
        }

        public Matrix World { get; set; }
        public Matrix View { get; set; }
        public Matrix Projection { get; set; }
    }
}
