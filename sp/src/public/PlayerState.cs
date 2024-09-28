using SourceSharp.SP.Public.Mathlib;

namespace SourceSharp.SP.Public;

public class CPlayerState
{
    ~CPlayerState()
    {

    }

    public CNetworkVar<bool> deadflag;
    public QAngle angle;

    public string netname;
    public int fixangle;
    public QAngle anglechange;
    public bool hltv;
    public bool replay;
    public int frags;
    public int deaths;
}