using System;
using System.Diagnostics;
using SourceSharp.SP.Mathlib;
using SourceSharp.SP.Tier1;

namespace SourceSharp.SP.Game.Client;

public enum CollideType
{
    ENTITY_SHOULD_NOT_COLLIDE = 0,
    ENTITY_SHOULD_COLLIDE,
    ENTITY_SHOULD_RESPOND
}

public class VarMapEntity
{
    public ushort type;
    public bool needsToInterpolate;

    public byte[] data;
    public IInterpolatedVar watcher;
}

public struct VarMapping
{
    public CUtlVector<VarMapEntity> entries;
    public int interpolatedEntries;
    public float lastInterpolationTime;

    public VarMapping()
    {
        interpolatedEntries = 0;
    }
}

public struct PredictionContext
{
    public bool active;
    public int creationCommandNumber;
    public string creationModule;
    public int creationLineNumber;
    public CHandle<C_BaseEntity> serverEntity;

    public PredictionContext()
    {
        active = false;
        creationCommandNumber = -1;
        creationModule = null;
        creationLineNumber = 0;
        serverEntity = null;
    }
}

public struct ThinkFunc
{
    public nint think;
    public string context;
    public int nextThinkTick;
    public int lastThinkTick;
}

public class C_BaseEntity : IClientEntity
{
    public static CHandle<C_BaseEntity> EHANDLE;

    public static C_BaseEntity BASEPTR()
    {
        return new C_BaseEntity();
    }

    public static C_BaseEntity ENTITYFUNCPTR(C_BaseEntity other)
    {
        throw new NotImplementedException();
    }

    public const int ENTCLIENTFLAG_GETTINGSHADOWRENDERBOUNDS = 0x0001;
    public const int ENTCLIENTFLAG_DONTUSEIK = 0x0002;
    public const int ENTCLIENTFLAG_ALWAYS_INTERPOLATE = 0x0004;

    public const int SLOT_ORIGINALDATA = -1;

    public const int MULTIPLAYER_BACKUP = 90;

    public string classname;
    public VarMapping varMap;

    public static bool allowPrecache;

    protected static bool disableTouchFuncs;

    public Action think;
    public Action<C_BaseEntity> touch;

    public int index;

    public byte renderFX;
    public byte renderFXBlend;

    public ushort entClientFlags;

    public CNetworkColor clrRender;

    private Model model;

    public float animTime;
    public float oldAnimTime;

    public float simulationTime;
    public float oldSimulationTime;

    public float createTime;

    public byte interpolationFrame;
    public byte oldInterpolationFrame;

    private int effects;
    private byte renderMode;
    private byte oldRenderMode;

    public ClientRenderHandle render;

    public bool readyToDraw;

    public int nextThinkTick;
    public int lastThinkTick;

    public short modelIndex;

#if TF_CLIENT
    public int[] modelIndexOverrides = new int[MAX_VISION_MODES];
#endif // TF_CLIENT

    public byte takedamage;
    public byte lifeState;

    public int health;

    public float speed;

    public int teamNum;

    public CPredictableId predictableId;
    public PredictionContext predictionContext;

    public int touchStamp;

    protected int fxComputeFrame;

    protected CBaseHandle refEHandle;

    private bool enabledInToolView;
    private bool toolRecording;
    private HTOOLHANDLE toolHandle;
    private int lastRecordedFrame;
    private bool recordInTools;

    protected IPhysicsObject physicsObject;

    protected bool predictionEligble;

    protected int simulationTick;

    protected CUtlVector<ThinkFunc> thinkFunctions;
    protected int currentThinkContext;

    protected Vector viewOffset;

#if SIXENSE
    protected Vector eyeOffset;
    protected QAngle eyeAngleOffset;
#endif // SIXENSE

    private Vector velocity;
    private CInterpolatedVar<Vector> iv_Velocity;

    private Vector absVelocity;

    private QAngle angVelocity;

    private bool dormantPredictable;
    private int incomingPacketEntityBecameDormant;

    private float spawnTime;
    private float lastMessageTime;

    private Vector baseVelocity;

    private float gravity;

    private ModelInstanceHandle modelInstance;
    private ClientShadowHandle shadowHandle;

    private float proxyRandomValue;

    private ClientThinkHandle think;

    private int eFlags;

    private byte moveType;
    private byte moveCollide;
    private byte parentAttachment;
    private byte oldParentAttachment;

    private byte waterLevel;
    private byte waterType;

    private bool dormant;
    private bool predictable;

    private CHandle<C_BaseEntity> moveParent;
    private CHandle<C_BaseEntity> moveChild;
    private CHandle<C_BaseEntity> movePeer;
    private CHandle<C_BaseEntity> movePrevPeer;

    private CHandle<C_BaseEntity> networkMoveParent;
    private CHandle<C_BaseEntity> oldMoveParent;

    private string modelName;

    private CNetworkVarEmbedded<CCollisionProperty> collision;
    private CNetwortVarEmbedded<CParticleProperty> particles;

    private float elasticity;

    private float shadowCastDistance;
    private EHANDLE shadowDirUseOtherEntity;

    private EHANDLE groundEntity;
    private float groundChangeTime;

    private float friction;

    private Vector absOrigin;
    private QAngle absRotation;
    private Vector oldOrigin;
    private QAngle oldAngRotation;

    private Vector origin;
    private CInterpolatedVar<Vector> iv_Origin;
    private QAngle rotation;
    private CInterpolatedVar<QAngle> iv_Rotation;

    private Matrix3x4 coordinateFrame;

    private Vector networkOrigin;
    private QAngle networkAngles;

    private int flags;

    private int collisionGroup;

    private byte[] intermediateData = new byte[MULTIPLAYER_BACKUP];
    private byte[] originalData;
    private int intermediateDataCount;

    private bool isPlayerSimulated;

    private CNetworkVar<bool> simulatedEveryTick;
    private CNetworkVar<bool> animatedEveryTick;
    private CNetworkVar<bool> alternateSorting;

    private byte textureFrameIndex;
    private ABSBBoxVisualization bboxVisFlags;

    private int dataChangeEventRef;

    private CHandle<C_BasePlayer> playerSimulationOwner;

    private EHANDLE ownerEntity;
    private EHANDLE effectEntity;

    private static int predictionRandomSeed;
    private static C_BasePlayer predictionPlayer;
    private static bool absQueriesValid;
    private static bool absRecomputationEnabled;
    private static bool interpolate;

    private int dataObjectTypes;

    private AimEntsListHandle aimEntsListHandle;
    private int creationTick;

    public float[] renderingClipPlane = new float[4];
    public bool enableRenderingClipPlane;

    protected ushort interpolationListEntry;
    protected ushort teleportListEntry;

    protected CThreadFastMutex calcAbsolutePositionMutex;
    protected CThreadFastMutex calcAbsoluteVelocityMutex;

#if TF_CLIENT
    protected bool validatedOwner;
    protected bool deemedInvalid;
    protected bool wasDeemedInvalid;
    protected RenderMode previousRenderMode;
    protected Color32 previousRenderColor;
#endif // TF_CLIENT

    public void cc_cl_interp_all_changed(IConVar conVar, string oldString, float oldValue)
    {

    }

    public C_BaseEntity()
    {

    }

    ~C_BaseEntity()
    {

    }

    public static C_BaseEntity CreatePredictedEntityByName(string classname, string module, int line, bool persist = false)
    {

    }

    public virtual void FireBullets(FireBulletsInfo info)
    {

    }

    public virtual void ModifyFireBulletsDamage(CTakeDamageInfo dmgInfo)
    {

    }

    public virtual bool ShouldDrawUnderwaterBulletBubbles()
    {

    }

    public virtual bool ShouldDrawWaterImpacts()
    {
        return true;
    }

    public virtual bool HandleShotImpactingWater(FireBulletsInfo info, Vector end, ITraceFilter traceFilter, Vector tracerDest)
    {

    }

    public virtual ITraceFilter GetBeamTraceFilter()
    {

    }

    public virtual void DispatchTraceAttack(CTakeDamageInfo info, Vector dir, Trace ptr, CDmgAccumulator accumulator = null)
    {

    }

    public virtual void TraceAttack(CTakeDamageInfo info, Vector dir, Trace ptr, CDmgAccumulator accumulator = null)
    {

    }

    public virtual void DoImpactEffect(Trace tr, int damageType)
    {

    }

    public virtual void MakeTracer(Vector tracerSrc, Trace tr, int tracerType)
    {

    }

    public virtual int GetTracerAttachment()
    {

    }

    public void ComputeTracerStartPosition(Vector shotSrc, Vector tracerStart)
    {

    }

    public void TraceBleed(float damage, Vector dir, Trace ptr, int damageType)
    {

    }

    public virtual int BloodColor()
    {

    }

    public virtual string GetTracerType()
    {

    }

    public virtual void Spawn()
    {

    }

    public virtual void SpawnClientEntity()
    {

    }

    public virtual void Precache()
    {

    }

    public virtual void Activate()
    {

    }

    public virtual void ParseMapData(CEntityMapData mapData)
    {

    }

    public virtual bool KeyValue(string keyName, string value)
    {

    }

    public virtual bool KeyValue(string keyName, float value)
    {

    }

    public virtual bool KeyValue(string keyName, Vector value)
    {

    }

    public virtual bool GetKeyValue(string keyName, string value, int maxLen)
    {

    }

    public void SetBlocksLOS(bool blocksLOS)
    {

    }

    public bool BlocksLOS()
    {

    }

    public void SetAIWalkable(bool blocksLOS)
    {

    }

    public bool IsAIWalkable()
    {

    }

    public void Interp_SetupMappings(VarMapping map)
    {

    }

    public int Interp_Interpolate(VarMapping map, float currentTime)
    {

    }

    public void Interp_RestoreToLastNetworked(VarMapping map)
    {

    }

    public void Interp_UpdateInterpolationAmounts(VarMapping map)
    {

    }

    public void Interp_HierarchyUpdateInterpolationAmounts()
    {

    }

    public virtual bool Init(int entnum, int serialNum)
    {

    }

    public void Term()
    {

    }

    public IClientUnknown GetIClientUnknown()
    {
        return this;
    }

    public virtual C_BaseAnimating GetBaseAnimating()
    {
        return null;
    }

    public virtual void SetClassname(string className)
    {
        classname = className;
    }

    public virtual void SetRefEHandle(CBaseHandle handle)
    {

    }

    public virtual CBaseHandle GetRefEHandle()
    {

    }

    public void SetToolHandle(uint handle)
    {

    }

    public uint GetToolHandle()
    {

    }

    public void EnableInToolView(bool enable)
    {

    }

    public bool IsEnabledInToolView()
    {

    }

    public void SetToolRecording(bool recording)
    {

    }

    public bool IsToolRecording()
    {

    }

    public bool HasRecordedThisFrame()
    {

    }

    public virtual void RecordToolMessage()
    {

    }

    public void DontRecordInTools()
    {

    }

    public bool ShouldRecordInTools()
    {

    }

    public virtual void Release()
    {

    }

    public virtual ICollideable GetCollideable()
    {
        return collision;
    }

    public virtual IClientNetworkable GetClientNetworkable()
    {
        return this;
    }

    public virtual IClientRenderable GetClientRenderable()
    {
        return this;
    }

    public virtual IClientEntity GetIClientEntity()
    {
        return this;
    }

    public virtual C_BaseEntity GetBaseEntity()
    {
        return this;
    }

    public virtual IClientThinkable GetClientThinkable()
    {
        return this;
    }

    public virtual Vector GetRenderOrigin()
    {

    }

    public virtual QAngle GetRenderAngles()
    {

    }

    public virtual Vector GetObserverCamOrigin()
    {
        return GetRenderOrigin();
    }

    public virtual Matrix3x4 RenderableToWorldTransform()
    {

    }

    public virtual bool IsTransparent()
    {

    }

    public virtual bool IsTwoPass()
    {

    }

    public virtual bool UsesPowerOfTwoFrameBufferTexture()
    {

    }

    public virtual bool UsesFullFrameBufferTexture()
    {

    }

    public virtual bool IgnoresZBuffer()
    {

    }

    public virtual Model GetModel()
    {

    }

    public virtual int DrawModel(int flags)
    {

    }

    public virtual void ComputeFxBlend()
    {

    }

    public virtual bool LODTest()
    {
        return true;
    }

    public virtual void GetRenderBounds(Vector mins, Vector maxs)
    {

    }

    public virtual IPVSNotify GetPVSNotifyInterface()
    {

    }

    public virtual void GetRenderBoundsWorldspace(Vector absMins, Vector absMaxs)
    {

    }

    public virtual void GetShadowRenderBounds(Vector mins, Vector maxs, ShadowType shadowType)
    {

    }

    public virtual void GetColorModulation(float[] color)
    {

    }

    public virtual void OnThreadedDrawSetup()
    {

    }

    public virtual bool TestCollision(Ray ray, uint contentsMask, Trace tr)
    {

    }

    public virtual bool TestHitboxes(Ray ray, uint contentsMask, Trace tr)
    {

    }

    public C_BaseEntity GetOwnerEntity()
    {

    }

    public void SetOwnerEntity(C_BaseEntity owner)
    {

    }

    public C_BaseEntity GetEffectEntity()
    {

    }

    public void SetEffectEntity(C_BaseEntity effectEnt)
    {

    }

    public virtual float GetAttackDamageScale()
    {

    }

    public virtual void NotifyShouldTransmit(ShouldTransmitState state)
    {

    }

    public virtual void PreDataUpdate(DataUpdateType updateType)
    {

    }

    public virtual void PostDataUpdate(DataUpdateType updateType)
    {

    }

    public virtual void ValidateModelIndex()
    {

    }

    public virtual void SetDormant(bool dormant)
    {

    }

    public virtual void IsDormant()
    {

    }

    public virtual void SetDestroyedOnRecreateEntities()
    {

    }

    public virtual int GetEFlags()
    {

    }

    public virtual int SetEFlags(int eFlags)
    {

    }

    public void AddEFlags(int eFlagMask)
    {

    }

    public void RemoveEFlags(int eFlagMask)
    {

    }

    public bool IsEFlagSet(int eFlagMask)
    {

    }

    public bool IsMarkedForDeletion()
    {

    }

    public virtual int entindex()
    {

    }

    public int GetSoundSourceIndex()
    {

    }

    public virtual void ReceiveMessage(int classID, BF_Read msg)
    {

    }

    public virtual nint GetDataTableBasePtr()
    {

    }

    public virtual void ClientThink()
    {

    }

    public virtual ClientThinkHandle GetThinkHandle()
    {

    }

    public virtual void SetThinkHandle(ClientThinkHandle think)
    {

    }

    public void AddVar(byte[] data, IInterpolatedVar watcher, int type, bool setup = false)
    {

    }

    public void RemoveVar(byte[] data, bool assert = true)
    {

    }

    public VarMapping GetVarMapping()
    {

    }

    public CCollisionProperty CollisionProp()
    {

    }

    public CParticleProperty ParticleProp()
    {

    }

    public bool IsFloating()
    {

    }

    public virtual bool ShouldSavePhysics()
    {

    }

    public virtual void OnSave()
    {

    }

    public virtual void OnRestore()
    {

    }

    public virtual int ObjectCaps()
    {

    }

    public virtual int Save(ISave save)
    {

    }

    public virtual int Restore(IRestore restore)
    {

    }

    private int SaveDataDescBlock(ISave save, DataMap dmap)
    {

    }

    private int RestoreDataBlock(IRestore restore, DataMap dmap)
    {

    }

    private void OnPostRestoreData()
    {

    }

    public virtual bool CreateVPhysics()
    {

    }

    public IPhysicsObject VPhysicsInitStatic()
    {

    }

    public IPhysicsObject VPhysicsInitNormal(SolidType solidType, int solidFlags, bool createAsleep, Solid solid = null)
    {

    }

    public IPhysicsObject VPhysicsInitShadow(bool allowPhysicsMovement, bool allowPhysicsRotation, Solid solid = null)
    {

    }

    private bool VPhysicsInitSetup()
    {

    }

    public void VPhysicsSetObject(IPhysicsObject physics)
    {

    }

    public virtual void VPhysicsDestroyObject()
    {

    }

    public virtual void VPhysicsUpdate(IPhysicsObject physics)
    {

    }

    public IPhysicsObject VPhysicsGetObject()
    {
        return physicsObject;
    }

    public virtual int VPhysicsGetObjectList(IPhysicsObject[] list, int listMax)
    {

    }

    public virtual bool VPhysicsIsFlesh()
    {

    }

    public virtual bool SetupBones(Matrix3x4 boneToWorldOut, int maxBones, int boneMask, float currentTime)
    {

    }

    public virtual void SetupWeights(Matrix3x4 boneToWorld, int flexWeightCount, float[] flexWeights, float[] flexDelayedWeights)
    {

    }

    public virtual bool UsesFlexDelayedWeights()
    {
        return false;
    }

    public virtual void DoAnimationEvents()
    {

    }

    public virtual void AddEntity()
    {

    }

    public virtual Vector GetAbsOrigin()
    {

    }

    public virtual QAngle GetAbsAngles()
    {

    }

    public Vector GetNetworkOrigin()
    {

    }

    public QAngle GetNetworkAngles()
    {

    }

    public void SetNetworkOrigin(Vector org)
    {

    }

    public void SetNetworkAngles(QAngle ang)
    {

    }

    public Vector GetLocalOrigin()
    {

    }

    public void SetLocalOrigin(Vector origin)
    {

    }

    public float GetLocalOriginDim(int dim)
    {

    }

    public void SetLocalOriginDim(int dim, float value)
    {

    }

    public QAngle GetLocalAngles()
    {

    }

    public void SetLocalAngles(QAngle angles)
    {

    }

    public float GetLocalAnglesDim(int dim)
    {

    }

    public void SetLocalAnglesDim(int dim, float value)
    {

    }

    public virtual Vector GetPrevLocalOrigin()
    {

    }

    public virtual QAngle GetPrevLocalAngles()
    {

    }

    public void SetLocalTransform(Matrix3x4 localTransform)
    {

    }

    public void SetModelName(string name)
    {

    }

    public string GetModelName()
    {

    }

    public int GetModelIndex()
    {

    }

    public void SetModelIndex(int index)
    {

    }

    public virtual int CalcOverrideModelIndex()
    {
        return -1;
    }

    public virtual Vector WorldAlignMins()
    {

    }

    public virtual Vector WorldAlignMaxs()
    {

    }

    public void SetCollisionBounds(Vector mins, Vector maxs)
    {

    }

    public virtual Vector WorldSpaceCenter()
    {
        
    }

    public Vector WorldAlignSize()
    {

    }

    public bool IsPointSized()
    {

    }

    public float BoundingRadius()
    {

    }

    public virtual void ComputeWorldSpaceSurroundingBox(Vector worldMins, Vector worldMaxs)
    {

    }

    public Matrix3x4 EntityToWorldTransform()
    {

    }

    public void EntityToWorldSpace(Vector @in, out Vector @out)
    {

    }

    public void WorldToEntitySpace(Vector @in, out Vector @out)
    {

    }

    public Matrix3x4 GetParentWorldTransform(Matrix3x4 tempMatrix)
    {

    }

    public void GetVectors(Vector forwar, Vector right, Vector up)
    {

    }

    public void SetAbsOrigin(Vector origin)
    {

    }

    public void SetAbsAngles(QAngle angles)
    {

    }

    public void AddFlag(int flags)
    {

    }

    public void RemoveFlag(int flagsToRemove)
    {

    }

    public void ToggleFlag(int flagToToggle)
    {

    }

    public int GetFlags()
    {

    }

    public void ClearFlags()
    {

    }

    public MoveType GetMoveType()
    {

    }

    public MoveCollide GetMoveCollide()
    {

    }

    public virtual SolidType GetSolid()
    {

    }

    public virtual int GetSolidFlags()
    {

    }

    public bool IsSolidFlagSet(int flagMask)
    {

    }

    public void SetSolidFlags(int flags)
    {

    }

    public void AddSolidFlags(int flags)
    {

    }

    public void RemoveSolidFlags(int flags)
    {

    }

    public bool IsSolid()
    {

    }

    public virtual CMouthInfo GetMouth()
    {

    }

    public virtual bool GetSoundSpatialization(SpatializationInfo info)
    {

    }

    public virtual int LookupAttachment(string attachmentName)
    {

    }

    public virtual bool GetAttachment(int number, Matrix3x4 matrix)
    {

    }

    public virtual bool GetAttachment(int number, Vector origin)
    {

    }

    public virtual bool GetAttachment(int number, Vector origin, QAngle angles)
    {

    }

    public virtual bool GetAttachmentVelocity(int number, Vector originLevel, Quaternion angleVel)
    {

    }

    public virtual C_Team GetTeam()
    {

    }

    public virtual int GetTeamNumber()
    {

    }

    public virtual void ChangeTeam(int teamNum)
    {

    }

    public virtual int GetRenderTeamNumber()
    {

    }

    public virtual bool InSameTeam(C_BaseEntity entity)
    {

    }

    public virtual bool InLocalTeam()
    {

    }

    public virtual bool IsValidIDTarget()
    {
        return false;
    }

    public virtual string GetIDString()
    {
        return "";
    }

    public virtual void ModifyEmitSoundParams(EmitSound @params)
    {

    }

    public void EmitSound(string soundname, float soundtime = 0.0f, float[] duration = null)
    {

    }

    public void EmitSound(string soundname, HSOUNDSCRIPTHANDLE handle, float soundtime = 0.0f, float[] duration = null)
    {

    }

    public void StopSound(string soundname)
    {

    }

    public void StopSound(string soundname, HSOUNDSCRIPTHANDLE handle)
    {

    }

    public void GenderExpandString(string @in, out string @out, int maxlen)
    {

    }

    public static float GetSoundDuration(string soundname, string actormodel)
    {

    }

    public static bool GetParametersForSound(string soundname, CSoundParameters @params, string actormodel)
    {

    }

    public static bool GetParametersForSound(string soundname, HSOUNDSCRIPTHANDLE handle, CSoundParameters @params, string actormodel)
    {

    }

    public static void EmitSound(IRecipientFilter filter, int entIndex, string soundname, Vector origin = null, float soundtime = 0.0f, float[] duration = null)
    {

    }

    public static void EmitSound(IRecipientFilter filter, int entIndex, string soundname, HSOUNDSCRIPTHANDLE handle, Vector origin = null, float soundtime = 0.0f, float[] duration = null)
    {

    }

    public static void StopSound(int entIndex, string soundname)
    {

    }

    public static SoundLevel LookupSoundLevel(string soundname)
    {

    }

    public static SoundLevel LookupSoundLevel(string soundname, HSOUNDSCRIPTHANDLE handle)
    {

    }

    public static void EmitSound(IRecipientFilter filter, int entIndex, EmitSound @params)
    {

    }

    public static void EmitSound(IRecipientFilter filter, int entIndex, EmitSound @params, HSOUNDSCRIPTHANDLE handle)
    {

    }

    public static void StopSound(int entIndex, int channel, string sample)
    {
        
    }

    public static void EmitAmbientSound(int entindex, Vector origin, string soundname, int flags = 0, float soundtime = 0.0f, float[] duration = null)
    {

    }

    public static HSOUNDSCRIPTHANDLE PrecacheScriptSound(string soundname)
    {

    }

    public static void PrefetchScriptSound(string soundname)
    {

    }

    public static void RemoveRecipientIfNotCloseCaptioning(C_RecipientFilter filter)
    {

    }

    public static void EmitCloseCaption(IRecipientFilter filter, int entindex, string token, CUtlVector<Vector> soundorigins, float duration, bool warnifmissing = false)
    {

    }

    public static void MarkAimEntsDirty()
    {

    }

    public static void CalcAimEntPositions()
    {

    }

    public static bool IsPrecacheAllowed()
    {

    }

    public static void SetAllowPrecache(bool allow)
    {

    }

    public static bool IsSimulatingOnAlternateTicks()
    {

    }

    public void UpdatePartitionListEntry()
    {

    }

    public virtual bool InitializeAsClientEntity(string modelName, RenderGroup renderGroup)
    {

    }

    public virtual void Simulate()
    {

    }

    public virtual void OnDataChanged(DataUpdateType type)
    {

    }

    public virtual void OnPreDataChanged(DataUpdateType type)
    {

    }

    public bool IsStandable()
    {

    }

    public bool IsBSPModel()
    {

    }

    public virtual IClientVehicle GetClientVehicle()
    {
        return null;
    }

    public virtual void GetAimEntOrigin(IClientEntity attachedTo, Vector absOrigin, QAngle absAngles)
    {

    }

    public virtual Vector GetOldOrigin()
    {

    }

    public C_BaseEntity GetMoveParent()
    {

    }

    public C_BaseEntity GetRootMoveParent()
    {

    }

    public C_BaseEntity FirstMoveChild()
    {

    }

    public C_BaseEntity NextMovePeer()
    {

    }

    public ClientEntityHandle GetClientHandle()
    {
        return new ClientEntityHandle(refEHandle);
    }

    public bool IsServerEntity()
    {

    }

    public virtual RenderGroup GetRenderGroup()
    {

    }

    public virtual void GetToolRecordingState(KeyValues msg)
    {

    }

    public virtual void CleanupToolRecordingState(KeyValues msg)
    {

    }

    public virtual CollideType GetCollideType()
    {

    }

    public virtual bool ShouldDraw()
    {

    }

    public bool IsVisible()
    {
        return render != Client_Render_Handle.INVALID_CLIENT_RENDER_HANDLE;
    }

    public void UpdateVisibility()
    {

    }

    public virtual bool IsSelfAnimating()
    {

    }

    public virtual void OnLatchInterpolatedVariables(int flags)
    {

    }

    public void OnStoreLastNetworkedValue()
    {

    }

    public virtual CStudioHdr OnNewModel()
    {

    }

    public virtual void OnNewParticleEffect(string particleName, CNewParticleEffect newParticleEffect)
    {

    }

    public bool IsSimulatedEveryTick()
    {

    }

    public bool IsAnimatedEveryTick()
    {

    }

    public void SetSimulatedEveryTick(bool sim)
    {

    }

    public void SetAnimatedEveryTick(bool anim)
    {

    }

    public void Interp_Reset(VarMapping map)
    {

    }

    public virtual void ResetLatched()
    {

    }

    public float GetInterpolationAmount(int flags)
    {

    }

    public float GetLastChangeTime(int flags)
    {

    }

    public virtual bool Interpolate(float currentTime)
    {

    }

    public bool Teleported()
    {

    }

    public virtual bool IsSubModel()
    {

    }

    public virtual void CreateLightEffects()
    {

    }

    public void AddToAimEntsList()
    {

    }

    public void RemoveFromAimEntsList()
    {

    }

    public virtual void Clear()
    {

    }

    public virtual int DrawBrushModel(bool translucent, int flags, bool twoPass)
    {

    }

    public virtual float GetTextureAnimationStartTime()
    {

    }

    public virtual void TextureAnimationWrapped()
    {

    }

    public virtual void SetNextClientThink(float nextThinkTime)
    {

    }

    public virtual void SetHealth(int health)
    {

    }

    public virtual int GetHealth()
    {
        return 0;
    }

    public virtual int GetMaxHealth()
    {
        return 1;
    }

    public virtual bool IsVisibleToTargetID()
    {
        return false;
    }

    public float HealthFraction()
    {

    }

    public virtual ShadowType ShadowCastType()
    {

    }

    public virtual bool ShouldReceiveProjectedTextures(int flags)
    {

    }

    public virtual bool IsShadowDirty()
    {

    }

    public virtual void MarkShadowDirty(bool dirty)
    {

    }

    public virtual IClientRenderable GetShadowParent()
    {

    }

    public virtual IClientRenderable FirstShadowChild()
    {

    }

    public virtual IClientRenderable NextShadowPeer()
    {

    }

    public void AddToLeafSystem()
    {

    }

    public void AddToLeafSystem(RenderGroup group)
    {

    }

    public void RemoveFromLeafSystem()
    {

    }

    public virtual void AddDecal(Vector rayStart, Vector rayEnd, Vector decalCenter, int hitbox, int decalIndex, bool doTrace, Trace tr, int maxLODToDecal = IStudioRender.ADDDECAL_TO_ALL_LODS)
    {

    }

    public virtual void AddColoredDecal(Vector rayStart, Vector rayEnd, Vector decalCenter, int hitbox, int decalIndex, bool doTrace, Trace tr, Color color, int maxLODDetail = IStudioRender.ADDDECAL_TO_ALL_LODS)
    {

    }

    public void RemoveAllDecals()
    {

    }

    public bool IsBrushModel()
    {

    }

    public float ProxyRandomValue()
    {
        return proxyRandomValue;
    }

    public float SpawnTime()
    {
        return spawnTime;
    }

    public virtual bool IsClientCreated()
    {

    }

    public virtual void UpdateOnRemove()
    {

    }

    public virtual void SUB_Remove()
    {

    }

    public void CheckInitPredictable(string context)
    {

    }

    public void AllocateIntermediateData()
    {

    }

    public void DestroyIntermediateData()
    {

    }

    public void ShiftIntermediateDataForward(int slots_to_remove, int previous_last_slot)
    {

    }

    public nint GetPredictedFrame(int framenumber)
    {

    }

    public nint GetOriginalNetworkDataObject()
    {

    }

    public bool IsIntermediateDataAllocated()
    {

    }

    public void InitPredictable()
    {

    }

    public void ShutdownPredictable()
    {

    }

    public virtual void SetPredictable(bool state)
    {

    }

    public bool GetPredictable()
    {

    }

    public void PreEntityPacketReceived(int commands_acknowledged)
    {

    }

    public void PostEntityPacketReceived()
    {

    }

    public bool PostNetworkDataRecived(int commands_acknowledged)
    {

    }

    public bool GetPredictionEligible()
    {

    }

    public void SetPredictionEligible()
    {

    }

    public int SaveData(string context, int slot, int type)
    {

    }

    public virtual int RestoreData(string context, int slot, int type)
    {
        
    }

    public virtual string DamageDecal(int damageType, int gameMaterial)
    {

    }

    public virtual void DecalTrace(Trace trace, string decalName)
    {

    }

    public virtual void ImpactTrace(Trace trace, int damageType, string customImpactName)
    {

    }

    public virtual bool ShouldPredict()
    {
        return false;
    }


    public virtual void Think()
    {
        Debug.Assert(think != Think, "Infinite recursion is inifinitely bad.");

        if (think != null)
        {
            think.Invoke();
        }
    }

    public void PhysicsDispatchThink(C_BaseEntity thinkFunc)
    {

    }

    public enum ABSBBoxVisualization
    {
        VISUALIZE_COLLISION_BOUNDS = 0x1,
        VISUALIZE_SURROUNDING_BOUNDS = 0x2,
        VISUALIZE_RENDER_BOUNDS = 0x4
    }

    public void ToggleBBoxVisualization(ABSBBoxVisualization visFlags)
    {

    }

    public void DrawBBoxVisualizations()
    {

    }

    public void SetSize(Vector min, Vector max)
    {

    }

    public string GetClassname()
    {

    }

    public string GetDebugName()
    {

    }

    public static int PrecacheModel(string name)
    {

    }

    public static bool PrecacheSound(string name)
    {

    }

    public static void PrefetchSound(string name)
    {

    }

    public void Remove()
    {

    }

    public char GetParentAttachment()
    {

    }

    public bool HasDataObjectType(int type)
    {

    }

    public void AddDataObjectType(int type)
    {

    }

    public void RemoveDataObjectType(int type)
    {

    }

    public nint GetDataObject(int type)
    {

    }
    
    public nint CreateDataObject(int type)
    {

    }

    public void DestroyDataObject(int type)
    {

    }

    public void DestroyAllDataObjects()
    {

    }

    public void EstimateAbsVelocity(Vector vel)
    {

    }

    public void SetPlayerSimulated(C_BasePlayer owner)
    {

    }

    public bool IsPlayerSimulated()
    {

    }

    public CBasePlayer GetSimulatingPlayer()
    {

    }

    public void UnsetPlayerSimulated()
    {

    }

    public virtual bool CanBePoweredUp()
    {
        return false;
    }

    public virtual bool AttemptToPowerup(int powerup, float time, float amount = 0, C_BaseEntity attacker = null, CDamageModifier damageModifier = null)
    {
        return false;
    }

    public void SetCheckUntouch(bool check)
    {

    }

    public bool GetCheckUntouch()
    {

    }

    public virtual bool IsCurrentlyTouching()
    {

    }

    public virtual void StartTouch(C_BaseEntity other)
    {
        
    }

    public virtual void Touch(C_BaseEntity other)
    {

    }

    public virtual void EndTouch(C_BaseEntity other)
    {

    }

    public void PhysicsStep()
    {

    }

    public TouchLink PhysicsMarkEntityAsTouched(C_BaseEntity other)
    {

    }

    public void PhysicsTouch(C_BaseEntity other)
    {

    }

    public void PhysicsStartTouch(C_BaseEntity other)
    {

    }

    public static Trace GetTouchTrace()
    {

    }

    public void PhysicsImpact(C_BaseEntity other, Trace trace)
    {

    }

    public void PhysicsMarkEntitiesAsTouching(C_BaseEntity other, Trace trace)
    {

    }

    public void PhysicsMarkEntitiesAsTouchingEventDriven(C_BaseEntity other, Trace trace)
    {

    }

    public static void PhysicsRemoveTouchedList(C_BaseEntity ent)
    {

    }

    public static void PhysicsNotifyOtherOfUntouch(C_BaseEntity ent, C_BaseEntity other)
    {

    }

    public static void PhysicsRemoveToucher(C_BaseEntity other, TouchLink link)
    {

    }

    public GroundLink AddEntityToGroundList(C_BaseEntity other)
    {

    }

    public void PhysicsStartGroundContact(C_BaseEntity other)
    {

    }

    public static void PhysicsNotifyOtherOfGroundRemoval(C_BaseEntity ent, C_BaseEntity other)
    {

    }

    public static void PhysicsRemoveGround(C_BaseEntity other, GroundLink link)
    {

    }

    public static void PhysicsRemoveGroundList(C_BaseEntity ent)
    {

    }

    public void StartGroundContact(C_BaseEntity ground)
    {

    }

    public void EndGroundContact(C_BaseEntity ground)
    {

    }

    public void SetGroundChangeTime(float time)
    {

    }

    public float GetGroundChangeTime()
    {

    }

    public void WakeRestingObjects()
    {

    }

    public bool HasNPCsOnIt()
    {

    }

    public bool PhysicsCheckWater()
    {

    }

    public void PhysicsCheckVelocity()
    {

    }

    public void PhysicsAddHalfVelocity(float timestep)
    {

    }

    public void PhysicsAddGravityMove(Vector move)
    {

    }

    public virtual uint PhysicsSolidMaskForEntity()
    {

    }

    public void SetGroundEntity(C_BaseEntity ground)
    {

    }

    public C_BaseEntity GetGroundEntity()
    {

    }

    public void PhysicsPushEntity(Vector push, Trace trace)
    {

    }

    public void PhysicsCheckWaterTransition()
    {

    }

    public void PerformFlyCollisionResolution(Trace trace, Vector move)
    {

    }

    public void ResolveFlyCollisionBounce(Trace trace, Vector velocity, float minTotalElasticity = 0.0f)
    {

    }

    public void ResolveFlyCollisionSlide(Trace trace, Vector velocity)
    {

    }

    public void ResolveFlyCollisionCustom(Trace trace, Vector velocity)
    {

    }

    public void PhysicsCheckForEntityUntouch()
    {

    }

    public void CreateShadow()
    {

    }

    public void DestroyShadow()
    {

    }

    protected enum ThinkMethods
    {
        THINK_FIRE_ALL_FUNCTIONS,
        THINK_FIRE_BASE_ONLY,
        THINK_FIRE_ALL_BUT_BASE
    }

    public void SetParent(C_BaseEntity parentEntity, int parentAttachment = 0)
    {

    }

    public bool PhysicsRunThink(ThinkMethods thinkMethod = ThinkMethods.THINK_FIRE_ALL_FUNCTIONS)
    {

    }

    public bool PhysicsRunSpecificThink(int contextIndex, C_BaseEntity thinkFunc)
    {

    }

    public virtual void PhysicsSimulate()
    {

    }

    public virtual bool IsAlive()
    {

    }

    public bool IsInWorld()
    {
        return true;
    }

    public bool IsWorld()
    {
        return entindex() == 0;
    }

    public virtual bool IsPlayer()
    {
        return false;
    }

    public virtual bool IsBaseCombatCharacter()
    {
        return false;
    }

    public virtual C_BaseCombatCharacter MyCombatCharacterPointer()
    {
        return null;
    }

    public virtual bool IsNPC()
    {
        return false;
    }

    public C_AI_BaseNPC MyNPCPointer()
    {

    }

    public virtual bool IsNextBot()
    {
        return false;
    }

    public virtual bool IsBaseObject()
    {
        return false;
    }

    public virtual bool IsBaseCombatWeapon()
    {
        return false;
    }

    public virtual C_BaseCombatWeapon MyCombatWeaponPointer()
    {
        return null;
    }

    public virtual bool IsCombatItem()
    {
        return false;
    }

    public virtual bool IsBaseTrain()
    {
        return false;
    }

    public virtual Vector EyePosition()
    {

    }

    public virtual QAngle EyeAngles()
    {

    }

    public virtual QAngle LocalEyeAngles()
    {

    }

    public virtual Vector EarPosition()
    {

    }

    public virtual bool ShouldCollide(int collisionGroup, int contentsMask)
    {

    }

    public void SetFriction(float friction)
    {

    }


    public void SetGravity(float gravity)
    {

    }

    public float GetGravity()
    {

    }

    public void SetModelByIndex(int modelIndex)
    {

    }

    public bool SetModel(string modelName)
    {

    }

    public void SetModelPointer(Model model)
    {

    }

    public void SetMoveType(MoveType val, MoveCollide moveCollide = MoveCollide.MOVECOLLIDE_DEFAULT)
    {

    }

    public void SetMoveCollide(MoveCollide val)
    {

    }

    public void SetSolid(SolidType val)
    {

    }

    public void SetLocalVelocity(Vector velocity)
    {

    }

    public void SetAbsVelocity(Vector velocity)
    {

    }

    public Vector GetLocalVelocity()
    {

    }

    public Vector GetAbsVelocity()
    {

    }

    public void ApplyLocalVelocityImpulse(Vector impulse)
    {

    }

    public void ApplyAbsVelocityImpulse(Vector impulse)
    {

    }

    public void ApplyLocalAngularVelocityImpulse(AngularImpulse angImpulse)
    {

    }

    public void SetLocalAngularVelocity(QAngle angVelocity)
    {

    }

    public QAngle GetLocalAngularVelocity()
    {

    }

    public Vector GetBaseVelocity()
    {

    }

    public void SetBaseVelocity(Vector velocity)
    {

    }

    public virtual Vector GetViewOffset()
    {

    }

    public virtual void SetViewOffset(Vector offset)
    {

    }

#if SIXENSE
    public Vector GetEyeOffset()
    {

    }

    public void SetEyeOffset(Vector offset)
    {

    }

    public QAngle GetEyeAngleOffset()
    {

    }

    public void SetEyeAngleOffset(QAngle offset)
    {

    }
#endif // SIXENSE

    public void InvalidatePhysicsRecursive(int changeFlags)
    {

    }

    public ClientRenderHandle GetRenderHandle()
    {

    }

    public void SetRemovalFlag(bool remove)
    {

    }

    public bool IsEffectActive(int effectIndex)
    {

    }

    public void AddEffects(int effects)
    {

    }

    public void RemoveEffects(int effects)
    {

    }

    public int GetEffects()
    {

    }

    public void ClearEffects()
    {

    }

    public void SetEffects(int effects)
    {

    }

    public void ComputeAbsPosition(Vector localPosition, Vector absPosition)
    {

    }

    public void ComputeAbsDirection(Vector localDirection, Vector absDirection)
    {

    }

    public void FollowEntity(C_BaseEntity baseEntity, bool boneMerge = true)
    {

    }

    public void StopFollowingEntity()
    {

    }

    public bool IsFollowingEntity()
    {

    }

    public C_BaseEntity GetFollowedEntity()
    {

    }

    public virtual int GetBody()
    {
        return 0;
    }

    public virtual int GetSkin()
    {
        return 0;
    }

    public void NetworkStateManualMode(bool activate)
    {

    }

    public void NetworkStateChanged()
    {

    }

    public void NetworkStateChanged(nint var)
    {

    }

    public void NetworkStateSetUpdateInterval(float n)
    {

    }

    public void NetworkStateForcedUpdate()
    {

    }

    public int RegisterThinkContext(string context)
    {

    }

    public C_BaseEntity ThinkSet(C_BaseEntity func, float nextThinkTime = 0, string context = null)
    {

    }

    public void SetNextThink(float nextThinkTime, string context = null)
    {

    }

    public float GetNextThink(string context = null)
    {

    }

    public float GetLastThink(string context = null)
    {

    }

    public int GetNextThinkTick(string context = null)
    {

    }

    public int GetLastThinkTick(string context = null)
    {

    }

    public void CheckHasThinkFunction(bool isThinkingHint = false)
    {

    }

    public void CheckHasGamePhysicsSimulation()
    {

    }

    public bool WillThink()
    {

    }

    public bool WillSimulateGamePhysics()
    {

    }

    public int GetFirstThinkTick()
    {

    }

    public float GetAnimTime()
    {

    }

    public void SetAnimTime(float at)
    {

    }

    public float GetSimulationTime()
    {

    }

    public void SetSimulationTime(float st)
    {

    }

    public float GetCreateTime()
    {
        return createTime;
    }

    public void SetCreateTime(float createTime)
    {
        this.createTime = createTime;
    }

    public int GetCreationTick()
    {

    }

#if DEBUG
    public void FunctionCheck(object function, string name)
    {

    }

    public ENTITYFUNCPTR TouchSet(ENTITYFUNCPTR func, string name)
    {
        touch = func;
        return func;
    }
#endif // DEBUG

    public virtual ModelInstanceHandle GetModelInstance()
    {
        return modelInstance;
    }

    public void SetModelInstance(ModelInstanceHandle instance)
    {
        modelInstance = instance;
    }

    public bool SnatchModelInstance(C_BaseEntity toEntity)
    {

    }

    public virtual ClientShadowHandle GetShadowHandle()
    {
        return shadowHandle;
    }

    public virtual ClientRenderHandle RenderHandle()
    {

    }

    public void CreateModelInstance()
    {

    }

    public void MoveToLastReceivedPosition(bool force = false)
    {

    }

    protected void DestroyModelInstance()
    {

    }

    protected static void ProcessTeleportList()
    {

    }

    protected static void ProcessInterpolatedList()
    {

    }

    protected static void CheckInterpolatedVarParanoidMeasurement()
    {

    }

    protected virtual bool ShouldInterpolate()
    {

    }

    protected void MarkMessageReceived()
    {

    }

    protected float GetLastMessageTime()
    {
        return lastMessageTime;
    }

    protected int PhysicsClipVelocity(Vector @in, Vector normal, out Vector @out, float overbounce)
    {

    }

    protected virtual byte GetClientSideFade()
    {
        return 255;
    }

    protected enum InterpolateStatus
    {
        INTERPOLATE_STOP = 0,
        INTERPOLATE_CONTINUE
    }

    protected int BaseInterpolatePart1(float currentTime, Vector oldOrigin, QAngle oldAngles, Vector oldVel, bool noMoreChanges)
    {

    }

    protected void BaseInterpolatePart2(Vector oldOrigin, QAngle oldAngles, Vector oldVel, int changeFlags)
    {

    }

    public static int GetPredictionRandomSeed()
    {

    }

    public static void SetPredictionRandomSeed(CUserCmd cmd)
    {

    }

    public static C_BasePlayer GetPredictionPlayer()
    {

    }

    public static void SetPredictionPlayer(C_BasePlayer player)
    {

    }

    public static void CheckCLInterpChanged()
    {

    }

    public int GetCollisionGroup()
    {

    }

    public void SetCollisionGroup(int collisionGroup)
    {

    }

    public void CollisionRulesChanged()
    {

    }

    public static C_BaseEntity Instance(int ent)
    {

    }

    public static C_BaseEntity Instance(IClientEntity ent)
    {

    }

    public static C_BaseEntity Instance(CBaseHandle ent)
    {

    }

    public static bool IsServer()
    {

    }

    public static bool IsClient()
    {

    }

    public static string GetDLLType()
    {

    }

    public static void SetAbsQueriesValid(bool enable)
    {

    }

    public static bool IsAbsQueriesValid()
    {

    }

    public static void PushEnableAbsRecomputations(bool enable)
    {
        
    }

    public static void PopEnableAbsRecomputations()
    {

    }

    public static void EnableAbsRecomputation(bool enable)
    {

    }

    public static bool IsAbsRecomputationEnabled()
    {

    }

    public virtual void BoneMergeFastCullBloat(Vector localMins, Vector localMaxs, Vector thisEntityMins, Vector thisEntityMaxs)
    {
        
    }

    public Color32 GetRenderColor()
    {

    }

    public void SetRenderColor(byte r, byte g, byte b)
    {

    }

    public void SetRenderColor(byte r, byte g, byte b, byte a)
    {

    }

    public void SetRenderColorR(byte r)
    {

    }

    public void SetRenderColorG(byte g)
    {

    }

    public void SetRenderColorB(byte b)
    {

    }

    public void SetRenderColorA(byte a)
    {

    }

    public void SetRenderMode(RenderMode renderMode, bool forceUpdate = false)
    {

    }

    public RenderMode GetRenderMode()
    {

    }

    public static bool IsInterpolationEnabled()
    {

    }

    public bool IsNoInterpolationFrame()
    {

    }

    public virtual bool OnPredictedEntityRemove(bool isbeingremoved, C_BaseEntity predicted)
    {

    }

    public bool IsDormantPredictable()
    {

    }

    public bool BecomeDormantThisPacket()
    {

    }

    public void SetDormantPredictable(bool dormant)
    {

    }

    public int GetWaterLevel()
    {
        
    }

    public void SetWaterLevel(int level)
    {

    }

    public int GetWaterType()
    {

    }

    public void SetWaterType(int type)
    {

    }

    public float GetElasticity()
    {

    }

    public int GetTextureFrameIndex()
    {

    }

    public void SetTextureFrameIndex(int index)
    {

    }

    public virtual bool GetShadowCastDistance(float dist, ShadowType shadowType)
    {

    }

    public virtual bool GetShadowCastDirection(Vector direction, ShadowType shadowType)
    {

    }

    public virtual C_BaseEntity GetShadowUseOtherEntity()
    {

    }

    public virtual void SetShadowUseOtherEntity(C_BaseEntity entity)
    {

    }

    public CInterpolatedVar<QAngle> GetRotationInterpolator()
    {

    }

    public CInterpolatedVar<Vector> GetOriginInterpolator()
    {

    }

    public virtual bool AddRagdollToFadeQueue()
    {
        return true;
    }

    public void MarkRenderHandleDirty()
    {

    }

    public void HierarchyUpdateMoveParent()
    {

    }

    public virtual bool IsDeflectable()
    {
        return false;
    }

    protected int GetIndexForThinkContext(string context)
    {

    }

    protected virtual int GetStudioBody()
    {
        return 0;
    }

    public bool InitializeAsClientEntityByIndex(int index, RenderGroup renderGroup)
    {

    }

    private void OnRenderStart()
    {

    }

    private static void InterpolateServerEntities()
    {

    }

    private static void AddVisibleEntities()
    {

    }

    private static void ToolRecordEntities()
    {

    }

    private void UpdateBaseVelocity()
    {

    }

    private void PhysicsPusher()
    {

    }

    private void PhysicsNone()
    {

    }

    private void PhysicsNoclip()
    {

    }

    private void PhysicsParent()
    {

    }

    private void PhysicsStepRunTimestep(float timestep)
    {

    }

    private void PhysicsToss()
    {

    }

    private void PhysicsCustom()
    {

    }

    private void PhysicsRigidChild()
    {

    }

    private void CalcAbsolutePosition()
    {

    }

    private void CalcAbsoluteVelocity()
    {

    }

    private void SimulateAngles(float frameTime)
    {

    }

    protected virtual void PerformCustomPhysics(Vector newPosition, Vector newVelocity, QAngle newAngles, QAngle newAngVelocity)
    {

    }

    private void AddStudioDecal(Ray ray, int hitbox, int decalIndex, bool doTrace, Trace tr, int maxLODToDecal = IStudioRender.ADDDECAL_TO_ALL_LODS)
    {

    }

    private void AddColoredStudioDecal(Ray ray, int hitbox, int decalIndex, bool doTrace, Trace tr, Color color, int maxLODToDecal)
    {

    }

    private void AddBrushModelDecal(Ray ray, Vector decalCenter, int decalIndex, bool doTrace, Trace trace)
    {

    }

    private void ComputePackedOffsets()
    {

    }

    private int ComputePackedSize_R(DataMap map)
    {

    }

    private int GetIntermediateDataSize()
    {

    }

    private void UnlinkChild(C_BaseEntity parent, C_BaseEntity child)
    {

    }

    private void LinkChild(C_BaseEntity parent, C_BaseEntity child)
    {

    }

    private void HierarchySetParent(C_BaseEntity newParent)
    {

    }

    private void UnlinkFromHierarchy()
    {

    }

    private void UpdateWaterState()
    {

    }

    private void PhysicsCheckSweep(Vector absStart, Vector absDelta, Trace trace)
    {

    }

    private void MoveToAimEnt()
    {

    }

    private void SetNextThink(int contextIndex, float thinkTime)
    {

    }

    private void SetLastThink(int contextIndex, float thinkTime)
    {

    }

    private float GetNextThink(int contextIndex)
    {

    }

    private int GetNextThinkTick(int contextIndex)
    {

    }

    public float GetRenderClipPlane()
    {

    }

    protected void AddToInterpolationList()
    {

    }

    protected void RemoveFromInterpolationList()
    {

    }

    protected void AddToTeleportList()
    {

    }

    protected void RemoveFromTeleportList()
    {

    }

#if TF_CLIENT
    public virtual bool ValidateEntityAttachedToPlayer(bool shouldRetry)
    {

    }

    public bool EntityDeemedInvalid()
    {
        return validatedOwner && deemedInvalid;
    }
#endif // TF_CLIENT

    public static bool FClassnameIs(C_BaseEntity entity, string classname)
    {
        Debug.Assert(entity != null);

        if (entity == null)
        {
            return false;
        }

        return string.Compare(entity.GetClassname(), classname) == 0 ? true : false;
    }
}