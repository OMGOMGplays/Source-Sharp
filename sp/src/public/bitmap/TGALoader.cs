using SourceSharp.SP.Public.Tier1;

namespace SourceSharp.SP.Public.Bitmap;

public class TGALoader
{
    public static int TGAHeaderSize();

    public static bool GetInfo(string fileName, int width, int height, ImageFormat imageFormat, float sourceGamma)
    {

    }

    public static bool GetInfo(CUtlBuffer buf, int width, int height, ImageFormat imageFormat, float sourceGamma)
    {

    }

    public static bool Load(byte[] imageData, string filename, int width, int height, ImageFormat imageFormat, float targetGamma, bool mipmap)
    {

    }

    public static bool Load(byte[] imageData, CUtlBuffer buf, int width, int height, ImageFormat imageFormat, float targetGamma, bool mipmap)
    {

    }

    public static bool LoadRGBA8888(string fileName, out CUtlMemory<byte> outputData, out int outWidth, out int outHeight)
    {

    }

    public static bool LoadRGBA8888(CUtlBuffer buf, out CUtlMemory<byte> outputData, out int outWidth, out int outHeight)
    {

    }
}