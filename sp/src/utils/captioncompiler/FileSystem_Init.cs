using SourceSharp.SP.Tier1;
using System.Runtime.InteropServices;

namespace SourceSharp.SP.Utils.CaptionCompiler;

public enum FSReturnCode
{
    FS_OK,
    FS_MISSING_GAMEINFO_FILE,
    FS_INVALID_GAMEINFO_FILE,
    FS_UNABLE_TO_INIT,
    FS_MISSING_STEAM_DLL
}

public enum FSErrorMode
{
    FS_ERRORMODE_AUTO, // Call Dbg.Error() in case of an error.
    FS_ERRORMODE_VCONFIG, // Call Dbg.Error() for errors and run vconfig when appropriate.
    FS_ERRORMODE_NONE // Just return FSReturnCode values and setup the string for FileSystem_GetLastErrorString.
}

public class CFSSteamSetupInfo
{
    /// <summary>
    /// If this is set, then the init code will look in this directory up to the root for gameinfo.txt. <br/>
    /// It must be set for FileSystem_LoadSearchPaths to work. <br/>
    /// <br/>
    /// (<see langword="default"/>: <see langword="null"/>)
    /// </summary>
    public string directoryName;
    
    /// <summary>
    /// If this is <see langword="true"/>, then it won't look at -vproject, -game, or the vproject environment variable
    /// to find gameinfo.txt. <br/>
    /// If this is <see langword="true"/>, then <see cref="directoryName"/> must be set.
    /// 
    /// (<see langword="default"/>: <see langword="false"/>)
    /// </summary>
    public bool onlyUseDirectoryName;
    
    /// <summary>
    /// If this is <see langword="true"/>, then:<br/>
    /// 1. It will set the environment variables that steam.dll looks at for startup info.<br/>
    /// 2. It will look for ToolsAppId in the gameinfo.txt file and load the
    /// steam caches associated with that cache if it's there.<br/>
    /// This is so apps like Hammer and hlmv can load the main steam caches (like for Counter-Strike
    /// or Half-Life 2), and also load the caches that include tools-specific materials
    /// (materials\editor, materials\debug, etc).<br/>
    /// <br/>
    /// (<see langword="default"/>: <see langword="true"/> - should be <see langword="false"/> for the engine)
    /// </summary>
    public bool toolsMode;
    
    public bool steam;
    
    public bool noGameInfo;
    
    public string gameInfoPath;

    public CFSSteamSetupInfo()
    {

    }
}

public class CTempEnvVar
{
    private bool restoreOriginValue;
    private string varName;
    private bool existed;
    private CUtlVector<byte> originalValue;

    public CTempEnvVar(string varName)
    {
        restoreOriginValue = true;
        this.varName = varName;

        string value = null;

        string buf = null;

        if (GetEnvironmentVariable(this.varName, buf, Marshal.SizeOf(buf)) != 0)
        {
            value = buf;
        }

        if (value != null)
        {
            existed = true;
            originalValue.SetSize(value.Length + 1);
            originalValue.Base() = value;
        }
        else
        {
            existed = false;
        }
    }

    ~CTempEnvVar()
    {
        if (restoreOriginValue)
        {
            if (existed)
            {
                SetValue("{0}", originalValue.Base());
            }
            else
            {
                ClearValue();
            }
        }
    }

    public void SetRestoreOriginalValue(bool restore)
    {
        restoreOriginValue = restore;
    }

    public int GetValue(byte[] buf, int bufSize)
    {
        if (buf == null || bufSize <= 0)
        {
            return 0;
        }

        return GetEnvironmentVariable(varName, buf, bufSize);
    }

    public void SetValue(string format, params object[] args)
    {
        // FIXME: This couldn't possibly be what's needed...
        string valueString = string.Format("{0}={1}", varName, args);
    }

    public void ClearValue()
    {
        string str = $"{varName}=";
    }
}

public class CSteamEnvVars
{
    public CTempEnvVar steamAppId;
    public CTempEnvVar steamUserPassphrase;
    public CTempEnvVar steamAppUser;
    public CTempEnvVar path;

    public CSteamEnvVars()
    {
        steamAppId = new CTempEnvVar("SteamAppId");
        steamUserPassphrase = new CTempEnvVar("SteamUserPassphrase");
        steamAppUser = new CTempEnvVar("SteamAppUser");
        path = new CTempEnvVar("path");
    }

    public void SetRestoreOriginalValue_ALL(bool restore)
    {
        steamAppId.SetRestoreOriginalValue(restore);
        steamUserPassphrase.SetRestoreOriginalValue(restore);
        steamAppUser.SetRestoreOriginalValue(restore);
        path.SetRestoreOriginalValue(restore);
    }
}