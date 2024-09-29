namespace SourceSharp.SP.Public.Engine;

public enum RenderGroup_Config
{
    RENDER_GROUP_CFG_NUM_OPAQUE_ENT_BUCKETS = 4,
}

public enum RenderGroup
{
    RENDER_GROUP_OPAQUE_STATIC_HUGE = 0,
    RENDER_GROUP_OPAQUE_ENTITY_HUGE = 1,
    RENDER_GROUP_OPAQUE_STATIC = RENDER_GROUP_OPAQUE_STATIC_HUGE + (RenderGroup_Config.RENDER_GROUP_CFG_NUM_OPAQUE_ENT_BUCKETS - 1) * 2,
    RENDER_GROUP_OPAQUE_ENTITY,

    RENDER_GROUP_TRANSLUCENT_ENTITY,
    RENDER_GROUP_TWOPASS,
    RENDER_GROUP_VIEW_MODEL_OPAQUE,
    RENDER_GROUP_VIEW_MODEL_TRANSLUCENT,

    RENDER_GROUP_OPAQUE_BRUSH,

    RENDER_GROUP_OTHER,

    RENDER_GROUP_COUNT
}

public interface IClientLeafSystem
{
    public const string CLIENTLEAFSYSTEM_INTERFACE_VERSION_1 = "ClientLeafSystem001";
    public const string CLIENTLEAFSYSTEM_INTERFACE_VERSION = "ClientLeafSystem002";

    public void CreateRenderableHandle(IClientRenderable renderable, bool isStaticProp = false);
    public void RemoveRenderable(ClientRenderHandle handle);
    public void AddRenderableToLeaves(ClientRenderHandle renderable, int leafCount, ushort[] leaves);
    public void ChangeRenderableRenderGroup(ClientRenderHandle handle, RenderGroup group);
}