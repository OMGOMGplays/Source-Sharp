namespace Mathlib;

public static class compressed_3d_unitvec
{
    public static void UNITVEC_DECLARE_STATICS()
    {
        cUnitVector.mUVAdjustment = new float[0x2000];
        cUnitVector.mTmpVec = new Vector();
    }

    public const int SIGN_MASK = 0xe000;
    public const int XSIGN_MASK = 0x8000;
    public const int YSIGN_MASK = 0x4000;
    public const int ZSIGN_MASK = 0x2000;

    public const int TOP_MASK = 0x1f80;

    public const int BOTTOM_MASK = 0x007f;
}

public class cUnitVector
{
    public ushort mVec;
    public static float[] mUVAdjustment;
    public static Vector mTmpVec;

    public cUnitVector()
    {
        mVec = 0;
    }

    public cUnitVector(Vector vec)
    {
        packVector(vec);
    }

    public cUnitVector(ushort val)
    {
        mVec = val;
    }

    public cUnitVector Equals(Vector vec)
    {
        packVector(vec);
        return this;
    }

    public Vector vector()
    {
        unpackVector(mTmpVec);
        return mTmpVec;
    }

    public void packVector(Vector vec)
    {
        Debug.Assert(vec.IsValid());
        Vector tmp = vec;

        mVec = 0;

        if (tmp.x < 0)
        {
            mVec |= compressed_3d_unitvec.XSIGN_MASK;
            tmp.x = -tmp.x;
        }

        if (tmp.y < 0)
        {
            mVec |= compressed_3d_unitvec.YSIGN_MASK;
            tmp.y = -tmp.y;
        }

        if (tmp.z < 0)
        {
            mVec |= compressed_3d_unitvec.ZSIGN_MASK;
            tmp.z = -tmp.z;
        }

        float w = 126.0f / (tmp.x + tmp.y + tmp.z);
        long xbits = (long)(tmp.x * w);
        long ybits = (long)(tmp.y * w);

        Debug.Assert(xbits < 127);
        Debug.Assert(xbits >= 0);
        Debug.Assert(ybits < 127);
        Debug.Assert(ybits >= 0);

        if (xbits >= 64)
        {
            xbits = 127 - xbits;
            ybits = 127 - ybits;
        }

        mVec |= (ushort)(xbits << 7);
        mVec |= (ushort)ybits;
    }

    public void unpackVector(Vector vec)
    {
        long xbits = ((mVec & compressed_3d_unitvec.TOP_MASK) >> 7);
        long ybits = (mVec & compressed_3d_unitvec.BOTTOM_MASK);

        if ((xbits + ybits) >= 127)
        {
            xbits = 127 - xbits;
            ybits = 127 - ybits;
        }

        float uvadj = mUVAdjustment[mVec & ~compressed_3d_unitvec.SIGN_MASK];
        vec.x = uvadj * (float)xbits;
        vec.y = uvadj * (float)ybits;
        vec.z = uvadj * (float)(126 - xbits - ybits);

        if ((mVec & compressed_3d_unitvec.XSIGN_MASK) != 0)
        {
            vec.x = -vec.x;
        }

        if ((mVec & compressed_3d_unitvec.YSIGN_MASK) != 0)
        {
            vec.y = -vec.y;
        }

        if ((mVec & compressed_3d_unitvec.ZSIGN_MASK) != 0)
        {
            vec.z = -vec.z;
        }

        Debug.Assert(vec.IsValid());
    }

    public static void InitializeStatics()
    {
        for (int idx = 0; idx < 0x2000; idx++)
        {
            long xbits = idx >> 7;
            long ybits = idx & compressed_3d_unitvec.BOTTOM_MASK;

            if ((xbits + ybits) >= 127)
            {
                xbits = 127 - xbits;
                ybits = 127 - ybits;
            }

            float x = (float)xbits;
            float y = (float)ybits;
            float z = (float)(126 - xbits - ybits);

            mUVAdjustment[idx] = 1.0f / sqrtf(y * y + z * z + x * x);
            Debug.Assert(platform._finite(mUVAdjustment[idx]));
        }
    }
}