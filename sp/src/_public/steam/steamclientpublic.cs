#define STEAMCLIENTPUBLIC_H

#define CSTEAMID_DEFINED
#define CSTEAMID_RENDERLINK_SUPPORT

namespace SourceSharp.sp.src._public.steam
{
    using HAuthTicket = uint;
    using BREAKPAD_HANDLE = IntPtr;

    public class steamclientpublic
    {
        public enum EResult
        {
            k_EResultOK = 1,                            // success
            k_EResultFail = 2,                          // generic failure 
            k_EResultNoConnection = 3,                  // no/failed network connection
            //k_EResultNoConnectionRetry = 4,			// OBSOLETE - removed
            k_EResultInvalidPassword = 5,               // password/ticket is invalid
            k_EResultLoggedInElsewhere = 6,             // same user logged in elsewhere
            k_EResultInvalidProtocolVer = 7,            // protocol version is incorrect
            k_EResultInvalidParam = 8,                  // a parameter is incorrect
            k_EResultFileNotFound = 9,                  // file was not found
            k_EResultBusy = 10,                         // called method busy - action not taken
            k_EResultInvalidState = 11,                 // called object was in an invalid state
            k_EResultInvalidName = 12,                  // name is invalid
            k_EResultInvalidEmail = 13,                 // email is invalid
            k_EResultDuplicateName = 14,                // name is not unique
            k_EResultAccessDenied = 15,                 // access is denied
            k_EResultTimeout = 16,                      // operation timed out
            k_EResultBanned = 17,                       // VAC2 banned
            k_EResultAccountNotFound = 18,              // account not found
            k_EResultInvalidSteamID = 19,               // steamID is invalid
            k_EResultServiceUnavailable = 20,           // The requested service is currently unavailable
            k_EResultNotLoggedOn = 21,                  // The user is not logged on
            k_EResultPending = 22,                      // Request is pending (may be in process, or waiting on third party)
            k_EResultEncryptionFailure = 23,            // Encryption or Decryption failed
            k_EResultInsufficientPrivilege = 24,        // Insufficient privilege
            k_EResultLimitExceeded = 25,                // Too much of a good thing
            k_EResultRevoked = 26,                      // Access has been revoked (used for revoked guest passes)
            k_EResultExpired = 27,                      // License/Guest pass the user is trying to access is expired
            k_EResultAlreadyRedeemed = 28,              // Guest pass has already been redeemed by account, cannot be acked again
            k_EResultDuplicateRequest = 29,             // The request is a duplicate and the action has already occurred in the past, ignored this time
            k_EResultAlreadyOwned = 30,                 // All the games in this guest pass redemption request are already owned by the user
            k_EResultIPNotFound = 31,                   // IP address not found
            k_EResultPersistFailed = 32,                // failed to write change to the data store
            k_EResultLockingFailed = 33,                // failed to acquire access lock for this operation
            k_EResultLogonSessionReplaced = 34,
            k_EResultConnectFailed = 35,
            k_EResultHandshakeFailed = 36,
            k_EResultIOFailure = 37,
            k_EResultRemoteDisconnect = 38,
            k_EResultShoppingCartNotFound = 39,         // failed to find the shopping cart requested
            k_EResultBlocked = 40,                      // a user didn't allow it
            k_EResultIgnored = 41,                      // target is ignoring sender
            k_EResultNoMatch = 42,                      // nothing matching the request found
            k_EResultAccountDisabled = 43,
            k_EResultServiceReadOnly = 44,              // this service is not accepting content changes right now
            k_EResultAccountNotFeatured = 45,           // account doesn't have value, so this feature isn't available
            k_EResultAdministratorOK = 46,              // allowed to take this action, but only because requester is admin
            k_EResultContentVersion = 47,               // A Version mismatch in content transmitted within the Steam protocol.
            k_EResultTryAnotherCM = 48,                 // The current CM can't service the user making a request, user should try another.
            k_EResultPasswordRequiredToKickSession = 49,// You are already logged in elsewhere, this cached credential login has failed.
            k_EResultAlreadyLoggedInElsewhere = 50,     // You are already logged in elsewhere, you must wait
            k_EResultSuspended = 51,                    // Long running operation (content download) suspended/paused
            k_EResultCancelled = 52,                    // Operation canceled (typically by user: content download)
            k_EResultDataCorruption = 53,               // Operation canceled because data is ill formed or unrecoverable
            k_EResultDiskFull = 54,                     // Operation canceled - not enough disk space.
            k_EResultRemoteCallFailed = 55,             // an remote call or IPC call failed
            k_EResultPasswordUnset = 56,                // Password could not be verified as it's unset server side
            k_EResultExternalAccountUnlinked = 57,      // External account (PSN, Facebook...) is not linked to a Steam account
            k_EResultPSNTicketInvalid = 58,             // PSN ticket was invalid
            k_EResultExternalAccountAlreadyLinked = 59, // External account (PSN, Facebook...) is already linked to some other account, must explicitly request to replace/delete the link first
            k_EResultRemoteFileConflict = 60,           // The sync cannot resume due to a conflict between the local and remote files
            k_EResultIllegalPassword = 61,              // The requested new password is not legal
            k_EResultSameAsPreviousValue = 62,          // new value is the same as the old one ( secret question and answer )
            k_EResultAccountLogonDenied = 63,           // account login denied due to 2nd factor authentication failure
            k_EResultCannotUseOldPassword = 64,         // The requested new password is not legal
            k_EResultInvalidLoginAuthCode = 65,         // account login denied due to auth code invalid
            k_EResultAccountLogonDeniedNoMail = 66,     // account login denied due to 2nd factor auth failure - and no mail has been sent
            k_EResultHardwareNotCapableOfIPT = 67,      // 
            k_EResultIPTInitError = 68,                 // 
            k_EResultParentalControlRestricted = 69,    // operation failed due to parental control restrictions for current user
            k_EResultFacebookQueryError = 70,           // Facebook query returned an error
            k_EResultExpiredLoginAuthCode = 71,         // account login denied due to auth code expired
            k_EResultIPLoginRestrictionFailed = 72,
            k_EResultAccountLockedDown = 73,
            k_EResultAccountLogonDeniedVerifiedEmailRequired = 74,
            k_EResultNoMatchingURL = 75,
            k_EResultBadResponse = 76,                  // parse failure, missing field, etc.
            k_EResultRequirePasswordReEntry = 77,       // The user cannot complete the action until they re-enter their password
            k_EResultValueOutOfRange = 78,              // the value entered is outside the acceptable range
            k_EResultUnexpectedError = 79,              // something happened that we didn't expect to ever happen
            k_EResultDisabled = 80,                     // The requested service has been configured to be unavailable
            k_EResultInvalidCEGSubmission = 81,         // The set of files submitted to the CEG server are not valid !
            k_EResultRestrictedDevice = 82,             // The device being used is not allowed to perform this action
            k_EResultRegionLocked = 83,                 // The action could not be complete because it is region restricted
            k_EResultRateLimitExceeded = 84,            // Temporary rate limit exceeded, try again later, different from k_EResultLimitExceeded which may be permanent
            k_EResultAccountLoginDeniedNeedTwoFactor = 85,  // Need two-factor code to login
            k_EResultItemDeleted = 86,                  // The thing we're trying to access has been deleted
            k_EResultAccountLoginDeniedThrottle = 87,   // login attempt failed, try to throttle response to possible attacker
            k_EResultTwoFactorCodeMismatch = 88,        // two factor code mismatch
            k_EResultTwoFactorActivationCodeMismatch = 89,	// activation code for two-factor didn't match
        }

        public enum EVoiceResult
        {
            k_EVoiceResultOK = 0,
            k_EVoiceResultNotInitialized = 1,
            k_EVoiceResultNotRecording = 2,
            k_EVoiceResultNoData = 3,
            k_EVoiceResultBufferTooSmall = 4,
            k_EVoiceResultDataCorrupted = 5,
            k_EVoiceResultRestricted = 6,
            k_EVoiceResultUnsupportedCodec = 7,
        }

        public enum EDenyReason
        {
            k_EDenyInvalid = 0,
            k_EDenyInvalidVersion = 1,
            k_EDenyGeneric = 2,
            k_EDenyNotLoggedOn = 3,
            k_EDenyNoLicense = 4,
            k_EDenyCheater = 5,
            k_EDenyLoggedInElseWhere = 6,
            k_EDenyUnknownText = 7,
            k_EDenyIncompatibleAnticheat = 8,
            k_EDenyMemoryCorruption = 9,
            k_EDenyIncompatibleSoftware = 10,
            k_EDenySteamConnectionLost = 11,
            k_EDenySteamConnectionError = 12,
            k_EDenySteamResponseTimedOut = 13,
            k_EDenySteamValidationStalled = 14,
            k_EDenySteamOwnerLeftGuestUser = 15,
        }

        public const HAuthTicket k_HAuthTicketInvalid = 0;

        public enum EBeginAuthSessionResult
        {
            k_EBeginAuthSessionResultOK = 0,                        // Ticket is valid for this game and this steamID.
            k_EBeginAuthSessionResultInvalidTicket = 1,             // Ticket is not valid.
            k_EBeginAuthSessionResultDuplicateRequest = 2,          // A ticket has already been submitted for this steamID
            k_EBeginAuthSessionResultInvalidVersion = 3,            // Ticket is from an incompatible interface version
            k_EBeginAuthSessionResultGameMismatch = 4,              // Ticket is not for this game
            k_EBeginAuthSessionResultExpiredTicket = 5,				// Ticket has expired
        }

        public enum EAuthSessionResponse
        {
            k_EAuthSessionResponseOK = 0,                           // Steam has verified the user is online, the ticket is valid and ticket has not been reused.
            k_EAuthSessionResponseUserNotConnectedToSteam = 1,      // The user in question is not connected to steam
            k_EAuthSessionResponseNoLicenseOrExpired = 2,           // The license has expired.
            k_EAuthSessionResponseVACBanned = 3,                    // The user is VAC banned for this game.
            k_EAuthSessionResponseLoggedInElseWhere = 4,            // The user account has logged in elsewhere and the session containing the game instance has been disconnected.
            k_EAuthSessionResponseVACCheckTimedOut = 5,             // VAC has been unable to perform anti-cheat checks on this user
            k_EAuthSessionResponseAuthTicketCanceled = 6,           // The ticket has been canceled by the issuer
            k_EAuthSessionResponseAuthTicketInvalidAlreadyUsed = 7, // This ticket has already been used, it is not valid.
            k_EAuthSessionResponseAuthTicketInvalid = 8,            // This ticket is not from a user instance currently connected to steam.
            k_EAuthSessionResponsePublisherIssuedBan = 9,			// The user is banned for this game. The ban came via the web api and not VAC
        }

        public enum EUserHasLicenseForAppResult
        {
            k_EUserHasLicenseResultHasLicense = 0,                  // User has a license for specified app
            k_EUserHasLicenseResultDoesNotHaveLicense = 1,          // User does not have a license for the specified app
            k_EUserHasLicenseResultNoAuth = 2,						// User has not been authenticated
        }

        public enum EAccountType
        {
            k_EAccountTypeInvalid = 0,
            k_EAccountTypeIndividual = 1,       // single user account
            k_EAccountTypeMultiseat = 2,        // multiseat (e.g. cybercafe) account
            k_EAccountTypeGameServer = 3,       // game server account
            k_EAccountTypeAnonGameServer = 4,   // anonymous game server account
            k_EAccountTypePending = 5,          // pending
            k_EAccountTypeContentServer = 6,    // content server
            k_EAccountTypeClan = 7,
            k_EAccountTypeChat = 8,
            k_EAccountTypeConsoleUser = 9,      // Fake SteamID for local PSN account on PS3 or Live account on 360, etc.
            k_EAccountTypeAnonUser = 10,

            // Max of 16 items in this field
            k_EAccountTypeMax
        }

        public enum EAppReleaseState
        {
            k_EAppReleaseState_Unknown = 0, // unknown, required appinfo or license info is missing
            k_EAppReleaseState_Unavailable = 1, // even if user 'just' owns it, can see game at all
            k_EAppReleaseState_Prerelease = 2,  // can be purchased and is visible in games list, nothing else. Common appInfo section released
            k_EAppReleaseState_PreloadOnly = 3, // owners can preload app, not play it. AppInfo fully released.
            k_EAppReleaseState_Released = 4,	// owners can download and play app.
        }

        public enum EAppOwnershipFlags
        {
            k_EAppOwnershipFlags_None = 0x0000, // unknown
            k_EAppOwnershipFlags_OwnsLicense = 0x0001,  // owns license for this game
            k_EAppOwnershipFlags_FreeLicense = 0x0002,  // not paid for game
            k_EAppOwnershipFlags_RegionRestricted = 0x0004, // owns app, but not allowed to play in current region
            k_EAppOwnershipFlags_LowViolence = 0x0008,  // only low violence version
            k_EAppOwnershipFlags_InvalidPlatform = 0x0010,  // app not supported on current platform
            k_EAppOwnershipFlags_SharedLicense = 0x0020,    // license was granted by authorized local device
            k_EAppOwnershipFlags_FreeWeekend = 0x0040,  // owned by a free weekend licenses
            k_EAppOwnershipFlags_RetailLicense = 0x0080,    // has a retail license for game, (CD-Key etc)
            k_EAppOwnershipFlags_LicenseLocked = 0x0100,    // shared license is locked (in use) by other user
            k_EAppOwnershipFlags_LicensePending = 0x0200,   // owns app, but transaction is still pending. Can't install or play
            k_EAppOwnershipFlags_LicenseExpired = 0x0400,   // doesn't own app anymore since license expired
            k_EAppOwnershipFlags_LicensePermanent = 0x0800, // permanent license, not borrowed, or guest or freeweekend etc
            k_EAppOwnershipFlags_LicenseRecurring = 0x1000, // Recurring license, user is charged periodically
            k_EAppOwnershipFlags_LicenseCanceled = 0x2000,	// Mark as canceled, but might be still active if recurring
        }

        public enum EAppType : uint
        {
            k_EAppType_Invalid = 0x000, // unknown / invalid
            k_EAppType_Game = 0x001,    // playable game, default type
            k_EAppType_Application = 0x002, // software application
            k_EAppType_Tool = 0x004,    // SDKs, editors & dedicated servers
            k_EAppType_Demo = 0x008,    // game demo
            k_EAppType_Media_DEPRECATED = 0x010,    // legacy - was used for game trailers, which are now just videos on the web
            k_EAppType_DLC = 0x020, // down loadable content
            k_EAppType_Guide = 0x040,   // game guide, PDF etc
            k_EAppType_Driver = 0x080,  // hardware driver updater (ATI, Razor etc)
            k_EAppType_Config = 0x100,  // hidden app used to config Steam features (backpack, sales, etc)
            k_EAppType_Film = 0x200,    // A Movie (feature film)
            k_EAppType_TVSeries = 0x400,    // A TV or other video series which will have episodes and perhaps seasons
            k_EAppType_Video = 0x800,   // A video component of either a Film or TVSeries (may be the feature, an episode, preview, making-of, etc)
            k_EAppType_Plugin = 0x1000, // Plug-in types for other Apps
            k_EAppType_Music = 0x2000,  // Music files

            k_EAppType_Shortcut = 0x40000000,   // just a shortcut, client side only
            k_EAppType_DepotOnly = 0x80000000,	// placeholder since depots and apps share the same namespace
        }

        public enum ESteamUserStatType
        {
            k_ESteamUserStatTypeINVALID = 0,
            k_ESteamUserStatTypeINT = 1,
            k_ESteamUserStatTypeFLOAT = 2,
            // Read as FLOAT, set with count / session length
            k_ESteamUserStatTypeAVGRATE = 3,
            k_ESteamUserStatTypeACHIEVEMENTS = 4,
            k_ESteamUserStatTypeGROUPACHIEVEMENTS = 5,

            // max, for sanity checks
            k_ESteamUserStatTypeMAX
        }

        public enum EChatEntryType
        {
            k_EChatEntryTypeInvalid = 0,
            k_EChatEntryTypeChatMsg = 1,        // Normal text message from another user
            k_EChatEntryTypeTyping = 2,         // Another user is typing (not used in multi-user chat)
            k_EChatEntryTypeInviteGame = 3,     // Invite from other user into that users current game
            k_EChatEntryTypeEmote = 4,          // text emote message (deprecated, should be treated as ChatMsg)
         // k_EChatEntryTypeLobbyGameStart = 5,	// lobby game is starting (dead - listen for LobbyGameCreated_t callback instead)
            k_EChatEntryTypeLeftConversation = 6, // user has left the conversation ( closed chat window )
                                                  // Above are previous FriendMsgType entries, now merged into more generic chat entry types
            k_EChatEntryTypeEntered = 7,        // user has entered the conversation (used in multi-user chat and group chat)
            k_EChatEntryTypeWasKicked = 8,      // user was kicked (data: 64-bit steamid of actor performing the kick)
            k_EChatEntryTypeWasBanned = 9,      // user was banned (data: 64-bit steamid of actor performing the ban)
            k_EChatEntryTypeDisconnected = 10,  // user disconnected
            k_EChatEntryTypeHistoricalChat = 11,    // a chat message from user's chat history or offilne message
            k_EChatEntryTypeReserved1 = 12,
            k_EChatEntryTypeReserved2 = 13,
        }

        public enum EChatRoomEnterResponse
        {
            k_EChatRoomEnterResponseSuccess = 1,        // Success
            k_EChatRoomEnterResponseDoesntExist = 2,    // Chat doesn't exist (probably closed)
            k_EChatRoomEnterResponseNotAllowed = 3,     // General Denied - You don't have the permissions needed to join the chat
            k_EChatRoomEnterResponseFull = 4,           // Chat room has reached its maximum size
            k_EChatRoomEnterResponseError = 5,          // Unexpected Error
            k_EChatRoomEnterResponseBanned = 6,         // You are banned from this chat room and may not join
            k_EChatRoomEnterResponseLimited = 7,        // Joining this chat is not allowed because you are a limited user (no value on account)
            k_EChatRoomEnterResponseClanDisabled = 8,   // Attempt to join a clan chat when the clan is locked or disabled
            k_EChatRoomEnterResponseCommunityBan = 9,   // Attempt to join a chat when the user has a community lock on their account
            k_EChatRoomEnterResponseMemberBlockedYou = 10, // Join failed - some member in the chat has blocked you from joining
            k_EChatRoomEnterResponseYouBlockedMember = 11, // Join failed - you have blocked some member already in the chat
         // k_EChatRoomEnterResponseNoRankingDataLobby = 12,  // No longer used
         // k_EChatRoomEnterResponseNoRankingDataUser = 13,  //  No longer used
         // k_EChatRoomEnterResponseRankOutOfRange = 14, //  No longer used
        }

        public delegate void PFNLegacyKeyRegistration(string pchCDKey, string pchInstallPath);
        public delegate bool PFNLegacyKeyInstalled();

        public const uint k_unSteamAccountIDMask = 0xFFFFFFFF;
        public const uint k_unSteamAccountInstanceMask = 0x000FFFFF;
        public const uint k_unSteamUserDesktopInstance = 1;
        public const uint k_unSteamUserConsoleInstance = 2;
        public const uint k_unSteamUserWebInstance = 4;

        public enum EChatSteamIDInstanceFlags : uint
        {
            k_EChatAccountInstanceMask = 0x00000FFF, // top 8 bits are flags

            k_EChatInstanceFlagClan = (k_unSteamAccountInstanceMask + 1) >> 1,  // top bit
            k_EChatInstanceFlagLobby = (k_unSteamAccountInstanceMask + 1) >> 2, // next one down, etc
            k_EChatInstanceFlagMMSLobby = (k_unSteamAccountInstanceMask + 1) >> 3,  // next one down, etc

            // Max of 8 flags
        }

        public enum EMarketingMessageFlags
        {
            k_EMarketingMessageFlagsNone = 0,
            k_EMarketingMessageFlagsHighPriority = 1 << 0,
            k_EMarketingMessageFlagsPlatformWindows = 1 << 1,
            k_EMarketingMessageFlagsPlatformMac = 1 << 2,
            k_EMarketingMessageFlagsPlatformLinux = 1 << 3,

            //aggregate flags
            k_EMarketingMessageFlagsPlatformRestrictions =
                k_EMarketingMessageFlagsPlatformWindows |
                k_EMarketingMessageFlagsPlatformMac |
                k_EMarketingMessageFlagsPlatformLinux,
        }

        public enum ENotificationPosition
        {
            k_EPositionTopLeft = 0,
            k_EPositionTopRight = 1,
            k_EPositionBottomLeft = 2,
            k_EPositionBottomRight = 3,
        }

        public class CSteamID
        {
            public CSteamID()
            {
                m_steamid.m_comp.m_unAccountID = 0;
                m_steamid.m_comp.m_EAccountType = EAccountType.k_EAccountTypeInvalid;
                m_steamid.m_comp.m_EUniverse = steamuniverse.EUniverse.k_EUniverseInvalid;
                m_steamid.m_comp.m_unAccountInstance = 0;
            }

            public CSteamID(uint unAccountID, steamuniverse.EUniverse eUniverse, EAccountType eAccountType)
            {
                Set(unAccountID, eUniverse, eAccountType);
            }

            public CSteamID(uint unAccountID, uint unAccountInstance, steamuniverse.EUniverse eUniverse, EAccountType eAccountType)
            {
#if _SERVER && Assert
                    xzip.Assert(!((EAccountType.k_EAccountTypeIndividual == eAccountType) && (unAccountInstance > k_unSteamUserWebInstance)));
#endif // _SERVER && Assert

                InstancedSet(unAccountID, unAccountInstance, eUniverse, eAccountType);
            }

            public CSteamID(ulong ulSteamID)
            {
                SetFromUint64(ulSteamID);
            }

            public void Set(uint unAccountID, steamuniverse.EUniverse eUniverse, EAccountType eAccountType)
            {
                m_steamid.m_comp.m_unAccountID = unAccountID;
                m_steamid.m_comp.m_EUniverse = eUniverse;
                m_steamid.m_comp.m_EAccountType = eAccountType;

                if (eAccountType == EAccountType.k_EAccountTypeClan || eAccountType == EAccountType.k_EAccountTypeGameServer)
                {
                    m_steamid.m_comp.m_unAccountInstance = 0;
                }
                else
                {
                    m_steamid.m_comp.m_unAccountInstance = k_unSteamUserDesktopInstance;
                }
            }

            public void InstancedSet(uint unAccountID, uint unInstance, steamuniverse.EUniverse eUniverse, EAccountType eAccountType)
            {
                m_steamid.m_comp.m_unAccountID = unAccountID;
                m_steamid.m_comp.m_EUniverse = eUniverse;
                m_steamid.m_comp.m_EAccountType = eAccountType;
                m_steamid.m_comp.m_unAccountInstance = unInstance;
            }

            public void FullSet(ulong ulIdentifier, steamuniverse.EUniverse eUniverse, EAccountType eAccountType)
            {
                m_steamid.m_comp.m_unAccountID = ((uint)ulIdentifier & k_unSteamAccountIDMask);                       // account ID is low 32 bits
                m_steamid.m_comp.m_unAccountInstance = (((uint)ulIdentifier >> 32) & k_unSteamAccountInstanceMask);           // account instance is next 20 bits
                m_steamid.m_comp.m_EUniverse = eUniverse;
                m_steamid.m_comp.m_EAccountType = eAccountType;
            }

            public void SetFromUint64(ulong ulSteamID)
            {
                m_steamid.m_unAll64Bits = ulSteamID;
            }

            public void Clear()
            {
                m_steamid.m_comp.m_unAccountID = 0;
                m_steamid.m_comp.m_EAccountType = EAccountType.k_EAccountTypeInvalid;
                m_steamid.m_comp.m_EUniverse = steamuniverse.EUniverse.k_EUniverseInvalid;
                m_steamid.m_comp.m_unAccountInstance = 0;
            }

#if INCLUDED_STEAM2_USERID_STRUCTS
            	//-----------------------------------------------------------------------------
            	// Purpose: Initializes a steam ID from a Steam2 ID structure
            	// Input:	pTSteamGlobalUserID -	Steam2 ID to convert
            	//			eUniverse -				universe this ID belongs to
            	//-----------------------------------------------------------------------------
            	void SetFromSteam2( TSteamGlobalUserID *pTSteamGlobalUserID, EUniverse eUniverse )
            	{
            		m_steamid.m_comp.m_unAccountID = pTSteamGlobalUserID->m_SteamLocalUserID.Split.Low32bits * 2 + 
            			pTSteamGlobalUserID->m_SteamLocalUserID.Split.High32bits;
            		m_steamid.m_comp.m_EUniverse = eUniverse;		// set the universe
            		m_steamid.m_comp.m_EAccountType = k_EAccountTypeIndividual; // Steam 2 accounts always map to account type of individual
            		m_steamid.m_comp.m_unAccountInstance = k_unSteamUserDesktopInstance; // Steam2 only knew desktop instances
            	}

            	//-----------------------------------------------------------------------------
            	// Purpose: Fills out a Steam2 ID structure
            	// Input:	pTSteamGlobalUserID -	Steam2 ID to write to
            	//-----------------------------------------------------------------------------
            	void ConvertToSteam2( TSteamGlobalUserID *pTSteamGlobalUserID ) const
            	{
            		// only individual accounts have any meaning in Steam 2, only they can be mapped
            		// Assert( m_steamid.m_comp.m_EAccountType == k_EAccountTypeIndividual );

            		pTSteamGlobalUserID->m_SteamInstanceID = 0;
            		pTSteamGlobalUserID->m_SteamLocalUserID.Split.High32bits = m_steamid.m_comp.m_unAccountID % 2;
            		pTSteamGlobalUserID->m_SteamLocalUserID.Split.Low32bits = m_steamid.m_comp.m_unAccountID / 2;
            	}
#endif // defined( INCLUDED_STEAM_COMMON_STEAMCOMMON_H )

            public ulong ConvertToUint64()
            {
                return m_steamid.m_unAll64Bits;
            }

            public ulong GetStaticAccountKey()
            {
                return (ulong)((((ulong)m_steamid.m_comp.m_EUniverse) << 56) + ((ulong)m_steamid.m_comp.m_EAccountType << 52) + m_steamid.m_comp.m_unAccountID);
            }

            public void CreateBlankAnonLogon(steamuniverse.EUniverse eUniverse)
            {
                m_steamid.m_comp.m_unAccountID = 0;
                m_steamid.m_comp.m_EAccountType = EAccountType.k_EAccountTypeAnonGameServer;
                m_steamid.m_comp.m_EUniverse = eUniverse;
                m_steamid.m_comp.m_unAccountInstance = 0;
            }

            public void CreateBlankAnonUserLogon(steamuniverse.EUniverse eUniverse)
            {
                m_steamid.m_comp.m_unAccountID = 0;
                m_steamid.m_comp.m_EAccountType = EAccountType.k_EAccountTypeAnonUser;
                m_steamid.m_comp.m_EUniverse = eUniverse;
                m_steamid.m_comp.m_unAccountInstance = 0;
            }

            public bool BBlankAnonAccount()
            {
                return m_steamid.m_comp.m_unAccountID == 0 && BAnonAccount() && m_steamid.m_comp.m_unAccountInstance == 0;
            }

            public bool BGameServerAccount()
            {
                return m_steamid.m_comp.m_EAccountType == EAccountType.k_EAccountTypeGameServer || m_steamid.m_comp.m_EAccountType == EAccountType.k_EAccountTypeAnonGameServer;
            }

            public bool BPersistentGameServerAccount()
            {
                return m_steamid.m_comp.m_EAccountType == EAccountType.k_EAccountTypeGameServer;
            }

            public bool BAnonGameServerAccount()
            {
                return m_steamid.m_comp.m_EAccountType == EAccountType.k_EAccountTypeAnonGameServer;
            }

            public bool BContentServerAccount()
            {
                return m_steamid.m_comp.m_EAccountType == EAccountType.k_EAccountTypeContentServer;
            }

            public bool BClanAccount()
            {
                return m_steamid.m_comp.m_EAccountType == EAccountType.k_EAccountTypeClan;
            }

            public bool BChatAccount()
            {
                return m_steamid.m_comp.m_EAccountType == EAccountType.k_EAccountTypeChat;
            }

            public bool IsLobby()
            {
                return (m_steamid.m_comp.m_EAccountType == EAccountType.k_EAccountTypeChat) && ((m_steamid.m_comp.m_unAccountInstance & (uint)EChatSteamIDInstanceFlags.k_EChatInstanceFlagLobby) != 0);
            }

            public bool BIndividualAccount()
            {
                return m_steamid.m_comp.m_EAccountType == EAccountType.k_EAccountTypeIndividual || m_steamid.m_comp.m_EAccountType == EAccountType.k_EAccountTypeConsoleUser;
            }

            public bool BAnonAccount()
            {
                return m_steamid.m_comp.m_EAccountType == EAccountType.k_EAccountTypeAnonUser || m_steamid.m_comp.m_EAccountType == EAccountType.k_EAccountTypeAnonGameServer;
            }

            public bool BAnonUserAccount()
            {
                return m_steamid.m_comp.m_EAccountType == EAccountType.k_EAccountTypeAnonUser;
            }

            public bool BConsoleUserAccount()
            {
                return m_steamid.m_comp.m_EAccountType == EAccountType.k_EAccountTypeConsoleUser;
            }

            public void SetAccountID(uint unAccountID) { m_steamid.m_comp.m_unAccountID = unAccountID; }
            public void SetAccountInstance(uint unInstance) { m_steamid.m_comp.m_unAccountInstance = unInstance; }
            public void ClearIndividualInstance() { if (BIndividualAccount()) m_steamid.m_comp.m_unAccountInstance = 0; }
            public bool HasNoIndividualInstance() { return BIndividualAccount() && (m_steamid.m_comp.m_unAccountInstance == 0); }
            public ulong GetAccountID() { return m_steamid.m_comp.m_unAccountID; }
            public uint GetUnAccountInstance() { return m_steamid.m_comp.m_unAccountInstance; }
            public EAccountType GetAccountType() { return m_steamid.m_comp.m_EAccountType; }
            public steamuniverse.EUniverse GetEUniverse() { return m_steamid.m_comp.m_EUniverse; }
            public void SetEUniverse(steamuniverse.EUniverse eUniverse) { m_steamid.m_comp.m_EUniverse = eUniverse; }

            public extern CSteamID(string pchSteamID, steamuniverse.EUniverse eDefaultUniverse = steamuniverse.EUniverse.k_EUniverseInvalid);
            public extern string Render();
            public static extern string Render(ulong ulSteamID);

#if CSTEAMID_RENDERLINK_SUPPORT
            public extern string RenderLink();
            public static extern string RenderLink(ulong ulSteamID);
#endif // CSTEAMID_RENDERLINK_SUPPORT

            public extern void SetFromString(string pchSteamID, steamuniverse.EUniverse eDefaultUniverse);
            public extern bool SetFromStringStrict(string pchSteamID, steamuniverse.EUniverse eDefaultUniverse);
            public extern bool SetFromSteam2String(string pchSteam2ID, steamuniverse.EUniverse eUniverse);

            public static bool operator ==(CSteamID lhs, CSteamID rhs) { return lhs.m_steamid.m_unAll64Bits == rhs.m_steamid.m_unAll64Bits; }
            public static bool operator !=(CSteamID lhs, CSteamID rhs) { return !(lhs == rhs); }
            public static bool operator <(CSteamID lhs, CSteamID rhs) { return lhs.m_steamid.m_unAll64Bits < rhs.m_steamid.m_unAll64Bits; }
            public static bool operator >(CSteamID lhs, CSteamID rhs) { return lhs.m_steamid.m_unAll64Bits > rhs.m_steamid.m_unAll64Bits; }

            public extern bool BValidExternalSteamID();

            //private CSteamID(uint);
            //private CSteamID(int);

            public class SteamID_t
            {
                public struct SteamIDComponent_t
                {
#if VALVE_BIG_ENDIAN
                    public steamuniverse.EUniverse m_EUniverse; // : 8;
                    public uint m_EAccountType; // : 4;
                    public uint m_unAccountInstance; // : 20;
                    public uint m_unAccountID; // : 32;
#else
                    public uint m_unAccountID; // : 32;
                    public uint m_unAccountInstance; // : 20;
                    public /*uint*/ EAccountType m_EAccountType; // : 4; - Why the uint?
                    public steamuniverse.EUniverse m_EUniverse; // : 8;
#endif // VALVE_BIG_ENDIAN
                }

                public SteamIDComponent_t m_comp;

                public ulong m_unAll64Bits;
            }

            public SteamID_t m_steamid;

            public bool IsValid()
            {
                if (m_steamid.m_comp.m_EAccountType <= EAccountType.k_EAccountTypeInvalid || m_steamid.m_comp.m_EAccountType >= EAccountType.k_EAccountTypeMax)
                {
                    return false;
                }

                if (m_steamid.m_comp.m_EUniverse <= steamuniverse.EUniverse.k_EUniverseInvalid || m_steamid.m_comp.m_EUniverse >= steamuniverse.EUniverse.k_EUniverseMax)
                {
                    return false;
                }

                if (m_steamid.m_comp.m_EAccountType == EAccountType.k_EAccountTypeIndividual)
                {
                    if (m_steamid.m_comp.m_unAccountID == 0 || m_steamid.m_comp.m_unAccountInstance > k_unSteamUserWebInstance)
                    {
                        return false;
                    }
                }

                if (m_steamid.m_comp.m_EAccountType == EAccountType.k_EAccountTypeClan)
                {
                    if (m_steamid.m_comp.m_unAccountID == 0 || m_steamid.m_comp.m_unAccountInstance != 0)
                    {
                        return false;
                    }
                }

                if (m_steamid.m_comp.m_EAccountType == EAccountType.k_EAccountTypeGameServer)
                {
                    if (m_steamid.m_comp.m_unAccountID == 0)
                    {
                        return false;
                    }
                }

                return true;
            }

            public static CSteamID k_steamIDNil = new();

            public static CSteamID k_steamIDOutOfDateGS = new(0, 0, steamuniverse.EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);
            public static CSteamID k_steamIDLanModeGS = new(0, 0, steamuniverse.EUniverse.k_EUniversePublic, EAccountType.k_EAccountTypeInvalid);
            public static CSteamID k_steamIDNotInitYetGS = new(1, 0, steamuniverse.EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);
            public static CSteamID k_steamIDNotSteamGS = new(2, 0, steamuniverse.EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);

#if STEAM
            public extern CSteamID ChatIDFromSteamID(CSteamID steamID);
            public extern CSteamID ClanIDFromSteamID(CSteamID steamID);
            public extern CSteamID ChatIDFromClanID(CSteamID steamIDClan);
            public extern CSteamID ClanIDFromChatID(CSteamID steamIDChat);
#endif // STEAM
        }

        public class CGameID
        {
            public CGameID()
            {
                m_gameID.m_nType = EGameIDType.k_EGameIDTypeApp;
                m_gameID.m_nAppID = k_uAppIdInvalid;
                m_gameID.m_nModID = 0;
            }

            public CGameID(ulong ulGameID)
            {
                m_ulGameID = ulGameID;
            }

            public CGameID(int nAppID)
            {
                m_ulGameID = 0;
                m_gameID.m_nAppID = (uint)nAppID;
            }

            public CGameID(int nAppID, int nModID)
            {
                m_ulGameID = 0;
                m_gameID.m_nAppID = (uint)nAppID;
                m_gameID.m_nModID = (uint)nModID;
                m_gameID.m_nType = EGameIDType.k_EGameIDTypeGameMod;
            }

            public extern CGameID(string pchGameID);
            public extern string Render();
            public static extern string Render(ulong ulGameID);

#if CHECKSUM_CRC_H

            public CGameID(uint nAppID, string pchModPath)
            {
                m_ulGameID = 0;
                m_gameID.m_nAppID = nAppID;
                m_gameID.m_nType = EGameIDType.k_EGameIDTypeGameMod;

                string rgchModDir;
                Q_FileBase(pchModPath, rgchModDir, sizeof(rgchModDir));
                CRC32_t crc32;
                CRC32_Init(crc32);
                CRC32_ProcessBuffer(crc32, rgchModDir, rgchModDir.Length);
                CRC32_Final(crc32);

                m_gameID.m_nModID = crc32 | (0x80000000);
            }

            public CGameID(string pchExePath, string pchAppName)
            {
                m_ulGameID = 0;
                m_gameID.m_nAppID = k_uAppIdInvalid;
                m_gameID.m_nType = EGameIDType.k_EGameIDTypeShortcut;

                CRC32_t crc32;
                CRC32_Init(crc32);
                CRC32_ProcessBuffer(crc32, pchExePath, pchExePath.Length);
                CRC32_ProcessBuffer(crc32, pchAppName, pchAppName.Length);
                CRC32_Final(crc32);

                m_gameID.m_nModID = crc32 | (0x80000000);
            }

#if VSTFILEID_H

            public CGameID(VstFileID vstFileID)
            {
                m_ulGameID = 0;
                m_gameID.m_nAppID = k_uAppInvalid;
                m_gameID.m_nType = EGameIDType.k_EGameIDTypeP2P;

                CRC32_t crc32;
                CRC32_Init(crc32);
                string pchFileId = vstFileID.Render();
                CRC32_ProcessBuffer(crc32, pchFileId, pchFileId.Length);
                CRC32_Final(crc32);

                m_GameID.m_nModID = crc32 | (0x80000000);
            }

#endif // VSTFILEID_H

#endif // CHECKSUM_CRC_H

            public ulong ToUint64()
            {
                return m_ulGameID;
            }

            public UIntPtr GetUint64Ptr()
            {
                return (UIntPtr)m_ulGameID;
            }

            public void Set(ulong ulGameID)
            {
                m_ulGameID = ulGameID;
            }

            public bool IsMod()
            {
                return (m_gameID.m_nType == EGameIDType.k_EGameIDTypeGameMod);
            }

            public bool IsShortcut()
            {
                return (m_gameID.m_nType == EGameIDType.k_EGameIDTypeShortcut);
            }

            public bool IsP2PFile()
            {
                return (m_gameID.m_nType == EGameIDType.k_EGameIDTypeP2P);
            }

            public bool IsSteamApp()
            {
                return (m_gameID.m_nType == EGameIDType.k_EGameIDTypeApp);
            }

            public uint ModID()
            {
                return m_gameID.m_nModID;
            }

            public uint AppID()
            {
                return m_gameID.m_nAppID;
            }

            public static bool operator ==(CGameID lhs, CGameID rhs)
            {
                return lhs.m_ulGameID == rhs.m_ulGameID;
            }

            public static bool operator !=(CGameID lhs, CGameID rhs)
            {
                return !(lhs == rhs);
            }

            public static bool operator <(CGameID lhs, CGameID rhs)
            {
                return (lhs.m_ulGameID < rhs.m_ulGameID);
            }

            public static bool operator >(CGameID lhs, CGameID rhs)
            {
                return (lhs.m_ulGameID > rhs.m_ulGameID);
            }

            public bool IsValid()
            {
                switch (m_gameID.m_nType)
                {
                    case EGameIDType.k_EGameIDTypeApp:
                        return m_gameID.m_nAppID != k_uAppIdInvalid;

                    case EGameIDType.k_EGameIDTypeGameMod:
                        return m_gameID.m_nAppID != k_uAppIdInvalid && (m_gameID.m_nModID & 0x80000000) != 0;

                    case EGameIDType.k_EGameIDTypeShortcut:
                        return (m_gameID.m_nModID & 0x80000000) != 0;

                    case EGameIDType.k_EGameIDTypeP2P:
                        return m_gameID.m_nAppID == k_uAppIdInvalid && (m_gameID.m_nModID & 0x80000000) != 0;

                    default:
#if Assert
                        xzip.Assert(false);
#endif // Assert
                        return false;
                }
            }

            public void Reset()
            {
                m_ulGameID = 0;
            }

            private enum EGameIDType
            {
                k_EGameIDTypeApp = 0,
                k_EGameIDTypeGameMod = 1,
                k_EGameIDTypeShortcut = 2,
                k_EGameIDTypeP2P = 3,
            }

            private struct GameID_t
            {
#if VALVE_BIG_ENDIAN
                public uint m_nModID; // : 32;
                public /*uint*/ m_nType; // : 8; - Again, why the uint?
                public uint m_nAppID; // : 24;
#else
                public uint m_nAppID; // : 24;
                public /*uint*/ EGameIDType m_nType; // : 8; - Again, why the uint?
                public uint m_nModID; // : 32;
#endif // VALVE_BIG_ENDIAN
            }

            private ulong m_ulGameID;
            private GameID_t m_gameID;
        }

        public const int k_cchGameExtraInfoMax = 64;

        public const int QUERY_PORT_NOT_INITIALIZED = 0xFFFF;
        public const int QUERY_PORT_ERROR = 0xFFFE;

        public delegate void PFNPreMinidumpCallback(IntPtr context);

        public static int BREAKPAD_INVALID_HANDLE(BREAKPAD_HANDLE x) => 0;
    }
}
