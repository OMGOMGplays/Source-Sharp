using SourceSharp.SP.Public.Mathlib;
using SourceSharp.SP.Public.Tier1;

namespace SourceSharp.SP.Public.Engine;

public interface IEngineSound
{
    public const int SOUND_FROM_UI_PANEL = -2;
    public const int SOUND_FROM_LOCAL_PLAYER = -1;
    public const int SOUND_FROM_WORLD = 0;

    public static SoundLevel SNDLEVEL_TO_COMPATIBILITY_MODE(dynamic x) { return (SoundLevel)(int)(x + 256); }
    public static SoundLevel SNDLEVEL_FROM_COMPATIBILITY_MODE(dynamic x) { return (SoundLevel)(int)(x - 256); }
    public static bool SNDLEVEL_IS_COMPATIBILITY_MODE(dynamic x) { return x >= 256; }

    public const string IENGINESOUND_CLIENT_INTERFACE_VERSION = "IEngineSoundClient003";
    public const string IENGINESOUND_SERVER_INTERFACE_VERSION = "IEngineSoundServer003";

    public bool PrecacheSound(string sample, bool reload = false, bool isUISound = false);
    public bool IsSoundPrecached(string sample);
    public void PrefetchSound(string sample);

    public float GetSoundDuration(string sample);

    public void EmitSound(IRecipientFilter filter, int entIndex, int channel, string sample, float volume, float attenuation, int flags = 0, int pitch = PITCH_NORM, int specialDSP = 0, Vector origin = null, Vector direction = null, CUtlVector<Vector> origins = null, bool updatePositions = true, float soundtime = 0.0f, int speakerentity = -1);
    public void EmitSound(IRecipientFilter filter, int entIndex, int channel, string sample, float volume, SoundLevel soundLevel, int flags = 0, int pitch = PITCH_NORM, int specialDSP = 0, Vector origin = null, Vector direction = null, CUtlVector<Vector> origins = null, bool updatePositions = true, float soundtime = 0.0f, int speakerentity = -1);
    public void EmitSound(IRecipientFilter filter, int endIndex, int channel, int sentenceIndex, float volume, SoundLevel soundLevel, int flags = 0, int pitch = PITCH_NORM, int specialDSP = 0, Vector origin = null, Vector direction = null, CUtlVector<Vector> origins = null, bool updatePositions = true, float soundtime = 0.0f, int speakerentity = -1);

    public void StopSound(int entIndex, int channel, string sample);

    public void StopAllSounds(bool clearBuffer);

    public void SetRoomType(IRecipientFilter filter, int roomType);

    public void SetPlayerDSP(IRecipientFilter filter, int dspType, bool fastReset);

    public void EmitAmbientSound(string sample, float volume, int pitch = PITCH_NORM, int flags = 0, float soundtime = 0.0f);

    public float GetDistGainFromSoundLevel(SoundLevel soundlevel, float dist);

    public int GetGuidForLastSoundEmitted();
    public bool IsSoundStillPlaying(int guid);
    public void StopSoundByGuid(int guid);
    public void SetVolumeByGuid(int guid, float vol);

    public void GetActiveSounds(CUtlVector<SndInfo> sndlist);

    public void PrecacheSentenceGroup(string groupName);
    public void NotifyBeginMoviePlayback();
    public void NotifyEndMoviePlayback();
}