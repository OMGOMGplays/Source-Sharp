using System;
using System.Diagnostics;

namespace SourceSharp.SP.Public.Mathlib;

public class Vector
{
    public static void CHECK_VALID(dynamic x) { Debug.Assert(x.IsValid()); }

    public const int X_INDEX = 0;
    public const int Y_INDEX = 1;
    public const int Z_INDEX = 2;

    public float x, y, z;

    public Vector()
    {
#if DEBUG
        x = y = z = float.NaN;
#endif // DEBUG
    }

    public Vector(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;

        CHECK_VALID(this);
    }

    public Vector(float xyz)
    {
        x = y = z = xyz;
        CHECK_VALID(this);
    }

    public void Init(float x = 0.0f, float y = 0.0f, float z = 0.0f)
    {
        this.x = x;
        this.y = y;
        this.z = z;

        CHECK_VALID(this);
    }

    public bool IsValid()
    {
        return BaseTypes.IsFinite(x) && BaseTypes.IsFinite(y) && BaseTypes.IsFinite(z);
    }

    public void Invalidate()
    {
        x = y = z = float.NaN;
    }

    public float this[int i]
    {
        get
        {
            switch (i)
            {
                case X_INDEX:
                    return x;

                case Y_INDEX:
                    return y;

                case Z_INDEX:
                    return z;

                default:
                    throw new IndexOutOfRangeException();
            }
        }

        set
        {
            switch (i)
            {
                case X_INDEX:
                    x = value;
                    break;

                case Y_INDEX:
                    y = value;
                    break;

                case Z_INDEX:
                    z = value;
                    break;

                default:
                    throw new IndexOutOfRangeException();
            }
        }
    }

    public float[] Base()
    {
        float[] array = [x, y, z];

        return array;
    }

    public Vector2D AsVector2D()
    {
        Vector2D vector = new Vector2D(x, y);

        return vector;
    }

    public void Random(float minVal, float maxVal)
    {
        Random random = new();

        x = minVal + (float)random.Next() / VALVE_RAND_MAX * (maxVal - minVal);
        y = minVal + (float)random.Next() / VALVE_RAND_MAX * (maxVal - minVal);
        z = minVal + (float)random.Next() / VALVE_RAND_MAX * (maxVal - minVal);

        CHECK_VALID(this);
    }

    public void Zero()
    {
        x = y = z = 0.0f;
    }

    public static bool operator ==(Vector lhs, Vector rhs)
    {
        CHECK_VALID(lhs);
        CHECK_VALID(rhs);
        return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
    }

    public static bool operator !=(Vector lhs, Vector rhs)
    {
        CHECK_VALID(lhs);
        CHECK_VALID(rhs);
        return !(lhs == rhs);
    }

    public static Vector operator -(Vector vector)
    {
        return new Vector(-vector.x, -vector.y, -vector.z);
    }

    public static Vector operator +(Vector lhs, Vector rhs)
    {
        CHECK_VALID(lhs);
        CHECK_VALID(rhs);

        lhs.x += rhs.x;
        lhs.y += rhs.y;
        lhs.z += rhs.z;

        return lhs;
    }

    public static Vector operator -(Vector lhs, Vector rhs)
    {
        CHECK_VALID(lhs);
        CHECK_VALID(rhs);

        lhs.x -= rhs.x;
        lhs.y -= rhs.y;
        lhs.z -= rhs.z;

        return lhs;
    }

    public static Vector operator *(Vector lhs, Vector rhs)
    {
        CHECK_VALID(rhs);

        lhs.x *= rhs.x;
        lhs.y *= rhs.y;
        lhs.z *= rhs.z;

        CHECK_VALID(lhs);

        return lhs;
    }

    public static Vector operator *(Vector lhs, dynamic rhs)
    {
        lhs.x *= rhs;
        lhs.y *= rhs;
        lhs.z *= rhs;

        CHECK_VALID(lhs);

        return lhs;
    }

    public static Vector operator /(Vector lhs, Vector rhs)
    {
        CHECK_VALID(rhs);

        Debug.Assert(rhs.x != 0.0f && rhs.y != 0.0f && rhs.z != 0.0f);

        lhs.x /= rhs.x;
        lhs.y /= rhs.y;
        lhs.z /= rhs.z;

        CHECK_VALID(lhs);

        return lhs;
    }

    public static Vector operator /(Vector lhs, dynamic rhs)
    {
        Debug.Assert(rhs != 0.0f);
        float oo = 1.0f / rhs;

        lhs.x /= oo;
        lhs.y /= oo;
        lhs.z /= oo;

        CHECK_VALID(lhs);

        return lhs;
    }

    public static Vector operator +(Vector lhs, dynamic rhs)
    {
        lhs.x += rhs;
        lhs.y += rhs;
        lhs.z += rhs;

        CHECK_VALID(lhs);

        return lhs;
    }

    public static Vector operator -(Vector lhs, dynamic rhs)
    {
        lhs.x -= rhs;
        lhs.y -= rhs;
        lhs.z -= rhs;

        CHECK_VALID(lhs);

        return lhs;
    }

    public void Negate()
    {
        CHECK_VALID(this);

        x = -x;
        y = -y;
        z = -z;
    }

    public float Length()
    {
        CHECK_VALID(this);
        return VectorLength(this);
    }

    public float LengthSqr()
    {
        CHECK_VALID(this);
        return x * x + y * y + z * z;
    }

    public bool IsZero(float tolerance = 0.01f)
    {
        return x > -tolerance && x < tolerance &&
               y > -tolerance && y < tolerance &&
               z > -tolerance && z < tolerance;
    }

    public float NormalizeInPlace()
    {
        return VectorNormalize(this);
    }

    public Vector Normalized()
    {
        Vector norm = this;
        VectorNormalize(norm);
        return norm;
    }

    public bool IsLengthGreaterThan(float val)
    {
        return LengthSqr() > val * val;
    }

    public bool IsLengthLessThan(float val)
    {
        return LengthSqr() < val * val;
    }

    public bool WithinAABox(Vector boxmin, Vector boxmax)
    {
        return
            (
                (x >= boxmin.x) && (x <= boxmax.x) &&
                (y >= boxmin.y) && (y <= boxmax.y) &&
                (z >= boxmin.z) && (z <= boxmax.z)
            );
    }

    public float DistTo(Vector other)
    {
        Vector delta;
        VectorSubtract(this, other, out delta);
        return delta.Length();
    }

    public float DistToSqr(Vector other)
    {
        Vector delta = new();

        delta.x = x - other.x;
        delta.y = y - other.y;
        delta.z = z - other.z;

        return delta.LengthSqr();
    }

    public void CopyToArray(float[] array)
    {
        Debug.Assert(array != null);
        CHECK_VALID(this);

        array[0] = x;
        array[1] = y;
        array[2] = z;
    }

    public void MulAdd(Vector a, Vector b, float scalar)
    {
        CHECK_VALID(a);
        CHECK_VALID(b);

        x = a.x + b.x * scalar;
        y = a.y + b.y * scalar;
        z = a.z + b.z * scalar;
    }

    public float Dot(Vector other)
    {
        CHECK_VALID(other);

        return DotProduct(this, other);
    }

    //public static Vector operator=(Vector rhs)
    //{

    //}

    public float Length2D()
    {
        return (float)MathF.Sqrt(x * x + y * y);
    }

    public float Length2DSqr()
    {
        return x * x + y * y;
    }

    public Vector Cross(Vector other)
    {
        Vector res = new();
        CrossProduct(this, other, ref res);
        return res;
    }

    public static Vector Min(Vector lhs, Vector rhs)
    {
        return new Vector(lhs.x < rhs.x ? lhs.x : rhs.x,
                          lhs.y < rhs.y ? lhs.y : rhs.y,
                          lhs.z < rhs.z ? lhs.z : rhs.z);
    }

    public static Vector Max(Vector lhs, Vector rhs)
    {
        return new Vector(lhs.x > rhs.x ? lhs.x : rhs.x,
                          lhs.y > rhs.y ? lhs.y : rhs.y,
                          lhs.z > rhs.z ? lhs.z : rhs.z);
    }

    private Vector(Vector other)
    {

    }

    private void NetworkVarConstruct(ref Vector v)
    {
        v.Zero();
    }

    public static void VectorClear(ref Vector vector)
    {
        vector.x = vector.y = vector.z = 0.0f;
    }

    public static void VectorCopy(Vector src, ref Vector dst)
    {
        CHECK_VALID(src);
        dst.x = src.x;
        dst.y = src.y;
        dst.z = src.z;
    }

    public static void VectorAdd(Vector a, Vector b, out Vector result)
    {
        CHECK_VALID(a);
        CHECK_VALID(b);

        result = new Vector(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static void VectorSubtract(Vector a, Vector b, out Vector result)
    {
        CHECK_VALID(a);
        CHECK_VALID(b);

        result = new Vector(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static void VectorMultiply(Vector a, Vector b, out Vector result)
    {
        CHECK_VALID(a);
        CHECK_VALID(b);

        result = new Vector(a.x * b.x, a.y * b.y, a.z * b.z);
    }

    public static void VectorMultiply(Vector a, float b, out Vector result)
    {
        CHECK_VALID(a);
        Debug.Assert(DataTypes.IsFinite(b));

        result = new Vector(a.x * b, a.y * b, a.z * b);
    }

    public static void VectorDivide(Vector a, Vector b, out Vector result)
    {
        CHECK_VALID(a);
        CHECK_VALID(b);

        Debug.Assert(b.x != 0.0f && b.y != 0.0f && b.z != 0.0f);

        result = new Vector(a.x / b.x, a.y / b.y, a.z / b.z);
    }

    public static void VectorDivide(Vector a, float b, out Vector result)
    {
        CHECK_VALID(a);
        Debug.Assert(b != 0.0f);

        float oo = 1.0f / b;

        result = new Vector(a.x / oo, a.y / oo, a.z / oo);
    }

    public static void VectorScale(Vector @in, float scale, out Vector result)
    {
        VectorMultiply(@in, scale, out result);
    }

    public static void VectorMA(Vector start, float scale, Vector direction, ref Vector dest)
    {

    }

    public static bool VectorsAreEqual(Vector src1, Vector src2, float tolerance)
    {
        if (BaseTypes.FloatMakePositive(src1.x - src2.x) > tolerance)
        {
            return false;
        }

        if (BaseTypes.FloatMakePositive(src1.y - src2.y) > tolerance)
        {
            return false;
        }

        return BaseTypes.FloatMakePositive(src1.z - src2.z) <= tolerance;
    }

    public static float VectorLength(Vector vector)
    {
        CHECK_VALID(vector);

        return (float)MathF.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
    }

    public static float DotProduct(Vector a, Vector b)
    {
        CHECK_VALID(a);
        CHECK_VALID(b);

        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    public static void CrossProduct(Vector a, Vector b, ref Vector result)
    {
        CHECK_VALID(a);
        CHECK_VALID(b);

        Debug.Assert(a != result);
        Debug.Assert(b != result);

        result.x = a.y * b.z - a.z * b.y;
        result.y = a.z * b.x - a.x * b.z;
        result.z = a.x * b.y - a.y * b.x;
    }

    public static void VectorMin(Vector a, Vector b, out Vector result)
    {
        result = new Vector(float.Min(a.x, b.x), float.Min(a.y, b.y), float.Min(a.z, b.z));
    }

    public static void VectorMax(Vector a, Vector b, out Vector result)
    {
        result = new Vector(float.Max(a.x, b.x), float.Max(a.y, b.y), float.Max(a.z, b.z));
    }

    public static void VectorLerp(Vector src1, Vector src2, float t, out Vector dest)
    {
        CHECK_VALID(src1);
        CHECK_VALID(src2);

        dest = new Vector(src1.x + (src2.x - src1.x) * t,
                          src1.y + (src2.y - src1.y) * t,
                          src1.z + (src2.z - src1.z) * t);
    }

    public static Vector VectorLerp(Vector src1, Vector src2, float t)
    {
        VectorLerp(src1, src2, t, out Vector result);
        return result;
    }

    public static Vector ReplicateToVector(float x)
    {
        return new Vector(x, x, x);
    }

    public static bool PointWithinAngle(Vector srcPosition, Vector targetPosition, Vector lookDirection, float cosHalfFOV)
    {
        Vector delta = targetPosition - srcPosition;
        float cosDiff = DotProduct(lookDirection, delta);

        if (cosDiff < 0)
        {
            return false;
        }

        float len2 = delta.LengthSqr();

        return cosDiff * cosDiff > len2 * cosHalfFOV * cosHalfFOV;
    }

    public static Vector CrossProduct(Vector a, Vector b)
    {
        return new Vector(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
    }

    public static Vector RandomVector(float minVal, float maxVal)
    {
        Vector random = new Vector();
        random.Random(minVal, maxVal);
        return random;
    }

    public static float RandomVectorUnitInSphere(Vector vector)
    {
        Random rand = new();
        float u = (float)rand.Next() / VALVE_RAND_MAX;
        float v = (float)rand.Next() / VALVE_RAND_MAX;
        float w = (float)rand.Next() / VALVE_RAND_MAX;

        float phi = (float)Math.Acos(1 - 2 * u);
        float theta = (float)(2 * Mathlib.M_PI * v);
        float radius = MathF.Pow(w, 1.0f / 3.0f);

        float sinPhi = 0.0f, cosPhi = 0.0f;
        float sinTheta = 0.0f, cosTheta = 0.0f;
        Math.SinCos(phi); Math.SinCos(sinPhi); Math.SinCos(cosPhi);
        Math.SinCos(theta); Math.SinCos(sinTheta); Math.SinCos(cosTheta);

        vector.x = radius * sinPhi * cosTheta;
        vector.y = radius * sinPhi * sinTheta;
        vector.z = radius * cosPhi;

        return radius;
    }

    public static float RandomVectorUnitInSphere(Vector2D vector)
    {
        Random rand = new();
        float u = (float)rand.Next() / VALVE_RAND_MAX;
        float v = (float)rand.Next() / VALVE_RAND_MAX;

        float theta = (float)(2 * Mathlib.M_PI * v);
        float radius = MathF.Pow(u, 1.0f / 2.0f);

        float sinTheta = 0.0f, cosTheta = 0.0f;
        Math.SinCos(theta); Math.SinCos(sinTheta); Math.SinCos(cosTheta);

        vector.x = radius * cosTheta;
        vector.y = radius * sinTheta;

        return radius;
    }

    public Vector AllocTempVector()
    {
        Vector[] temp = new Vector[128];
        CInterlockedInt s_index;

        int index;

        for (; ; )
        {
            int oldIndex = s_index;
            index = ((oldIndex + 0x10001) & 0x7F);

            if (s_index.AssignIf(oldIndex, index))
            {
                break;
            }

            ThreadTools.ThreadPause();
        }

        return temp[index];
    }

    public static float DotProductAbs(Vector v0, Vector v1)
    {
        CHECK_VALID(v0);
        CHECK_VALID(v1);

        return BaseTypes.FloatMakePositive(v0.x * v1[0]) + BaseTypes.FloatMakePositive(v0.y * v1[1]) + BaseTypes.FloatMakePositive(v0.z * v1[2]);
    }

    public static void ComputeClosestPoint(Vector start, float maxDist, Vector target, ref Vector result)
    {
        Vector delta;
        VectorSubtract(target, start, out delta);
        float distSqr = delta.LengthSqr();

        if (distSqr <= maxDist * maxDist)
        {
            result = target;
        }
        else
        {
            delta /= MathF.Sqrt(distSqr);
            VectorMA(start, maxDist, delta, ref result);
        }
    }

    public static void VectorAbs(Vector src, ref Vector dst)
    {
        dst.x = BaseTypes.FloatMakePositive(src.x);
        dst.y = BaseTypes.FloatMakePositive(src.y);
        dst.z = BaseTypes.FloatMakePositive(src.z);
    }

    public static float ComputeVolume(Vector mins, Vector maxs)
    {
        Vector delta = new Vector();
        VectorSubtract(maxs, mins, out delta);
        return DotProduct(delta, delta);
    }

    public static Vector RandomAngularImpulse(float minVal, float maxVal)
    {
        Vector angImp = new Vector();
        angImp.Random(minVal, maxVal);
        return angImp;
    }

    public static void VectorCopy(RadianEuler src, out RadianEuler dst)
    {
        CHECK_VALID(src);

        dst = new RadianEuler(src.x, src.y, src.z);
    }

    public static void VectorScale(RadianEuler src, float b, out RadianEuler dst)
    {
        CHECK_VALID(src);
        Debug.Assert(BaseTypes.IsFinite(b));

        dst = new RadianEuler(src.x * b, src.y * b, src.z * b);
    }

    public static void VectorAdd(QAngle a, QAngle b, out QAngle result)
    {
        CHECK_VALID(a);
        CHECK_VALID(b);

        result = new QAngle(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static void VectorMA(QAngle start, float scale, QAngle direction, out QAngle dest)
    {
        CHECK_VALID(start);
        CHECK_VALID(direction);

        dest = new QAngle(start.x + scale * direction.x,
                          start.y + scale * direction.y,
                          start.z + scale * direction.z);
    }

    public static void VectorCopy(QAngle src, out QAngle dst)
    {
        CHECK_VALID(src);

        dst = new QAngle(src.x, src.y, src.z);
    }

    public static float VectorNormalize(ref Vector vector)
    {
        float sqrlen = vector.LengthSqr() + 1.0e-10f, invlen;
        invlen = MathF.Sqrt(sqrlen);
        vector.x *= invlen;
        vector.y *= invlen;
        vector.z *= invlen;

        return sqrlen * invlen;
    }
}

public class ShortVector
{
    public short x, y, z, w;

    public void Init(short x, short y, short z, short w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public void Set(ShortVector other)
    {
        x = other.x;
        y = other.y;
        z = other.z;
        w = other.w;
    }

    public void Set(short x, short y, short z, short w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public short this[int i]
    {
        get
        {
            switch (i)
            {
                case Vector.X_INDEX:
                    return x;

                case Vector.Y_INDEX:
                    return y;

                case Vector.Z_INDEX:
                    return z;

                case Vector.Z_INDEX + 1:
                    return w;

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

                case Vector.Z_INDEX:
                    z = value;
                    break;

                case Vector.Z_INDEX + 1:
                    w = value;
                    break;

                default:
                    throw new IndexOutOfRangeException();
            }
        }
    }

    public short[] Base()
    {
        short[] array = [x, y, z, w];

        return array;
    }

    public static bool operator ==(ShortVector lhs, ShortVector rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z && lhs.w == rhs.w;
    }

    public static bool operator !=(ShortVector lhs, ShortVector rhs)
    {
        return !(lhs == rhs);
    }

    public static ShortVector operator +(ShortVector lhs, ShortVector rhs)
    {
        lhs.x += rhs.x;
        lhs.y += rhs.y;
        lhs.z += rhs.z;
        lhs.w += rhs.w;

        return lhs;
    }

    public static ShortVector operator -(ShortVector lhs, ShortVector rhs)
    {
        lhs.x -= rhs.x;
        lhs.y -= rhs.y;
        lhs.z -= rhs.z;
        lhs.w -= rhs.w;

        return lhs;
    }

    public static ShortVector operator *(ShortVector lhs, ShortVector rhs)
    {
        lhs.x *= rhs.x;
        lhs.y *= rhs.y;
        lhs.z *= rhs.z;
        lhs.w *= rhs.w;

        return lhs;
    }

    public static ShortVector operator *(ShortVector lhs, dynamic rhs)
    {
        lhs.x *= (short)rhs;
        lhs.y *= (short)rhs;
        lhs.z *= (short)rhs;
        lhs.w *= (short)rhs;

        return lhs;
    }

    public static ShortVector operator /(ShortVector lhs, ShortVector rhs)
    {
        Debug.Assert(rhs.x != 0.0f && rhs.y != 0.0f && rhs.z != 0.0f);

        lhs.x /= rhs.x;
        lhs.y /= rhs.y;
        lhs.z /= rhs.z;
        lhs.w /= rhs.w;

        return lhs;
    }

    public static ShortVector operator /(ShortVector lhs, dynamic rhs)
    {
        Debug.Assert(rhs != 0.0f);
        float oo = 1.0f / rhs;

        lhs.x *= (short)oo;
        lhs.y *= (short)oo;
        lhs.z *= (short)oo;
        lhs.w *= (short)oo;

        return lhs;
    }

    public static void ShortVectorMultiply(ShortVector src, float fl, ref ShortVector res)
    {
        Debug.Assert(BaseTypes.IsFinite(fl));

        res.x = (short)(src.x * fl);
        res.y = (short)(src.y * fl);
        res.z = (short)(src.z * fl);
        res.w = (short)(src.w * fl);
    }
}

public class IntVector4D
{
    public int x, y, z, w;

    public void Init(int x, int y, int z, int w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public void Set(IntVector4D other)
    {
        x = other.x;
        y = other.y;
        z = other.z;
        w = other.w;
    }

    public void Set(int x, int y, int z, int w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public int this[int i]
    {
        get
        {
            switch (i)
            {
                case Vector.X_INDEX:
                    return x;

                case Vector.Y_INDEX:
                    return y;

                case Vector.Z_INDEX:
                    return z;

                case Vector.Z_INDEX + 1:
                    return w;

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

                case Vector.Z_INDEX:
                    z = value;
                    break;

                case Vector.Z_INDEX + 1:
                    w = value;
                    break;

                default:
                    throw new IndexOutOfRangeException();
            }
        }
    }

    public int[] Base()
    {
        int[] array = [x, y, z, w];

        return array;
    }

    public static bool operator ==(IntVector4D lhs, IntVector4D rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z && lhs.w == rhs.w;
    }

    public static bool operator !=(IntVector4D lhs, IntVector4D rhs)
    {
        return !(lhs == rhs);
    }

    public static IntVector4D operator +(IntVector4D lhs, IntVector4D rhs)
    {
        lhs.x += rhs.x;
        lhs.y += rhs.y;
        lhs.z += rhs.z;
        lhs.w += rhs.w;

        return lhs;
    }

    public static IntVector4D operator -(IntVector4D lhs, IntVector4D rhs)
    {
        lhs.x -= rhs.x;
        lhs.y -= rhs.y;
        lhs.z -= rhs.z;
        lhs.w -= rhs.w;

        return lhs;
    }

    public static IntVector4D operator *(IntVector4D lhs, IntVector4D rhs)
    {
        lhs.x *= rhs.x;
        lhs.y *= rhs.y;
        lhs.z *= rhs.z;
        lhs.w *= rhs.w;

        return lhs;
    }

    public static IntVector4D operator *(IntVector4D lhs, dynamic rhs)
    {
        lhs.x *= (int)rhs;
        lhs.y *= (int)rhs;
        lhs.z *= (int)rhs;
        lhs.w *= (int)rhs;

        return lhs;
    }

    public static IntVector4D operator /(IntVector4D lhs, IntVector4D rhs)
    {
        Debug.Assert(rhs.x != 0.0f && rhs.y != 0.0f && rhs.z != 0.0f && rhs.w != 0.0f);

        lhs.x /= rhs.x;
        lhs.y /= rhs.y;
        lhs.z /= rhs.z;
        lhs.w /= rhs.w;

        return lhs;
    }

    public static IntVector4D operator /(IntVector4D lhs, dynamic rhs)
    {
        Debug.Assert(rhs != 0.0f);
        float oo = 1.0f / rhs;

        lhs.x *= (int)oo;
        lhs.y *= (int)oo;
        lhs.z *= (int)oo;
        lhs.w *= (int)oo;

        return lhs;
    }

    public static void IntVector4DMultiply(IntVector4D src, float fl, ref IntVector4D res)
    {
        Debug.Assert(DataTypes.IsFinite(fl));

        res.x = src.x * (int)fl;
        res.y = src.y * (int)fl;
        res.z = src.z * (int)fl;
        res.w = src.w * (int)fl;
    }
}

public class VectorByValue : Vector
{
    public VectorByValue() : base()
    {

    }

    public VectorByValue(float x, float y, float z) : base(x, y, z)
    {

    }

    public VectorByValue(VectorByValue other)
    {
        x = other.x;
        y = other.y;
        z = other.z;
    }
}

public class TableVector
{
    public float x, y, z;

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

                case Vector.Z_INDEX:
                    return z;

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

                case Vector.Z_INDEX:
                    z = value;
                    break;

                default:
                    throw new IndexOutOfRangeException();
            }
        }
    }
}

public class VectorAligned : Vector
{
    public VectorAligned()
    {

    }

    public VectorAligned(float x, float y, float z)
    {
        Init(x, y, z);
    }

    private VectorAligned(VectorAligned other)
    {

    }

    public VectorAligned(Vector other)
    {

    }

    //public VectorAligned operator=(Vector other)
    //{
    //    Init(other.x, other.y, other.z);
    //    return this;
    //}

    public float w;
}

public class Quaternion
{
    public float x, y, z, w;

    public Quaternion()
    {
#if DEBUG
        x = y = z = w = float.NaN;
#endif // DEBUG
    }

    public Quaternion(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public Quaternion(RadianEuler angle)
    {
        AngleQuaternion(angle, this);
    }

    public void Init(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public bool IsValid()
    {
        return BaseTypes.IsFinite(x) && BaseTypes.IsFinite(y) && BaseTypes.IsFinite(z) && BaseTypes.IsFinite(w);
    }

    public void Invalidate()
    {
        x = y = z = w = float.NaN;
    }

    public static bool operator ==(Quaternion lhs, Quaternion rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z && lhs.w == rhs.w;
    }

    public static bool operator !=(Quaternion lhs, Quaternion rhs)
    {
        return !(lhs == rhs);
    }

    public float[] Base()
    {
        float[] array = [x, y, z, w];

        return array;
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

                case Vector.Z_INDEX:
                    return z;

                case Vector.Z_INDEX + 1:
                    return w;

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

                case Vector.Z_INDEX:
                    z = value;
                    break;

                case Vector.Z_INDEX + 1:
                    w = value;
                    break;

                default:
                    throw new IndexOutOfRangeException();
            }
        }
    }

    public static bool QuaternionsAreEqual(Quaternion src1, Quaternion src2, float tolerance)
    {
        if (BaseTypes.FloatMakePositive(src1.x - src2.x) > tolerance)
        {
            return false;
        }

        if (BaseTypes.FloatMakePositive(src1.y - src2.y) > tolerance)
        {
            return false;
        }

        if (BaseTypes.FloatMakePositive(src1.z - src2.z) > tolerance)
        {
            return false;
        }

        return BaseTypes.FloatMakePositive(src1.w - src2.w) <= tolerance;
    }

    public static void AngleQuaternion(RadianEuler angles, Quaternion qt)
    {

    }

    public static void NetworkVarConstruct(ref Quaternion q)
    {
        q.x = q.y = q.z = q.w = 0.0f;
    }
}

public class QuaternionAligned : Quaternion
{
    public QuaternionAligned()
    {

    }

    public QuaternionAligned(float x, float y, float z, float w)
    {
        Init(x, y, z, w);
    }

    private QuaternionAligned(QuaternionAligned other)
    {

    }

    public QuaternionAligned(Quaternion other)
    {
        Init(other.x, other.y, other.z, other.w);
    }
}

public class RadianEuler
{
    public float x, y, z;

    public RadianEuler()
    {

    }

    public RadianEuler(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public RadianEuler(Quaternion q)
    {
        QuaternionAngles(q, this);
    }

    public RadianEuler(QAngle angles)
    {
        Init(angles.z * 3.14159265358979323846f / 180.0f,
             angles.x * 3.14159265358979323846f / 180.0f,
             angles.y * 3.14159265358979323846f / 180.0f);
    }

    public void Init(float x = 0.0f, float y = 0.0f, float z = 0.0f)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public QAngle ToQAngle()
    {
        return new QAngle(y * 180.0f / 3.14159265358979323846f,
                          z * 180.0f / 3.14159265358979323846f,
                          x * 180.0f / 3.14159265358979323846f);
    }

    public bool IsValid()
    {
        return BaseTypes.IsFinite(x) && BaseTypes.IsFinite(y) && BaseTypes.IsFinite(z);
    }

    public void Invalidate()
    {
        x = y = z = float.NaN;
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

                case Vector.Z_INDEX:
                    return z;

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

                case Vector.Z_INDEX:
                    z = value;
                    break;

                default:
                    throw new IndexOutOfRangeException();
            }
        }
    }

    public static void QuaternionAngles(Quaternion qt, RadianEuler angles)
    {

    }
}

public class QAngle
{
    public float x, y, z;

    public QAngle()
    {
#if DEBUG
        x = y = z = float.NaN;
#endif // DEBUG
    }

    public QAngle(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;

        Vector.CHECK_VALID(this);
    }

    public void Init(float x = 0, float y = 0, float z = 0)
    {
        this.x = x;
        this.y = y;
        this.z = z;

        Vector.CHECK_VALID(this);
    }

    public void Random(float minVal, float maxVal)
    {
        Random rand;

        x = minVal + (float)rand.Next() / VALVE_RAND_MAX * (maxVal - minVal);
        y = minVal + (float)rand.Next() / VALVE_RAND_MAX * (maxVal - minVal);
        z = minVal + (float)rand.Next() / VALVE_RAND_MAX * (maxVal - minVal);

        Vector.CHECK_VALID(this);
    }

    public bool IsValid()
    {
        return BaseTypes.IsFinite(x) && BaseTypes.IsFinite(y) && BaseTypes.IsFinite(z);
    }

    public void Invalidate()
    {
        x = y = z = float.NaN;
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

                case Vector.Z_INDEX:
                    return z;

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

                case Vector.Z_INDEX:
                    z = value;
                    break;

                default:
                    throw new IndexOutOfRangeException();
            }
        }
    }

    public float[] Base()
    {
        float[] array = [x, y, z];

        return array;
    }

    public static bool operator ==(QAngle lhs, QAngle rhs)
    {
        Vector.CHECK_VALID(lhs);
        Vector.CHECK_VALID(rhs);

        return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
    }

    public static bool operator !=(QAngle lhs, QAngle rhs)
    {
        Vector.CHECK_VALID(lhs);
        Vector.CHECK_VALID(rhs);

        return !(lhs == rhs);
    }

    public static QAngle operator -(QAngle angle)
    {
        return new QAngle(-angle.x, -angle.y, -angle.z);
    }

    public static QAngle operator +(QAngle lhs, QAngle rhs)
    {
        Vector.CHECK_VALID(lhs);
        Vector.CHECK_VALID(rhs);

        lhs.x += rhs.x;
        lhs.y += rhs.y;
        lhs.z += rhs.z;

        return lhs;
    }

    public static QAngle operator -(QAngle lhs, QAngle rhs)
    {
        Vector.CHECK_VALID(lhs);
        Vector.CHECK_VALID(rhs);

        lhs.x -= rhs.x;
        lhs.y -= rhs.y;
        lhs.z -= rhs.z;

        return lhs;
    }

    public static QAngle operator *(QAngle lhs, dynamic rhs)
    {
        lhs.x *= rhs;
        lhs.y *= rhs;
        lhs.z *= rhs;

        Vector.CHECK_VALID(lhs);
        return lhs;
    }

    public static QAngle operator /(QAngle lhs, dynamic rhs)
    {
        Debug.Assert(rhs != 0.0f);
        float oo = 1.0f / rhs;

        lhs.x *= oo;
        lhs.y *= oo;
        lhs.z *= oo;

        Vector.CHECK_VALID(lhs);
        return lhs;
    }

    public float Length()
    {
        Vector.CHECK_VALID(this);
        return (float)MathF.Sqrt(LengthSqr());
    }

    public float LengthSqr()
    {
        Vector.CHECK_VALID(this);
        return x * x + y * y + z * z;
    }

    public static void NetworkVarConstruct(ref QAngle q)
    {
        q.x = q.y = q.z = 0.0f;
    }

    public static QAngle RandomAngle(float minVal, float maxVal)
    {
        Vector random = new Vector();
        random.Random(minVal, maxVal);
        QAngle ret = new QAngle(random.x, random.y, random.z);
        return ret;
    }

    public static bool QAnglesAreEqual(QAngle src1, QAngle src2, float tolerance = 0.0f)
    {
        if (BaseTypes.FloatMakePositive(src1.x - src2.x) > tolerance)
        {
            return false;
        }

        if (BaseTypes.FloatMakePositive(src1.y - src2.y) > tolerance)
        {
            return false;
        }

        return BaseTypes.FloatMakePositive(src1.z - src2.z) <= tolerance;
    }

    public static void QAngleToAngularImpulse(QAngle angles, out Vector impulse)
    {
        impulse = new Vector(angles.z, angles.x, angles.y);
    }

    public static void AngularImpulseToQAngle(Vector impulse, out QAngle angles)
    {
        angles = new QAngle(impulse.y, impulse.z, impulse.x);
    }
}

public class QAngleByValue : QAngle
{
    public QAngleByValue() : base()
    {

    }

    public QAngleByValue(float x, float y, float z) : base(x, y, z)
    {

    }

    public QAngleByValue(QAngleByValue other)
    {
        Init(other.x, other.y, other.z);
    }
}