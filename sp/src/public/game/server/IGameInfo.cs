namespace SourceSharp.SP.Public.Game.Server;

public interface IGameInfo
{
    public int GetInfo_GameType();
    public string GetInfo_GameTypeName();
    public string GetInfo_GetTeamName(int teamNumber);
    public int GetInfo_GetTeamCount();
    public int GetInfo_NumPlayersOnTeam(int teamNumber);

    public bool GetInfo_Custom(int valueType, out PluginVariant outValue, PluginVariant options);
}

public interface IGameInfoManager
{
    public const string INTERFACEVERSION_GAMEINFOMANAGER = "GameInfoManager001";

    public IGameInfo GetGameInfo();
}