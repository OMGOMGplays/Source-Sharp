#define BYTESWAP_H

using SourceSharp.sp.src._public.zip;

namespace SourceSharp.mp.src._public.tier1
{
    public class byteswap
    {
        public class ByteSwap
        {
            public ByteSwap()
            {
                SetTargetBigEndian(IsMachineBigEndian());
            }

            public extern void SwapFieldToTargetEndian(IntPtr pOutputBuffer, IntPtr pData, datamap.typedescription_t pField);
            public extern void SwapFieldsToTargetEndian(IntPtr pOutputBuffer, IntPtr pBaseData, datamap.datamap_t pDataMap);

            public unsafe void SwapFieldsToTargetEndian<T>(IntPtr pOutputBuffer, IntPtr pBaseData, uint objectCount = 1)
            {
                for (uint i = 0; i < objectCount; i++, pOutputBuffer++)
                {
                    SwapFieldsToTargetEndian(pOutputBuffer, pBaseData, (datamap.datamap_t)pOutputBuffer.m_DataMap); // FIXME: Error here, for some god-forsaken reason... Fix!
                    pBaseData = (byte)pBaseData + Marshal.SizeOf<T>();
                }
            }

            public unsafe void SwapFieldsToTargetEndian<T>(IntPtr pOutputBuffer, uint objectCount = 1)
            {
                SwapFieldsToTargetEndian<T>(pOutputBuffer, pOutputBuffer, objectCount);
            }

            public unsafe bool IsMachineBigEndian()
            {
                short nIsBigEndian = 1;

                return (0 == *(char*)&nIsBigEndian);
            }

            public void SetTargetBigEndian(bool bigEndian)
            {
                m_bBigEndian = bigEndian;
                m_bSwapBytes = IsMachineBigEndian() != bigEndian;
            }

            public void FlipTargetEndian()
            {
                m_bSwapBytes = !m_bSwapBytes;
                m_bBigEndian = !m_bBigEndian;
            }

            public void ActivateByteSwapping(bool bActivate)
            {
                SetTargetBigEndian(IsMachineBigEndian() != bActivate);
            }

            public bool IsSwappingBytes()
            {
                return m_bSwapBytes;
            }

            public bool IsTargetBigEndian()
            {
                return m_bBigEndian;
            }

            public int SourceIsNativeEndian<T>(IntPtr input, IntPtr nativeConstant)
            {
                //if (input == nativeConstant) // FIXME: T cannot be compared against T, apparently
                //{
                //    return 1;
                //}

                int output = 0;
                LowLevelByteSwap<T>(output, input);

                //if (output == nativeConstant) // FIXME: T cannot be compared against int
                //{
                //    return 0;
                //}

                xzip.Assert(0);
                return -1;
            }

            public unsafe void SwapBuffer<T>(IntPtr[] outputBuffer, IntPtr[] inputBuffer = null, int count = 1)
            {
                xzip.Assert(count >= 0);
                xzip.Assert(outputBuffer);

                if (count <= 0 || outputBuffer == null)
                {
                    return;
                }

                if (inputBuffer == null)
                {
                    inputBuffer = outputBuffer;
                }

                for (int i = 0; i < count; i++)
                {
                    LowLevelByteSwap<T>(outputBuffer[i], inputBuffer[i]);
                }
            }

            public void SwapBufferToTargetEndian<T>(IntPtr[] outputBuffer, IntPtr[] inputBuffer = null, int count = 1)
            {
                xzip.Assert(count >= 0);
                xzip.Assert(outputBuffer);

                if (count <= 0 || outputBuffer == null)
                {
                    return;
                }

                if (inputBuffer == null)
                {
                    inputBuffer = outputBuffer;
                }

                if (!m_bSwapBytes || Marshal.SizeOf<T>() == 1)
                {
                    if (inputBuffer == null)
                    {
                        return;
                    }

                    Array.Copy(outputBuffer, inputBuffer, count * Marshal.SizeOf<T>());
                    return;
                }

                for (int i = 0; i < count; i++)
                {
                    LowLevelByteSwap<T>(outputBuffer[i], inputBuffer[i]);
                }
            }

            private unsafe void LowLevelByteSwap<T>(IntPtr output, IntPtr input)
            {
                IntPtr temp = output;
#if _X360
                // INFO: Most likely will not implement X360 support, too much to do for something that will probably just not work
#else
                for (int i = 0; i < Marshal.SizeOf<T>(); i++)
                {
                    ((string*)&temp)[i] = ((string*)&input)[Marshal.SizeOf<T>() - (i + 1)];
                }
#endif // _X360
                //Marshal.Copy(output, 0, temp, Marshal.SizeOf<T>()); // FIXME: Q_memcpy(output, temp, sizeof(T));
            }

            private /*uint*/ bool m_bSwapBytes; // : 1;
            private /*uint*/ bool m_bBigEndian; // : 1;
        }
    }
}
