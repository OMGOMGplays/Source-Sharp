using System;
using System.Diagnostics;

using SourceSharp.SP.Public.Mathlib;

namespace SourceSharp.SP.Public.Bitmap;

public struct PixRGBAF
{
    public float red;
    public float green;
    public float blue;
    public float alpha;
}

public struct PixRGBA8
{
    public byte red;
    public byte green;
    public byte blue;
    public byte alpha;
}

public class FloatBitMap
{
    public const int SPFLAGS_MAXGRADIENT = 1;

    public const int SSBUMP_OPTION_NONDIRECTIONAL = 1;
    public const int SSBUMP_MOD2X_DETAIL_TEXTURE = 2;

    public int width, height;
    public float[] rgbaData;

    public FloatBitMap()
    {
        width = height = 0;
        rgbaData = null;
    }

    public FloatBitMap(int width, int height)
    {

    }

    public FloatBitMap(string filename)
    {

    }

    public FloatBitMap(FloatBitMap orig)
    {

    }

    public bool WriteTGAFile(string filename)
    {

    }

    public bool LoadFromPFM(string filename)
    {

    }

    public bool WritePFM(string filename)
    {

    }

    public void InitializeWithRandomPixelsFromAnotherFloatBM(FloatBitMap other)
    {

    }

    public float Pixel(int x, int y, int comp)
    {
        Debug.Assert(x >= 0 && x < width);
        Debug.Assert(y >= 0 && y < height);

        return rgbaData[4 * (x + width * y) + comp];
    }

    public float PixelWrapped(int x, int y, int comp)
    {
        if (x < 0)
        {
            x += width;
        }
        else
        {
            if (x >= width)
            {
                x -= width;
            }
        }

        if (y < 0)
        {
            y += height;
        }
        else
        {
            if (y >= height)
            {
                y -= height;
            }
        }

        return rgbaData[4 * (x + width * y) + comp];
    }

    public float PixelClamped(int x, int y, int comp)
    {
        x = Math.Clamp(x, 0, width - 1);
        y = Math.Clamp(y, 0, height - 1);

        return rgbaData[4 * (x + width * y) + comp];
    }

    public float Alpha(int x, int y)
    {
        Debug.Assert(x >= 0 && x < width);
        Debug.Assert(y >= 0 && y < height);

        return rgbaData[3 + 4 * (x + width * y)];
    }

    public float InterpolatedPixel(float x, float y, int comp)
    {

    }

    public PixRGBAF PixelRGBAF(int x, int y)
    {
        Debug.Assert(x >= 0 && x < width);
        Debug.Assert(y >= 0 && y < height);

        PixRGBAF retPix;
        int rgbOffset = 4 * (x + width * y);
        retPix.red = rgbaData[rgbOffset + 0];
        retPix.green = rgbaData[rgbOffset + 1];
        retPix.blue = rgbaData[rgbOffset + 2];
        retPix.alpha = rgbaData[rgbOffset + 3];

        return retPix;
    }

    public void WritePixelRGBAF(int x, int y, PixRGBAF value)
    {
        Debug.Assert(x >= 0 && x < width);
        Debug.Assert(y >= 0 && y < height);

        int rgbOffset = 4 * (x + width * y);
        rgbaData[rgbOffset + 0] = value.red;
        rgbaData[rgbOffset + 1] = value.green;
        rgbaData[rgbOffset + 2] = value.blue;
        rgbaData[rgbOffset + 3] = value.alpha;
    }

    public void WritePixel(int x, int y, int comp, float value)
    {
        Debug.Assert(x >= 0 && x < width);
        Debug.Assert(y >= 0 && y < height);
        rgbaData[4 * (x + width * y) + comp] = value;
    }

    public void SmartPaste(FloatBitMap brush, int xofs, int yofs, uint flags)
    {

    }

    public void MakeTileable()
    {

    }

    public void ReSize(int newXSize, int newYSize)
    {
        
    }

    public void GetAlphaBounds(int minx, int miny, int maxx, int maxy)
    {

    }

    public void Poisson(FloatBitMap[] deltas, int iters, uint flags)
    {

    }

    public FloatBitMap QuarterSize()
    {

    }

    public FloatBitMap QuarterSizeBlocky()
    {
        
    }

    public FloatBitMap QuarterSizeWithGaussian()
    {

    }

    public void RaiseToPower(float pow)
    {

    }

    public void ScaleGradients()
    {

    }

    public void Logize()
    {

    }

    public void UnLogize()
    {

    }

    public void Compress8Bits(float overbright)
    {

    }

    public void Uncompress(float overbright)
    {

    }

    public Vector AverageColor()
    {

    }

    public float BrightestColor()
    {

    }

    public void Clear(float r, float g, float b, float alpha)
    {

    }

    public void ScaleRGB(float scale_factor)
    {

    }

    public void ComputeVertexPositionsAndNormals(float heightScale, out Vector[] posOut, out Vector[] normalOut)
    {

    }

    public FloatBitMap ComputeSelfShadowedBumpmapFromHeightInAlphaChannel(float bump_scale, int rays_to_trace_per_pixel = 100, uint optionFlags = 0)
    {

    }

    public FloatBitMap ComputeBumpmapFromHeightInAlphaChannel(float bump_scale)
    {

    }

    public void TileableBilateralFilter(int radius_in_pixels, float edge_threshold_value)
    {

    }

    ~FloatBitMap()
    {

    }

    public void AllocateRGB(int w, int h)
    {
        if (rgbaData != null)
        {
            rgbaData = null;
        }

        rgbaData = new float[w * h * 4];
        width = w;
        height = h;
    }

    public static PixRGBAF PixRGBA8_to_F(PixRGBA8 x)
    {
        PixRGBAF f;
        f.red = x.red / 255.0f;
        f.green = x.green / 255.0f;
        f.blue = x.blue / 255.0f;
        f.alpha = x.alpha / 255.0f;
        return f;
    }

    public static PixRGBA8 PixRGBAF_to_8(PixRGBAF f)
    {
        PixRGBA8 x;
        x.red = (byte)Math.Max(0, Math.Min(255, 255.0f * f.red));
        x.green = (byte)Math.Max(0, Math.Min(255, 255.0f * f.green));
        x.blue = (byte)Math.Max(0, Math.Min(255, 255.0f * f.blue));
        x.alpha = (byte)Math.Max(0, Math.Min(255, 255.0f * f.alpha));
        return x;
    }

    public static float FLerp(float f1, float f2, float t)
    {
        return f1 + (f2 - f1) * t;
    }
}

public class FloatCubeMap
{
    public FloatBitMap[] face_maps = new FloatBitMap[6];

    public FloatCubeMap(int xfsize, int yfsize)
    {
        for (int f = 0; f < 6; f++)
        {
            face_maps[f].AllocateRGB(xfsize, yfsize);
        }
    }

    public FloatCubeMap(string basename)
    {

    }

    public void WritePFMs(string basename)
    {

    }

    public Vector AverageColor()
    {
        Vector ret = new Vector(0, 0, 0);
        int faces = 0;

        for (int f = 0; f < 6; f++)
        {
            if (face_maps[f].rgbaData != null)
            {
                faces++;
                ret += face_maps[f].AverageColor();
            }

            if (faces != 0)
            {
                ret *= (1.0f / faces);
            }

            return ret;
        }
    }

    public float BrightestColor()
    {
        float ret = 0.0f;
        int faces = 0;

        for (int f = 0; f < 6; f++)
        {
            if (face_maps[f].rgbaData != null)
            {
                faces++;
                ret = Math.Max(ret, face_maps[f].BrightestColor());
            }

            return ret;
        }
    }

    public void Resample(ref FloatCubeMap dest, float phongExponent)
    {

    }

    public Vector PixelDirection(int face, int x, int y)
    {

    }

    public Vector FaceNormal(int faceNumber)
    {

    }
}

public enum ImagePyramidMode
{
    PYRAMID_MODE_GAUSSIAN
}

public class FloatImagePyramid
{
    public const int MAX_IMAGE_PYRAMID_LEVELS = 16;

    public int numLevels;
    public FloatBitMap[] levels = new FloatBitMap[MAX_IMAGE_PYRAMID_LEVELS];

    public FloatImagePyramid()
    {
        numLevels = 0;
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i] = 0;
        }
    }

    public FloatImagePyramid(FloatBitMap src, ImagePyramidMode mode)
    {

    }

    public float Pixel(int x, int y, int component, int level)
    {

    }

    public FloatBitMap Level(int lvl)
    {
        Debug.Assert(lvl < numLevels);
        Debug.Assert(lvl < levels.Length);

        return levels[lvl];
    }

    public void ReconstructLowerResolutionValues(int starting_level)
    {

    }

    ~FloatImagePyramid()
    {

    }

    public void WriteTGAs(string basename)
    {

    }
}