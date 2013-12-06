// BenchMark7.NativeRenderer.cpp : 콘솔 응용 프로그램에 대한 진입점을 정의합니다.
//

#include "stdafx.h"
#include "BenchMark7.h"

#include "GBufferShader.h"
#include "AmbientLightShader.h"
#include "PointLightShader.h"

using namespace boost::program_options;
using namespace std;
using namespace BenchMark7;

shared_ptr<Model> ReadModel(wstring objPath)
{
	locale empty_locale = locale::empty();
	typedef codecvt_utf8<wchar_t> converter_type;
	const converter_type* converter = new converter_type;
	const locale utf8_locale = locale(empty_locale, converter);

	wifstream objStream(objPath);
	objStream.imbue(utf8_locale);

	wstring objContent;
	while (objStream.eof() == false)
	{
		wstring input;
		getline(objStream, input);
		objContent += input + L"\n";
	}

	return make_shared<Model>(Model::FromString(objContent));
}

void ReadTexture(wstring texturePath, Buffer3 &texture)
{
	using namespace Gdiplus;

	GdiplusStartupInput gdiplusStartupInput;
	ULONG_PTR gdiplusToken;
	GdiplusStartup(&gdiplusToken, &gdiplusStartupInput, nullptr);

	auto bitmap = shared_ptr<Bitmap>(Bitmap::FromFile(texturePath.c_str()));

	UINT width = bitmap->GetWidth(), height = bitmap->GetHeight();
	Rect rect(0, 0, width, height);

	texture = Buffer3(width, height);

	BitmapData lockedBitmapData;
	bitmap->LockBits(&rect, ImageLockModeRead, PixelFormat24bppRGB, &lockedBitmapData);

	auto pixels = (BYTE*)lockedBitmapData.Scan0;
	auto stride = lockedBitmapData.Stride;

	for (UINT i = 0; i < height; i++)
	{
		for (UINT j = 0; j < width; j++)
		{
			texture.Data[i][j].X = pixels[i * stride + j * 3 + 0] / 255.0f;
			texture.Data[i][j].Y = pixels[i * stride + j * 3 + 1] / 255.0f;
			texture.Data[i][j].Z = pixels[i * stride + j * 3 + 2] / 255.0f;
		}
	}

	bitmap->UnlockBits(&lockedBitmapData);
	bitmap.reset();

	GdiplusShutdown(gdiplusToken);
}

int GetEncoderClsid(const WCHAR* format, CLSID* pClsid)
{
	using namespace Gdiplus;

	UINT  num = 0;          // number of image encoders
	UINT  size = 0;         // size of the image encoder array in bytes

	ImageCodecInfo* pImageCodecInfo = NULL;

	GetImageEncodersSize(&num, &size);
	if(size == 0)
		return -1;  // Failure

	pImageCodecInfo = (ImageCodecInfo*)(malloc(size));
	if(pImageCodecInfo == NULL)
		return -1;  // Failure

	GetImageEncoders(num, size, pImageCodecInfo);

	for(UINT j = 0; j < num; ++j)
	{
		if( wcscmp(pImageCodecInfo[j].MimeType, format) == 0 )
		{
			*pClsid = pImageCodecInfo[j].Clsid;
			free(pImageCodecInfo);
			return j;  // Success
		}    
	}

	free(pImageCodecInfo);
	return -1;  // Failure
}

void WriteTexture(wstring texturePath, Buffer3 &texture)
{
	using namespace Gdiplus;

	GdiplusStartupInput gdiplusStartupInput;
	ULONG_PTR gdiplusToken;
	GdiplusStartup(&gdiplusToken, &gdiplusStartupInput, nullptr);

	UINT width = texture.Width, height = texture.Height;
	Rect rect(0, 0, width, height);

	auto bitmap = make_shared<Bitmap>(width, height, PixelFormat24bppRGB);

	BitmapData lockedBitmapData;
	bitmap->LockBits(&rect, ImageLockModeWrite, PixelFormat24bppRGB, &lockedBitmapData);

	auto pixels = (BYTE*)lockedBitmapData.Scan0;
	auto stride = lockedBitmapData.Stride;

	for (UINT i = 0; i < height; i++)
	{
		for (UINT j = 0; j < width; j++)
		{
			pixels[i * stride + j * 3 + 0] = BitmapClamp(texture.Data[i][j].X);
			pixels[i * stride + j * 3 + 1] = BitmapClamp(texture.Data[i][j].Y);
			pixels[i * stride + j * 3 + 2] = BitmapClamp(texture.Data[i][j].Z);
		}
	}

	bitmap->UnlockBits(&lockedBitmapData);

	CLSID pngClsid;
	GetEncoderClsid(L"image/png", &pngClsid);
	bitmap->Save(texturePath.c_str(), &pngClsid, nullptr);
	bitmap.reset();

	GdiplusShutdown(gdiplusToken);
}

void WriteTexture(wstring texturePath, Buffer1 &texture)
{
	using namespace Gdiplus;

	GdiplusStartupInput gdiplusStartupInput;
	ULONG_PTR gdiplusToken;
	GdiplusStartup(&gdiplusToken, &gdiplusStartupInput, nullptr);

	UINT width = texture.Width, height = texture.Height;
	Rect rect(0, 0, width, height);

	auto bitmap = make_shared<Bitmap>(width, height, PixelFormat24bppRGB);

	BitmapData lockedBitmapData;
	bitmap->LockBits(&rect, ImageLockModeWrite, PixelFormat24bppRGB, &lockedBitmapData);

	auto pixels = (BYTE*)lockedBitmapData.Scan0;
	auto stride = lockedBitmapData.Stride;

	for (UINT i = 0; i < height; i++)
	{
		for (UINT j = 0; j < width; j++)
		{
			pixels[i * stride + j * 3 + 0] = BitmapClamp(texture.Data[i][j]);
			pixels[i * stride + j * 3 + 1] = BitmapClamp(texture.Data[i][j]);
			pixels[i * stride + j * 3 + 2] = BitmapClamp(texture.Data[i][j]);
		}
	}

	bitmap->UnlockBits(&lockedBitmapData);

	CLSID pngClsid;
	GetEncoderClsid(L"image/png", &pngClsid);
	bitmap->Save(texturePath.c_str(), &pngClsid, nullptr);
	bitmap.reset();

	GdiplusShutdown(gdiplusToken);
}

int _tmain(int argc, _TCHAR* argv[])
{
	options_description desc;
	desc.add_options()
		("objPath", wvalue<wstring>() ,"objPath")
		("texturePath", wvalue<wstring>(), "texturePath")
		("width", value<int>(), "width")
		("height", value<int>(), "height")
		;

	variables_map vm;
	store(parse_command_line(argc, argv, desc), vm);

	auto objPath = vm["objPath"].as<wstring>();
	shared_ptr<Model> model = ReadModel(objPath);

	auto texturePath = vm["texturePath"].as<wstring>();
	Buffer3 &texture = model->Texture;
	ReadTexture(texturePath, texture);

	int width = vm["width"].as<int>(),
		height = vm["height"].as<int>();

	shared_ptr<Engine> engine = make_shared<Engine>(width, height);

	Camera camera;
	camera.World = Matrix::CreateScale(600);
	camera.View = Matrix::CreateLook(
		Vector3(0, 520, 0),
		Vector3(400, 520, -1024),
		Vector3(1, 0, 0)
		);
	camera.Projection = Matrix::CreateOrthogonalProjection(
		1280, 720, 1024, 0
		);

	auto gBufferShader = make_shared<GBufferShader>(engine);
	gBufferShader->SpecularIntensity = 15.0f;
	gBufferShader->SpecularPower = 15.0f;

	auto pointLightShader = make_shared<PointLightShader>(engine);
	pointLightShader->Intensity = 0.4f;
	pointLightShader->LightPosition = Vector4(-100, 50, 100, 1);

	auto ambientLightShader = make_shared<AmbientLightShader>(engine);
	ambientLightShader->Intensity = 0.1f;

	engine->ModelShaders.push_back(gBufferShader);
	engine->ScreenShaders.push_back(ambientLightShader);
	engine->ScreenShaders.push_back(pointLightShader);

	engine->Debug = [&]()
	{
		WriteTexture(L"albedo.png", engine->AlbedoBuffer);
		WriteTexture(L"position.png", engine->PositionBuffer);
		WriteTexture(L"depth.png", engine->DepthBuffer);
		WriteTexture(L"normal.png", engine->NormalBuffer);
		WriteTexture(L"specularIntensity.png", engine->SpecularIntensityBuffer);
		WriteTexture(L"specularPower.png", engine->SpecularPowerBuffer);
	};

	engine->Render(model, camera);


	WriteTexture(L"result.png", engine->BackBuffer);

	return 0;
}
