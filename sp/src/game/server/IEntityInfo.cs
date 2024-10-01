using SourceSharp.SP.Mathlib;

namespace SourceSharp.SP.Public.Game.Server;

public interface IEntityInfo
{
    public int EntityIndex();
    public string GetEntityName();
    public string GetClassname();
    public string GetModelName();

    public string GetTargetName();
    public void SetModel(string modelName);
    public bool IsPlayer();
    public bool IsNPC();
    public bool IsDead();
    public bool IsAlive();
    public bool IsInWorld();
    public bool IsTemplate();
    public int GetEFlags();
    public void SetEFlags(int eFlags);
    public void AddEFlags(int eFlagMask);
    public bool IsEFlagSet(int eFlagMask);

    public int GetEffects();
    public void AddEffects(int effects);
    public void RemoveEffects(int effects);
    public void ClearEffects();
    public void SetEffects(int effects);
    public bool IsEffectActive(int effects);
    public int GetRenderMode();
    public void SetRenderMode(int renderMode);

    public void SetBlocksLOS(bool blocksLOS);
    public bool BlocksLOS();

    public int GetHealth();
    public int GetMaxHealth();
    public void SetHealth(int health);
    public void SetMaxHealth(int maxHealth);

    public int GetTeamIndex();
    public void ChangeTeam(int teamNum);

    public Vector GetAbsOrigin();
    public void SetAbsOrigin(Vector vec);
    public QAngle GetAbsAngles();
    public void SetAbsAngles(QAngle ang);
    public Vector GetLocalOrigin();
    public void SetLocalOrigin(Vector vec);
    public QAngle GetLocalAngles();
    public void SetLocalAngles(QAngle ang);
    public Vector GetAbsVelocity();
    public Vector GetLocalVelocity();
    public QAngle GetLocalAngularVelocity();
    public void EntityToWorldSpace(Vector @in, out Vector @out);
    public void WorldToEntitySpace(Vector @in, out Vector @out);
    public Vector EyePosition();
    public QAngle EyeAngles();
    public QAngle LocalEyeAngles();
    public Vector EarPosition();

    public Vector GetWorldMins();
    public Vector GetWorldMaxs();
    public Vector WorldSpaceCenter();

    public int GetWaterLevel();

    public Edict GetOwner();
    public Edict GetParent();
    public Edict GetMoveParent();
    public Edict GetRootMoveParent();

    public Edict GetFollowedEntity();
    public Edict GetGroundEntity();

    public bool GetCustomInfo(int valueType, out PluginVariant outValue, PluginVariant options);

    public string GetDebugName();
    public void EntityText(int text_offset, string text, float duration, int r = 255, int g = 255, int b = 255, int a = 255);

    public bool GetKeyValue(string keyName, byte[] value, int maxLen);
}

public interface IEntityInfoManager
{
    public const string INTERFACEVERSION_ENTITYINFOMANAGER = "EntityInfoManager001";

    public IEntityInfo GetEntityInfo(Edict edict);
    public IEntityInfo GetEntityInfo(int index);

    public IServerUnknown GetServerEntity(Edict edict);

    public Edict FindEntityByClassname(Edict startEntity, string name);

    public Edict FindEntityByName(Edict startEntity, string name);

    public Edict FindEntityByModel(Edict startEntity, string modelName);

    public Edict FindEntityInSphere(Edict startEntity, Vector center, float radius);

    public void GetWorldBounds(Vector mins, Vector maxs);
}