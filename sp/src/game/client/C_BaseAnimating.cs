using SourceSharp.SP.Mathlib;

namespace SourceSharp.SP.Game.Client;

public struct ClientModelRenderInfo : ModelRenderInfo
{
    public Matrix3x4 lightingOffset;
    public Matrix3x4 modelToWorld;
}

public struct RagdollInfo
{
    public bool active;
    public float saveTime;
    public int numBones;
    public Vector[] bonePos = new Vector[MStudioBone.MAXSTUDIOBONES];
    public Quaternion[] boneQuaternion = new Quaternion[MStudioBone.MAXSTUDIOBONES];

    public RagdollInfo() { }
}

public class CAttachmentData
{
    public Matrix3x4 attachmentToWorld;
    public QAngle angRotation;
    public Vector originVelocity;
    public int lastFramecount = 31;
    public bool anglesComputed = true;
}

public class C_BaseAnimating : C_BaseEntity, IModelLoadCallback
{
    public enum NumValues
    {
        NUM_POSEPAREMETERS = 24,
        NUM_BONECTRLS = 4
    }

    public C_BaseAnimating()
    {

    }

    ~C_BaseAnimating()
    {

    }

    public virtual C_BaseAnimating GetBaseAnimating()
    {
        return this;
    }

    public bool UsesPowerOfTwoFrameBufferTexture()
    {

    }

    public virtual byte GetClientSideFade()
    {

    }

    /// <summary>
    /// Get bone controller values.
    /// </summary>
    public virtual void GetBoneControllers(float[] controllers)
    {

    }

    public virtual float SetBoneController(int controller, float value)
    {

    }

    public LocalFlexController GetNumFlexControllers()
    {

    }

    public string GetFlexDescFacs(int flexDesc)
    {

    }

    public string GetFlexControllerName(LocalFlexController flexController)
    {

    }

    public string GetFlexControllerType(LocalFlexController flexController)
    {

    }

    public virtual void GetAimEntOrigin(IClientEntity attachedTo, Vector absOrigin, QAngle absAngles)
    {

    }

    // Computes a box that surrounds all hitboxes
    public bool ComputeHitboxSurroundingBox(Vector worldMins, Vector worldMaxs)
    {

    }

    public bool ComputeEntitySpaceHitboxSurroundingBox(Vector worldMins, Vector worldMaxs)
    {

    }

    // Gets the hitbox-to-world transfroms, returns false if there was a problem
    public bool HitboxToWorldTransforms(Matrix3x4[] hitboxToWorld)
    {

    }

    // Base model functionality
    public float ClampCycle(float cycle, bool isLooping)
    {

    }

    public void GetPoseParameters(CStudioHdr studioHdr, float[] poseParameter)
    {

    }

    public void BuildTransforms(CStudioHdr studioHdr, Vector pos, Quaternion[] q, Matrix3x4 cameraTransform, int boneMask, CBoneBitList boneComputed)
    {

    }

    public void ApplyBoneMatrixTransform(Matrix3x4 transform)
    {

    }

    public int VPhysicsGetObjectList(out IPhysicsObject[] list, int listMax)
    {

    }

    // Model specific
    public bool SetupBones(out Matrix3x4 boneToWorldOut, int maxBones, int boneMask, float currentTime)
    {

    }
}