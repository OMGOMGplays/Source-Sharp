namespace SourceSharp.SP.Public.Engine;

public struct SndInfo
{
    public int guid;
    public FileNameHandle filenameHandle;
    public int soundSource;
    public int channel;
    public int speakerEntity;
    public float volume;
    public float lastSpatializedVolume;
    public float radius;
    public int pitch;
    public Vector origin;
    public Vector direction;

    public bool updatePositions;
    public bool isSentence;
    public bool dryMix;
    public bool speaker;
    public bool specialDSP;
    public bool fromServer;
}