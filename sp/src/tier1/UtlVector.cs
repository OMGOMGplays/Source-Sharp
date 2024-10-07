using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SourceSharp.SP.Tier1;

public class CUtlVector<T>
{
    public CUtlMemory<T> A;

    public CUtlMemory<T> CAllocator;
    public T ElemType;
    public T[] iterator;

    protected CUtlMemory<T> memory;
    protected int size;
    protected T[] elements;

    public CUtlVector(int growSize = 0, int initSize = 0)
    {
        memory = new CUtlMemory<T>(growSize, initSize);
        size = 0;

        ResetDbgInfo();
    }

    public CUtlVector(T[] memory, int allocationCount, int numElements = 0)
    {
        memory = new CUtlMemory<T>(memory, allocationCount);
        size = numElements;

        ResetDbgInfo();
    }

    ~CUtlVector()
    {
        Purge();
    }

    public T this[int i]
    {
        get
        {
            Dbg.Assert(IsValidIndex(i));

            return memory[i];
        }

        set
        {
            Dbg.Assert(IsValidIndex(i));

            memory[i] = value;
        }
    }

    public T Element(int i)
    {
        Dbg.Assert((uint)i < (uint)size);
        return memory[i];
    }

    public T Head()
    {
        Dbg.Assert(size > 0);
        return memory[0];
    }

    public T Tail()
    {
        Dbg.Assert(size > 0);
        return memory[size - 1];
    }

    public T begin()
    {
        return Base()[0];
    }

    public T end()
    {
        return Base()[Count()];
    }

    public T[] Base()
    {
        return memory.Base();
    }

    public int Count()
    {
        return size;
    }

    [Obsolete("Deprecated! Use UtlVector.Count() instead.")]
    public int Size()
    {
        return size;
    }

    public bool IsEmpty()
    {
        return Count() == 0;
    }

    public bool IsValidIndex(int i)
    {
        return i >= 0 && i < size;
    }

    public static int InvalidIndex()
    {
        return -1;
    }

    public int AddToHead()
    {
        return InsertBefore(0);
    }

    public int AddToTail()
    {
        return InsertBefore(size);
    }

    public int InsertBefore(int elem)
    {
        Dbg.Assert(elem == Count() || IsValidIndex(elem));

        GrowVector();
        ShiftElementsRight(elem);
        Construct(Element(elem));
        return elem;
    }

    public int InsertAfter(int elem)
    {
        return InsertBefore(elem + 1);
    }

    public int AddToHead(T src)
    {
        Dbg.Assert(Base() == null || src < Base() || src >= (Base() + Count()));
        return InsertBefore(0, src);
    }

    public int AddToTail(T src)
    {
        Dbg.Assert(Base() == null || src < Base() || src >= (Base() + Count()));
        return InsertBefore(size, src);
    }

    public int InsertBefore(int elem, T src)
    {
        Dbg.Assert(Base() == null || src < Base() || src >= (Base() + Count()));
        Dbg.Assert(elem == Count() || IsValidIndex(elem));

        GrowVector();
        ShiftElementsRight(elem);
        CopyConstruct(Element(elem), src);
        return elem;
    }

    public int InsertAfter(int elem, T src)
    {
        Dbg.Assert(Base() == null || src < Base() || src >= (Base() + Count()));
        return InsertBefore(elem + 1, src);
    }

    public int AddMultipleToHead(int num)
    {
        return InsertMultipleBefore(0, num);
    }

    public int AddMultipleToTail(int num, T toCopy = default)
    {
        Dbg.Assert(Base() == null || toCopy == null || toCopy + num < Base() || toCopy >= (Base() + Count()));

        return InsertMultipleBefore(size, num, toCopy);
    }

    public int InsertMultipleBefore(int elem, int num, T[] toInsert = null)
    {
        if (num == 0)
        {
            return elem;
        }

        Dbg.Assert(elem == Count() || IsValidIndex(elem));

        GrowVector(num);
        ShiftElementsRight(elem, num);

        for (int i = 0; i < num; i++)
        {
            Construct(Element(elem + i));
        }

        if (toInsert != null)
        {
            for (int i = 0; i < num; i++)
            {
                Element(elem + i) = toInsert[i];
            }
        }

        return elem;
    }

    public int InsertMultipleAfter(int elem, int num)
    {
        return InsertMultipleBefore(elem + 1, num);
    }

    public void SetSize(int size)
    {
        SetCount(size);
    }

    public void SetCount(int count)
    {
        RemoveAll();
        AddMultipleToTail(count);
    }

    public void SetCountNonDestructively(int count)
    {
        int delta = count - size;

        if (delta > 0)
        {
            AddMultipleToTail(delta);
        }
        else if (delta < 0)
        {
            RemoveMultipleFromTail(-delta);
        }
    }

    public void CopyArray(T[] array, int size)
    {
        Dbg.Assert(Base() == null || array != null || Base() >= (array + size) || array >= (Base() + Count()));

        SetSize(size);

        for (int i = 0; i < size; i++)
        {
            this[i] = array[i];
        }
    }

    public void Swap(CUtlVector<T> vector)
    {
        memory.Swap(vector.memory);
        //V_swap(size, vector.size);

        //V_swap(elements, vector.elements);
    }

    public int AddVectorToTail(CUtlVector<T> src)
    {
        Dbg.Assert(src != this);

        int @base = Count();

        AddMultipleToTail(src.Count());

        for (int i = 0; i < src.Count(); i++)
        {
            this[@base + i] = src[i];
        }

        return @base;
    }

    public int Find(T src)
    {
        for (int i = 0; i < Count(); i++)
        {
            if (Element(i) == src)
            {
                return i;
            }
        }

        return -1;
    }

    public bool HasElement(T src)
    {
        return Find(src) >= 0;
    }

    public void EnsureCapacity(int num)
    {
        memory.EnsureCapacity(num);
        ResetDbgInfo();
    }

    public void EnsureCount(int num)
    {
        if (Count() < num)
        {
            AddMultipleToTail(num - Count());
        }
    }

    public void FastRemove(int elem)
    {
        Dbg.Assert(IsValidIndex(elem));

        Platform.Destruct(Element(elem));

        if (size > 0)
        {
            if (elem != size - 1)
            {
                Element(elem) = Element(size - 1);
            }

            --size;
        }
    }

    public void Remove(int elem)
    {
        Platform.Destruct(Element(elem));
        ShiftElementsLeft(elem);
        --size;
    }

    public bool FindAndRemove(T src)
    {
        int elem = Find(src);

        if (elem != -1)
        {
            Remove(elem);
            return true;
        }

        return false;
    }

    public bool FindAndFastRemove(T src)
    {
        int elem = Find(src);

        if (elem != -1)
        {
            FastRemove(elem);
            return true;
        }

        return false;
    }

    public void RemoveMultiple(int elem, int num)
    {
        Dbg.Assert(elem >= 0);
        Dbg.Assert(elem + num <= Count());

        for (int i = elem + num; --i >= elem;)
        {
            Platform.Destruct(Element(i));
        }

        ShiftElementsLeft(elem, num);
        size -= num;
    }

    public void RemoveMultipleFromHead(int num)
    {
        Dbg.Assert(num <= Count());

        for (int i = num; --i >= 0;)
        {
            Platform.Destruct(Element(i));
        }

        ShiftElementsLeft(0, num);
        size -= num;
    }

    public void RemoveMultipleFromTail(int num)
    {
        Dbg.Assert(num <= Count());

        for (int i = size - num; i < size; i++)
        {
            Platform.Destruct(Element(i));
        }

        size -= num;
    }

    public void RemoveAll()
    {
        for (int i = size; --i >= 0;)
        {
            Platform.Destruct(Element(i));
        }

        size = 0;
    }

    public void Purge()
    {
        RemoveAll();
        memory.Purge();
        ResetDbgInfo();
    }

    public void PurgeAndDeleteElements()
    {
        for (int i = 0; i < size; i++)
        {
            Element(i) = null;
        }

        Purge();
    }

    public void Compact()
    {
        memory.Purge(size);
    }

    public void SetGrowSize(int size)
    {
        memory.SetGrowSize(size);
    }

    public int NumAllocated()
    {
        return memory.NumAllocated();
    }

    public void Sort(T in1, T in2)
    {
        if (Count() <= 1)
        {
            return;
        }

        if (Base() != null)
        {
            //qsort(Base(), Count(), Marshal.SizeOf<T>(), out QSortCompareFunc compare);       
        }
        else
        {
            Dbg.Assert(false);

            for (int i = size - 1; i >= 0; i--)
            {
                for (int j = 1; j <= i; j++)
                {
                    if (compare(Element(j - 1), Element(j)) < 0)
                    {
                        V_swap(Element(j - 1), Element(j));
                    }
                }
            }
        }
    }

#if DEBUG
    public void Validate(CValidator validator, string name)
    {
        validator.Push(typeid(this).name(), this, name);

        memory.Validate(validator, "memory");

        validator.Pop();
    }
#endif // DEBUG

    protected void GrowVector(int num = 1)
    {
        if (size + num > memory.NumAllocated())
        {
            memory.Grow(size + num - memory.NumAllocated());
        }

        size += num;
        ResetDbgInfo();
    }

    protected void ShiftElementsRight(int elem, int num = 1)
    {
        Dbg.Assert(IsValidIndex(elem) || size == 0 || num == 0);
        int numToMove = size - elem - num;

        if (numToMove > 0 && num > 0)
        {
            //Q_memmove(Element(elem+num), Element(elem), numToMove * Marshal.SizeOf<T>());
        }
    }

    protected void ShiftElementsLeft(int elem, int num = 1)
    {
        Dbg.Assert(IsValidIndex(elem) || size == 0 || num == 0);
        int numToMove = size - elem - num;

        if (numToMove > 0 && num > 0)
        {
            //Q_memmove(Element(elem), Element(elem+num), numToMove * Marshal.SizeOf<T>());

#if DEBUG
            // Q_memset(Elemet(size - num, 0xDD, num * Marshal.SizeOf<T>());
#endif // DEBUG
        }
    }

    protected void ResetDbgInfo()
    {
        elements = Base();
    }

    private CUtlVector(CUtlVector<T> vector)
    {

    }
}

public class CUtlBlockVector<T> : CUtlVector<T>
{
    public CUtlBlockVector(int growSize = 0, int initSize = 0) : base(growSize, initSize)
    {

    }

    private CUtlVector<T> @base;
    //typename Base::iterator begin();
    //typename Base::const_iterator begin() const;
    //typename Base::iterator end();
    //typename Base::const_iterator end() const;
}

public class CUtlVectorMT<BASE_UTLVECTOR, MUTEX_TYPE>
{
    public MUTEX_TYPE mutex;

    public CUtlVectorMT(int growSize = 0, int initSize = 0)
    {

    }

    public CUtlVectorMT(BASE_UTLVECTOR.elemType memory, int numElements)
    {

    }
}

public class CUtlVectorFixed<T, MAX_SIZE> : CUtlVector<T>
{
    public CUtlVectorFixed(int growSize = 0, int initSize = 0) : base(growSize, initSize)
    {

    }

    public CUtlVectorFixed(T[] memory, int numElements) : base(memory, numElements)
    {

    }
}

public class CUtlVectorFixedGrowable<T, MAX_SIZE> : CUtlVector<T>
{
    public CUtlVectorFixedGrowable(int growSize = 0) : base(growSize, MAX_SIZE)
    {

    }
}

public class CUtlVectorConservative<T> : CUtlVector<T>
{
    public CUtlVectorConservative(int growSize = 0, int initSize = 0) : base(growSize, initSize)
    {

    }

    public CUtlVectorConservative(T[] memory, int numElements) : base(memory, numElements)
    {

    }
}

public class CUtlVectorUltraConservativeAllocator
{
    public static nint Alloc(ulong size)
    {
        return Marshal.AllocHGlobal((int)size);
    }

    public static nint Realloc(nint mem, ulong size)
    {
        return Marshal.ReAllocHGlobal(mem, (int)size);
    }

    public static void Free(nint mem)
    {
        Marshal.FreeHGlobal(mem);
    }

    public static ulong GetSize(nint mem)
    {
        return (ulong)Marshal.SizeOf(mem);
    }
}

public class CUtlVectorUltraConservative<T, A> where A : CUtlVectorUltraConservativeAllocator
{
    public Data data;

    public CUtlVectorUltraConservative()
    {
        data = StaticData();
    }

    ~CUtlVectorUltraConservative()
    {
        RemoveAll();
    }

    public int Count()
    {
        return data.size;
    }

    public static int InvalidIndex()
    {
        return -1;
    }

    public bool IsValidIndex(int i)
    {
        return i >= 0 && i < Count();
    }

    public T this[int i]
    {
        get
        {
            Dbg.Assert(IsValidIndex(i));

            return data.elements[i];
        }

        set
        {
            Dbg.Assert(IsValidIndex(i));

            data.elements[i] = value;
        }
    }

    public T Element(int i)
    {
        Dbg.Assert(IsValidIndex(i));
        return data.elements[i];
    }

    public void EnsureCapacity(int num)
    {
        int curCount = Count();

        if (num <= curCount)
        {
            return;
        }

        if (data == StaticData())
        {
            data = (Data)A.Alloc(sizeof(int) + (num * Marshal.SizeOf<T>()));
            data.size = 0;
        }
        else
        {
            int needed = sizeof(int) + (num * Marshal.SizeOf<T>());
            int have = A.GetSize(data);

            if (needed > have)
            {
                data = (Data)A.Realloc(data, needed);
            }
        }
    }

    public int AddToTail(T src)
    {
        int @new = Count();
        EnsureCapacity(Count() + 1);
        data.elements[@new] = src;
        data.size++;
        return @new;
    }

    public void RemoveAll()
    {
        if (Count() != 0)
        {
            for (int i = data.size; --i >= 0;)
            {
                Platform.Destruct(data.elements[i]);
            }
        }

        if (data != StaticData())
        {
            A.Free(data);
            data = StaticData();
        }
    }

    public void PurgeAndDeleteElements()
    {
        if (data != StaticData())
        {
            for (int i = 0; i < data.size; i++)
            {
                // delete Element(i);
            }

            RemoveAll();
        }
    }

    public void FastRemove(int elem)
    {
        Dbg.Assert(IsValidIndex(elem));

        Platform.Destruct(Element(elem));

        if (Count() > 0)
        {
            if (elem != data.size - 1)
            {
                Element(data.size - 1) = Element(elem);
            }

            --data.size;
        }

        if (data.size == 0)
        {
            A.Free(data);
            data = StaticData();
        }
    }

    public void Remove(int elem)
    {
        Platform.Destruct(Element(elem));
        ShiftElementsLeft(elem);
        --data.size;

        if (data.size == 0)
        {
            A.Free(data);
            data = StaticData();
        }
    }

    public int Find(T src)
    {
        int count = Count();

        for (int i = 0; i < count; i++)
        {
            if (Element(i) == src)
            {
                return i;
            }
        }

        return -1;
    }

    public bool FindAndRemove(T src)
    {
        int elem = Find(src);

        if (elem != -1)
        {
            Remove(elem);
            return true;
        }

        return false;
    }

    public bool FastFindAndRemove(T src)
    {
        int elem = Find(src);

        if (elem != -1)
        {
            FastRemove(elem);
            return true;
        }

        return false;
    }

    public struct Data
    {
        public int size;
        public T[] elements = new T[0];

        public Data() { }
    }

    private void ShiftElementsLeft(int elem, int num = 1)
    {
        int size = Count();
        Dbg.Assert(IsValidIndex(elem) || (size == 0) || (num == 0));
        int numToMove = size - elem - num;

        if (numToMove > 0 && num > 0)
        {


#if DEBUG
            Element(size - num) = 0xDD;
#endif // DEBUG
        }
    }

    private static Data StaticData()
    {
        Data staticData = new();
        Dbg.Assert(staticData.size == 0);
        return staticData;
    }
}

public class CCopyableVector<T> : CUtlVector<T>
{
    public CCopyableVector(int growSize = 0, int initSize = 0) : base(growSize, initSize) { }
    public CCopyableVector(T[] memory, int numElements) : base(memory, numElements) { }
    ~CCopyableVector() { }

    public CCopyableVector(CCopyableVector<T> vector)
    {
        CopyArray(vector.Base(), vector.Count());
    }
}

public class CUtlVectorAutoPurge<T> : CUtlVector<T>
{
    ~CUtlVectorAutoPurge()
    {
        PurgeAndDeleteElements();
    }
}

public class CUtlStringList : CUtlVectorAutoPurge<string>
{
    public void CopyAndAddToTail(string @string)
    {
        string newStr;
        newStr = @string;
        AddToTail(newStr);
    }

    public static int SortFunc(string sz1, string sz2)
    {
        return sz1 == sz2 ? 1 : 0;
    }

    public new void PurgeAndDeleteElements()
    {
        for (int i = 0; i < size; i++)
        {
            Element(i) = null;
        }

        Purge();
    }

    ~CUtlStringList()
    {
        PurgeAndDeleteElements();
    }
}

public class CSplitString : CUtlVector<string>
{
    private string buffer;

    public CSplitString(string @string, string separator)
    {

    }

    public CSplitString(string @string, string[] seperators, int nSeperators)
    {

    }

    ~CSplitString()
    {

    }

    private void Construct(string @string, string[] separators, int nSeparatos)
    {

    }

    private new void PurgeAndDeleteElements()
    {

    }
}