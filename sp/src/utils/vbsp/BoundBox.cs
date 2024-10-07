using SourceSharp.SP.Mathlib;
using System;

namespace SourceSharp.SP.VBSP;

public enum Axis
{
    AXIS_X = 0,
    AXIS_Y,
    AXIS_Z
}

public class BoundBox
{
    public const float COORD_NOTINIT = 99999.0f;

    public Vector mins, maxs;

    public BoundBox()
    {
        ResetBounds();
    }

    public BoundBox(Vector mins, Vector maxs)
    {
        this.mins = mins;
        this.maxs = maxs;
    }

    public void ResetBounds()
    {
        mins[0] = mins[1] = mins[2] = COORD_NOTINIT;
        maxs[0] = maxs[1] = maxs[2] = -COORD_NOTINIT;
    }

    public void SetBounds(Vector mins, Vector maxs)
    {
        this.mins = mins;
        this.maxs = maxs;
    }

    public void UpdateBounds(Vector mins, Vector maxs)
    {
        if (mins[0] < this.mins[0])
        {
            this.mins[0] = mins[0];
        }

        if (mins[1] < this.mins[1])
        {
            this.mins[1] = mins[1];
        }

        if (mins[2] < this.mins[2])
        {
            this.mins[2] = mins[2];
        }

        if (maxs[0] > this.maxs[0])
        {
            this.maxs[0] = maxs[0];
        }

        if (maxs[1] > this.maxs[1])
        {
            this.maxs[1] = maxs[1];
        }

        if (maxs[2] > this.maxs[2])
        {
            this.maxs[2] = maxs[2];
        }
    }

    public void UpdateBounds(Vector pt)
    {
        if (pt[0] < mins[0])
        {
            mins[0] = pt[0];
        }

        if (pt[1] < mins[1])
        {
            mins[1] = pt[1];
        }

        if (pt[2] < mins[2])
        {
            mins[2] = pt[2];
        }

        if (pt[0] > maxs[0])
        {
            maxs[0] = pt[0];
        }

        if (pt[1] > maxs[1])
        {
            maxs[1] = pt[1];
        }

        if (pt[2] > maxs[2])
        {
            maxs[2] = pt[2];
        }
    }

    public void UpdateBounds(BoundBox box)
    {
        UpdateBounds(box.mins, box.maxs);
    }

    public void GetBoundsCenter(out Vector dest)
    {
        dest = (mins + maxs) / 2.0f;
    }

    public void GetBounds(out Vector mins, out Vector maxs)
    {
        mins = this.mins;
        maxs = this.maxs;
    }

    public virtual bool IsIntersectingBox(Vector mins, Vector maxs)
    {
        if ((this.mins[0] >= maxs[0]) || (this.maxs[0] <= mins[0]))
        {
            return false;
        }

        if ((this.mins[1] >= maxs[1]) || (this.maxs[1] <= mins[1]))
        {
            return false;
        }

        if ((this.mins[2] >= maxs[2]) || (this.maxs[2] <= mins[2]))
        {
            return false;
        }

        return true;
    }

    public bool IsInsideBox(Vector mins, Vector maxs)
    {
        if ((this.mins[0] < maxs[0]) || (this.maxs[0] > mins[0]))
        {
            return false;
        }

        if ((this.mins[1] < maxs[1]) || (this.maxs[1] > mins[1]))
        {
            return false;
        }

        if ((this.mins[2] < maxs[2]) || (this.maxs[2] > mins[2]))
        {
            return false;
        }

        return true;
    }

    public bool ContainsPoint(Vector pt)
    {
        for (int i = 0; i < 3; i++)
        {
            if (pt[i] < mins[i] || pt[i] > maxs[i])
            {
                return false;
            }
        }

        return true;
    }

    public bool IsValidBox()
    {
        for (int i = 0; i < 3; i++)
        {
            if (mins[i] > maxs[i])
            {
                return false;
            }
        }

        return true;
    }

    public void GetBoundsSize(out Vector size)
    {
        size = new Vector(maxs[0] - mins[0], maxs[1] - mins[1], maxs[2] - mins[2]);
    }

    public void SnapToGrid(int gridSize)
    {
        GetBoundsSize(out Vector size);

        for (int i = 0; i < 3; i++)
        {
            mins[i] = Snap(mins[i], gridSize);
            maxs[i] = mins[i] + size[i];
        }
    }

    public void Rotate90(Axis axis)
    {
        Axis e1 = Axis.AXIS_X, e2 = Axis.AXIS_Y;

        GetBoundsCenter(out Vector center);

        switch (axis)
        {
            case Axis.AXIS_Z:
                e1 = Axis.AXIS_X;
                e2 = Axis.AXIS_Y;
                break;

            case Axis.AXIS_X:
                e1 = Axis.AXIS_Y;
                e2 = Axis.AXIS_Z;
                break;

            case Axis.AXIS_Y:
                e1 = Axis.AXIS_X;
                e2 = Axis.AXIS_Z;
                break;
        }

        float tmp1, tmp2;

        tmp1 = mins[(int)e1] - center[(int)e1] + center[(int)e2];
        tmp2 = maxs[(int)e1] - center[(int)e1] + center[(int)e2];
        mins[(int)e1] = mins[(int)e2] - center[(int)e2] + center[(int)e1];

    }

    public static float V_rint(float f)
    {
        if (f > 0.0f)
        {
            return (float)Math.Floor(f + 0.5f);
        }
        else if (f < 0.0f)
        {
            return (float)Math.Floor(f - 0.5f);
        }
        else
        {
            return 0.0f;
        }
    }

    public static int Snap(float value, int gridSize)
    {
        return (int)V_rint(value / gridSize) * gridSize;
    }
}