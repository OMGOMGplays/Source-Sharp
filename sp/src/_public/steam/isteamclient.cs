#define ISTEAMCLIENT_H

namespace SourceSharp.sp.src._public.steam
{
    using ValvePackingSentinel_t = isteamclient.ValvePackingSentinel_s;

    using HSteamPipe = int;
    using HSteamUser = int;
    using System.Reflection;

    public class isteamclient
    {
        public static string VALVE_COMPILE_TIME_ASSERT(bool pred) => null; // FIXME: What's the purpose of this method? How are we to implement it? (Reference `L#18 - sp/src/public/steam/isteamclient.h`)

        public static object REFERENCE(dynamic arg) => (object)arg; // FIXME: Find out the actual implementation and use-case of this method!

#if __linux__ || __APPLE__
        public const bool VALVE_CALLBACK_PACK_SMALL = true;
#else
        public const bool VALVE_CALLBACK_PACK_LARGE = true;
#endif // __linux || __APPLE__

        public struct ValvePackingSentinel_s
        {
            uint m_u32;
            ulong m_u64;
            ushort m_u16;
            double m_d;
        }

        // FIXME: Implement!
        //#if defined(VALVE_CALLBACK_PACK_SMALL)
        //VALVE_COMPILE_TIME_ASSERT( sizeof(ValvePackingSentinel_t) == 24 )
        //#elif defined(VALVE_CALLBACK_PACK_LARGE)
        //VALVE_COMPILE_TIME_ASSERT( sizeof(ValvePackingSentinel_t) == 32 )
        //#else
        //#error ???
        //#endif

        public class ISteamUser;
        public class ISteamGameServer;
        public class ISteamFriends;
        public class ISteamUtils;
        public class ISteamMatchmaking;
        public class ISteamContentServer;
        public class ISteamMatchmakingServers;
        public class ISteamUserStats;
        public class ISteamApps;
        public class ISteamNetworking;
        public class ISteamRemoteStorage;
        public class ISteamScreenshots;
        public class ISteamMusic;
        public class ISteamMusicRemote;
        public class ISteamGameServerStats;
        public class ISteamPS3OverlayRender;
        public class ISteamHTTP;
        public class ISteamUnifiedMessages;
        public class ISteamController;
        public class ISteamUGC;
        public class ISteamAppList;
        public class ISteamHTMLSurface;

        public class ISteamClient
        {
            public virtual HSteamPipe CreateSteamPipe() => 0;

            public virtual bool BReleaseSteamPipe(HSteamPipe hSteamPipe) => false;

            public virtual HSteamUser ConnectToGlobalUser(HSteamPipe hSteamPipe) => 0;

            public virtual HSteamUser CreateLocalUser(HSteamPipe phSteamPipe, steamclientpublic.EAccountType eAccountType) => 0;

            public virtual void ReleaseUser(HSteamPipe hSteamPipe, HSteamUser hUser) { }

            public virtual ISteamUser GetISteamUser(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion) => null;

            public virtual ISteamGameServer GetISteamGameServer(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion) => null;

            public virtual void SetLocalIPBinding(uint unIP, ushort usPort) { }

            public virtual ISteamFriends GetISteamFriends(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion) => null;

            public virtual ISteamUtils GetISteamUtils(HSteamPipe hSteamPipe, string pchVersion) => null;

            public virtual ISteamMatchmaking GetISteamMatchMaking(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion) => null;

            public virtual ISteamMatchmakingServers GetISteamMatchmakingServers(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion) => null;

            public virtual object GetISteamGenericInterface(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion) => null;

            public virtual ISteamUserStats GetISteamUserStats(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion) => null;

            public virtual ISteamGameServerStats GetISteamGameServerStats(HSteamUser hSteamUser, HSteamPipe hSteamPipe, int pchVersion) => null;

            public virtual ISteamApps GetISteamApps(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion) => null;

            public virtual ISteamNetworking GetISteamNetworking(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion) => null;

            public virtual ISteamRemoteStorage GetISteamRemoteStorage(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion) => null;

            public virtual ISteamScreenshots GetISteamScreenshots(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion) => null;

            public virtual void RunFrame() { }

            public virtual uint GetIPCCallCount() => 0;

            public virtual void SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction) { }

            public virtual bool BShutdownIfAllPipesClosed() => false;

#if _PS3
            public virtual ISteamPS3OverlayRender GetISteamPS3OverlayRender() => null;
#endif // _PS3

            public virtual ISteamHTTP GetISteamHTTP(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion) => null;

            public virtual ISteamUnifiedMessages GetISteamUnifiedMessages(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion) => null;

            public virtual ISteamController GetISteamController(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion) => null;

            public virtual ISteamUGC GetISteamUGC(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion) => null;

            public virtual ISteamAppList GetISteamAppList(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion) => null;

            public virtual ISteamMusic GetISteamMusic(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion) => null;

            public virtual ISteamMusicRemote GetISteamMusicRemote(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion) => null;

            public virtual ISteamHTMLSurface GetISteamHTMLSurface(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion) => null;

            public virtual void Set_SteamAPI_CPostAPIResultInProcess(SteamAPI_PostAPIResultInProcess_t func) { }
            public virtual void Remove_SteamAPI_CPostAPIResultInProcess(SteamAPI_PostAPIResultInProcess_t func) { }
            public virtual void Set_SteamAPI_CCheckCallbackRegisteredInProcess(SteamAPI_CheckCallbackRegistered_t func) { }
        }

        public const string STEAMCLIENT_INTERFACE_VERSION = "SteamClient016";

        public static int k_iSteamUserCallbacks = 100;
        public static int k_iSteamGameServerCallbacks = 200;
        public static int k_iSteamFriendsCallbacks = 300;
        public static int k_iSteamBillingCallbacks = 400;
        public static int k_iSteamMatchmakingCallbacks = 500;
        public static int k_iSteamContentServerCallbacks = 600;
        public static int k_iSteamUtilsCallbacks = 700;
        public static int k_iClientFriendsCallbacks = 800;
        public static int k_iClientUserCallbacks = 900;
        public static int k_iSteamAppsCallbacks = 1000;
        public static int k_iSteamUserStatsCallbacks = 1100;
        public static int k_iSteamNetworkingCallbacks = 1200;
        public static int k_iClientRemoteStorageCallbacks = 1300;
        public static int k_iClientDepotBuilderCallbacks = 1400;
        public static int k_iSteamGameServerItemsCallbacks = 1500;
        public static int k_iClientUtilsCallbacks = 1600;
        public static int k_iSteamGameCoordinatorCallbacks = 1700;
        public static int k_iSteamGameServerStatsCallbacks = 1800;
        public static int k_iSteam2AsyncCallbacks = 1900;
        public static int k_iSteamGameStatsCallbacks = 2000;
        public static int k_iClientHTTPCallbacks = 2100;
        public static int k_iClientScreenshotsCallbacks = 2200;
        public static int k_iSteamScreenshotsCallbacks = 2300;
        public static int k_iClientAudioCallbacks = 2400;
        public static int k_iClientUnifiedMessagesCallbacks = 2500;
        public static int k_iSteamStreamLauncherCallbacks = 2600;
        public static int k_iClientControllerCallbacks = 2700;
        public static int k_iSteamControllerCallbacks = 2800;
        public static int k_iClientParentalSettingsCallbacks = 2900;
        public static int k_iClientDeviceAuthCallbacks = 3000;
        public static int k_iClientNetworkDeviceManagerCallbacks = 3100;
        public static int k_iClientMusicCallbacks = 3200;
        public static int k_iClientRemoteClientManagerCallbacks = 3300;
        public static int k_iClientUGCCallbacks = 3400;
        public static int k_iSteamStreamClientCallbacks = 3500;
        public static int k_IClientProductBuilderCallbacks = 3600;
        public static int k_iClientShortcutsCallbacks = 3700;
        public static int k_iClientRemoteControlManagerCallbacks = 3800;
        public static int k_iSteamAppListCallbacks = 3900;
        public static int k_iSteamMusicCallbacks = 4000;
        public static int k_iSteamMusicRemoteCallbacks = 4100;
        public static int k_iClientVRCallbacks = 4200;
        public static int k_iClientReservedCallbacks = 4300;
        public static int k_iSteamReservedCallbacks = 4400;
        public static int k_iSteamHTMLSurfaceCallbacks = 4500;
        public static int k_iClientVideoCallbacks = 4600;

        public class SteamCallback_t
        {
            public SteamCallback_t()
            {

            }
        }

        // FIXME: BIG mess of #define'd code blocks, no knowledge on how to interpret them to C#... Please, figure *something* out!
    }
}
