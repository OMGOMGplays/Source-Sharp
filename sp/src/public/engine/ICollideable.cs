namespace SourceSharp.SP.Public.Engine;

public enum SolidType;
public class IHandleEntity;
public struct Ray;
public struct Model;
public class Vector;
public class QAngle;
public class CGameTrace;
public class Trace : CGameTrace;
public class IClientUnknown;

public interface ICollideable
{
    public IHandleEntity GetEntityHandle();

    public Vector OBBMinsPreScaled();
    public Vector OBBMaxsPreScaled();
    public Vector OBBMins();
    public Vector OBBMaxs();

    public void WorldSpaceTriggerBounds(Vector worldMins, Vector worldMaxs);

    public bool TestCollision(Ray ray, uint contentsMask, Trace tr);
    public bool TestHitboxes(Ray ray, uint contentsMask, Trace tr);

    public int GetCollisionModelIndex();

    public Model GetCollisionModel();

    public Vector GetCollisionOrigin();
    public QAngle GetCollisionAngles();
    public Matrix3x4 CollisionToWorldTransform();

    public SolidType GetSolid();
    public int GetSolidFlags();

    public IClientUnknown GetIClientUnknown();

    public int GetCollisionGroup();

    public void WorldSpaceSurroundingBounds(Vector mins, Vector maxs);

    public Matrix3x4 GetRootParentToWorldTransform();
}