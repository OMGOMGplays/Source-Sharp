using System;
using System.Diagnostics;

namespace SourceSharp.SP.Public.Mathlib;

public class Vector2D
{
    public static Vector2D vec2_origin = new Vector2D(0, 0);
    public static Vector2D vec2_invalid = new Vector2D(float.MaxValue, float.MaxValue);

    public float x, y;

    public Vector2D()
    {
#if DEBUG
        x = y = float.NaN;
#endif // DEBUG
    }

    public Vector2D(float x, float y)
    {
        this.x = x;
        this.y = y;
        Debug.Assert(IsValid());
    }

    public Vector2D(float[] array)
    {
        Debug.Assert(array != null);
        x = array[0];
        y = array[1];
        Debug.Assert(IsValid());
    }

    public Vector2D(Vector2D other)
    {
        Debug.Assert(other.IsValid());
        x = other.x;
        y = other.y;
    }

    public void Init(float x, float y)
    {
        this.x = x;
        this.y = y;
        Debug.Assert(IsValid());
    }

    public bool IsValid()
    {
        return BaseTypes.IsFinite(x) && BaseTypes.IsFinite(y);
    }

    public float this[int i]
    {
        get
        {
            switch (i)
            {
                case Vector.X_INDEX:
                    return x;

                case Vector.Y_INDEX:
                    return y;

                default:
                    throw new IndexOutOfRangeException();
            }
        }

        set
        {
            switch (i)
            {
                case Vector.X_INDEX:
                    x = value;
                    break;

                case Vector.Y_INDEX:
                    y = value;
                    break;

                default:
                    throw new IndexOutOfRangeException();
            }
        }
    }

    public float[] Base()
    {
        return (float[])this;
    }

    public void Random(float minVal, float maxVal)
    {
        Random rand = new();

        x = minVal + (float)rand.Next() / VALVE_RAND_MAX * (maxVal - minVal);
        y = minVal + (float)rand.Next() / VALVE_RAND_MAX * (maxVal - minVal);
    }

    public static bool operator ==(Vector2D lhs, Vector2D rhs)
    {
        Debug.Assert(lhs.IsValid() && rhs.IsValid());

        return lhs.x == rhs.x && lhs.y == rhs.y;
    }

    public static bool operator !=(Vector2D lhs, Vector2D rhs)
    {
        return !(lhs == rhs);
    }

    public static Vector2D operator -(Vector2D vector)
    {
        return new Vector2D(-vector.x, -vector.y);
    }

    public static Vector2D operator +(Vector2D lhs, Vector2D rhs)
    {
        Debug.Assert(lhs.IsValid() && rhs.IsValid());

        lhs.x += rhs.x;
        lhs.y += rhs.y;

        return lhs;
    }

    public static Vector2D operator -(Vector2D lhs, Vector2D rhs)
    {
        Debug.Assert(lhs.IsValid() && rhs.IsValid());

        lhs.x -= rhs.x;
        lhs.y -= rhs.y;

        return lhs;
    }

    public static Vector2D operator *(Vector2D lhs, Vector2D rhs)
    {
        lhs.x *= rhs.x;
        lhs.y *= rhs.y;

        Debug.Assert(lhs.IsValid());
        return lhs;
    }

    public static Vector2D operator *(Vector2D lhs, float rhs)
    {
        lhs.x *= rhs;
        lhs.y *= rhs;

        Debug.Assert(lhs.IsValid());
        return lhs;
    }

    public static Vector2D operator /(Vector2D lhs, Vector2D rhs)
    {
        Debug.Assert(rhs.x != 0.0f && rhs.y != 0.0f);
        lhs.x /= rhs.x;
        lhs.y /= rhs.y;

        Debug.Assert(lhs.IsValid());
        return lhs;
    }

    public static Vector2D operator /(Vector2D lhs, float rhs)
    {
        Debug.Assert(rhs != 0.0f);
        float oo = 1.0f / rhs;

        lhs.x /= oo;
        lhs.y /= oo;

        Debug.Assert(lhs.IsValid());
        return lhs;
    }

    public void Negate()
    {
        Debug.Assert(IsValid());
        x = -x;
        y = -y;
    }

    public float Length()
    {
        return Vector2DLength(this);
    }

    public float LengthSqr()
    {
        Debug.Assert(IsValid());
        return x * x + y * y;
    }

    public bool IsZero(float tolerance = 0.01f)
    {
        return x > -tolerance && x < tolerance &&
               y > -tolerance && y < tolerance;
    }

    public float NormalizeInPlace()
    {
        return Vector2DNormalize(this);
    }

    public bool IsLengthGreaterThan(float val)
    {
        return LengthSqr() > val * val;
    }

    public bool IsLengthLessThan(float val)
    {
        return LengthSqr() < val * val;
    }

    public float DistTo(Vector2D other)
    {
        Vector2D delta = new Vector2D();
        Vector2DSubtract(this, other, out delta);
        return delta.Length();
    }

    public float DistToSqr(Vector2D other)
    {
        Vector2D delta = new Vector2D();
        Vector2DSubtract(this, other, out delta);
        return delta.LengthSqr();
    }

    public void CopyToArray(float[] array)
    {
        Debug.Assert(IsValid());
        Debug.Assert(array != null);

        array[0] = x;
        array[1] = y;
    }

    public void MulAdd(Vector2D a, Vector2D b, float scalar)
    {
        x = a.x + b.x * scalar;
        y = a.y + b.y * scalar;
    }

    public float Dot(Vector2D other)
    {
        return DotProduct2D(this, other);
    }

    //public Vector2D Cross(Vector2D other)
    //{
    //}

    public Vector2D Min(Vector2D other)
    {
        return new Vector2D(x < other.x ? x : other.x, y < other.y ? y : other.y);
    }

    public Vector2D Max(Vector2D other)
    {
        return new Vector2D(x > other.x ? x : other.x, y > other.y ? y : other.y);
    }

    public static void Vector2DClear(Vector2D vector)
    {
        vector.x = vector.y = 0.0f;
    }

    public static void Vector2DCopy(Vector2D src, out Vector2D dst)
    {
        Debug.Assert(src.IsValid());

        dst = new Vector2D(src.x, src.y);
    }

    public static void Vector2DAdd(Vector2D a, Vector2D b, out Vector2D result)
    {
        Debug.Assert(a.IsValid() && b.IsValid());

        result = new Vector2D(a.x + b.x, a.y + b.y);
    }

    public static void Vector2DSubtract(Vector2D a, Vector2D b, out Vector2D result)
    {
        Debug.Assert(a.IsValid() && b.IsValid());

        result = new Vector2D(a.x - b.x, a.y - b.y);
    }

    public static void Vector2DMultiply(Vector2D a, Vector2D b, out Vector2D result)
    {
        Debug.Assert(a.IsValid() && b.IsValid());

        result = new Vector2D(a.x * b.x, a.y * b.y);
    }

    public static void Vector2DMultiply(Vector2D a, float b, out Vector2D result)
    {
        Debug.Assert(a.IsValid() && BaseTypes.IsFinite(b));

        result = new Vector2D(a.x * b, a.y * b);
    }

    public static void Vector2DDivide(Vector2D a, Vector2D b, out Vector2D result)
    {
        Debug.Assert(a.IsValid());
        Debug.Assert(b.x != 0.0f && b.y != 0.0f);

        result = new Vector2D(a.x / b.x, a.y / b.y);
    }

    public static void Vector2DDivide(Vector2D a, float b, out Vector2D result)
    {
        Debug.Assert(a.IsValid());
        Debug.Assert(b != 0.0f);

        float oo = 1.0f / b;
        result = new Vector2D(a.x / oo, a.y / oo);
    }

    public static void Vector2DMA(Vector2D start, float s, Vector2D dir, out Vector2D result)
    {
        Debug.Assert(start.IsValid() && BaseTypes.IsFinite(s) && dir.IsValid());

        result = new Vector2D(start.x + s * dir.x, start.y + s * dir.y);
    }

    public static void Vector2DMin(Vector2D a, Vector2D b, out Vector2D result)
    {
        result = new Vector2D(a.x < b.x ? a.x : b.x, a.y < b.y ? a.y : b.y);
    }

    public static void Vector2DMax(Vector2D a, Vector2D b, out Vector2D result)
    {
        result = new Vector2D(a.x > b.x ? a.x : b.x, a.y > b.y ? a.y : b.y);
    }

    public static float Vector2DNormalize(Vector2D vector)
    {
        Debug.Assert(vector.IsValid());
        float l = vector.Length();

        if (l != 0.0f)
        {
            vector /= l;
        }
        else
        {
            vector.x = vector.y = 0.0f;
        }

        return l;
    }

    public static float Vector2DLength(Vector2D vector)
    {
        Debug.Assert(vector.IsValid());
        return (float)MathF.Sqrt(vector.x*vector.x+ vector.y*vector.y);
    }

    public static float DotProduct2D(Vector2D a, Vector2D b)
    {
        Debug.Assert(a.IsValid() && b.IsValid());
        return (a.x * b.x + a.y * b.y);
    }

    public static void Vector2Lerp(Vector2D src1, Vector2D src2, float t, out Vector2D dest)
    {
        dest = new Vector2D(src1[0] + (src2[0] - src1[0]) * t, src1[1] + (src2[1] - src1[1]) * t);
    }

    public static void ComputeClosestPoint2D(Vector2D start, float maxDist, Vector2D target, out Vector2D result)
    {
        Vector2D delta = new Vector2D();
        Vector2DSubtract(target, start, out delta);
        float distSqr = delta.LengthSqr();

        if (distSqr <= maxDist * maxDist)
        {
            result = target;
        }
        else
        {
            delta /= MathF.Sqrt(distSqr);
            Vector2DMA(start, maxDist, delta, out result);
        }
    }
}