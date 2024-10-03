using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SourceSharp.SP.Tier1;

public class CUtlMemory<T>
{
    protected T[] memory;
    protected int allocationCount;
    protected int growSize;

    protected enum ExternalBufferMarkers
    {
        EXTERNAL_BUFFER_MARKER = -1,
        EXTERNAL_CONST_BUFFER_MARKER = -2
    }

    public CUtlMemory(int growSize = 0, int initAllocationSize = 0)
    {
        memory = [];
        allocationCount = initAllocationSize;

        ValidateGrowSize();
        Debug.Assert(growSize >= 0);

        if (allocationCount != 0)
        {
            memory = new T[allocationCount];
        }
        else
        {
            memory = Array.Empty<T>();
        }
    }

    public CUtlMemory(T[] memory, int numElements)
    {
        this.memory = memory;
        allocationCount = numElements;

        growSize = (int)ExternalBufferMarkers.EXTERNAL_BUFFER_MARKER;
    }

    ~CUtlMemory()
    {
        Purge();
    }

    public void Init(int growSize = 0, int initSize = 0)
    {
        Purge();

        this.growSize = growSize;
        allocationCount = initSize;
        ValidateGrowSize();
        Debug.Assert(growSize >= 0);

        if (allocationCount != null)
        {
            memory = new T[allocationCount * Marshal.SizeOf<T>()];
        }
    }

    public class Iterator
    {
        public int index;

        public Iterator(int i)
        {
            index = i;
        }

        public static bool operator ==(Iterator lhs, Iterator rhs)
        {
            return lhs.index == rhs.index;
        }

        public static bool operator !=(Iterator lhs, Iterator rhs)
        {
            return !(lhs == rhs);
        }
    }

    public Iterator First()
    {
        return new Iterator(IsIdxValid(0) ? 0 : InvalidIndex());
    }

    public Iterator Next(Iterator it)
    {
        return new Iterator(IsIdxValid(it.index + 1) ? it.index + 1 : InvalidIndex());
    }

    public int GetIndex(Iterator it)
    {
        return it.index;
    }

    public bool IsIdxAfter(int i, Iterator it)
    {
        return i > it.index;
    }

    public bool IsValidIterator(Iterator it)
    {
        return IsIdxValid(it.index);
    }

    public Iterator InvalidIterator()
    {
        return new Iterator(InvalidIndex());
    }

    public T this[int i]
    {
        get
        {
            Debug.Assert(growSize != (int)ExternalBufferMarkers.EXTERNAL_CONST_BUFFER_MARKER);
            Debug.Assert((uint)i < (uint)allocationCount);

            return memory[i];
        }

        set
        {
            Debug.Assert(growSize != (int)ExternalBufferMarkers.EXTERNAL_CONST_BUFFER_MARKER);
            Debug.Assert((uint)i < (uint)allocationCount);

            memory[i] = value;
        }
    }

    public T Element(int i)
    {
        Debug.Assert(growSize != (int)ExternalBufferMarkers.EXTERNAL_CONST_BUFFER_MARKER);
        Debug.Assert((uint)i < (uint)allocationCount);

        return memory[i];
    }

    public bool IsIdxValid(int i)
    {
        return (uint)i < (uint)allocationCount;
    }

    public static int INVALID_INDEX => -1;
    public static int InvalidIndex()
    {
        return INVALID_INDEX;
    }

    public T[] Base()
    {
        Debug.Assert(!IsReadOnly());
        return memory;
    }

    public void SetExternalBuffer(T[] memory, int numElements)
    {
        Purge();

        this.memory = memory;
        allocationCount = numElements;

        growSize = (int)ExternalBufferMarkers.EXTERNAL_BUFFER_MARKER;
    }

    public void AssumeMemory(T[] memory, int size)
    {
        Purge();

        this.memory = memory;
        allocationCount = size;
    }

    public void Swap(CUtlMemory<T> other)
    {
        (memory, other.memory) = (other.memory, memory);
        (allocationCount, other.allocationCount) = (other.allocationCount, allocationCount);
        (growSize, other.growSize) = (other.growSize, growSize);
    }

    public void ConvertToGrowableMemory(int growSize)
    {
        if (!IsExternallyAllocated())
        {
            return;
        }

        this.growSize = growSize;

        if (allocationCount != 0)
        {
            int numBytes = allocationCount * Marshal.SizeOf<T>();
            T[] memory = new T[numBytes];
            this.memory = memory;
        }
        else
        {
            memory = null;
        }
    }

    public int NumAllocated()
    {
        return allocationCount;
    }

    public int Count()
    {
        return allocationCount;
    }

    public virtual void Grow(int num = 1)
    {
        Debug.Assert(num > 0);

        if (IsExternallyAllocated())
        {
            Debug.Assert(false);
            return;
        }

        int allocationRequested = allocationCount + num;

        int newAllocationCount = UtlMemory_CalcNewAllocationCount(allocationCount, growSize, allocationRequested, Marshal.SizeOf<T>());

        if (newAllocationCount < allocationRequested)
        {
            if (newAllocationCount == 0 && newAllocationCount - 1 >= allocationRequested)
            {
                --newAllocationCount;
            }
            else
            {
                if (allocationRequested != allocationRequested)
                {
                    Debug.Assert(false);
                    return;
                }

                while (newAllocationCount < allocationRequested)
                {
                    newAllocationCount = (newAllocationCount + allocationRequested) / 2;
                }
            }
        }

        allocationCount = newAllocationCount;

        if (memory != null)
        {
            memory = new T[allocationCount * Marshal.SizeOf<T>()];
            Debug.Assert(memory != null);
        }
        else
        {
            memory = new T[allocationCount * Marshal.SizeOf<T>()];
            Debug.Assert(memory != null);
        }
    }

    public virtual void EnsureCapacity(int num)
    {
        if (allocationCount >= num)
        {
            return;
        }

        if (IsExternallyAllocated())
        {
            Debug.Assert(false);
            return;
        }

        allocationCount = num;

        if (memory != null)
        {
            memory = new T[allocationCount * Marshal.SizeOf<T>()];
        }
    }

    public void Purge()
    {
        if (!IsExternallyAllocated())
        {
            if (memory != null)
            {
                memory = null;
            }

            allocationCount = 0;
        }
    }

    public void Purge(int numElements)
    {
        Debug.Assert(numElements >= 0);

        if (numElements > allocationCount)
        {
            Debug.Assert(numElements <= allocationCount);
            return;
        }

        if (numElements == 0)
        {
            Purge();
            return;
        }

        if (IsExternallyAllocated())
        {
            return;
        }

        if (numElements == allocationCount)
        {
            return;
        }

        if (memory == null)
        {
            Debug.Assert(memory != null);
            return;
        }

        allocationCount = numElements;

        memory = new T[(allocationCount - numElements) * Marshal.SizeOf<T>()];
    }

    public bool IsExternallyAllocated()
    {
        return growSize < 0;
    }

    public bool IsReadOnly()
    {
        return growSize == (int)ExternalBufferMarkers.EXTERNAL_CONST_BUFFER_MARKER;
    }

    public void SetGrowSize(int size)
    {
        Debug.Assert(!IsExternallyAllocated());
        Debug.Assert(size >= 0);

        growSize = size;
        ValidateGrowSize();
    }

    protected void ValidateGrowSize()
    {
#if X360
        if (growSize != 0 && growSize != (int)ExternalBufferMarkers.EXTERNAL_BUFFER_MARKER)
        {
            int MAX_GROW = 128;

            if (growSize * Marshal.SizeOf<T>() > MAX_GROW)
            {
                growSize = Math.Max(1, MAX_GROW / Marshal.SizeOf<T>());
            }
        }
#endif // X360
    }

    public static int UtlMemory_CalcNewAllocationCount(int allocationCount, int growSize, int newSize, int bytesItem)
    {
        if (growSize != 0)
        {
            allocationCount = (1 + ((newSize - 1) / growSize)) * growSize;
        }
        else
        {
            if (allocationCount == 0)
            {
                allocationCount = (31 + bytesItem) / bytesItem;
            }

            while (allocationCount < newSize)
            {
#if !X360
                allocationCount *= 2;
#else
                int newAllocationCount = (allocationCount * 9) / 8;

                if (newAllocationCount > allocationCount)
                {
                    allocationCount = newAllocationCount;
                }
                else
                {
                    allocationCount *= 2;
                }
#endif // !X360
            }
        }

        return allocationCount;
    }
}

public class CUtlMemoryFixedGrowable<T> : CUtlMemory<T>
{
    private int size;
    private int mallocGrowSize;
    private T[] fixedMemory;

    public CUtlMemoryFixedGrowable(int growSize = 0, int initSize = 0)
    {
        Debug.Assert(initSize == 0);
        mallocGrowSize = growSize;
        size = initSize;
    }

    public override void Grow(int count = 1)
    {
        if (IsExternallyAllocated())
        {
            ConvertToGrowableMemory(mallocGrowSize);
        }

        base.Grow(count);
    }

    public override void EnsureCapacity(int num)
    {
        if (allocationCount >= num)
        {
            return;
        }

        if (IsExternallyAllocated())
        {
            ConvertToGrowableMemory(mallocGrowSize);
        }

        base.EnsureCapacity(num);
    }
}

public class CUtlMemoryFixed<T>
{
    private readonly int SIZE;
    private readonly int ALIGNMENT;
    private char[] memory;

    public CUtlMemoryFixed(int growSize = 0, int initSize = 0)
    {
        Debug.Assert(initSize == 0 || initSize == SIZE);
        memory = new char[SIZE * Marshal.SizeOf<T>() + ALIGNMENT];
    }

    public CUtlMemoryFixed(T[] memory, int numElements)
    {
        Debug.Assert(false);
    }

    public bool IsIdxValid(int i)
    {
        return i < SIZE;
    }

    public const int INVALID_INDEX = -1;

    public static int InvalidIndex()
    {
        return INVALID_INDEX;
    }

    public T[] Base()
    {
        if (ALIGNMENT == 0)
        {
            T[] values = new T[memory.Length];

            for (int i = 0; i < memory.Length; i++)
            {
                values[i] = Marshal.PtrToStructure<T>(memory[i]);
            }
        }
        else
        {
            return (T)BaseTypes.AlignValue(memory[0], ALIGNMENT);
        }

        return null;
    }

    public T this[int i]
    {
        get
        {
            Debug.Assert(i < SIZE);

            return Base()[i];
        }

        set
        {
            Debug.Assert(i < SIZE);

            Base()[i] = value;
        }
    }

    public T Element(int i)
    {
        Debug.Assert(i < SIZE);

        return Base()[i];
    }

    public void SetExternalBuffer(T[] memory, int numElements)
    {
        Debug.Assert(false);
    }

    public int NumAllocated()
    {
        return SIZE;
    }

    public int Count()
    {
        return SIZE;
    }

    public void Grow(int num = 1)
    {
        Debug.Assert(false);
    }

    public void EnsureCapacity(int num)
    {
        Debug.Assert(num <= SIZE);
    }

    public void Purge()
    {

    }

    public void Purge(int numElements)
    {
        Debug.Assert(false);
    }

    public bool IsExternallyAllocated()
    {
        return false;
    }

    public void SetGrowSize(int size)
    {

    }

    public class Iterator
    {
        public int index;

        public Iterator(int i)
        {
            index = i;
        }

        public static bool operator ==(Iterator lhs, Iterator rhs)
        {
            return lhs.index == rhs.index;
        }

        public static bool operator !=(Iterator lhs, Iterator rhs)
        {
            return !(lhs == rhs);
        }
    }

    public Iterator First()
    {
        return new Iterator(IsIdxValid(0) ? 0 : InvalidIndex());
    }

    public Iterator Next(Iterator it)
    {
        return new Iterator(IsIdxValid(it.index + 1) ? it.index + 1 : InvalidIndex());
    }

    public int GetIndex(Iterator it)
    {
        return it.index;
    }

    public bool IsIdxAfter(int i, Iterator it)
    {
        return i > it.index;
    }

    public bool IsValidIterator(Iterator it)
    {
        return IsIdxValid(it.index);
    }

    public Iterator InvalidIterator()
    {
        return new Iterator(InvalidIndex());
    }
}

public class CUtlMemoryConservative<T>
{
    private T[] memory;

    public CUtlMemoryConservative(int growSize = 0, int initSize = 0)
    {
        memory = null;
    }

    public CUtlMemoryConservative(T[] memory, int numElements)
    {
        Debug.Assert(false);
    }

    ~CUtlMemoryConservative()
    {
        if (memory != null)
        {
            memory = null;
        }
    }

    public bool IsIdxValid(int i)
    {
        return Platform.IsDebug() ? i >= 0 && i < NumAllocated() : i >= 0;
    }

    public static int InvalidIndex()
    {
        return -1;
    }

    public T[] Base()
    {
        return memory;
    }

    public T this[int i]
    {
        get
        {
            Debug.Assert(IsIdxValid(i));

            return Base()[i];
        }

        set
        {
            Debug.Assert(IsIdxValid(i));

            Base()[i] = value;
        }
    }

    public T Element(int i)
    {
        Debug.Assert(IsIdxValid(i));

        return Base()[i];
    }

    public void SetExternalBuffer(T[] memory, int numElements)
    {
        Debug.Assert(false);
    }

    public void RememberAllocSize(ulong sz)
    {

    }

    public ulong AllocSize()
    {
        return memory != null ? (ulong)Marshal.SizeOf(memory) : 0;
    }

    public int NumAllocated()
    {
        return (int)AllocSize() / Marshal.SizeOf<T>();
    }

    public int Count()
    {
        return NumAllocated();
    }

    public void ReAlloc(ulong sz)
    {
        memory = new T[sz];
        RememberAllocSize(sz);
    }

    public void Grow(int num = 1)
    {
        int curN = NumAllocated();
        ReAlloc((ulong)((curN + num) * Marshal.SizeOf<T>()));
    }

    public void EnsureCapacity(int num)
    {
        ulong size = (ulong)(Marshal.SizeOf<T>() * Math.Max(num, Count()));
    }

    public void Purge()
    {
        memory = null;
        RememberAllocSize(0);
    }

    public void Purge(int numElements)
    {
        ReAlloc((ulong)(numElements * Marshal.SizeOf<T>()));
    }

    public bool IsExternallyAllocated()
    {
        return false;
    }

    public void SetGrowSize(int size)
    {

    }

    public class Iterator
    {
        public int index;
        public int limit;

        public Iterator(int index, int limit)
        {
            this.index = index;
            this.limit = limit;
        }

        public static bool operator ==(Iterator lhs, Iterator rhs)
        {
            return lhs.index == rhs.index;
        }

        public static bool operator !=(Iterator lhs, Iterator rhs)
        {
            return !(lhs == rhs);
        }
    }

    public Iterator First()
    {
        int limit = NumAllocated();

        return new Iterator(limit != 0 ? 0 : InvalidIndex(), limit);
    }

    public Iterator Next(Iterator it)
    {
        return new Iterator(it.index + 1 < it.limit ? it.index + 1 : InvalidIndex(), it.limit);
    }

    public int GetIndex(Iterator it)
    {
        return it.index;
    }

    public bool IsIdxAfter(int i, Iterator it)
    {
        return i > it.index;
    }

    public bool IsValidIterator(Iterator it)
    {
        return IsIdxValid(it.index) && it.index < it.limit;
    }

    public Iterator InvalidIterator()
    {
        return new Iterator(InvalidIndex(), 0);
    }
}

public class CUtlMemoryAligned<T> : CUtlMemory<T>
{
    private readonly int ALIGNMENT;

    public CUtlMemoryAligned(int growSize = 0, int initSize = 0)
    {
        memory = null;
    }

    public CUtlMemoryAligned(T[] memory, int numElements)
    {

    }

    ~CUtlMemoryAligned()
    {

    }

    public void SetExternalBuffer(T[] memory, int numElements)
    {

    }

    public void Grow(int num = 1)
    {

    }

    public void EnsureCapacity(int num)
    {

    }

    public void Purge()
    {

    }

    public void Purge(int numElements)
    {
        Debug.Assert(false);
    }

    private nint Align(nint addr)
    {
        uint alignmentMask = (uint)ALIGNMENT - 1;

        return (nint)(((uint)addr + alignmentMask) & ~alignmentMask);
    }
}