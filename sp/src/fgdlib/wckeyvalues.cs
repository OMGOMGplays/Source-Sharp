namespace FGDLib;

public class wckeyvalues
{
    public const int KEYVALUE_MAX_KEY_LENGTH = 80;
    public const int KEYVALUE_MAX_VALUE_LENGTH = 512;

    public CUtlVector<MDkeyvalue> KeyValueArray;

    public WCKeyValuesT<WCKVBase_Dict> WCKeyValues;
    public WCKeyValuesT<WCKVBase_Vector> WCKeyValuesVector;

    public static void StripEdgeWhiteSpace(string psz)
    {
        if (psz == null)
        {
            return;
        }

        string pszBase = psz;

        psz.Replace(' ', '\0');
    }
}

public class MDkeyvalue
{
    public string szKey;
    public string szValue;

    public MDkeyvalue()
    {
        szKey = "\0";
        szValue = "\0";
    }

    public MDkeyvalue(string pszKey, string pszValue)
    {
        szKey = "\0";
        szValue = "\0";

        Set(pszKey, pszValue);
    }

    public void Set(string pszKey, string pszValue)
    {
        Debug.Assert(pszKey != null);
        Debug.Assert(pszValue != null);

        szKey = pszKey;
        szValue = pszValue;
    }

    public string Key()
    {
        return szKey;
    }

    public string Value()
    {
        return szValue;
    }

    public int SerializeRMF(FileStream f, bool bRMF)
    {
        return 0;
    }

    public int SerializeMAP(FileStream f, bool bRMF)
    {
        return 0;
    }

    public MDkeyvalue Equals(MDkeyvalue other)
    {
        szKey = other.szKey;
        szValue = other.szValue;

        return this;
    }
}

public class WCKVBase_Vector
{
    protected CUtlVector<MDkeyvalue> m_KeyValues;

    public int GetCount()
    {
        return m_KeyValues.Count();
    }

    public int GetFirst()
    {
        return m_KeyValues.Count() - 1;
    }

    public int GetNext(int i)
    {
        return i - 1;
    }

    public static int GetInvalidIndex()
    {
        return -1;
    }

    public void RemoveKeyAt(int nIndex)
    {
        Debug.Assert(nIndex >= 0);
        Debug.Assert(nIndex < (int)m_KeyValues.Count());

        if ((nIndex >= 0) && (nIndex < (int)m_KeyValues.Count()))
        {
            m_KeyValues.Remove(nIndex);
        }
    }

    public int FindKeyByName(string pKeyName)
    {
        for (int i = 0; i < m_KeyValues.Count(); i++)
        {
            if (string.Compare(m_KeyValues[i].szKey.ToLower(), pKeyName.ToLower()) == 0)
            {
                return i;
            }
        }

        return GetInvalidIndex();
    }

    public void AddKeyValue(string pszKey, string pszValue)
    {
        if (pszKey == null || pszValue == null)
        {
            return;
        }

        string szTmpKey = string.Empty;
        string szTmpValue = string.Empty;

        szTmpKey = pszKey;
        szTmpValue = pszValue;

        wckeyvalues.StripEdgeWhiteSpace(szTmpKey);
        wckeyvalues.StripEdgeWhiteSpace(szTmpValue);

        MDkeyvalue newkv = new();
        newkv.szKey = szTmpKey;
        newkv.szValue = szTmpValue;
        m_KeyValues.AddToTail(newkv);
    }

    protected void InsertKeyValue(MDkeyvalue kv)
    {
        m_KeyValues.AddToTail(kv);
    }
}

public class WCKVBase_Dict
{
    protected CUtlDict<MDkeyvalue, ushort> m_KeyValues;

    public int GetFirst()
    {
        return m_KeyValues.First();
    }

    public int GetNext(int i)
    {
        return m_KeyValues.Next(i);
    }

    public static int GetInvalidIndex()
    {
        return CUtlDict<MDkeyvalue, ushort>.InvalidIndex();
    }

    public int FindByKeyName(string pKeyName)
    {
        return m_KeyValues.Find(pKeyName);
    }

    public void RemoveKeyAt(int nIndex)
    {
        m_KeyValues.RemoveAt(nIndex);
    }

    protected void InsertKeyValue(MDkeyvalue kv)
    {
        m_KeyValues.Insert(kv.szKey, kv);
    }
}

public class WCKeyValuesT<TBase> where TBase : class
{
    public WCKeyValuesT()
    {

    }

    public void RemoveAll()
    {
        m_KeyValues.RemoveAll();   
    }

    public void RemoveKey(string pszKey)
    {
        SetValue(pszKey, null);
    }

    public void SetValue(string pszKey, string pszValue)
    {
        string szTmpKey = string.Empty;
        string szTmpValue = string.Empty;

        szTmpKey = pszKey;

        if (pszValue != null)
        {
            szTmpValue = pszValue;
        }
        else
        {
            szTmpValue = null;
        }

        wckeyvalues.StripEdgeWhiteSpace(szTmpKey);
        wckeyvalues.StripEdgeWhiteSpace(szTmpValue);

        int i = FindKeyByName(szTmpKey);

        if (i == GetInvalidIndex())
        {
            if (pszValue != null)
            {
                MDkeyvalue newkv = new();
                newkv.szKey = szTmpKey;
                newkv.szValue = szTmpValue;
                InsertKeyValue(newkv);
            }
        }
        else
        {
            if (pszValue != null)
            {
                m_KeyValues[i].szValue = szTmpValue;
            }
            else
            {
                RemoveKeyAt(i);
            }
        }
    }

    public void SetValue(string pszKey, int iValue)
    {
        string szValue = string.Empty;
        itoa(iValue, szValue, 10);

        SetValue(pszKey, szValue);
    }

    public string GetKey(int nIndex)
    {
        return m_KeyValues.Element(nIndex).szKey;
    }

    public MDkeyvalue GetKeyValue(int nIndex)
    {
        return m_KeyValues.Element(nIndex);
    }

    public string GetValue(int nIndex)
    {
        return m_KeyValues.Element(nIndex).szValue;
    }

    public string GetValue(string pszKey, int piIndex = 0)
    {
        int i = FindByKeyName(pszKey);

        if (i == GetInvalidIndex())
        {
            return null;
        }
        else
        {
            if (piIndex != 0)
            {
                piIndex = i;
            }

            return m_KeyValues[i].szValue;
        }
    }
}