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

    }

    public static C_AnimationLayer LoopingLerp(float percent, C_AnimationLayer from, ref C_AnimationLayer to)
    {
        C_AnimationLayer output = new C_AnimationLayer();

        output.sequence = to.sequence;
        output.cycle = Lerp_Functions.LoopingLerp(percent, (float)from.cycle, ref (float)to.cycle);
    }
}