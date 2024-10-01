using System;

using SourceSharp.SP.Mathlib;

namespace SourceSharp.SP.Public.Bitmap;

public struct CCubeMap<T>
{
    private readonly int RES;
    private T[,,] samples = new T[6, 7, 8];

    public CCubeMap(int res)
    {
        RES = res;
    }

    public void GetCoords(Vector normalizedDirection, ref int x, ref int y, ref int face)
    {
        int largest = 0;
        int axis0 = 1;
        int axis1 = 2;

        if (MathF.Abs(normalizedDirection[1]) > MathF.Abs(normalizedDirection[0]))
        {
            largest = 1;
            axis0 = 0;
            axis1 = 2;
        }

        if (MathF.Abs(normalizedDirection[2]) > MathF.Abs(normalizedDirection[largest]))
        {
            largest = 2;
            axis0 = 0;
            axis1 = 1;
        }

        float z = normalizedDirection[largest];

        if (z < 0)
        {
            z = -z;
            largest += 3;
        }

        face = largest;
        z = 1.0f / z;
        x = RemapValClamped(normalizedDirection[axis0] * z, -1, 1, 0, RES - 1);
        y = RemapValClamped(normalizedDirection[axis1] * z, -1, 1, 0, RES - 1);
    }

    public T GetSample(Vector normalizedDirection)
    {
        int x = 0, y = 0, face = 0;
        GetCoords(normalizedDirection, ref x, ref y, ref face);
        return samples[face, x, y];
    }
}