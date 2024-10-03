namespace SourceSharp.SP.Utils.CaptionCompiler;

public enum FSInitType
{
    FS_INIT_FULL,
    FS_INIT_COMPATIBILITY_MODE
}

public static class FileSystem_Tools
{
    public static string qdir;
    public static string gamedir;

    public static bool FileSystem_Init(string filename, int maxMemoryUsage = 0, FSInitType initType = FSInitType.FS_INIT_FULL, bool onlyUseFilename = false)
    {

    }

    public static void FileSystem_Term()
    {

    }

    public static void FileSystem_SetupStandardDirectories(string filename, string gameInfoPath)
    {

    }

    public CreateInterfaceFn FileSystem_GetFactory()
    {

    }

    public static IBaseFileSystem fileSystem;
    public static IFileSystem fullFileSystem;
}