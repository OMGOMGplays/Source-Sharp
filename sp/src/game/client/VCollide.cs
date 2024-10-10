namespace SourceSharp.SP.Game.Client;

public struct VCollide
{
    public ushort solidCount = 16;
    public ushort isPacked = 1;
    public ushort descSize;
    public CPhysCollide[] solids;
    public string keyValues;
}