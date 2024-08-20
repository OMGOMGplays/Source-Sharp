namespace Mathlib;

public class HaltonSequenceGenerator_t
{
    private int seed;
    private int ibase;
    private float fbase;

    public HaltonSequenceGenerator_t(int ibase)
    {

    }
    
    public float GetElement(int element)
    {

    }

    public float NextValue()
    {
        return GetElement(seed++);
    }
}

public class DirectionalSampler_t
{
    private HaltonSequenceGenerator_t zdot;
    private HaltonSequenceGenerator_t vrot;

    public DirectionalSampler_t()
    {
        zdot = new(2);
        vrot = new(3);
    }

    public Vector NextValue()
    {
        float zvalue = zdot.NextValue();
        zvalue = 2 * zvalue - 1.0f;
        float phi = corecrt_math.acos(zvalue);
        float theta = 2.0f * basetypes.M_PI * vrot.NextValue();
        float sin_p = corecrt_math.sin(phi);

        return new Vector(corecrt_math.cos(theta) * sin_p,
                          corecrt_math.sin(theta) * sin_p,
                          zvalue);
    }
}