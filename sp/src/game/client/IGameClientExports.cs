namespace SourceSharp.SP.Public.Game.Client;

public interface IGameClientExports : IBaseInterface
{
    public const string GAMECLIENTEXPORTS_INTERFACE_VERSION = "GameClientExports001";

#if !X360
    public bool IsPlayerGameVoiceMuted(int playerIndex);
    public void MutePlayerGameVoice(int playerIndex);
    public void UnmutePlayerGameVoice(int playerIndex);

    public void OnGameUIActivated();
    public void OnGameUIHidden();
#endif // !X360

    public void CreateAchievementsPanel(Vgui.Panel parent);
    public void DisplayAchievementPanel();
    public void ShutdownAchievementPanel();
    public int GetAchievementsPanelMinWidth();

    public string GetHolidayString();
}