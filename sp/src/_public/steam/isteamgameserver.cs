#define ISTEAMGAMESERVER_H

namespace SourceSharp.sp.src._public.steam
{
    public class isteamgameserver
    {
        public const uint MASTERSERVERUPDATEPORT_USEGAMESOCKETSHARE = /*(uint)-1*/ 0; // FIXME: unchecked cannot be used outside of methods - find out a way to have the negative uint value!

        public class ISteamGameServer
        {
            public virtual bool InitGameServer(uint unIP, ushort usGamePort, ushort usQueryPort, uint unFlags, AppId_t nGameAppId, string pchVersionString) => false;

            public virtual void SetProduct(string pszProduct) { }

            public virtual void SetGameDescription(string pszGameDescription) { }

            public virtual void SetModDir(string pszModDir) { }

            public virtual void SetDedicatedServer(bool bDedicated) { }

            public virtual void LogOn(string pszToken) { }

            public virtual void LogOnAnonymous() { }

            public virtual void LogOff() { }

            public virtual bool BLoggedOn() => false;
            public virtual bool BSecure() => false;
            public virtual steamclientpublic.CSteamID GetSteamID() => null;

            public virtual bool WasRestartRequested() => false;

            public virtual void SetMaxPlayerCount(int cPlayersMax) { }

            public virtual void SetBotPlayerCount(int cBotPlayers) { }

            public virtual void SetServerName(string pszServerName) { }

            public virtual void SetMapName(string pszMapName) { }

            public virtual void SetPasswordProtected(bool bPasswordProtected) { }

            public virtual void SetSpectatorPort(ushort unSpectatorPort) { }

            public virtual void SetSpectatorServerName(string pszSpectatorServerName) { }

            public virtual void ClearAllValues() { }

            public virtual void SetKeyValue(string pKey, string pValue) { }

            public virtual void SetGameTags(string pchGameTags) { }

            public virtual void SetGameData(string pchGameData) { }

            public virtual void SetRegion(string pszRegion) { }

            public virtual bool SendUserConnectAndAuthenticate(uint unIPClient, object pvAuthBlob, uint cubAuthBlobSize, steamclientpublic.CSteamID pSteamIDUser) => false;

            public virtual steamclientpublic.CSteamID CreateUnauthenticatedUserConnection() => null;

            public virtual void SendUserDisconnect(steamclientpublic.CSteamID steamIDUser) { }

            public virtual bool BUpdateUserData(steamclientpublic.CSteamID steamIDUser, string pchPlayerName, uint uScore) => false;

            public virtual HAuthTicket GetAuthSessionTicket(object pTicket, int cbMaxTicket, uint pcbTicket) => 0;

            public virtual steamclientpublic.EBeginAuthSessionResult BeginAuthSession(object pAuthTicket, int cbAuthTicket, steamclientpublic.CSteamID steamID) => steamclientpublic.EBeginAuthSessionResult.k_EBeginAuthSessionResultOK; // <- 0

            public virtual void EndAuthSession(steamclientpublic.CSteamID steamID) { }

            public virtual void CancelAuthTicket(HAuthTicket hAuthTicket) { }

            public virtual steamclientpublic.EUserHasLicenseForAppResult UserHasLicenseForApp(steamclientpublic.CSteamID steamID, AppId_t appID) => steamclientpublic.EUserHasLicenseForAppResult.k_EUserHasLicenseResultHasLicense; // <- 0

            public virtual bool RequestUserGroupStatus(steamclientpublic.CSteamID steamIDUser, steamclientpublic.CSteamID steamIDGroup) => false;

            public virtual void GetGameplayStats() { }
            public virtual SteamAPICall_t GetServerReputation() => null;

            public virtual uint GetPublicIP() => 0;

            public virtual bool HandleIncomingPacket(object pData, int cbData, uint srcIP, ushort srcPort) => false;

            public virtual int GetNextOutgoingPacket(object pOut, int cbMaxOut, uint pNetAdr, ushort pPort) => 0;

            public virtual void EnableHeartbeats(bool bActive) { }

            public virtual void SetHeartbeatsInterval(int iHeartbeatInterval) { }

            public virtual SteamAPICall_t AssociateWithClan(steamclientpublic.CSteamID steamIDClan) => null;

            public virtual SteamAPICall_t ComputeNewPlayerCompatibility(steamclientpublic.CSteamID steamIDNewPlayer) => null;
        }

        public const string STEAMGAMESERVER_INTERFACE_VERSION = "SteamGameServer012";

        public const uint k_unServerFlagNone        = 0x00;
        public const uint k_unServerFlagActive      = 0x01;
        public const uint k_unServerFlagSecure      = 0x02;
        public const uint k_unServerFlagDedicated   = 0x04;
        public const uint k_unServerFlagLinux       = 0x08;
        public const uint k_unServerFlagPassworded  = 0x10;
        public const uint k_unServerFlagPrivate     = 0x20;

        public struct GSClientApprove_t
        {
            public GSClientApprove_t()
            {
                k_iCallback = isteamclient.k_iSteamGameServerCallbacks + 1;
            }

            public int k_iCallback;

            public steamclientpublic.CSteamID m_SteamID;
            public steamclientpublic.CSteamID m_OwnerSteamID;
        }

        public struct GSClientDeny_t
        {
            public GSClientDeny_t()
            {
                k_iCallback = isteamclient.k_iSteamGameServerCallbacks + 2;
            }

            public int k_iCallback;

            public steamclientpublic.CSteamID m_SteamID;
            public steamclientpublic.EDenyReason m_eDenyReason;
            public string m_rgchOptionalText;
        }

        public struct GSClientKick_t
        {
            public GSClientKick_t()
            {
                k_iCallback = isteamclient.k_iSteamGameServerCallbacks + 3;
            }

            public int k_iCallback;

            public steamclientpublic.CSteamID m_SteamID;
            public steamclientpublic.EDenyReason m_eDenyReason;
        }

        public struct GSClientAchievementStatus_t
        {
            public GSClientAchievementStatus_t()
            {
                k_iCallback = isteamclient.k_iSteamGameServerCallbacks + 6;
            }

            public int k_iCallback;

            public ulong m_SteamID;
            public string m_pchAchievement;
            public bool m_bUnlocked;
        }

        public struct GSPolicyResponse_t
        {
            public GSPolicyResponse_t()
            {
                k_iCallback = isteamclient.k_iSteamGameServerCallbacks + 15;
            }

            public int k_iCallback;

            public ushort m_bSecure;
        }

        public struct GSGameplayStats_t
        {
            public GSGameplayStats_t()
            {
                k_iCallback = isteamclient.k_iSteamGameServerCallbacks + 7;
            }

            public int k_iCallback;

            public steamclientpublic.EResult m_eResult;
            public int m_nRank;
            public uint m_unTotalConnects;
            public uint m_unTotalMinutesPlayed;
        }

        public struct GSClientGroupStatus_t
        {
            public GSClientGroupStatus_t()
            {
                k_iCallback = isteamclient.k_iSteamGameServerCallbacks + 8;
            }

            public int k_iCallback;

            public steamclientpublic.CSteamID m_SteamIDUser;
            public steamclientpublic.CSteamID m_SteamIDGroup;
            public bool m_bMember;
            public bool m_bOfficer;
        }

        public struct GSReputation_t
        {
            public GSReputation_t()
            {
                k_iCallback = isteamclient.k_iSteamGameServerCallbacks + 9;
            }

            public int k_iCallback;

            public steamclientpublic.EResult m_eResult;
            public uint m_unReputationScore;
            public bool m_bBanned;

            public uint m_unBannedIP;
            public ushort m_usBannedPort;
            public ulong m_ulBannedGameID;
            public uint m_unBanExpires;
        }

        public struct AssociateWithClanResult_t
        {
            public AssociateWithClanResult_t()
            {
                k_iCallback = isteamclient.k_iSteamGameServerCallbacks + 10;
            }

            public int k_iCallback;

            public steamclientpublic.EResult m_eResult;
        }

        public struct ComputeNewPlayerCompatibilityResult_t
        {
            public ComputeNewPlayerCompatibilityResult_t()
            {
                k_iCallback = isteamclient.k_iSteamGameServerCallbacks + 11;
            }

            public int k_iCallback;

            public steamclientpublic.EResult m_eResult;
            public int m_cPlayersThatDontLikeCandidate;
            public int m_cPlayersThatCandidateDoesntLike;
            public int m_cClanPlayersThatDontLikeCandidate;
            public steamclientpublic.CSteamID m_SteamIDCandidate;
        }
    }
}
