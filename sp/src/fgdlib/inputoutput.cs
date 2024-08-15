namespace FGDLib;

public enum InputOutputType_t
{
    iotInvalid = -1,
    iotVoid,
    iotInt,
    iotBool,
    iotString,
    iotFloat,
    iotVector,
    iotEHandle,
    iotColor,
}

public class inputoutput
{
    public struct TypeMap_t
    {
        public InputOutputType_t eType;
        public string pszName;
    }
 
    public static TypeMap_t[] TypeMap =
    {
        new TypeMap_t {eType = InputOutputType_t.iotVoid, pszName = "void"},
        new TypeMap_t {eType = InputOutputType_t.iotInt, pszName = "integer"},
        new TypeMap_t {eType = InputOutputType_t.iotBool, pszName = "bool"},
        new TypeMap_t {eType = InputOutputType_t.iotString, pszName = "string"},
        new TypeMap_t {eType = InputOutputType_t.iotFloat, pszName = "float"},
        new TypeMap_t {eType = InputOutputType_t.iotVector, pszName = "vector"},
        new TypeMap_t {eType = InputOutputType_t.iotEHandle, pszName = "target_destination"},
        new TypeMap_t {eType = InputOutputType_t.iotColor, pszName = "color255"},
        new TypeMap_t {eType = InputOutputType_t.iotEHandle, pszName = "ehandle"},
    };
}

public class CClassInputOutputBase
{
    protected string g_pszEmpty = string.Empty;

    protected string m_szName;
    protected InputOutputType_t m_eType;
    protected string m_pszDescription;

    public CClassInputOutputBase()
    {
        m_eType = InputOutputType_t.iotInvalid;
        m_pszDescription = null;
    }

    public CClassInputOutputBase(string pszName, InputOutputType_t eType)
    {
        m_pszDescription = null;
    }

    public string GetName()
    {
        return m_szName;
    }

    public InputOutputType_t GetOutputType()
    {
        return m_eType;
    }

    public string GetTypeText()
    {
        for (int i = 0; i < Marshal.SizeOf(inputoutput.TypeMap) / Marshal.SizeOf(inputoutput.TypeMap[0]); i++)
        {
            if (inputoutput.TypeMap[i].eType == m_eType)
            {
                return inputoutput.TypeMap[i].pszName;
            }
        }

        return "unknown";
    }

    public string GetDescription()
    {
        if (m_pszDescription != null)
        {
            return m_pszDescription;
        }

        return g_pszEmpty;
    }

    public void SetName(string szName)
    {
        m_szName = szName;
    }

    public void SetType(InputOutputType_t eType)
    {
        m_eType = eType;
    }

    public InputOutputType_t SetType(string szType)
    {
        for (int i = 0; i < Marshal.SizeOf(inputoutput.TypeMap) / Marshal.SizeOf(inputoutput.TypeMap[0]); i++)
        {
            if (string.Compare(inputoutput.TypeMap[i].pszName.ToLower(), szType.ToLower()) == 0)
            {
                m_eType = inputoutput.TypeMap[i].eType;
                return m_eType;
            }
        }

        return InputOutputType_t.iotInvalid;
    }

    public void SetDescription(string pszDescription)
    {
        m_pszDescription = pszDescription;
    }

    public CClassInputOutputBase Equals(CClassInputOutputBase other)
    {
        m_szName = other.m_szName;
        m_eType = other.m_eType;

        m_pszDescription = null;

        if (other.m_pszDescription != null)
        {
            m_pszDescription = other.m_pszDescription;
        }
        else
        {
            m_pszDescription = null;
        }

        return this;
    }
}

public class CClassInput : CClassInputOutputBase
{
    public CClassInput()
    {

    }

    public CClassInput(string pszName, InputOutputType_t eType)
    {

    }
}

public class CClassOutput : CClassInputOutputBase
{
    public CClassOutput()
    {

    }

    public CClassOutput(string pszName, InputOutputType_t eType)
    {

    }
}