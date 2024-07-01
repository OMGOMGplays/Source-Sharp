#define ISTEAMCLIENT_H

namespace SourceSharp.sp.src._public.steam
{
    using ValvePackingSentinel_t = isteamclient.ValvePackingSentinel_s;

    using HSteamPipe = int;
    using HSteamUser = int;

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

            public virtual void ReleaseUser(HSteamPipe hSteamPipe, HSteamUser hUser) {}
    }
}
}
