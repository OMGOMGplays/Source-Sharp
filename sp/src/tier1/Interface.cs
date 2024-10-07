namespace SourceSharp.SP.Tier1;

public interface IBaseInterface;

public delegate void CreateInterface(string name, int returnCode);
public delegate void InstantiateInterface();

public class InterfaceReg
{
    public InstantiateInterface createFn;
    public string name;

    public InterfaceReg next;
    public static InterfaceReg[] interfaceRegs;

    public InterfaceReg(InstantiateInterface fn, string name)
    {

    }
}

public enum InterfaceReturnVal
{
    IFACE_OK = 0,
    IFACE_FAILED
}

public static class Interface
{
    public static object CreateInterface(string name, int returnCode)
    {

    }

#if X360
    public static object CreateInterfaceThunk(string name, int returnCode)
    {

    }
#endif // X360

    public static CreateInterface Sys_GetFactory(CSysModule module)
    {

    }

    public static CreateInterface Sys_GetFactory(string moduleName)
    {

    }

    public static CreateInterface Sys_GetFactoryThis()
    {

    }

    public static CSysModule Sys_LoadModule(string moduleName, Sys_Flags flags = Sys_Flags.SYS_NOFLAGS)
    {

    }

    public static void Sys_UnloadModule(CSysModule module)
    {

    }

    public static bool Sys_LoadInterface(string moduleName, string interfaceVersionName, out CSysModule outModule, out object outInterface)
    {

    }

    public static bool Sys_IsDebuggerPresent()
    {

    }
}

public enum Sys_Flags
{
    SYS_NOFLAGS = 0x00,
    SYS_NOLOAD = 0x01
}

public class CDllDemandLoader
{
    private string moduleName;
    private CSysModule module;
    private bool loadAttempted;

    public CDllDemandLoader(string moduleName)
    {

    }

    ~CDllDemandLoader()
    {

    }

    public CreateInterface GetFactory()
    {

    }

    public void Unload()
    {

    }
}