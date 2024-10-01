using SourceSharp.SP.Mathlib;

namespace SourceSharp.SP.Public.Game.Server;

public class CBotCmd
{
    public int command_number;
    public int tick_count;
    public int buttons;
    public int weaponselect;
    public int weaponsubtype;
    public int random_seed;

    public QAngle viewangles;

    public float forwardmove;
    public float sidemove;
    public float upmove;

    public byte impulse;

    public short mousedx;
    public short mousedy;

    public bool hasbeenpredicted;

    public CBotCmd()
    {
        Reset();
    }

    ~CBotCmd()
    {

    }

    public void Reset()
    {
        command_number = 0;
        tick_count = 0;
        viewangles.Init();
        forwardmove = 0.0f;
        sidemove = 0.0f;
        upmove = 0.0f;
        buttons = 0;
        impulse = 0;
        weaponselect = 0;
        weaponsubtype = 0;
        random_seed = 0;
        mousedx = 0;
        mousedy = 0;

        hasbeenpredicted = false;
    }
}

public interface IPlayerInfo
{
    public string GetName();
    public int GetUserID();
    public string GetNetworkIDString();
    public int GetTeamIndex();
    public void ChangeTeam(int teamNum);
    public int GetFragCount();
    public int GetDeathCount();
    public bool IsConnected();
    public int GetArmorValue();

    public bool IsHLTV();
    public bool IsPlayer();
    public bool IsFakeClient();
    public bool IsDead();
    public bool IsInAVehicle();
    public bool IsObserver();

    public Vector GetAbsOrigin();
    public QAngle GetAbsAngles();
    public Vector GetPlayerMins();
    public Vector GetPlayerMaxs();
    public string GetWeaponName();
    public string GetModelName();
    public int GetHealth();
    public int GetMaxHealth();
    public CBotCmd GetLastUserCommand();

    public bool IsReplay();
}

public interface IPlayerInfoManager
{
    public const string INTERFACEVERSION_PLAYERINFOMANAGER = "PlayerInfoManager002";

    public IPlayerInfo GetPlayerInfo(Edict edict);
    public CGlobalVars GetGlobalVars();
}

public interface IBotController
{
    public void SetAbsOrigin(Vector vec);
    public void SetAbsAngles(QAngle ang);
    public void SetLocalOrigin(Vector origin);
    public Vector GetLocalOrigin();
    public void SetLocalAngles(QAngle angles);
    public QAngle GetLocalAngles();

    public void RemoveAllItems(bool removeSuit);
    public void SetActiveWeapon(string weaponName);
    public bool IsEFlagSet(int eFlagMask);
    public void RunPlayerMove(CBotCmd ucmd);
}

public interface IBotManager
{
    public const string INTERFACEVERSION_PLAYERBOTMANAGER = "BotManager001";

    public IBotController GetBotController(Edict edict);
    public Edict CreateBot(string botname);
}