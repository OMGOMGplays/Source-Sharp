#define VECTOR_H

using SourceSharp.sp.src._public.zip;

namespace SourceSharp.sp.src._public.mathlib
{
    public class vector
    {
        public const int X_INDEX = 0;
        public const int Y_INDEX = 1;
        public const int Z_INDEX = 2;

#if VECTOR_PARANOIA
        public static bool CHECK_VALID(dynamic v) => xzip.Assert((v).IsValid());
#else
        public static bool CHECK_VALID(dynamic v) => false;
#endif // VECTOR_PARANOIA

        public static string VecToString(Vector v) => new string($"({v.x} {v.y} {v.z})");

        public class Vector
        {
            public vec_t x, y, z;

            public Vector()
            {
#if DEBUG
#if VECTOR_PARANOIA
                x = y = z = basetypes.VEC_T_NAN;
#endif // VECTOR_PARANOIA
#endif // DEBUG
            }

            public Vector(vec_t x, vec_t y, vec_t z)
            {
                this.x = x; this.y = y; this.z = z;
                CHECK_VALID(this);
            }

            public Vector(vec_t xyz)
            {
                x = y = z = xyz;
                CHECK_VALID(this);
            }

            public void Init(vec_t ix = 0.0f, vec_t iy = 0.0f, vec_t iz = 0.0f)
            {
                x = ix; y = iy; z = iz;
                CHECK_VALID(this);
            }

            public bool IsValid()
            {
                return basetypes.IsFinite(x) && basetypes.IsFinite(y) && basetypes.IsFinite(z);
            }

            public void Invalidate()
            {
                x = y = z = basetypes.VEC_T_NAN;
            }

            public vec_t this[int i]
            {
                get
                {
                    xzip.Assert(cond: (i >= 0) && (i < 3));

                    switch (i)
                    {
                        case X_INDEX:
                            return x;

                        case Y_INDEX:
                            return y;

                        case Z_INDEX:
                            return z;

                        default:
                            return -1;
                    }
                }
                set
                {
                    xzip.Assert(cond: (i >= 0) && (i < 3));

                    switch (i)
                    {
                        case X_INDEX:
                            x = value;
                            break;

                        case Y_INDEX:
                            y = value;
                            break;

                        case Z_INDEX:
                            z = value;
                            break;

                        default:
                            break;
                    }
                }
            }

            public vec_t[] Base()
            {
                vec_t[] result = new vec_t[3];
                result[0] = this[0];
                result[1] = this[1];
                result[2] = this[2];

                return result;
            }

            public vector2d.Vector2D AsVector2D()
            {
                return (vector2d.Vector2D)this;
            }

            public void Random(vec_t minVal, vec_t maxVal)
            {
                x = minVal + ((float)randoverride.rand() / platform.VALVE_RAND_MAX) * (maxVal - minVal);
                y = minVal + ((float)randoverride.rand() / platform.VALVE_RAND_MAX) * (maxVal - minVal);
                z = minVal + ((float)randoverride.rand() / platform.VALVE_RAND_MAX) * (maxVal - minVal);
                CHECK_VALID(this);
            }

            public void Zero()
            {
                x = y = z = 0.0f;
            }

            public static bool operator ==(Vector lhs, Vector rhs)
            {
                CHECK_VALID(lhs);
                CHECK_VALID(rhs);
                return (lhs.x == rhs.x) &&
                       (lhs.y == rhs.y) &&
                       (lhs.z == rhs.z);
            }

            public static bool operator ==(float f, Vector rhs)
            {
                xzip.Assert(cond: false);
                return false;
            }

            public static bool operator ==(Vector lhs, float f)
            {
                xzip.Assert(cond: false);
                return false;
            }

            public static bool operator !=(Vector lhs, Vector rhs)
            {
                CHECK_VALID(lhs);
                CHECK_VALID(rhs);
                return (lhs.x != rhs.x) &&
                       (lhs.y != rhs.y) &&
                       (lhs.z != rhs.z);
            }
            public static bool operator !=(float f, Vector rhs)
            {
                xzip.Assert(cond: false);
                return false;
            }

            public static bool operator !=(Vector lhs, float f)
            {
                xzip.Assert(cond: false);
                return false;
            }

            public static Vector operator +(Vector lhs, Vector rhs)
            {
                CHECK_VALID(lhs);
                CHECK_VALID(rhs);
                lhs.x += rhs.x;
                lhs.y += rhs.y;
                lhs.z += rhs.z;
                return lhs;
            }

            public static Vector operator -(Vector lhs, Vector rhs)
            {
                CHECK_VALID(lhs);
                CHECK_VALID(rhs);
                lhs.x -= rhs.x;
                lhs.y -= rhs.y;
                lhs.z -= rhs.z;
                return lhs;
            }

            public static Vector operator *(Vector lhs, Vector rhs)
            {
                CHECK_VALID(rhs);
                lhs.x *= rhs.x;
                lhs.y *= rhs.y;
                lhs.z *= rhs.z;
                CHECK_VALID(lhs);
                return lhs;
            }

            public static Vector operator *(Vector lhs, float fl)
            {
                lhs.x *= fl;
                lhs.y *= fl;
                lhs.z *= fl;
                CHECK_VALID(lhs);
                return lhs;
            }

            public static Vector operator /(Vector lhs, Vector rhs)
            {
                CHECK_VALID(rhs);
                xzip.Assert(cond: rhs.x != 0.0f && rhs.y != 0.0f && rhs.z != 0.0f);
                lhs.x /= rhs.x;
                lhs.y /= rhs.x;
                lhs.z /= rhs.z;
                CHECK_VALID(lhs);
                return lhs;
            }

            public static Vector operator /(Vector lhs, float fl)
            {
                xzip.Assert(cond: fl != 0.0f);
                float oofl = 1.0f / fl;
                lhs.x *= oofl;
                lhs.y *= oofl;
                lhs.z *= oofl;
                CHECK_VALID(lhs);
                return lhs;
            }

            public static Vector operator +(Vector lhs, float fl)
            {
                lhs.x += fl;
                lhs.y += fl;
                lhs.z += fl;
                CHECK_VALID(lhs);
                return lhs;
            }

            public static Vector operator -(Vector lhs, float fl)
            {
                lhs.x -= fl;
                lhs.y -= fl;
                lhs.z -= fl;
                CHECK_VALID(lhs);
                return lhs;
            }

            public void Negate()
            {
                CHECK_VALID(this);
                x = -x; y = -y; z = -z;
            }

            public vec_t Length()
            {
                CHECK_VALID(this);
                return VectorLength(this);
            }

            public vec_t LengthSqr()
            {
                CHECK_VALID(this);
                return (x * x + y * y + z * z);
            }

            public bool IsZero(float tolerance = 0.0f)
            {
                return (x > -tolerance && x < tolerance &&
                        y > -tolerance && x < tolerance &&
                        z > -tolerance && z < tolerance);
            }

            public vec_t NormalizeInPlace()
            {
                return VectorNormalize(this);
            }

            public Vector Normalized()
            {
                Vector norm = this;
                VectorNormalize(norm);
                return norm;
            }

            public bool IsLengthGreaterThan(float val)
            {
                return LengthSqr() > val * val;
            }

            public bool IsLengthLessThan(float val)
            {
                return LengthSqr() < val * val;
            }

            public bool WithinAABox(Vector boxmin, Vector boxmax)
            {
                return (
                        (x >= boxmin.x) && (x <= boxmax.x) &&
                        (y >= boxmin.y) && (y <= boxmax.y) &&
                        (z >= boxmin.z) && (z <= boxmax.z)
                       );
            }

            public vec_t DistTo(Vector vOther)
            {
                VectorSubtract(this, vOther, out Vector delta);
                return delta.Length();
            }

            public vec_t DistToSqr(Vector vOther)
            {
                Vector delta = new();

                delta.x = x - vOther.x;
                delta.y = y - vOther.y;
                delta.z = z - vOther.z;

                return delta.LengthSqr();
            }

            public void CopyToArray(float[] rgfl)
            {
                xzip.Assert(cond: rgfl != null);
                CHECK_VALID(this);
                rgfl[0] = x;
                rgfl[1] = y;
                rgfl[2] = z;
            }

            public void MulAdd(Vector a, Vector b, float scalar)
            {
                CHECK_VALID(a);
                CHECK_VALID(b);
                x = a.x + b.x * scalar;
                y = a.y + b.y * scalar;
                z = a.z + b.z * scalar;
            }

            public vec_t Dot(Vector vOther)
            {
                CHECK_VALID(vOther);
                return DotProduct(this, vOther);
            }

            //public static Vector operator =(Vector rhs) { return rhs; } // FIXME: Implement, unless automatically so

            public vec_t Length2D()
            {
                return (vec_t)MathF.Sqrt(x * x + y * y);
            }

            public vec_t Length2DSqr()
            {
                return (x * x + y * y);
            }

            public static VectorByValue operator &(Vector lhs, Vector rhs) { return ((VectorByValue)(lhs)); } // FIXME: Correct implementation?

#if !VECTOR_NO_SLOW_OPERATIONS
            public Vector Cross(Vector vOther)
            {
                CrossProduct(this, vOther, out Vector result);
                return result;
            }

            public Vector Min(Vector vOther)
            {
                return new Vector(x < vOther.x ? x : vOther.x, y < vOther.y ? y : vOther.y, z < vOther.z ? z : vOther.z);
            }

            public Vector Max(Vector vOther)
            {
                return new Vector(x > vOther.x ? x : vOther.x, y > vOther.y ? y : vOther.y, z > vOther.z ? z : vOther.z);
            }
#else
            public Vector(Vector vOther)
            {
                return;
            }
#endif // !VECTOR_NO_SLOW_OPERATIONS
        }

        public void NetworkVarConstruct(Vector v) { v.Zero(); }

        //#define USE_M64S ((!defined(_X360))) // FIXME: Find a way to replace #define's, without const's preferrably...

        public class ShortVector
        {
            public short x, y, z, w;

            public void Init(short ix = 0, short iy = 0, short iz = 0, short iw = 0)
            {
                x = ix; y = iy; z = iz; w = iw;
            }

#if USE_M64S
            __m64 AsM64() {return (__m64)x;}
#endif // USE_M64S

            public void Set(ShortVector vOther)
            {
                x = vOther.x;
                y = vOther.y;
                z = vOther.z;
                w = vOther.w;
            }

            public void Set(short ix, short iy, short iz, short iw)
            {
                x = ix;
                y = iy;
                z = iz;
                w = iw;
            }

            public short this[int i]
            {
                get
                {
                    xzip.Assert(cond: (i >= 0) && (i < 4));

                    switch (i)
                    {
                        case X_INDEX:
                            return x;

                        case Y_INDEX:
                            return y;

                        case Z_INDEX:
                            return z;

                        case Z_INDEX + 1:
                            return w;

                        default:
                            return -1;
                    }
                }

                set
                {
                    xzip.Assert(cond: (i >= 0) && (i < 4));

                    switch (i)
                    {
                        case X_INDEX:
                            x = value;
                            break;

                        case Y_INDEX:
                            y = value;
                            break;

                        case Z_INDEX:
                            z = value;
                            break;

                        case Z_INDEX + 1:
                            w = value;
                            break;

                        default:
                            break;
                    }
                }
            }

            public short[] Base()
            {
                short[] val = new short[4];
                val[0] = this[0];
                val[1] = this[1];
                val[2] = this[2];
                val[3] = this[3];

                return val;
            }

            public static bool operator ==(ShortVector lhs, ShortVector rhs)
            {
                return (lhs.x == rhs.x) && (lhs.y == rhs.y) && (lhs.z == rhs.z) && (lhs.w == rhs.w);
            }

            public static bool operator !=(ShortVector lhs, ShortVector rhs)
            {
                return (lhs.x != rhs.x) && (lhs.y != rhs.y) && (lhs.z != rhs.z) && (lhs.w != rhs.w);
            }

            public static ShortVector operator +(ShortVector lhs, ShortVector rhs) { lhs.x += rhs.x; lhs.y += rhs.y; lhs.z += rhs.z; lhs.w += rhs.w; return lhs; }
            public static ShortVector operator -(ShortVector lhs, ShortVector rhs) { lhs.x -= rhs.x; lhs.y -= rhs.y; lhs.z -= rhs.z; lhs.w -= rhs.w; return lhs; }
            public static ShortVector operator *(ShortVector lhs, ShortVector rhs) { lhs.x *= rhs.x; lhs.y *= rhs.y; lhs.z *= rhs.z; lhs.w *= rhs.w; return lhs; }

            public static ShortVector operator *(ShortVector lhs, float fl)
            {
                ShortVectorMultiply(lhs, fl, out ShortVector res);
                return res;
            }

            public static ShortVector operator /(ShortVector lhs, ShortVector rhs) { lhs.x /= rhs.x; lhs.y /= rhs.y; lhs.z /= rhs.z; lhs.w /= rhs.w; return lhs; }
            public static ShortVector operator /(ShortVector lhs, float fl) { lhs.x /= (short)fl; lhs.y /= (short)fl; lhs.z /= (short)fl; lhs.w /= (short)fl; return lhs; }
        }

        public class IntVector4D
        {
            public int x, y, z, w;

            public void Init(int ix = 0, int iy = 0, int iz = 0, int iw = 0)
            {
                x = ix; y = iy; z = iz; w = iw;
            }

#if USE_M64S
            __m64 AsM64() {return (__m64)x;}
#endif // USE_M64S

            public void Set(IntVector4D vOther)
            {
                x = vOther.x;
                y = vOther.y;
                z = vOther.z;
                w = vOther.w;
            }

            public void Set(int ix, int iy, int iz, int iw)
            {
                x = ix;
                y = iy;
                z = iz;
                w = iw;
            }

            public int this[int i]
            {
                get
                {
                    xzip.Assert(cond: (i >= 0) && (i < 4));

                    switch (i)
                    {
                        case X_INDEX:
                            return x;

                        case Y_INDEX:
                            return y;

                        case Z_INDEX:
                            return z;

                        case Z_INDEX + 1:
                            return w;

                        default:
                            return -1;
                    }
                }

                set
                {
                    xzip.Assert(cond: (i >= 0) && (i < 4));

                    switch (i)
                    {
                        case X_INDEX:
                            x = value;
                            break;

                        case Y_INDEX:
                            y = value;
                            break;

                        case Z_INDEX:
                            z = value;
                            break;

                        case Z_INDEX + 1:
                            w = value;
                            break;

                        default:
                            break;
                    }
                }
            }

            public int[] Base()
            {
                int[] val = new int[4];
                val[0] = this[0];
                val[1] = this[1];
                val[2] = this[2];
                val[3] = this[3];
                return val;
            }

            public static bool operator ==(IntVector4D lhs, IntVector4D rhs)
            {
                return (lhs.x == rhs.x) && (lhs.y == rhs.y) && (lhs.z == rhs.z) && (lhs.w == rhs.w);
            }

            public static bool operator !=(IntVector4D lhs, IntVector4D rhs)
            {
                return (lhs.x != rhs.x) && (lhs.y != rhs.y) && (lhs.z != rhs.z) && (lhs.w != rhs.w);
            }

            public static IntVector4D operator +(IntVector4D lhs, IntVector4D rhs) { lhs.x += rhs.x; lhs.y += rhs.y; lhs.z += rhs.z; lhs.w += rhs.w; return lhs; }
            public static IntVector4D operator -(IntVector4D lhs, IntVector4D rhs) { lhs.x -= rhs.x; lhs.y -= rhs.y; lhs.z -= rhs.z; lhs.w -= rhs.w; return lhs; }
            public static IntVector4D operator *(IntVector4D lhs, IntVector4D rhs) { lhs.x *= rhs.x; lhs.y *= rhs.y; lhs.z *= rhs.z; lhs.w *= rhs.w; return lhs; }

            public static IntVector4D operator *(IntVector4D lhs, float fl)
            {
                IntVector4DMultiply(lhs, fl, out IntVector4D res);
                return res;
            }

            public static IntVector4D operator /(IntVector4D lhs, IntVector4D rhs)
            {
                xzip.Assert(cond: rhs.x != 0 && rhs.y != 0 && rhs.z != 0 && rhs.w != 0);
                lhs.x /= rhs.x;
                lhs.y /= rhs.y;
                lhs.z /= rhs.z;
                lhs.w /= rhs.w;
                return lhs;
            }

            public static IntVector4D operator /(IntVector4D lhs, float fl)
            {
                xzip.Assert(cond: fl != 0.0f);
                float oofl = 1.0f / fl;
                lhs.x /= (int)oofl;
                lhs.y /= (int)oofl;
                lhs.z /= (int)oofl;
                lhs.w /= (int)oofl;
                return lhs;
            }
        }

        public class VectorByValue : Vector
        {
            public Vector vector;

            public VectorByValue() { vector = new(); }
            public VectorByValue(vec_t x, vec_t y, vec_t z) { vector = new(x, y, z); }
            public VectorByValue(VectorByValue vOther) { vector = vOther; }
        }

        public class TableVector
        {
            public vec_t x, y, z;

            public static Vector operator &(TableVector lhs, TableVector rhs) { return new Vector(lhs.x, lhs.y, lhs.z); }

            public vec_t this[int i]
            {
                get
                {
                    xzip.Assert(cond: (i >= 0) && (i < 3));

                    switch (i)
                    {
                        case X_INDEX:
                            return x;

                        case Y_INDEX:
                            return y;

                        case Z_INDEX:
                            return z;

                        default:
                            return -1;
                    }
                }

                set
                {
                    xzip.Assert(cond: (i >= 0) && (i < 3));

                    switch (i)
                    {
                        case X_INDEX:
                            x = value;
                            break;

                        case Y_INDEX:
                            y = value;
                            break;

                        case Z_INDEX:
                            z = value;
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        public class VectorAligned : Vector
        {
            public VectorAligned() { }
            public VectorAligned(vec_t x, vec_t y, vec_t z)
            {
                Init(x, y, z);
            }

#if VECTOR_NO_SLOW_OPERATIONS
            private VectorAligned(VectorAligned vOther) {}
            private VectorAligned(Vector vOther) {}
#else
            public VectorAligned(Vector vOther)
            {
                Init(vOther.x, vOther.y, vOther.z);
            }

            //public static VectorAligned operator =(Vector lhs, Vector rhs) // FIXME: Random error... Find out the cause and fix!
            //{
            //    lhs.Init(rhs.x, rhs.y, rhs.z);
            //    return (VectorAligned)lhs;
            //}
#endif // VECTOR_NO_SLOW_OPERATIONS

            public float w;
        }

        public static void VectorClear(Vector a)
        {
            a.x = a.y = a.z = 0.0f;
        }

        public static void VectorCopy(Vector src, Vector dst)
        {
            CHECK_VALID(src);
            dst.x = src.x;
            dst.y = src.y;
            dst.z = src.z;
        }

        public static void VectorAdd(Vector a, Vector b, out Vector result)
        {
            CHECK_VALID(a);
            CHECK_VALID(b);
            Vector c = new();
            c.x = a.x + b.x;
            c.y = a.y + b.y;
            c.z = a.z + b.z;
            result = c;
        }

        public static void VectorSubtract(Vector a, Vector b, out Vector result)
        {
            CHECK_VALID(a);
            CHECK_VALID(b);
            Vector c = new();
            c.x = a.x - b.x;
            c.y = a.y - b.y;
            c.z = a.z - b.z;
            result = c;
        }

        public static void VectorMultiply(Vector a, vec_t b, out Vector result)
        {
            CHECK_VALID(a);
            xzip.Assert(cond: basetypes.IsFinite(b));
            Vector c = new();
            c.x = a.x * b;
            c.y = a.y * b;
            c.z = a.z * b;
            result = c;
        }

        public static void VectorMultiply(Vector a, Vector b, out Vector result)
        {
            CHECK_VALID(a);
            CHECK_VALID(b);
            Vector c = new();
            c.x = a.x * b.x;
            c.y = a.y * b.y;
            c.z = a.z * b.z;
            result = c;
        }

        public static void VectorScale(Vector a, vec_t scale, out Vector result)
        {
            VectorMultiply(a, scale, out result);
        }

        public static void VectorDivide(Vector a, vec_t b, out Vector result)
        {
            CHECK_VALID(a);
            xzip.Assert(cond: b != 0.0f);
            vec_t oob = 1.0f / b;
            Vector c = new();
            c.x = a.x / oob;
            c.y = a.y / oob;
            c.z = a.z / oob;
            result = c;
        }

        public static void VectorDivide(Vector a, Vector b, out Vector result)
        {
            CHECK_VALID(a);
            CHECK_VALID(b);
            xzip.Assert(cond: (b.x != 0.0f) && (b.y != 0.0f) && (b.z != 0.0f));
            Vector c = new();
            c.x = a.x / b.x;
            c.y = a.y / b.y;
            c.z = a.z / b.z;
            result = c;
        }

        public static void VectorMA(Vector start, float scale, Vector direction, Vector dest)
        {

        }

        public static bool VectorsAreEqual(Vector src1, Vector src2, float tolerance = 0.0f)
        {
            if (basetypes.FloatMakePositive(src1.x - src2.x) > tolerance)
            {
                return false;
            }

            if (basetypes.FloatMakePositive(src1.y - src2.y) > tolerance)
            {
                return false;
            }

            return (basetypes.FloatMakePositive(src1.z - src2.z) <= tolerance);
        }

        public static void VectorExpand(Vector v) => throw new NotImplementedException(); // FIXME: Implement!!!

        public static vec_t VectorLength(Vector v)
        {
            CHECK_VALID(v);
            return (vec_t)MathF.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
        }

        public static vec_t DotProduct(Vector a, Vector b)
        {
            CHECK_VALID(a);
            CHECK_VALID(b);
            return (a.x * b.x + a.y * b.y + a.z * b.z);
        }

        public static void CrossProduct(Vector a, Vector b, out Vector result)
        {
            CHECK_VALID(a);
            CHECK_VALID(b);
            //xzip.Assert(a != result); // Potentially not necessary? Results in an error at least
            //xzip.Assert(b != result);
            Vector res = new();
            res.x = a.y * b.z - a.z * b.y;
            res.y = a.z * b.x - a.x * b.z;
            res.z = a.x * b.y - a.y * b.x;
            result = res;
        }

        public static vec_t DotProductAbs(Vector v0, Vector v1)
        {
            CHECK_VALID(v0);
            CHECK_VALID(v1);
            return basetypes.FloatMakePositive(v0.x * v1.x) + basetypes.FloatMakePositive(v0.y * v1.y) + basetypes.FloatMakePositive(v0.z * v1.z);
        }

        public static vec_t DotProductAbs(Vector v0, float[] v1)
        {
            return basetypes.FloatMakePositive(v0.x * v1[0]) + basetypes.FloatMakePositive(v0.y * v1[1]) + basetypes.FloatMakePositive(v0.z * v1[2]);
        }

        public static void VectorMin(Vector a, Vector b, out Vector result)
        {
            Vector res = new();
            res.x = basetypes.MIN(a.x, b.x);
            res.y = basetypes.MIN(a.y, b.y);
            res.z = basetypes.MIN(a.z, b.z);
            result = res;
        }

        public static void VectorMax(Vector a, Vector b, out Vector result)
        {
            Vector res = new();
            res.x = basetypes.MAX(a.x, b.x);
            res.y = basetypes.MAX(a.y, b.y);
            res.z = basetypes.MAX(a.z, b.z);
            result = res;
        }

        public static void VectorLerp(Vector src1, Vector src2, vec_t t, out Vector dest)
        {
            CHECK_VALID(src1);
            CHECK_VALID(src2);
            Vector result = new();
            result.x = src1.x + (src2.x - src1.x) * t;
            result.y = src1.y + (src2.y - src1.y) * t;
            result.z = src1.z + (src2.z - src1.z) * t;
            dest = result;
        }

        public static Vector VectorLerp(Vector src1, Vector src2, vec_t t)
        {
            Vector result = new();
            VectorLerp(src1, src2, t, out result);
            return result;
        }

        public static Vector ReplicateToVector(float x)
        {
            return new Vector(x, x, x);
        }

        public static bool PointWithinViewAngle(Vector vecSrcPosition, Vector vecTargetPosition, Vector vecLookDirection, float flCosHalfFOV)
        {
            Vector vecDelta = vecTargetPosition - vecSrcPosition;
            float cosDiff = DotProduct(vecLookDirection, vecDelta);

            if (cosDiff < 0)
            {
                return false;
            }

            float flLen2 = vecDelta.LengthSqr();

            return (cosDiff * cosDiff > flLen2 * flCosHalfFOV * flCosHalfFOV);
        }

#if !VECTOR_NO_SLOW_OPERATIONS
        public static Vector CrossProduct(Vector a, Vector b)
        {
            return new Vector(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
        }

        public static Vector RandomVector(vec_t minVal, vec_t maxVal)
        {
            Vector random = new();
            random.Random(minVal, maxVal);
            return random;
        }
#endif // !VECTOR_NO_SLOW_OPERATIONS

        public static float RandomVectorInUnitSphere(out Vector pVector)
        {
            float u = ((float)randoverride.rand() / platform.VALVE_RAND_MAX);
            float v = ((float)randoverride.rand() / platform.VALVE_RAND_MAX);
            float w = ((float)randoverride.rand() / platform.VALVE_RAND_MAX);

            float flPhi = mathlib_base.acos(1 - 2 * u);
            float flTheta = 2 * (float)basetypes.M_PI * v;
            float flRadius = mathlib_base.powf(w, 1.0f / 3.0f);

            float flSinPhi, flCosPhi;
            float flSinTheta, flCosTheta;
            mathlib_base.SinCos(flPhi, out flSinPhi, out flCosPhi);
            mathlib_base.SinCos(flTheta, out flSinTheta, out flCosTheta);

            Vector vector = new();

            vector.x = flRadius * flSinPhi * flCosTheta;
            vector.y = flRadius * flSinPhi * flSinTheta;
            vector.z = flRadius * flCosPhi;

            pVector = vector;
            return flRadius;
        }

        public static float RandomVectorInUnitCircle(out vector2d.Vector2D pVector)
        {
            float u = ((float)randoverride.rand() / platform.VALVE_RAND_MAX);
            float v = ((float)randoverride.rand() / platform.VALVE_RAND_MAX);

            float flTheta = 2 * (float)basetypes.M_PI * v;
            float flRadius = mathlib_base.powf(u, 1.0f / 2.0f);

            float flSinTheta, flCosTheta;
            mathlib_base.SinCos(flTheta, out flSinTheta, out flCosTheta);

            vector2d.Vector2D vector = new();

            vector.x = flRadius * flCosTheta;
            vector.y = flRadius * flSinTheta;

            pVector = vector;
            return flRadius;
        }

        // FIXME: This is a random error I don't know the fix for, saying something about short can't multiply with int, despite both sides and the recipient (r.*) being short:s?
        //public static void ShortVectorMultiply(ShortVector src, float fl, out ShortVector res)
        //{
        //    xzip.Assert(cond: basetypes.IsFinite(fl));
        //    ShortVector r = new();
        //    r.x = src.x * (short)fl;
        //    r.y = src.y * (short)fl;
        //    r.z = src.z * (short)fl;
        //    r.w = src.w * (short)fl;
        //    res = r;
        //}

        public static void IntVector4DMultiply(IntVector4D src, float fl, out IntVector4D res)
        {
            xzip.Assert(cond: basetypes.IsFinite(fl));
            IntVector4D r = new();
            r.x = src.x * (int)fl;
            r.y = src.y * (int)fl;
            r.z = src.z * (int)fl;
            r.w = src.w * (int)fl;
            res = r;
        }

        public static Vector AllocTempVector()
        {
            Vector[] s_vecTemp = new Vector[128];
            threadtools.CInterlockedInt s_nIndex;

            int nIndex = 0;

            for (; ; )
            {
                int nOldIndex = s_nIndex;
                nIndex = ((nOldIndex + 0x10001) & 0x7F);

                if (s_nIndex.AssignIf(nOldIndex, nIndex))
                {
                    break;
                }

                threadtools.ThreadPause();
            }

            return s_vecTemp[nIndex];
        }

        public static void ComputeClosestPoint(Vector vecStart, float flMaxDist, Vector vecTarget, out Vector pResult)
        {
            Vector vecDelta = new();
            VectorSubtract(vecTarget, vecStart, out vecDelta);
            float flDistSqr = vecDelta.LengthSqr();

            Vector result = new();

            if (flDistSqr <= flMaxDist * flMaxDist)
            {
                result = vecTarget;
            }
            else
            {
                vecDelta /= MathF.Sqrt(flDistSqr);
                VectorMA(vecStart, flMaxDist, vecDelta, result);
            }

            pResult = result;
        }

        public static void VectorAbs(Vector src, out Vector dst)
        {
            Vector res = new();
            res.x = basetypes.FloatMakePositive(src.x);
            res.y = basetypes.FloatMakePositive(src.y);
            res.z = basetypes.FloatMakePositive(src.z);
            dst = res;
        }

        public static float ComputeVolume(Vector vecMins, Vector vecMaxs)
        {
            Vector vecDelta;
            VectorSubtract(vecMins, vecMaxs, out vecDelta);
            return DotProduct(vecDelta, vecDelta);
        }

        public static Vector AngularImpulse;

#if !VECTOR_NO_SLOW_OPERATIONS
        public static Vector RandomAngularImpulse(float minVal, float maxVal)
        {
            Vector angImp = new();
            angImp.Random(minVal, maxVal);
            return angImp;
        }
#endif // !VECTOR_NO_SLOW_OPERATIONS

        public class Quaternion
        {
            public Quaternion()
            {
#if DEBUG
#if VECTOR_PARANOIA
                x = y = z = w = basetypes.VEC_T_NAN;
#endif // VECTOR_PARANOIA
#endif // DEBUG
            }

            public Quaternion(vec_t ix, vec_t iy, vec_t iz, vec_t iw)
            {
                x = ix; y = iy; z = iz; w = iw;
            }

            public Quaternion(RadianEuler angle)
            {
                AngleQuaternion(angle, this);
            }

            public void Init(vec_t ix = 0.0f, vec_t iy = 0.0f, vec_t iz = 0.0f, vec_t iw = 0.0f)
            {
                x = ix; y = iy; z = iz; w = iw;
            }

            public bool IsValid()
            {
                return basetypes.IsFinite(x) && basetypes.IsFinite(y) && basetypes.IsFinite(z) && basetypes.IsFinite(w);
            }

            public void Invalidate()
            {
                x = y = z = w = basetypes.VEC_T_NAN;
            }

            public static bool operator ==(Quaternion lhs, Quaternion rhs)
            {
                return (lhs.x == rhs.x) && (lhs.y == rhs.y) && (lhs.z == rhs.z) && (lhs.w == rhs.w);
            }

            public static bool operator !=(Quaternion lhs, Quaternion rhs)
            {
                return (lhs.x != rhs.x) && (lhs.y != rhs.y) && (lhs.z != rhs.z) && (lhs.w != rhs.w);
            }

            public vec_t[] Base()
            {
                vec_t[] val = new vec_t[4];
                val[0] = x;
                val[1] = y;
                val[2] = z;
                val[3] = w;

                return val;
            }

            public vec_t this[int i]
            {
                get
                {
                    xzip.Assert(cond: (i >= 0) && (i < 4));

                    switch (i)
                    {
                        case X_INDEX:
                            return x;

                        case Y_INDEX:
                            return y;

                        case Z_INDEX:
                            return z;

                        case Z_INDEX + 1:
                            return w;

                        default:
                            return -1;
                    }
                }

                set
                {
                    xzip.Assert(cond: (i >= 0) && (i < 4));

                    switch (i)
                    {
                        case X_INDEX:
                            x = value;
                            break;

                        case Y_INDEX:
                            y = value;
                            break;

                        case Z_INDEX:
                            z = value;
                            break;

                        case Z_INDEX + 1:
                            w = value;
                            break;

                        default:
                            break;
                    }
                }
            }

            public vec_t x, y, z, w;
        }

        public static bool QuaternionsAreEqual(Quaternion src1, Quaternion src2, float tolerance)
        {
            if (basetypes.FloatMakePositive(src1.x - src2.x) > tolerance)
            {
                return false;
            }

            if (basetypes.FloatMakePositive(src1.y - src2.y) > tolerance)
            {
                return false;
            }

            if (basetypes.FloatMakePositive(src1.z - src2.z) > tolerance)
            {
                return false;
            }

            return (basetypes.FloatMakePositive(src1.w - src2.w) <= tolerance);
        }

        public class QuaternionAligned : Quaternion
        {
            public QuaternionAligned() { }

            public QuaternionAligned(vec_t x, vec_t y, vec_t z, vec_t w)
            {
                Init(x, y, z, w);
            }

#if VECTOR_NO_SLOW_OPERATIONS
            private QuaternionAligned(QuaternionAligned vOther) {}
            private QuaternionAligned(Quaternion vOther) {}
#else
            public QuaternionAligned(Quaternion vOther)
            {
                Init(vOther.x, vOther.y, vOther.z, vOther.w);
            }

            //public static QuaternionAligned operator =(QuaternionAligned lhs, QuaternionAligned rhs)
            //{
            //    lhs.Init(rhs.x, rhs.y, rhs.z, rhs.w);
            //    return lhs;
            //}
#endif // VECTOR_NO_SLOW_OPERATIONS
        }

        public class RadianEuler
        {
            public RadianEuler()
            {
            }

            public RadianEuler(vec_t x, vec_t y, vec_t z)
            {
                this.x = x; this.y = y; this.z = z;
            }

            public RadianEuler(Quaternion q)
            {
                QuaternionAngles(q, this);
            }

            public RadianEuler(QAngle angles)
            {
                Init(angles.z * 3.1414159265358979323846f / 180.0f, angles.x * 3.1414159265358979323846f / 180.0f, angles.y * 3.1414159265358979323846f / 180.0f);
            }

            public void Init(vec_t ix = 0.0f, vec_t iy = 0.0f, vec_t iz = 0.0f) { x = ix; y = iy; z = iz; }

            public QAngle ToQAngle()
            {
                return new QAngle(y * 180.0f / 3.1414159265358979323846f, z * 180.0f / 3.1414159265358979323846f, x * 180.0f / 3.1414159265358979323846f);
            }

            public bool IsValid()
            {
                return basetypes.IsFinite(x) && basetypes.IsFinite(y) && basetypes.IsFinite(z);
            }

            public void Invalidate()
            {
                x = y = z = basetypes.VEC_T_NAN;
            }

            public vec_t this[int i]
            {
                get
                {
                    xzip.Assert(cond: (i >= 0) && (i < 3));

                    switch (i)
                    {
                        case X_INDEX:
                            return x;

                        case Y_INDEX:
                            return y;

                        case Z_INDEX:
                            return z;

                        default:
                            return -1;
                    }
                }

                set
                {
                    xzip.Assert(cond: (i >= 0) && (i < 3));

                    switch (i)
                    {
                        case X_INDEX:
                            x = value;
                            break;

                        case Y_INDEX:
                            y = value;
                            break;

                        case Z_INDEX:
                            z = value;
                            break;

                        default:
                            break;
                    }
                }
            }

            public vec_t x, y, z;
        }

        public static void AngleQuaternion(RadianEuler angles, Quaternion qt) { }
        public static void QuaternionAngles(Quaternion q, RadianEuler angles) { }

        public static void NetworkVarConstruct(Quaternion q) { q.x = q.y = q.z = q.w = 0.0f; }

        public static void VectorCopy(RadianEuler src, out RadianEuler dst)
        {
            //CHECK_VALID(src);
            RadianEuler res = new();
            res.x = src.x;
            res.y = src.y;
            res.z = src.z;
            dst = res;
        }

        public static void VectorScale(RadianEuler src, float b, out RadianEuler dst)
        {
            CHECK_VALID(src);
            xzip.Assert(cond: basetypes.IsFinite(b));
            RadianEuler res = new();
            res.x = src.x * b;
            res.y = src.y * b;
            res.z = src.z * b;
            dst = res;
        }

        public class QAngle
        {
            public vec_t x, y, z;

            public QAngle()
            {
#if DEBUG
#if VECTOR_PARANOIA
                x = y = z = basetypes.VEC_T_NAN;
#endif // VECTOR_PARANOIA
#endif // DEBUG
            }

            public QAngle(vec_t x, vec_t y, vec_t z)
            {
                this.x = x; this.y = y; this.z = z;
                CHECK_VALID(this);
            }

            //public static QAngleByValue operator &(QAngle lhs, QAngle rhs) { return ((QAngleByValue)lhs); } // FIXME: Implement properly

            public void Init(vec_t ix = 0.0f, vec_t iy = 0.0f, vec_t iz = 0.0f)
            {
                x = ix; y = iy; z = iz;
                CHECK_VALID(this);
            }

            public void Random(vec_t minVal, vec_t maxVal)
            {
                x = minVal + ((float)randoverride.rand() / platform.VALVE_RAND_MAX) * (maxVal - minVal);
                y = minVal + ((float)randoverride.rand() / platform.VALVE_RAND_MAX) * (maxVal - minVal);
                z = minVal + ((float)randoverride.rand() / platform.VALVE_RAND_MAX) * (maxVal - minVal);
                CHECK_VALID(this);
            }

            public bool IsValid()
            {
                return basetypes.IsFinite(x) && basetypes.IsFinite(y) && basetypes.IsFinite(z);
            }

            public void Invalidate()
            {
                x = y = z = basetypes.VEC_T_NAN;
            }

            public vec_t this[int i]
            {
                get
                {
                    xzip.Assert(cond: (i >= 0) && (i < 3));

                    switch (i)
                    {
                        case X_INDEX:
                            return x;

                        case Y_INDEX:
                            return y;

                        case Z_INDEX:
                            return z;

                        default:
                            return -1;
                    }
                }

                set
                {
                    xzip.Assert(cond: (i >= 0) && (i < 3));

                    switch (i)
                    {
                        case X_INDEX:
                            x = value;
                            break;

                        case Y_INDEX:
                            y = value;
                            break;

                        case Z_INDEX:
                            z = value;
                            break;

                        default:
                            break;
                    }
                }
            }

            public vec_t[] Base()
            {
                vec_t[] val = new vec_t[3];
                val[0] = this[0];
                val[1] = this[1];
                val[2] = this[2];
                return val;
            }

            public static bool operator ==(QAngle lhs, QAngle rhs)
            {
                return (lhs.x == rhs.x) && (lhs.y == rhs.y) && (lhs.z == rhs.z);
            }

            public static bool operator !=(QAngle lhs, QAngle rhs)
            {
                return (lhs.x != rhs.x) && (lhs.y != rhs.y) && (lhs.z != rhs.z);
            }

            public static QAngle operator +(QAngle lhs, QAngle rhs)
            {
                lhs.x += rhs.x;
                lhs.y += rhs.y;
                lhs.z += rhs.z;
                return lhs;
            }

            public static QAngle operator -(QAngle lhs, QAngle rhs)
            {
                lhs.x -= rhs.x;
                lhs.y -= rhs.y;
                lhs.z -= rhs.z;
                return lhs;
            }

            public static QAngle operator *(QAngle lhs, float fl)
            {
                lhs.x *= fl;
                lhs.y *= fl;
                lhs.z *= fl;
                return lhs;
            }

            public static QAngle operator /(QAngle lhs, float fl)
            {
                lhs.x /= fl;
                lhs.x /= fl;
                lhs.x /= fl;
                return lhs;
            }

            public vec_t Length()
            {
                return (vec_t)MathF.Sqrt(LengthSqr());
            }

            public vec_t LengthSqr()
            {
                return x * x + y * y + z * z;
            }

#if !VECTOR_NO_SLOW_OPERATIONS
            // INFO: The header file contains operators above
#else
            private QAngle(QAngle vOther) {}
#endif // !VECTOR_NO_SLOW_OPERATIONS
        }

        public static void NetworkVarConstruct(QAngle q) { q.x = q.y = q.z = 0.0f; }

        public class QAngleByValue : QAngle
        {
            public QAngleByValue() : base() { }

            public QAngleByValue(vec_t x, vec_t y, vec_t z) : base(x, y, z) { }

            public QAngleByValue(QAngleByValue vOther)
            {
                x = vOther.x;
                y = vOther.y;
                z = vOther.z;
            }

            private QAngle angle;
        }

        public static void VectorAdd(QAngle a, QAngle b, out QAngle result)
        {
            QAngle res = new();
            res.x = a.x + b.x;
            res.y = a.y + b.y;
            res.z = a.z + b.z;
            result = res;
        }

        public static void VectorCopy(QAngle src, out QAngle dst)
        {
            QAngle d = new();
            d.x = src.x;
            d.y = src.y;
            d.z = src.z;
            dst = d;
        }

        public static void VectorMA(QAngle start, float scale, QAngle direction, out QAngle dest)
        {
            QAngle d = new();
            d.x = start.x + scale * direction.x;
            d.y = start.y + scale * direction.y;
            d.z = start.z + scale * direction.z;
            dest = d;
        }

#if !VECTOR_NO_SLOW_OPERATIONS
        public static QAngle RandomAngle(float minVal, float maxVal)
        {
            Vector random = new();
            random.Random(minVal, maxVal);
            QAngle ret = new(random.x, random.y, random.z);
            return ret;
        }
#endif // !VECTOR_NO_SLOW_OPERATIONS

        public static bool QAnglesAreEqual(QAngle src1, QAngle src2, float tolerance = 0.0f)
        {
            if (basetypes.FloatMakePositive(src1.x - src2.x) > tolerance)
            {
                return false;
            }

            if (basetypes.FloatMakePositive(src1.y - src2.y) > tolerance)
            {
                return false;
            }

            return (basetypes.FloatMakePositive(src1.z - src2.z) <= tolerance);
        }

        public static void QAngleToAngularImpulse(QAngle angles, Vector impulse)
        {
            impulse.x = angles.z;
            impulse.y = angles.x;
            impulse.z = angles.y;
        }

        public static void AngularImpulseToQAngle(Vector impulse, QAngle angles)
        {
            angles.x = impulse.y;
            angles.y = impulse.z;
            angles.z = impulse.x;
        }

        public static float VectorNormalize(Vector vec)
        {
            float sqrlen = vec.LengthSqr() + 1.0e-10f, invlen;
            invlen = MathF.Sqrt(sqrlen);
            vec.x *= invlen;
            vec.y *= invlen;
            vec.z *= invlen;
            return sqrlen * invlen;
        }
    }
}
