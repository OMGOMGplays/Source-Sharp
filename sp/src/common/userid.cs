#define USERID_H

namespace SourceSharp.sp.src.common
{
    public class userid
    {
        public const int IDTYPE_WON = 0;
        public const int IDTYPE_STEAM = 1;
        public const int IDTYPE_VALVE = 2;
        public const int IDTYPE_HLTV = 3;
        public const int IDTYPE_REPLAY = 4;

        public struct USERID_s
        {
            public int idtype;
            steamclientpublic.CSteamID steamid;
        }

        public USERID_s USERID_t;
    }
}
