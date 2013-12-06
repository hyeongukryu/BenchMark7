#pragma once

#include "Vertex.h"
#include "Buffers.h"

namespace BenchMark7
{
	struct Triangle
	{
		Vertex A, B, C;

		Triangle(Vertex a, Vertex b, Vertex c)
			: A(a), B(b), C(c)
		{
		}
	};

	struct Model
	{
		Buffer3 Texture;
		std::vector<Triangle> Triangles;

	private:
		static std::vector<int> ParseVertex(std::wstring data)
		{
			int first = data.find(L"/");
			int second = data.find(L"/", first + 1);

			if (second == std::wstring::npos)
			{
				std::vector<int> result(1, std::stoi(data.substr(0, first)));
				return result;
			}

			std::vector<int> result(3);
			result[0] = std::stoi(data.substr(0, first));
			result[1] = std::stoi(data.substr(first + 1, second - first));
			result[2] = std::stoi(data.substr(second + 1));
			return result;
		}

	public:
		static Model FromString(std::wstring data)
		{
			std::vector<Vector3> positions, normals;
			std::vector<Vector2> textureCoords;

			Model model;

			std::wistringstream stream(std::move(data));
			std::wstring line;
			while (std::getline(stream, line))
			{
				if (line[0] == L'#' || line[0] == L's' || line.length() <= 2)
				{
					continue;
				}

				std::wistringstream lineStream(line);
				std::vector<std::wstring> tokens;
				while (lineStream.eof() == false)
				{
					std::wstring token;
					std::getline(lineStream, token, L' ');
					if (token != L"")
					{
						tokens.push_back(std::move(token));
					}
				}


				if (line[0] == L'v')
				{
					float x = std::stof(tokens[1]);
					float y = std::stof(tokens[2]);
					float z = std::stof(tokens[3]);

					if (line[1] == L'n')
					{
						normals.push_back(Vector3(x, y, z));
					}
					else if (line[1] == L't')
					{
						textureCoords.push_back(Vector2(x, y));
					}
					else
					{
						positions.push_back(Vector3(x, y, -z));
					}
				}
				else if (line[0] == L'f')
				{
					auto i = ParseVertex(tokens[1]);
					auto j = ParseVertex(tokens[2]);
					auto k = ParseVertex(tokens[3]);
				
					if (i.size() == 1)
					{
						auto a = Vertex(positions[i[0] - 1], Vector3(1, 0, 0));
						auto b = Vertex(positions[j[0] - 1], Vector3(0, 1, 0));
						auto c = Vertex(positions[k[0] - 1], Vector3(0, 0, 1));
						model.Triangles.push_back(Triangle(a, b, c));
					}
					else
					{
						auto a = Vertex(positions[i[0] - 1], normals[i[2] - 1], textureCoords[i[1] - 1]);
						auto b = Vertex(positions[j[0] - 1], normals[j[2] - 1], textureCoords[j[1] - 1]);
						auto c = Vertex(positions[k[0] - 1], normals[k[2] - 1], textureCoords[k[1] - 1]);

						a.Color = Vector3(1, 0, 0);
						b.Color = Vector3(0, 1, 0);
						c.Color = Vector3(0, 0, 1);

						model.Triangles.push_back(Triangle(a, b, c));
					}
				}
			}

			return model;
		}
	};
}
