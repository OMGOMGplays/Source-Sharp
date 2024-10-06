namespace SourceSharp.SP.Tier1;

public interface IConVar
{
    public const int FCVAR_NONE = 0;

    public const int FCVAR_UNREGISTERED = 1 << 0;
    public const int FCVAR_DEVELOPMENTONLY = 1 << 1;
    public const int FCVAR_GAMEDLL = 1 << 2;
    public const int FCVAR_CLIENTDLL = 1 << 3;
    public const int FCVAR_HIDDEN = 1 << 4;

    public const int FCVAR_PROTECTED = 1 << 5;
    public const int FCVAR_SPONLY = 1 << 6;
    public const int FCVAR_ARCHIVE = 1 << 7;
    public const int FCVAR_NOTIFY = 1 << 8;
    public const int FCVAR_USERINFO = 1 << 9;
    public const int FCVAR_CHEAT = 1 << 14;

    public const int FCVAR_PRINTABLEONLY = 1 << 10;
    public const int FCVAR_UNLOGGED = 1 << 11;
    public const int FCVAR_NEVER_AS_STRING = 1 << 12;

    public const int FCVAR_REPLICATED = 1 << 13;
    public const int FCVAR_DEMO = 1 << 16;
    public const int FCVAR_DONTRECORD = 1 << 17;
    public const int FCVAR_RELOAD_MATERIALS = 1 << 20;
    public const int FCVAR_RELOAD_TEXTURES = 1 << 21;

    public const int FCVAR_NOT_CONNECTED = 1 << 22;
    public const int FCVAR_MATERIAL_SYSTEM_THREAD = 1 << 23;
    public const int FCVAR_ARCHIVE_XBOX = 1 << 24;

    public const int FCVAR_ACCESSIBLE_FROM_THREADS = 1 << 25;

    public const int FCVAR_SERVER_CAN_EXECUTE = 1 << 28;
    public const int FCVAR_SERVER_CANNOT_QUERY = 1 << 29;
    public const int FCVAR_CLIENTCMD_CAN_EXECUTE = 1 << 30;

    public const int FCVAR_MATERIAL_THREAD_MASK = FCVAR_RELOAD_MATERIALS | FCVAR_RELOAD_TEXTURES | FCVAR_MATERIAL_SYSTEM_THREAD;

    public delegate void ChangeCallback(IConVar var, string oldValue, float flOldValue);

    public void SetValue(string value);
    public void SetValue(float value);
    public void SetValue(int value);

    public string GetName();

    public bool IsFlagSet(int flag);
}