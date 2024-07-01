#define BASETYPES_H

namespace SourceSharp.mp.src._public.tier0
{
    using HANDLE_TYPE = dynamic;

    public class basetypes
    {
        //public static void DLLExtTokenPaste(IntPtr x) => x;
        //public static void DLLExtTokenPaste2(IntPtr x) => DLLExtTokenPaste(x);
        //public static void DLL_EXT_STRING() => DLLExtTokenPaste2(_DLL_EXT) // FIXME: Figure out a way to implement these

        public static void ExecuteNTimes(int nTimes, Action x)
        {
            int __executeCount = 0;

            if (__executeCount < nTimes)
            {
                x.Invoke();
                ++__executeCount;
            }
        }

        public static void ExecuteOnce(Action x) => ExecuteNTimes(1, x);

        public static UIntPtr AlignValue<T>(IntPtr val, UIntPtr alignment) // FIXME: Proper implementation awaits!
        {
            return ((UIntPtr)val + alignment - 1) & ~(alignment - 1);
        }

        public static int PAD_NUMBER(int number, int boundary) => (((number) + ((boundary) - 1)) / (boundary)) * (boundary);

        public const double M_PI = 3.14159265358979323846;

        public static float COMPILETIME_MIN(dynamic a, dynamic b) => (((a) < (b)) ? (a) : (b));
        public static float COMPILETIME_MAX(dynamic a, dynamic b) => (((a) > (b)) ? (a) : (b));
        public static float MIN(dynamic a, dynamic b) => (((a) < (b)) ? (a) : (b));
        public static float MAX(dynamic a, dynamic b) => (((a) > (b)) ? (a) : (b));

        public static dynamic Clamp(dynamic val, dynamic minVal, dynamic maxVal)
        {
            if (val < minVal)
            {
                return minVal;
            }
            else if (val > maxVal)
            {
                return maxVal;
            }
            else
            {
                return val;
            }
        }

        public static dynamic Min(dynamic val1, dynamic val2)
        {
            return val1 < val2 ? val1 : val2;
        }

        public static dynamic Max(dynamic val1, dynamic val2)
        {
            return val1 > val2 ? val1 : val2;
        }

        public enum ThreeState_t
        {
            TRS_FALSE,
            TRS_TRUE,
            TRS_NONE,
        }

        public static long FloatBits(vec_t f)
        {
            return (long)f;
        }

        public static vec_t BitsToFloat(ulong i)
        {
            return (vec_t)i;
        }

        public static bool IsFinite(vec_t f)
        {
            return ((FloatBits(f) & 0x7F800000) != 0x7F800000);
        }

        public static float FloatMakeNegative(vec_t f)
        {
            return -MathF.Abs(f);
        }

        public static float FloatMakePositive(vec_t f)
        {
            return MathF.Abs(f);
        }

        public static float FloatNegate(vec_t f)
        {
            return -f;
        }

        public const ulong FLOAT32_NAN_BITS = 0x7FC00000;
        public const float FLOAT32_NAN = /*BitsToFloat(*/FLOAT32_NAN_BITS/*)*/;

        public const float VEC_T_NAN = FLOAT32_NAN;

        public struct color24
        {
            public byte r, g, b;
        }

        public struct color32_s
        {
            public static bool operator ==(color32_s lhs, color32_s rhs)
            {
                return (lhs.r == rhs.r) && (lhs.g == rhs.g) && (lhs.b == rhs.b) && (lhs.a == rhs.a);
            }

            public static bool operator !=(color32_s lhs, color32_s rhs)
            {
                return (lhs.r != rhs.r) && (lhs.g != rhs.g) && (lhs.b != rhs.b) && (lhs.a != rhs.a);
            }

            public byte r, g, b, a;
        }

        public struct colorVec
        {
            public uint r, g, b, a;
        }

        public struct vrect_t
        {
            public int x, y, width, height;
            public static vrect_t pnext;
        }

        public struct Rect_t
        {
            public int x, y;
            public int width, height;
        }

        public struct interval_t
        {
            public float start;
            public float range;
        }

        public class CBaseIntHandle
        {
            public static bool operator ==(CBaseIntHandle lhs, CBaseIntHandle rhs) { return lhs.m_Handle == rhs.m_Handle; }
            public static bool operator !=(CBaseIntHandle lhs, CBaseIntHandle rhs) { return lhs.m_Handle != rhs.m_Handle; }

            public dynamic GetHandleType() { return m_Handle; }
            public void SetHandleValue(dynamic val) { m_Handle = val; }

            protected dynamic m_Handle;
        }

        public class CIntHandle16 : CBaseIntHandle
        {
            public CIntHandle16() { }

            public static CIntHandle16 MakeHandle(HANDLE_TYPE val)
            {
                return new CIntHandle16(val);
            }

            protected CIntHandle16(HANDLE_TYPE val)
            {
                m_Handle = val;
            }
        }

        public class CIntHandle32 : CBaseIntHandle
        {
            public CIntHandle32() { }

            public static CIntHandle32 MakeHandle(HANDLE_TYPE val)
            {
                return new CIntHandle32(val);
            }

            protected CIntHandle32(HANDLE_TYPE val)
            {
                m_Handle = val;
            }
        }
    }
}
