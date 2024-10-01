using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SourceSharp.SP.Mathlib;

namespace SourceSharp.SP.Public.Tier1;

public struct CharacterSet;

public class CUtlCharConversion
{
    protected char escapeChar;
    protected string delimiter;
    protected int delimiterLength;
    protected int count;
    protected int maxConversionLength;
    protected char[] list = new char[256];
    protected ConversionInfo[] replacements = new ConversionInfo[256];

    public struct ConversionArray
    {
        public char actualChar;
        public string[] replacementString;
    }

    public CUtlCharConversion(char escapeChar, string delimiter, int count, ConversionArray array)
    {

    }

    public char GetEscapeChar()
    {

    }

    public string GetDelimiter()
    {

    }

    public int GetDelimiterLength()
    {

    }

    public string GetConversionString(char c)
    {

    }

    public int GetConversionLength(char c)
    {

    }

    public int MaxConversionLength()
    {

    }

    public virtual char FindConversion(string @string, int length)
    {

    }

    protected struct ConversionInfo
    {
        public int length;
        public string replacementString;
    }

    public static CUtlCharConversion GetCStringCharConversion()
    {

    }

    public static CUtlCharConversion GetNoEscCharConversion()
    {

    }
}

public class CUtlBuffer
{
    protected CUtlMemory<char> memory;
    protected int get;
    protected int put;

    protected char error;
    protected BufferFlags flags;
    protected char reserved;
#if X360
    protected char pad;
#endif // X360

    protected int tab;
    protected int maxPut;
    protected int offset;

    protected UtlBufferOverflowFunc getOverflowFunc;
    protected UtlBufferOverflowFunc putOverflowFunc;

    protected CByteSwap byteswap;

    public enum SeekType
    {
        SEEK_HEAD = 0,
        SEEK_CURRENT,
        SEEK_TAIL
    }

    public enum BufferFlags
    {
        TEXT_BUFFER = 0x1,
        EXTERNAL_GROWABLE = 0x2,
        CONTAINS_CRLF = 0x4,
        READ_ONLY = 0x8,
        AUTO_TABS_DISABLED = 0x10
    }

    public delegate bool UtlBufferOverflowFunc(int size);

    public CUtlBuffer(int growSize = 0, int initSize = 0, BufferFlags flags = 0)
    {

    }

    public CUtlBuffer(nint buffer, int size, BufferFlags flags = 0)
    {

    }

    public CUtlBuffer(nint buffer, int size, bool crap)
    {

    }

    public BufferFlags GetFlags()
    {
        return flags;
    }

    public bool IsExternallyAllocated()
    {
        return memory.IsExternallyAllocated();
    }

    public void SetBufferType(bool isText, bool containsCRLF)
    {

    }

    public void EnsureCapacity(int num)
    {

    }

    public void SetExternalBuffer(nint memory, int size, int initialPut, BufferFlags flags)
    {

    }

    public void AssumeMemory(nint memory, int size, int initialPut, BufferFlags flags = 0)
    {

    }

    public void CopyBuffer(CUtlBuffer buffer)
    {
        CopyBuffer(buffer.Base(), buffer.TellPut());
    }

    public void CopyBuffer(nint pubData, int cubData)
    {
        Clear();

        if (cubData != 0)
        {
            Put(pubData, cubData);
        }
    }

    public void Swap(CUtlBuffer buffer)
    {

    }

    public void Swap(CUtlMemory<char> mem)
    {

    }

    public void ActivateByteSwappingIfBigEndian()
    {
        if (IsX360())
        {
            ActivateByteSwapping(true);
        }
    }

    public void ActivateByteSwapping(bool activate)
    {

    }

    public void SetBigEndian(bool bigEndian)
    {

    }

    public bool IsBigEndian()
    {

    }

    public void Clear()
    {
        get = 0;
        put = 0;
        error = '\0';
        offset = 0;
        maxPut = -1;
        AddNullTermination();
    }

    public void Purge()
    {
        get = 0;
        put = 0;
        offset = 0;
        maxPut = 0;
        error = '\0';
        memory.Purge();
    }

    public char GetChar()
    {
        GetType(out char c, "{0}");
        return c;
    }

    public char GetUnsignedChar()
    {
        GetType(out char c, "{0}");
        return c;
    }

    public short GetShort()
    {
        GetType(out short s, "{0}");
        return s;
    }

    public ushort GetUnsignedShort()
    {
        GetType(out ushort s, "{0}");
        return s;
    }

    public int GetInt()
    {
        GetType(out int i, "{0}");
        return i;
    }

    public long GetInt64()
    {
        GetType(out long i, "{0}");
        return i;
    }

    public int GetIntHex()
    {
        GetType(out int i, "{0}");
        return i;
    }

    public uint GetUnsignedInt()
    {
        GetType(out uint u, "{0}");
        return u;
    }

    public float GetFloat()
    {
        GetType(out float f, "{0}");
        return f;
    }

    public double GetDouble()
    {
        GetType(out double d, "{0}");
        return d;
    }

    public void GetString(ref string @string, int maxChars)
    {

    }

    public void Get(ref nint mem, int size)
    {

    }

    public void GetLine(string line, int maxChars = 0)
    {

    }

    public void GetObjects<T>(out T[] dest, int count = 1)
    {
        T[] array = new T[count];

        for (int i = 0; i < count; ++i)
        {
            GetObject(out T item);
            array[i] = item;
        }

        dest = array;
    }

    public int GetUpTo(nint mem, int size)
    {

    }

    public void GetDelimitedString(CUtlCharConversion conv, string @string, int maxChars = 0)
    {

    }

    public char GetDelimitedChar(CUtlCharConversion conv)
    {

    }

    public int PeekStringLength()
    {

    }

    public int PeekDelimitedStringLength(CUtlCharConversion conv, bool actualSize = true)
    {

    }

    public int Scanf(string fmt, params object[] args)
    {

    }

    [Obsolete("Obsolete in C#! Use UtlBuffer.Scanf() instead")]
    public int VaScanf(string fmt, params object[] args)
    {
        throw new NotSupportedException("UtlBuffer.VaScanf(): Obsolete in C#! Use UtlBuffer.Scanf() instead.");
    }

    public void EatWhiteSpace()
    {

    }

    public bool EatCPPComment()
    {

    }

    public bool ParseToken(string startingDelta, string endingDelta, string @string, int maxLen)
    {

    }

    public bool GetToken(string token)
    {

    }

    public int ParseToken(CharacterSet breaks, string tokenBuf, int maxLen, bool parseComments = true)
    {

    }

    public void PutChar(char c)
    {
        if (WasLastCharacterCR())
        {
            PutTabs();
        }

        PutTypeBin(c);
    }

    public void PutUnsignedChar(char c)
    {
        PutType(c, "%u");
    }

    public void PutUint64(ulong ub)
    {
        PutType(ub, "%llu");
    }

    public void PutInt16(short s16)
    {
        PutType(s16, "%d");
    }

    public void PutShort(short s)
    {
        PutType(s, "%d");
    }

    public void PutUnsignedShort(ushort us)
    {
        PutType(us, "%u");
    }

    public void PutInt(int i)
    {
        PutType(i, "%d");
    }

    public void PutInt64(long i)
    {
        PutType(i, "%llu");
    }

    public void PutUnsignedInt(uint u)
    {
        PutType(u, "%u");
    }

    public void PutFloat(float f)
    {
        PutType(f, "%f");
    }

    public void PutDouble(double d)
    {
        PutType(d, "%d");
    }

    public void PutString(string @string)
    {

    }

    public void Put(nint mem, int size)
    {

    }

    public void PutObjects<T>(T src, int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            PutObject(src);
        }
    }

    public void PutDelimitedString(CUtlCharConversion conv, string @string)
    {

    }

    public void PutDelimitedChar(CUtlCharConversion conv, char c)
    {

    }

    public void Printf(string fmt, params object[] args)
    {

    }

    [Obsolete("Obsolete in C#! Use UtlBuffer.Printf() instead.")]
    public void VaPrintf(string fmt, params object[] args)
    {
        throw new NotSupportedException("UtlBuffer.VaPrintf(): Obsolete in C#! Use UtlBuffer.Printf() instead.");
    }

    public nint PeekGet(int offset = 0)
    {

    }

    public nint PeekGet(int maxSize, int offset)
    {
        return memory[get + offset - this.offset];
    }

    public nint PeekPut(int offset = 0)
    {
        return memory[put + offset - this.offset];
    }

    public int TellPut()
    {
        return put;
    }

    public int TellGet()
    {
        return get;
    }

    public int TellMaxPut()
    {
        return maxPut;
    }

    public int GetBytesRemaining()
    {
        return maxPut - TellGet();
    }

    public void SeekPut(SeekType type, int offset)
    {

    }

    public void SeekGet(SeekType type, int offset)
    {

    }

    public nint Base()
    {
        return (nint)memory.Base();
    }

    public string String()
    {
        Debug.Assert(IsText());

        return memory.Base().ToString();
    }

    public int Size()
    {
        return memory.NumAllocated();
    }

    public bool IsText()
    {
        return (flags & BufferFlags.TEXT_BUFFER) != 0;
    }

    public bool IsGrowable()
    {
        return (flags & BufferFlags.EXTERNAL_GROWABLE) != 0;
    }

    public bool IsValid()
    {
        return error == 0;
    }

    public bool ContainsCRLF()
    {
        return IsText() && (flags & BufferFlags.CONTAINS_CRLF) != 0;
    }

    public bool IsReadOnly()
    {
        return (flags & BufferFlags.READ_ONLY) != 0;
    }

    public bool ConvertCRLF(out CUtlBuffer outBuf)
    {

    }

    public void PushTab()
    {
        tab++;
    }

    public void PopTab()
    {
        if (--tab < 0)
        {
            tab = 0;
        }
    }

    public void EnableTabs(bool enable)
    {
        if (enable)
        {
            flags &= ~BufferFlags.AUTO_TABS_DISABLED;
        }
        else
        {
            flags |= BufferFlags.AUTO_TABS_DISABLED;
        }
    }

    protected enum ErrorFlags
    {
        PUT_OVERFLOW = 0x1,
        GET_OVERFLOW = 0x2,
        MAX_ERROR_FLAG = GET_OVERFLOW
    }

    protected static void SetOverflowFuncs(UtlBufferOverflowFunc getFunc, UtlBufferOverflowFunc putFunc)
    {

    }

    protected bool OnPutOverflow(int size)
    {

    }

    protected bool OnGetOverflow(int size)
    {

    }

    protected bool CheckPut(int size)
    {

    }

    protected bool CheckGet(int size)
    {

    }

    protected void AddNullTermination()
    {

    }

    protected bool WasLastCharacterCR()
    {
        if (!IsText() || TellPut() == 0)
        {
            return false;
        }

        return (char)PeekPut(-1) == '\n';
    }

    protected void PutTabs()
    {
        int tabCount = (flags & BufferFlags.AUTO_TABS_DISABLED) != 0 ? 0 : tab;

        for (int i = tabCount; --i >= 0;)
        {
            PutTypeBin('\t');
        }
    }

    protected char GetDelimitedCharInternal(CUtlCharConversion conv)
    {

    }

    protected void PutDelimitedCharInternal(CUtlCharConversion conv, char c)
    {

    }

    protected bool PutOverflow(int size)
    {

    }

    protected bool GetOverflow(int size)
    {

    }

    protected bool PeekStringMatch(int offset, string @string, int len)
    {

    }

    protected int PeekLineLength()
    {

    }

    protected int PeekWhiteSpace(int offset)
    {

    }

    protected bool CheckPeekGet(int offset, int size)
    {

    }

    protected bool CheckArbitraryPeekGet(int offset, int increment)
    {

    }

    protected void GetType<T>(out T dest, string fmt)
    {
        if (!IsText())
        {
            GetTypeBin(out dest);
        }
        else
        {
            dest = default;
            Scanf(fmt, dest);
        }
    }

    protected void GetTypeBin<T>(out T dest)
    {
        if (CheckGet(Marshal.SizeOf<T>()))
        {
            if (!byteswap.IsSwappingBytes() || Marshal.SizeOf<T>() == 1)
            {
                dest = (T)PeekGet();
            }
            else
            {
                byteswap.SwapBufferToTargetEndian<T>(out dest, (T)PeekGet());
            }

            get += Marshal.SizeOf<T>();
        }
        else
        {
            dest = default;
        }
    }

    protected void GetTypeBin(out float dest)
    {
        if (CheckGet(sizeof(float)))
        {
            uint data = (uint)PeekGet();

            if (IsX360() && (data & 0x03) != 0)
            {
                ((char[])dest)[0] = ((char[])data)[0];
                ((char[])dest)[1] = ((char[])data)[1];
                ((char[])dest)[2] = ((char[])data)[2];
                ((char[])dest)[3] = ((char[])data)[3];
            }
            else
            {
                dest = data;
            }

            if (byteswap.IsSwappingBytes())
            {
                byteswap.SwapBufferToTargetEndian(dest, out dest);
            }

            get += sizeof(float);
        }
        else
        {
            dest = 0;
        }
    }

    protected void GetObject<T>(out T dest)
    {
        if (CheckGet(Marshal.SizeOf<T>()))
        {
            if (!byteswap.IsSwappingBytes() || Marshal.SizeOf<T>() == 1)
            {
                dest = (T)PeekGet();
            }
            else
            {
                byteswap.SwapFieldsToTargetEndian<T>(out dest, (T)PeekGet());
            }

            get += Marshal.SizeOf<T>();
        }
        else
        {
            dest = default;
        }
    }

    protected void PutType<T>(T src, string fmt)
    {
        if (!IsText())
        {
            PutTypeBin(src);
        }
        else
        {
            Printf(fmt, src);
        }
    }

    protected void PutTypeBin<T>(T src)
    {
        if (CheckPut(Marshal.SizeOf<T>()))
        {
            if (!byteswap.IsSwappingBytes() || Marshal.SizeOf<T>() == 1)
            {
                (T)PeekPut() = src;
            }
            else
            {
                byteswap.SwapBufferToTargetEndian<T>((T)PeekPut(), src);
            }

            put += Marshal.SizeOf<T>();
            AddNullTermination();
        }
    }

    protected void PutObject<T>(T src)
    {
        if (CheckPut(Marshal.SizeOf<T>()))
        {
            if (!byteswap.IsSwappingBytes() || Marshal.SizeOf<T>() == 1)
            {
                (T)PeekPut() = src;
            }
            else
            {
                byteswap.SwapFieldsToTargetEndian<T>((T)PeekPut(), src);
            }

            put += Marshal.SizeOf<T>();
            AddNullTermination();
        }
    }

    public static CUtlBuffer operator <<(CUtlBuffer lhs, char rhs)
    {
        lhs.PutChar(rhs);
        return lhs;
    }

    public static CUtlBuffer operator <<(CUtlBuffer lhs, short rhs)
    {
        lhs.PutShort(rhs);
        return lhs;
    }

    public static CUtlBuffer operator <<(CUtlBuffer lhs, ushort rhs)
    {
        lhs.PutUnsignedShort(rhs);
        return lhs;
    }

    public static CUtlBuffer operator <<(CUtlBuffer lhs, int rhs)
    {
        lhs.PutInt(rhs);
        return lhs;
    }

    public static CUtlBuffer operator <<(CUtlBuffer lhs, uint rhs)
    {
        lhs.PutUnsignedInt(rhs);
        return lhs;
    }

    public static CUtlBuffer operator <<(CUtlBuffer lhs, float rhs)
    {
        lhs.PutFloat(rhs);
        return lhs;
    }

    public static CUtlBuffer operator <<(CUtlBuffer lhs, double rhs)
    {
        lhs.PutDouble(rhs);
        return lhs;
    }

    public static CUtlBuffer operator <<(CUtlBuffer lhs, string rhs)
    {
        lhs.PutString(rhs);
        return lhs;
    }

    public static CUtlBuffer operator <<(CUtlBuffer lhs, Vector rhs)
    {
        lhs = lhs << rhs.x << " " << rhs.y << " " << rhs.z;
        return lhs;
    }

    public static CUtlBuffer operator <<(CUtlBuffer lhs, Vector2D rhs)
    {
        lhs = lhs << rhs.x << " " << lhs.y;
        return lhs;
    }

    public static void SetUtlBufferOverflowFuncs(dynamic get, dynamic put)
    {
        SetOverflowFuncs((UtlBufferOverflowFunc)get, (UtlBufferOverflowFunc)put);
    }
}

public class CUtlInplaceBuffer : CUtlBuffer
{
    public CUtlInplaceBuffer(int growSize = 0, int initSize = 0, BufferFlags flags = 0)
    {

    }

    public bool InplaceGetLinePtr(out char[] inBuffer, out int lineLength)
    {

    }

    public string InplaceGetLinePtr()
    {

    }
}