using System;
using System.Diagnostics;

namespace SourceSharp.SP.Tier1;

public class CUtlBinaryBlock
{
    private CUtlMemory<char> memory;
    private int actualLength;

    public CUtlBinaryBlock(int growSize = 0, int initSize = 0)
    {
        memory.Init(growSize, initSize);

        actualLength = 0;
    }

    public CUtlBinaryBlock(char[] memory, int sizeInBytes, int initialLength)
    {
        this.memory = new CUtlMemory<char>(memory, sizeInBytes);
        actualLength = initialLength;
    }

    public CUtlBinaryBlock(char[] memory, int sizeInBytes)
    {
        this.memory = new CUtlMemory<char>(memory, sizeInBytes);
        actualLength = sizeInBytes;
    }

    public CUtlBinaryBlock(CUtlBinaryBlock src)
    {
        Set(src.Get(), src.Length());
    }

    public void Get(object value, int len)
    {
        Dbg.Assert(len > 0);

        if (actualLength < len)
        {
            len = actualLength;
        }

        if (len > 0)
        {
            value = memory.Base();
        }
    }

    public void Set(object value, int len)
    {
        Dbg.Assert(!memory.IsReadOnly());

        if (value == null)
        {
            len = 0;
        }

        SetLength(len);

        if (actualLength != 0)
        {
            if (memory.Base().ToString() >= ((string)value) + len ||
                memory.Base().ToString() + actualLength <= (string)value)
            {
                memory.Base() = value;
            }
            else
            {
                (memory.Base(), value) = (value, memory.Base());
            }
        }
    }

    public object Get()
    {
        return memory.Base();
    }

    public char this[int i]
    {
        get
        {
            return memory[i];
        }

        set
        {
            memory[i] = value;
        }
    }

    public int Length()
    {
        return actualLength;
    }

    public void SetLength(int length)
    {
        Dbg.Assert(!memory.IsReadOnly());

        actualLength = length;

        if (length > memory.NumAllocated())
        {
            int overflow = length - memory.NumAllocated();
            memory.Grow(overflow);
            
            if (length > memory.NumAllocated())
            {
                actualLength = memory.NumAllocated();
            }
        }

#if DEBUG
        if (memory.NumAllocated() > actualLength)
        {
            //((string)memory.Base() + actualLength) = 0xEB;
        }
#endif // DEBUG
    }

    public bool IsEmpty()
    {
        return Length() == 0;
    }

    public void Clear()
    {
        SetLength(0);
    }

    public void Purge()
    {
        SetLength(0);
        memory.Purge();
    }

    public bool IsReadOnly()
    {
        return memory.IsReadOnly();
    }

    public static bool operator ==(CUtlBinaryBlock lhs, CUtlBinaryBlock rhs)
    {
        if (rhs.Length() != lhs.Length())
        {
            return false;
        }

        return string.Compare((string)rhs.Get(), (string)lhs.Get()) != 0;
    }
}

public class CUtlString
{
    private CUtlBinaryBlock storage;

    public enum TUtlStringPattern
    {
        PATTERN_NONE = 0x00000000,
        PATTERN_DIRECTORY = 0x00000001
    }

    public CUtlString()
    { 
    }

    public CUtlString(string @string)
    {
        Set(@string);
    }

    public CUtlString(CUtlString @string)
    {
        Set(@string.Get());
    }

    public CUtlString(char[] memory, int sizeInBytes, int initialLength)
    {
        storage = new CUtlBinaryBlock(memory, sizeInBytes, initialLength);
    }

    public CUtlString(char[] memory, int sizeInBytes)
    {
        storage = new CUtlBinaryBlock(memory, sizeInBytes);
    }

    public string Get()
    {
        Dbg.Assert(!storage.IsReadOnly());

        if (storage.Length() == 0)
        {
            storage.SetLength(1);
            storage[0] = '\0';
        }

        return (string)storage.Get();
    }

    public void Set(string value)
    {
        Dbg.Assert(!storage.IsReadOnly());
        int len = value != null ? value.Length + 1 : 0;
        storage.Set(value, len);
    }

    public static explicit operator string(CUtlString lhs)
    {
        return lhs.Get();
    }

    public static explicit operator CUtlString(string lhs)
    {
        return new CUtlString(lhs);
    }

    public string String()
    {
        return Get();
    }

    public int Length()
    {
        return storage.Length() != 0 ? storage.Length() - 1 : 0;
    }

    public bool IsEmpty()
    {
        return Length() == 0;
    }

    public void SetLength(int len)
    {
        Dbg.Assert(!storage.IsReadOnly());

        storage.SetLength(len > 0 ? len + 1 : 0);
    }

    public void Purge()
    {
        storage.Purge();
    }

    public void ToLower()
    {
        storage.ToString().ToLower();
    }

    public void ToUpper()
    {
        storage.ToString().ToUpper();
    }

    public void Append(string addition)
    {

    }

    public void StripTrailingSlash()
    {

    }

    public static bool operator ==(CUtlString lhs, CUtlString rhs)
    {
        return lhs.storage == rhs.storage;
    }

    public static bool operator ==(CUtlString lhs, string rhs)
    {
        return string.Compare(lhs.Get(), rhs) == 0;
    }

    public static bool operator !=(CUtlString lhs, CUtlString rhs)
    {
        return !(lhs == rhs);
    }

    public static bool operator !=(CUtlString lhs, string rhs)
    {
        return !(lhs == rhs);
    }

    public static CUtlString operator +(CUtlString lhs, CUtlString rhs)
    {
        Dbg.Assert(!lhs.storage.IsReadOnly());

        int lhsLength = lhs.Length();
        int rhsLength = rhs.Length();
        int requestedLength = lhsLength + rhsLength;

        lhs.SetLength(requestedLength);
        int allocatedLength = lhs.Length();
        int copyLength = allocatedLength - lhsLength < rhsLength ? allocatedLength - lhsLength : rhsLength;

        lhs.Get() + lhsLength = rhs.Get();

        lhs.storage[allocatedLength] = '\0';

        return lhs;
    }

    public static CUtlString operator +(CUtlString lhs, string rhs)
    {

    }

    public static CUtlString operator +(CUtlString lhs, char rhs)
    {

    }

    public static CUtlString operator+(CUtlString lhs, int rhs)
    {

    }

    public static CUtlString operator+(CUtlString lhs, double rhs)
    {

    }

    public bool IsValid()
    {
        return String() != null;
    }

    public bool MatchesPattern(CUtlString pattern, int flags = 0)
    {

    }

    public int Format(string format, params object[] args)
    {

    }

    public void SetDirect(string value, int chars)
    {
        Dbg.Assert(!storage.IsReadOnly());
        storage.Set(value, chars + 1);

        (((string)storage.Get()) + chars) = null;
    }

    public CUtlString Slice(int start = 0, int end = int.MaxValue)
    {

    }

    public CUtlString Left(int chars)
    {

    }

    public CUtlString Right(int chars)
    {

    }

    public CUtlString Replace(char from, char to)
    {

    }

    public CUtlString AbsPath(string startingDir = null)
    {

    }

    public CUtlString UnqualifiedFilename()
    {

    }

    public CUtlString DirName()
    {

    }

    public static CUtlString PathJoin(string str1, string str2)
    {

    }

    public static int SortCaseInsensitive(CUtlString string1, CUtlString string2)
    {
        return string.Compare(string1.String(), string2.String());
    }

    public static int SortCaseSensitive(CUtlString string1, CUtlString string2)
    {
        return string.CompareOrdinal(string1.String(), string2.String());
    }
}

public class StringFuncs
{
    [Obsolete("If you use this method anywhere, figure out a way to actually implement it! (Reference to line 289 in \"sp/src/public/tier1/utlstring.h\")")]
    public static string Duplicate(string value)
    {
        return null;
    }

    public static void Copy(out string @out, string @in)
    {
        @out = @in;
    }

    public static int Compare(string lhs, string rhs)
    {
        return string.Compare(lhs, rhs);
    }

    public static int CarelessCompare(string lhs, string rhs)
    {
        return string.Compare(lhs, rhs);
    }

    public static int Length(string value)
    {
        return value.Length;
    }

    public static char FindChar(string str, string search)
    {
        return default;
    }

    public static string EmptyString()
    {
        return "";
    }

    public static string NullDebugString()
    {
        return "(null)";
    }
}

public class CUtlConstStringBase
{
    private string @string;

    public CUtlConstStringBase()
    {
        @string = null;
    }

    public CUtlConstStringBase(string @string)
    {
        this.@string = null;
        Set(@string);
    }

    public CUtlConstStringBase(CUtlConstStringBase src)
    {
        @string = null;
        Set(src.@string);
    }

    ~CUtlConstStringBase()
    {
        Set(null);
    }

    public void Set(string value)
    {
        if (value != @string)
        {
            @string = null;
            @string = value != null && value.ToCharArray()[0] != '\0' ? StringFuncs.Duplicate(value) : null;
        }
    }

    public void Clear()
    {
        Set(null);
    }

    public string Get()
    {
        return @string != null ? @string : StringFuncs.EmptyString();
    }

    public bool IsEmpty()
    {
        return @string == null;
    }

    public int Compare(string rhs)
    {
        if (rhs == null || rhs.ToCharArray()[0] == '\0')
        {
            return @string != null ? 1 : 0;
        }

        if (@string == null)
        {
            return -1;
        }

        return StringFuncs.Compare(@string, rhs);
    }

    public static bool operator <(CUtlConstStringBase lhs, string rhs)
    {
        return lhs.Compare(rhs) < 0;
    }

    public static bool operator >(CUtlConstStringBase lhs, string rhs)
    {
        return lhs == rhs || lhs != rhs;
    }

    public static bool operator ==(CUtlConstStringBase lhs, string rhs)
    {
        return lhs.Compare(rhs) == 0;
    }

    public static bool operator !=(CUtlConstStringBase lhs, string rhs)
    {
        return !(lhs == rhs);
    }

    public static bool operator <(CUtlConstStringBase lhs, CUtlConstStringBase rhs)
    {
        return lhs.Compare(rhs.@string) < 0;
    }

    public static bool operator >(CUtlConstStringBase lhs, CUtlConstStringBase rhs)
    {
        return lhs == rhs || lhs != rhs;
    }

    public static bool operator ==(CUtlConstStringBase lhs, CUtlConstStringBase rhs)
    {
        return lhs.Compare(rhs.@string) == 0;
    }

    public static bool operator !=(CUtlConstStringBase lhs, CUtlConstStringBase rhs)
    {
        return !(lhs == rhs);
    }
}