namespace SourceSharp.SP.Public.Engine;

public interface IEngineVoice
{
    public const string IENGINEVOICE_INTERFACE_VERSION = "IEngineVoice001";

    public bool IsHeadsetPresent(int controller);
    public bool IsLocalPlayerTalking(int controller);

    public void AddPlayerToVoiceList(XUID player, int controller);
    public void RemovePlayerFromVoiceList(XUID player, int controller);

    public void GetRemoteTalkers(int numTalkers, XUID[] remoteTalkers);

    public bool VoiceUpdateData(int controller);
    public void GetVoiceData(int controller, byte[] voiceDataBuffer, uint numVoiceDataBytes);
    public void VoiceResetLocalData(int controller);

    public void SetPlaybackPriority(XUID remoteTalker, int controller, int allowPlayback);
    public void PlayIncomingVoiceData(XUID xuid, byte[] data, uint dataSize, bool[] audiblePlayers = null);

    public void RemoveAllTalkers();
}