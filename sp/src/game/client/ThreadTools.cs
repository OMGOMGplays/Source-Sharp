namespace SourceSharp.SP.Game.Client;

public enum ThreadWaitResult
{
    TW_FAILED = 0xfffffff, // WAIT_FAILED
    TW_TIMEOUT = 0x00000102 // WAIT_TIMEOUT
}

public class CThreadLocalBase
{
    private uint index;

    public CThreadLocalBase()
    {

    }

    ~CThreadLocalBase()
    {

    }

    public object Get()
    {
        return index;
    }

    public void Set(object value)
    {
        index = (uint)value;
    }
}

public class CThreadLocal<T> : CThreadLocalBase
{
    public CThreadLocal()
    {

    }

    public new T Get()
    {
        return (T)base.Get();
    }

    public void Set(T value)
    {
        Set(value);
    }
}

public class CThreadLocalInt : CThreadLocal<int>
{
    public CThreadLocalInt()
    {

    }

    public static explicit operator int(CThreadLocalInt lhs)
    {
        return lhs.Get();
    }

    public static int operator +(CThreadLocalInt lhs)
    {
        int i = lhs.Get();
        lhs.Set(++i);
        return i;
    }

    public static int operator -(CThreadLocalInt lhs)
    {
        int i = lhs.Get();
        lhs.Set(--i);
        return i;
    }
}

public class CThreadLocalPtr<T> : CThreadLocalBase
{
    public CThreadLocalPtr()
    {

    }


}