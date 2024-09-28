namespace SourceSharp.SP.Public;

public interface IGameUIFuncs
{
    public const string VENGINE_GAMEUIFUNCS_VERSION = "VENGINE_GAMEUIFUNCS_VERSION005";

    public bool IsKeyDown(string keyname, bool isDown);
    public string GetBindingForButtonCode(ButtonCode code);
    public ButtonCode GetButtonCodeForBind(string bind);
    public void GetVideoModes(VMode[] listStart, int count);
    public void SetFriendsID(uint friendsID, string friendsName);
    public void GetDesktopResolution(out int width, out int height);
    public bool IsConnectedToVACSecureServer();
}