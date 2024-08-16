namespace Tier1;

public class tier1
{
    public static ICvar cvar;
    public static ICvar g_pCVar;
    public static IProcessUtils g_pProcessUtils;
    public static bool s_bConnected = false;

    public static void ConnectTier1Libraries(CreateInterfaceFn pFactoryList, int nFactoryCount)
    {
        if (s_bConnected)
        {
            return;
        }

        s_bConnected = true;

        for (int i = 0; i < nFactoryCount; i++)
        {
            if (g_pCVar == null)
            {
                cvar = g_pCVar = (ICvar)pFactoryList[i](icvar.CVAR_INTERFACE_VERSION, null);
            }
            if (g_pProcessUtils == null)
            {
                g_pProcessUtils = (IProcessUtils)pFactoryList[i](iprocessutils.PROCESS_UTILS_INTERFACE_VERSION, null);
            }
        }
    }

    public static void DisconnectTier1Libraries()
    {
        if (!s_bConnected)
        {
            return;
        }

        g_pCVar = cvar = null;
        g_pProcessUtils = null;
        s_bConnected = false;
    }
}

public class CTier1AppSystem<IInterface> : CTier0AppSystem<IInterface>
{
    public static int ConVarFlag { get; set; }

    public CTier0AppSystem<IInterface> BaseClass;

    public CTier1AppSystem(bool bIsPrimaryAppSystem = true)
    {
        BaseClass = new(bIsPrimaryAppSystem);
    }

    public virtual bool Connect(CreateInterfaceFn factory)
    {
        if (!BaseClass.Connect(factory))
        {
            return false;
        }

        if (BaseClass.IsPrimaryAppSystem())
        {
            tier1.ConnectTier1Libraries(factory, 1);
        }

        return true;
    }

    public virtual void Disconnect()
    {
        if (BaseClass.IsPrimaryAppSystem())
        {
            tier1.DisconnectTier1Libraries();
        }

        BaseClass.Disconnect();
    }

    public virtual InitReturnVal_t Init()
    {
        InitReturnVal_t nRetVal = BaseClass.Init();

        if (nRetVal != InitReturnVal_t.INIT_OK)
        {
            return nRetVal;
        }

        if (tier1.g_pCVar != null && BaseClass.IsPrimaryAppSystem())
        {
            ConVar_Register(ConVarFlag);
        }

        return InitReturnVal_t.INIT_OK;
    }

    public virtual void Shutdown()
    {
        if (tier1.g_pCVar != null && BaseClass.IsPrimaryAppSystem())
        {
            ConVar_Unregister();
        }

        BaseClass.Shutdown();
    }
}