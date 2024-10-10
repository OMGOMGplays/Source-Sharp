using SourceSharp.SP.Mathlib;
using SourceSharp.SP.Tier1;
using System;

namespace SourceSharp.SP.Game.Client;

public interface IModelLoadCallback
{
    public void OnModelLoadComplete(Model model);
}

public class CRefCountedModelIndex
{
    private int index;

    public CRefCountedModelIndex()
    {
        index = -1;
    }

    ~CRefCountedModelIndex()
    {
        Set(-1);
    }

    public CRefCountedModelIndex(CRefCountedModelIndex src)
    {
        Set(src.index);
    }

    public CRefCountedModelIndex(int i)
    {
        Set(i);
    }

    public int Get()
    {
        return index;
    }

    public void Set(int i)
    {
        if (i == index)
        {
            return;
        }

        CDLL_Client_Int.modelInfo.AddRefDynamicModel(i);
        CDLL_Client_Int.modelInfo.ReleaseDynamicModel(index);
        index = i;
    }

    public void Clear()
    {
        Set(-1);
    }

    public static explicit operator int(CRefCountedModelIndex src)
    {
        return src.index;
    }
}

public interface IVModelInfo
{
    public const string VMODELINFO_CLIENT_INTERFACE_VERSION = "VModelInfoClient006";
    public const string VMODELINFO_SERVER_INTERFACE_VERSION_3 = "VModelInfoServer003";
    public const string VMODELINFO_SERVER_INTERFACE_VERSION = "VModelInfoServer004";

    public Model GetModel(int modelindex);

    public int GetModelIndex(string name);

    public string GetModelName(Model model);
    public VCollide GetVCollide(Model model);
    public VCollide GetVCollide(int modelindex);
    public void GetModelBounds(Model model, out Vector mins, out Vector maxs);
    public void GetModelRenderBounds(Model model, out Vector mins, out Vector maxs);
    public int GetModelFrameCount(Model model);
    public int GetModelType(Model model);
    public object GetModelExtraData(Model model);
    public bool ModelHasMaterialProxy(Model model);
    public bool IsTranslucent(Model model);
    public bool IsTranslucentTwoPass(Model model);
    public void RecomputeTranslucency(Model model, int skin, int body, object clientRenderable, float instanceAlphaModulate = 1.0f);
    public int GetModelMaterialCount(Model model);
    public void GetModelMaterials(Model model, int count, out IMaterial[] materials);
    public bool IsModelVertexLit(Model model);
    public string GetModelKeyValueText(Model model);
    public bool GetModelKeyValue(Model model, CUtlBuffer buf);
    public float GetModelRadius(Model model);

    public StudioHdr FindModel(StudioHdr studioHdr, object[] cache, string modelname);
    public StudioHdr FindModel(object[] cache);
    public VirtualModel GetVirtualModel(StudioHdr studioHdr);
    public byte GetAnimBlock(StudioHdr studioHdr, int block);

    public void GetModelMaterialColorAndLighting(Model model, Vector origin, QAngle angles, Trace trace, Vector lighting, Vector matColor);
    public void GetIlluminationPoint(Model model, IClientRenderable renderable, Vector origin, QAngle angles, Vector lightingCenter);

    public int GetModelContents(int modelIndex);
    public StudioHdr GetStudioModel(out Model model);
    public int GetModelSpriteWidth(Model model);
    public int GetModelSpriteHeight(Model model);

    public void SetLevelScreenFadeRange(float minSize, float maxSize);
    public void GetLevelScreenFadeRange(out float minSize, out float maxSize);

    public void SetViewScreenFadeRange(float minSize, float maxSize);

    public byte ComputeLevelScreenFade(Vector absOrigin, float radius, float fadeScale);
    public byte ComputeViewScreenFade(Vector absOrigin, float radius, float fadeScale);

    public int GetAutoplayList(StudioHdr studioHdr, out ushort[] autoplayList);

    public CPhysCollide GetCollideForVirtualTerrain(int index);

    public bool IsUsingFBTexture(Model model, int skin, int body, object clientRenderable);

    [Obsolete] public Model FindOrLoadModel(string name) { Dbg.Warning("IVModelInfo::FindOrLoadModel is now obsolete.\n"); return null; }
    [Obsolete] public void InitDynamicModels() { Dbg.Warning("IVModelInfo::InitDynamicModels is now obsolete.\n"); }
    [Obsolete] public void ShutdownDynamicModels() { Dbg.Warning("IVModelInfo::ShutdownDynamicModels is now obsolete.\n"); }
    [Obsolete] public void AddDynamicModel(string name, int modelindex = -1) { Dbg.Warning("IVModelInfo::AddDynamicModel is now obsolete.\n"); }
    [Obsolete] public void ReferenceModel(int modelindex) { Dbg.Warning("IVModelInfo::ReferenceModel is now obsolete.\n"); }
    [Obsolete] public void UnreferenceModel(int modelindex) { Dbg.Warning("IVModelInfo::UnreferenceModel is now obsolete.\n"); }
    [Obsolete] public void CleanupDynamicModels(bool force = false) { Dbg.Warning("IVModelInfo::CleanupDynamicModels is now obsolete.\n"); }

    public MDLHandle GetCacheHandle(Model model);

    public int GetBrushModelPlaneCount(Model model);
    public void GetBrushModelPlane(Model model, int index, CPlane plane, Vector origin);
    public int GetSurfacepropsForVirtualTerrain(int index);

    public void OnLevelChange();

    public int GetModelClientSideIndex(string name);

    public int RegisterDynamicModel(string name, bool clientSide);

    public bool IsDynamicModelLoading(int modelindex);

    public void AddRefDynamicModel(int modelindex);
    public void ReleaseDynamicModel(int modelindex);

    public bool RegisterModelLoadCallback(int modelindex, ref IModelLoadCallback callback, bool callImmediatelyIfLoaded = true);
    public void UnregisterModelLoadCallback(int modelindex, ref IModelLoadCallback callback);
}

public interface IVModelInfoClient : IVModelInfo
{
    public void OnDynamicModelsStringTableChange(int stringIndex, string @string, object data);

    //public Model FindOrLoadModel(string name);
}

public struct VirtualTerrainParams
{
    public int index;
}