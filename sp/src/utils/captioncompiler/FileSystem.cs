using SourceSharp.SP.Tier1;
using System;

namespace SourceSharp.SP.Utils.CaptionCompiler;

public enum PureServerFileClass
{
    PureServerFileClass_Unknown = -1,
    PureServerFileClass_Any = 0,
    PureServerFileClass_AnyTrusted,
    PureServerFileClass_CheckHash
}

public interface IPureServerWhitelist
{
    public void AddRef();
    public void Release();

    public PureServerFileClass GetFileClass(string filename);

    public int GetTrustedKeyCount();
    public byte GetTrustedKey(int keyIndex, int keySize);
}

public enum FileSystemSeek
{
    FILESYSTEM_SEEK_HEAD = 0, // SEEK_SET
    FILESYSTEM_SEEK_CURRENT = 1, // SEEK_CUR
    FILESYSTEM_SEEK_TAIL = 2 // SEEK_TAIL
}

public enum FileSystemInvalidHandle
{
    FILESYSTEM_INVALID_FIND_HANDLE = -1
}

public enum FileWarningLevel
{
    // A problem!
    FILESYSTEM_WARNING = -1,

    // Don't print anything
    FILESYSTEM_WARNING_QUIET = 0,

    // On shutdown, report names of files left unclosed
    FILESYSTEM_WARNING_REPORTUNCLOSED,

    // Report number of times a file was opened, closed
    FILESYSTEM_WARNING_REPORTUSAGE,

    // Report all open/close events to console (!slow!)
    FILESYSTEM_WARNING_REPORTALLACCESSES,

    // Report all open/close/read events to the console (!slower!)
    FILESYSTEM_WARNING_REPORTALLACCESSES_READ,

    // Report all open/close/read/write events to the console (!slower!)
    FILESYSTEM_WARNING_REPORTALLACCESSES_READWRITE,

    // Report all open/close/read/write events and all async I/O file events to the console (!slower(est)!)
    FILESYSTEM_WARNING_REPORTALLACCESSES_ASYNC
}

// Search path filtering
public enum PathTypeFilter
{
    FILTER_NONE = 0, // No filtering, all search path types match
    FILTER_CULLPACK = 1, // Pack based search paths are culled (maps and zips)
    FILTER_CULLNONPACK = 2 // Non-pack based search paths are culled
}

// Search path querying (bit flags)
public enum PathTypeQuery
{
    PATH_IS_NORMAL = 0x00, // Normal path, not pack based
    PATH_IS_PACKFILE = 0x01, // Path is a pack file
    PATH_IS_MAPPACKFILE = 0x02, // Path is a map pack file
    PATH_IS_REMOTE = 0x04 // Path is the remote filesystem
}

public enum DVDMode
{
    DVDMODE_OFF = 0, // Not using DVD
    DVDMODE_STRICT = 1, // DVD device only
    DVDMODE_DEV = 2 // Dev mode, multiple devices ok
}

#if DEBUG
public enum FileBlockingWarning
{
    // Report how long synchronous I/O took to complete
    FILESYSTEM_BLOCKING_SYNCHRONOUS = 0,
    // Report how long async I/O took to complete if AsyncFileFinished caused it to load via "blocking" I/O
    FILESYSTEM_BLOCKING_ASYNCHRONOUS_BLOCK,
    // Report how long async I/O took to complete
    FILESYSTEM_BLOCKING_ASYNCHRONOUS,
    // Report how long the async "callback" took
    FILESYSTEM_BLOCKING_CALLBACKTIMING,

    FILESYSTEM_BLOCKING_NUMBITS
}

public class FileBlockingItem
{
    public FileBlockingWarning itemType;
    public float elapsed;
    public Access accessType;

    private string filename;

    public enum Access
    {
        FB_ACCESS_OPEN = 1,
        FB_ACCESS_CLOSE = 2,
        FB_ACCESS_READ = 3,
        FB_ACCESS_WRITE = 4,
        FB_ACCESS_APPEND = 5,
        FB_ACCESS_SIZE = 6
    }

    public FileBlockingItem()
    {
        itemType = 0;
        elapsed = 0.0f;
        accessType = 0;

        SetFileName(null);
    }

    public FileBlockingItem(FileBlockingWarning type, string filename, float elapsed, Access accessType)
    {
        itemType = type;
        this.elapsed = elapsed;
        this.accessType = accessType;

        SetFileName(filename);
    }

    public void SetFileName(string filename)
    {
        if (filename == null)
        {
            this.filename = null;
            return;
        }

        this.filename = filename;
    }

    public string GetFileName()
    {
        return filename;
    }
}

public interface IBlockingFileItemList
{
    public void LockMutex();
    public void UnlockMutex();

    public int First();
    public int Next(int i);
    public int InvalidIndex();

    public FileBlockingItem Get(int index);

    public void Reset();
}
#endif // DEBUG

public enum FilesystemMountRetval
{
    FILESYSTEM_MOUNT_OK = 0,
    FILESYSTEM_MOUNT_FAILED
}

public enum SearchPathAdd
{
    PATH_ADD_TO_HEAD, // First path searched
    PATH_ADD_TO_TAIL // Last path searched
}

public enum FileSystemOpenExFlags
{
    FSOPEN_UNBUFFERED = 1 << 0,
    FSOPEN_FORCE_TRACK_CRC = 1 << 1, // This makes it calculate a CRC for the file (if the file came from disk) regardless
                                     // of the IFileList passed to RegisterWhitelist.
    FSOPEN_NEVERINPACK = 1 << 2 // 360 only, hint to FS that file is not allowed to be in pack file
}

/// <summary>
/// Structure used by the interfaces
/// </summary>
public struct FileSystemStatistics
{
    public CInterlockedUInt reads, writes, bytesRead, bytesWritten, seeks;
}

/// <summary>
/// Async file status
/// </summary>
public enum FSAsyncStatus
{
    FSASYNC_ERR_NOT_MINE = -8, // Filename not part of the specified file system, try a different one. (Used internally to find the right filesystem)
    FSASYNC_ERR_RETRY_LATER = -7, // Failure for a reason that might be temporary. You might retry, but not immediately. (E.g. network problems)
    FSASYNC_ERR_ALIGNMENT = -6, // Read parameters invalid for unbuffered I/O
    FSASYNC_ERR_FAILURE = -5, // Hard subsystem failure
    FSASYNC_ERR_READING = -4, // Read error on file
    FSASYNC_ERR_NOMEMORY = -3, // Out of memory for file read
    FSASYNC_ERR_UNKNOWNID = -2, // Caller's provided ID is not recognized
    FSASYNC_ERR_FILEOPEN = -1, // Filename could not be opened (bad path, not exist, etc.)
    FSASYNC_OK = 0, // Operation is successful
    FSASYNC_STATUS_PENDING, // File is properly queued, waiting for service
    FSASYNC_STATUS_INPROGRESS, // File is being accessed
    FSASYNC_STATUS_ABORTED, // File was aborted by caller
    FSASYNC_STATUS_UNSERVICED // File is not yet queued
}

/// <summary>
/// Async request flags
/// </summary>
public enum FSAsyncFlags
{
    FSASYNC_FLAGS_ALLOCNOFREE = 1 << 0, // Do the allocation for dataPtr, but don't free
    FSASYNC_FLAGS_FREEDATAPTR = 1 << 1, // Free the memory for the dataPtr post callback
    FSASYNC_FLAGS_SYNC = 1 << 2, // Actually perform the operation synchronously. Used to simplify client code paths
    FSASYNC_FLAGS_NULLTERMINATE = 1 << 3 // Allocate an extra byte and null terminate the buffer read in
}

/// <summary>
/// Return value for CheckFileCRC.
/// </summary>
public enum FileCRCStatus
{
    FileCRCStatus_CantOpenFile, // We don't have this file
    FileCRCStatus_GotCRC,
    FileCRCStatus_FileInVPK
}

public enum CacheCRCType
{
    CacheCRCType_SingleFile,
    CacheCRCType_Directory,
    CacheCRCType_Directory_Recursive
}

/// <summary>
/// Description of an async request
/// </summary>
public struct FileAsyncRequest
{
    public const FSAsyncFile FS_INVALID_ASYNC_FILE = (FSAsyncFile)0x0000ffff;

    public string filename;
    public object data;
    public int offset;
    public int bytes;
    public FSAsyncCallbackFunc callback;
    public object context;
    public int priority;
    public uint flags;
    public string pathID;
    public FSAsyncFile specificAsyncFile;
    public FSAllocFunc alloc;

    public FileAsyncRequest()
    {
        specificAsyncFile = FS_INVALID_ASYNC_FILE;
    }
}

public struct FileHash
{
    public FileHashType fileHashType;
    public CRC32 ioSequence;
    public MD5Value contents;
    public int fileLen;
    public int packFileID;
    public int packFileNumber;

    public enum FileHashType
    {
        FileHashTypeUnknown = 0,
        FileHashTypeEntireFile = 1,
        FileHashTypeIncompleteFile = 2
    }

    public FileHash()
    {
        fileHashType = FileHashType.FileHashTypeUnknown;
        fileLen = 0;
        packFileID = 0;
        packFileNumber = 0;
    }

    public static bool operator ==(FileHash lhs, FileHash rhs)
    {
        return lhs.ioSequence == rhs.ioSequence && lhs.contents == rhs.contents && lhs.fileHashType == rhs.fileHashType;
    }

    public static bool operator !=(FileHash lhs, FileHash rhs)
    {
        return !(lhs == rhs);
    }
}

public class CUnverifiedFileHash
{
    public string pathID;
    public string filename;
    public int fileFraction;
    public FileHash fileHash;
}

public class CUnverifiedCRCFile
{
    public string pathID;
    public string filename;
    public CRC32 crc;
}

public class CUnverifiedMD5File
{
    public string pathID;
    public string filename;
    public byte[] bits = new byte[MD5Value.MD5_DIGEST_LENGTH];
}

public interface IAsyncFileFetch
{
    /// <summary>
    /// Initiate a request. Returns error status, or on success
    /// returns an opaque handle used to terminate the job<br/>
    /// <br/>
    /// Should return <see cref="FSAsyncStatus.FSASYNC_ERR_NOT_MINE"/> if the filename isn't
    /// handled by this interface<br/>
    /// <br/>
    /// The callback is required, and is the only mechanism to communicate
    /// status. (No polling.) The request is automatically destroyed anytime
    /// after the callback is executed.
    /// </summary>
    public FSAsyncStatus Start(FileAsyncRequest request, out object outHandle, IThreadPool threadPool);

    /// <summary>
    /// Attempt to complete any active work, returning status.<br/> The callbacks WILL
    /// be executed (this is necessary in case we allocated the buffer).<br/>
    /// Afterwards, the request is automatically destroyed.
    /// </summary>
    public FSAsyncStatus FinishSynchronous(object control);

    /// <summary>
    /// Terminate any active work and destroy all resources and bookkeeping info.<br/>
    /// The callback will NOT be executed.
    /// </summary>
    public FSAsyncStatus Abort(object control);
}

public interface IThreadedFileMD5Processor
{
    public int SubmitThreadedMD5Request(byte pubBuffer, int cubBuffer, int packFileID, int packFileNumber, int packFileFraction);
    public bool BlockUntilMD5RequestComplete(int request, out MD5Value md5ValueOut);
    public bool IsMD5RequestComplete(int request, out MD5Value md5ValueOut);
}

public interface IBaseFileSystem
{
    public const string BASEFILESYSTEM_INTERFACE_VERSION = "VBaseFileSystem011";

    public int Read(out object output, int size, FileHandle file);
    public int Write(object input, int size, FileHandle file);

    public FileHandle Open(string filename, string options, string pathID = null);
    public void Close(FileHandle file);

    public void Seek(FileHandle file, int pos, FileSystemSeek seekType);
    public uint Tell(FileHandle file);
    public uint Size(FileHandle file);
    public uint Size(string filename, string pathID = null);

    public void Flush(FileHandle file);
    public bool Precache(string filename, string pathID = null);

    public bool FileExists(string filename, string pathID = null);
    public bool IsFileWritable(string filename, string pathID = null);
    public bool SetFileWritable(string filename, bool writable, string pathID = null);

    public long GetFileTime(string filename, string pathID = null);

    public bool ReadFile(string filename, string path, CUtlBuffer buf, int maxBytes = 0, int startingByte = 0, FSAllocFunc alloc = null);
    public bool WriteFile(string filename, string path, CUtlBuffer buf);
    public bool UnzipFile(string filename, string path, string destination);
}

public interface IFileSystem : IAppSystem, IBaseFileSystem
{
    // ---------------------------------------------------------
    // Steam operations
    // ---------------------------------------------------------


    public bool IsSteam();

    /// <summary>
    /// Supplying an extra app id will mount this app in addition
    /// to the one specified in the environment variable "<see cref="CSteamEnvVars.steamAppId"/>". <br/>
    /// <br/>
    /// If extraAppId is < -1, then it will mount that app ID only. <br/>
    /// (Was needed by the dedicated servers beause the "<see cref="CSteamEnvVars.steamAppId"/>" env var only gets passed to steam.dll
    /// at load time, so the dedicated server couldn't pass it in that way.)
    /// </summary>
    public FilesystemMountRetval MountSteamContent(int extraAppId = -1);

    // ---------------------------------------------------------
    // Search path manipulation
    // ---------------------------------------------------------

    /// <summary>
    /// Add paths in priority order (mod dir, game dir, ...)<br/>
    /// <br/>
    /// If one or more .pak files are in the specified directory, then they are<br/>
    /// added after the file system path<br/>
    /// <br/>
    /// If the path is the relative path to a .bsp file, then any previous .bsp file<br/>
    /// override is cleared and the current .bsp is searched for an embedded PAK file<br/>
    /// and this file becomes the highest priority search path (i.e., it's looked at first<br/>
    /// even before the mod's file system path).
    /// </summary>
    public void AddSearchPath(string path, string pathID, SearchPathAdd addType = SearchPathAdd.PATH_ADD_TO_TAIL);
    public bool RemoveSearchPath(string path, string pathID = null);

    /// <summary>
    /// Remove all search paths (including write path?)
    /// </summary>
    public void RemoveAllSearchPaths();

    /// <summary>
    /// Remove search paths associated with a given pathID
    /// </summary>
    public void RemoveSearchPaths(string pathID);

    /// <summary>
    /// This is for optimization. If you mark a path ID as "by request only", then files inside it<br/>
    /// will only be accessed if the path ID is specifically requested. Otherwise, it will be ignored.<br/>
    /// If there are currently no search paths with the specified path ID, then it will still<br/>
    /// remember it in case you add search paths with this path ID.
    /// </summary>
    public void MarkPathIDByRequestOnly(string pathID, bool requestOnly);

    /// <summary>
    /// Converts a partial path into a full path
    /// </summary>
    public string RelativePathToFullPath(string filename, string pathID, string localPath, int localPathBufferSize, PathTypeFilter pathFilter = PathTypeFilter.FILTER_NONE, PathTypeQuery pathType = PathTypeQuery.PATH_IS_NORMAL);

    /// <summary>
    /// Returns the search path, each path is separated by ;s. Returns the length of the string returned
    /// </summary>
    public int GetSearchPath(string pathID, bool getPackFiles, string path, int maxLen);

    /// <summary>
    /// Interface for custom pack files > 4GB
    /// </summary>
    public bool AddPackFile(string fullPath, string pathID);

    // ---------------------------------------------------------
    // File manipulation operations
    // ---------------------------------------------------------

    /// <summary>
    /// Deletes a file (on the WritePath)
    /// </summary>
    public void RemoveFile(string relativePath, string pathID = null);

    /// <summary>
    /// Renames a file (on the WritePath)
    /// </summary>
    public bool RenameFile(string oldPath, string newPath, string pathID = null);

    /// <summary>
    /// Create a local directory structure
    /// </summary>
    public void CreateDirHierarchy(string path, string pathID = null);

    /// <summary>
    /// File I/O and info
    /// </summary>
    public bool IsDirectory(string filename, string pathID = null);

    public void FileTimeToString(out string strip, int maxCharsIncludingTerminator, long fileTime);

    // ---------------------------------------------------------
    // Open file operations
    // ---------------------------------------------------------

    public void SetBufferSize(FileHandle file, uint bytes);

    public bool IsOk(FileHandle file);

    public bool EndOfFile(FileHandle file);

    public string ReadLine(out string output, int maxChars, FileHandle file);
    public int FPrintf(FileHandle file, string format, params object[] args);

    // ---------------------------------------------------------
    // Dynamic library operations
    // ---------------------------------------------------------

    // Load / unload modules
    public CSysModule LoadModule(string filename, string pathID = null, bool validatedDLLOnly = true);
    public void UnloadModule(CSysModule module);

    // ---------------------------------------------------------
    // File searching operations
    // ---------------------------------------------------------

    // FindFirst/FindNext. Also see FindFirstEx.
    public string FindFirst(string wildCard, FileFindHandle handle);
    public string FindNext(FileFindHandle handle);
    public bool FindIsDirectory(FileFindHandle handle);
    public void FindClose(FileFindHandle handle);

    /// <summary>
    /// Same as FindFirst, but you can filter by path ID, which can make it faster.
    /// </summary>
    public string FindFirstEx(string wildCard, string pathID, FileFindHandle handle);

    // ---------------------------------------------------------
    // File name and directory operations
    // ---------------------------------------------------------

    /// <summary>
    /// Converts a partial path into a full path
    /// </summary>
    [Obsolete("This method is obsolete! Use RelativePathToFullPath instead!")]
    public string GetLocalPath(string filename, string localPath, int localPathBuffer);

    /// <summary>
    /// Returns <see langword="true"/> on success (based on current list of search paths, otherwise <see langword="false"/> if
    /// it can't be resolved)
    /// </summary>
    public bool FullPathToRelativePath(string fullPath, out string relative, int maxlen);

    /// <summary>
    /// Gets the current working directory
    /// </summary>
    public bool GetCurrentDirectory(string directory, int maxlen);

    // ---------------------------------------------------------
    // Filename directory operations
    // ---------------------------------------------------------

    public FileNameHandle FindOrAddFileName(string filename);
    public bool String(FileNameHandle handle, byte[] buf, int buflen);

    // ---------------------------------------------------------
    // Asynchronous file operations
    // ---------------------------------------------------------

    // ---------------------------------------------------------
    // Global operations
    // ---------------------------------------------------------

    public FSAsyncStatus AsyncRead(FileAsyncRequest request, FSAsyncControl control = null) { return AsyncReadMultiple(request, 1, [control]); }
    public FSAsyncStatus AsyncReadMultiple(FileAsyncRequest request, int requests, FSAsyncControl[] controls = null);
    public FSAsyncStatus AsyncAppend(string filename, byte[] src, int srcBytes, bool freeMemory, FSAsynControl control = null);
    public FSAsyncStatus AsyncAppendFile(string appendToFileName, string appendFromFileName, FSAsyncControl control = null);
    public void AsyncFinishAll(int priority = 0);
    public void AsyncFinishAllWrites();
    public FSAsyncStatus AsyncFlush();
    public bool AsyncSuspend();
    public bool AsyncResume();

    /// <summary>
    /// Add async fetcher interface (<see cref="IAsyncFileFetch"/>). This gives apps a 
    /// hook to intercept async requests and pull the data from a source of their choosing.<br/>
    /// The immediate use case is to load assets from the CDN via HTTP.
    /// </summary>
    public void AsyncAddFetcher(IAsyncFileFetch fetcher);
    public void AsyncRemoveFetcher(IAsyncFileFetch fetcher);

    // ---------------------------------------------------------
    // Functions to hold a file open if planning on doing multiple reads. Use is optional,
    // and is taken only as a hint
    // ---------------------------------------------------------

    public FSAsyncStatus AsyncBeginRead(string file, FSAsyncFile file);
    public FSAsyncStatus AsyncEndRead(FSAsyncFile file);

    // ---------------------------------------------------------
    // Request management
    // ---------------------------------------------------------

    public FSAsyncStatus AsyncFinish(FSAsyncControl control, bool wait = true);
    public FSAsyncStatus AsyncGetResult(FSAsyncControl control, object[] data, int size);
    public FSAsyncStatus AsyncAbort(FSAsyncControl control);
    public FSAsyncStatus AsyncStatus(FSAsyncControl control);
    /// <summary>
    /// Set a new priority for a file already in the queue
    /// </summary>
    public FSAsyncStatus AsyncSetPriority(FSAsyncControl control, int newpriority);
    public void AsyncAddRef(FSAsyncControl control);
    public void AsyncRelease(FASyncControl control);

    // ---------------------------------------------------------
    // Remote resource management
    // ---------------------------------------------------------

    /// <summary>
    /// Starts waiting for resources to be available<br/>
    /// Returns <see cref="WaitForResourcesHandle.FILESYSTEM_INVALID_HANDLE"/> if there's nothing to wait on
    /// </summary>
    public WaitForResourceHandle WaitForResources(string resourcelist);

    /// <summary>
    /// Get progress on waiting for resources; progress is a float [0, 1], complete is <see langword="true"/> on the waiting being done<br/>
    /// Returns <see langword="false"/> if no progress is available<br/>
    /// Any calls after complete is <see langword="true"/> or on an invalid handle will return <see langword="false"/>, 0.0f, <see langword="true"/>
    /// </summary>
    public bool GetWaitForResourcesProgress(WaitForResourcesHandle handle, out float progress, out bool complete);

    /// <summary>
    /// Cancels a progress call
    /// </summary>
    public void CancelWaitForResources(WaitForResourcesHandle handle);

    /// <summary>
    /// Hints that a set of files will be loaded in near future<br/>
    /// <see cref="HintResourceNeed(string, int)"/> is not to be confused with resource precaching.
    /// </summary>
    public int HintResourceNeed(string hintlist, int forgetEverything);

    /// <summary>
    /// Returns <see langword="true"/> if a file is on disk
    /// </summary>
    public bool IsFileImmediatelyAvailable(string filename);

    /// <summary>
    /// Copies file out of pak/bsp/steam cache onto disk (to be accessible by third-party code)
    /// </summary>
    public void GetLocalCopy(string filename);

    // ---------------------------------------------------------
    // Debugging operations
    // ---------------------------------------------------------

    /// <summary>
    /// Dumpt to printf/<see cref="DebugApi.OutputDebugString(string)"/> the list of files that have not been closed
    /// </summary>
    public void PrintOpenedFile();
    public void PrintSearchPaths();

    // Output
    public void SetWarningFunc(/*delegate void Warning(string format, params object[] args)*/);
    public void SetWarningLevel(FileWarningLevel level);
    public void AddLoggingFunc(/*delegate void LogFunc(string filename, string accessType)*/);
    public void RemoveLoggingFunc(FileSystemLoggingFunc logFunc);

    /// <summary>
    /// Returns the file system statistics retrieved by the implementation. Returns <see langword="null"/> if not supported.
    /// </summary>
    public FileSystemStatistics GetFilesystemStatistics();

    // ---------------------------------------------------------
    // Start of new functions after Lost Coast release (7/05)
    // ---------------------------------------------------------

    public FileHandle OpenEx(string filename, string options, uint flags = 0, string pathID = null, string resolvedFilename = null);

    /// <summary>
    /// Extended version of read provides more context to allow for more optimal reading
    /// </summary>
    public int ReadEx(out object output, int sizeDest, int size, FileHandle file);
    public int ReadFileEx(string filename, string path, object[] buf, bool nullTerminate = false, bool optimalAlloc = false, int maxBytes = 0, int startingByte = 0, FSAllocFunc alloc = null);

    public FileNameHandle FindFileName(string filename);

#if DEBUG
    public void EnableBlockingFileAccessTracking(bool state);
    public bool IsBlockingFileAccessEnabled();

    public IBlockingFileItemList RetrieveBlockingFileAccessInfo();
#endif // DEBUG

    public void SetupPreloadData();
    public void DiscardPreloadData();

    // Fixme, we could do these via a string embedded into the compiled data, etc...
    public enum KeyValuesPreloadType
    {
        TYPE_VMT,
        TYPE_SOUNDEMITTER,
        TYPE_SOUNDSCAPE,
        NUM_PRELOAD_TYPES
    }

    public void LoadCompiledKeyValues(KeyValuesPreloadType type, string archiveFile);

    /// <summary>
    /// If the "PreloadedData" hasn't been purged, then this'll try and instance the <see cref="KeyValues"/> using the fast path of compiled <see cref="KeyValues"/> loaded during startup. <br/>
    /// Otherwise, it'll just fall through to the regular <see cref="KeyValues"/> loading routines
    /// </summary>
    public KeyValues LoadKeyValues(KeyValuesPreloadType type, string filename, string pathID = null);
    public bool LoadKeyValues(KeyValues head, KeyValuesPreloadType type, string filename, string pathID = null);
    public bool ExtractRootKeyName(KeyValuesPreloadType type, out string outbuf, uint bufsize, string filename, string pathID = null);

    public FSAsyncStatus AsyncWrite(string filename, object src, int srcBytes, bool freeMemory, bool append = false, FSAsyncControl control = null);
    public FSAsyncStatus AsyncWriteFile(string filename, CUtlBuffer buf, int srcBytes, bool freeMemory, bool append = false, FSAsyncControl control = null);
    // Async read functions with memory blame
    public FSAsyncStatus AsyncReadCreditAlloc(FileAsyncRequest request, string file, int line, FSAsyncControl control = null) { return AsyncReadMultipleCreditAlloc([request], 1, file, line, control); }
    public FSAsyncStatus AsyncReadMultipleCreditAlloc(FileAsyncRequest[] requests, int numRequests, string file, int line, FSAsyncControl control = null);

    public bool GetFileTypeForFullPath(string fullPath, string buf, uint bufSizeInBytes);

    // ---------------------------------------------------------
    // ---------------------------------------------------------

    public bool ReadToBuffer(FileHandle file, CUtlBuffer buf, int maxBytes = 0, FSAllocFunc alloc = null);

    // ---------------------------------------------------------
    // Optimal I/O operations
    // ---------------------------------------------------------

    public bool GetOptimalIOConstraints(FileHandle file, uint offsetAlign, uint sizeAlign, uint bufferAlign);
    public uint GetOptimalReadSize(FileHandle file, uint logicalSize);
    public object AllocOptimalReadBuffer(FileHandle file, uint size = 0, uint offset = 0);
    public void FreeOptimalReadBuffer();

    // ---------------------------------------------------------
    // 
    // ---------------------------------------------------------

    public void BeginMapAccess();
    public void EndMapAccess();

    /// <summary>
    /// Returns <see langword="true"/> on success, otherwise <see langword="false"/> if it can't be resolved
    /// </summary>
    public bool FullPathToRelativePathEx(string fullPath, string pathID, string relative, int maxLen);

    public int GetPathIndex(FileNameHandle handle);
    public long GetPathTime(string path, string pathID);

    public DVDMode GetDVDMode();

    // ---------------------------------------------------------
    // Whitelisting for pure servers
    // ---------------------------------------------------------

    /// <summary>
    /// This should be called ONCE at startup. Multiplayer games (gameinfo.txt does not contain singleplayer_only)
    /// want to enable this so sv_pure works.
    /// </summary>
    public void EnableWhitelistFileTracking(bool enable, bool cacheAllVPKHashes, bool recalculateAndCheckHashes);

    /// <summary>
    /// This is called when the client connects to a server using a pure_server_whitelist.txt file.
    /// </summary>
    public void RegisterFileWhitelist(IPureServerWhitelist whiteList, IFileList[] filesToReload);

    /// <summary>
    /// Called when the client logs onto a server. Any files that came off disk should be marked as
    /// unverified because this server may have a different set of files it wants to guarantee.
    /// </summary>
    public void MarkAllCRCsUnverified();
}