using System;

namespace SourceSharp.SP.Public.Tier1;

public class CUtlVector<T, A> where A : CUtlMemory<T>
{
    public A CAllocator;
    public T ElemType;
    public T[] iterator;

    protected A memory;
    protected int size;

    public CUtlVector(int growSize = 0, int initSize = 0)
    {

    }

    public CUtlVector(T[] memory, int allocationCount, int numElements = 0)
    {

    }

    ~CUtlVector()
    {

    }

    public T Element(int i)
    {

    }

    public T Head()
    {

    }

    public T Tail()
    {

    }

    public T begin()
    {
        return Base();
    }

    public T end()
    {
        return Base() + Count();
    }

    public T Base()
    {
        return memory.Base();
    }

    public int Count()
    {

    }

    [Obsolete]
    public int Size()
    {
        return 0;
    }

    public bool IsEmpty()
    {
        return Count() == 0;
    }
}