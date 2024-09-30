using System.IO;

namespace SourceSharp.SP.Public.Bitmap;

public enum NormalDecodeMode
{
    NORMAL_DECODE_NONE = 0,
    NORMAL_DECODE_ATI2N = 1,
    NORMAL_DECODE_ATI2N_ALPHA = 2
}

public enum ImageFormat
{
    IMAGE_FORMAT_UNKNOWN = -1,
    IMAGE_FORMAT_RGBA8888 = 0,
    IMAGE_FORMAT_ABGR8888,
    IMAGE_FORMAT_RGB888,
    IMAGE_FORMAT_BGR888,
    IMAGE_FORMAT_RGB565,
    IMAGE_FORMAT_I8,
    IMAGE_FORMAT_IA88,
    IMAGE_FORMAT_P8,
    IMAGE_FORMAT_A8,
    IMAGE_FORMAT_RGB888_BLUESCREEN,
    IMAGE_FORMAT_BGR888_BLUESCREEN,
    IMAGE_FORMAT_ARGB8888,
    IMAGE_FORMAT_BGRA8888,
    IMAGE_FORMAT_DXT1,
    IMAGE_FORMAT_DXT3,
    IMAGE_FORMAT_DXT5,
    IMAGE_FORMAT_BGRX8888,
    IMAGE_FORMAT_BGR565,
    IMAGE_FORMAT_BGRX5551,
    IMAGE_FORMAT_BGRA4444,
    IMAGE_FORMAT_DXT1_ONEBITALPHA,
    IMAGE_FORMAT_BGRA5551,
    IMAGE_FORMAT_UV88,
    IMAGE_FORMAT_UVWQ8888,
    IMAGE_FORMAT_RGBA16161616F,
    IMAGE_FORMAT_RGBA16161616,
    IMAGE_FORMAT_UVLX8888,
    IMAGE_FORMAT_R32F,
    IMAGE_FORMAT_RGB323232F,
    IMAGE_FORMAT_RGBA32323232F,

    IMAGE_FORMAT_NV_DST16,
    IMAGE_FORMAT_NV_DST24,
    IMAGE_FORMAT_NV_INTZ,
    IMAGE_FORMAT_NV_RAWZ,
    IMAGE_FORMAT_ATI_DST16,
    IMAGE_FORMAT_ATI_DST24,
    IMAGE_FORMAT_NV_NULL,

    IMAGE_FORMAT_ATI2N,
    IMAGE_FORMAT_ATI1N,

#if X360
	IMAGE_FORMAT_X360_DST16,
	IMAGE_FORMAT_X360_DST24,
	IMAGE_FORMAT_X360_DST24F,

	IMAGE_FORMAT_LINEAR_BGRX8888,
	IMAGE_FORMAT_LINEAR_RGBA8888,
	IMAGE_FORMAT_LINEAR_ABGR8888,
	IMAGE_FORMAT_LINEAR_ARGB8888,
	IMAGE_FORMAT_LINEAR_BGRA8888,
	IMAGE_FORMAT_LINEAR_RGB888,
	IMAGE_FORMAT_LINEAR_BGR888,
	IMAGE_FORMAT_LINEAR_BGRX5551,
	IMAGE_FORMAT_LINEAR_I8,
	IMAGE_FORMAT_LINEAR_RGBA16161616,

	IMAGE_FORMAT_LE_BGRX8888,
	IMAGE_FORMAT_LE_BGRA8888,
#endif // X360

    NUM_IMAGE_FORMATS
}

public enum D3DFORMAT
{
#if POSIX || DX_TO_GL_ABSTRACTION
    D3DFMT_INDEX16,
    D3DFMT_D16,
    D3DFMT_D24S8,
    D3DFMT_A8R8G8B8,
    D3DFMT_A4R4G4B4,
    D3DFMT_X8R8G8B8,
    D3DFMT_R5G6R5,
    D3DFMT_X1R5G5B5,
    D3DFMT_A1R5G5B5,
    D3DFMT_L8,
    D3DFMT_A8L8,
    D3DFMT_A,
    D3DFMT_DXT1,
    D3DFMT_DXT3,
    D3DFMT_DXT5,
    D3DFMT_V8U8,
    D3DFMT_Q8W8V8U8,
    D3DFMT_X8L8V8U8,
    D3DFMT_A16B16G16R16F,
    D3DFMT_A16B16G16R16,
    D3DFMT_R32F,
    D3DFMT_A32B32G32R32F,
    D3DFMT_R8G8B8,
    D3DFMT_D24X4S4,
    D3DFMT_A8,
    D3DFMT_R5G6B5,
    D3DFMT_D15S1,
    D3DFMT_D24X8,
    D3DFMT_VERTEXDATA,
    D3DFMT_INDEX32,

    D3DFMT_NV_INTZ = 0x5a544e49,
    D3DFMT_NV_RAWZ = 0x5a574152,

    D3DFMT_NV_NULL = 0x4c4c554e,

    D3DFMT_ATI_D16 = 0x36314644,
    D3DFMT_ATI_D24S8 = 0x34324644,

    D3DFMT_ATI_2N = 0x32495441,
    D3DFMT_ATI_1N = 0x31495441,

    D3DFMT_UNKNOWN
}
#else
}
#endif // POSIX || DX_TO_GL_ABSTRACTION

public struct BGRA8888
{
    public byte b;
    public byte g;
    public byte r;
    public byte a;
}

public struct RGBA8888
{
    public byte r;
    public byte g;
    public byte b;
    public byte a;
}

public struct RGB888
{
    public byte r;
    public byte g;
    public byte b;

    public static bool operator ==(RGB888 lhs, RGB888 rhs)
    {
        return lhs.r == rhs.r && lhs.g == rhs.g && lhs.b == rhs.b;
    }

    public static bool operator !=(RGB888 lhs, RGB888 rhs)
    {

        return !(lhs == rhs);
    }
}

public struct BGR888
{
    public byte b;
    public byte g;
    public byte r;
}

public struct BGR565
{
    public ushort b;
    public ushort g;
    public ushort r;

    public BGR565 Set(int red, int green, int blue)
    {
        r = (ushort)(red >> 3);
        g = (ushort)(green >> 2);
        b = (ushort)(blue >> 3);

        return this;
    }
}

public struct BGRA5551
{
    public ushort b;
    public ushort g;
    public ushort r;
    public ushort a;
}

public struct BGRA4444
{
    public ushort b;
    public ushort g;
    public ushort r;
    public ushort a;
}

public struct RGBX5551
{
    public ushort r;
    public ushort g;
    public ushort b;
    public ushort x;
}

public struct ImageFormatInfo
{
    public string name;
    public int numBytes;
    public int numRedBits;
    public int numGreenBits;
    public int numBlueBits;
    public int numAlphaBits;
    public bool isCompressed;
}

public static class ImageLoader
{
    public const float ARTWORK_GAMMA = 2.2f;
    public const int IMAGE_MAX_DIM = 2048;

    public static bool GetInfo(string fileName, int width, int height, ImageFormat imageFormat, float sourceGamma)
    {

    }

    public static int GetMemRequired(int width, int height, int depth, ImageFormat imageFormat, bool mipmap)
    {

    }

    public static int GetMipMapLevelByteOffset(int width, int height, ImageFormat imageFormat, int skipMipLevels)
    {

    }

    public static void GetMipMapLevelDimensions(int width, int height, int skipMipLevels)
    {

    }

    public static int GetNumMipMapLevels(int width, int height, int depth = 1)
    {

    }

    public static bool Load(byte[] imageData, string fileName, int width, int height, ImageFormat imageFormat, float targetGamma, bool mipmap)
    {

    }

    public static bool Load(byte[] imageData, File fp, int width, int height, ImageFormat imageFormat, float targetGamma, bool mipmap)
    {

    }

    public static bool ConvertImageFormat(byte[] src, ImageFormat srcImageFormat, out byte[] dst, out ImageFormat dstImageFormat, int width, int height, int srcStride = 0, int dstStride = 0)
    {

    }

    public static void PreConvertSwapImageData(byte[] imageData, int imageSize, ImageFormat imageFormat, int width = 0, int stride = 0)
    {

    }

    public static void PostConvertSwapImageData(byte[] imageData, int imageSize, ImageFormat imageFormat, int width = 0, int stride = 0)
    {

    }

    public static void ByteSwapImageData(byte[] imageData, int imageSize, ImageFormat imageFormat, int width = 0, int stride = 0)
    {

    }

    public static bool IsFormatValidForConversion(ImageFormat fmt)
    {

    }

    public static ImageFormat D3DFormatToImageFormat(D3DFORMAT format)
    {

    }

    public static D3DFORMAT ImageFormatToD3DFormat(ImageFormat format)
    {

    }

    public enum ResampleRGBA8888Flags
    {
        RESAMPLE_NORMALMAP = 0x1,
        RESAMPLE_ALPHATEST = 0x2,
        RESAMPLE_NICE_FILTER = 0x4,
        RESAMPLE_CLAMPS = 0x8,
        RESAMPLE_CLAMPT = 0x10,
        RESAMPLE_CLAMPU = 0x20
    }

    public struct ResampleInfo
    {
        public string src;
        public string dst;

        public int srcWidth;
        public int srcHeight;
        public int srcDepth;

        public int dstWidth;
        public int dstHeight;
        public int dstDepth;

        public float srcGamma;
        public float dstGamma;

        public float[] colorScale = new float[4];
        public float[] colorGoal = new float[4];

        public float alphaThreshhold;
        public float alphaHiFreqThreshhold;

        public int flags;

        public ResampleInfo()
        {
            flags = 0;
            alphaThreshhold = 0.4f;
            alphaHiFreqThreshhold = 0.4f;
            srcDepth = 1;
            dstDepth = 1;

            colorScale[0] = 1.0f; colorScale[1] = 1.0f; colorScale[2] = 1.0f; colorScale[3] = 1.0f;
            colorGoal[0] = 0.0f; colorGoal[1] = 0.0f; colorGoal[2] = 0.0f; colorGoal[3] = 0.0f;
        }
    }

    public static bool ResampleRGBA8888(ResampleInfo info)
    {

    }

    public static bool ResampleRGBA16161616(ResampleInfo info)
    {

    }

    public static bool ResampleRGB323232F(ResampleInfo info)
    {

    }

    public static void ConvertNormalMapRGBA8888ToDUDVMapUVLX8888(byte[] src, int width, int height, out byte[] dst)
    {

    }

    public static void ConvertNormalMapRGBA8888ToDUDVMapUVWQ8888(byte[] src, int width, int height, out byte[] dst)
    {

    }

    public static void ConvertNormalMapRGBA8888ToDUDVMapUV88(byte[] src, int width, int height, out byte[] dst)
    {

    }

    public static void ConvertIA88ImageToNormalMapRGBA8888(byte[] src, int width, int height, out byte[] dst, float bumpScale)
    {

    }

    public static void NormalizeNormalMapRGBA8888(byte[] src, int numTexels)
    {

    }

    public static void GammaCorrectRGBA8888(byte[] src, out byte[] dst, int width, int height, int depth, float srcGamma, out float dstGamma)
    {

    }

    public static void ConstructGammaTable(byte[] table, float srcGamma, out float dstGamma)
    {

    }

    public static void GammaCorrectRGBA8888(byte[] src, out byte[] dst, int width, int height, int depth, byte[] gammaTable)
    {

    }

    public static void GenerateMipMapLevels(byte[] src, out byte[] dst, int width, int height, int depth, ImageFormat imageFormat, float srcGamma, out float dstGamma, int numLevels = 0)
    {

    }

    public static bool RotateImageLeft(byte[] src, out byte[] dst, int widthHeight, ImageFormat imageFormat)
    {

    }

    public static bool RotateImage180(byte[] src, out byte[] dst, int widthHeight, ImageFormat imageFormat)
    {

    }

    public static bool FlipImageVertically(nint src, out nint dst, int width, int height, ImageFormat imageFormat, int dstStride = 0)
    {

    }

    public static bool FlipImageHorizontally(nint src, out nint dst, int width, int height, ImageFormat imageFormat, int dstStride = 0)
    {

    }

    public static bool SwapAxes(byte[] src, int widthHeight, ImageFormat imageFormat)
    {

    }

    public static ImageFormatInfo ImageFormatInfo(ImageFormat fmt)
    {

    }

    public static string GetName(ImageFormat fmt)
    {
        return ImageFormatInfo(fmt).name;
    }

    public static int SizeInBytes(ImageFormat fmt)
    {
        return ImageFormatInfo(fmt).numBytes;
    }

    public static bool IsTransparent(ImageFormat fmt)
    {
        return ImageFormatInfo(fmt).numAlphaBits > 0;
    }

    public static bool IsCompressed(ImageFormat fmt)
    {
        return ImageFormatInfo(fmt).isCompressed;
    }

    public static bool HasChannelLargerThan8Bits(ImageFormat fmt)
    {
        ImageFormatInfo info = ImageFormatInfo(fmt);

        return info.numRedBits > 8 || info.numGreenBits > 8 || info.numBlueBits > 8 || info.numAlphaBits > 8;
    }
}