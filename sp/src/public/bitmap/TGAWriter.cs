using SourceSharp.SP.Public.Tier1;

namespace SourceSharp.SP.Public.Bitmap;

public class TGAWriter
{
    public static bool WriteToBuffer(byte[] imageData, CUtlBuffer buf, int width, int height, ImageFormat srcFormat, out ImageFormat dstFormat)
    {

    }

    public static bool WriteTGAFile(string fileName, int width, int height, ImageFormat srcFormat, byte[] srcData, int stride)
    {

    }

    public static bool WriteDummyFileNoAlloc(string fileName, int width, int height, out ImageFormat dstFormat)
    {

    }

    public static bool WriteRectNoAlloc(byte[] imageData, string fileName, int xOrigin, int yOrigin, int width, int height, int stride, ImageFormat srcFormat)
    {

    }
}