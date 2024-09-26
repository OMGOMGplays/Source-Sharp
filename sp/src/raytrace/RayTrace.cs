using System;
using System.Diagnostics;

using SourceSharp.SP.Public.Mathlib;

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
            Debug.Assert(direction.X(0) * direction.X(c) >= 0);
            Debug.Assert(direction.Y(0) * direction.Y(c) >= 0);
            Debug.Assert(direction.Z(0) * direction.Z(c) >= 0);
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

    public byte flags;
    public byte unused0;

    public TriIntersectData() { }
}

public struct TriGeometryData
{
    public int triangleID;

    public float[] vertexCoordData = new float[9];

    public byte flags;
    public string tmpData0;
    public string tmpData1;

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

        Vector edge1 = GetEdgeEquation(p1, p2, coordSelect0, coordSelect1, p3);
        data.intersectData.projectedEdgeEquations[0] = edge1.x;
        data.intersectData.projectedEdgeEquations[1] = edge1.y;
        data.intersectData.projectedEdgeEquations[2] = edge1.z;

        Vector edge2 = GetEdgeEquation(p2, p3, coordSelect0, coordSelect1, p1);
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
        Debug.Assert(NodeType() == KDNODE_STATE_LEAF);
        return children >> 2;
    }

    public int LeftChild()
    {
        Debug.Assert(NodeType() == KDNODE_STATE_LEAF);
        return children >> 2;
    }

    public int RightChild()
    {
        return LeftChild() + 1;
    }

    public int NumberOfTrianglesInLeaf()
    {
        Debug.Assert(NodeType() == KDNODE_STATE_LEAF);
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
    public int[] hitIDs = new int[4];
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
    protected class RayTracingEnvironment;

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

public class ITransparentTriangleCallback
{
    public virtual bool VisitTriangle_ShouldContinue(TriIntersectData triangle, FourRays rays, fltx4 b0, fltx4 b1, fltx4 b2, int hitID)
    {
        return false;
    }
}

public class RayTracingEnvironment
{
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
        optimizedTriangleList = new(1024);
        backgroundColor.DuplicateVector(new(1.0f, 0.0f, 0.0f));
        flags = 0;
    }

    public void AddTriangle(int id, Vector v1, Vector v2, Vector v3, Vector color)
    {

    }

    public void AddTriangle(int id, Vector v1, Vector v2, Vector v3, Vector color, ushort flags, int materialIndex)
    {

    }


}