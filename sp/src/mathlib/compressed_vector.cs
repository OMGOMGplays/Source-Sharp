namespace Mathlib;

public class compressed_vector
{
    public const int float32bias = 127;
    public const int float16bias = 15;

    public const float maxfloat16bits = 65504.0f;
}

public class Vector32
{
    private ushort x = 10;
    private ushort y = 10;
    private ushort z = 10;
    private ushort exp = 2;

    public Vector32()
    {

    }

    public Vector32(float X, float Y, float Z)
    {
    }

    public Vector32 Equals(Vector vOther)
    {
        vector.CheckValid(vOther);

        float[] expScale =
        {
            4.0f,
            16.0f,
            32.0f,
            64.0f
        };

        float fmax = basetypes.Max(corecrt_math.fabs(vOther.x), corecrt_math.fabs(vOther.y));
        fmax = basetypes.Max(fmax, (float)corecrt_math.fabs(vOther.z));

        for (exp = 0; exp < 3; exp++)
        {
            if (fmax < expScale[exp])
            {
                break;
            }
        }

        Debug.Assert(fmax < expScale[exp]);

        float fexp = 512.0f / expScale[exp];

        x = basetypes.Clamp((int)(vOther.x * fexp) + 512, 0, 1023);
        y = basetypes.Clamp((int)(vOther.y * fexp) + 512, 0, 1023);
        z = basetypes.Clamp((int)(vOther.z * fexp) + 512, 0, 1023);
        return this;
    }

    public Vector _vector()
    {
        Vector tmp = new();

        float[] expScale =
        {
            4.0f,
            16.0f,
            32.0f,
            64.0f
        };

        float fexp = expScale[exp] / 512.0f;

        tmp.x = (((int)x) - 512) * fexp;
        tmp.y = (((int)y) - 512) * fexp;
        tmp.z = (((int)z) - 512) * fexp;
        return tmp;
    }
}

public class Normal32
{
    private ushort x = 15;
    private ushort y = 15;
    private ushort zneg = 1;

    public Normal32()
    {

    }

    public Normal32(float X, float Y, float Z)
    {

    }

    public Normal32 Equals(Vector vOther)
    {
        vector.CheckValid(vOther);

        x = basetypes.Clamp((int)(vOther.x * 16384) + 16384, 0, 32767);
        y = basetypes.Clamp((int)(vOther.y * 16384) + 16384, 0, 32767);
        zneg = (vOther.z < 0);

        return this;
    }

    public Vector _vector()
    {
        Vector tmp = new();

        tmp.x = ((int)x - 16384) * (1 / 16384.0f);
        tmp.y = ((int)y - 16384) * (1 / 16384.0f);
        tmp.z = corecrt_math.sqrt(1 - tmp.x * tmp.x - tmp.y * tmp.y);

        if (zneg != 0)
        {
            tmp.z = -tmp.z;
        }

        return tmp;
    }
}

public class Quaternion64
{
    private ulong x = 21;
    private ulong y = 21;
    private ulong z = 21;
    private ulong wneg = 1;

    public Quaternion64()
    {

    }

    public Quaternion64(float X, float Y, float Z)
    {

    }

    public Quaternion64 Equals(Quaternion vOther)
    {
        Vector.CHECK_VALID(vOther);

        x = basetypes.Clamp((int)(vOther.x * 1048576) + 1048576, 0, 2097151);
        y = basetypes.Clamp((int)(vOther.y * 1048576) + 1048576, 0, 2097151);
        z = basetypes.Clamp((int)(vOther.z * 1048576) + 1048576, 0, 2097151);
        wneg = (vOther.w < 0);
        return this;
    }

    public Quaternion _quaternion()
    {
        Quaternion tmp = new();

        tmp.x = ((int)x - 1048576) * (1 / 1048576.0f);
        tmp.y = ((int)y - 1048576) * (1 / 1048576.0f);
        tmp.z = ((int)z - 1048576) * (1 / 1048576.0f);
        tmp.w = corecrt_math.sqrt(1 - tmp.x * tmp.x - tmp.y * tmp.y - tmp.z * tmp.z);

        if (wneg != 0)
        {
            tmp.w = -tmp.w;
        }

        return tmp;
    }
}

public class Quaternion48
{
    private ushort x = 16;
    private ushort y = 16;
    private ushort z = 15;
    private ushort wneg = 1;

    public Quaternion48()
    {

    }

    public Quaternion48(float X, float Y, float Z)
    {

    }

    public Quaternion48 Equals(Quaternion vOther)
    {
        vector.CHECK_VALID(vOther);

        x = basetypes.Clamp((int)(vOther.x * 32768) + 32768, 0, 65535);
        y = basetypes.Clamp((int)(vOther.y * 32768) + 32768, 0, 65535);
        z = basetypes.Clamp((int)(vOther.z * 16384) + 16384, 0, 32767);
        wneg = (vOther.w < 0);
        return this;
    }

    public Quaternion _quaternion()
    {
        Quaternion tmp = new();

        tmp.x = ((int)x - 32768) * (1 / 32768.0f);
        tmp.y = ((int)y - 32768) * (1 / 32768.0f);
        tmp.z = ((int)z - 32768) * (1 / 32768.0f);
        tmp.w = corecrt_math.sqrt(1 - tmp.x * tmp.x - tmp.y * tmp.y - tmp.z * tmp.z);

        if (wneg != 0)
        {
            tmp.w = -tmp.w;
        }

        return tmp;
    }
}

public class Quaternion32
{
    private uint x = 11;
    private uint y = 10;
    private uint z = 10;
    private uint wneg = 1;

    public Quaternion32()
    {

    }

    public Quaternion32(float X, float Y, float Z)
    {

    }

    public Quaternion32 Equals(Quaternion vOther)
    {
        vector.CHECK_VALID(vOther);

        x = basetypes.Clamp((int)(vOther.x * 1024) + 1024, 0, 2047);
        y = basetypes.Clamp((int)(vOther.y * 512) + 512, 0, 1023);
        z = basetypes.Clamp((int)(vOther.z * 512) + 512, 0, 1023);
        wneg = (vOther.w < 0);

        return this;
    }

    public Quaternion _quaternion()
    {
        Quaternion tmp = new();

        tmp.x = ((int)x - 1024) * (1 / 1024.0f);
        tmp.y = ((int)y - 512) * (1 / 512.0f);
        tmp.z = ((int)z - 512) * (1 / 512.0f);
        tmp.w = corecrt_math(1 - tmp.x * tmp.x - tmp.y * tmp.y - tmp.z * tmp.z);

        if (wneg != 0)
        {
            tmp.w = -tmp.w;
        }

        return tmp;
    }
}
public struct float32bits
{
    public float rawFloat;

    public uint mantissa { get; set; } = 32;
    public uint biased_exponent { get; set; } = 8;
    public uint sign { get; set; } = 1;

    public float32bits()
    {
    }
}

public struct float16bits
{
    public ushort rawWord { get; set; }

    public ushort mantissa { get; set; } = 10;
    public ushort biased_exponent { get; set; } = 5;
    public ushort sign { get; set; } = 1;

    public float16bits()
    {
    }
}

public class float16
{
    protected float16bits m_storage;

    public void Init()
    {
        m_storage.rawWord = 0;
    }

    public ushort GetFloat()
    {
        return Convert16bitFloatTo32bits(m_storage.rawWord);
    }

    public void SetFloat(float fIn)
    {
        m_storage.rawWord = ConvertFloatTo16bits(fIn);
    }

    public bool IsInfinity()
    {
        return m_storage.biased_exponent == 31 && m_storage.mantissa == 0;
    }

    public bool IsNaN()
    {
        return m_storage.biased_exponent == 31 && m_storage.mantissa != 0;
    }

    public static bool operator ==(float16 in1, float16 in2)
    {
        return in1.m_storage.rawWord == in2.m_storage.rawWord;
    }

    public static bool operator !=(float16 in1, float16 in2)
    {
        return in1.m_storage.rawWord != in2.m_storage.rawWord;
    }

    protected static bool IsNaN(float16bits fIn)
    {
        return fIn.biased_exponent == 31 && fIn.mantissa != 0;
    }

    protected static bool IsInfinity(float16bits fIn)
    {
        return fIn.biased_exponent == 31 && fIn.mantissa == 0;
    }

    protected static ushort ConvertFloatTo16bits(float input)
    {
        if (input > compressed_vector.maxfloat16bits)
        {
            input = compressed_vector.maxfloat16bits;
        }
        else if (input < -compressed_vector.maxfloat16bits)
        {
            input = -compressed_vector.maxfloat16bits;
        }

        float16bits output;
        float32bits inFloat = new();

        inFloat.rawFloat = input;

        output.sign = (ushort)inFloat.sign;

        if ((inFloat.biased_exponent == 0) && (inFloat.mantissa == 0))
        {
            output.mantissa = 0;
            output.biased_exponent = 0;
        }
        else if ((inFloat.biased_exponent == 0) && (inFloat.mantissa != 0))
        {
            output.mantissa = 0;
            output.biased_exponent = 0;
        }
        else if ((inFloat.biased_exponent == 0xff) && (inFloat.mantissa == 0))
        {
            output.mantissa = 0x3ff;
            output.biased_exponent = 0x1e;
        }
        else if ((inFloat.biased_exponent == 0xff) && (inFloat.mantissa != 0))
        {
            output.mantissa = 0;
            output.biased_exponent = 0;
        }
        else
        {
            int new_exp = (int)inFloat.biased_exponent - 127;

            if (new_exp < -24)
            {
                output.mantissa = 0;
                output.biased_exponent = 0;
            }

            if (new_exp < -14)
            {
                output.biased_exponent = 0;

                uint exp_val = (uint)(-14 - (inFloat.biased_exponent - compressed_vector.float32bias));

                if (exp_val > 0 && exp_val < 11)
                {
                    output.mantissa = (ushort)((1 << (int)(10 - exp_val)) + ((int)inFloat.mantissa >> (int)(13 + exp_val)));
                }
            }
            else if (new_exp > 15)
            {
                output.biased_exponent = 0x3ff;
                output.mantissa = 0x1e;
            }
            else
            {

                output.biased_exponent = (ushort)(new_exp + 15);
                output.mantissa = (ushort)(inFloat.mantissa >> 13);
            }
        }

        return output.rawWord;
    }

    public static float Convert16bitFloatTo32bits(ushort input)
    {
        float32bits output = new();
        float16bits inFloat = new();
        inFloat.rawWord = input;

        if (IsInfinity(inFloat))
        {
            return compressed_vector.maxfloat16bits * ((inFloat.sign == 1) ? -1.0f : 1.0f);
        }

        if (IsNaN(inFloat))
        {
            return 0.0f;
        }

        if (inFloat.biased_exponent == 0 && inFloat.mantissa != 0)
        {
            float half_denom = (1.0f / 16384.0f);
            float mantissa = ((float)(inFloat.mantissa)) / 1024.0f;
            float sgn = (inFloat.sign != 0) ? -1.0f : 1.0f;
            output.rawFloat = sgn * mantissa * half_denom;
        }
        else
        {
            uint mantissa = inFloat.mantissa;
            uint biased_exponent = inFloat.biased_exponent;
            uint sign = ((uint)inFloat.sign) << 31;
            biased_exponent = (uint)((biased_exponent - compressed_vector.float16bias + compressed_vector.float32bias) * ((biased_exponent != 0) ? 1 : 0)) << 23;
            mantissa <<= (23 - 10);

            output.mantissa = mantissa;
            output.biased_exponent = biased_exponent;
            output.sign = sign;
        }

        return output.rawFloat;
    }
}

public class float16_with_assign : float16
{
    public float16_with_assign()
    {

    }

    public float16_with_assign(float f)
    {
        m_storage.rawWord = ConvertFloatTo16bits(f);
    }

    public float16 Equals(float16 other)
    {
        m_storage.rawWord = ((float16_with_assign)other).m_storage.rawWord;
        return this;
    }

    public float16 Equals(float other)
    {
        m_storage.rawWord = ConvertFloatTo16bits(other);
        return this;
    }

    public float _float16_with_assign()
    {
        return Convert16bitFloatTo32bits(m_storage.rawWord);
    }
}

public class Vector48
{
    public float16 x;
    public float16 y;
    public float16 z;

    public Vector48()
    {

    }

    public Vector48(float X, float Y, float Z)
    {
        x.SetFloat(X);
        y.SetFloat(Y);
        z.SetFloat(Z);
    }

    public Vector48 Equals(Vector vOther)
    {
        vector.CHECK_VALID(vOther);

        x.SetFloat(vOther.x);
        y.SetFloat(vOther.y);
        z.SetFloat(vOther.z);
        return this;
    }

    public Vector _vector()
    {
        Vector tmp = new();

        tmp.x = x.GetFloat();
        tmp.y = y.GetFloat();
        tmp.z = z.GetFloat();

        return tmp;
    }

    public float this[int i]
    {
        get
        {
            switch (i)
            {
                case 1:
                    return x.GetFloat();

                case 2:
                    return y.GetFloat();

                case 3:
                    return z.GetFloat();
                
                default:
                    return 0.0f;
            }
        }
    }
}

public class Vector2d32
{
    public float16_with_assign x;
    public float16_with_assign y;

    public Vector2d32()
    {

    }

    public Vector2d32(float X, float Y)
    {
        x.SetFloat(X);
        y.SetFloat(Y);
    }

    public Vector2d32 Equals(Vector vOther)
    {
        x.SetFloat(vOther.x);
        y.SetFloat(vOther.y);
        return this;
    }

    public Vector2d32 Equals(Vector2D vOther)
    {
        x.SetFloat(vOther.x);
        y.SetFloat(vOther.y);
        return this;
    }

    public Vector2D _vector2d()
    {
        Vector2D tmp = new();

        tmp.x = x.GetFloat();
        tmp.y = y.GetFloat();

        return tmp;
    }

    public void Init(float ix = 0.0f, float iy = 0.0f)
    {
        x.SetFloat(ix);
        y.SetFloat(iy);
    }
}