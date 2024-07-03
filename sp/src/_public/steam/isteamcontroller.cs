#define ISTEAMCONTROLLER_H

namespace SourceSharp.sp.src._public.steam
{
    public class isteamcontroller
    {
        public const int MAX_STEAM_CONTROLLERS = 16;

        public enum ESteamControllerPad
        {
            k_ESteamControllerPad_Left,
            k_ESteamControllerPad_Right,
        }

        public class ISteamController
        {
            public virtual bool Init(string pchAbsolutePathToControllerConfigVDF) => false;
            public virtual bool Shutdown() => false;

            public virtual void RunFrame() { }

            public virtual bool GetControllerState(uint unControllerIndex, SteamControllerState_t pState) => false;

            public virtual void TriggerHapticPulse(uint unControllerIndex, ESteamControllerPad eTargetPad, ushort usDurationMicroSec) { }

            public virtual void SetOverrideMode(string pchMode) { }
        }

        public const string STEAMCONTROLLER_INTERFACE_VERSION = "STEAMCONTROLLER_INTERFACE_VERSION";
    }
}
