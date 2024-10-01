using SourceSharp.SP.Public.Tier1;
using SourceSharp.SP.Mathlib;

using System;

namespace SourceSharp.SP.Public;

public class CScratchPadGraph
{
    private IScratchPad3D pad;

    private Vector timeAxis;
    private float inchesPerSecond;

    private Vector valueAxis;
    private float inchesPerValue;

    private float timeLabelEveryNSeconds;
    private int timeLabelsDrawn;

    private Vector timeLineColor;
    private Vector valueLineColor;

    private float timeOrigin;
    private float valueOrigin;
    private float highestTime;

    private float highestValue;
    private float lowestValue;

    private CUtlVector<CLineInfo> lineInfos;

    public CScratchPadGraph()
    {
        pad = null;
    }

    public void Init(
                     IScratchPad3D pad,

                     Vector timeAxis,
                     Vector timeLineColor,

                     Vector valueAxis,
                     Vector valueLineColor,

                     float inchesPerSecond = 1,
                     float timeOrigin = 0,

                     float timeLabelEveryNSeconds = 1,

                     float inchesPerValue = 1,
                     float valueOrigin = 0
                     )
    {
        this.pad = pad;
        this.timeAxis = timeAxis;
        this.inchesPerSecond = inchesPerSecond;
        this.valueAxis = valueAxis;
        this.inchesPerValue = inchesPerValue;
        this.timeLabelEveryNSeconds = timeLabelEveryNSeconds;

        this.timeLineColor = timeLineColor;
        this.valueLineColor = valueLineColor;

        this.timeOrigin = timeOrigin;
        this.valueOrigin = valueOrigin;

        timeLabelsDrawn = 0;
        highestTime = timeOrigin;
        highestValue = valueOrigin;
    }

    public bool IsInitted()
    {
        return pad != null;
    }

    public int AddLine(Vector color)
    {
        CLineInfo info = new();
        info.first = true;
        info.color = color;
        return lineInfos.AddToTail(info);
    }

    public void AddSample(int line, float time, float value)
    {
        CLineInfo info = lineInfos[line];

        UpdateTicksAndStuff(time, value);

        if (!info.first)
        {
            Vector start = GetSamplePosition(info.lastTime, info.lastValue);
            Vector end = GetSamplePosition(time, value);
            pad.DrawLine(
                             new CSPVert(start, new CSPColor(info.color)),
                             new CSPVert(start, new CSPColor(info.color))
                         );
        }

        info.lastTime = time;
        info.lastValue = value;
        info.first = false;
    }

    public void AddVerticalLine(float time, float minValue, float maxValue, CSPColor color)
    {
        Vector v1 = GetSamplePosition(time, minValue);
        Vector v2 = GetSamplePosition(time, maxValue);

        pad.DrawLine(new CSPVert(v1, color),
                     new CSPVert(v2, color));
    }

    public Vector GetSamplePosition(float time, float value)
    {
        Vector ret = timeAxis * ((time - timeOrigin) * inchesPerSecond) +
                     valueAxis * ((value - valueOrigin) * inchesPerValue);

        return ret;
    }

    private void UpdateTicksAndStuff(float time, float value)
    {
        if (time > highestTime)
        {
            Vector start = GetSamplePosition(highestTime, valueOrigin);
            Vector end = GetSamplePosition(time, valueOrigin);

            pad.DrawLine(new CSPVert(start, new CSPColor(timeLineColor)),
                         new CSPVert(end, new CSPColor(timeLineColor)));

            highestTime = time;
        }

        if (value > highestValue)
        {
            Vector start = GetSamplePosition(timeOrigin, highestValue);
            Vector end = GetSamplePosition(timeOrigin, value);

            pad.DrawLine(new CSPVert(start, new CSPColor(valueLineColor)),
                         new CSPVert(end, new CSPColor(valueLineColor)));

            for (int i = 0; i < timeLabelsDrawn; i++)
            {
                float _time = timeOrigin + timeLabelsDrawn * timeLabelEveryNSeconds;

                pad.DrawLine(new CSPVert(GetSamplePosition(_time, highestValue)),
                             new CSPVert(GetSamplePosition(_time, value)));
            }

            highestValue = value;
        }

        int highestTextLabel = (int)Math.Ceiling((time - timeOrigin) / timeLabelEveryNSeconds + 0.5f);

        while (timeLabelsDrawn < highestTextLabel)
        {
            CTextParams @params = new CTextParams();

            float _time = timeOrigin + timeLabelsDrawn * timeLabelEveryNSeconds;

            @params.solidBackground = true;
            @params.pos = GetSamplePosition(time, valueOrigin - 5);
            @params.twoSided = true;

            string str = $"Time {_time:0.##}";
            pad.DrawText(str, @params);

            pad.DrawLine(new CSPVert(GetSamplePosition(_time, valueOrigin)),
                         new CSPVert(GetSamplePosition(_time, highestValue)));

            timeLabelsDrawn++;
        }
    }

    private class CLineInfo
    {
        public bool first;
        public float lastTime;
        public float lastValue;
        public Vector color;
    }

    public static void ScratchPad_DrawLitCone(
                                              IScratchPad3D pad,
                                              Vector baseCenter,
                                              Vector tip,
                                              Vector brightColor,
                                              Vector darkColor,
                                              Vector lightDir,
                                              float baseWidth,
                                              int segments)
    {
        Vector dir = tip - baseCenter;
        Vector.VectorNormalize(ref dir);

        Vector right = new Vector(), up = new Vector();
        Mathlib.Mathlib.VectorVectors(dir, right, up);
        right *= baseWidth;
        up *= baseWidth;

        CSPVertList bottomCap = new CSPVertList(), tri = new CSPVertList();
        bottomCap.verts.SetSize(segments);
        tri.verts.SetSize(3);

        float dot = -lightDir.Dot(dir);
        Vector topColor, bottomColor;
        Vector.VectorLerp(darkColor, brightColor, RemapVal(-dot, -1, 1, 0, 1), out bottomColor);

        Vector prevBottom = baseCenter + right;

        for (int i = 0; i < segments; i++)
        {
            float angle = (i + 1) * Mathlib.Mathlib.M_PI_F * 2.0f / segments;
            Vector offset = right * Math.Cos(angle) + up * Math.Sin(angle);
            Vector curBottom = baseCenter + offset;

            Vector v1 = tip;
            Vector v2 = prevBottom;
            Vector v3 = curBottom;
            Vector faceNormal = (v2 - v1).Cross(v3 - v1);
            Vector.VectorNormalize(ref faceNormal);

            dot = -lightDir.Dot(faceNormal);
            Vector color;
            Vector.VectorLerp(darkColor, brightColor, RemapVal(dot, -1, 1, 0, 1), out color);

            tri.verts[0] = new CSPVert(v1, new CSPColor(color));
            tri.verts[1] = new CSPVert(v2, new CSPColor(color));
            tri.verts[2] = new CSPVert(v3, new CSPColor(color));
            pad.DrawPolygon(tri);

            bottomCap.verts[i] = new CSPVert(curBottom, new CSPColor(bottomColor));
        }

        pad.DrawPolygon(bottomCap);
    }

    public static void ScratchPad_DrawLitCylinder(
                                                  IScratchPad3D pad,
                                                  Vector v1,
                                                  Vector v2,
                                                  Vector brightColor,
                                                  Vector darkColor,
                                                  Vector lightDir,
                                                  float width,
                                                  int segments)
    {
        Vector dir = v2 - v1;
        Vector.VectorNormalize(ref dir);

        Vector right = new Vector(), up = new Vector();
        Mathlib.Mathlib.VectorVectors(dir, right, up);
        right *= width;
        up *= width;

        CSPVertList topCap = new CSPVertList();
        CSPVertList bottomCap = new CSPVertList();
        CSPVertList quad = new CSPVertList();

        topCap.verts.SetSize(segments);
        bottomCap.verts.SetSize(segments);
        quad.verts.SetSize(4);

        float dot = -lightDir.Dot(dir);
        Vector topColor, bottomColor;

        Vector.VectorLerp(darkColor, brightColor, RemapVal( dot, -1, 1, 0, 1), out topColor   );
        Vector.VectorLerp(darkColor, brightColor, RemapVal(-dot, -1, 1, 0, 1), out bottomColor);

        Vector prevTop = v1 + right;
        Vector prevBottom = v2 + right;

        for (int i = 0; i < segments; i++)
        {
            float angle = (i + 1) * Mathlib.Mathlib.M_PI_F * 2.0f / segments;
            Vector offset = right * Math.Cos(angle) + up * Math.Sin(angle);
            Vector curTop = v1 + offset;
            Vector curBottom = v2 + offset;

            Vector.VectorNormalize(ref offset);
            dot = -lightDir.Dot(offset);
            Vector color;
            Vector.VectorLerp(darkColor, brightColor, RemapVal(dot, -1, 1, 0, 1), out color);

            quad.verts[0] = new CSPVert(prevTop, new CSPColor(color));
            quad.verts[1] = new CSPVert(prevBottom, new CSPColor(color));
            quad.verts[2] = new CSPVert(curBottom, new CSPColor(color));
            quad.verts[3] = new CSPVert(curTop, new CSPColor(color));
            pad.DrawPolygon(quad);

            topCap.verts[i] = new CSPVert(curTop, new CSPColor(topColor));
        }

        pad.DrawPolygon(topCap);
        pad.DrawPolygon(bottomCap);
    }

    public static void ScratchPad_DrawArrow(
                                            IScratchPad3D pad,
                                            Vector pos,
                                            Vector direction,
                                            Vector color,
                                            float length = 20,
                                            float lineWidth = 3,
                                            float headWidth = 8,
                                            int cylinderSegments = 5,
                                            int headSegments = 8,
                                            float arrowheadPercentage = 0.3f)
    {
        Vector normDir = direction;
        Vector.VectorNormalize(ref normDir);

        Vector coneBase = pos + normDir * (length * (1 - arrowheadPercentage));
        Vector coneEnd = pos + normDir * length;

        Vector lightDir = new Vector(-1, -1, -1);
        Vector.VectorNormalize(ref lightDir);

        pad.SetRenderState(IScratchPad3D.RenderState.RS_FillMode, IScratchPad3D.FillMode.FillMode_Solid);
        pad.SetRenderState(IScratchPad3D.RenderState.RS_ZRead, true);

        ScratchPad_DrawLitCylinder(pad, pos, coneBase, color, color * 0.25f, lightDir, lineWidth, cylinderSegments);
        ScratchPad_DrawLitCone(pad, coneBase, coneEnd, color, color * 0.25f, lightDir, headWidth, headSegments);
    }

    public static void ScratchPad_DrawArrowSimple(
                                                  IScratchPad3D pad,
                                                  Vector pos,
                                                  Vector direction,
                                                  Vector color,
                                                  float length)
    {
        ScratchPad_DrawArrow(pad, pos, direction, color, length, length * 1.0f / 15, length * 3.0f / 15, 4, 4);
    }

    public static void ScratchPad_DrawSphere(
                                             IScratchPad3D pad,
                                             Vector center,
                                             float radius,
                                             Vector color,
                                             int subDivs = 7)
    {
        CUtlVector<Vector> prevPoints = new CUtlVector<Vector>();
        prevPoints.SetSize(subDivs);

        for (int slice = 0; slice < subDivs; slice++)
        {
            float halfSliceAngle = Mathlib.Mathlib.M_PI_F * slice / (subDivs - 1);

            if (slice == 0)
            {
                prevPoints[0] = center + new Vector(0, 0, radius);

                for (int z = 1; z < prevPoints.Count(); z++)
                {
                    prevPoints[z] = prevPoints[0];
                }
            }
            else
            {
                for (int subPt = 0; subPt < subDivs; subPt++)
                {
                    float halfAngle = Mathlib.Mathlib.M_PI_F * subPt / (subDivs - 1);
                    float angle = halfAngle * 2;

                    Vector pt = new Vector();

                    if (slice == subDivs - 1)
                    {
                        pt = center - new Vector(0, 0, radius);
                    }
                    else
                    {
                        pt.x = (float)(Math.Cos(angle) * Math.Sin(halfSliceAngle));
                        pt.y = (float)(Math.Sin(angle) * Math.Sin(halfSliceAngle));
                        pt.z = (float)Math.Cos(angle);

                        pt *= radius;
                        pt += center;
                    }

                    pad.DrawLine(new CSPVert(pt, new CSPColor(color)), new CSPVert(prevPoints[subPt], new CSPColor(color)));
                    prevPoints[subPt] = pt;
                }

                if (slice != subDivs - 1)
                {
                    for (int i = 0; i < subDivs; i++)
                    {
                        pad.DrawLine(new CSPVert(prevPoints[i], new CSPColor(color)), new CSPVert(prevPoints[(i + 1) % subDivs], new CSPColor(color)));
                    }
                }
            }
        }
    }

    public static void ScratchPad_DrawAABB(
                                           IScratchPad3D pad,
                                           Vector mins,
                                           Vector maxs,
                                           Vector color)
    {
        int[,] vertOrder = { { 0, 0 }, { 1, 0 }, { 1, 1 }, { 0, 1 } };
        Vector[] vecs = { mins, maxs };

        Vector top = new Vector(), bottom = new Vector(), prevTop = new Vector(), prevBottom = new Vector();
        top.z = prevTop.z = maxs.z;
        bottom.z = prevBottom.z = mins.z;

        prevTop.x = prevBottom.x = vecs[vertOrder[3, 0]].x;
        prevTop.y = prevBottom.y = vecs[vertOrder[3, 1]].y;

        for (int i = 0; i < 4; i++)
        {
            top.x = bottom.x = vecs[vertOrder[i, 0]].x;
            top.y = bottom.y = vecs[vertOrder[i, 1]].y;

            pad.DrawLine(new CSPVert(prevTop   , new CSPColor(color)), new CSPVert(top   , new CSPColor(color)));
            pad.DrawLine(new CSPVert(prevBottom, new CSPColor(color)), new CSPVert(bottom, new CSPColor(color)));
            pad.DrawLine(new CSPVert(top       , new CSPColor(color)), new CSPVert(bottom, new CSPColor(color)));

            prevTop = top;
            prevBottom = bottom;
        }
    }
}