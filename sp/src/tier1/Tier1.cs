namespace SourceSharp.SP.Tier1;

public static class Tier1
{
    public static ICvar cVar;
    public static IProcessUtils processUtils;

    public static void ConnectTier1Libraries(CreateInterface[] factoryList, int factoryCount)
    {

    }

    public static void DisconnectTier1Libraries()
    {

    }
}

public class CTier1AppSystem : CTier0AppSystem
{
    public CTier1AppSystem(bool isPrimaryAppSystem = true) : base(isPrimaryAppSystem)
    {
    }

    public virtual bool Connect(CreateInterface factory)
    {
        if (!base.Connect(factory))
        {
            return false;
        }

        if (base.IsPrimaryAppSystem())
        {
            Tier1.ConnectTier1Libraries([factory], 1);
        }

        return true;
    }

    public virtual void Disconnect(CreateInterface factory)
    {
        if (base.IsPrimaryAppSystem())
        {
            Tier1.DisconnectTier1Libraries();
        }

        base.Disconnect();
    }

    public virtual InitReturnVal Init()
    {
        InitReturnVal retVal = base.Init();

        if (retVal != InitReturnVal.INIT_OK)
        {
            return retVal;
        }

        if (Tier1.cVar != null && base.IsPrimaryAppSystem())
        {
            ConVar.ConVar_Register(conVarFlag);
        }

        return InitReturnVal.INIT_OK;
    }

    public virtual void Shutdown()
    {
        if (Tier1.cVar != null && base.IsPrimaryAppSystem())
        {
            ConVar.ConVar_Unregister();
        }

        base.Shutdown();
    }
}