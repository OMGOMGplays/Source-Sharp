#define ISTEAMAPPLIST_H

namespace SourceSharp.sp.src._public.steam
{
    public class isteamapplist
    {
        public class ISteamAppList
        {
            public virtual uint GetNumInstalledApps() => 0;
            public virtual uint GetInstalledApps(AppId_t pvecAppID, uint unMaxAppIDs) => 0;

            public virtual int GetAppName(AppId_t nAppID, string pchName, int cchNameMax) => 0;
            public virtual int GetAppInstallDir(AppId_t nAppID, string pchDirectory, int cchNameMax) => 0;

            public virtual int GetAppBuildId(AppId_t nAppID) => 0;
        }

        public const string STEAMAPPLIST_INTERFACE_VERSION = "STEAMAPPLIST_INTERFACE_VERSION001";

        static isteamapplist()
        {
            isteamclient.DEFINE_CALLBACK(SteamAppInstalled_t, k_iSteamAppListCallbacks + 1);
            isteamclient.CALLBACK_MEMBER(0, AppId_t, m_nAppID);
            isteamclient.END_DEFINE_CALLBACK_1();

            isteamclient.DEFINE_CALLBACK(SteamAppUninstalled_t, k_iSteamAppListCallbacks + 2);
            isteamclient.CALLBACK_MEMBER(0, AppId_t, m_nAppID);
            isteamclient.END_DEFINE_CALLBACK_1();
        }
    }
}
