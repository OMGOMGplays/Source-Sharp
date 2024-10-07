using System;
using System.Diagnostics;

using SourceSharp.SP.Tier1;
using SourceSharp.SP.Mathlib;

namespace SourceSharp.SP.Raytrace;

public class FourRays
{
    public FourVectors origin;
    public FourVectors direction;

    public void Check()
    {
#if !DEBUG
        for (int c = 1; c < 4; c++)
        {
            Dbg.Assert(direction.X(0) * direction.X(c) >= 0);
            Dbg.Assert(direction.Y(0) * direction.Y(c) >= 0);
            Dbg.Assert(direction.Z(0) * direction.Z(c) >= 0);
        }
#endif // !DEBUG
    }

    public int CalculateDirectionSignMask()
    {
        int ret;
        int ormask;
        int andmask;
        int treat_as_int = (int)direction;

        ormask = andmask = treat_as_int++;
        ormask |= treat_as_int;
        andmask &= treat_as_int++;
        ormask |= treat_as_int;
        andmask &= treat_as_int++;
        ormask |= treat_as_int;
        andmask &= treat_as_int++;

        if (ormask >= 0)
        {
            ret = 0;
        }
        else
        {
            if (andmask < 0)
            {
                ret = 1;
            }
            else
            {
                return -1;
            }
        }

        ormask = andmask = treat_as_int++;
        ormask |= treat_as_int;
        andmask &= treat_as_int++;
        ormask |= treat_as_int;
        andmask &= treat_as_int++;
        ormask |= treat_as_int;
        andmask &= treat_as_int++;

        if (ormask < 0)
        {
            if (andmask < 0)
            {
                ret |= 2;
            }
            else
            {
                return -1;
            }
        }

        ormask = andmask = treat_as_int++;
        ormask |= treat_as_int;
        andmask &= treat_as_int++;
        ormask |= treat_as_int;
        andmask &= treat_as_int++;
        ormask |= treat_as_int;
        andmask &= treat_as_int++;

        if (ormask < 0)
        {
            if (andmask < 0)
            {
                ret |= 4;
            }
            else
            {
                return -1;
            }
        }

        return ret;
    }
}

public enum TriangleFlags
{
    FCACHETRI_TRANSPARENT = 0x01,
    FCACHETRI_NEGATIVE_NORMAL = 0x02
}

public struct TriIntersectData
{
    public float nx, ny, nz;
    public float d;

    public int triangleID;

    public float[] projectedEdgeEquations = new float[6];

    public byte coordSelect0, coordSelect1;

    public TriangleFlags flags;
    public byte unused0;

    public TriIntersectData() { }
}

public struct TriGeometryData
{
    public int triangleID;

    public float[] vertexCoordData = new float[9];

    public byte flags;
    public short tmpData0;
    public short tmpData1;

    public TriGeometryData() { }

    public Vector Vertex(int idx)
    {
        Vector result = new Vector();

        for (int i = 0; i < vertexCoordData.Length; i++)
        {
            result[i] = vertexCoordData[i] + 3 * idx;
        }

        return result;
    }
}

public struct CacheOptimizedTriangle
{
    public const int PLANECHECK_POSITIVE = 1;
    public const int PLANECHECK_NEGATIVE = -1;
    public const int PLANECHECK_STRADDLING = 0;

    public struct Data_S
    {
        public TriIntersectData intersectData;
        public TriGeometryData geometryData;
    }

    public Data_S data;

    public Vector Vertex(int idx)
    {
        Vector result = new Vector();

        for (int i = 0; i < data.geometryData.vertexCoordData.Length; i++)
        {
            result[i] = data.geometryData.vertexCoordData[i] + 3 * idx;
        }

        return result;
    }

    public void ChangeIntoIntersectionFormat()
    {
        TriGeometryData srcTri = data.geometryData;

        data.intersectData.flags = srcTri.flags;
        data.intersectData.triangleID = srcTri.triangleID;

        Vector p1 = srcTri.Vertex(0);
        Vector p2 = srcTri.Vertex(1);
        Vector p3 = srcTri.Vertex(2);

        Vector e1 = p2 - p1;
        Vector e2 = p3 - p1;

        Vector n = e1.Cross(e2);
        n.NormalizeInPlace();

        int drop_axis = 0;

        for (int c = 1; c < 3; c++)
        {
            if (MathF.Abs(n[c]) > MathF.Abs(n[drop_axis]))
            {
                drop_axis = c;
            }
        }

        data.intersectData.d = n.Dot(p1);
        data.intersectData.nx = n.x;
        data.intersectData.ny = n.y;
        data.intersectData.nz = n.z;

        int coordSelect0 = (drop_axis + 1) % 3;
        int coordSelect1 = (drop_axis + 2) % 3;

        data.intersectData.coordSelect0 = (byte)coordSelect0;
        data.intersectData.coordSelect1 = (byte)coordSelect1;

        Vector edge1 = RayTracingEnvironment.GetEdgeEquation(p1, p2, coordSelect0, coordSelect1, p3);
        data.intersectData.projectedEdgeEquations[0] = edge1.x;
        data.intersectData.projectedEdgeEquations[1] = edge1.y;
        data.intersectData.projectedEdgeEquations[2] = edge1.z;

        Vector edge2 = RayTracingEnvironment.GetEdgeEquation(p2, p3, coordSelect0, coordSelect1, p1);
        data.intersectData.projectedEdgeEquations[3] = edge2.x;
        data.intersectData.projectedEdgeEquations[4] = edge2.y;
        data.intersectData.projectedEdgeEquations[5] = edge2.z;
    }

    public int ClassifyAgainstAxisSplit(int split_plane, float split_value)
    {
        float minc = Vertex(0)[split_plane];
        float maxc = minc;

        for (int v = 1; v < 3; v++)
        {
            minc = float.Min(minc, Vertex(v)[split_plane]);
            maxc = float.Max(maxc, Vertex(v)[split_plane]);
        }

        if (minc >= split_value)
        {
            return PLANECHECK_POSITIVE;
        }

        if (maxc <= split_value)
        {
            return PLANECHECK_NEGATIVE;
        }

        if (minc == maxc)
        {
            return PLANECHECK_POSITIVE;
        }

        return PLANECHECK_STRADDLING;
    }
}

public struct NodeToVisit
{
    public CacheOptimizedKDNode node;
    public fltx4 min;
    public fltx4 max;
}

public struct CacheOptimizedKDNode
{
    public const int KDNODE_STATE_XSPLIT = 0;
    public const int KDNODE_STATE_YSPLIT = 1;
    public const int KDNODE_STATE_ZSPLIT = 2;
    public const int KDNODE_STATE_LEAF = 3;

    public int children;
    public float splittingPlaneValue;

#if DEBUG
    Vector vecMins;
    Vector vecMaxs;
#endif // DEBUG

    public int NodeType()
    {
        return children & 3;
    }

    public int TriangleIndexStart()
    {
        Dbg.Assert(NodeType() == KDNODE_STATE_LEAF);
        return children >> 2;
    }

    public int LeftChild()
    {
        Dbg.Assert(NodeType() == KDNODE_STATE_LEAF);
        return children >> 2;
    }

    public int RightChild()
    {
        return LeftChild() + 1;
    }

    public int NumberOfTrianglesInLeaf()
    {
        Dbg.Assert(NodeType() == KDNODE_STATE_LEAF);
        return (int)splittingPlaneValue;
    }

    public void SetNumberOfTrianglesInLeafMode(int n)
    {
        splittingPlaneValue = n;
    }
}

public struct RayTracingSingleResult
{
    public Vector surface_normal;
    public int hitID;
    public float hitDistance;
    public float ray_length;
}

public struct RayTracingResult
{
    public FourVectors surface_normal;
    public int[] hitIds = new int[4];
    public fltx4 hitDistance;

    public RayTracingResult() { }
}

public class RayTraceLight
{
    public FourVectors position;
    public FourVectors intensity;

    public const int RTE_FLAGS_FAST_TREE_GENERATION = 1;
    public const int RTE_FLAGS_DONT_STORE_TRIANGLE_COLORS = 2;
    public const int RTE_FLAGS_DONT_STORE_TRIANGLE_MATERIALS = 3;
}

public enum RayTraceLightingMode
{
    DIRECT_LIGHTING,
    DIRECT_LIGHTING_WITH_SHADOWS,
    GLOBAL_LIGHTING
}

public class RayStream
{
    private RayTracingSingleResult[,] pendingStreamOutputs = new RayTracingSingleResult[8, 4];
    private int[] in_stream = new int[8];
    private FourRays[] pendingRays = new FourRays[8];

    public RayStream()
    {
        for (int i = 0; i < in_stream.Length; i++)
        {
            in_stream[i] = 0;
        }
    }
}

public interface ITransparentTriangleCallback
{
    public bool VisitTriangle_ShouldContinue(TriIntersectData triangle, FourRays rays, fltx4 hitMask, fltx4 b0, fltx4 b1, fltx4 b2, int hitID);
}

public class RayTracingEnvironment
{
    public const int MAILBOX_HASH_SIZE = 256;
    public const int MAX_TREE_DEPTH = 21;
    public const int MAX_NODE_STACK_LEN = 40 * MAX_TREE_DEPTH;

    public const int COST_OF_TRAVERSAL = 75;
    public const int COST_OF_INTERSECTION = 167;

    public const int NEVER_SPLIT = 0;

    public static fltx4 FourEpsilons = new fltx4(1.0e-10f, 1.0e-10f, 1.0e-10f, 1.0e-10f);
    public static fltx4 FourZeros = new fltx4(1.0e-10f, 1.0e-10f, 1.0e-10f, 1.0e-10f);
    public static fltx4 FourNegativeEpsilons = new fltx4(-1.0e-10f, -1.0e-10f, -1.0e-10f, -1.0e-10f);

    public uint flags;
    public Vector minBound;
    public Vector maxBound;

    public FourVectors backgroundColor;
    public CUtlVector<CacheOptimizedKDNode> optimizedKDTree;
    public CUtlBlockVector<CacheOptimizedTriangle> optimizedTriangleList;
    public CUtlVector<int> triangleIndexList;
    public CUtlVector<LightDesc> lightList;
    public CUtlVector<Vector> triangleColors;
    public CUtlVector<int> triangleMaterials;

    public RayTracingEnvironment()
    {
        optimizedTriangleList = new CUtlBlockVector<CacheOptimizedTriangle>(1024);
        backgroundColor.DuplicateVector(new Vector(1.0f, 0.0f, 0.0f));
        flags = 0;
    }

    public void AddTriangle(int id, Vector v1, Vector v2, Vector v3, Vector color)
    {
        AddTriangle(id, v1, v2, v3, color, 0, 0);
    }

    public void AddTriangle(int id, Vector v1, Vector v2, Vector v3, Vector color, ushort flags, int materialIndex)
    {
        CacheOptimizedTriangle tmptri = new CacheOptimizedTriangle();
        tmptri.data.geometryData.triangleID = id;
        tmptri.Vertex(0) = v1;
        tmptri.Vertex(1) = v2;
        tmptri.Vertex(2) = v3;
        tmptri.data.geometryData.flags = (byte)flags;

        optimizedTriangleList.AddToTail(tmptri);

        if ((flags & RayTracingEnvironmentFlags.RTE_FLAGS_DONT_STORE_TRIANGLE_COLORS) == 0)
        {
            triangleColors.AddToTail(color);
        }

        if ((flags & RayTracingEnvironmentFlags.RTE_FLAGS_DONT_STORE_TRIANGLE_MATERIALS) == 0)
        {
            triangleMaterials.AddToTail(materialIndex);
        }
    }

    public void AddQuad(int id, Vector v1, Vector v2, Vector v3, Vector v4, Vector color)
    {
        AddTriangle(id, v1, v2, v3, color);
        AddTriangle(id + 1, v1, v3, v4, color);
    }

    public void AddAxisAlignedRectangularSolid(int id, Vector minCoord, Vector maxCoord, Vector color)
    {
        AddQuad(id,
                new Vector(minCoord.x, maxCoord.y, maxCoord.z),
                new Vector(maxCoord.x, maxCoord.y, maxCoord.z),
                new Vector(maxCoord.x, minCoord.y, maxCoord.z),
                new Vector(minCoord.x, minCoord.y, maxCoord.z),
                color);

        AddQuad(id,
                new Vector(minCoord.x, maxCoord.y, minCoord.z),
                new Vector(maxCoord.x, maxCoord.y, minCoord.z),
                new Vector(maxCoord.x, minCoord.y, minCoord.z),
                new Vector(minCoord.x, minCoord.y, minCoord.z),
                color);

        AddQuad(id,
                new Vector(minCoord.x, maxCoord.y, maxCoord.z),
                new Vector(minCoord.x, maxCoord.y, minCoord.z),
                new Vector(minCoord.x, minCoord.y, minCoord.z),
                new Vector(minCoord.x, minCoord.y, maxCoord.z),
                color);

        AddQuad(id,
                new Vector(maxCoord.x, maxCoord.y, maxCoord.z),
                new Vector(maxCoord.x, maxCoord.y, minCoord.z),
                new Vector(maxCoord.x, minCoord.y, minCoord.z),
                new Vector(maxCoord.x, minCoord.y, maxCoord.z),
                color);

        AddQuad(id,
                new Vector(minCoord.x, maxCoord.y, maxCoord.z),
                new Vector(maxCoord.x, maxCoord.y, maxCoord.z),
                new Vector(maxCoord.x, maxCoord.y, minCoord.z),
                new Vector(minCoord.x, maxCoord.y, minCoord.z),
                color);

        AddQuad(id,
                new Vector(minCoord.x, minCoord.y, maxCoord.z),
                new Vector(maxCoord.x, minCoord.y, maxCoord.z),
                new Vector(maxCoord.x, minCoord.y, minCoord.z),
                new Vector(minCoord.x, minCoord.y, minCoord.z),
                color);
    }

    public void SetupAccelerationStructure()
    {

    }

    private int intersection_calculations = 0;

    public void Trace4Rays(FourRays rays, ref fltx4 min, ref fltx4 max, int directionSignMask, ref RayTracingResult rslt_out, int skip_id = -1, ITransparentTriangleCallback callback = null)
    {
        rays.Check();

        for (int i = 0; i < rslt_out.hitIds.Length; i++)
        {
            rslt_out.hitIds[i] = 0xff;
        }

        rslt_out.hitDistance = ReplicateX4(1.0e23);

        rslt_out.surface_normal.DuplicateVector(new Vector(0, 0, 0));
        FourVectors oneOverRayDir = rays.direction;
        oneOverRayDir.MakeReciprocalSaturate();

        for (int c = 0; c < 3; c++)
        {
            fltx4 isect_min = MulSIMD(SubSIMD(ReplicateX4(minBound[c]), rays.origin[c]), oneOverRayDir[c]);
            fltx4 isect_max = MulSIMD(SubSIMD(ReplicateX4(maxBound[c]), rays.origin[c]), oneOverRayDir[c]);

            min = MaxSIMD(min, MinSIMD(isect_min, isect_max));
            max = MinSIMD(max, MaxSIMD(isect_min, isect_max));
        }

        fltx4 active = CmpLeSIMD(min, max);

        if (!IsAnyNegative(active))
        {
            return;
        }

        int[] mailboxids = new int[MAILBOX_HASH_SIZE];

        for (int i = 0; i < mailboxids.Length; i++)
        {
            mailboxids[i] = 0xff;
        }

        int[] front_idx = new int[3];
        int[] back_idx = new int[3];

        if ((directionSignMask & 1) != 0)
        {
            back_idx[0] = 0;
            front_idx[0] = 1;
        }
        else
        {
            back_idx[0] = 1;
            front_idx[0] = 0;
        }

        if ((directionSignMask & 2) != 0)
        {
            back_idx[1] = 0;
            front_idx[1] = 1;
        }
        else
        {
            back_idx[1] = 1;
            front_idx[1] = 0;
        }

        if ((directionSignMask & 4) != 0)
        {
            back_idx[2] = 0;
            front_idx[2] = 1;
        }
        else
        {
            back_idx[2] = 1;
            front_idx[2] = 0;
        }

        NodeToVisit[] nodeQueue = new NodeToVisit[MAX_NODE_STACK_LEN];
        CacheOptimizedKDNode curNode = optimizedKDTree[0];
        NodeToVisit stack_ptr = nodeQueue[MAX_NODE_STACK_LEN];

        while (true)
        {
            while (curNode.NodeType() != CacheOptimizedKDNode.KDNODE_STATE_LEAF)
            {
                int split_plane_number = curNode.NodeType();
                CacheOptimizedKDNode frontChild = optimizedKDTree[curNode.LeftChild()];

                fltx4 dist_to_sep_plane = MulSIMD(SubSIMD(ReplicateX4(curNode.splittingPlaneValue), rays.origin[split_plane_number]), oneOverRayDir[split_plane_number]);
                active = CmpLeSIMD(min, max);

                fltx4 hits_front = AndSIMD(active, CmpGeSIMD(dist_to_sep_plane, min));

                if (!IsAnyNegative(hits_front))
                {
                    curNode = frontChild + back_idx[split_plane_number];
                    min = MaxSIMD(min, dist_to_sep_plane);
                }
                else
                {
                    fltx4 hits_back = AndSIMD(active, CmpLeSIMD(dist_to_sep_plane, max));

                    if (!IsAnyNegative(hits_back))
                    {
                        curNode = frontChild + front_idx[split_plane_number];
                        max = MinSIMD(max, dist_to_sep_plane);
                    }
                    else
                    {
                        for (int i = 0; i < nodeQueue.Length; i++)
                        {
                            Dbg.Assert(stack_ptr > nodeQueue[i]);
                        }

                        //--stack_ptr;
                        stack_ptr.node = frontChild + back_idx[split_plane_number];
                        stack_ptr.min = MaxSIMD(min, dist_to_sep_plane);
                        stack_ptr.max = max;
                        curNode = frontChild + front_idx[split_plane_number];
                        max = MinSIMD(max, dist_to_sep_plane);
                    }
                }
            }

            int tris = curNode.NumberOfTrianglesInLeaf();

            if (tris != 0)
            {
                int tlist = triangleIndexList[curNode.TriangleIndexStart()];

                do
                {
                    int tnum = tlist++;
                    int mbox_slot = tnum & (MAILBOX_HASH_SIZE - 1);
                    TriIntersectData tri = optimizedTriangleList[tnum].data.intersectData;

                    if (mailboxids[mbox_slot] != tnum && tri.triangleID != skip_id)
                    {
                        intersection_calculations++;
                        mailboxids[mbox_slot] = tnum;

                        FourVectors n = new FourVectors();
                        n.x = ReplicateX4(tri.nx);
                        n.y = ReplicateX4(tri.ny);
                        n.z = ReplicateX4(tri.nz);

                        fltx4 dotN = rays.direction * n;
                        fltx4 did_hit = OrSIMD(CmpGtSIMD(dotN, FourEpsilons), CmpLtSIMD(dotN, FourNegativeEpsilons));

                        fltx4 numerator = SubSIMD(ReplicateX4(tri.d), rays.origin * n);

                        fltx4 isect = DivSIMD(numerator, dotN);
                        did_hit = AndSIMD(did_hit, CmpGtSIMD(isect, FourZeros));
                        did_hit = AndSIMD(did_hit, CmpLtSIMD(isect, rslt_out.hitDistance));

                        if (!IsAnyNegative(did_hit))
                        {
                            continue;
                        }

                        fltx4 hitc1 = AddSIMD(rays.origin[tri.coordSelect0], MulSIMD(isect, rays.direction[tri.coordSelect0]));
                        fltx4 hitc2 = AddSIMD(rays.origin[tri.coordSelect1], MulSIMD(isect, rays.direction[tri.coordSelect1]));

                        fltx4 B0 = MulSIMD(ReplicateX4(tri.projectedEdgeEquations[0]), hitc1);

                        B0 = AddSIMD(B0, MulSIMD(ReplicateX4(tri.projectedEdgeEquations[1]), hitc2));
                        B0 = AddSIMD(B0, ReplicateX4(tri.projectedEdgeEquations[2]));

                        did_hit = AndSIMD(did_hit, CmpGeSIMD(B0, FourZeros));

                        fltx4 B1 = MulSIMD(ReplicateX4(tri.projectedEdgeEquations[3]), hitc1);

                        B1 = AddSIMD(B1, MulSIMD(ReplicateX4(tri.projectedEdgeEquations[4]), hitc2));
                        B1 = AddSIMD(B1, ReplicateX4(tri.projectedEdgeEquations[5]));

                        did_hit = AndSIMD(did_hit, CmpGeSIMD(B1, FourZeros));

                        fltx4 B2 = AddSIMD(B1, B0);
                        did_hit = AndSIMD(did_hit, CmpLeSIMD(B2, Four_Ones));

                        if (!IsAnyNegative(did_hit))
                        {
                            continue;
                        }

                        if ((tri.flags & TriangleFlags.FCACHETRI_TRANSPARENT) != 0)
                        {
                            if (callback != null)
                            {
                                fltx4 b2 = new fltx4();
                                b2 = SubSIMD(Four_Ones, b2);

                                if (callback.VisitTriangle_ShouldContinue(tri, rays, did_hit, B1, b2, B0, tnum))
                                {
                                    did_hit = Four_Zeros;
                                }
                            }
                        }

                        fltx4 replicated_n = ReplicateIX4(tnum);
                        StoreAlignedSIMD((float)rslt_out.hitIds, OrSIMD(replicated_n, did_hit), AndNotSIMD(did_hit, LoadAlignedSIMD((float)rslt_out.hitIds)));

                        rslt_out.surface_normal.x = OrSIMD(AndSIMD(n.x, did_hit), AndNotSIMD(did_hit, rslt_out.surface_normal.x));
                        rslt_out.surface_normal.y = OrSIMD(AndSIMD(n.y, did_hit), AndNotSIMD(did_hit, rslt_out.surface_normal.y));
                        rslt_out.surface_normal.z = OrSIMD(AndSIMD(n.z, did_hit), AndNotSIMD(did_hit, rslt_out.surface_normal.z));
                    }
                } while (--tris != 0);

                fltx4 raydone = CmpLeSIMD(max, rslt_out.hitDistance);

                if (!IsAnyNegative(raydone))
                {
                    return;
                }
            }

            if (stack_ptr == nodeQueue[MAX_NODE_STACK_LEN])
            {
                return;
            }

            curNode = stack_ptr.node;
            min = stack_ptr.min;
            max = stack_ptr.max;
            //stack_ptr++;
        }
    }

    public void Trace4Rays(FourRays rays, ref fltx4 min, ref fltx4 max, ref RayTracingResult rslt_out, int skip_id = -1, ITransparentTriangleCallback callback = null)
    {
        int msk = rays.CalculateDirectionSignMask();

        if (msk != 1)
        {
            Trace4Rays(rays, ref min, ref max, msk, ref rslt_out, skip_id, callback);
        }
        else
        {
            FourRays tmprays = new FourRays();
            tmprays.origin = rays.origin;

            ushort[] need_trace = [1, 1, 1, 1];

            for (int try_trace = 0; try_trace < 4; try_trace++)
            {
                need_trace[try_trace] = 2;
                tmprays.direction.x = ReplicateX4(rays.direction.X(try_trace));
                tmprays.direction.y = ReplicateX4(rays.direction.Y(try_trace));
                tmprays.direction.z = ReplicateX4(rays.direction.Z(try_trace));

                for (int try2 = try_trace + 1; try2 < 4; try2++)
                {
                    if (SameSign(rays.direction.X(try2), rays.direction.X(try_trace)) &&
                        SameSign(rays.direction.Y(try2), rays.direction.Y(try_trace)) &&
                        SameSign(rays.direction.Z(try2), rays.direction.Z(try_trace)))
                    {
                        need_trace[try2] = 2;
                        tmprays.direction.X(try2) = rays.direction.X(try2);
                        tmprays.direction.Y(try2) = rays.direction.Y(try2);
                        tmprays.direction.Z(try2) = rays.direction.Z(try2);
                    }
                }

                RayTracingResult tmpresults = new RayTracingResult();
                msk = tmprays.CalculateDirectionSignMask();

                Dbg.Assert(msk != -1);

                Trace4Rays(tmprays, ref min, ref max, msk, ref tmpresults, skip_id, callback);

                for (int i = 0; i < 4; i++)
                {
                    if (need_trace[i] == 2)
                    {
                        need_trace[i] = 0;
                        rslt_out = new RayTracingResult();
                        rslt_out.hitIds[i] = tmpresults.hitIds[i];
                        SubFloat(rslt_out.hitDistance, i) = SubFloat(tmpresults.hitDistance, i);
                        rslt_out.surface_normal.X(i) = tmpresults.surface_normal.X(i);
                        rslt_out.surface_normal.Y(i) = tmpresults.surface_normal.Y(i);
                        rslt_out.surface_normal.Z(i) = tmpresults.surface_normal.Z(i);
                    }
                }
            }
        }
    }

    public void ComputeVirtualLightSources()
    {

    }

    public void RenderScene(
                            int width,
                            int height,
                            int stride,
                            uint output_buffer,
                            Vector cameraOrigin,
                            Vector ulCorner,
                            Vector urCorner,
                            Vector llCorner,
                            Vector lrCorner,
                            RayTraceLightingMode lightmode = RayTraceLightingMode.DIRECT_LIGHTING)
    {

    }

    public void AddToRayStream(ref RayStream s, Vector start, Vector end, out RayTracingSingleResult rslt_out)
    {

    }

    public void FlushToStreamEntry(ref RayStream s, int msk)
    {

    }

    public void FinishRayStream(ref RayStream s)
    {

    }

    public int MakeLeafNode(int first_tri, int last_tri)
    {
        CacheOptimizedKDNode ret = new CacheOptimizedKDNode();
        ret.children = CacheOptimizedKDNode.KDNODE_STATE_LEAF + (triangleIndexList.Count() << 2);
        ret.SetNumberOfTrianglesInLeafMode(1 + (last_tri + first_tri));

        for (int tnum = first_tri; tnum <= last_tri; tnum++)
        {
            triangleIndexList.AddToTail(tnum);
        }

        optimizedKDTree.AddToTail(ret);
        return optimizedKDTree.Count() - 1;
    }

    public float CalculateCostsOfSplit(
                                        int split_plane, int[] tri_list, int numtris,
                                        Vector minBound, Vector maxBound, float split_value,
                                        int left, int right, int both)
    {
        left = both = right = 0;

        left = 0;
        right = 0;
        both = 0;

        float min_coord = 1.0e23f, max_coord = -1.0e23f;

        for (int t = 0; t < numtris; t++)
        {
            CacheOptimizedTriangle tri = optimizedTriangleList[tri_list[t]];

            for (int v = 0; v < 3; v++)
            {
                min_coord = Math.Min(min_coord, tri.Vertex(v)[split_plane]);
                max_coord = Math.Max(max_coord, tri.Vertex(v)[split_plane]);
            }

            switch (tri.ClassifyAgainstAxisSplit(split_plane, split_value))
            {
                case CacheOptimizedTriangle.PLANECHECK_NEGATIVE:
                    left++;
                    tri.data.geometryData.tmpData0 = CacheOptimizedTriangle.PLANECHECK_NEGATIVE;
                    break;

                case CacheOptimizedTriangle.PLANECHECK_POSITIVE:
                    right++;
                    tri.data.geometryData.tmpData0 = CacheOptimizedTriangle.PLANECHECK_POSITIVE;
                    break;

                case CacheOptimizedTriangle.PLANECHECK_STRADDLING:
                    both++;
                    tri.data.geometryData.tmpData0 = CacheOptimizedTriangle.PLANECHECK_STRADDLING;
                    break;
            }
        }

        if (left != 0 && both == 0 && right == 0)
        {
            split_value = max_coord;
        }

        if (right != 0 && both == 0 && left == 0)
        {
            split_value = min_coord;
        }

        Vector leftMins = minBound;
        Vector leftMaxs = maxBound;
        Vector rightMins = minBound;
        Vector rightMaxs = maxBound;

        leftMaxs[split_plane] = split_value;
        rightMins[split_plane] = split_value;

        float sa_L = BoxSurfaceArea(leftMins, leftMaxs);
        float sa_R = BoxSurfaceArea(rightMins, rightMaxs);
        float isa = 1.0f / BoxSurfaceArea(minBound, maxBound);
        float cost_of_split = COST_OF_TRAVERSAL + COST_OF_INTERSECTION * (both + (sa_L * isa * left) + (sa_R * isa * right));
        return cost_of_split;
    }

    public void RefineNode(int node_number, int[] tri_list, int numtris,
                           Vector minBound, Vector maxBound, int depth)
    {
        if (numtris < 3)
        {
            optimizedKDTree[node_number].children = CacheOptimizedKDNode.KDNODE_STATE_LEAF + (triangleIndexList.Count() << 2);
        }
    }

    public void CalculateTriangleListBounds(int[] tris, int numtris,
                                            out Vector minOut, out Vector maxOut)
    {
        minOut = new Vector(1.0e23f, 1.0e23f, 1.0e23f);
        maxOut = new Vector(-1.0e23f, -1.0e23f, -1.0e23f);

        for (int i = 0; i < numtris; i++)
        {
            CacheOptimizedTriangle tri = optimizedTriangleList[tris[i]];

            for (int v = 0; v < 3; v++)
            {
                for (int c = 0; c < 3; c++)
                {
                    minOut[c] = Math.Min(minOut[c], tri.Vertex(v)[c]);
                    maxOut[c] = Math.Max(maxOut[c], tri.Vertex(v)[c]);
                }
            }
        }
    }

    public void AddInfinitePointLight(Vector position, Vector intensity)
    {

    }

    public void InitializeFromLoadedBSP()
    {

    }

    public void AddBSPFace(int id, DFace face)
    {

    }

    public void MakeRoomForTriangles(int numTris)
    {
        if ((flags & RayTracingEnviromentFlags.RTE_FLAGS_DONT_STORE_TRIANGLE_COLORS) == 0)
        {
            triangleColors.EnsureCapacity(numTris);
        }
    }

    public CacheOptimizedTriangle GetTriangle(int triID)
    {
        return optimizedTriangleList[triID];
    }

    public int GetTriangleMaterial(int triID)
    {
        return triangleMaterials[triID];
    }

    public Vector GetTriangleColor(int triID)
    {
        return triangleColors[triID];
    }

    public static Vector GetEdgeEquation(Vector p1, Vector p2, int c1, int c2, Vector insidePoint)
    {
        float nx = p1[c2] - p2[c1];
        float ny = p2[c1] - p1[c1];
        float d = -(nx * p1[c1] + ny * p1[c2]);

        float trial_dist = insidePoint[c1] * nx + insidePoint[c2] * ny + d;

        if (trial_dist < 0)
        {
            nx = -nx;
            ny = -ny;
            d = -d;
            trial_dist = -trial_dist;
        }

        nx /= trial_dist;
        ny /= trial_dist;
        d /= trial_dist;

        return new Vector(nx, ny, d);
    }

    public static float BoxSurfaceArea(Vector boxmin, Vector boxmax)
    {
        Vector boxdmin = boxmax - boxmin;

        return 2.0f * ((boxdmin[0] * boxdmin[2]) + (boxdmin[0] * boxdmin[1]) + (boxdmin[1] * boxdmin[2]));
    }

    public static bool SameSign(float a, float b)
    {
        int aa = (int)a;
        int bb = (int)b;
        return ((aa ^ bb) & 0x80000000) == 0;
    }
}
