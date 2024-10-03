using SourceSharp.SP.Mathlib;

namespace SourceSharp.SP.Game.Client;

public class C_AnimationLayer
{
    public CRangeCheckedVar<int> sequence = new CRangeCheckedVar<int>(-1, 65535, 0);
    public CRangeCheckedVar<float> prevCycle = new CRangeCheckedVar<float>(-2, 2, 0);
    public CRangeCheckedVar<float> weight = new CRangeCheckedVar<float>(-5, 5, 0);
    public int order;

    public CRangeCheckedVar<float> playbackRate = new CRangeCheckedVar<float>(-50, 50, 0);
    public CRangeCheckedVar<float> cycle = new CRangeCheckedVar<float>(-2, 2, 0);

    public float layerAnimTime;
    public float layerFadeOuttime;

    public float blendIn;
    public float blendOut;

    public bool clientBlend;

    public C_AnimationLayer()
    {
        Reset();
    }

    public void Reset()
    {
        sequence = 0;
        prevCycle = 0;
        weight = 0;
        playbackRate = 0;
        cycle = 0;
        layerAnimTime = 0;
        layerFadeOuttime = 0;
        blendIn = 0;
        blendOut = 0;
        clientBlend = false;
    }

    public void SetOrder(int order)
    {
        this.order = order;
    }

    public bool IsActive()
    {
        return order != C_BaseAnimationOverlay.MAX_OVERLAYS;
    }

    public float GetFadeout(float curTime)
    {
        float s;

        if (layerFadeOuttime <= 0.0f)
        {
            s = 0;
        }
        else
        {
            s = 1.0f - (curTime - layerAnimTime) / layerFadeOuttime;

            if (s > 0 && s <= 1.0f)
            {
                s = 3 * s * s - 2 * s * s * s;
            }
            else if (s > 1.0f)
            {
                s = 1.0f;
            }
        }

        return s;
    }

    public void BlendWeight()
    {
        if (!clientBlend)
        {
            return;
        }

        weight = 1;

        if (blendIn != 0.0f)
        {
            if (cycle < blendIn)
            {
                weight = cycle / blendIn;
            }
        }

        if (blendOut != 0.0f)
        {
            if (cycle > 1.0f - blendOut)
            {
                weight = (1.0f - cycle) / blendOut;
            }
        }

        weight = 3.0f * weight * weight - 2.0f * weight * weight * weight;

        if (sequence == 0)
        {
            weight = 0;
        }
    }

    public static C_AnimationLayer LoopingLerp(float percent, C_AnimationLayer from, C_AnimationLayer to)
    {
        C_AnimationLayer output = new C_AnimationLayer();

        output.sequence = to.sequence;
        output.cycle = Lerp_Functions.LoopingLerp(percent, (float)from.cycle, ref (float)to.cycle);
        output.prevCycle = to.prevCycle;
        output.weight = Mathlib.Mathlib.Lerp(percent, from.weight, to.weight);
        output.order = to.order;

        output.layerAnimTime = to.layerAnimTime;
        output.layerFadeOuttime = to.layerFadeOuttime;
        return output;
    }

    public static C_AnimationLayer Lerp(float percent, C_AnimationLayer from, C_AnimationLayer to)
    {
        C_AnimationLayer output = new C_AnimationLayer();

        output.sequence = to.sequence;
        output.cycle = Mathlib.Mathlib.Lerp(percent, from.cycle, to.cycle);
        output.prevCycle = to.prevCycle;
        output.weight = Mathlib.Mathlib.Lerp(percent, from.weight, to.weight);
        output.order = to.order;

        output.layerAnimTime = to.layerAnimTime;
        output.layerFadeOuttime = to.layerFadeOuttime;
        return output;
    }

    public static C_AnimationLayer LoopingLerp_Hermite(float percent, C_AnimationLayer prev, C_AnimationLayer from, C_AnimationLayer to)
    {
        C_AnimationLayer output = new C_AnimationLayer();

        output.sequence = to.sequence;
        output.cycle = Lerp_Functions.LoopingLerp_Hermite(percent, prev.cycle, from.cycle, to.cycle);
        output.prevCycle = to.prevCycle;
        output.weight = Mathlib.Mathlib.Lerp(percent, from.weight, to.weight);
        output.order = to.order;

        output.layerAnimTime = to.layerAnimTime;
        output.layerFadeOuttime = to.layerFadeOuttime;
        return output;
    }

    public static C_AnimationLayer Lerp_Hermite(float percent, C_AnimationLayer prev, C_AnimationLayer from, C_AnimationLayer to)
    {
        C_AnimationLayer output = new C_AnimationLayer();

        output.sequence = to.sequence;
        output.cycle = Lerp_Functions.Lerp_Hermite(percent, prev.cycle, from.cycle, to.cycle);
        output.prevCycle = to.prevCycle;
        output.weight = Mathlib.Mathlib.Lerp(percent, from.weight, to.weight);
        output.order = to.order;

        output.layerAnimTime = to.layerAnimTime;
        output.layerFadeOuttime = to.layerFadeOuttime;
        return output;
    }

    public static void Lerp_Clamp(ref C_AnimationLayer val)
    {
        Lerp_Functions.Lerp_Clamp(ref val.sequence);
        Lerp_Functions.Lerp_Clamp(ref val.cycle);
        Lerp_Functions.Lerp_Clamp(ref val.prevCycle);
        Lerp_Functions.Lerp_Clamp(ref val.weight);
        Lerp_Functions.Lerp_Clamp(ref val.order);
        Lerp_Functions.Lerp_Clamp(ref val.layerAnimTime);
        Lerp_Functions.Lerp_Clamp(ref val.layerFadeOuttime);
    }
}