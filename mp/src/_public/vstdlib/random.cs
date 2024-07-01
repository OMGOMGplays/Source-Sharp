#define VSTDLIB_RANDOM_H

namespace SourceSharp.mp.src._public.vstdlib
{
    public class random
    {
        public const int NTAB = 32;

        public class IUniformRandomStream
        {
            public virtual void SetSeed(int iSeed) { }

            public virtual float RandomFloat(float flMinVal = 0.0f, float flMaxVal = 1.0f) { return 0.0f; }
            public virtual int RandomInt(int iMinVal, int iMaxVal) { return 0; }
            public virtual float RandomFloatExp(float flMinVal = 0.0f, float flMaxVal = 1.0f, float flExponent = 1.0f) { return 0.0f; }

            private int GenerateRandomNumber() { return 0; }

            private int m_idum;
            private int m_iy;
            private int[] m_iv = new int[NTAB];

            private CThreadFastMutex m_mutex;
        }

        public class CGaussianRandomStream
        {
            public CGaussianRandomStream(IUniformRandomStream pUniformStream = null) { }

            public void AttachToStream(IUniformRandomStream pUniformStream = null) { }

            public float RandomFloat(float flMean = 0.0f, float flStdDev = 1.0f) { return 0.0f; }

            private IUniformRandomStream m_pUniformStream;
            private bool m_bHaveValue;
            private float m_flRandomValue;

            private CThreadFastMutex m_mutex;
        }

        public static void RandomSeed(int iSeed) { }
        public static float RandomFloat(float flMinVal = 0.0f, float flMaxVal = 1.0f) { return 0.0f; }
        public static float RandomFloatExp(float flMinVal = 0.0f, float flMaxVal = 1.0f, float flExponent = 1.0f) { return 0.0f; }
        public static int RandomInt(int iMinVal, int iMaxVal) { return 0; }
        public static float RandomGaussianFloat(float flMean = 0.0f, float flStdDev = 1.0f) { return 0.0f; }

        public class CDefaultUniformRandomStream : IUniformRandomStream
        {
            public override void SetSeed(int iSeed) { base.SetSeed(iSeed); }
            public override float RandomFloat(float flMinVal = 0, float flMaxVal = 1) { return base.RandomFloat(flMinVal, flMaxVal); }
            public override int RandomInt(int iMinVal, int iMaxVal) { return base.RandomInt(iMinVal, iMaxVal); }
            public override float RandomFloatExp(float flMinVal = 0, float flMaxVal = 1, float flExponent = 1) { return base.RandomFloatExp(flMinVal, flMaxVal, flExponent); }
        }

        public static void InstallUniformRandomStream(IUniformRandomStream pStream) { }
    }
}
