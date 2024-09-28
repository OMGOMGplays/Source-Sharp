using SourceSharp.SP.Public.Mathlib;
using SourceSharp.SP.Public.Tier1;

namespace SourceSharp.SP.Public;

public interface IFileSystem;

public class CSPColor
{
    public Vector color;
    public float alpha;

    public CSPColor()
    {
        alpha = 1;
    }

    public CSPColor(Vector color, float alpha = 1)
    {
        this.color = color;
        this.alpha = alpha;
    }

    public CSPColor(float r, float g, float b, float a = 1)
    {
        color.Init(r, g, b);
        alpha = a;
    }
}

public class CSPVert
{
    public Vector pos;
    public CSPColor color;

    public CSPVert()
    {
    }

    public CSPVert(Vector pos, CSPColor color = null)
    {
        Init(pos, color);
    }

    public void Init(Vector pos, CSPColor color = null)
    {
        this.pos = pos;
        this.color = color;
    }
}

public class CSPVertList
{
    public CUtlVector<CSPVert> verts;

    public CSPVertList(int numVerts = 0)
    {
        if (numVerts != 0)
        {
            verts.AddMultipleToTail(numVerts);
        }
    }

    public CSPVertList(CSPVert[] verts, int numVerts)
    {
        this.verts.CopyArray(verts, numVerts);
    }

    public CSPVertList(Vector[] verts, int numVerts, CSPColor color = null)
    {
        this.verts.AddMultipleToTail(numVerts);

        for (int i = 0; i < numVerts; i++)
        {
            this.verts[i].pos = verts[i];
            this.verts[i].color = color;
        }
    }

    public CSPVertList(Vector[] verts, Vector[] colors, int numVerts)
    {
        this.verts.AddMultipleToTail(numVerts);

        for (int i = 0; i < numVerts; i++)
        {
            this.verts[i].pos = verts[i];
            this.verts[i].color = new CSPColor(colors[i]);
        }
    }

    public CSPVertList(Vector[] verts, CSPColor[] colors, int numVerts)
    {
        this.verts.AddMultipleToTail(numVerts);

        for (int i = 0; i < numVerts; i++)
        {
            this.verts[i].pos = verts[i];
            this.verts[i].color = colors[i];
        }
    }

    public CSPVertList(Vector vert1, CSPColor color1,
                       Vector vert2, CSPColor color2,
                       Vector vert3, CSPColor color3)
    {
        verts.AddMultipleToTail(3);

        verts[0].Init(vert1, color1);
        verts[1].Init(vert2, color2);
        verts[2].Init(vert3, color3);
    }
}

public class SPRGBA
{
    public char r, g, b, a;
}

public class CTextParams
{
    public Vector color;
    public float alpha;
    public bool solidBackground;
    public bool outline;

    public Vector pos;
    public bool centered;

    public QAngle angles;

    public bool twoSided;
    public float letterWidth;

    public CTextParams()
    {
        color.Init(1, 1, 1);
        alpha = 1;
        solidBackground = true;
        outline = true;
        pos.Init();
        centered = true;
        angles.Init();
        twoSided = true;
        letterWidth = 3;
    }
}

public interface IScratchPad3D
{
    public enum RenderState
    {
        RS_FillMode = 0,
        RS_ZRead,
        RS_ZBias
    }

    public enum FillMode
    {
        FillMode_WireFrame = 0,
        FillMode_Solid
    }

    public void Release();

    public void SetMapping(Vector inputMin,
                           Vector inputMax,
                       out Vector outputMin,
                       out Vector outputMax);

    public bool GetAutoFlush();
    public void SetAutoFlush(bool autoFlush);

    public void DrawPoint(CSPVert v, float pointSize);

    public void DrawLine(CSPVert v, CSPVert v2);

    public void DrawPolygon(CSPVertList vers);

    public void DrawRectYZ(float xPos, Vector2D min, Vector2D max, CSPColor color);
    public void DrawRectXZ(float yPos, Vector2D min, Vector2D max, CSPColor color);
    public void DrawRectXY(float zPos, Vector2D min, Vector2D max, CSPColor color);

    public void DrawWireframeBox(Vector min, Vector max, Vector color);

    public void DrawText(string str, CTextParams @params);

    public void SetRenderState(RenderState state, dynamic val);

    public void Clear();
    public void Flush();

    public void DrawImageBW(char[] data,
                            int width,
                            int height,
                            int pitchInBytes,
                            bool outlinePixels = true,
                            bool outlineImage = false,
                            Vector[] corners = null);

    public void DrawImageRGBA(SPRGBA[] data,
                              int width,
                              int height,
                              int pitchInBytes,
                              bool outlinePixels = true,
                              bool outlineImage = false,
                              Vector[] corners = null);
}

public class CScratchPadAutoRelease
{
    public IScratchPad3D pad;

    public CScratchPadAutoRelease(IScratchPad3D pad)
    {
        this.pad = pad;
    }

    ~CScratchPadAutoRelease()
    {
        if (pad != null)
        {
            pad.Release();
        }
    }
}