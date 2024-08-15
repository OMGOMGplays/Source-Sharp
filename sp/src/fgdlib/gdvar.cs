namespace FGDLib;

public enum GDIV_TYPE
{
    ivBadType = -1,
    ivAngle,
    ivTargetDest,
    ivTargetNameOrClass,
    ivTargetSrc,
    ivInteger,
    ivString,
    ivChoices,
    ivFlags,
    ivDecal,
    ivColor255,
    ivColor1,
    ivStudioModel,
    ivSprite,
    ivSound,
    ivVector,
    ivNPCClass,
    ivFilterClass,
    ivFloat,
    ivMaterial,
    ivScene,
    ivSide,
    ivSideList,
    ivOrigin,
    ivVecLine,
    ivAxis,
    ivPointEntityClass,
    ivNodeDest,
    ivInstanceFile,
    ivAngleNegativePitch,
    ivInstanceVariable,
    ivInstanceParm,

    ivMax
}

public struct GDIVITEM
{
    public ulong iValue { get; set; }
    public string szValue { get; set; }
    public string szCaption { get; set; }
    public bool bDefault { get; set; }
}

public struct TypeMap_t
{
    public GDIV_TYPE eType { get; set; }
    public string pszName { get; set; }
    public trtoken_t eStoreAs { get; set; }
}

public class GDinputvariable
{
    public static TypeMap_t[] TypeMap =
    {
        new TypeMap_t { eType = GDIV_TYPE.ivAngle,                  pszName = "angle",                eStoreAs = trtoken_t.STRING  },
        new TypeMap_t { eType = GDIV_TYPE.ivChoices,                pszName = "choices",              eStoreAs = trtoken_t.STRING  },
        new TypeMap_t { eType = GDIV_TYPE.ivColor1,                 pszName = "color1",               eStoreAs = trtoken_t.STRING  },
        new TypeMap_t { eType = GDIV_TYPE.ivColor255,               pszName = "color255",             eStoreAs = trtoken_t.STRING  },
        new TypeMap_t { eType = GDIV_TYPE.ivDecal,                  pszName = "decal",                eStoreAs = trtoken_t.STRING  },
        new TypeMap_t { eType = GDIV_TYPE.ivFlags,                  pszName = "flags",                eStoreAs = trtoken_t.INTEGER },
        new TypeMap_t { eType = GDIV_TYPE.ivInteger,                pszName = "integer",              eStoreAs = trtoken_t.INTEGER },
        new TypeMap_t { eType = GDIV_TYPE.ivSound,                  pszName = "sound",                eStoreAs = trtoken_t.STRING  },
        new TypeMap_t { eType = GDIV_TYPE.ivSprite,                 pszName = "sprite",               eStoreAs = trtoken_t.STRING  },
        new TypeMap_t { eType = GDIV_TYPE.ivStudioModel,            pszName = "studio",               eStoreAs = trtoken_t.STRING  },
        new TypeMap_t { eType = GDIV_TYPE.ivTargetDest,             pszName = "target_destination",   eStoreAs = trtoken_t.STRING  },
        new TypeMap_t { eType = GDIV_TYPE.ivTargetSrc,              pszName = "target_source",        eStoreAs = trtoken_t.STRING  },
        new TypeMap_t { eType = GDIV_TYPE.ivTargetNameOrClass,      pszName = "target_name_or_class", eStoreAs = trtoken_t.STRING  },
        new TypeMap_t { eType = GDIV_TYPE.ivVector,                 pszName = "vector",               eStoreAs = trtoken_t.STRING  },
        new TypeMap_t { eType = GDIV_TYPE.ivNPCClass,               pszName = "npcclass",             eStoreAs = trtoken_t.STRING  },
        new TypeMap_t { eType = GDIV_TYPE.ivFilterClass,            pszName = "filterclass",          eStoreAs = trtoken_t.STRING  },
        new TypeMap_t { eType = GDIV_TYPE.ivFloat,                  pszName = "float",                eStoreAs = trtoken_t.STRING  },
        new TypeMap_t { eType = GDIV_TYPE.ivMaterial,               pszName = "material",             eStoreAs = trtoken_t.STRING  },
        new TypeMap_t { eType = GDIV_TYPE.ivScene,                  pszName = "scene",                eStoreAs = trtoken_t.STRING  },
        new TypeMap_t { eType = GDIV_TYPE.ivSide,                   pszName = "side",                 eStoreAs = trtoken_t.STRING  },
        new TypeMap_t { eType = GDIV_TYPE.ivSideList,               pszName = "sidelist",             eStoreAs = trtoken_t.STRING  },
        new TypeMap_t { eType = GDIV_TYPE.ivOrigin,                 pszName = "origin",               eStoreAs = trtoken_t.STRING  },
        new TypeMap_t { eType = GDIV_TYPE.ivAxis,                   pszName = "axis",                 eStoreAs = trtoken_t.STRING  },
        new TypeMap_t { eType = GDIV_TYPE.ivVecLine,                pszName = "vecline",              eStoreAs = trtoken_t.STRING  },
        new TypeMap_t { eType = GDIV_TYPE.ivPointEntityClass,       pszName = "pointentityclass",     eStoreAs = trtoken_t.STRING  },
        new TypeMap_t { eType = GDIV_TYPE.ivNodeDest,               pszName = "node_dest",            eStoreAs = trtoken_t.INTEGER },
        new TypeMap_t { eType = GDIV_TYPE.ivInstanceFile,           pszName = "instance_file",        eStoreAs = trtoken_t.STRING  },
        new TypeMap_t { eType = GDIV_TYPE.ivAngleNegativePitch,     pszName = "angle_negative_pitch", eStoreAs = trtoken_t.STRING  },
        new TypeMap_t { eType = GDIV_TYPE.ivInstanceVariable,       pszName = "instance_variable",    eStoreAs = trtoken_t.STRING  },
        new TypeMap_t { eType = GDIV_TYPE.ivInstanceParm,           pszName = "instance_parm",        eStoreAs = trtoken_t.STRING  },
    };

    private CUtlVector<GDIVITEM> m_Items;

    private static string m_pszEmpty = string.Empty;

    private string m_szName;
    private string m_szLongName;
    private string m_pszDescription;

    private GDIV_TYPE m_eType;

    private int m_nDefault;
    private string m_szDefault;

    private int m_nValue;
    private string m_szValue;

    private bool m_bReportable;
    private bool m_bReadOnly;

    public GDinputvariable()
    {
        m_szDefault = "\0";
        m_nDefault = 0;
        m_szValue = "\0";
        m_bReportable = false;
        m_bReadOnly = false;
        m_pszDescription = null;
    }

    public GDinputvariable(string szType, string szName)
    {
        m_szDefault = "\0";
        m_nDefault = 0;
        m_szValue = "\0";
        m_bReportable = false;
        m_bReadOnly = false;
        m_pszDescription = null;

        m_eType = GetTypeFromToken(szType);
        m_szName = szName;
    }

    public bool InitFromTokens(TokenReader tr)
    {
        string szToken = string.Empty;

        if (!gamedata.GDGetToken(tr, m_szName, m_szName.Length, trtoken_t.IDENT))
        {
            return false;
        }

        if (!gamedata.GDSkipToken(tr, trtoken_t.OPERATOR, "("))
        {
            return false;
        }

        trtoken_t ttype = tr.NextToken(szToken, szToken.Length);

        if (ttype == trtoken_t.OPERATOR)
        {
            if (string.Compare(szToken.ToLower(), "*") == 0)
            {
                m_bReportable = true;
            }
        }
        else
        {
            tr.Stuff(ttype, szToken);
        }

        if (!gamedata.GDGetToken(tr, szToken, szToken.Length, trtoken_t.IDENT))
        {
            return false;
        }

        if (!gamedata.GDSkipToken(tr, trtoken_t.OPERATOR, ")"))
        {
            return false;
        }

        m_eType = GetTypeFromToken(szToken);

        if (m_eType == GDIV_TYPE.ivBadType)
        {
            gamedata.GDError(tr, "'{0}' is not a valid variable type", szToken);
            return false;
        }

        ttype = tr.PeekTokenType(szToken, szToken.Length);

        if (ttype == trtoken_t.IDENT && tokenreader.IsToken(szToken, "readonly"))
        {
            tr.NextToken(szToken, szToken.Length);
            m_bReadOnly = true;

            ttype = tr.PeekTokenType(szToken, szToken.Length);
        }

        if (ttype == trtoken_t.OPERATOR && tokenreader.IsToken(szToken, ":"))
        {
            tr.NextToken(szToken, szToken.Length);

            if (m_eType == GDIV_TYPE.ivFlags)
            {
                gamedata.GDError(tr, "flag sets do not have long names");
                return false;
            }

            if (!gamedata.GDGetToken(tr, m_szLongName, m_szLongName.Length, trtoken_t.STRING))
            {
                return false;
            }

            ttype = tr.PeekTokenType(szToken, szToken.Length);

            if (ttype == trtoken_t.OPERATOR && tokenreader.IsToken(szToken, ":"))
            {
                tr.NextToken(szToken, szToken.Length);

                ttype = tr.PeekTokenType(szToken, szToken.Length);

                if (ttype == trtoken_t.OPERATOR && tokenreader.IsToken(szToken, ":"))
                {

                }
                else
                {
                    trtoken_t eStoreAs = GetStoreAsFromType(m_eType);

                    if (eStoreAs == trtoken_t.STRING)
                    {
                        if (!gamedata.GDGetToken(tr, m_szDefault, m_szDefault.Length, trtoken_t.STRING))
                        {
                            return false;
                        }
                    }
                    else if (eStoreAs == trtoken_t.INTEGER)
                    {
                        if (!gamedata.GDGetToken(tr, szToken, szToken.Length, trtoken_t.INTEGER))
                        {
                            return false;
                        }

                        m_nDefault = atoi(szToken);
                    }

                    ttype = tr.PeekTokenType(szToken, szToken.Length);
                }
            }

            if (ttype == trtoken_t.OPERATOR && tokenreader.IsToken(szToken, ":"))
            {
                tr.NextToken(szToken, szToken.Length);

                if (m_pszDescription != null)
                {
                    m_pszDescription = null;
                }

                if (!gamedata.GDGetTokenDynamic(tr, m_pszDescription, trtoken_t.STRING))
                {
                    return false;
                }

                ttype = tr.PeekTokenType(szToken, szToken.Length);
            }
        }
        else
        {
            m_szLongName = m_szName;
        }

        if ((ttype == trtoken_t.OPERATOR && tokenreader.IsToken(szToken, "]")) || ttype != trtoken_t.OPERATOR)
        {
            if (m_eType == GDIV_TYPE.ivFlags || m_eType == GDIV_TYPE.ivChoices)
            {
                gamedata.GDError(tr, "no {0} specified", m_eType == GDIV_TYPE.ivFlags ? "flags" : "choices");
                return false;
            }

            return true;
        }

        if (!gamedata.GDSkipToken(tr, trtoken_t.OPERATOR, "="))
        {
            return false;
        }

        if (m_eType == GDIV_TYPE.ivFlags && m_eType != GDIV_TYPE.ivChoices)
        {
            gamedata.GDError(tr, "didn't expect '=' here");
            return false;
        }

        if (!gamedata.GDSkipToken(tr, trtoken_t.OPERATOR, "["))
        {
            return false;
        }

        if (m_eType == GDIV_TYPE.ivFlags)
        {
            GDIVITEM ivi = new();

            while (true)
            {
                ttype = tr.PeekTokenType();

                if (ttype != trtoken_t.INTEGER)
                {
                    break;
                }

                gamedata.GDGetToken(tr, szToken, szToken.Length, trtoken_t.INTEGER);
                szToken = string.Format(szToken + "{0}", ivi.iValue);

                if (!gamedata.GDSkipToken(tr, trtoken_t.OPERATOR, ":"))
                {
                    return false;
                }

                if (!gamedata.GDGetToken(tr, szToken, szToken.Length, trtoken_t.STRING))
                {
                    return false;
                }

                ivi.szCaption = szToken;

                if (!gamedata.GDSkipToken(tr, trtoken_t.OPERATOR, ":"))
                {
                    return false;
                }

                if (!gamedata.GDGetToken(tr, szToken, szToken.Length, trtoken_t.INTEGER))
                {
                    return false;
                }

                ivi.bDefault = atoi(szToken) == 0 ? true : false;

                m_Items.AddToTail(ivi);
            }

            ulong nDefault = 0;

            for (int i = 0; i < m_Items.Count(); i++)
            {
                if (m_Items[i].bDefault)
                {
                    nDefault |= m_Items[i].iValue;
                }
            }

            m_nDefault = (int)nDefault;
            m_szDefault = string.Format(m_szDefault + "{0} {1}", m_szDefault.Length, m_nDefault);
        }
        else if(m_eType == GDIV_TYPE.ivChoices)
        {
            GDIVITEM ivi = new();

            while (true)
            {
                ttype = tr.PeekTokenType();

                if ((ttype != trtoken_t.INTEGER) && (ttype != trtoken_t.STRING))
                {
                    break;
                }

                gamedata.GDGetToken(tr, szToken, szToken.Length, ttype);
                ivi.iValue = 0;
                ivi.szValue = szToken;

                if (!gamedata.GDSkipToken(tr, trtoken_t.OPERATOR, ":"))
                {
                    return false;
                }

                if (!gamedata.GDGetToken(tr, szToken, szToken.Length, trtoken_t.STRING))
                {
                    return false;
                }

                ivi.szCaption = szToken;

                m_Items.AddToTail(ivi);
            }
        }

        if (!gamedata.GDSkipToken(tr, trtoken_t.OPERATOR, "]"))
        {
            return false;
        }

        return true;
    }

    public string GetName()
    {
        return m_szName;
    }

    public string GetLongName()
    {
        return m_szLongName;
    }

    public string GetDescription()
    {
        if (m_pszDescription != null)
        {
            return m_pszDescription;
        }

        return m_pszEmpty;
    }

    public int GetFlagCount()
    {
        return m_Items.Count();
    }

    public int GetFlagMask(int nFlag)
    {
        Debug.Assert(m_eType == GDIV_TYPE.ivFlags);
        return m_Items.Element(nFlag).iValue;
    }

    public string GetFlagCaption(int nFlag)
    {
        Debug.Assert(m_eType == GDIV_TYPE.ivFlags);
        return m_Items.Element(nFlag).szCaption;
    }

    public int GetChoiceCount()
    {
        return m_Items.Count();
    }

    public string GetChoiceCaption(int nChoice)
    {
        Debug.Assert(m_eType == GDIV_TYPE.ivChoices);
        return m_Items.Element(nChoice).szCaption;
    }

    public GDIV_TYPE GetType()
    {
        return m_eType;
    }

    public string GetTypeText()
    {
        for (int i = 0; i < Marshal.SizeOf(TypeMap) / Marshal.SizeOf(TypeMap[0]); i++)
        {
            if (TypeMap[i].eType == m_eType)
            {
                return TypeMap[i].pszName;
            }
        }

        return "unknown";
    }

    public void GetDefault(out int pnStore)
    {
        pnStore = m_nDefault;
    }

    public void GetDefault(out string pszStore)
    {
        pszStore = m_szDefault;
    }

    public GDIV_TYPE GetTypeFromToken(string pszToken)
    {
        for (int i = 0; i < Marshal.SizeOf(TypeMap) / Marshal.SizeOf(TypeMap[0]); i++)
        {
            if (tokenreader.IsToken(pszToken, TypeMap[i].pszName))
            {
                return TypeMap[i].eType;
            }
        }

        return GDIV_TYPE.ivBadType;
    }

    public trtoken_t GetStoreAsFromType(GDIV_TYPE eType)
    {
        for (int i = 0; i < Marshal.SizeOf(TypeMap) / Marshal.SizeOf(TypeMap[0]); i++)
        {
            if (TypeMap[i].eType == eType)
            {
                return TypeMap[i].eStoreAs;
            }
        }

        Debug.Assert(false);
        return trtoken_t.STRING;
    }

    public string ItemStringForValue(string szValue)
    {
        int nCount = m_Items.Count();

        for (int i = 0; i < nCount; i++)
        {
            if (string.Compare(m_Items[i].szValue.ToLower(), szValue.ToLower()) == 0)
            {
                return m_Items[i].szCaption;
            }
        }

        return null;
    }

    public string ItemValueForString(string szString)
    {
        int nCount = m_Items.Count();

        for (int i = 0; i < nCount; i++)
        {
            if (string.Compare(m_Items[i].szCaption, szString))
            {
                return m_Items[i].szValue;
            }
        }

        return null;
    }

    public bool IsFlagSet(uint uCheck)
    {
        Debug.Assert(m_eType == GDIV_TYPE.ivFlags);
        return ((uint)m_nValue & uCheck) == uCheck ? true : false;
    }

    public void SetFlag(uint uFlags, bool bSet)
    {
        Debug.Assert(m_eType == GDIV_TYPE.ivFlags);

        if (bSet)
        {
            m_nValue |= (int)uFlags;
        }
        else
        {
            m_nValue &= ~(int)uFlags;
        }
    }

    public void ResetDefaults()
    {
        if (m_eType == GDIV_TYPE.ivFlags)
        {
            m_nValue = 0;

            int nCount = m_Items.Count();

            for (int i = 0; i < nCount; i++)
            {
                if (m_Items[i].bDefault)
                {
                    m_nValue |= GetFlagMask(i);
                }
            }
        }
        else
        {
            m_nValue = m_nDefault;
            m_szValue = m_szDefault;
        }
    }

    public void ToKeyValue(MDkeyvalue pkv)
    {
        pkv.szValue = m_szName;

        trtoken_t eStoreAs = GetStoreAsFromType(m_eType);

        if (eStoreAs == trtoken_t.STRING)
        {
            pkv.szValue = m_szValue;
        }
        else if (eStoreAs == trtoken_t.INTEGER)
        {
            itoa(m_nValue, pkv.szValue, 10);
        }
    }

    public void FromKeyValue(MDkeyvalue pkv)
    {
        trtoken_t eStoreAs = GetStoreAsFromType(m_eType);

        if (eStoreAs == trtoken_t.STRING)
        {
            m_szValue = pkv.szValue;
        }
        else if (eStoreAs == trtoken_t.INTEGER)
        {
            m_nValue = atoi(pkv.szValue);
        }
    }

    public bool IsReportable()
    {
        return m_bReportable;
    }

    public bool IsReadOnly()
    {
        return m_bReadOnly;
    }

    public static GDinputvariable Equals(GDinputvariable in1, GDinputvariable in2)
    {
        in1.m_eType = in2.GetType();
        in1.m_szName = in2.m_szName;
        in1.m_szLongName = in2.m_szLongName;
        in1.m_szDefault = in2.m_szDefault;

        in1.m_pszDescription = null;

        if (in2.m_pszDescription != null)
        {
            in1.m_pszDescription = in2.m_pszDescription;
        }
        else
        {
            in1.m_pszDescription = null;
        }

        in1.m_nDefault = in2.m_nDefault;
        in1.m_bReportable = in2.m_bReportable;
        in1.m_bReadOnly = in2.m_bReadOnly;

        in1.m_Items.RemoveAll();

        int nCount = in2.m_Items.Count();

        for (int i = 0; i < nCount; i++)
        {
            in1.m_Items.AddToTail(in2.m_Items.Element(i));
        }

        return in1;
    }

    public void Merge(GDinputvariable other)
    {
        if (other.GetType() != GetType())
        {
            return;
        }

        bool bFound = false;
        int nOurItems = m_Items.Count();

        for (int i = 0; i < other.m_Items.Count(); i++)
        {
            GDIVITEM theirItem = other.m_Items[i];

            for (int j = 0; j < nOurItems; j++)
            {
                GDIVITEM ourItem = m_Items[j];

                if (theirItem.iValue == ourItem.iValue)
                {
                    bFound = true;
                    break;
                }
            }

            if (!bFound)
            {
                m_Items.AddToTail(theirItem);
            }
        }
    }

    public static string GetVarTypeName(GDIV_TYPE eType)
    {
        return TypeMap[(int)eType].pszName;
    }
}