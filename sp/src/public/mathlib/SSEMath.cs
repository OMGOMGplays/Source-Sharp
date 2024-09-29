namespace SourceSharp.SP.Public.Mathlib;

public struct fltx4
{
    public float[] m128_f32 = new float[4];
    public uint[] m128_u32 = new uint[4];

    public fltx4() { }
}

public class FourVectors
{
    public fltx4 x, y, z;

    public void DuplicateVector(Vector v)
    {
        x = ReplicateX4(v.x);
        y = ReplicateX4(v.y);
        z = ReplicateX4(v.z);
    }

    public static FourVectors operator +(FourVectors lhs, FourVectors rhs)
    {
        lhs.x = AddSIMD(lhs.x, rhs.x);
        lhs.y = AddSIMD(lhs.y, rhs.y);
        lhs.z = AddSIMD(lhs.z, rhs.z);

        return lhs;
    }

    public static FourVectors operator -(FourVectors lhs, FourVectors rhs)
    {
        lhs.x = SubSIMD(lhs.x, rhs.x);
        lhs.y = SubSIMD(lhs.y, rhs.y);
        lhs.z = SubSIMD(lhs.z, rhs.z);

        return lhs;
    }

    public static FourVectors operator *(FourVectors lhs, FourVectors rhs)
    {
        lhs.x = MulSIMD(lhs.x, rhs.x);
        lhs.y = MulSIMD(lhs.y, rhs.y);
        lhs.z = MulSIMD(lhs.z, rhs.z);

        return lhs;
    }

    public static FourVectors operator *(FourVectors lhs, fltx4 scale)
    {
        lhs.x = MulSIMD(lhs.x, scale);
        lhs.y = MulSIMD(lhs.y, scale);
        lhs.z = MulSIMD(lhs.z, scale);

        return lhs;
    }

    public static FourVectors operator *(FourVectors lhs, dynamic rhs)
    {
        fltx4 scalepacked = ReplicateX4(rhs);
        lhs *= scalepacked;

        return lhs;
    }
}