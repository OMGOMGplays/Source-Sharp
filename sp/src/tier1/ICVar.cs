namespace SourceSharp.SP.Tier1;

public interface IConsoleDisplayFunc
{
    public void ColorPrint(Color clr, string message);
    public void Print(string message);
    public void DPrint(string message);
}

public interface ICvarQuery : IAppSystem
{
    public const string CVAR_QUERY_INTERFACE_VERSION = "ICvarQuery001";

    public bool AreConVarsLinkable(ConVar child, ConVar parent);
}

public interface ICvar : IAppSystem
{
    public const string CVAR_INTERFACE_VERSION = "VEngineCvar004";

    protected ICVarIteratorIternal FactoryInternalIterator();

    public CVarDLLIdentifier AllocateDLLIdentifier();

    public void RegisterConCommand(ConCommandBase commandBase);
    public void UnregisterConCommand(ConCommandBase commandBase);
    public void UnregisterConCommand(CVarDLLIdentifier id);

    public string GetCommandLineValue(string variableName);

    public ConCommandBase FindCommandBase(string name);
    public ConVar FindVar(string var_name);
    public ConCommand FindCommand(string name);

    public ConCommandBase[] GetCommands();

    public void InstallGlobalChangeCallback(ChangeCallback callback);
    public void RemoveGlobalChangeCallback(ChangeCallback callback);
    public void CallGlobalChangeCallback(ConVar var, string oldString, float oldValue);

    public void InstallConsoleDisplayFunc(IConsoleDisplayFunc displayFunc);
    public void RemoveConsoleDisplayFunc(IConsoleDisplayFunc displayFunc);
    public void ConsoleColorPrintf(Color clr, string format, params object[] args);
    public void ConsolePrintf(string format, params object[] args);
    public void ConsoleDPrintf(string format, params object[] args);

    public void RevertFlaggedConVars(int flag);

    public void InstallCVarQuery(ICvarQuery query);
#if X360
    public void PublishToVXConsole();
#endif // X360

    public bool IsMaterialThreadSetAllowed();
    public void QueueMaterialThreadSetValue(ConVar conVar, string value);
    public void QueueMaterialThreadSetValue(ConVar conVar, int value);
    public void QueueMaterialThreadSetValue(ConVar conVar, float value);
    public bool HasQueuedMaterialThreadConVarSets();
    public int ProcessQueuedMaterialThreadConVarSets();

    public class Iterator
    {
        private ICVarIteratorIternal iter;

        public Iterator(ICvar cvar)
        {
            iter = cvar.FactoryInternalIterator();
        }

        ~Iterator()
        {
            iter = null;
        }

        public void SetFirst()
        {
            iter.SetFirst();
        }

        public void Next()
        {
            iter.Next();
        }

        public bool IsValid()
        {
            return iter.IsValid();
        }

        public ConCommandBase Get()
        {
            return iter.Get();
        }
    }

    protected interface ICVarIteratorIternal
    {
        public void SetFirst();
        public void Next();
        public bool IsValid();
        public ConCommandBase Get();
    }
}