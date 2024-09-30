namespace SourceSharp.SP.Public.Engine;

public enum SESSION_NOTIFY
{
    SESSION_NOTIFY_FAIL_SEARCH,
    SESSION_NOTIFY_SEARCH_COMPLETED,
    SESSION_NOFIFY_MODIFYING_SESSION,
    SESSION_NOTIFY_MODIFYING_COMPLETED_HOST,
    SESSION_NOTIFY_MODIFYING_COMPLETED_CLIENT,
    SESSION_NOTIFY_MIGRATION_COMPLETED,
    SESSION_NOTIFY_CONNECT_SESSIONFULL,
    SESSION_NOTIFY_CONNECT_NOTAVAILABLE,
    SESSION_NOTIFY_CONNECTED_TOSESSION,
    SESSION_NOTIFY_CONNECTED_TOSERVER,
    SESSION_NOTIFY_CONNECT_FAILED,
    SESSION_NOTIFY_FAIL_CREATE,
    SESSION_NOTIFY_FAIL_MIGRATE,
    SESSION_NOTIFY_REGISTER_COMPLETED,
    SESSION_NOTIFY_FAIL_REGISTER,
    SESSION_NOTIFY_CLIENT_KICKED,
    SESSION_NOTIFY_CREATED_HOST,
    SESSION_NOTIFY_CREATED_CLIENT,
    SESSION_NOTIFY_LOST_HOST,
    SESSION_NOTIFY_LOST_SERVER,
    SESSION_NOTIFY_COUNTDOWN,
    SESSION_NOTIFY_ENDGAME_RANKED,
    SESSION_NOTIFY_ENDGAME_HOST,
    SESSION_NOTIFY_ENDGAME_CLIENT,
    SESSION_NOTIFY_DUMPSTATS,
    SESSION_NOTIFY_WELCOME,
}

public enum SESSION_PROPS
{
    SESSION_CONTEXT,
    SESSION_PROPERTY,
    SESSION_FLAG
}

public struct HostData
{
    public string hostName;
    public string scenario;
    public int gameState;
    public int gameTime;
    public XUID xuid;
}

public struct MM_QOS
{
    public int pingMsMin;
    public int pingMsMed;
    public float bwUpKbs;
    public float bwDnKbs;
    public float loss;
}

public interface IMatchmaking
{
    public const int NO_TIME_LIMIT = 65000;

    public const string VENGINE_MATCHMAKING_VERSION = "VENGINE_MATCHMAKING_VERSION001";

    public void SessionNotification(SESSION_NOTIFY notification, int param = 0);
    public void AddSessionProperty(uint type, string id, string value, string valueType);
    public void SetSessionProperties(KeyValues[] propertyKeys);
    public void SelectSession(int idx);
    public void ModifySession();
    public void UpdateMuteList();
    public void StartHost(bool systemLink = false);
    public void StartClient(bool systemLink = false);
    public bool StartGame();
    public bool CancelStartGame();
    public void ChangeTeam(string teamName);
    public void TellClientsToConnect();
    public void KickPlayerFromSession(ulong id);
    public void JoinInviteSession(XSESSION_INFO hostInfo);
    public void JoinInviteSessionByID(XNKID sessionID);
    public void EndStatsReporting();

    public KeyValues GetSessionProperties();

    public ulong PlayerIdToXuid(int playerId);
    public bool IsPlayerMuted(int userId, XUID id);

    public MM_QOS GetQosWithLIVE();

    public bool PreventFullServerStartup();
}