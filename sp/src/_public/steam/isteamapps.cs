#define ISTEAMAPPS_H

namespace SourceSharp.sp.src._public.steam
{
    public class isteamapps
    {
        public const int k_cubAppProofOfPurchaseKeyMax = 64;

        public class ISteamApps
        {
            public virtual bool BIsSubscribed() => false;
            public virtual bool BIsLowViolence() => false;
            public virtual bool BIsCybercafe() => false;
            public virtual bool BIsVACBanned() => false;
            public virtual string GetCurrentGameLanguage() => null;
            public virtual string GetAvailableGameLanguages() => null;

            public virtual bool BIsSubscribedApp(AppId_t appID) => false;

            public virtual bool BIsDlcInstalled(AppId_t appID) => false;

            public virtual uint GetEarliestPurchaseUnixTime(AppId_t nAppID) => 0;

            public virtual bool BIsSubscribedFromFreeWeekend() => false;

            public virtual int GetDLCCount() => 0;

            public virtual bool BGetDLCDataByIndex(int iDLC, AppId_t pAppID, bool pbAvailable, string pchName, int cchNameBufferSize) => false;

            public virtual void InstallDLC(AppId_t nAppID) { }
            public virtual void UninstallDLC(AppId_t nAppID) { }

            public virtual void RequestAppProofOfPurchaseKey(AppId_t nAppID) { }

            public virtual bool GetCurrentBetaName(string pchName, int cchNameBufferSize) => false;
            public virtual bool MarkContentCorrupt(bool bMissingFilesOnly) => false;
            public virtual uint GetInstalledDepots(AppId_t appID, DepotId_t pvecDepots, uint cMaxDepots) => 0;

            public virtual uint GetAppInstallDir(AppId_t appID, string pchFolder, uint cchFolderBufferSize) => 0;
            public virtual bool BIsAppInstalled(AppId_t appID) => false;

            public virtual steamclientpublic.CSteamID GetAppOwner() => null;

            public virtual string GetLaunchQueryParam(string pchKey) => null;

#if _PS3
            public virtual SteamAPICall_t RegisterActivationCode(string pchActivationCode) => null;
#endif // _PS3
        }

        public const string STEAMAPPS_INTERFACE_VERSION = "STEAMAPPS_INTERFACE_VERSION006";

        public struct DlcInstalled_t
        {
            public DlcInstalled_t()
            {
                k_iCallback = isteamclient.k_iSteamAppsCallbacks + 5;
            }

            public int k_iCallback;

            public AppId_t m_nAppID;
        }

        public enum ERegisterActivationCodeResult
        {
            k_ERegisterActivationCodeResultOK = 0,
            k_ERegisterActivationCodeResultFail = 1,
            k_ERegisterActivationCodeResultAlreadyRegistered = 2,
            k_ERegisterActivationCodeResultTimeout = 3,
            k_ERegisterActivationCodeAlreadyOwned = 4,
        }

        public struct RegisterActivationCodeResponse_t
        {
            public RegisterActivationCodeResponse_t()
            {
                k_iCallback = isteamclient.k_iSteamAppsCallbacks + 8;
            }

            public int k_iCallback;

            public ERegisterActivationCodeResult m_eResult;
            public uint m_unPackageRegistered;
        }

        public struct AppProofOfPurchaseKeyResponse_t
        {
            public AppProofOfPurchaseKeyResponse_t()
            {
                k_iCallback = isteamclient.k_iSteamAppsCallbacks + 13;
            }

            public int k_iCallback;

            public steamclientpublic.EResult m_eResult;
            public uint m_nAppID;
            public string m_rgchKey;
        }

        public struct NewLaunchQueryParameters_t
        {
            public NewLaunchQueryParameters_t()
            {
                k_iCallback = isteamclient.k_iSteamAppsCallbacks + 14;
            }

            public int k_iCallback;
        }
    }
}
