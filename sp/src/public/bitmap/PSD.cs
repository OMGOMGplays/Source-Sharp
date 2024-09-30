using SourceSharp.SP.Public.Tier1;

namespace SourceSharp.SP.Public.Bitmap;

public class PSDImageResources
{
    protected uint numBytes;
    protected byte[] buffer;

    public enum Resource
    {
        eResFileInfo = 0x404
    }

    public struct ResElement
    {
        public Resource type;
        public ushort numBytes;
        public byte[] data;
    }

    public PSDImageResources(uint numBytes, byte[] buffer)
    {
        this.numBytes = numBytes;
        this.buffer = buffer;
    }

    public ResElement FindElement(Resource type)
    {

    }
}

public class PSDResFileInfo
{
    protected PSDImageResources.ResElement res;

    public enum ResFileInfo
    {
        eTitle = 0x05,
        eAuthor = 0x50,
        eAuthorTitle = 0x55,
        eDescription = 0x78,
        eDescriptionWriter = 0x7A,
        eKeywords = 0x19,
        eCopyrightNotice = 0x74
    }

    public struct ResFileInfoElement
    {
        public ResFileInfo type;
        public ushort numBytes;
        public byte[] data;
    }

    public PSDResFileInfo(PSDImageResources.ResElement res)
    {
        this.res = res;
    }

    public ResFileInfoElement FindElement(ResFileInfo type)
    {

    }
}

public static class PSD
{
    public static bool IsPSDFile(string fileName, string pathID)
    {

    }

    public static bool IsPSDFile(CUtlBuffer buf)
    {

    }

    public static bool PSDGetInfo(string fileName, string pathID, int width, int height, ImageFormat imageFormat, float sourceGamma)
    {

    }

    public static bool PSDGetInfo(CUtlBuffer buf, int width, int height, ImageFormat imageFormat, float sourceGamma)
    {

    }

    public static PSDImageResources PSDGetImageResources(CUtlBuffer buf)
    {

    }

    public static bool PSDReadFileRGBA8888(CUtlBuffer buf, out Bitmap bitmap)
    {

    }

    public static bool PSDReadFileRGBA8888(string fileName, string pathID, out Bitmap bitmap)
    {

    }
}