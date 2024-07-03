#define CMDLIB_H

global using cblock_t = cmdlib.cblock_s;

public class cmdlib
{
    // cmdlib.h

    public const string TOOLS_READ_PATH_ID = "GAME";

    public struct cblock_s
    {
        public byte[] data;
        public int count;
    }

    // cmdlib.cpp

    public static int myargc;
    public static string myargv;

    public static string com_token;

    public qboolean archive;
    public string archivedir;

    public FileHandle_t g_pLogFile = null;

    public CUtlLinkedList<CleanupFn, ushort> g_CleanupFunction;
    public CUtlLinkedList<SpewHookFn, ushort> g_ExtraSpewHooks;

    public static bool g_bStopOnExit = false;
    public static object g_ExtraSpewHook(string s) => null;

//#if _WIN32 || WIN32
    public void CmdLib_FPrintf(FileHandle_t hFile, string pFormat, params object[] args)
    {
        CUtlVector<char> buf;

        if (buf.Count() == 0)
        {
            buf.SetCount(1024);
        }

        while (/*1*/ true)
        {
            int ret = Q_vsnprintf(buf.Base(), buf.Count(), pFormat, marker);
        }
    }
//#endif // _WIN32 || WIN32
}

