namespace Tier1;

public class bitbuf
{
    public delegate void BitBufErrorHandler(BitBufErrorType errorType, string pDebugName);

    public const int kMaxVarintBytes = 10;
    public const int kMaxVarint32Bytes = 5;

    public static BitBufErrorHandler g_BitBufErrorHandler = null;

    public static ulong[] g_LittleBits = new ulong[32];

    public static ulong[,] g_BitWriteMasks = new ulong[32, 33];

    public static ulong[] g_ExtraMasks = new ulong[33];

    public static CBitWriteMasksInit g_BitWriteMasksInit;

#if DEBUG
    private static void InternalBitBufErrorHandler(BitBufErrorType errorType, string pDebugName)
    {
        if (g_BitBufErrorHandler != null)
        {
            g_BitBufErrorHandler(errorType, pDebugName);
        }
    }

    public static void CallErrorHandler(BitBufErrorType errorType, string pDebugName)
    {
        InternalBitBufErrorHandler(errorType, pDebugName);
    }
#endif // DEBUG

    public static void SetBitBufErrorHandler(BitBufErrorHandler fn)
    {
        g_BitBufErrorHandler = fn;
    }

    public static int BitByte(int bits)
    {
        return (bits + 7) >> 3;
    }

    public static uint ZigZagEncode32(int n)
    {
        return (uint)((n << 1) ^ (n >> 31));
    }

    public static int ZigZagDecode32(uint n)
    {
        return (int)((n >> 1) ^ (int)(n & 1));
    }

    public static ulong ZigZagEncode64(long n)
    {
        return (ulong)((n << 1) ^ (n >> 63));
    }

    public static long ZigZagDecode64(ulong n)
    {
        return (long)((n >> 1) ^ (n & 1));
    }

    public static uint CountLeadingZeros(uint x)
    {
        ulong firstBit = 0;

        if (intrin0._BitScanReserve(firstBit, x))
        {
            return 31 - (uint)firstBit;
        }

        return 32;
    }

    public static uint CountTrailingZeros(uint elem)
    {
        ulong _out = 0;

        if (intrin0._BitScanForward(_out, elem))
        {
            return (uint)_out;
        }

        return 32;
    }

    public static int BitForBitnum(int bitnum)
    {
        return GetBitForBitnum(bitnum);
    }
}

public enum BitBufErrorType
{
    BITBUFERROR_VALUE_OUT_OF_RANGE = 0,
    BITBUFERROR_BUFFER_OVERRUN,

    BITBUFERROR_NUM_ERRORS
}

public class bf_write
{
    public ulong[] m_pData;
    public int m_nDataBytes;
    public int m_nDataBits;

    public int m_iCurBit;

    private bool m_bOverflow;

    private bool m_bAssertOnOverflow;
    private string m_pDebugName;

    public bf_write()
    {
        m_pData = null;
        m_nDataBytes = 0;
        m_nDataBits = -1;
        m_iCurBit = 0;
        m_bOverflow = false;
        m_bAssertOnOverflow = true;
        m_pDebugName = null;
    }

    public bf_write(object pData, int nBytes, int nBits)
    {
        m_bAssertOnOverflow = true;
        m_pDebugName = null;
        StartWriting(pData, nBytes, 0, nBits);
    }

    public bf_write(string pDebugName, object pData, int nBytes, int nBits)
    {
        m_bAssertOnOverflow = true;
        m_pDebugName = pDebugName;
        StartWriting(pData, nBytes, 0, nBits);
    }

    public void StartWriting(object pData, int nBytes, int iStartBit = 0, int nBits = -1)
    {
        Debug.Assert((nBytes % 4) == 0);
        Debug.Assert(((ulong)pData & 3) == 0);

        nBytes &= ~3;

        m_pData = (ulong[])pData;
        m_nDataBytes = nBytes;

        if (nBits == -1)
        {
            m_nDataBits = nBytes << 3;
        }
        else
        {
            Debug.Assert(nBits <= nBytes * 8);
            m_nDataBits = nBits;
        }

        m_iCurBit = 0;
        m_bOverflow = false;
    }

    public void Reset()
    {
        m_iCurBit = 0;
        m_bOverflow = false;
    }

    public string GetBasePointer()
    {
        return m_pData.ToString();
    }

    public void SetAssertOnOverflow(bool bAssert)
    {
        m_bAssertOnOverflow = bAssert;
    }

    public string GetDebugName()
    {
        return m_pDebugName;
    }

    public void SetDebugName(string pDebugName)
    {
        m_pDebugName = pDebugName;
    }

    public void SeekToBit(int bitPos)
    {
        m_iCurBit = bitPos;
    }

    public void WriteOneBit(int nValue)
    {
        if (m_iCurBit >= m_nDataBits)
        {
            SetOverflowFlag();
            bitbuf.CallErrorHandler(BitBufErrorType.BITBUFERROR_BUFFER_OVERRUN, GetDebugName());
            return;
        }

        WriteOneBitNoCheck(nValue);
    }

    public void WriteOneBitNoCheck(int nValue)
    {
        if (nValue != 0)
        {
            m_pData[m_iCurBit >> 5] |= 1u << (m_iCurBit & 31);
        }
        else
        {
            m_pData[m_iCurBit >> 5] &= ~(1u << (m_iCurBit & 31));
        }

        ++m_iCurBit;
    }

    public void WriteOneBitAt(int iBit, int nValue)
    {
        if (iBit >= m_nDataBits)
        {
            SetOverflowFlag();
            bitbuf.CallErrorHandler(BitBufErrorType.BITBUFERROR_BUFFER_OVERRUN, GetDebugName());
            return;
        }

        if (nValue != 0)
        {
            m_pData[iBit >> 5] |= 1u << (iBit & 31);
        }
        else
        {
            m_pData[iBit >> 5] &= ~(1u << (iBit & 31));
        }
    }

    public void WriteUBitLong(uint curData, int numbits, bool bCheckRange = true)
    {
#if DEBUG
        if (bCheckRange && numbits < 32)
        {
            if (curData >= (ulong)(1 << numbits))
            {
                bitbuf.CallErrorHandler(BitBufErrorType.BITBUFERROR_VALUE_OUT_OF_RANGE, GetDebugName());
            }
        }
        Debug.Assert(numbits >= 0 && numbits <= 32);
#endif // DEBUG

        if (GetNumBitsLeft() < numbits)
        {
            m_iCurBit = m_nDataBits;
            SetOverflowFlag();
            bitbuf.CallErrorHandler(BitBufErrorType.BITBUFERROR_BUFFER_OVERRUN, GetDebugName());
            return;
        }

        int iCurBitMasked = m_iCurBit & 31;
        int iDWord = m_iCurBit >> 5;
        m_iCurBit += numbits;

        Debug.Assert((iDWord * 4 + sizeof(long)) <= (uint)m_nDataBytes);
        ulong pOut = m_pData[iDWord];

        curData = (curData << iCurBitMasked) | (curData >> (32 - iCurBitMasked));

        uint temp = (uint)(1 << (numbits - 1));
        uint mask1 = (temp * 2 - 1) << iCurBitMasked;
        uint mask2 = (temp - 1) >> (31 - iCurBitMasked);

        int i = (int)mask2 & 1;
        ulong dword1 = platform.LoadLittleDWord(pOut, 0);
        ulong dword2 = platform.LoadLittleDWord(pOut, i);

        dword1 ^= (mask1 & (curData ^ dword1));
        dword2 ^= (mask2 & (curData ^ dword2));

        platform.StoreLittleDWord(pOut, i, dword2);
        platform.StoreLittleDWord(pOut, 0, dword1);
    }

    public void WriteSBitLong(int data, int numbits)
    {
        int nValue = data;
        int nPreservedBits = (0x7FFFFFFF >> (32 - numbits));
        int nSignExtension = (nValue >> 31) & ~nPreservedBits;
        nValue &= nPreservedBits;
        nValue |= nSignExtension;

        Debug.Assert(nValue == data, string.Format("WriteSBitLong: {0} does not fit in {1} bits", data, numbits));

        WriteUBitLong((uint)nValue, numbits, false);
    }

    public void WriteBitLong(uint data, int numbits, bool bSigned)
    {
        if (bSigned)
        {
            WriteSBitLong((int)data, numbits);
        }
        else
        {
            WriteUBitLong(data, numbits);
        }
    }

    public bool WriteBits(object pInData, int nBits)
    {
        string pOut = (string)pInData;
        int nBitsLeft = nBits;

        if ((m_iCurBit + nBits) > m_nDataBits)
        {
            SetOverflowFlag();
            bitbuf.CallErrorHandler(BitBufErrorType.BITBUFERROR_BUFFER_OVERRUN, GetDebugName());
            return false;
        }

        while (((ulong)pOut & 3) != 0 && nBitsLeft >= 8)
        {
            WriteUBitLong((uint)pOut, 8, false);
            pOut += 1;
            nBitsLeft -= 8;
        }

        if (platform.IsPC() && (nBitsLeft >= 32) && (m_iCurBit & 7) == 0)
        {
            int numbytes = nBitsLeft >> 3;
            int numbits = numbytes << 3;

            m_pData.ToString() + (m_iCurBit >> 3) = pOut;
            pOut += numbytes;
            nBitsLeft -= numbits;
            m_iCurBit += numbits;
        }

        if (platform.IsPC() && nBitsLeft >= 32)
        {
            ulong iBitsRight = (ulong)(m_iCurBit & 31);
            ulong iBitsLeft = 32 - iBitsRight;
            ulong bitMaskLeft = bitbuf.g_BitWriteMasks[iBitsRight, 32];
            ulong bitMaskRight = bitbuf.g_BitWriteMasks[0, iBitsRight];

            ulong pData = m_pData[m_iCurBit >> 5];

            while (nBitsLeft >= 32)
            {
                ulong curData = (ulong)pOut;
                pOut += sizeof(ulong);

                pData &= bitMaskLeft;
                pData |= (ulong)((int)curData << (int)iBitsRight);

                pData++;

                if (iBitsLeft < 32)
                {
                    curData = (ulong)((int)curData >> (int)iBitsLeft);
                    pData &= bitMaskRight;
                    pData |= curData;
                }

                nBitsLeft -= 32;
                m_iCurBit += 32;
            }
        }

        while (nBitsLeft >= 8)
        {
            WriteUBitLong((uint)pOut, 8, false);
            pOut += 1;
            nBitsLeft -= 8;
        }

        if (nBitsLeft != 0)
        {
            WriteUBitLong((uint)pOut, nBitsLeft, false);
        }

        return !IsOverflowed();
    }

    public void WriteUBitVar(uint data)
    {
        int n = (data < 0x10u ? -1 : 0) + (data < 0x100u ? -1 : 0) + (data < 0x1000u ? -1 : 0);
        WriteUBitLong((uint)(data * 4 + n + 3), 6 + n * 4 + 12);

        if (data >= 0x1000u)
        {
            WriteUBitLong(data >> 16, 16);
        }
    }

    public void WriteVarInt32(uint data)
    {
        if ((m_iCurBit & 7) == 0 && (m_iCurBit + bitbuf.kMaxVarint32Bytes * 8) <= m_nDataBits)
        {
            int byteOffset = m_iCurBit >> 3;
            ushort[] target = new ushort[m_pData.Length * sizeof(ulong)];
            Buffer.BlockCopy(m_pData, 0, target, 0, target.Length);

            target[0] = (ushort)(data | 0x80);

            if (data >= (1 << 7))
            {
                target[1] = (ushort)((data >> 7) | 0x80);

                if (data >= (1 << 14))
                {
                    target[2] = (ushort)((data >> 14) | 0x80);

                    if (data >= (1 << 21))
                    {
                        target[3] = (ushort)((data >> 21) | 0x80);

                        if (data >= (1 << 28))
                        {
                            target[4] = (ushort)(data >> 28);
                            m_iCurBit += 5 * 8;
                            return;
                        }
                        else
                        {
                            target[3] &= 0x7F;
                            m_iCurBit += 4 * 8;
                            return;
                        }
                    }
                    else
                    {
                        target[2] &= 0x7F;
                        m_iCurBit += 3 * 8;
                        return;
                    }
                }
                else
                {
                    target[1] &= 0x7F;
                    m_iCurBit += 2 * 8;
                    return;
                }
            }
            else
            {
                target[0] &= 0x7F;
                m_iCurBit += 1 * 8;
                return;
            }
        }
        else
        {
            while (data > 0x7F)
            {
                WriteUBitLong((data & 0x7F) | 0x80, 8);
                data >>= 7;
            }

            WriteUBitLong(data & 0x7F, 8);
        }
    }

    public void WriteVarInt64(ulong data)
    {
        if ((m_iCurBit & 7) == 0 && (m_iCurBit + bitbuf.kMaxVarintBytes * 8) <= m_nDataBits)
        {
            int byteOffset = m_iCurBit >> 3;
            ushort[] target = new ushort[m_pData.Length * sizeof(ulong)];
            Buffer.BlockCopy(m_pData, 0, target, 0, target.Length);

            uint part0 = (uint)data;
            uint part1 = (uint)(data >> 28);
            uint part2 = (uint)(data >> 56);

            int size;

            if (part2 == 0)
            {
                if (part1 == 0)
                {
                    if (part0 < (1 << 14))
                    {
                        if (part0 < (1 << 7))
                        {
                            size = 1;
                            goto size1;
                        }
                        else
                        {
                            size = 2;
                            goto size2;
                        }
                    }
                    else
                    {
                        if (part0 < (1 << 21))
                        {
                            size = 3;
                            goto size3;
                        }
                        else
                        {
                            size = 4;
                            goto size4;
                        }
                    }
                }
                else
                {
                    if (part1 < (1 << 14))
                    {
                        if (part1 < (1 << 7))
                        {
                            size = 5;
                            goto size5;
                        }
                        else
                        {
                            size = 6;
                            goto size6;
                        }
                    }
                    else
                    {
                        if (part1 < (1 << 21))
                        {
                            size = 7;
                            goto size7;
                        }
                        else
                        {
                            size = 8;
                            goto size8;
                        }
                    }
                }
            }
            else
            {
                if (part2 < (1 << 7))
                {
                    size = 9;
                    goto size9;
                }
                else
                {
                    size = 10;
                    goto size10;
                }
            }

            Debug.Assert(false, "Can't get here.");

        size10: target[9] = (ushort)((part2 >> 7) | 0x80);
        size9: target[8] = (ushort)((part2) | 0x80);
        size8: target[7] = (ushort)((part1 >> 21) | 0x80);
        size7: target[6] = (ushort)((part1 >> 14) | 0x80);
        size6: target[5] = (ushort)((part1 >> 7) | 0x80);
        size5: target[4] = (ushort)((part1) | 0x80);
        size4: target[3] = (ushort)((part0 >> 21) | 0x80);
        size3: target[2] = (ushort)((part0 >> 14) | 0x80);
        size2: target[1] = (ushort)((part0 >> 7) | 0x80);
        size1: target[0] = (ushort)((part0) | 0x80);

            target[size - 1] &= 0x7F;
            m_iCurBit += size * 8;
        }
        else
        {
            while (data > 0x7F)
            {
                WriteUBitLong((uint)((data & 0x7F) | 0x80), 8);
                data >>= 7;
            }

            WriteUBitLong((uint)(data & 0x7F), 8);
        }
    }

    public void WriteSignedVarInt32(int data)
    {
        WriteVarInt32(bitbuf.ZigZagEncode32(data));
    }

    public void WriteSignedVarInt64(long data)
    {
        WriteVarInt64(bitbuf.ZigZagEncode64(data));
    }

    public int ByteSizeVarInt32(uint data)
    {
        int size = 1;

        while (data > 0x7F)
        {
            size++;
            data >>= 7;
        }

        return size;
    }

    public int ByteSizeVarInt64(ulong data)
    {
        int size = 1;

        while (data > 0x7F)
        {
            size++;
            data >>= 7;
        }

        return size;
    }

    public int ByteSizeSignedVarInt32(int data)
    {
        return ByteSizeVarInt32(bitbuf.ZigZagEncode32(data));
    }

    public int ByteSizeSignedVarInt64(long data)
    {
        return ByteSizeVarInt64(bitbuf.ZigZagEncode64(data));
    }

    public bool WriteBitsFromBuffer(bf_read pIn, int nBits)
    {
        while (nBits > 32)
        {
            WriteUBitLong(pIn.ReadUBitLong(32), 32);
            nBits -= 32;
        }

        WriteUBitLong(pIn.ReadUBitLong(nBits), nBits);
        return !IsOverflowed() && !pIn.IsOverflowed();
    }

    public void WriteBitAngle(float fAngle, int numbits)
    {
        int d;
        uint mask;
        uint shift;

        shift = (uint)bitbuf.BitForBitnum(numbits);
        mask = shift - 1;

        d = (int)((fAngle / 360.0) * shift);
        d &= (int)mask;

        WriteUBitLong((uint)d, numbits);
    }

    public void WriteBitCoord(float f)
    {
        int signbit = (f <= -coordsize.COORD_RESOLUTION ? 1 : 0);
        int intval = (int)abs(f);
        int fractval = abs((int)(f * coordsize.COORD_DENOMINATOR)) & (coordsize.COORD_DENOMINATOR - 1);

        WriteOneBit(intval);
        WriteOneBit(fractval);

        if (intval != 0 || fractval != 0)
        {
            WriteOneBit(signbit);

            if (intval != 0)
            {
                intval--;
                WriteUBitLong((uint)intval, coordsize.COORD_INTEGER_BITS);
            }

            if (fractval != 0)
            {
                WriteUBitLong((uint)fractval, coordsize.COORD_FRACTIONAL_BITS);
            }
        }
    }

    public void WriteBitCoordMP(float f, bool bIntegral, bool bLowPrecision)
    {
        int signbit = (f <= -(bLowPrecision ? coordsize.COORD_RESOLUTION_LOWPRECISION : coordsize.COORD_RESOLUTION));
        int intval = (int)abs(f);
        int fractval = bLowPrecision ?
            (abs((int)(f * coordsize.COORD_DENOMINATOR_LOWPRECISION)) & (coordsize.COORD_DENOMINATOR_LOWPRECISION - 1)) :
            (abs((int)(f * coordsize.COORD_DENOMINATOR)) & (coordsize.COORD_DENOMINATOR - 1));

        bool bInBounds = intval < (1 << coordsize.COORD_INTEGER_BITS_MP);

        uint bits, numbits;

        if (bIntegral)
        {
            if (intval != 0)
            {
                --intval;
                bits = (uint)(intval * 8 + signbit * 4 + 2 + (bInBounds ? 1 : 0));
                numbits = 3 + (bInBounds ? coordsize.COORD_INTEGER_BITS_MP : coordsize.COORD_INTEGER_BITS);
            }
            else
            {
                bits = (uint)(bInBounds ? 1 : 0);
                numbits = 2;
            }
        }
        else
        {
            if (intval != 0)
            {
                --intval;
                bits = (uint)(intval * 8 + signbit * 4 + 2 + (bInBounds ? 1 : 0));
                bits += bInBounds ? (fractval << (3 + coordsize.COORD_INTEGER_BITS_MP)) : (fractval << (3 + coordsize.COORD_INTEGER_BITS));
                numbits = 3 + (bInBounds ? coordsize.COORD_INTEGER_BITS_MP : coordsize.COORD_INTEGER_BITS)
                            + (bLowPrecision ? coordsize.COORD_FRACTIONAL_BITS_MP_LOWPRECISION : coordsize.COORD_FRACTIONAL_BITS);
            }
            else
            {
                bits = (uint)(fractval * 8 + signbit * 4 + 0 + (bInBounds ? 1 : 0));
                numbits = 3 + (bLowPrecision ? coordsize.COORD_FRACTIONAL_BITS_MP_LOWPRECISION : coordsize.COORD_FRACTIONAL_BITS);
            }
        }

        WriteUBitLong(bits, (int)numbits);
    }

    public void WriteBitFloat(float val)
    {
        long intVal;

        Debug.Assert(sizeof(long) == sizeof(float));
        Debug.Assert(sizeof(float) == 4);

        intVal = ((long)val);
        WriteUBitLong((uint)intVal, 32);
    }

    public void WriteBitVec3Coord(Vector fa)
    {
        int xFlag, yFlag, zFlag;

        xFlag = (fa[0] >= coordsize.COORD_RESOLUTION) || (fa[0] <= -coordsize.COORD_RESOLUTION);
        yFlag = (fa[1] >= coordsize.COORD_RESOLUTION) || (fa[1] <= -coordsize.COORD_RESOLUTION);
        zFlag = (fa[2] >= coordisze.COORD_RESOLUTION) || (fa[2] <= -coordsize.COORD_RESOLUTION);

        WriteOneBit(xFlag);
        WriteOneBit(yFlag);
        WriteOneBit(zFlag);

        if (xFlag != 0)
        {
            WriteBitCoord(fa[0]);
        }

        if (yFlag != 0)
        {
            WriteBitCoord(fa[1]);
        }

        if (zFlag != 0)
        {
            WriteBitCoord(fa[2]);
        }
    }

    public void WriteBitNormal(float f)
    {
        int signbit = (f <= -coordsize.NORMAL_RESOLUTION);

        uint fractval = abs((int)(f * coordsize.NORMAL_RESOLUTION));

        if (fractval > coordsize.NORMAL_DENOMINATOR)
        {
            fractval = coordsize.NORMAL_DENOMINATOR;
        }

        WriteOneBit(signbit);

        WriteUBitLong(fractval, coordsize.NORMAL_FRACTIONAL_BITS);
    }

    public void WriteBitVector3Normal(Vector fa)
    {
        int xFlag, yFlag;

        xFlag = (fa[0] >= coordsize.NORMAL_RESOLUTION) || (fa[0] <= -coordsize.NORMAL_RESOLUTION);
        yFlag = (fa[1] >= coordsize.NORMAL_RESOLUTION) || (fa[1] <= -coordsize.NORMAL_RESOLUTION);

        WriteOneBit(xFlag);
        WriteOneBit(yFlag);

        if (xFlag != 0)
        {
            WriteBitNormal(fa[0]);
        }

        if (yFlag != 0)
        {
            WriteBitNormal(fa[1]);
        }

        int signbit = (fa[2] <= -coordsize.NORMAL_RESOLUTION);
        WriteOneBit(signbit);
    }

    public void WriteBitAngles(QAngle fa)
    {
        Vector tmp = new(fa.x, fa.y, fa.z);
        WriteBitVec3Coord(tmp);
    }

    public void WriteChar(int val)
    {
        WriteSBitLong(val, sizeof(char) << 3);
    }

    public void WriteByte(int val)
    {
        WriteUBitLong((uint)val, sizeof(uint) << 3);
    }

    public void WriteShort(int val)
    {
        WriteSBitLong(val, sizeof(short) << 3);
    }

    public void WriteWord(int val)
    {
        WriteUBitLong((uint)val, sizeof(ushort) << 3);
    }

    public void WriteLong(long val)
    {
        WriteSBitLong(uint val, sizeof(long) << 3);
    }

    public void WriteLongLong(long val)
    {
        uint pLongs = (uint)val;

        short endianIndex = 0x0100;
        byte idx = (byte)endianIndex;
        WriteUBitLong(pLongs[idx++], sizeof(long) << 3);
        WriteUBitLong(pLongs[idx], sizeof(long) << 3);
    }

    public void WriteFloat(float val)
    {
        platform.LittleFloat(val, val);

        WriteBits(val, Marshal.SizeOf(val) << 3);
    }

    public bool WriteBytes(object pBuf, int nBytes)
    {
        return WriteBits(pBuf, nBytes << 3);
    }

    public bool WriteString(string pStr)
    {
        if (pStr != null)
        {
            do
            {
                WriteChar(pStr);
                pStr += 1;
            } while ((pStr - 1) != 0);
        }
        else
        {
            WriteChar(0);
        }

        return !IsOverflowed();
    }

    public int GetNumBytesWritten()
    {
        return bitbuf.BitByte(m_iCurBit);
    }

    public int GetNumBitsWritten()
    {
        return m_iCurBit;
    }

    public int GetMaxNumBits()
    {
        return m_nDataBits;
    }

    public int GetNumBitsLeft()
    {
        return m_nDataBits - m_iCurBit;
    }

    public int GetNumBytesLeft()
    {
        return GetNumBitsLeft() >> 3;
    }

    public string GetData()
    {
        return m_pData.ToString();
    }

    public bool CheckForOverflow(int nBits)
    {
        if (m_iCurBit + nBits > m_nDataBits)
        {
            SetOverflowFlag();
            bitbuf.CallErrorHandler(BitBufErrorType.BITBUFERROR_BUFFER_OVERRUN, GetDebugName());
        }

        return m_bOverflow;
    }

    public bool IsOverflowed()
    {
        return m_bOverflow;
    }

    public void SetOverflowFlag()
    {
        m_bOverflow = true;
    }
}

public class old_bf_write_static : bf_write
{
    public static int SIZE { get; set; }
    public static string m_StaticData;

    public old_bf_write_static() : base(m_StaticData, SIZE)
    {

    }
}

public class bf_read
{
    public string m_pData;
    public int m_nDataBytes;
    public int m_nDataBits;

    public int m_iCurBit;

    private bool m_bOverflow;

    private bool m_bAssertOnOverflow;

    private string m_pDebugName;

    public bf_read()
    {
        m_pData = null;
        m_nDataBytes = 0;
        m_nDataBits = -1;
        m_iCurBit = 0;
        m_bOverflow = false;
        m_bAssertOnOverflow = true;
        m_pDebugName = null;
    }

    public bf_read(object pData, int nBytes, int nBits = -1)
    {
        m_bAssertOnOverflow = true;
        StartReading(pData, nBytes, 0, nBits);
    }

    public bf_read(string pDebugName, object pData, int nBytes, int nBits = -1)
    {
        m_bAssertOnOverflow = true;
        m_pDebugName = pDebugName;
        StartReading(pData, nBytes, 0, nBits);
    }

    public void StartReading(object pData, int nBytes, int iStartBit = 0, int nBits = -1)
    {
        Debug.Assert(((uint)pData & 3) == 0);

        m_pData = (string)pData;
        m_nDataBytes = nBytes;

        if (nBits == -1)
        {
            m_nDataBits = m_nDataBytes << 3;
        }
        else
        {
            Debug.Assert(nBits <= nBytes * 8);
            m_nDataBits = nBits;
        }

        m_iCurBit = iStartBit;
        m_bOverflow = false;
    }

    public void Reset()
    {
        m_iCurBit = 0;
        m_bOverflow = false;
    }

    public void SetAssertOnOverflow(bool bAssert)
    {
        m_bAssertOnOverflow = bAssert;
    }

    public string GetDebugName()
    {
        return m_pDebugName;
    }

    public void SetDebugName(string pName)
    {
        m_pDebugName = pName;
    }

    public void ExciseBits(int startbit, int bitstoremove)
    {
        int endbit = startbit + bitstoremove;
        int remaining_to_end = m_nDataBits - endbit;

        bf_write temp;
        temp.StartWriting(m_pData, m_nDataBits << 3, startbit);
    }

    public int ReadOneBit()
    {
        if (GetNumBitsLeft() <= 0)
        {
            SetOverflowFlag();
            bitbuf.CallErrorHandler(BitBufErrorType.BITBUFERROR_BUFFER_OVERRUN, GetDebugName());
            return 0;
        }

        return ReadOneBitNoCheck();
    }

    protected uint CheckReadUBitLong(int numbits)
    {
        int i, nBitValue;
        uint r = 0;

        for (i = 0; i < numbits; i++)
        {
            nBitValue = ReadOneBitNoCheck();
            r |= (uint)nBitValue << i;
        }

        m_iCurBit -= numbits;

        return r;
    }

    protected int ReadOneBitNoCheck()
    {
        uint value = (uint)m_pData[m_iCurBit >> 5] >> (m_iCurBit & 31);

        ++m_iCurBit;
        return (int)value & 1;
    }

    protected bool CheckForOverflow(int nBits)
    {
        if (m_iCurBit + nBits > m_nDataBits)
        {
            SetOverflowFlag();
            bitbuf.CallErrorHandler(BitBufErrorType.BITBUFERROR_BUFFER_OVERRUN, GetDebugName());
        }

        return m_bOverflow;
    }

    public string GetBasePointer()
    {
        return m_pData.ToString();
    }

    public int TotalBytesAvailable()
    {
        return m_nDataBytes;
    }

    public void ReadBits(object pOutData, int nBits)
    {
        string pOut = (string)pOutData;
        int nBitsLeft = nBits;

        while (((uint)pOut & 3) != 0 && nBitsLeft >= 8)
        {
            pOut = ReadUBitLong(8).ToString();
            pOut += 1;
            nBitsLeft -= 8;
        }

        if (platform.IsPC())
        {
            while (nBitsLeft >= 32)
            {
                pOut = ReadUBitLong(32).ToString();
                pOut += sizeof(ulong);
                nBitsLeft -= 32;
            }
        }

        while (nBitsLeft >= 8)
        {
            pOut = ReadUBitLong(8).ToString();
            pOut += 1;
            nBitsLeft -= 8;
        }

        if (nBitsLeft != 0)
        {
            pOut = ReadUBitLong(nBitsLeft).ToString();
        }
    }

    public int ReadBitsClamped_ptr(object pOutData, uint outSizeBytes, uint nBits)
    {
        uint outSizeBits = outSizeBytes * 8;
        uint readSizeBits = nBits;
        int skippedBits = 0;

        if (readSizeBits > outSizeBits)
        {
            Debug.Assert(false, "Oversized network packet received, and clamped.");
            readSizeBits = outSizeBits;
            skippedBits = (int)(nBits - outSizeBits);
        }

        ReadBits(pOutData, (int)readSizeBits);
        SeekRelative(skippedBits);

        return (int)readSizeBits;
    }

    public int ReadBitsClamped<T>(T pOut, uint nBits, uint N)
    {
        return ReadBitsClamped_ptr(pOut, N, nBits);
    }

    public float ReadBitAngle(int numbits)
    {
        float fReturn;
        int i;
        float shift;

        shift = (float)(bitbuf.BitForBitnum(numbits));

        i = ReadUBitLong(numbits);
        fReturn = (float)i * (360.0f / shift);

        return fReturn;
    }

    public uint ReadUBitLong(int numbits)
    {
        Debug.Assert(numbits > 0 && numbits <= 32);

        if (GetNumBitsLeft() < numbits)
        {
            m_iCurBit = m_nDataBits;
            SetOverflowFlag();
            bitbuf.CallErrorHandler(BitBufErrorType.BITBUFERROR_BUFFER_OVERRUN, GetDebugName());
            return 0;
        }

        uint iStartBit = (uint)m_iCurBit & 31u;
        int iLastBit = m_iCurBit + numbits - 1;
        uint iWordOffset1 = (uint)m_iCurBit >> 5;
        uint iWordOffset2 = (uint)iLastBit >> 5;
        m_iCurBit += numbits;

        uint bitmask = (uint)(2 << (numbits - 1)) - 1;

        uint dw1 = platform.LoadLittleDWord((ulong)m_pData, iWordOffset1) >> iStartBit;
        uint dw2 = platform.LoadLittleDWord((ulong)m_pData, iWordOffset2) << (32 - iStartBit);

        return (dw1 | dw2) & bitmask;
    }

    public uint ReadUBitLongNoInline(int numbits)
    {
        return ReadUBitLong(numbits);
    }

    public uint PeekUBitLong(int numbits)
    {
        uint r;
        int i, nBitValue;

        bf_read savebf;

        savebf = this;

        r = 0;

        for (i = 0; i < numbits; i++)
        {
            nBitValue = ReadOneBit();

            if (nBitValue != 0)
            {
                r |= (uint)bitbuf.BitForBitnum(i);
            }
        }

        this = savebf;

        return r;
    }

    public int ReadSBitLong(int numbits)
    {
        uint r = ReadUBitLong(numbits);
        uint s = 1 << ((uint)numbits - 1);

        if (r >= s)
        {
            r = r - s - s;
        }

        return r;
    }

    public uint ReadUBitVar()
    {
        uint sixbits = ReadUBitLong(6);
        uint encoding = sixbits & 3;

        if (encoding != 0)
        {
            return ReadUBitVarInternal((int)encoding);
        }

        return sixbits >> 2;
    }

    public uint ReadUBitVarInternal(int encodingType)
    {
        m_iCurBit -= 4;
        int bits = 4 + encodingType * 4 + (((2 - encodingType) >> 31) & 16);
        return ReadUBitLong(bits);
    }

    public uint ReadVarInt32()
    {
        uint result = 0;
        int count = 0;
        uint b;

        do
        {
            if (count == bitbuf.kMaxVarint32Bytes)
            {
                return result;
            }

            b = ReadUBitLong(8);
            result |= (b & 0x7F) << (7 * count);
            ++count;
        } while ((b & 0x80) != 0);

        return result;
    }

    public ulong ReadVarInt64()
    {
        ulong result = 0;
        int count = 0;
        ulong b;

        do
        {
            if (count == bitbuf.kMaxVarintBytes)
            {
                return result;
            }

            b = ReadUBitLong(8);
            result |= (ulong)(b & 0x7F) << (7 * count);
            ++count;
        } while ((b & 0x80) != 0);

        return result;
    }

    public int ReadSignedVarInt32()
    {
        uint value = ReadVarInt32();
        return bitbuf.ZigZagDecode32(value);
    }

    public long ReadSignedVarInt64()
    {
        ulong value = ReadVarInt64();
        return bitbuf.ZigZagDecode64(value);
    }

    public uint ReadBitLong(int numbits, bool bSigned)
    {
        if (bSigned)
        {
            return (uint)ReadSBitLong(numbits);
        }
        else
        {
            return ReadUBitLong(numbits);
        }
    }

    public float ReadBitCoord()
    {
        int intval = 0, fractval = 0, signbit = 0;
        float value = 0.0f;

        intval = ReadOneBit();
        fractval = ReadOneBit();

        if (intval != 0 || fractval != 0)
        {
            signbit = ReadOneBit();

            if (intval != 0)
            {
                intval = ReadUBitLong(coordsize.COORD_INTEGER_BITS) + 1;
            }

            if (fractval != 0)
            {
                fractval = ReadUBitLong(coordsize.COORD_FRACTIONAL_BITS);
            }

            value = intval + ((float)fractval * coordsize.COORD_RESOLUTION);

            if (signbit != 0)
            {
                value = -value;
            }
        }

        return value;
    }

    enum RBCMP_enum
    {
        INBOUNDS = 1,
        INTVAL = 2,
        SIGN = 4,
    }

    public float ReadBitCoordMP(bool bIntegral, bool bLowPrecision)
    {
        int flags = (int)ReadUBitLong(3 - (bIntegral ? 1 : 0));

        if (bIntegral)
        {
            if ((flags & (int)RBCMP_enum.INTVAL) != 0)
            {
                uint bits = ReadUBitLong((flags & (int)RBCMP_enum.INBOUNDS) != 0 ? coordsize.COORD_INTEGER_BITS_MP + 1 : coordsize.COORD_INTEGER_BITS + 1);
                int intval = ((int)bits >> 1) + 1;
                return (bits & 1) != 0 ? -intval : intval;
            }

            return 0.0f;
        }

        float[] mul_table =
        {
            1.0f/(1<<coordsize.COORD_FRACTIONAL_BITS),
            -1.0f/(1<<coordsize.COORD_FRACTIONAL_BITS),
            1.0f/(1<<coordsize.COORD_FRACTIONAL_BITS_MP_LOWPRECISION),
            -1.0f/(1<<coordsize.COORD_FRACTIONAL_BITS_MP_LOWPRECISION)
        };

        float multiply = (float)((uint)mul_table[0] + (flags & 4) + (bLowPrecision ? 1 : 0) * 8);

        int[] numbits_table =
        {
            coordsize.COORD_FRACTIONAL_BITS,
            coordsize.COORD_FRACTIONAL_BITS,
            coordsize.COORD_FRACTIONAL_BITS + coordsize.COORD_INTEGER_BITS,
            coordsize.COORD_FRACTIONAL_BITS + coordsize.COORD_INTEGER_BITS_MP,
            coordsize.COORD_FRACTIONAL_BITS_MP_LOWPRECISION,
            coordsize.COORD_FRACTIONAL_BITS_MP_LOWPRECISION,
            coordsize.COORD_FRACTIONAL_BITS_MP_LOWPRECISION + coordsize.COORD_INTEGER_BITS,
            coordsize.COORD_FRACTIONAL_BITS_MP_LOWPRECISION + coordsize.COORD_INTEGER_BITS_MP,
        }

        uint bits = ReadUBitLong(numbits_table[(flags & ((int)RBCMP_enum.INBOUNDS | (int)RBCMP_enum.INTVAL)) + (bLowPrecision ? 1 : 0) * 4]);

        if ((flags & (int)RBCMP_enum.INTVAL) != 0)
        {
            uint fracbitsMP = bits >> coordsize.COORD_INTEGER_BITS_MP;
            uint fracbits = bits >> coordsize.COORD_INTEGER_BITS;

            uint intmaskMP = ((1 << coordsize.COORD_INTEGER_BITS_MP) - 1);
            uint intmask = ((1 << coordsize.COORD_INTEGER_BITS) - 1);

            uint selectNotMP = (uint)(flags & (int)RBCMP_enum.INBOUNDS) - 1;

            fracbits -= fracbitsMP;
            fracbits &= selectNotMP;
            fracbits += fracbitsMP;

            intmask -= intmaskMP;
            intmask &= selectNotMP;
            intmask += intmaskMP;

            uint intpart = (bits & intmask) + 1;
            uint intbitsLow = intpart << coordsize.COORD_FRACTIONAL_BITS_MP_LOWPRECISION;
            uint intbits = intpart << coordsize.COORD_FRACTIONAL_BITS;
            uint selectNotLow = (uint)(bLowPrecision ? 1 : 0) - 1;

            intbits -= intbitsLow;
            intbits &= selectNotLow;
            intbits += intbitsLow;

            bits = fracbits | intbits;
        }

        return (int)bits * multiply;
    }

    struct RBF_union
    {
        public uint u;
        public float f;
    }

    public float ReadBitFloat()
    {
        RBF_union c = new();
        c.f = ReadUBitLong(32);
        return c.f;
    }

    public float ReadBitNormal()
    {
        int signbit = ReadOneBit();

        uint fracval = ReadUBitLong(coordsize.NORMAL_FRACTIONAL_BITS);

        float value = (float)fracval * coordsize.NORMAL_RESOLUTION;

        if (signbit != 0)
        {
            value = -value;
        }

        return value;
    }

    public void ReadBitVec3Coord(Vector fa)
    {
        int xFlag, yFlag, zFlag;

        fa.Init(0, 0, 0);

        xFlag = ReadOneBit();
        yFlag = ReadOneBit();
        zFlag = ReadOneBit();

        if (xFlag != 0)
        {
            fa[0] = ReadBitCoord();
        }

        if (yFlag != 0)
        {
            fa[1] = ReadBitCoord();
        }

        if (zFlag != 0)
        {
            fa[2] = ReadBitCoord();
        }
    }

    public void ReadBitVec3Normal(Vector fa)
    {
        int xFlag = ReadOneBit();
        int yFlag = ReadOneBit();

        if (xFlag != 0)
        {
            fa[0] = ReadBitNormal();
        }
        else
        {
            fa[0] = 0.0f;
        }

        if (yFlag != 0)
        {
            fa[1] = ReadBitNormal();
        }
        else
        {
            fa[1] = 0.0f;
        }

        int znegative = ReadOneBit();

        float fafafbfb = fa[0] * fa[0] + fa[1] * fa[1];

        if (fafafbfb < 1.0f)
        {
            fa[2] = sqrt(1.0f - fafafbfb);
        }
        else
        {
            fa[2] = 0.0f;
        }

        if (znegative != 0)
        {
            fa[2] = -fa[2];
        }
    }

    public void ReadBitAngles(out QAngle fa)
    {
        Vector tmp;
        ReadBitVec3Coord(tmp);
        fa.Init(tmp.x, tmp.y, tmp.z);
    }

    public uint ReadBitCoordBits()
    {
        uint flags = ReadUBitLong(2);

        if (flags == 0)
        {
            return 0;
        }

        int[] numbits_table =
        {
            coordsize.COORD_INTEGER_BITS + 1,
            coordsize.COORD_FRACTIONAL_BITS + 1,
            coordsize.COORD_INTEGER_BITS + coordsize.COORD_FRACTIONAL_BITS + 1
        };

        return ReadUBitLong(numbits_table[flags - 1]) * 4 + flags;
    }

    enum RBCMPB_enum
    {
        INBOUNDS = 1,
        INTVAL = 2,
    }

    public uint ReadBitCoordMPBits(bool bIntegral, bool bLowPrecision)
    {
        uint flags = ReadUBitLong(2);
        int numbits = 0;

        if (bIntegral)
        {
            if ((flags & (int)RBCMPB_enum.INTVAL) != 0)
            {
                numbits = (flags & (int)RBCMPB_enum.INBOUNDS) != 0 ? (1 + coordsize.COORD_INTEGER_BITS_MP) : (1 + coordsize.COORD_INTEGER_BITS);
            }
            else
            {
                return flags;
            }
        }
        else
        {
            int[] numbits_table =
            {
                1 + coordsize.COORD_FRACTIONAL_BITS,
                1 + coordsize.COORD_FRACTIONAL_BITS,
                1 + coordsize.COORD_FRACTIONAL_BITS + coordsize.COORD_INTEGER_BITS,
                1 + coordsize.COORD_FRACTIONAL_BITS + coordsize.COORD_INTEGER_BITS_MP,
                1 + coordsize.COORD_FRACTIONAL_BITS_MP_LOWPRECISION,
                1 + coordsize.COORD_FRACTIONAL_BITS_MP_LOWPRECISION,
                1 + coordsize.COORD_FRACTIONAL_BITS_MP_LOWPRECISION + coordsize.COORD_INTEGER_BITS,
                1 + coordsize.COORD_FRACTIONAL_BITS_MP_LOWPRECISION + coordsize.COORD_INTEGER_BITS_MP,
            };

            numbits = numbits_table[flags + (bLowPrecision ? 1 : 0) * 4];
        }

        return flags + ReadUBitLong(numbits) * 4;
    }

    public int ReadChar()
    {
        return (char)ReadUBitLong(8);
    }

    public int ReadByte()
    {
        return (int)ReadUBitLong(8);
    }

    public int ReadShort()
    {
        return (short)ReadUBitLong(16);
    }

    public int ReadWord()
    {
        return (int)ReadUBitLong(16);
    }

    public long ReadLong()
    {
        return (long)ReadUBitLong(32);
    }

    public long ReadLongLong()
    {
        long retval;
        uint pLongs = (uint)retval;

        short endianIndex = 0x0100;
        byte idx = (byte)endianIndex;
        pLongs[idx++] = ReadUBitLong(sizeof(long) << 3);
        pLongs[idx] = ReadUBitLong(sizeof(long) << 3);

        return retval;
    }

    public float ReadFloat()
    {
        float ret = 0.0f;
        Debug.Assert(Marshal.SizeOf(ret) == 4);
        ReadBits(ret, 32);

        platform.LittleFloat(ret, ret);
        return ret;
    }

    public bool ReadBytes(object pOut, int nBytes)
    {
        ReadBits(pOut, nBytes << 3);
        return !IsOverflowed();
    }

    public bool ReadString(string pStr, int maxLen, bool bLine = false, int pOutNumChars = 0)
    {
        Debug.Assert(maxLen != 0);

        bool bTooSmall = false;
        int iChar = 0;

        while (true)
        {
            char val = ReadChar();

            if (val == 0)
            {
                break;
            }
            else if (bLine && val == '\n')
            {
                break;
            }

            if (iChar < (maxLen - 1))
            {
                pStr[iChar] = val;
                ++iChar;
            }
            else
            {
                bTooSmall = true;
            }
        }

        Debug.Assert(iChar < maxLen);
        pStr[iChar] = 0;

        if (pOutNumChars != 0)
        {
            pOutNumChars = iChar;
        }

        return !IsOverflowed() && !bTooSmall;
    }

    public string ReadAndAllocateString(bool pOverflow = false)
    {
        string str = string.Empty;

        int nChars;
        bool bOverflow = !ReadString(str, Marshal.SizeOf(str), false, nChars);

        if (pOverflow)
        {
            pOverflow = bOverflow;
        }

        string pRet = string.Empty;

        for (int i = 0; i <= nChars; i++)
        {
            pRet[i] = str[i];
        }

        return pRet;
    }

    public bool CompareBits(bf_read other, int numbits)
    {
        return ReadUBitLong(numbits) != other.ReadUBitLong(numbits);
    }

    public int CompareBitsAt(int offset, bf_read other, int otherOffset, int bits)
    {

    }

    public int GetNumBytesLeft()
    {
        return GetNumBitsLeft() >> 3;
    }

    public int GetNumBytesRead()
    {
        return bitbuf.BitByte(m_iCurBit);
    }

    public int GetNumBitsLeft()
    {
        return m_nDataBits - m_iCurBit;
    }

    public int GetNumBitsRead()
    {
        return m_iCurBit;
    }

    public bool IsOverflowed()
    {
        return m_bOverflow;
    }

    public bool Seek(int iBit)
    {
        if (iBit < 0 || iBit > m_nDataBits)
        {
            SetOverflowFlag();
            m_iCurBit = m_nDataBits;
            return false;
        }
        else
        {
            m_iCurBit = iBit;
            return true;
        }
    }

    public bool SeekRelative(int iBitDelta)
    {
        return Seek(m_iCurBit + iBitDelta);
    }

    public void SetOverflowFlag()
    {
        if (m_bAssertOnOverflow)
        {
            Debug.Assert(false);
        }

        m_bOverflow = true;
    }
}

public class CBitWriteMasksInit
{
    public CBitWriteMasksInit()
    {
        for (uint startbit = 0; startbit < 32; startbit++)
        {
            for (uint nBitsLeft = 0; nBitsLeft < 33; nBitsLeft++)
            {
                uint endbit = startbit + nBitsLeft;
                bitbuf.g_BitWriteMasks[startbit, nBitsLeft] = (ulong)bitbuf.BitForBitnum((int)startbit) - 1;

                if (endbit < 32)
                {
                    bitbuf.g_BitWriteMasks[startbit, nBitsLeft] |= ~((ulong)bitbuf.BitForBitnum((int)endbit) - 1);
                }
            }
        }

        for (uint maskBit = 0; maskBit < 32; maskBit++)
        {
            bitbuf.g_ExtraMasks[maskBit] = (ulong)bitbuf.BitForBitnum((int)maskBit) - 1;
        }

        bitbuf.g_ExtraMasks[32] = ~0ul;

        for (uint littleBit = 0; littleBit < 32; littleBit++)
        {
            platform.StoreLittleDWord(bitbuf.g_LittleBits[littleBit], 0, (uint)((int)1u << (int)littleBit));
        }
    }
}