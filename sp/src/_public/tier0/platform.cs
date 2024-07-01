#define PLATFORM_H

namespace SourceSharp.sp.src._public.tier0 // FIXME: Implement properly, so far this is a bunch of unnecessary #define and #if jargon...
{
    //using RTime32 = uint;

    //using float32 = float;
    //using float64 = double;

    public class platform
    {
//#if DEBUG
//#if !PLAT_COMPILE_TIME_ASSERT
//        public static void PLAT_COMPILE_TIME_ASSERT(float pred) { }
//#endif // !PLAT_COMPILE_TIME_ASSERT
//#else
//#if !PLAT_COMPILE_TIME_ASSERT
//        public static void PLAT_COMPILE_TIME_ASSERT(float pred) { }
//#endif // !PLAT_COMPILE_TIME_ASSERT
//#endif // DEBUG

//        public const bool NEW_SOFTWARE_LIGHTING = true;

//#if _RETAIL
//        public static bool IsRetail() => true;
//#else
//        public static bool IsRetail() => false;
//#endif // _RETAIL

//#if DEBUG
//        public static bool IsRelease() => false;
//        public static bool IsDebug() => true;
//#else
//        public static bool IsRelease() => true;
//        public static bool IsDebug() => false;
//#endif // DEBUG

//        public static bool IsXbox() => false;

//#if _WIN32
//        public static bool IsLinux() => false;
//        public static bool IsOSX() => false;
//        public static bool IsPosix() => false;
//        public const bool PLATFORM_WINDOWS = true;
//#if !_X360
//            public static bool IsWindows() => true;
//            public static bool IsPC() => true;
//            public static bool IsConsole => false;
//            public static bool IsX360() => false;
//            public static bool IsPS3() => false;
//            public const bool IS_WINDOWS_PC = true;
//            public const bool PLATFORM_WINDOWS_PC = true;
//#if _WIN64
//                public static bool IsPlatformWindowsPC64() => true;
//                public static bool IsPlatformWindowsPC32() => false;
//                public const bool PLATFORM_WINDOWS_PC64 = true;
//#else
//                public static bool IsPlatformWindowsPC64() => false;
//                public static bool IsPlatformWindowsPC32() => true;
//                public const bool PLATFORM_WINDOWS_PC32 = true;
//#endif // _WIN64
//#else
//            public const bool PLATFORM_X360 = true;
//#if !_CONSOLE
//                public const bool _CONSOLE = true;
//#endif // !_CONSOLE
//            public static bool IsWindows() => false;
//            public static bool IsPC() => false;
//            public static bool IsConsole() => true;
//            public static bool IsX360() => true;
//            public static bool IsPS3() => false;
//#endif // !_x360
//#if DX_TO_GL_ABSTRACTION
//            public static bool IsPlatformOpenGL() => true;
//#else
//            public static bool IsPlatformOpenGL() => false;
//#endif // D_XTO_GL_ABSTRACTION
//#elif POSIX
//        public static bool IsPC() => true;
//        public static bool IsWindows() => false;
//        public static bool IsConsole() => false;
//        public static bool IsX360() => false;
//        public static bool IsPS3() => false;
//#if LINUX
//        public static bool IsLinux() => true;
//#else
//        public static bool IsLinux() => false;
//#endif // LINUX

//#if OSX
//        public static bool IsOSX() => true;
//#else
//        public static bool IsOSX() => false;
//#endif // OSX
        
//        public static bool IsPosix() => true;
//        public static bool IsPlatformOpenGL => true;
//#else
//#error "No suitable OS found!"
//#endif // _WIN32

//#if _WIN64
//        public const bool X64BITS = true;
//#endif // _WIN64

        public const int VALVE_RAND_MAX = 0x7FFF;
    }
}
