using SourceSharp.SP.Public.Tier1;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SourceSharp.SP.Public;

public interface IBaseCacheInfo
{
    public void Save(CUtlBuffer buf);
    public void Restore(CUtlBuffer buf);

    public void Rebuild(string filename);
}

public enum UtlCachedFileDataType
{
    UTL_CACHED_FILE_USE_TIMESTAMP = 0,
    UTL_CACHED_FILE_USE_FILESIZE
}

public class CUtlCachedFileData<T>
{
    public const int UTL_CACHE_SYSTEM_VERSION = 1;

    public const int UTL_CACHED_FILE_DATA_UNDEFINED_DISKINFO = sizeof(long) - 2;

    private CUtlRBTree<ElementType> elements;
    private CUtlVector<T> data;
    private CUtlString repositoryFileName;
    private int version;
    private PFNCOMPUTECACHEMETACHECKSUM metaChecksum;
    private uint currentMetaChecksum;
    private UtlCachedFileDataType fileCheckType;
    private bool neverCheckDisk = false;
    private bool @readonly = false;
    private bool saveManifest = false;
    private bool dirty = false;
    private bool initialized = false;

    public CUtlCachedFileData(
                              string repositoryFileName,
                              int version,
                              PFNCOMPUTECACHEMETACHECKSUM checksumfunc = null,
                              UtlCachedFileDataType fileCheckType = UtlCachedFileDataType.UTL_CACHED_FILE_USE_TIMESTAMP,
                              bool neverCheckDisk = false,
                              bool @readonly = false,
                              bool savemanifest = false)
    {
        elements = new CUtlRBTree<ElementType>(0, 0, FileNameHandleLessFunc);
        this.repositoryFileName = repositoryFileName;
        this.version = version;
        metaChecksum = checksumfunc;
        dirty = false;
        initialized = false;
        currentMetaChecksum = 0;
        this.fileCheckType = fileCheckType;
        this.neverCheckDisk = neverCheckDisk;
        this.@readonly = @readonly;
        saveManifest = savemanifest;

        Debug.Assert(!string.IsNullOrEmpty(repositoryFileName));
    }

    ~CUtlCachedFileData()
    {
        elements.RemoveAll();

        int c = data.Count();

        for (int i = 0; i < c; i++)
        {
            data[i] = default;
        }

        data.RemoveAll();
    }

    public T[] Get(string filename)
    {
        int idx = GetIndex(filename);

        ElementType e = elements[idx];

        if (e.fileinfo == -1 && fileCheckType == UtlCachedFileDataType.UTL_CACHED_FILE_USE_FILESIZE)
        {
            e.fileinfo = 0;
        }

        long cachefileinfo = e.fileinfo;

        if (e.diskfileinfo == UTL_CACHED_FILE_DATA_UNDEFINED_DISKINFO)
        {
            if (neverCheckDisk)
            {
                e.diskfileinfo = cachefileinfo;
            }
            else
            {
                if (fileCheckType == UtlCachedFileDataType.UTL_CACHED_FILE_USE_FILESIZE)
                {
                    e.diskfileinfo = fullFileSystem.Size(filename, "GAME");

                    if (e.diskfileinfo == -1)
                    {
                        e.diskfileinfo = 0;
                    }
                }
                else
                {
                    e.diskfileinfo = fullFileSystem.GetFileTime(filename, "GAME");
                }
            }
        }

        Debug.Assert(e.dataIndex != CUtlVector<T>.InvalidIndex());

        T[] data = this.data[e.dataIndex];

        Debug.Assert(data != null);

        if (cachefileinfo != e.diskfileinfo)
        {
            if (!@readonly)
            {
                RebuildCache(filename, data);
            }

            e.fileinfo = e.diskfileinfo;
        }

        return data;
    }

    public T this[int i]
    {
        get
        {
            return data[elements[i].dataIndex];
        }

        set
        {
            data[elements[i].dataIndex] = value;
        }
    }

    public int Count()
    {
        return elements.Count();
    }

    public void GetElementName(int i, string buf, int buflen)
    {
        buf.ToCharArray()[0] = '\0';

        if (!elements.IsValidIndex(i))
        {
            return;
        }

        fullFileSystem.String(elements[i].handle, buf, buflen);
    }

    public bool EntryExists(string filename)
    {
        ElementType element = new ElementType();
        element.handle = fullFileSystem.FindOrAddFileName(filename);

        int idx = elements.Find(element);
        return idx != elements.InvalidIndex() ? true : false;
    }

    public void SetElement(string name, long fileinfo, T src)
    {
        SetDirty(true);

        int idx = GetIndex(name);

        Debug.Assert(idx != elements.InvalidIndex());

        ElementType e = elements[idx];

        CUtlBuffer buf = new CUtlBuffer(0, 0, 0);

        Debug.Assert(e.dataIndex != CUtlVector<T>.InvalidIndex());

        T dest = data[e.dataIndex];

        Debug.Assert(dest != null);

        ((IBaseCacheInfo)src).Save(buf);
        ((IBaseCacheInfo)dest).Restore(buf);

        e.fileinfo = fileinfo;

        if (e.fileinfo == -1 && fileCheckType == UtlCachedFileDataType.UTL_CACHED_FILE_USE_FILESIZE)
        {
            e.fileinfo = 0;
        }

        e.diskfileinfo = UTL_CACHED_FILE_DATA_UNDEFINED_DISKINFO;
    }

    public bool IsUpToDate()
    {
        Debug.Assert(!initialized);

        if (string.IsNullOrEmpty(repositoryFileName))
        {
            Dbg.Error("CUtlCachedFileData: Can't IsUpToDate, no repository file specified.");
            return false;
        }

        currentMetaChecksum = metaChecksum != null ? metaChecksum.Invoke() : 0;

        FileHandle fh;

        fh = fullFileSystem.Open(repositoryFileName, "rb", "MOD");

        if (fh == FILESYSTEM_INVALID_HANDLE)
        {
            return false;
        }

        byte[] header = new byte[12];
        fullFileSystem.Read(header, Marshal.SizeOf(header), fh);
        fullFileSystem.Close(fh);

        int cacheversion = header[0];

        if (UTL_CACHE_SYSTEM_VERSION != cacheversion)
        {
            Dbg.DevMsg("Discarding repository '%s' due to cache system version change\n", repositoryFileName);

            Debug.Assert(!@readonly);

            if (!@readonly)
            {
                fullFileSystem.RemoveFile(repositoryFileName, "MOD");
            }

            return false;
        }

        int version = header[4];

        if (version != this.version)
        {
            Dbg.DevMsg("Discarding repository '%s' due to version change\n", repositoryFileName);

            Debug.Assert(!@readonly);

            if (!@readonly)
            {
                fullFileSystem.RemoveFile(repositoryFileName, "MOD");
            }

            return false;
        }

        uint cache_meta_checksum = header[8];

        if (cache_meta_checksum != currentMetaChecksum)
        {
            Dbg.DevMsg("Discarding repository '%s' due to meta checksum change\n", repositoryFileName);

            Debug.Assert(!@readonly);

            if (!@readonly)
            {
                fullFileSystem.RemoveFile(repositoryFileName, "MOD");
            }

            return false;
        }

        return true;
    }

    public void Shutdown()
    {

    }

    public bool Init()
    {

    }

    public void Save()
    {

    }

    public void Reload()
    {
        Shutdown();
        Init();
    }

    public void ForceRecheckDiskInfo()
    {

    }

    public void CheckDiskInfo(bool force_rebuild, long cacheFileTime = 0L)
    {

    }

    public void SaveManifest()
    {

    }

    public bool ManifestExists()
    {

    }

    public string GetRepositoryFileName()
    {
        return repositoryFileName;
    }

    public long GetFileInfo(string filename)
    {
        ElementType element = new ElementType();
        element.handle = fullFileSystem.FindOrAddFileName(filename);
        int idx = elements.Find(element);

        if (idx == elements.InvalidIndex())
        {
            return 0L;
        }

        return elements[idx].fileinfo;
    }

    public int GetNumElements()
    {
        return elements.Count();
    }

    public bool IsDirty()
    {
        return dirty;
    }

    public T RebuildItem(string filename)
    {

    }

    private void InitSmallBuffer(FileHandle fh, int fileSize, bool deleteFile)
    {

    }

    private void InitLargeBuffer(FileHandle fh, bool deleteFile)
    {

    }

    private int GetIndex(string filename)
    {
        ElementType element = new ElementType();
        element.handle = fullFileSystem.FindOrAddFileName(filename);
        int idx = elements.Find(element);

        if (idx == elements.InvalidIndex())
        {
            T data = default;

            int dataIndex = data.AddToTail(data);
            idx = elements.Insert(element);
            elements[idx].dataIndex = dataIndex;
        }

        return idx;
    }

    private void CheckInit()
    {

    }

    private void SetDirty(bool dirty)
    {
        this.dirty = dirty;
    }

    private void RebuildCache(string filename, T[] data)
    {

    }

    private struct ElementType
    {
        public FileNameHandle handle;
        public long fileinfo;
        public long diskfileinfo;
        public int dataIndex;

        public ElementType()
        {
            handle = null;
            fileinfo = 0;
            diskfileinfo = UTL_CACHED_FILE_DATA_UNDEFINED_DISKINFO;
            dataIndex = -1;
        }
    }

    private static bool FileNameHandleLessFunc(ElementType lhs, ElementType rhs)
    {
        return lhs.handle < rhs.handle;
    }
}