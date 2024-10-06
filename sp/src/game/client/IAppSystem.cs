namespace SourceSharp.SP.Game.Client;

public enum InitReturnVal
{
    INIT_FAILED = 0,
    INIT_OK,

    INIT_LAST_VAL
}

public interface IAppSystem
{
    public bool Connect(CreateInterface factory);
    public void Disconnect();

    public nint QueryInterface(string interfaceName);

    public InitReturnVal Init();
    public void Shutdown();
}

public class CBaseAppSystem : IAppSystem
{
    public bool Connect(CreateInterface)
    {
        return true;
    }

    public void Disconnect()
    {

    }

    public nint QueryInterface(string interfaceName)
    {
        return 0;
    }

    public InitReturnVal Init()
    {
        return InitReturnVal.INIT_OK;
    }

    public void Shutdown()
    {

    }
}

public class CTier0AppSystem : CBaseAppSystem
{
    private bool isPrimaryAppSystem;

    public CTier0AppSystem(bool isPrimaryAppSystem = true)
    {
        this.isPrimaryAppSystem = isPrimaryAppSystem;
    }

    protected bool IsPrimaryAppSystem()
    {
        return isPrimaryAppSystem;
    }
}

public interface IAppSystemV0
{
    public bool Connect(CreateInterface factory);
    public void Disconnect();

    public nint QueryInterface(string interfaceName);

    public InitReturnVal Init();
    public void Shutdown();
}