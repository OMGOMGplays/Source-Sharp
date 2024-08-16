namespace Tier1;

public class bitbuf
{
    public delegate void BitBufErrorHandler(BitBufErrorType errorType, string pDebugName);

#if DEBUG
    private static void InternalBitBufErrorHandler(BitBufErrorType errorType, string pDebugName)
    {

    }

    public static void CallErrorHandler(BitBufErrorType errorType, string pDebugName)
    {
        InternalBitBufErrorHandler(errorType, pDebugName);
    }
#endif // DEBUG

    public static void SetBitBufErrorHandler(BitBufErrorHandler fn)
    {

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

    public const int kMaxVarintBytes = 10;
    public const int kMaxVarint32Bytes = 5;
}

public enum BitBufErrorType
{
    BITBUFERROR_VALUE_OUT_OF_RANGE = 0,
    BITBUFERROR_BUFFER_OVERRUN,

    BITBUFERROR_NUM_ERRORS
}

public class bf_write
{
    public bf_write()
    {

    }

    public bf_write(object pData, int nBytes, int nMaxBits = -1)
    {

    }

    public bf_write(string pDebugName, object pData, int nBytes, int nMaxBits = -1)
    {

    }

    public void StartWriting(object pData, int nBytes, int iStartBit = 0, int nMaxBits = -1)
    {

    }

    public void Reset()
    {

    }

    public string GetBasePointer()
    {
        return (string)m_pData;
    }

    public void SetAssertOnOverflow(bool bAssert)
    {

    }

    public string GetDebugName()
    {

    }

    public void SetDebugName(string pDebugName)
    {

    }

    public void SeekToBit(int bitPos)
    {

    }

    public void WriteOneBit(int nValue)
    {

    }

    public void WriteOneBitNoCheck(int nValue)
    {

    }

    public void WriteOneBitAt(int iBit, int nValue)
    {

    }

    public void WriteUBitLong(uint data, int numbits, bool bCheckRange = true)
    {

    }

    public void WriteSBitLong(int data, int numbits)
    {

    }

    public void WriteBitLong(uint data, int numbits, bool bSigned)
    {

    }

    public bool WriteBits(object pIn, int nBits)
    {

    }

    public void WriteUBitVar(uint data)
    {

    }

    public void WriteVarInt32(int data)
    {

    }

    public void WriteVarInt64(long data)
    {

    }
}