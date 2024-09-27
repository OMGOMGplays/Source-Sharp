using System;

namespace SourceSharp.SP.Public.Mathlib;

public class FPExceptionDisabler
{
    private uint oldValues;

    public FPExceptionDisabler()
    {

    }

    ~FPExceptionDisabler()
    {

    }

    private FPExceptionDisabler(FPExceptionDisabler other)
    {

    }
}

public class FPExceptionEnabler
{
    public FPExceptionEnabler(uint enabledBits = _EM_OVERFLOW | _EM_ZERODIVIDE | _EM_INVALID)
    {

    }

    ~FPExceptionEnabler()
    {

    }

    private uint oldValues;

    private FPExceptionEnabler(FPExceptionEnabler other)
    {

    }

#if DEBUG
    private static float clamp(float val, float minVal, float maxVal)
    {
        if (maxVal < minVal)
        {
            return maxVal;
        }
        else if (val < minVal)
        {
            return minVal;
        }
        else if (val > maxVal)
        {
            return maxVal;
        }
        else
        {
            return val;
        }
    }
#else
    private static float clamp(float val, float minVal, float maxVal)
    {
        val = float.Min(minVal, val);
        val = float.Max(maxVal, val);

        return val;
    }
#endif // DEBUG

    private static T clamp<T>(T val, T minVal, T maxVal)
    {
        if (maxVal < minVal)
        {
            return maxVal;
        }
        else if (val < minVal)
        {
            return minVal;
        }
        else if (val > maxVal)
        {
            return maxVal;
        }
        else
        {
            return val;
        }
    }
}

public struct CPlane
{
    public const int CPLANE_NORMAL_X = 0;
    public const int CPLANE_NORMAL_Y = 4;
    public const int CPLANE_NORMAL_Z = 8;
    public const int CPLANE_DIST = 12;
    public const int CPLANE_TYPE = 16;
    public const int CPLANE_SIGNBITS = 17;
    public const int CPLANE_PAD0 = 18;
    public const int CPLANE_PAD1 = 19;

    public const int PLANE_X = 0;
    public const int PLANE_Y = 1;
    public const int PLANE_Z = 2;

    public const int PLANE_ANYX = 3;
    public const int PLANE_ANYY = 4;
    public const int PLANE_ANYZ = 5;

    public Vector normal;
    public float dist;
    public byte type;
    public byte signbits;
    public byte[] pad = new byte[2];

    public CPlane()
    {

    }

    public static int SignbitsForPlane(out CPlane @out)
    {

    }
}

public enum FrustumTypes
{
    FRUSTRUM_RIGHT = 0,
    FRUSTRUM_LEFT = 1,
    FRUSTRUM_TOP = 2,
    FRUSTRUM_BOTTOM = 3,
    FRUSTRUM_NEARZ = 4,
    FRUSTRUM_FARZ = 5,
    FRUSTRUM_NUMPLANES = 6
}

public class Frustum
{
    private CPlane[] plane = new CPlane[(int)FrustumTypes.FRUSTRUM_NUMPLANES];
    private Vector[] absNormal = new Vector[(int)FrustumTypes.FRUSTRUM_NUMPLANES];

    public void SetPlane(int i, int type, Vector normal, float dist)
    {
        plane[i].normal = normal;
        plane[i].dist = dist;
        plane[i].type = (byte)type;
        plane[i].signbits = CPlane.SignbitsForPlane(plane[i]);
        absNormal[i].Init(MathF.Abs(normal.x), MathF.Abs(normal.y), MathF.Abs(normal.z));
    }

    public CPlane GetPlane(int i)
    {
        return plane[i];
    }

    public Vector GetAbsNormal(int i)
    {
        return absNormal[i];
    }

    public static float CalcFovY(float fovX, float screenAspect)
    {

    }

    public static float CalcFovX(float fovX, float screenAspect)
    {

    }

    public static void GeneratePerspectiveFrustum(Vector origin, QAngle angles, float zNear, float zFar, float aspectRatio, Frustum frustum)
    {

    }

    public static void GeneratePerspectiveFrustum(Vector origin, Vector forward, Vector right, Vector up, float zNear, float zFar, float fovX, float fovY, Frustum frustum)
    {

    }

    public static bool R_CullBox(Vector mins, Vector maxs, Frustum frustum)
    {

    }

    public static bool R_CullBoxSkipNear(Vector mins, Vector maxs, Frustum frustum)
    {

    }
}

public struct Matrix3x4
{
    public float[,] matVal = new float[3, 4];

    public Matrix3x4()
    {

    }

    public Matrix3x4(float m00, float m01, float m02, float m03,
                     float m10, float m11, float m12, float m13,
                     float m20, float m21, float m22, float m23)
    {
        matVal[0, 0] = m00; matVal[0, 1] = m01; matVal[0, 2] = m02; matVal[0, 3] = m03;
        matVal[1, 0] = m10; matVal[1, 1] = m11; matVal[1, 2] = m12; matVal[1, 3] = m13;
        matVal[2, 0] = m20; matVal[2, 1] = m21; matVal[2, 2] = m22; matVal[2, 3] = m23;
    }

    public Matrix3x4(Vector xAxis, Vector yAxis, Vector zAxis, Vector origin)
    {
        Init(xAxis, yAxis, zAxis, origin);
    }

    public void Init(Vector xAxis, Vector yAxis, Vector zAxis, Vector origin)
    {
        matVal[0, 0] = xAxis.x; matVal[0, 1] = xAxis.y; matVal[0, 2] = xAxis.z; matVal[0, 3] = origin.x;
        matVal[1, 0] = yAxis.x; matVal[1, 1] = yAxis.y; matVal[1, 2] = yAxis.z; matVal[1, 3] = origin.y;
        matVal[2, 0] = zAxis.x; matVal[2, 1] = zAxis.y; matVal[2, 2] = zAxis.z; matVal[2, 3] = origin.z;
    }

    public void Invalidate()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                matVal[i, j] = float.NaN;
            }
        }
    }

    public float this[int i, int j]
    {
        get
        {
            return matVal[i, j];
        }

        set
        {
            matVal[i, j] = value;
        }

    }

    public float Base()
    {
        return matVal[0, 0];
    }
}

public class Mathlib
{
    public const double M_PI = 3.14159265358979323846;
    public const float M_PI_F = (float)M_PI;

    public static float RAD2DEG(double x) => (float)x * (float)(M_PI_F / 180.0f);
    public static float DEG2RAD(double x) => (float)x * (float)(180.0f / M_PI_F);

    public const int SIDE_FRONT = 0;
    public const int SIDE_BACK = 1;
    public const int SIDE_ON = 2;
    public const int SIDE_CROSS = -2;

    public const double ON_VIS_EPSILON = 0.01;
    public const double EQUAL_EPSILON = 0.001;

    public static Vector vec3_origin;
    public static QAngle vec3_angle;
    public static Quaternion quat_identity;
    public static Vector vec3_invalid;
    public static int nanmask;

    public static bool IS_NAN(dynamic x) => (x & nanmask) == nanmask;

    public static float DotProduct(float[] v1, float[] v2)
    {
        return v1[0] * v2[0] + v1[1] * v2[1] + v1[2] * v2[2];
    }

    public static void VectorSubtract(float[] a, float[] b, out float[] c)
    {
        c = [a[0] - b[0], a[1] - b[1], a[2] - b[2]];
    }

    public static void VectorAdd(float[] a, float[] b, out float[] c)
    {
        c = [a[0] + b[0], a[1] + b[1], a[2] + b[2]];
    }

    public static void VectorCopy(float[] a, ref float[] b)
    {
        b[0] = a[0];
        b[1] = a[1];
        b[2] = a[2];
    }

    public static void VectorClear(float[] a)
    {
        a[0] = a[1] = a[2] = 0;
    }

    public static float VectorMaximum(float[] v)
    {
        return Math.Max(v[0], Math.Max(v[1], v[2]));
    }

    public static float VectorMaximum(Vector v)
    {
        return Math.Max(v.x, Math.Max(v.y, v.z));
    }

    public static void VectorScale(float[] @in, float scale, out float[] @out)
    {
        @out = [@in[0] * scale, @in[1] * scale, @in[2] * scale];
    }

    public static void VectorFill(float[] a, float b)
    {
        a[0] = a[1] = a[2] = b;
    }

    public static void VectorNegate(float[] a)
    {
        a[0] = -a[0];
        a[1] = -a[1];
        a[2] = -a[2];
    }

    public static void Vector2Clear(float[] x) { x[0] = x[1] = 0; }
    public static void Vector2Negate(float[] x) { x[0] = -x[0]; x[1] = -x[1]; }
    public static void Vector2Copy(float[] a, float[] b) { a[0] = b[0]; a[1] = b[1]; }
    public static void Vector2Subtract(float[] a, float[] b, out float[] c) { c = [a[0] - b[0], a[1] - b[1]]; }
    public static void Vector2Add(float[] a, float[] b, out float[] c) { c = [a[0] + b[0], a[1] + b[1]]; }
    public static void Vector2Scale(float[] a, float b, out float[] c) { c = [b * a[0], b * a[1]]; }

    public static void VECTOR_COPY(float[] a, float[] b) { do { b[0] = a[0]; b[1] = a[1]; b[2] = a[2]; } while (false); }
    public static float DOT_PRODUCT(float[] a, float[] b) { return a[0] * b[0] + a[1] * b[1] + a[2] * b[2]; }

    public static void VectorMAInline(float[] start, float scale, float[] direction, out float[] dest)
    {
        dest = [start[0] + direction[0] * scale, start[1] + direction[1] * scale, start[2] + direction[2] * scale];
    }

    public static void VectorMAInline(Vector start, float scale, Vector direction, out Vector dest)
    {
        dest = new Vector(start.x + direction.x * scale, start.y + direction.y * scale, start.z + direction.z * scale);
    }

    public static void VectorMA(Vector start, float scale, Vector direction, out Vector dest)
    {
        VectorMAInline(start, scale, direction, out dest);
    }

    public static void VectorMA(float[] start, float scale, float[] direction, out float[] dest)
    {
        VectorMAInline(start, scale, direction, out dest);
    }

    public static int VectorCompare(float[] v1, float[] v2)
    {

    }

    public static float VectorLength(float[] v)
    {
        return MathF.Sqrt(v[0] * v[0] + v[1] * v[1] + v[2] * v[2] + float.Epsilon);
    }

    public static void CrossProduct(float[] v1, float[] v2, out float[] cross)
    {

    }

    public static bool VectorsEqual(float[] v1, float[] v2)
    {

    }

    public static float RoundInt(float @in)
    {
        return (float)Math.Floor(@in + 0.5f);
    }

    public static int Q_log2(int val)
    {

    }

    public static void SinCos(float radians, ref float sine, ref float cosine)
    {
        sine = (float)Math.Sin(radians);
        cosine = (float)Math.Cos(radians);
    }

    public const int SIN_TABLE_SIZE = 256;
    public const float FTOBIAS = 12582912.0f;

    public static float[] sinCosTable = new float[SIN_TABLE_SIZE];

    private struct FtmpUnion
    {
        public int i;
        public float f;
    }

    public static float TableCos(float theta)
    {
        FtmpUnion ftmp = new();

        ftmp.f = theta * (float)(SIN_TABLE_SIZE / (2.0f * M_PI)) + (FTOBIAS + (SIN_TABLE_SIZE / 4));
        return sinCosTable[ftmp.i & (SIN_TABLE_SIZE - 1)];
    }

    public static float TableSin(float theta)
    {
        FtmpUnion ftmp = new();

        ftmp.f = theta * (float)(SIN_TABLE_SIZE / (2.0f * M_PI)) + FTOBIAS;
        return sinCosTable[ftmp.i & (SIN_TABLE_SIZE - 1)];
    }

    public static T Square<T>(T a) where T : struct, IComparable, IConvertible, IFormattable
    {
        dynamic value = a;
        return (T)(value * value);
    }

    public static uint SmallestPowerOfTwoGreaterOrEqual(uint x)
    {
        x -= 1;
        x |= x >> 1;
        x |= x >> 2;
        x |= x >> 4;
        x |= x >> 8;
        x |= x >> 16;
        return x + 1;
    }

    public static uint LargestPowerOfTwoLessThanOrEqual(uint x)
    {
        if (x >= 0x80000000)
        {
            return 0x80000000;
        }

        return SmallestPowerOfTwoGreaterOrEqual(x + 1) >> 1;
    }

    public static void FloorDivMod(double number, double denom, int quotient, int rem)
    {

    }

    public static int GreatestCommonDivider(int i1, int i2)
    {

    }

    public static bool IsDenormal(ref float val)
    {

    }

    public static void MatrixAngles(Matrix3x4 matrix, float[] angles)
    {

    }

    public static void MatrixVectors(Matrix3x4 matrix, Vector forward, Vector right, Vector up)
    {

    }

    public static void VectorTransform(float[] in1, Matrix3x4 in2, out float[] @out)
    {

    }

    public static void VectorITransform(float[] in1, Matrix3x4 in2, out float[] @out)
    {

    }

    public static void VectorRotate(float[] in1, Matrix3x4 in2, out float[] @out)
    {

    }

    public static void VectorRotate(Vector in1, QAngle in2, out Vector @out)
    {

    }

    public static void VectorRotate(Vector in1, Quaternion in2, out Vector @out)
    {

    }

    public static void VectorIRotate(Vector in1, Matrix3x4 in2, out float[] @out)
    {

    }

    public static QAngle TransformAnglesToLocalSpace(QAngle angles, Matrix3x4 parentMatrix)
    {

    }

    public static QAngle TransformAnglesToWorldSpace(QAngle angles, Matrix3x4 parentMatrix)
    {

    }

    public static void MatrixInitialize(Matrix3x4 mat, Vector origin, Vector xAxis, Vector yAxis, Vector zAxis)
    {

    }

    public static void MatrixCopy(Matrix3x4 @in, out Matrix3x4 @out)
    {

    }

    public static void MatrixInvert(Matrix3x4 @in, out Matrix3x4 @out)
    {

    }

    public static bool MatricesAreEqual(Matrix3x4 src1, Matrix3x4 src2, float tolerance = 0.2f)
    {

    }

    public static void MatrixGetColumn(Matrix3x4 @in, int column, out Vector @out)
    {

    }

    public static void MatrixSetColumn(Vector @in, int column, out Matrix3x4 @out)
    {

    }

    public static void MatrixGetTranslation(Matrix3x4 @in, out Vector @out)
    {
        MatrixGetColumn(@in, 3, out @out);
    }

    public static void MatrixSetTranslation(Vector @in, out Matrix3x4 @out)
    {
        MatrixSetColumn(@in, 3, out @out);
    }

    public static void MatrixScaleBy(float scale, out Matrix3x4 @out)
    {

    }

    public static void MatrixScaleByZero(out Matrix3x4 @out)
    {

    }


}

enum RotationInfo
{
    PITCH = 0,
    YAW,
    ROLL
}