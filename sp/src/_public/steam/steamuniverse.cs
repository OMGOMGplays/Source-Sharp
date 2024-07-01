#define STEAMUNIVERSE_H

namespace SourceSharp.sp.src._public.steam
{
    public class steamuniverse
    {
        public enum EUniverse
        {
            k_EUniverseInvalid = 0,
            k_EUniversePublic = 1,
            k_EUniverseBeta = 2,
            k_EUniverseInternal = 3,
            k_EUniverseDev = 4,
            // k_EUniverseRC = 5,				// no such universe anymore
            k_EUniverseMax
        }
    }
}
