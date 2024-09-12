using SourceSharp.SP.Public.Steam;

namespace SourceSharp.SP.Common;

public class UserID
{
    public const int IDTYPE_WON = 0;
    public const int IDTYPE_STEAM = 1;
    public const int IDTYPE_VALVE = 2;
    public const int IDTYPE_HLTV = 3;
    public const int IDTYPE_REPLAY = 4;

    public struct USERID
    {
        public int idtype;
        public CSteamID steamid;
    }
}