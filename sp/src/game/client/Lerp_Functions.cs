using System;

using SourceSharp.SP.Mathlib;

namespace SourceSharp.SP.Game.Client;

public static class Lerp_Functions
{
    public static dynamic LoopingLerp(float percent, dynamic from, ref dynamic to)
    {
        dynamic s = to * percent + from * (1.0f - percent);
        return s;
    }

    public static float LoopingLerp(float percent, float from, float to)
    {
        if (MathF.Abs(to - from) >= 0.5f)
        {
            if (from < to)
            {
                from += 1.0f;
            }
            else
            {
                to += 1.0f;
            }
        }

        float s = to * percent + from * (1.0f - percent);

        s = s - (int)s;

        if (s < 0.0f)
        {
            s = s + 1.0f;
        }

        return s;
    }

    public static dynamic Lerp_Hermite(float t, dynamic p0, dynamic p1, dynamic p2)
    {
        dynamic d1 = p1 - p0;
        dynamic d2 = p2 - p1;

        dynamic output;
        float tSqr = t * t;

        output = p1 * (6 * tSqr - 6 * t);
        output += p2 * (-6 * tSqr + 6 * t);
        output += d1 * (3 * tSqr - 4 * t + 1);
        output += d2 * (3 * tSqr - 2 * t);

        return output;
    }

    public static void Lerp_Clamp(dynamic val)
    {
    }

    public static void Lerp_Clamp(CRangeCheckedVar<dynamic> val, int maxValue, int minValue)
    {
        val.maxValue = maxValue;
        val.minValue = minValue;
        val.Clamp();
    }

    public static QAngle Lerp_Hermite(float t, QAngle p0, QAngle p1, QAngle p2)
    {

    }
}