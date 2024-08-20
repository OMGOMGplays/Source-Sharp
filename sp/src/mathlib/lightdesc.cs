namespace Mathlib;

public enum LightType_t
{
    MATERIAL_LIGHT_DISABLE = 0,
    MATERIAL_LIGHT_POINT,
    MATERIAL_LIGHT_DIRECTIONAL,
    MATERIAL_LIGHT_SPOT,
}

public enum LightType_OptimizationFlags_t
{
    LIGHTTYPE_OPTIMIZATIONFLAGS_HAS_ATTENUATION0 = 1,
    LIGHTTYPE_OPTIMIZATIONFLAGS_HAS_ATTENUATION1 = 2,
    LIGHTTYPE_OPTIMIZATIONFLAGS_HAS_ATTENUATION2 = 4,
    LIGHTTYPE_OPTIMIZATIONFLAGS_DERIVED_VALUES_CALCED = 8,
}

public struct LightDesc_t
{
    private LightType_t m_Type;
    private Vector m_Color;
    private Vector m_Position;
    private Vector m_Direction;
    private float m_Range;
    private float m_Falloff;
    private float m_Attenuation0;
    private float m_Attenuation1;
    private float m_Attenuation2;
    private float m_Theta;

    private float m_Phi;

    private float m_ThetaDot;
    private float m_PhiDot;
    private uint m_Flags;

    private float OneOver_ThetaDot_Minus_PhiDot;
    private float m_RangeSquared;

    public void RecalculateDerivedValues()
    {
        m_Flags = (int)LightType_OptimizationFlags_t.LIGHTTYPE_OPTIMIZATIONFLAGS_DERIVED_VALUES_CALCED;

        if (m_Attenuation0 != 0)
        {
            m_Flags |= (int)LightType_OptimizationFlags_t.LIGHTTYPE_OPTIMIZATIONFLAGS_HAS_ATTENUATION0;
        }

        if (m_Attenuation1 != 0)
        {
            m_Flags |= (int)LightType_OptimizationFlags_t.LIGHTTYPE_OPTIMIZATIONFLAGS_HAS_ATTENUATION1;
        }

        if (m_Attenuation2 != 0)
        {
            m_Flags |= (int)LightType_OptimizationFlags_t.LIGHTTYPE_OPTIMIZATIONFLAGS_HAS_ATTENUATION2;
        }

        if (m_Type == LightType_t.MATERIAL_LIGHT_SPOT)
        {
            m_ThetaDot = corecrt_math.cos(m_Theta);
            m_PhiDot = corecrt_math.cos(m_Phi);

            float spread = m_ThetaDot - m_PhiDot;

            if (spread > 1.0e-10)
            {
                OneOver_ThetaDot_Minus_PhiDot = 1.0f / spread;
            }
            else
            {
                OneOver_ThetaDot_Minus_PhiDot = 1.0f;
            }
        }

        if (m_Type == LightType_t.MATERIAL_LIGHT_DIRECTIONAL)
        {
            m_Position = m_Direction;
            m_Position *= 2.0e6;
        }

        m_RangeSquared = m_Range * m_Range;
    }

    public LightDesc_t()
    {

    }

    public LightDesc_t(Vector pos, Vector color)
    {
        InitPoint(pos, color);
    }

    public LightDesc_t(Vector pos, Vector color, Vector point_at,
        float inner_cone_boundary, float outer_cone_boundary)
    {
        InitSpot(pos, color, point_at, inner_cone_boundary, outer_cone_boundary);
    }

    public void InitPoint(Vector pos, Vector color)
    {
        m_Type = LightType_t.MATERIAL_LIGHT_POINT;
        m_Color = color;
        m_Position = pos;
        m_Range = 0.0f;
        m_Attenuation0 = 1.0f;
        m_Attenuation1 = 0.0f;
        m_Attenuation2 = 0.0f;
        RecalculateDerivedValues();
    }

    public void InitDirectional(Vector dir, Vector color)
    {
        m_Type = LightType_t.MATERIAL_LIGHT_DIRECTIONAL;
        m_Color = color;
        m_Direction = dir;
        m_Range = 0.0f;
        m_Attenuation0 = 1.0f;
        m_Attenuation1 = 0.0f;
        m_Attenuation2 = 0.0f;
        RecalculateDerivedValues();
    }

    public void InitSpot(Vector pos, Vector color, Vector point_at,
        float inner_cone_boundary, float outer_cone_boundary)
    {
        m_Type = LightType_t.MATERIAL_LIGHT_SPOT;
        m_Color = color;
        m_Position = pos;
        m_Direction = point_at;
        m_Direction -= pos;
        vector.VectorNormalizeFast(m_Direction);
        m_Falloff = 5.0f;
        m_Theta = inner_cone_boundary;
        m_Phi = outer_cone_boundary;

        m_Range = 0.0f;

        m_Attenuation0 = 1.0f;
        m_Attenuation1 = 0.0f;
        m_Attenuation2 = 0.0f;
        RecalculateDerivedValues();
    }

    public void ComputeLightAtPoints(FourVectors pos, FourVectors normal,
                                     FourVector color, bool doHalfLambert = false)
    {
        FourVectors delta;
        Debug.Assert((m_Type == LightType_t.MATERIAL_LIGHT_POINT) || (m_Type == LightType_t.MATERIAL_LIGHT_SPOT) || (m_Type == LightType_t.MATERIAL_LIGHT_DIRECTIONAL));

        switch (m_Type)
        {
            case LightType_t.MATERIAL_LIGHT_POINT:
            case LightType_t.MATERIAL_LIGHT_SPOT:
                delta.DuplicateVector(m_Position);
                delta -= pos;
                break;

            case LightType_t.MATERIAL_LIGHT_DIRECTIONAL:
                ComputeLightAtPointsForDirectional(pos, normal, color, doHalfLambert);
                return;
        }

        long dist2 = delta * delta;

        dist2 = ssemath.MaxSIMD(sseconst.Four_Ones, dist2);

        long falloff;

        if ((m_Flags & (int)LightType_OptimizationFlags_t.LIGHTTYPE_OPTIMIZATIONFLAGS_HAS_ATTENUATION0) != 0)
        {
            falloff = ssemath.ReplicateX4(m_Attenuation0);
        }
        else
        {
            falloff = sseconst.Four_Epsilons;
        }

        if ((m_Flags & (int)LightType_OptimizationFlags_t.LIGHTTYPE_OPTIMIZATIONFLAGS_HAS_ATTENUATION1) != 0)
        {
            falloff = ssemath.AddSIMD(falloff, ssemath.MulSIMD(ssemath.ReplicateX4(m_Attenuation1), ssemath.SqrtEstSIMD(dist2)));
        }

        if ((m_Flags & (int)LightType_OptimizationFlags_t.LIGHTTYPE_OPTIMIZATIONFLAGS_HAS_ATTENUATION2) != 0)
        {
            falloff = ssemath.AddSIMD(falloff, ssemath.MulSIMD(ssemath.ReplicateX4(m_Attenuation2), dist2));
        }

        falloff = ssemath.ReciprocalEstSIMD(falloff);

        if (m_Range != 0.0f)
        {
            long RangeSquared = ssemath.ReplicateX4(m_RangeSquared);
            falloff = ssemath.AddSIMD(falloff, ssemath.CmpLtSIMD(dist2, RangeSquared));
        }

        delta.VectorNormalizeFast();
        long strength = delta * normal;

        if (doHalfLambert)
        {
            strength = ssemath.AddSIMD(ssemath.MulSIMD(strength, sseconst.Four_PointFives), sseconst.Four_PointFives);
        }
        else
        {
            strength = ssemath.MaxSIMD(sseconst.Four_Zeros, delta * normal);
        }

        switch (m_Type)
        {

        }
    }

    public void ComputeNonincidenceLightAtPoints(FourVectors pos, FourVectors color)
    {

    }

    public void ComputeLightAtPointsForDirectional(FourVectors pos,
                                                   FourVectors normal,
                                                   FourVectors color, bool doHalfLambert = false)
    {
        FourVectors delta;
        delta.DuplicateVector(m_Direction);
        long strength = delta * normal;

        if (doHalfLambert)
        {
            strength = ssemath.AddSIMD(ssemath.MulSIMD(strength, sseconst.FourPointFives), sseconst.FourPointFives);
        }
        else
        {
            strength = ssemath.MaxSIMD(sseconst.Four_Zeroes, delta * normal);
        }

        color.x = ssemath.AddSIMD(color.x, ssemath.MulSIMD(strength, ssemath.ReplicateX4(m_Color.x)));
        color.y = ssemath.AddSIMD(color.y, ssemath.MulSIMD(strength, ssemath.ReplicateX4(m_Color.y)));
        color.z = ssemath.AddSIMD(color.z, ssemath.MulSIMD(strength, ssemath.ReplicateX4(m_Color.z)));
    }

    public void SetupOldStyleAttenuation(float fQuadraticAttn, float fLinearAttn, float fConstantAttn)
    {

    }

    public void SetupNewStyleAttenuation(float fFiftyPercentDistance, float fZeroPercentDistance)
    {

    }

    public bool IsDirectionWithinLightCone(Vector rdir)
    {
        return (m_Type != LightType_t.MATERIAL_LIGHT_SPOT) || (rdir.Dot(m_Direction) >= m_PhiDot);
    }

    public float OneOverThetaDotMinusPhiDot()
    {
        return OneOver_ThetaDot_Minus_PhiDot;
    }
}