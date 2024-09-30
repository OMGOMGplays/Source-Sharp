using SourceSharp.SP.Public.Mathlib;

namespace SourceSharp.SP.Public.Game.Server;

public class PluginVariant
{
    public bool bVal;
    public int iVal;
    public float flVal;
    public float[] vecVal = new float[3];
    public Color32 rgbaVal;
    private Edict eVal;
    private string stringVal;

    private FieldType fieldType;

    public PluginVariant()
    {
        fieldType = FieldType.FIELD_VOID;
        iVal = 0;
    }

    public bool Bool()
    {
        return fieldType == FieldType.FIELD_BOOLEAN ? bVal : false;
    }

    public string String()
    {
        return ToString();
    }

    public int Int()
    {
        return fieldType == FieldType.FIELD_INTEGER ? iVal : 0;
    }

    public float Float()
    {
        return fieldType == FieldType.FIELD_FLOAT ? flVal : 0;
    }

    public Edict Edict()
    {
        if (fieldType == FieldType.FIELD_EHANDLE)
        {
            return eVal;
        }

        return null;
    }

    public Color32 Color32()
    {
        return rgbaVal;
    }

    public void Vector3D(ref Vector vec)
    {
        if (fieldType == FieldType.FIELD_VECTOR || fieldType == FieldType.FIELD_POSITION_VECTOR)
        {
            vec[0] = vecVal[0];
            vec[1] = vecVal[1];
            vec[2] = vecVal[2];
        }
        else
        {
            vec = Mathlib.Mathlib.vec3_origin;
        }
    }

    public FieldType FieldType()
    {
        return fieldType;
    }

    public void SetBool(bool b)
    {
        bVal = b;
        fieldType = FieldType.FIELD_BOOLEAN;
    }

    public void SetString(string s)
    {
        stringVal = s;
        fieldType = FieldType.FIELD_STRING;
    }

    public void SetInt(int i)
    {
        iVal = i;
        fieldType = FieldType.FIELD_INTEGER;
    }

    public void SetFloat(float f)
    {
        flVal = f;
        fieldType = FieldType.FIELD_FLOAT;
    }

    public void SetEdict(Edict e)
    {
        eVal = e;
        fieldType = FieldType.FIELD_EHANDLE;
    }

    public void SetVector3D(Vector vec)
    {
        vecVal[0] = vec[0]; vecVal[1] = vec[1]; vecVal[2] = vec[2];
        fieldType = FieldType.FIELD_VECTOR;
    }

    public void SetPositionVector3D(Vector vec)
    {
        vecVal[0] = vec[0]; vecVal[1] = vec[1]; vecVal[2] = vec[2];
        fieldType = FieldType.FIELD_POSITION_VECTOR;
    }

    public void SetColor32(Color32 rgba)
    {
        rgbaVal = rgba;
        fieldType = FieldType.FIELD_COLOR32;
    }

    public void SetColor32(int r, int g, int b, int a)
    {
        rgbaVal.r = r; rgbaVal.g = g; rgbaVal.b = b; rgbaVal.a = a;
        fieldType = FieldType.FIELD_COLOR32;
    }

    protected new string ToString()
    {
        string buf;

        switch (fieldType)
        {
            case FieldType.FIELD_STRING:
                return stringVal;

            case FieldType.FIELD_BOOLEAN:
                if (bVal == false)
                {
                    buf = "false";
                }
                else
                {
                    buf = "true";
                }

                return buf;

            case FieldType.FIELD_INTEGER:
                return buf = iVal.ToString();

            case FieldType.FIELD_FLOAT:
                return buf = flVal.ToString();

            case FieldType.FIELD_COLOR32:
                buf = $"{rgbaVal.r} {rgbaVal.g} {rgbaVal.b} {rgbaVal.a}";
                return buf;

            case FieldType.FIELD_VECTOR:
                buf = $"{vecVal[0]} {vecVal[1]} {vecVal[2]}";
                return buf;

            case FieldType.FIELD_VOID:
                return buf = null;
        }

        return "No conversion to string";
    }
}