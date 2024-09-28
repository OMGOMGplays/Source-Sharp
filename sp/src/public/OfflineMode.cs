namespace SourceSharp.SP.Public;

public class OfflineMode
{
    public const string STEAM_OFFLINE_MODE = "HKEY_CURRENT_USER\\Software\\Valve\\Steam\\Offline";
    public const string STEAM_AFS_MODE = "HKEY_CURRENT_USER\\Softwate\\Valve\\Steam\\OfflineAFS";
    public const string OFFLINE_FILE = "Steam\\cached\\offline_";

    public static bool IsSteamInOfflineMode()
    {
        int offline = 0;

        VGUI.System().GetRegistryInteger(STEAM_OFFLINE_MODE, out offline);

        return offline == 1;
    }

    public static bool IsSteamInAuthenticationFailSafeMode()
    {
        int offline = 0;

        VGUI.System().GetRegistryInteger(STEAM_AFS_MODE, out offline);

        return offline == 1;
    }

    public static bool IsSteamGameServerBrowsingEnabled()
    {
        return IsSteamInAuthenticationFailSafeMode() || !IsSteamInOfflineMode();
    }
}