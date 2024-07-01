#define STUDIOBYTESWAP_H

namespace SourceSharp.sp.src.common
{
    public class studiobyteswap
    {
        // - studiobyteswap.h -

        public delegate bool CompressFunc_t(IntPtr pInput, int inputSize, IntPtr pOutput, int pOutputSize);

        public const int BYTESWAP_ALIGNMENT_PADDING = 4096;

        // - studiobyteswap.cpp -

        public static void ALIGN4(int a) => a = (byte)((int)((byte)a + 3) & ~3);
        public static void ALIGN16(int a) => a = (byte)((int)((byte)a + 15) & ~15);
        public static void ALIGN32(int a) => a = (byte)((int)((byte)a + 31) & ~31);
        public static void ALIGN64(int a) => a = (byte)((int)((byte)a + 63) & ~63);

        //#pragma warning push
#pragma warning disable 4189
#pragma warning disable 4366

        public static bool g_bVerbose = true;
        public static bool g_bNativeSrc = true;
        public static byteswap.CByteSwap
    }
}
