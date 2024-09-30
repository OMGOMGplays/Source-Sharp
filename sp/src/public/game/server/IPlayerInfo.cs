using SourceSharp.SP.Public.Mathlib;

namespace SourceSharp.SP.Public.Game.Server;

public class CBotCmd
{
    public int command_number;
    public int tick_count;
    public int buttons;
    public int weaponselect;
    public int weaponsubtype;
    public int random_seed;

    public QAngle viewangles;

    public float forwardmove;
    public float sidemove;
    public float upmove;

    public byte impulse;

    public short mousedx;
    public short mousedy;

    public bool hasbeenpredicted;

    public CBotCmd()
    {
        Reset();
    }

    ~CBotCmd()
    {

    }

    public void Reset()
    {
        command_number = 0;
    }
}