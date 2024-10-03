using System;
using System.IO;

namespace SourceSharp.SP.Utils.CaptionCompiler;

public struct AnalysisData
{
    public CUtlSymbolTable symbols;
}

public static class CaptionCompiler
{
    public static bool useLogFile = false;
    public static bool x360 = false;

    public static AnalysisData analysis;

    public static IFileSystem filesystem = null;

    public static bool spewed = false;

    public static SpewRetval SpewFunc(SpewType type, string msg)
    {
        spewed = true;

        Console.Write(msg);

        if (type == SpewType.SPEW_ERROR)
        {
            Console.Write("\n");
        }

        return SpewRetval.SPEW_CONTINUE;
    }

    public static void vprint(int depth, string fmt, params object[] args)
    {
        throw new NotImplementedException();
    }
        

}