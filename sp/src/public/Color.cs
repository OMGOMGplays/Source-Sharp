using System;

namespace SourceSharp.SP.Public;

public class Color
{
    public char[] color = new char[4];

    public Color()
    {
        color = new char[4];
    }

    public Color(int r, int g, int b)
    {
        SetColor(r, g, b, 0);
    }

    public Color(int r, int g, int b, int a)
    {
        SetColor(r, g, b, a);
    }

    public void SetColor(int r, int g, int b, int a)
    {
        color[0] = (char)r;
        color[1] = (char)g;
        color[2] = (char)b;
        color[3] = (char)a;
    }

    public void GetColor(ref int r, ref int g, ref int b, ref int a)
    {
        r = color[0];
        g = color[1];
        b = color[2];
        a = color[3];
    }

    public void SetRawColor(char[] color32)
    {
        color = color32;
    }

    public char[] GetRawColor()
    {
        return color;
    }

    public int r { get { return color[0]; } set { color[0] = (char)value; } }
    public int g { get { return color[1]; } set { color[1] = (char)value; } }
    public int b { get { return color[2]; } set { color[2] = (char)value; } }
    public int a { get { return color[3]; } set { color[3] = (char)value; } }

    public char this[int i]
    {
        get
        {
            if (i < 0 || i >= 4)
            {
                throw new IndexOutOfRangeException();
            }

            return color[i];
        }

        set
        {
            if (i < 0 || i >= 4)
            {
                throw new IndexOutOfRangeException();
            }

            color[i] = (char)value;
        }
    }

    public static bool operator ==(Color lhs, Color rhs)
    {
        return lhs.color == rhs.color;
    }

    public static bool operator !=(Color lhs, Color rhs)
    {
        return !(lhs == rhs);
    }
}