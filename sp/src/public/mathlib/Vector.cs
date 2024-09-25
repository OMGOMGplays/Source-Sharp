using System;
using System.Diagnostics;

namespace Mathlib;

public class Vector
{
    public static void CHECK_VALID(Vector v) { Debug.Assert(v.IsValid()); }

    public const int X_INDEX = 0;
    public const int Y_INDEX = 1;
    public const int Z_INDEX = 2;

    public float x, y, z;

    public Vector() { }

    public Vector(float x, float y, float z)
    {

    }

    public Vector(float xyz)
    {

    }

    public void Init(float x = 0.0f, float y = 0.0f, float z = 0.0f)
    {

    }

    public bool IsValid()
    {

    }

    public void Invalidate()
    {

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

    public float Base()
    {

    }

    public Vector2D AsVector2D()
    {

    }

    public void Random(float minVal, float maxVal)
    {

    }

    public void Zero()
    {

    }

    public bool operator ==(Vector rhs)
    {

    }

    public bool operator !=(Vector rhs)
    {

    }

    public Vector operator +(Vector rhs)
    {

    }

    public Vector operator -(Vector rhs)
    {

    }

    public Vector operator *(Vector rhs)
    {

    }

    public Vector operator *(float rhs)
    {

    }

    public Vector operator /(Vector rhs)
    {

    }

    public Vector operator /(float rhs)
    {

    }

    public Vector operator +(float rhs)
    {

    }

    public Vector operator -(float rhs)
    {

    }

    public void Negate()
    {

    }

    public float Length()
    {

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

    }

    public Vector Normalized()
    {

    }

    public bool IsLengthGreaterThan(float val)
    {

    }

    public bool IsLengthLessThan(float val)
    {

    }

    public bool WithinAABox(Vector boxmin, Vector boxmax)
    {

    }

    public float DistTo(Vector other)
    {

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

    }

    public void MulAdd(Vector a, Vector b, float scalar)
    {

    }

    public float Dot(Vector other)
    {

    }

    //public Vector operator=(Vector rhs)
    //{

    //}

    public float Length2D()
    {

    }

    public float Length2DSqr()
    {

    }

    public Vector Cross(Vector other)
    {

    }

    public Vector Min(Vector other)
    {

    }

    public Vector Max(Vector other)
    {

    }

    private Vector(Vector other)
    {

    }

    private void NetworkVarConstruct(Vector v)
    {
        v.Zero();
    }
}

public class ShortVector
{
    public short x, y, z, w;

    public void Init(short x, short y, short z, short w)
    {

    }

    public void Set(ShortVector other)
    {

    }

    public void Set(short x, short y, short z, short w)
    {

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

    public short Base()
    {

    }

    public bool operator ==(ShortVector rhs)
    {

    }

    public bool operator !=(ShortVector rhs)
    {

    }

    public ShortVector operator +(ShortVector rhs)
    {

    }

    public ShortVector operator -(ShortVector rhs)
    {

    }

    public ShortVector operator *(ShortVector rhs)
    {

    }

    public ShortVector operator *(float rhs)
    {

    }

    public ShortVector operator /(ShortVector rhs)
    {

    }

    public ShortVector operator /(float rhs)
    {

    }
}

public class IntVector4D
{
    public int x, y, z, w;

    public void Init(int x, int y, int z, int w)
    {

    }

    public void Set(IntVector4D other)
    {

    }

    public void Set(int x, int y, int z, int w)
    {

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

    public int Base()
    {

    }

    public bool operator ==(IntVector4D rhs)
    {

    }

    public bool operator !=(IntVector4D rhs)
    {

    }

    public IntVector4D operator +(IntVector4D rhs)
    {

    }

    public IntVector4D operator -(IntVector4D rhs)
    {

    }

    public IntVector4D operator *(IntVector4D rhs)
    {

    }

    public IntVector4D operator *(float rhs)
    {

    }

    public IntVector4D operator /(IntVector4D rhs)
    {

    }

    public IntVector4D operator /(float rhs)
    {

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

}