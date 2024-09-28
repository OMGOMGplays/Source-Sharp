namespace SourceSharp.SP.Public;

public struct TagMSG;

public interface IStudioDataCache;

public enum RequestRetval
{
    REQUEST_OK = 0,
    REQUEST_QUIT
}

public interface IHammer : IAppSystem
{
    public const string INTERFACEVERSION_HAMMER = "Hammer001";

    public bool HammerPreTranslateMessage(TagMSG msg);
    public bool HammerIsIdleMessage(TagMSG msg);
    public bool HammerOnIdle(long count);

    public void RunFrame();

    public string GetDefaultMod();
    public string GetDefaultGame();

    public bool InitSessionGameConfig(string gameDir);

    public RequestRetval RequestNewConfig();

    public string GetDefaultModFullPath();

    public int MainLoop();
}