namespace SourceSharp.SP.Public.Game.Client;

public interface IClientRenderTargets
{
    public void InitClientRenderTargets(IMaterialSystem materialSystem, IMaterialSystemHardwareConfig hardwareConfig);

    public void ShutdownClientRenderTargets();
}