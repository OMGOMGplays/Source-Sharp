#define ISTEAMFRIENDS_H

namespace SourceSharp.sp.src._public.steam
{
    public class isteamfriends
    {
        public enum EFriendRelationship
        {
            k_EFriendRelationshipNone = 0,
            k_EFriendRelationshipBlocked = 1,
            k_EFriendRelationshipRequestRecipient = 2,
            k_EFriendRelationshipFriend = 3,
            k_EFriendRelationshipRequestInitiator = 4,
            k_EFriendRelationshipIgnored = 5,
            k_EFriendRelationshipIgnoredFriend = 6,
            k_EFriendRelationshipSuggested = 7,

            // keep this updated
            k_EFriendRelationshipMax = 8,
        }

        public const int k_cchMaxFriendsGroupName = 64;

        public const int k_cFriendsGroupLimit = 100;

        public const int k_cEnumerateFollowersMax = 50;

        public enum EPersonaState
        {
            k_EPersonaStateOffline = 0,         // friend is not currently logged on
            k_EPersonaStateOnline = 1,          // friend is logged on
            k_EPersonaStateBusy = 2,            // user is on, but busy
            k_EPersonaStateAway = 3,            // auto-away feature
            k_EPersonaStateSnooze = 4,          // auto-away for a long time
            k_EPersonaStateLookingToTrade = 5,  // Online, trading
            k_EPersonaStateLookingToPlay = 6,   // Online, wanting to play
            k_EPersonaStateMax,
        }

        public enum EFriendFlags
        {
            k_EFriendFlagNone = 0x00,
            k_EFriendFlagBlocked = 0x01,
            k_EFriendFlagFriendshipRequested = 0x02,
            k_EFriendFlagImmediate = 0x04,          // "regular" friend
            k_EFriendFlagClanMember = 0x08,
            k_EFriendFlagOnGameServer = 0x10,
            // k_EFriendFlagHasPlayedWith	= 0x20,	// not currently used
            // k_EFriendFlagFriendOfFriend	= 0x40, // not currently used
            k_EFriendFlagRequestingFriendship = 0x80,
            k_EFriendFlagRequestingInfo = 0x100,
            k_EFriendFlagIgnored = 0x200,
            k_EFriendFlagIgnoredFriend = 0x400,
            k_EFriendFlagSuggested = 0x800,
            k_EFriendFlagAll = 0xFFFF,
        }

        public struct FriendGameInfo_t
        {
            public steamclientpublic.CGameID m_gameID;
            public uint m_unGameIP;
            public ushort m_usGamePort;
            public ushort m_usQueryPort;
            public steamclientpublic.CSteamID m_steamIDLobby;
        }

        public static int k_cchPersonaNameMax = 128;
        public static int k_cwchPersonaNameMax = 32;

        public enum EUserRestriction
        {
            k_nUserRestrictionNone = 0,
            k_nUserRestrictionUnknown = 1,
            k_nUserRestrictionAnyChat = 2,
            k_nUserRestrictionVoiceChat = 4,
            k_nUserRestrictionGroupChat = 8,
            k_nUserRestrictionRating = 16,
            k_nUserRestrictionGameInvites = 32,
            k_nUserRestrictionTrading = 64,
        }

        public struct FriendSessionStateInfo_t
        {
            public uint m_uiOnlineSessionInstances;
            public ushort m_uiPublishedToFriendsSessionInstance;
        }

        public const uint k_cubChatMetadataMax = 8192;

        public static int k_cchMaxRichPrecenseKeys = 20;
        public static int k_cchMaxRichPrecenseKeyLength = 64;
        public static int k_cchMaxRichPrecenseValueLength = 256;

        public enum EOverlayToStoreFlag
        {
            k_EOverlayToStoreFlag_None = 0,
            k_EOverlayToStoreFlag_AddToCart = 1,
            k_EOverlayToStoreFlag_AddToCartAndShow = 2,
        }

        public class ISteamFriends
        {
            public virtual string GetPersonaName() => null;

            public virtual SteamAPICall_t SetPersonaName(string pchPersonaName) => null;

            public virtual EPersonaState GetPersonaState() => EPersonaState.k_EPersonaStateOffline;

            public virtual int GetFriendCount(int iFriendFlags) => 0;

            public virtual steamclientpublic.CSteamID GetFriendByIndex(int iFriend, int iFriendFlags) => null;

            public virtual EFriendRelationship GetFriendRelationship(steamclientpublic.CSteamID steamIDFriend) => EFriendRelationship.k_EFriendRelationshipNone;

            public virtual EPersonaState GetFriendPersonaState(steamclientpublic.CSteamID steamIDFriend) => EPersonaState.k_EPersonaStateOffline;

            public virtual string GetFriendPersonaName(steamclientpublic.CSteamID steamIDFriend) => null;

            public virtual bool GetFriendGamePlayed(steamclientpublic.CSteamID steamIDFriend, FriendGameInfo_t pFriendGameInfo) => false;
            public virtual string GetFriendPersonaNameHistory(steamclientpublic.CSteamID steamIDFriend, int iPersonaName) => null;

            public virtual string GetPlayerNickname(steamclientpublic.CSteamID steamIDPlayer) => null;

            public virtual bool HasFriend(steamclientpublic.CSteamID steamIDFriend, int iFriendFlags) => false;

            public virtual int GetClanCount() => 0;
            public virtual steamclientpublic.CSteamID GetClanByIndex(int iClan) => null;
            public virtual string GetClanName(steamclientpublic.CSteamID steamIDClan) => null;
            public virtual string GetClanTag(steamclientpublic.CSteamID steamIDClan) => null;
            public virtual bool GetClanActivityCounts(steamclientpublic.CSteamID steamIDClan, int pnOnline, int pnInGame, int pnChatting) => false;
            public virtual SteamAPICall_t DownloadClanActivityCounts(steamclientpublic.CSteamID psteamIDClans, int cClansToRequest) => null;

            public virtual int GetFriendCountFromSource(steamclientpublic.CSteamID steamIDSource) => 0;
            public virtual steamclientpublic.CSteamID GetFriendFromSourceByIndex(steamclientpublic.CSteamID steamIDSource, int iFriend) => null;

            public virtual bool IsUserInSource(steamclientpublic.CSteamID steamIDUser, steamclientpublic.CSteamID steamIDSource) => false;

            public virtual void SetInGameVoiceSpeaking(steamclientpublic.CSteamID steamIDUser, bool bSpeaking) { }

            public virtual void ActivateGameOverlay(string pchDialog) { }

            public virtual void ActivateGameOverlayToUser(string pchDialog, steamclientpublic.CSteamID steamID) { }

            public virtual void ActivateGameOverlayToWebPage(string pchURL) { }

            public virtual void ActivateGameOverlayToStore(AppId_t nAppID, EOverlayToStoreFlag eFlag) { }

            public virtual void SetPlayedWith(steamclientpublic.CSteamID steamIDUserPlayedWith) { }

            public virtual void ActivateGameOverlayInviteDialog(steamclientpublic.CSteamID steamIDLobby) { }

            public virtual int GetSmallFriendAvatar(steamclientpublic.CSteamID steamIDFriend) => 0;
            public virtual int GetMediumFriendAvatar(steamclientpublic.CSteamID steamIDFriend) => 0;
            public virtual int GetLargeFriendAvatar(steamclientpublic.CSteamID steamIDFriend) => 0;

            public virtual bool RequestUserInformation(steamclientpublic.CSteamID steamIDUser, bool bRequireNameOnly) => false;

            public virtual SteamAPICall_t RequestClanOfficerList(steamclientpublic.CSteamID steamIDClan) => null;

            public virtual steamclientpublic.CSteamID GetClanOwner(steamclientpublic.CSteamID steamIDClan) => null;
            public virtual int GetClanOfficerCount(steamclientpublic.CSteamID steamIDClan) => 0;
            public virtual steamclientpublic.CSteamID GetClanOfficerByIndex(steamclientpublic.CSteamID steamIDClan, int iOfficer) => null;
            public virtual uint GetUserRestrictions() => 0;

            public virtual bool SetRichPresence(string pchKey, string pchValue) => false;
            public virtual void ClearRichPresence() { }
            public virtual string GetFriendRichPresence(steamclientpublic.CSteamID steamIDFriend, string pchKey) => null;
            public virtual int GetFriendRichPresenceKeyCount(steamclientpublic.CSteamID steamIDFriend) => 0;
            public virtual string GetFriendRichPresenceKeyByIndex(steamclientpublic.CSteamID steamIDFriend, int iKey) => null;
            public virtual void RequestFriendRichPresence(steamclientpublic.CSteamID steamIDFriend) { }

            public virtual bool InviteUserToGame(steamclientpublic.CSteamID steamIDFriend, string pchConnectString) => false;

            public virtual int GetCoplayFriendCount() => 0;
            public virtual steamclientpublic.CSteamID GetCoplayFriend(int iCoplayFriend) => null;
            public virtual int GetFriendCoplayTime(steamclientpublic.CSteamID steamIDFriend) => 0;
            public virtual AppId_t GetFriendCoplayGame(steamclientpublic.CSteamID steamIDFriend) => null;

            public virtual SteamAPICall_t JoinClanChatRoom(steamclientpublic.CSteamID steamIDClan) => null;
            public virtual bool LeaveClanChatRoom(steamclientpublic.CSteamID steamIDClan) => false;
            public virtual int GetClanChatMemberCount(steamclientpublic.CSteamID steamIDClan) => 0;
            public virtual steamclientpublic.CSteamID GetChatMemberByIndex(steamclientpublic.CSteamID steamIDClan, int iUser) => null;
            public virtual bool SendClanChatMessage(steamclientpublic.CSteamID steamIDClanChat, string pchText) => false;
            public virtual int GetClanChatMessage(steamclientpublic.CSteamID steamIDClanChat, int iMessage, object prgchText, int cchTextMax, steamclientpublic.EChatEntryType peChatEntryType, steamclientpublic.CSteamID psteamidchatter) => 0;
            public virtual bool IsClanChatAdmin(steamclientpublic.CSteamID steamIDClanChat, steamclientpublic.CSteamID steamIDUser) => false;

            public virtual bool IsClanChatWindowOpenInSteam(steamclientpublic.CSteamID steamIDClanChat) => false;
            public virtual bool OpenClanChatWindowInSteam(steamclientpublic.CSteamID steamIDClanChat) => false;
            public virtual bool CloseClanChatWindowInSteam(steamclientpublic.CSteamID steamIDClanChat) => false;

            public virtual bool SetListenForFriendMessages(bool bInterceptEnabled) => false;
            public virtual bool ReplyToFriendsMessage(steamclientpublic.CSteamID steamIDFriend, string pchMsgToSend) => false;
            public virtual int GetFriendMessage(steamclientpublic.CSteamID steamIDFriend, int iMessageID, object pvData, int cubData, steamclientpublic.EChatEntryType peChatEntryType) => 0;

            public virtual SteamAPICall_t GetFollowerCount(steamclientpublic.CSteamID steamID) => null;
            public virtual SteamAPICall_t IsFollowing(steamclientpublic.CSteamID steamID) => null;
            public virtual SteamAPICall_t EnumerateFollowingList(steamclientpublic.CSteamID steamID) => null;
        }

        public const string STEAMFRIENDS_INTERFACE_VERSION = "SteamFriends014";

        public struct PersonaStateChange_t
        {
            public PersonaStateChange_t()
            {
                k_iCallback = isteamclient.k_iSteamFriendsCallbacks + 4;
            }

            public int k_iCallback;

            public uint m_ulSteamID;
            public int m_nChangeFlags;
        }

        public enum EPersonaChange
        {
            k_EPersonaChangeName = 0x0001,
            k_EPersonaChangeStatus = 0x0002,
            k_EPersonaChangeComeOnline = 0x0004,
            k_EPersonaChangeGoneOffline = 0x0008,
            k_EPersonaChangeGamePlayed = 0x0010,
            k_EPersonaChangeGameServer = 0x0020,
            k_EPersonaChangeAvatar = 0x0040,
            k_EPersonaChangeJoinedSource = 0x0080,
            k_EPersonaChangeLeftSource = 0x0100,
            k_EPersonaChangeRelationshipChanged = 0x0200,
            k_EPersonaChangeNameFirstSet = 0x0400,
            k_EPersonaChangeFacebookInfo = 0x0800,
            k_EPersonaChangeNickname = 0x1000,
            k_EPersonaChangeSteamLevel = 0x2000,
        }

        public struct GameOverlayActivated_t
        {
            public GameOverlayActivated_t()
            {
                k_iCallback = isteamclient.k_iSteamFriendsCallbacks + 31;
            }

            public int k_iCallback;

            public ushort m_bActive;
        }

        public struct GameServerChangeRequested_t
        {
            public GameServerChangeRequested_t()
            {
                k_iCallback = isteamclient.k_iSteamFriendsCallbacks + 32;
            }

            public int k_iCallback;

            public string m_rgchServer;
            public string m_rgchPassword;
        }

        public struct GameLobbyJoinRequested_t
        {
            public GameLobbyJoinRequested_t()
            {
                k_iCallback = isteamclient.k_iSteamFriendsCallbacks + 33;
            }

            public int k_iCallback;

            public steamclientpublic.CSteamID m_steamIDLobby;
            public steamclientpublic.CSteamID m_steamIDFriend;
        }

        public struct AvatarImageLoaded_t
        {
            public AvatarImageLoaded_t()
            {
                k_iCallback = isteamclient.k_iSteamFriendsCallbacks + 34;
            }

            public int k_iCallback;

            public steamclientpublic.CSteamID m_steamID;
            public int m_iImage;
            public int m_iWide;
            public int m_iTall;
        }

        public struct ClanOfficerListResponse_t
        {
            public ClanOfficerListResponse_t()
            {
                k_iCallback = isteamclient.k_iSteamFriendsCallbacks + 35;
            }

            public int k_iCallback;

            public steamclientpublic.CSteamID m_steamIDClan;
            public int m_cOfficers;
            public ushort m_bSuccess;
        }

        public struct FriendRichPresenceUpdate_t
        {
            public FriendRichPresenceUpdate_t()
            {
                k_iCallback = isteamclient.k_iSteamFriendsCallbacks + 36;
            }

            public int k_iCallback;

            public steamclientpublic.CSteamID m_steamIDFriend;
            public AppId_t m_nAppID;
        }

        public struct GameRichPresenceJoinRequested_t
        {
            public GameRichPresenceJoinRequested_t()
            {
                k_iCallback = isteamclient.k_iSteamFriendsCallbacks + 37;
            }

            public int k_iCallback;

            public steamclientpublic.CSteamID m_steamIDFriend;
            public string m_rgchConnect;
        }

        public struct GameConnectedClanChatMsg_t
        {
            public GameConnectedClanChatMsg_t()
            {
                k_iCallback = isteamclient.k_iSteamFriendsCallbacks + 38;
            }

            public int k_iCallback;

            public steamclientpublic.CSteamID m_steamIDClanChat;
            public steamclientpublic.CSteamID m_steamIDUser;
            public int m_iMessageID;
        }

        public struct GameConnectedChatJoin_t
        {
            public GameConnectedChatJoin_t()
            {
                k_iCallback = isteamclient.k_iSteamFriendsCallbacks + 39;
            }

            public int k_iCallback;

            public steamclientpublic.CSteamID m_steamIDClanChat;
            public steamclientpublic.CSteamID m_steamIDUser;
        }

        public struct GameConnectedChatLeave_t
        {
            public GameConnectedChatLeave_t()
            {
                k_iCallback = isteamclient.k_iSteamFriendsCallbacks + 40;
            }

            public int k_iCallback;

            public steamclientpublic.CSteamID m_steamIDClanChat;
            public steamclientpublic.CSteamID m_steamIDUser;
            public bool m_bKicked;
            public bool m_bDropped;
        }

        public struct DownloadClanActivityCountsResult_t
        {
            public DownloadClanActivityCountsResult_t()
            {
                k_iCallback = isteamclient.k_iSteamFriendsCallbacks + 41;
            }

            public int k_iCallback;

            public bool m_bSuccess;
        }

        public struct JoinClanChatRoomCompletionResult_t
        {
            public JoinClanChatRoomCompletionResult_t()
            {
                k_iCallback = isteamclient.k_iSteamFriendsCallbacks + 42;
            }

            public int k_iCallback;

            public steamclientpublic.CSteamID m_steamIDClanChat;
            public steamclientpublic.EChatRoomEnterResponse m_eChatRoomEnterResponse;
        }

        public struct GameConnectedFriendChatMsg_t
        {
            public GameConnectedFriendChatMsg_t()
            {
                k_iCallback = isteamclient.k_iSteamFriendsCallbacks + 43;
            }

            public int k_iCallback;

            public steamclientpublic.CSteamID m_steamIDUser;
            public int m_iMessageID;
        }

        public struct FriendsGetFollowerCount_t
        {
            public FriendsGetFollowerCount_t()
            {
                k_iCallback = isteamclient.k_iSteamFriendsCallbacks + 44;
            }

            public int k_iCallback;

            public steamclientpublic.EResult m_eResult;
            public steamclientpublic.CSteamID m_steamID;
            public int m_nCount;
        }

        public struct FriendsIsFollowing_t
        {
            public FriendsIsFollowing_t()
            {
                k_iCallback = isteamclient.k_iSteamFriendsCallbacks + 45;
            }

            public int k_iCallback;

            public steamclientpublic.EResult m_eResult;
            public steamclientpublic.CSteamID m_steamID;
            public bool m_bIsFollowing;
        }

        public struct FriendsEnumerateFollowingList_t
        {
            public FriendsEnumerateFollowingList_t()
            {
                k_iCallback = isteamclient.k_iSteamFriendsCallbacks + 46;
            }

            public int k_iCallback;

            public steamclientpublic.EResult m_eResult;
            public steamclientpublic.CSteamID[] m_rgSteamID = new steamclientpublic.CSteamID[k_cEnumerateFollowersMax];
            public int m_nResultsReturned;
            public int m_nTotalResultCount;
        }

        public struct SetPersonaNameResponse_t
        {
            public SetPersonaNameResponse_t()
            {
                k_iCallback = isteamclient.k_iSteamFriendsCallbacks + 47;
            }

            public int k_iCallback;

            public bool m_bSuccess;
            public bool m_bLocalSuccess;
            public steamclientpublic.EResult m_result;
        }
    }
}
