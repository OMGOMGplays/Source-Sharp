namespace FGDLib;

public class helperinfo
{
    public const int MAX_HELPER_NAME_LEN = 256;

    public CUtlVector<CHelperInfo> CHelperInfoList;
}

public class CHelperInfo
{
    protected string m_szName;
    protected CParameterList m_Parameters;

    public CHelperInfo()
    {
        m_szName = "\0";
    }

    public string GetName()
    {
        return m_szName;
    }

    public void SetName(string pszName)
    {
        if (pszName != null)
        {
            m_szName = pszName;
        }
    }

    public bool AddParameter(string pszParameter)
    {
        if ((pszParameter != null) && (pszParameter[0] != '\0'))
        {
            int nLen = pszParameter.Length;

            if (nLen > 0)
            {
                string pszNew = string.Empty;

                if (pszNew != null)
                {
                    pszNew = pszParameter;
                    m_Parameters.AddToTail(pszNew);
                    return true;
                }
            }
        }

        return false;
    }

    public int GetParameterCount()
    {
        return m_Parameters.Count();
    }

    public string GetParameter(int nIndex)
    {
        if (nIndex >= m_Parameters.Count())
        {
            return null;
        }

        return m_Parameters.Element(nIndex);
    }
}