using System.Diagnostics;

namespace SourceSharp.SP.Public.Bitmap;

public struct Bitmap
{
    private int width;
    private int height;

    private int pixelSize;
    private int stride;
    private bool ownsBuffer;
    private ImageFormat imageFormat;
    private byte[,] bits;

    public Bitmap()
    {
        Reset();
    }

    ~Bitmap()
    {
        Clear();
    }

    public int Width()
    {
        return width;
    }

    public int Height()
    {
        return height;
    }

    public ImageFormat Format()
    {
        return imageFormat;
    }

    public string GetBits()
    {
        return bits;
    }

    public int Stride()
    {
        return stride;
    }

    public bool GetOwnsBuffer()
    {
        return ownsBuffer;
    }

    public void Init(int width, int height, ImageFormat imageFormat, int stride = 0)
    {

    }

    public void SetBuffer(int width, int height, ImageFormat imageFormat, byte[] bits, bool assumeOwnership, int stride = 0)
    {

    }

    public void SetsOwnBuffer(bool ownsBuffer)
    {
        Debug.Assert(bits != null);
        this.ownsBuffer = ownsBuffer;
    }

    public void Clear()
    {

    }

    public bool IsValid()
    {

    }

    public byte GetPixel(int x, int y)
    {
        if (bits == null)
        {
            return 0;
        }

        return bits[x * stride, y * stride];
    }

    public Color GetColor(int x, int y)
    {

    }

    public void SetColor(int x, int y, Color c)
    {

    }

    public void MakeLogicalCopyOf(Bitmap src, bool transferBufferOwnership = false)
    {

    }

    public void Crop(int x0, int y0, int width, int height, Bitmap imgSource = null)
    {

    }

    public void SetPixelData(Bitmap src, int srcX1, int srcY1, int copySizeX, int copySizeY, int destX1, int destY1)
    {

    }

    public void SetPixelData(Bitmap src, int destX1 = 0, int destY1 = 0)
    {

    }

    private void Reset()
    {
        width = 0;
        height = 0;
        imageFormat = ImageFormat.IMAGE_FORMAT_UNKNOWN;
        bits = null;
        pixelSize = 0;
        ownsBuffer = false;
        stride = 0;
    }
}