using BenchMark7.Renderer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchMark7.Driver
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\HyeongUk\Documents\GitHub\BenchMark7\models\mark7\Mark 7.obj";
            string content = File.ReadAllText(path);

            Engine engine = new Engine(800, 600);
            Model model = Model.FromString(content);
            engine.Render(model, null);
        }
    }
}
