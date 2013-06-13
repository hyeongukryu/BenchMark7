using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchMark7
{
    public class Model
    {
        public Buffer3 Texture { get; set; }
        public List<Triangle> Triangles { get; set; }

        private Model()
        {
            Triangles = new List<Triangle>();
        }

        private static List<int> ParseVertex(string vertexString)
        {
            var tokens = vertexString.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 1)
            {
                return new List<int>(new[] { int.Parse(tokens[0]) });
            }
            return new List<int>(new[] { int.Parse(tokens[0]), int.Parse(tokens[1]), int.Parse(tokens[2]) });
        }

        public static Model FromString(string data)
        {
            List<Vector3> Positions = new List<Vector3>();
            List<Vector3> Normals = new List<Vector3>();
            List<Vector2> TextureCoords = new List<Vector2>();

            Model model = new Model();

            var lines = from line in data.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                        where !line.StartsWith("#") && !line.StartsWith("#") && !line.StartsWith("s")
                        where line.Length > 2
                        select line;

            foreach (var line in lines)
            {
                var tokens = line.Split(new[] { "\t", " " }, StringSplitOptions.RemoveEmptyEntries);

                if (line[0] == 'v')
                {
                    float x = float.Parse(tokens[1]);
                    float y = float.Parse(tokens[2]);
                    float z = float.Parse(tokens[3]);

                    if (line[1] == 'n')
                    {
                        // normal
                        Normals.Add(new Vector3(x, y, z));
                    }
                    else if (line[1] == 't')
                    {
                        // textureCoord
                        TextureCoords.Add(new Vector2(x, y));
                    }
                    else
                    {
                        // position
                        Positions.Add(new Vector3(x, y, z));
                    }
                }
                else if (line[0] == 'f')
                {
                    // triangle
                    var i = ParseVertex(tokens[1]);
                    var j = ParseVertex(tokens[2]);
                    var k = ParseVertex(tokens[3]);

                    if (i.Count == 1)
                    {
                        var a = new Vertex(Positions[i[0] - 1], new Vector3(1, 0, 0));
                        var b = new Vertex(Positions[j[0] - 1], new Vector3(1, 0, 0));
                        var c = new Vertex(Positions[k[0] - 1], new Vector3(1, 0, 0));
                        model.Triangles.Add(new Triangle(a, b, c));
                    }
                    else
                    {
                        var a = new Vertex(Positions[i[0] - 1], Normals[i[2] - 1], TextureCoords[i[1] - 1]);
                        var b = new Vertex(Positions[j[0] - 1], Normals[j[2] - 1], TextureCoords[j[1] - 1]);
                        var c = new Vertex(Positions[k[0] - 1], Normals[k[2] - 1], TextureCoords[k[1] - 1]);
                        model.Triangles.Add(new Triangle(a, b, c));
                    }
                }
            }

            return model;
        }
    }
}
