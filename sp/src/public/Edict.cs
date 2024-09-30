namespace SourceSharp.SP.Public;

public enum MapLoadType
{
    MapLoad_NewGame = 0,
    MapLoad_LoadGame,
    MapLoad_Transition,
    MapLoad_Background
}

public class CGlobalVars : CGlobalVarsBase
{
    public string mapname;
    public int mapversion;
    public string startspot;
    public MapLoadType loadType;
    public bool mapLoadFailed;

    public bool deathmatch;
    public bool coop;
    public bool teamplay;

    public int maxEntities;
    public int serverCount;

    public CGlobalVars(bool isClient)
    {
        serverCount = 0;
    }
}

public class CEdictChangeInfo
{
    public ushort[] changeOffsets = new ushort[CBaseEdict.MAX_CHANGE_OFFSETS];
    public ushort changeOffset;
}

public class CSharedEdictChangeInfo
{
    public ushort serialNumber;
    public CEdictChangeInfo[] changeInfos = new CEdictChangeInfo[CBaseEdict.MAX_EDICT_CHANGE_INFOS];
    public ushort numChangeInfos;

    public CSharedEdictChangeInfo()
    {
        serialNumber = 1;
    }
}

public class IChangeInfoAccessor
{
    private ushort changeInfo;
    private ushort changeInfoSerialNumber;

    public void SetChangeInfo(ushort info)
    {
        changeInfo = info;
    }

    public void SetChangeInfoSerialNumber(ushort sn)
    {
        changeInfoSerialNumber = sn;
    }

    public ushort GetChangeInfo()
    {
        return changeInfo;
    }

    public ushort GetChangeInfoSerialNumber()
    {
        return changeInfoSerialNumber;
    }
}

public class CBaseEdict
{
    public const int FL_EDICT_CHANGED = 1 << 0;

    public const int FL_EDICT_FREE = 1 << 1;
    public const int FL_EDICT_FULL = 1 << 2;

    public const int FL_EDICT_FULLCHECK = 0 << 0;
    public const int FL_EDICT_ALWAYS = 1 << 3;
    public const int FL_EDICT_DONTSEND = 1 << 4;
    public const int FL_EDICT_PVSCHECK = 1 << 5;

    public const int FL_EDICT_PENDING_DORMANT_CHECK = 1 << 6;

    public const int FL_EDICT_DIRTY_PVS_INFORMATION = 1 << 7;

    public const int FL_FULL_EDICT_CHANGED = 1 << 8;

    public const int MAX_CHANGE_OFFSETS = 19;
    public const int MAX_EDICT_CHANGE_INFOS = 100;

#if X360
    public ushort stateFlags;
#else
    public int stateFlags;
#endif // X360

    public short networkSerialNumber;
    public short edictIndex;

    public IServerNetworkable networkable;

    protected IServerUnknown unk;

    public IServerEntity GetIServerEntity()
    {
        if ((stateFlags & FL_EDICT_FULL) != 0)
        {
            return (IServerEntity)unk;
        }
        else
        {
            return null;
        }
    }

    public IServerNetworkable GetNetworkable()
    {

    }

    public IServerUnknown GetUnknown()
    {

    }

    public void SetEdict(IServerUnknown unk, bool fullEdict)
    {

    }

    public int AreaNum()
    {

    }

    public string GetClassName()
    {

    }

    public bool IsFree()
    {
        return (stateFlags & FL_EDICT_FREE) != 0;
    }

    public void SetFree()
    {

    }

    public void ClearFree()
    {

    }

    public bool HasStateChanged()
    {
        return (stateFlags & FL_EDICT_CHANGED) != 0;
    }

    public void ClearStateChanged()
    {
        stateFlags &= ~(FL_EDICT_CHANGED | FL_FULL_EDICT_CHANGED);
        SetChangeInfoSerialNumber(0);
    }
    
    public void StateChanged()
    {
        if ((stateFlags & FL_FULL_EDICT_CHANGED) != 0)
        {
            return;
        }

        stateFlags |= FL_EDICT_CHANGED;

        IChangeInfoAccessor accessor = GetChangeAccessor();

        if (accessor.GetChangeInfoSerialNumber() == )
    }

    public void StateChanged(ushort offset)
    {

    }

    public void SetChangeInfo(ushort info)
    {

    }

    public void SetChangeInfoSerialNumber(ushort sn)
    {

    }

    public ushort GetChangeInfo()
    {

    }

    public ushort GetChangeInfoSerialNumber()
    {

    }

    public IChangeInfoAccessor GetChangeAccessor()
    {

    }

    public void InitializeEntityDLLFields(Edict edict)
    {

    }
}