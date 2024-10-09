using System;
using System.Runtime.InteropServices;
using SourceSharp.SP.Mathlib;
using SourceSharp.SP.Tier1;

namespace SourceSharp.SP.Game.Client;

public class CInterpolationContext
{
    public static float lastPacketTimestamp;

    public static ConVar cl_extrapolate_amount = new ConVar("cl_extrapolate_amount", "0.25", IConVar.FCVAR_CHEAT, "Set how many seconds the client will extrapolate entities for.");

    private CInterpolationContext next;
    private bool oldAllowExtrapolation;
    private float oldLastTimeStamp;

    private static CInterpolationContext head;
    private static bool allowExtrapolation;
    private static float lastTimeStamp;

    public CInterpolationContext()
    {
        oldAllowExtrapolation = allowExtrapolation;
        oldLastTimeStamp = lastTimeStamp;

        allowExtrapolation = false;

        next = head;
        head = this;
    }

    ~CInterpolationContext()
    {
        allowExtrapolation = oldAllowExtrapolation;
        lastTimeStamp = oldLastTimeStamp;

        Dbg.Assert(head == this);
        head = next;
    }

    public static void EnableExtrapolation(bool state)
    {
        allowExtrapolation = state;
    }

    public static bool IsThereAContext()
    {
        return head != null;
    }

    public static bool IsExtrapolationAllowed()
    {
        return allowExtrapolation;
    }

    public static void SetLastTimeStamp(float timeStamp)
    {
        lastTimeStamp = timeStamp;
    }

    public static float GetLastTimeStamp()
    {
        return lastTimeStamp;
    }

    public static void Interpolation_SetLastPacketTimeStamp(float timestamp)
    {
        Dbg.Assert(timestamp > 0);
        lastPacketTimestamp = timestamp;
    }

    public static T ExtrapolateInterpolatedVarType<T>(T oldVal, T newVal, float divisor, float extrapolationAmount)
    {
        return newVal;
    }

    public static Vector ExtrapolateInterpolatedVarType(Vector oldVal, Vector newVal, float divisor, float extrapolationAmount)
    {
        return Lerp_Functions.Lerp(1.0f + extrapolationAmount * divisor, oldVal, newVal);
    }

    public static float ExtrapolateInterpolatedVarType(float oldVal, float newVal, float divisor, float extrapolationAmount)
    {
        return Lerp_Functions.Lerp(1.0f + extrapolationAmount * divisor, oldVal, newVal);
    }

    public static QAngle ExtrapolateInterpolatedVarType(QAngle oldVal, QAngle newVal, float divisor, float extrapolationAmount)
    {
        return Lerp_Functions.Lerp<QAngle>(1.0f + extrapolationAmount * divisor, oldVal, newVal);
    }
}

public interface IInterpolatedVar
{
    public const int LATCH_ANIMATION_VAR = 1 << 0;
    public const int LATCH_SIMULATION_VAR = 1 << 1;

    public const int EXCLUDE_AUTO_LATCH = 1 << 2;
    public const int EXCLUDE_AUTO_INTERPOLATE = 1 << 3;

    public const int INTERPOLATE_LINEAR_ONLY = 1 << 4;
    public const int INTERPOLATE_OMIT_UPDATE_LAST_NETWORKED = 1 << 5;

    public const float EXTRA_INTERPOLATION_HISTORY_STORED = 0.05f;

    public void Setup(object value, int type);
    public void SetInterpolationAmount(float seconds);

    public void NoteLastNetworkedValue();
    public bool NoteChanged(float changetime, bool updateLastNetworkedValue);
    public void Reset();

    public int Interpolate(float currentTime);

    public int GetType();
    public void RestoreToLastNetworked();
    public void Copy(IInterpolatedVar src);

    public string GetDebugName();
    public void SetDebugName(string name);

    public void SetDebug(bool debug);
}

public struct CInterpolatedVarEntryBase<T>
{
    public float changetime;
    public int count;
    public T[] value;

    public CInterpolatedVarEntryBase()
    {
        value = null;
        count = 0;
        changetime = 0;
    }

    public void FastTransferFrom(CInterpolatedVarEntryBase<T> src)
    {
        Dbg.Assert(value != null);
        value = src.value;
        count = src.count;
        changetime = src.changetime;
        src.value = null;
        src.count = 0;
    }

    public T[] GetValue()
    {
        return value;
    }

    public void Init(int maxCount)
    {
        if (maxCount == 0)
        {
            DeleteEntry();
        }
        else
        {
            if (maxCount != count)
            {
                DeleteEntry();
            }

            if (value == null)
            {
                count = maxCount;
                value = new T[maxCount];
            }
        }

        Dbg.Assert(count == maxCount);
    }

    public object NewEntry(T value, int maxCount, float time)
    {
        changetime = time;
        Init(maxCount);

        if (value != null && maxCount != 0)
        {
            this.value = [value];
        }

        return this.value;
    }

    public void DeleteEntry()
    {
        value = null;
        count = 0;
    }

    private CInterpolatedVarEntryBase(CInterpolatedVarEntryBase<T> src)
    {

    }
}

public class CSimpleRingBuffer<T>
{
    private T[] elements;
    private ushort maxElement;
    private ushort firstElement;
    private ushort count;
    private ushort growSize;

    public CSimpleRingBuffer(int startSize = 4)
    {
        elements = null;
        maxElement = 0;
        firstElement = 0;
        count = 0;
        growSize = 16;
        EnsureCapacity(startSize);
    }

    ~CSimpleRingBuffer()
    {
        elements = null;
    }

    public int Count()
    {
        return count;
    }

    public int Head()
    {
        return count > 0 ? 0 : InvalidIndex();
    }

    public bool IsIdxValid(int i)
    {
        return i >= 0 && i < count ? true : false;
    }

    public bool IsValidIndex(int i)
    {
        return IsIdxValid(i);
    }

    public static int InvalidIndex()
    {
        return -1;
    }

    public T this[int i]
    {
        get
        {
            Dbg.Assert(IsIdxValid(i));
            i += firstElement;
            i = WrapRange(i);

            return elements[i];
        }

        set
        {
            Dbg.Assert(IsIdxValid(i));
            i += firstElement;
            i = WrapRange(i);

            elements[i] = value;
        }
    }

    public void EnsureCapacity(int capSize)
    {
        if (capSize > maxElement)
        {
            int newMax = maxElement + ((capSize + growSize - 1) / growSize) * growSize;
            T[] newElements = new T[newMax];

            for (int i = 0; i < maxElement; i++)
            {
                newElements[i].FastTransferFrom(elements[WrapRange(i + firstElement)]);
            }

            firstElement = 0;
            maxElement = (ushort)newMax;
            elements = newElements;
        }
    }

    public int AddToHead()
    {
        EnsureCapacity(count + 1);
        int i = firstElement + maxElement - 1;
        count++;
        i = WrapRange(i);
        firstElement = (ushort)i;
        return 0;
    }

    public int AddToHead(T elem)
    {
        AddToHead();
        elements[firstElement] = elem;
        return 0;
    }

    public int AddToTail()
    {
        EnsureCapacity(count + 1);
        count++;
        return WrapRange(firstElement + count - 1);
    }

    public void RemoveAll()
    {
        count = 0;
        firstElement = 0;
    }

    public void RemoveAtHead()
    {
        if (count > 0)
        {
            firstElement = (ushort)WrapRange(firstElement + 1);
            count--;
        }
    }

    public void Truncate(int newLength)
    {
        if (newLength < count)
        {
            Dbg.Assert(newLength >= 0);
            count = (ushort)newLength;
        }
    }

    private int WrapRange(int i)
    {
        return i >= maxElement ? i - maxElement : i;
    }
}

public class CInterpolatedVarArrayBase<T> : IInterpolatedVar
{
    public class CInterpolatedVarPrivate;

    protected class CInterpolationInfo
    {
        public bool hermite;
        public int oldest;
        public int older;
        public int newer;
        public float frac;
    }

    protected T[] value;
    protected CSimpleRingBuffer<CInterpolatedVarEntryBase<T>> varHistory;
    protected T[] lastNetworkedValue;
    protected float lastNetworkedTime;
    protected byte type;
    protected byte maxCount;
    protected bool[] looping;
    protected float interpolationAmount;
    protected string debugName;
    protected bool debug = true;

    public CInterpolatedVarArrayBase(string debugName = "no debug name")
    {
        this.debugName = debugName;
        value = default;
        type = IInterpolatedVar.LATCH_ANIMATION_VAR;
        interpolationAmount = 0.0f;
        maxCount = 0;
        lastNetworkedTime = 0;
        lastNetworkedValue = default;
        looping = [false];
        debug = false;
    }

    ~CInterpolatedVarArrayBase()
    {
        ClearHistory();
        looping = [false];
        lastNetworkedValue = default;
    }

    public void Setup(object value, int type)
    {
        value = (T)value;
        this.type = (byte)type;
    }

    public void SetInterpolationAmount(float seconds)
    {
        interpolationAmount = seconds;
    }

    public void NoteLastNetworkedValue()
    {
        lastNetworkedValue = value;
        lastNetworkedTime = CInterpolationContext.lastPacketTimestamp;
    }

    public bool NoteChanged(float changetime, bool updateLastNetworkedValue)
    {
        return NoteChanged(changetime, interpolationAmount, updateLastNetworkedValue);
    }

    public void Reset()
    {
        ClearHistory();

        if (value != null)
        {
            AddToHead(CDLL_Client_Int.curtime, value, false);
            AddToHead(CDLL_Client_Int.curtime, value, false);
            AddToHead(CDLL_Client_Int.curtime, value, false);

            lastNetworkedValue = value;
        }
    }

    public int Interpolate(float currentTime)
    {
        return Interpolate(currentTime, interpolationAmount);
    }

    public void RestoreToLastNetworked()
    {
        Dbg.Assert(value != null);
        value = lastNetworkedValue;
    }

    public void Copy(IInterpolatedVar inSrc)
    {
        CInterpolatedVarArrayBase<T> src = (CInterpolatedVarArrayBase<T>)inSrc;

        if (src == null || src.maxCount != maxCount)
        {
            if (src != null)
            {
                Dbg.AssertMsg3(false, "src.maxCount ({0}) != maxCount ({1}) for {2}.", src.maxCount, maxCount, debugName);
            }
            else
            {
                Dbg.AssertMsg(false, "src was null in CInterpolatedVarArrayBase<T>::Copy.");
            }

            return;
        }

        Dbg.Assert((type & ~IInterpolatedVar.EXCLUDE_AUTO_LATCH) == (src.type & ~IInterpolatedVar.EXCLUDE_AUTO_LATCH));
        Dbg.Assert(debugName == src.GetDebugName());

        for (int i = 0; i < maxCount; i++)
        {
            lastNetworkedValue[i] = src.lastNetworkedValue[i];
            looping[i] = src.looping[i];
        }

        lastNetworkedTime = src.lastNetworkedTime;

        varHistory.RemoveAll();

        for (int i = 0; i < src.varHistory.Count(); i++)
        {
            int newslot = varHistory.AddToTail();

            CInterpolatedVarEntryBase<T> dest = varHistory[newslot];
            CInterpolatedVarEntryBase<T> _src = src.varHistory[i];
            dest.NewEntry(_src.GetValue()[0], maxCount, _src.changetime);
        }
    }

    public string GetDebugName()
    {
        return debugName;
    }

    public bool NoteChanged(float changetime, float interpolation_amount, bool updateLastNetworkedValue)
    {
        Dbg.Assert(value != null);

        bool ret = true;

        if (varHistory.Count() != 0)
        {
            if (value.GetType() == varHistory[0].GetValue().GetType())
            {
                ret = false;
            }
        }

        if (debug)
        {
            string diffString = ret ? "differs" : "identical";

            Dbg.Msg("{0} LatchChanged at {1} changetime {2}:    {3}", GetDebugName(), CDLL_Client_Int.globals.curtime, changetime, diffString);
        }

        AddToHead(changetime, value, true);

        if (updateLastNetworkedValue)
        {
            NoteLastNetworkedValue();
        }

        RemoveEntriesPreviousTo(CDLL_Client_Int.curtime - interpolation_amount - IInterpolatedVar.EXTRA_INTERPOLATION_HISTORY_STORED);

        return ret;
    }

    public int Interpolate(float currentTime, float interpolation_amount)
    {
        int noMoreChanges = 0;

        CInterpolationInfo info = new CInterpolationInfo();

        if (!GetInterpolationInfo(ref info, currentTime, interpolation_amount, noMoreChanges))
        {
            return noMoreChanges;
        }

        CSimpleRingBuffer<CInterpolatedVarEntryBase<T>> history = varHistory;

        if (debug)
        {
            Dbg.Msg("{0} Interpolate at {1}{2}\n", GetDebugName(), currentTime, noMoreChanges != 0 ? " [value will hold]" : "");
        }

        if (info.hermite)
        {
            _Interpolate_Hermite(out value[0], info.frac, history[info.oldest], history[info.older], history[info.newer]);
        }
        else if (info.newer == info.older)
        {
            int realOlder = info.newer + 1;

            if (CInterpolationContext.IsExtrapolationAllowed() && IsValidIndex(realOlder) && history[realOlder].changetime != 0.0f && interpolation_amount > 0.000001f && CInterpolationContext.GetLastTimeStamp() <= lastNetworkedTime)
            {
                _Extrapolate(out value, history[realOlder], history[info.newer], currentTime - interpolation_amount, CInterpolationContext.cl_extrapolate_amount.GetFloat());
            }
            else
            {
                _Interpolate(out value[0], info.frac, history[info.older], history[info.newer]);
            }
        }
        else
        {
            _Interpolate(out value[0], info.frac, history[info.older], history[info.newer]);
        }

        RemoveEntriesPreviousTo(currentTime - interpolation_amount - IInterpolatedVar.EXTRA_INTERPOLATION_HISTORY_STORED);
        return noMoreChanges;
    }

    public void DebugInterpolate(out T @out, float currentTime)
    {
        float interpolation_amount = interpolationAmount;

        int noMoreChanges = 0;

        CInterpolationInfo info = new CInterpolationInfo();
        GetInterpolationInfo(ref info, currentTime, interpolation_amount, noMoreChanges);

        CSimpleRingBuffer<CInterpolatedVarEntryBase<T>> history = varHistory;

        if (info.hermite)
        {
            _Interpolate_Hermite(out @out, info.frac, history[info.oldest], history[info.older], history[info.newer]);
        }
        else if (info.newer == info.older)
        {
            int realOlder = info.newer + 1;

            if (CInterpolationContext.IsExtrapolationAllowed() && IsValidIndex(realOlder) && history[realOlder].changetime != 0.0f && interpolation_amount > 0.000001f && CInterpolationContext.GetLastTimeStamp <= lastNetworkedTime)
            {
                _Extrapolate(out @out, history[realOlder], history[info.newer], currentTime - interpolation_amount, CInterpolationContext.cl_extrapolate_amount.GetFloat());
            }
            else
            {
                _Interpolate(out @out, info.frac, history[info.older], history[info.newer]);
            }
        }
        else
        {
            _Interpolate(out @out, info.frac, history[info.older], history[info.newer]);
        }
    }

    public void GetDerivative(out T @out, float currentTime)
    {
        CInterpolationInfo info = new CInterpolationInfo();

        if (!GetInterpolationInfo(ref info, currentTime, interpolationAmount, 0))
        {
            @out = default;
            return;
        }

        if (info.hermite)
        {
            _Derivative_Hermite(out @out, info.frac, varHistory[info.oldest], varHistory[info.older], varHistory[info.newer]);
        }
        else
        {
            _Derivative_Linear(out @out, varHistory[info.older], varHistory[info.newer]);
        }
    }

    public void GetDerivative_SmoothVelocity(out T @out, float currentTime)
    {
        CInterpolationInfo info = new CInterpolationInfo();

        if (!GetInterpolationInfo(ref info, currentTime, interpolationAmount, 0))
        {
            @out = default;
            return;
        }

        CSimpleRingBuffer<CInterpolatedVarEntryBase<T>> history = varHistory;
        bool extrapolate = false;
        int realOlder = 0;

        if (info.hermite)
        {
            _Derivative_Hermite_SmoothVelocity(out @out, info.frac, history[info.oldest], history[info.older], history[info.newer]);
            return;
        }
        else if (info.newer == info.older && CInterpolationContext.IsExtrapolationAllowed())
        {
            realOlder = info.newer + 1;

            if (IsValidIndex(realOlder) && history[realOlder].changetime != 0.0f)
            {
                if (interpolationAmount > 0.000001f && CInterpolationContext.GetLastTimeStamp() <= (currentTime - interpolationAmount))
                {
                    extrapolate = true;
                }
            }
        }

        if (extrapolate)
        {
            _Derivative_Linear(out @out, history[realOlder], history[info.newer]);

            float destTime = currentTime - interpolationAmount;
            float diff = destTime - history[info.newer].changetime;
            diff = Math.Clamp(diff, 0.0f, CInterpolationContext.cl_extrapolate_amount.GetFloat() * 2);

            if (diff > CInterpolationContext.cl_extrapolate_amount.GetFloat())
            {
                float scale = 1 - (diff - CInterpolationContext.cl_extrapolate_amount.GetFloat()) / CInterpolationContext.cl_extrapolate_amount.GetFloat();

                for (int i = 0; i < maxCount; i++)
                {
                    @out *= scale;
                }
            }
        }
        else
        {
            _Derivative_Linear(out @out, history[info.older], history[info.newer]);
        }
    }

    public void ClearHistory()
    {
        for (int i = 0; i < varHistory.Count(); i++)
        {
            varHistory[i].DeleteEntry();
        }

        varHistory.RemoveAll();
    }

    public void AddToHead(float changeTime, T[] values, bool flushNewer)
    {
        int newslot;

        if (flushNewer)
        {
            while (varHistory.Count() != 0)
            {
                if (varHistory[0].changetime + 0.0001f > changeTime)
                {
                    varHistory.RemoveAtHead();
                }
                else
                {
                    break;
                }
            }

            newslot = varHistory.AddToHead();
        }
        else
        {
            newslot = varHistory.AddToHead();

            for (int i = 0; i < varHistory.Count(); i++)
            {
                if (varHistory[i].changetime <= changeTime)
                {
                    break;
                }

                varHistory[newslot].FastTransferFrom(varHistory[i]);
                newslot = i;
            }
        }

        CInterpolatedVarEntryBase<T> e = varHistory[newslot];
        e.NewEntry(values[0], maxCount, changeTime);
    }

    public T GetPrev(int arrayIndex = 0)
    {
        Dbg.Assert(value != null);
        Dbg.Assert(arrayIndex >= 0 && arrayIndex < maxCount);

        if (varHistory.Count() > 1)
        {
            return varHistory[1].GetValue()[arrayIndex];
        }

        return value[arrayIndex];
    }

    public T GetCurrent(int arrayIndex = 0)
    {
        Dbg.Assert(value != null);
        Dbg.Assert(arrayIndex >= 0 && arrayIndex < maxCount);

        if (varHistory.Count() > 0)
        {
            return varHistory[0].GetValue()[arrayIndex];
        }

        return value[arrayIndex];
    }

    public float GetInterval()
    {
        if (varHistory.Count() > 1)
        {
            return varHistory[0].changetime - varHistory[1].changetime;
        }

        return 0.0f;
    }

    public new int GetType()
    {
        return type;
    }

    public bool IsValidIndex(int i)
    {
        return varHistory.IsValidIndex(i);
    }

    public T GetHistoryValue(int index, out float changetime, int arrayIndex = 0)
    {
        Dbg.Assert(arrayIndex >= 0 && arrayIndex < maxCount);

        if (varHistory.IsIdxValid(index))
        {
            CInterpolatedVarEntryBase<T> entry = varHistory[index];
            changetime = entry.changetime;
            return entry.GetValue()[arrayIndex];
        }
        else
        {
            changetime = 0.0f;
            return default;
        }
    }

    public int GetHead()
    {
        return 0;
    }

    public int GetNext(int i)
    {
        int next = i++;

        if (!varHistory.IsValidIndex(next))
        {
            return CSimpleRingBuffer<CInterpolatedVarEntryBase<T>>.InvalidIndex();
        }

        return next;
    }

    public void SetHistoryValuesForItem(int item, T value)
    {
        Dbg.Assert(item >= 0 && item < maxCount);

        for (int i = 0; i < varHistory.Count(); i++)
        {
            CInterpolatedVarEntryBase<T> entry = varHistory[i];
            entry.GetValue()[item] = value;
        }
    }

    public void SetLooping(bool looping, int arrayIndex = 0)
    {
        Dbg.Assert(arrayIndex >= 0 && arrayIndex < maxCount);
        this.looping[arrayIndex] = looping;
    }

    public void SetMaxCount(int newmax)
    {
        bool changed = newmax != maxCount ? true : false;

        newmax = Math.Max(1, newmax);

        maxCount = (byte)newmax;

        if (changed)
        {
            looping = null;
            lastNetworkedValue = null;
            looping = new bool[maxCount];
            lastNetworkedValue = new T[maxCount];

            for (int i = 0; i < maxCount; i++)
            {
                looping[i] = false;
                lastNetworkedValue = default;
            }

            Reset();
        }
    }

    public int GetMaxCount()
    {
        return maxCount;
    }

    public float GetOldestEntry()
    {
        float lastVal = 0;

        if (varHistory.Count() != 0)
        {
            lastVal = varHistory[varHistory.Count() - 1].changetime;
        }

        return lastVal;
    }

    public void SetDebugName(string name)
    {
        debugName = name;
    }

    public void SetDebug(bool debug)
    {
        this.debug = debug;
    }

    public bool GetInterpolationInfo(float currentTime, int newer, int older, int oldest)
    {
        CInterpolationInfo info = new CInterpolationInfo();
        bool result = GetInterpolationInfo(ref info, currentTime, interpolationAmount, 0);

        if (newer != 0)
        {
            newer = info.newer;
        }

        if (older != 0)
        {
            older = info.older;
        }

        if (oldest != 0)
        {
            oldest = info.oldest;
        }

        return result;
    }

    protected void RemoveOldEntries(float oldesttime)
    {
        int newCount = varHistory.Count();

        for (int i = varHistory.Count(); --i > 2;)
        {
            if (varHistory[i].changetime > oldesttime)
            {
                break;
            }

            newCount = i;
        }

        varHistory.Truncate(newCount);
    }

    protected void RemoveEntriesPreviousTo(float time)
    {
        for (int i = 0; i < varHistory.Count(); i++)
        {
            if (varHistory[i].changetime < time)
            {
                varHistory.Truncate(i + 3);
                break;
            }
        }
    }

    protected bool GetInterpolationInfo(ref CInterpolationInfo info, float currentTime, float interpolation_amount, int noMoreChanges)
    {
        Dbg.Assert(value != null);

        CSimpleRingBuffer<CInterpolatedVarEntryBase<T>> varHistory = this.varHistory;

        float targettime = currentTime - interpolation_amount;

        info.hermite = false;
        info.frac = 0;
        info.oldest = info.older = info.newer = CSimpleRingBuffer<CInterpolatedVarEntryBase<T>>.InvalidIndex();

        for (int i = 0; i < varHistory.Count(); i++)
        {
            info.older = i;

            float older_change_time = this.varHistory[i].changetime;

            if (older_change_time == 0.0f)
            {
                break;
            }

            if (targettime < older_change_time)
            {
                info.newer = info.older;
                continue;
            }

            if (info.newer == CSimpleRingBuffer<CInterpolatedVarEntryBase<T>>.InvalidIndex())
            {
                info.newer = info.older;

                if (noMoreChanges != 0)
                {
                    noMoreChanges = 1;
                }

                return true;
            }

            float newer_change_time = varHistory[info.newer].changetime;
            float dt = newer_change_time - older_change_time;

            if (dt > 0.0001f)
            {
                info.frac = (targettime - older_change_time) / (newer_change_time - older_change_time);
                info.frac = Math.Min(info.frac, 2.0f);

                int oldestindex = i + 1;

                if ((type & IInterpolatedVar.INTERPOLATE_LINEAR_ONLY) == 0 && varHistory.IsIdxValid(oldestindex))
                {
                    info.oldest = oldestindex;
                    float oldest_change_time = varHistory[oldestindex].changetime;
                    float dt2 = older_change_time - oldest_change_time;

                    if (dt2 > 0.0001f)
                    {
                        info.hermite = true;
                    }
                }

                if (noMoreChanges != 0 && info.newer == this.varHistory.Head())
                {
                    if (COMPARE_HISTORY(info.newer, info.older))
                    {
                        if (!info.hermite || COMPARE_HISTORY(info.newer, info.oldest))
                        {
                            noMoreChanges = 1;
                        }
                    }
                }

            }

            return true;
        }

        if (info.newer != CSimpleRingBuffer<CInterpolatedVarEntryBase<T>>.InvalidIndex())
        {
            info.older = info.newer;
            return true;
        }

        info.newer = info.older;
        return info.older != CSimpleRingBuffer<CInterpolatedVarEntryBase<T>>.InvalidIndex();
    }

    protected void TimeFixup_Hermite(CInterpolatedVarEntryBase<T> fixup, CInterpolatedVarEntryBase<T> prev, CInterpolatedVarEntryBase<T> start, CInterpolatedVarEntryBase<T> end)
    {
        TimeFixup2_Hermite(fixup, prev, start, end.changetime - start.changetime);
    }

    protected void TimeFixup2_Hermite(CInterpolatedVarEntryBase<T> fixup, CInterpolatedVarEntryBase<T> prev, CInterpolatedVarEntryBase<T> start, float dt)
    {
        float dt2 = start.changetime - prev.changetime;

        if (MathF.Abs(dt - dt2) > 0.0001f && dt2 > 0.0001f)
        {
            float frac = dt / dt2;

            fixup.changetime = start.changetime - dt;

            for (int i = 0; i < maxCount; i++)
            {
                if (looping[i])
                {
                    fixup.GetValue()[i] = Lerp_Functions.LoopingLerp(1 - frac, prev.GetValue()[i], ref start.GetValue()[i]);
                }
                else
                {
                    fixup.GetValue()[i] = Lerp_Functions.Lerp(1 - frac, prev.GetValue()[i], ref start.GetValue()[i]);
                }
            }

            prev = fixup;
        }
    }

    protected void _Extrapolate(out T[] @out, CInterpolatedVarEntryBase<T> old, CInterpolatedVarEntryBase<T> @new, float destinationTime, float maxExtrapolationAmount)
    {
        if (MathF.Abs(old.changetime - @new.changetime) < 0.001f || destinationTime <= @new.changetime)
        {
            for (int i = 0; i < maxCount; i++)
            {
                @out[i] = @new.GetValue()[i];
            }
        }
        else
        {
            float extrapolationAmount = Math.Min(destinationTime - @new.changetime, maxExtrapolationAmount);

            float divisor = 1.0f / (@new.changetime - old.changetime);

            for (int i = 0; i < maxCount; i++)
            {
                @out[i] = CInterpolationContext.ExtrapolateInterpolatedVarType(old.GetValue()[i], @new.GetValue()[i], divisor, extrapolationAmount);
            }
        }
    }

    protected void _Interpolate(out T[] @out, float frac, CInterpolatedVarEntryBase<T> start, CInterpolatedVarEntryBase<T> end)
    {
        Dbg.Assert(start);
        Dbg.Assert(end);

        if (start.GetValue() == end.GetValue())
        {
            for (int i = 0; i < maxCount; i++)
            {
                @out[i] = end.GetValue()[i];
                Lerp_Functions.Lerp_Clamp(ref @out[i]);
            }

            @out = default;
            return;
        }

        Dbg.Assert(frac >= 0.0f && frac <= 1.0f);

        for (int i = 0; i < maxCount; i++)
        {
            if (looping[i])
            {
                @out[i] = Lerp_Functions.LoopingLerp(frac, start.GetValue()[i], ref end.GetValue()[i]);
            }
            else
            {
                @out[i] = Lerp_Functions.Lerp(frac, start.GetValue()[i], end.GetValue()[i]);
            }

            Lerp_Functions.Lerp_Clamp(ref @out[i]);
        }
    }

    protected void _Interpolate_Hermite(out T[] @out, float frac, CInterpolatedVarEntryBase<T> prev, CInterpolatedVarEntryBase<T> start, CInterpolatedVarEntryBase<T> end, bool looping = false)
    {
        Dbg.Assert(start);
        Dbg.Assert(end);

        CDisableRangeChecks disableRangeChecks;

        CInterpolatedVarEntryBase<T> fixup = new CInterpolatedVarEntryBase<T>();
        fixup.Init(maxCount);
        TimeFixup_Hermite(fixup, prev, start, end);
        @out = new T[maxCount];

        for (int i = 0; i < maxCount; i++)
        {
            if (this.looping[i])
            {
                @out[i] = Lerp_Functions.LoopingLerp_Hermite(frac, prev.GetValue()[i], start.GetValue()[i], end.GetValue()[i]);
            }
            else
            {
                @out[i] = Lerp_Functions.Lerp_Hermite(frac, prev.GetValue()[i], start.GetValue()[i], end.GetValue()[i]);
            }

            Lerp_Functions.Lerp_Clamp(ref @out[i]);
        }
    }

    protected void _Derivative_Hermite(out T[] @out, float frac, CInterpolatedVarEntryBase<T> prev, CInterpolatedVarEntryBase<T> start, CInterpolatedVarEntryBase<T> end)
    {
        Dbg.Assert(start);
        Dbg.Assert(end);

        CDisableRangeChecks disableRangeChecks;

        CInterpolatedVarEntryBase<T> fixup = new CInterpolatedVarEntryBase<T>();
        fixup.value = new T[Marshal.SizeOf<T>() * maxCount];
        TimeFixup_Hermite(fixup, prev, start, end);
        @out = new T[maxCount];

        float divisor = 1.0f / (end.changetime - start.changetime);

        for (int i = 0; i < maxCount; i++)
        {
            Dbg.Assert(!looping[i]);
            @out[i] = Lerp_Functions.Derivative_Hermite(frac, prev.GetValue()[i], start.GetValue()[i], end.GetValue()[i]);
            @out[i] *= divisor;
        }
    }

    protected void _Derivative_Hermite_SmoothVelocity(out T[] @out, float frac, CInterpolatedVarEntryBase<T> b, CInterpolatedVarEntryBase<T> c, CInterpolatedVarEntryBase<T> d)
    {
        CInterpolatedVarEntryBase<T> fixup = new CInterpolatedVarEntryBase<T>();
        fixup.Init(maxCount);
        TimeFixup_Hermite(fixup, b, c, d);

        for (int i = 0; i < maxCount; i++)
        {
            T prevVel = (c.GetValue()[i] - b.GetValue()[i]) / (c.changetime - b.changetime);
            T curVel = (d.GetValue()[i] - c.GetValue()[i]) / (d.changetime - c.changetime);
            @out[i] = Lerp_Functions.Lerp(frac, prevVel, curVel);
        }
    }

    protected void _Derivative_Linear(out T[] @out, CInterpolatedVarEntryBase<T> start, CInterpolatedVarEntryBase<T> end)
    {
        if (start == end || MathF.Abs(start.changetime - end.changetime) < 0.0001f)
        {
            for (int i = 0; i < maxCount; i++)
            {
                @out[i] = start.GetValue()[i] * 0;
            }
        }
    }

    protected bool ValidOrder()
    {

    }

    public bool COMPARE_HISTORY(dynamic a, dynamic b)
    {
        return varHistory[a].GetValue() == varHistory[b].GetValue();
    }
}