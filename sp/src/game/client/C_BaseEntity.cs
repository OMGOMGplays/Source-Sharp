using System;
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

    public string classname;
    public VarMapping varMap;

    public static bool allowPrecache;

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
}