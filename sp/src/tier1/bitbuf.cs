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

    public static uint ZigZagEncode32(uint n)
    {
        return (uint)((n >> 1) ^ (int)(n & 1));
    }

    public static ulong ZigZagEncode64(long n)
    {
        return (ulong)((n << 1) ^ (n >> 63));
    }

    public static ulong ZigZagEncode64(ulong n)
    {
        return (ulong)((n >> 1) ^ (n & 1));
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

    public void StartWriting(object pData, int nBytes, int iStartBit, int nBits)
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
            ++pOut;
            nBitsLeft -= 8;
        }
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

    public bool WriteBitsFromBuffer(object pIn, int nBits)
    {

    }

    public void WriteBitAngle(float fAngle, int numBits)
    {

    }

    public void WriteBitCoord(float f)
    {

    }

    public void WriteBitCoordMP(float f, bool bIntegral, bool bLowPrecision)
    {

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

    }

    public void WriteBitNormal(float f)
    {

    }

    public void WriteBitVector3Normal(Vector fa)
    {

    }

    public void WriteBitAngles(QAngle fa)
    {

    }

    public void WriteChar(int val)
    {

    }

    public void WriteByte(int val)
    {

    }

    public void WriteShort(int val)
    {

    }

    public void WriteWord(int val)
    {

    }

    public void WriteLong(long val)
    {

    }

    public void WriteLongLong(long val)
    {

    }

    public void WriteFloat(float val)
    {

    }

    public bool WriteBytes(object pBuf, int nBytes)
    {

    }

    public bool WriteString(string pStr)
    {

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

    }

    public bf_read(object pData, int nBytes, int nBits = -1)
    {

    }

    public bf_read(string pDebugName, object pData, int nBytes, int nBits = -1)
    {

    }

    public void StartReading(object pData, int nBytes, int iStartBit = 0, int nBits = -1)
    {

    }

    public void Reset()
    {

    }

    public void SetAssertOnOverflow(bool bAssert)
    {

    }

    public string GetDebugName()
    {
        return m_pDebugName;
    }

    public void SetDebugName(string pName)
    {

    }

    public void ExciseBits(int startbit, int bitstoremove)
    {

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

    public void ReadBits(object pOut, int nBits)
    {

    }

    public int ReadBitsClamped_ptr(object pOut, uint outSizeBytes, uint nBits)
    {

    }

    public int ReadBitsClamped<T>(T pOut, uint nBits, uint N)
    {
        return ReadBitsClamped_ptr(pOut, N, nBits);
    }

    public float ReadBitAngle(int numbits)
    {

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

    }

    public uint PeekUBitLong(int numbits)
    {

    }

    public int ReadSBitLong(int numbits)
    {

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

    }

    public uint ReadVarInt32()
    {

    }

    public ulong ReadVarInt64()
    {

    }

    public int ReadSignedVarInt32()
    {

    }

    public long ReadSignedVarInt64()
    {

    }

    public uint ReadBitLong(int numbits, bool bSigned)
    {

    }

    public float ReadBitCoord()
    {

    }

    public float ReadBitCoordMP(bool bIntegral, bool bLowPrecision)
    {

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

    }

    public void ReadBitVec3Coord(out Vector fa)
    {

    }

    public void ReadBitVec3Normal(out Vector fa)
    {

    }

    public void ReadBitAngles(out QAngle fa)
    {

    }

    public uint ReadBitCoordBits()
    {

    }

    public uint ReadBitCoordMPBits(bool bIntegral, bool bLowPrecision)
    {

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

    }

    public float ReadFloat()
    {

    }

    public bool ReadBytes(object pOut, int nBytes)
    {

    }

    public bool ReadString(string pStr, int bufLen, bool bLine = false, int pOutNumChars = 0)
    {

    }

    public string ReadAndAllocateString(bool pOverflow = false)
    {

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