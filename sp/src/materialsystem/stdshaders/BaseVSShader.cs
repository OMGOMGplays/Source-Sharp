using System;

namespace SourceSharp.SP.MaterialSystem.StdShaders;

public class CBaseVSShader : CBaseShader
{
#if X360
    public const bool SUPPORT_DX8 = false;
    public const bool SUPPORT_DX7 = false;
#else
    public const bool SUPPORT_DX8 = true;
    public const bool SUPPORT_DX7 = true;
#endif // X360

    public static void BEGIN_VS_SHADER_FLAGS(string name, int help, int flags) { throw new NotImplementedException(); }
    public static void BEGIN_VS_SHADER(string name, int help) { throw new NotImplementedException(); }

    public void LoadBumpLightmapCoordinateAxes_PixelShader(int pixelReg)
    {

    }

    public void LoadBumpLightmapCoordinateAxes_VertexShader(int vertexReg)
    {

    }

    public void SetPixelShaderConstant(int pixelReg, int constantVar)
    {

    }

    public void SetPixelShaderConstantGammaToLinear(int pixelReg, int constantVar)
    {

    }

    public void SetPixelShaderConstant(int pixelReg, int constantVar, int constantVar2)
    {

    }

    public void SetPixelShaderConstantGammaToLinear(int pixelReg, int constantVar, int constantVar2)
    {

    }

    public void SetVertexShaderConstantGammaToLinear(int @var, float[] vec, int numConst = 1, bool force = false)
    {

    }

    public void SetPixelShaderConstantGammaToLinear(int @var, float[] vec, int numConst = 1, bool force = false)
    {

    }

    public void SetVertexShaderConstant(int vertexReg, int constantVar)
    {

    }

    public void SetPixelShaderConstant_W(int pixelReg, int constantVar, float wValue)
    {

    }

    public void SetPixelShaderConstantFudge(int pixelReg, int constantVar)
    {

    }

    public void SetPixelShaderLightColors(int pixelReg)
    {

    }

    public void SetVertexShaderTextureTranslation(int vertexReg, int translationVar)
    {

    }

    public void SetVertexShaderTextureScale(int vertexReg, int scaleVar)
    {

    }

    public void SetVertexShaderTextureTransform(int vertexReg, int transformVar)
    {

    }

    public void SetVertexShaderTextureScaledTransform(int vertexReg, int transformVar, int scaleVar)
    {

    }


}