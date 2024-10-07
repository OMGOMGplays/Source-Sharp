namespace SourceSharp.SP.Game.Client;

public class CInterpolationContext
{
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
}

public interface IInterpolatedVar
{
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

public struct CInterpolatedVarEntryBase
{

}