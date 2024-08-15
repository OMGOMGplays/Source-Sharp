namespace FGDLib;

public class gamedata
{
    public static object GameDataMessageFunc_t(int level, PRINTF_FORMAT_STRING fmt, params object[] args)
    {
        return null;
    }

    public const int MAX_DIRECTORY_SIZE = 32;

    public static void GDSetMessageFunc(GameDataMessageFunc_t pFunc)
    {
        g_pMsgFunc = pFunc;
    }

    public static bool GDError(TokenReader tr, PRINTF_FORMAT_STRING error, params object[] args)
    {
        string szBuf;
        Console.WriteLine(string.Format(error, args));
        szBuf = string.Format(error, args);

        if (g_pMsgFunc != null)
        {
            g_pMsgFunc(1, tr.Error(szBuf));
        }

        if (tr.GetErrorCount() >= MAX_ERRORS)
        {
            if (g_pMsgFunc != null)
            {
                g_pMsgFunc(1, " - too many errors; aborting.");
            }

            return false;
        }

        return true;
    }

    public static bool GDSkipToken(TokenReader tr, trtoken_t ttexpecting = trtoken_t.TOKENNONE, string pszExpecting)
    {
        string szDiscardBuf = null;
        string pszDiscardBuf = szDiscardBuf;
        return DoGetToken(tr, pszDiscardBuf, szDiscardBuf.Length, ttexpecting, pszExpecting);
    }

    public static bool GDGetToken(TokenReader tr, string pszStore, int nSize, trtoken_t ttexpecting = trtoken_t.TOKENNONE, string pszExpecting = null)
    {
        Assert(cond: pszStore != null);

        if (pszStore != null)
        {
            return DoGetToken(tr, pszStore, nSize, ttexpecting, pszExpecting);
        }

        return false;
    }

    public static bool GDGetTokenDynamic(TokenReader tr, string ppszStore, trtoken_t ttexpecting, string pszExpecting = null)
    {
        if (ppszStore == null)
        {
            return false;
        }

        ppszStore = null;
        return DoGetToken(tr, ppszStore, -1, ttexpecting, pszExpecting);
    }

    // gamedata.cpp

    public const int MAX_ERRORS = 5;

    public static GameDataMessageFunc_t g_pMsgFunc = null;

    public static bool DoGetToken(TokenReader tr, string ppszStore, int nSize, trtoken_t ttexpecting, string pszExpecting)
    {
        trtoken_t ttype;

        if (ppszStore != null)
        {
            ttype = tr.NextToken(ppszStore, nSize);
        }
        else
        {
            ttype = tr.NextTokenDynamic(ppszStore);
        }

        if (ttype == trtoken_t.TOKENSTRINGTOOLONG)
        {
            GDError(tr, "unterminated string or string too long");
            return false;
        }

        string pszStore = ppszStore;
        bool bBadTokenType = false;

        if ((ttype != ttexpecting) && (ttexpecting != trtoken_t.TOKENNONE))
        {
            if (!((ttexpecting == trtoken_t.STRING) && (ttype == trtoken_t.INTEGER)))
            {
                bBadTokenType = true;
            }
        }

        if (bBadTokenType && (pszExpecting == null))
        {
            string pszTokenName;

            switch (ttexpecting)
            {
                case trtoken_t.IDENT:
                    pszTokenName = "identifier";
                    break;

                case trtoken_t.INTEGER:
                    pszTokenName = "integer";
                    break;

                case trtoken_t.STRING:
                    pszTokenName = "string";
                    break;

                case trtoken_t.OPERATOR:
                default:
                    pszTokenName = "symbol";
                    break;
            }

            GDError(tr, "expecting {0}", pszTokenName);
            return false;
        }
        else if (bBadTokenType || (pszExpecting != null) && !IsToken(pszStore, pszExpecting))
        {
            GDError(tr, "expecting '{0}', but found '{1}'", pszExpecting, pszStore);
            return false;
        }

        return true;
    }

    public static string[] RequiredKeys =
    {
        "Origin",
        "Angles",
        null
    };

    public static CUtlMapz<GDIV_TYPE, tRemapOperation> RemapOperation;

    public static bool CUtlType_LessThan(GDIV_TYPE type1, GDIV_TYPE type2)
    {
        return type1 < type2;
    }
}

public struct FGDMatExclusions_s
{
    public string szDirectory;
    public bool bUserGenerated;
}

public struct FGDVisGroupsBaseClass_s
{
    public string szClass;
    public CUtlStringList szEntities;
}

public struct FGDAutoVisGroups_s
{
    public string szParent;
    public CUtlVector<FGDVisGroupsBaseClass_s> m_Classes;
}

public enum tRemapOperation
{
    REMAP_NAME = 0,
    REMAP_POSITION,
    REMAP_ANGLE,
    REMAP_ANGLE_NEGATIVE_PITCH,
}

public class GameData
{
    public enum TNameFixup_e
    {
        NAME_FIXUP_PREFIX = 0,
        NAME_FIXUP_POSTFIX,
        NAME_FIXUP_NONE,
    }

    public GameData()
    {
        m_nMaxMapCoord = 8192;
        m_nMinMapCoord = -8192;
        m_InstanceClass = null;
    }

    ~GameData()
    {
        ClearData();
    }

    public bool Load(string pszFilename)
    {
        TokenReader tr = new();

        if (GetFileAttributes(pszFilename) == 0xffffffff)
        {
            return false;
        }

        if (!tr.Open(pszFilename))
        {
            return false;
        }

        trtoken_t ttype;
        string szToken = string.Empty;

        while (true)
        {
            if (tr.GetErrorCount() >= MAX_ERRORS)
            {
                break;
            }

            ttype = tr.NextToken(szToken, szToken.Length);

            if (ttype == trtoken_t.TOKENEOF)
            {
                break;
            }

            if (ttype != trtoken_t.OPERATOR || !IsToken(szToken, "@"))
            {
                if (!gamedata.GDError(tr, "expected @"))
                {
                    return false;
                }
            }

            if (tr.NextToken(szToken, szToken.Length) != trtoken_t.IDENT)
            {
                if (!gamedata.GDError(tr, "expected identifier after @"))
                {
                    return false;
                }
            }

            if (IsToken(szToken, "baseclass") || IsToken(szToken, "pointclass") || IsToken(szToken, "solidclass") || IsToken(szToken, "keyframeclass") ||
                IsToken(szToken, "moveclass") || IsToken(szToken, "npcclass") || IsToken(szToken, "filterclass"))
            {
                GDclass pNewClass = new();

                if (!pNewClass.InitFromTokens(tr, this))
                {
                    tr.IgnoreTill(trtoken_t.OPERATOR, "@");
                    pNewClass = null;
                }
                else
                {
                    if (IsToken(szToken, "baseclass"))
                    {
                        pNewClass.SetBaseClass(true);
                    }
                    else if (IsToken(szToken, "pointclass"))
                    {
                        pNewClass.SetPointClass(true);
                    }
                    else if (IsToken(szToken, "solidclass"))
                    {
                        pNewClass.SetSolidClass(true);
                    }
                    else if (IsToken(szToken, "npcclass"))
                    {
                        pNewClass.SetPointClass(true);
                        pNewClass.SetNPCClass(true);
                    }
                    else if (IsToken(szToken, "filterclass"))
                    {
                        pNewClass.SetPointClass(true);
                        pNewClass.SetFilterClass(true);
                    }
                    else if (IsToken(szToken, "moveclass"))
                    {
                        pNewClass.SetPointClass(true);
                        pNewClass.SetMoveClass(true);
                    }
                    else if (IsToken(szToken, "keyframeclass"))
                    {
                        pNewClass.SetKeyFrameClass(true);
                        pNewClass.SetPointClass(true);
                    }

                    int nExistingClassIndex = 0;
                    GDclass pExistingClass = ClassForName(pNewClass.GetName(), nExistingClassIndex);

                    if (pExistingClass != null)
                    {
                        m_Classes.InsertAfter(nExistingClassIndex, pNewClass);
                        m_Classes.Remove(nExistingClassIndex);
                    }
                    else
                    {
                        m_Classes.AddToTail(pNewClass);
                    }
                }
            }
            else if (IsToken(szToken, "include"))
            {
                if (GDGetToken(tr, szToken, szToken.Length, trtoken_t.STRING))
                {
                    string justPath = null, loadFilename = null;

                    if (Q_ExtractFilePath(pszFilename, justPath, justPath.Length))
                    {
                        Q_snprintf(loadFilename, loadFilename.Length, "{0}{1}", justPath, szToken);
                    }
                    else
                    {
                        Q_strncpy(loadFilename, szToken, loadFilename.Length);
                    }

                    if (!Load(loadFilename))
                    {
                        if (!Load(szToken))
                        {
                            GDError(tr, "error including file: {0}", szToken);
                        }
                    }
                }
            }
            else if (IsToken(szToken, "mapsize"))
            {
                if (!ParseMapSize(tr))
                {
                    tr.IgnoreTill(trtoken_t.OPERATOR, "@");
                }
            }
            else if (IsToken(szToken, "materialexclusion"))
            {
                if (!LoadFGDMaterialExclusions(tr))
                {
                    tr.IgnoreTill(trtoken_t.OPERATOR, "@");
                }
            }
            else if (IsToken(szToken, "autovisgroup"))
            {
                if (!LoadFGDAutoVisGroups(tr))
                {
                    tr.IgnoreTill(trtoken_t.OPERATOR, "@");
                }
            }
            else
            {
                GDError(tr, "unrecognized section name {0}", szToken);
                tr.IgnoreTill(trtoken_t.OPERATOR, "@");
            }
        }

        if (tr.GetErrorCount > 0)
        {
            return false;
        }

        tr.Close();

        return true;
    }

    public GDclass ClassForName(string pszName, int[] piIndex = null)
    {
        int nCount = m_Classes.Count();

        for (int i = 0; i < nCount; i++)
        {
            GDclass mp = m_Classes.Element(i);

            if (string.Compare(mp.GetName(), pszName) == 0)
            {
                if (piIndex != null)
                {
                    piIndex[0] = i;
                }

                return mp;
            }
        }

        return null;
    }

    public void ClearData()
    {
        int nCount = m_Classes.Count();

        for (int i = 0; i < nCount; i++)
        {
            GDclass pm = m_Classes.Element(i);
            pm = null;
        }

        m_Classes.RemoveAll();
    }

    public int GetMaxMapCoord()
    {
        return m_nMaxMapCoord;
    }

    public int GetMinMapCoord()
    {
        return m_nMinMapCoord;
    }

    public int GetClassCount()
    {
        return m_Classes.Count();
    }

    public GDclass GetClass(int nIndex)
    {
        if (nIndex >= m_Classes.Count())
        {
            return null;
        }

        return m_Classes.Element(nIndex);
    }

    public GDclass BeginInstanceRemap(string pszClassName, string pszInstancePrefix, Vector origin, QAngle angle)
    {
        m_InstanceOrigin = origin;
        m_InstanceAngle = angle;
        AngleMatrix(m_InstanceAngle, m_InstanceOrigin, m_InstanceMat);

        m_InstancePrefix = pszInstancePrefix;

        if (m_InstanceClass != null)
        {
            m_InstanceClass = null;
        }

        if (string.Compare(pszClassName.ToLower(), "info_overlay_accessor") == 0)
        {
            pszClassName = "info_overlay";
        }

        GDclass BaseClass = ClassForName(pszClassName);

        if (BaseClass != null)
        {
            m_InstanceClass = new();
            m_InstanceClass.Parent = this;
            m_InstanceClass.AddBase(BaseClass);

            for (int i = 0; RequiredKeys[i] != null; i++)
            {
                if (m_InstanceClass.VarForName(RequiredKeys[i]) == null)
                {
                    BaseClass = ClassForName(RequiredKeys[i]);

                    if (BaseClass != null)
                    {
                        m_InstanceClass.AddBase(BaseClass);
                    }
                }
            }
        }
        else
        {
            m_InstanceClass = null;
        }
        return m_InstanceClass;
    }

    public bool RemapKeyValue(string pszKey, string pszInValue, out string pszOutValue, TNameFixup nameFixup)
    {
        if (RemapOperation.Count() == 0)
        {
            RemapOperation.SetLessFunc(CUtlType_LessThan);
            RemapOperation.Insert(GDIV_TYPE.ivAngle, tRemapOperation.REMAP_ANGLE);
            RemapOperation.Insert(GDIV_TYPE.ivTargetDesc, tRemapOperation.REMAP_NAME);
            RemapOperation.Insert(GDIV_TYPE.ivTargetSrc, tRemapOperation.REMAP_NAME);
            RemapOperation.Insert(GDIV_TYPE.ivOrigin, tRemapOperation.REMAP_POSITION);
            RemapOperation.Insert(GDIV_TYPE.ivAxis, tRemapOperation.REMAP_ANGLE);
            RemapOperation.Insert(GDIV_TYPE.ivAngleNegativePitch, tRemapOperation.REMAP_ANGLE_NEGATIVE_PITCH);
        }

        if (m_InstanceClass == null)
        {
            pszOutValue = null;
            return false;
        }

        GDinputvariable KVVar = m_InstanceClass.VarForName(pszKey);

        if (KVVar == null)
        {
            pszOutValue = null;
            return false;
        }

        GDIV_TYPE KVType = KVVar.GetType();
        int KVRemapIndex = RemapOperation.Find(KVType);

        if (KVRemapIndex == RemapOperation.InvalidIndex())
        {
            pszOutValue = null;
            return false;
        }

        pszOutValue = pszInValue; // strcpy(pszOutValue, pszInValue);

        switch (RemapOperation[KVRemapIndex])
        {
            case tRemapOperation.REMAP_NAME:
                if (KVType != GDIV_TYPE.ivInstanceVariable)
                {
                    RemapNameField(pszInValue, out pszOutValue, nameFixup);
                }
                break;

            case tRemapOperation.REMAP_POSITION:
                Vector inPoint = new(0.0f, 0.0f, 0.0f), outPoint;

                string[] positionParts = pszInValue.Split(' ');
                if (positionParts.Length == 3)
                {
                    float.TryParse(positionParts[0], out inPoint.x);
                    float.TryParse(positionParts[1], out inPoint.y);
                    float.TryParse(positionParts[2], out inPoint.z);
                }
                else
                {
                    throw new Exception("RemapKeyValue: parts.Length != 3");
                }

                VectorTransform(inPoint, m_InstanceMat, outPoint);

                string.Format(pszOutValue + "{0} {1} {2}", outPoint.x, outPoint.y, outPoint.z);

                break;

            case tRemapOperation.REMAP_ANGLE:
                if (m_InstanceAngle.x != 0.0f || m_InstanceAngle.y != 0.0f || m_InstanceAngle.z != 0.0f)
                {
                    QAngle inAngles = new(0.0f, 0.0f, 0.0f), outAngles;
                    matrix4x3 angToWorld, localMatrix;

                    string[] angleParts = pszInValue.Split(' ');
                    if (angleParts.Length == 3)
                    {
                        float.TryParse(angleParts[0], out inAngles.x);
                        float.TryParse(angleParts[1], out inAngles.y);
                        float.TryParse(angleParts[2], out inAngles.z);
                    }
                    else
                    {
                        throw new Exception("RemapKeyValue: parts.Length != 3");
                    }

                    AngleMatrix(inAngles, angToWorld);
                    MatrixMultiply(m_InstanceMat, angToWorld, localMatrix);
                    MatrixAngles(localMatrix, outAngles);

                    string.Format(pszOutValue + "{0} {1} {2}", outAngles.x, outAngles.y, outAngles.z);
                }

                break;

            case tRemapOperation.REMAP_ANGLE_NEGATIVE_PITCH:
                if (m_InstanceAngle.x != 0.0f || m_InstanceAngle.y != 0.0f || m_InstanceAngle.z != 0.0f)
                {
                    QAngle inAngles = new(0.0f, 0.0f, 0.0f), outAngles;
                    matrix3x4_t angToWorld, localMatrix;

                    float.TryParse(pszInValue, out inAngles.x);
                    inAngles.x = -inAngles.x;

                    AngleMatrix(inAngles, angToWorld);
                    MatrixMultiply(m_InstanceMat, angToWorld, localMatrix);
                    MatrixAngles(localMatrix, outAngles);

                    string.Format(pszOutValue + "{0}", -outAngles.x);
                }

                break;
        }

        return string.Compare(pszInValue.ToLower(), pszOutValue.ToLower()) != 0;
    }

    ///-----------------------------------------------------------------------------
    /// <summary>
    /// Purpose: this function will attempt to remap a name field. <br></br>
    /// <br></br>
    /// Input  : pszInvalue - the original value <br></br>
    ///			    AllowNameRemapping - only do name remapping if this parameter is true. <br></br>
    ///				this is generally only false on the instance level.<br></br>
    ///	<br></br>
    /// Output : returns true if the value changed<br></br>
    ///			pszOutValue - the new value if changed
    /// </summary>
    ///-----------------------------------------------------------------------------
    public bool RemapNameField(string pszInValue, out string pszOutValue, TNameFixup nameFixup)
    {
        pszOutValue = pszInValue;

        char[] pszInValue_Char = pszInValue.ToCharArray();

        if (pszInValue_Char[0] != '\0' && pszInValue_Char[0] != '@')
        {
            switch (nameFixup)
            {
                case TNameFixup.NAME_FIXUP_PREFIX:
                    string.Format(pszOutValue + "{0}-{1}", m_InstancePrefix, pszInValue);
                    break;

                case TNameFixup.NAME_FIXUP_POSTFIX:
                    string.Format(pszOutValue + "{0}-{1}", pszInValue, m_InstancePrefix);
                    break;
            }
        }

        return string.Compare(pszInValue.ToLower(), pszOutValue.ToLower()) != 0;
    }

    ///-----------------------------------------------------------------------------
    /// <summary>
    /// Purpose: Gathers any FGD-defined material directory exclusions <br></br>
    /// <br></br>
    /// Input  : <br></br>
    /// <br></br>
    /// Output : 
    /// </summary>
    ///-----------------------------------------------------------------------------
    public bool LoadFGDMaterialExclusions(TokenReader tr)
    {
        if (!GDSkipToken(tr, trtoken_t.OPERATOR, "["))
        {
            return false;
        }

        while (true)
        {
            string szToken = string.Empty;
            int szTokenSize = 128;
            bool bMatchFound = false;

            if (tr.PeekTokenType(szToken, szTokenSize) == trtoken_t.OPERATOR)
            {
                break;
            }
            else if (GDGetToken(tr, szToken, szTokenSize, trtoken_t.STRING))
            {
                for (int i = 0; i < m_FGDMaterialExclusions.Count(); i++)
                {
                    if (string.Compare(szToken.ToLower(), m_FGDMaterialExclusions[i].szDirectory.ToLower()) == 0)
                    {
                        bMatchFound = true;
                        break;
                    }
                }

                if (bMatchFound == false)
                {
                    int index = m_FGDMaterialExclusions.AddToTail();
                    m_FGDMaterialExclusions[index].szDirectory = szToken;
                    m_FGDMaterialExclusions[index].bUserGenerated = false;
                }
            }
        }

        if (!GDSkipToken(tr, trtoken_t.OPERATOR, "]"))
        {
            return false;
        }

        return true;
    }

    ///-----------------------------------------------------------------------------
    ///<summary>
    /// Purpose: Gathers any FGD-defined Auto VisGroups <br></br>
    /// <br></br>
    /// Input  : <br></br> 
    /// <br></br>
    /// Output :
    /// </summary>
    ///-----------------------------------------------------------------------------
    public bool LoadFGDAutoVisGroups(TokenReader tr)
    {
        int gindex = 0;
        int cindex = 0;

        string szToken = string.Empty;
        int szTokenSize = 128;

        if (GDSkipToken(tr, trtoken_t.OPERATOR, "="))
        {
            if (!GDGetToken(tr, szToken, szTokenSize, trtoken_t.STRING))
            {
                return false;
            }

            gindex = m_FGDAutoVisGroups.AddToTail();
            m_FGDAutoVisGroups[gindex].szParent = szToken;

            if (!GDSkipToken(tr, trtoken_t.OPERATOR, "["))
            {
                return false;
            }
        }

        while (true)
        {
            if (GDGetToken(tr, szToken, szTokenSize, trtoken_t.STRING))
            {
                cindex = m_FGDAutoVisGroups[gindex].m_Classes.AddToTail();
                m_FGDAutoVisGroups[gindex].m_Classes[cindex].szClass = szToken;

                if (!GDSkipToken(tr, trtoken_t.OPERATOR, "["))
                {
                    return false;
                }

                while (true)
                {
                    if (tr.PeekTokenType(szToken, szTokenSize) == trtoken_t.OPERATOR)
                    {
                        break;
                    }

                    if (!GDGetToken(tr, szToken, szTokenSize, trtoken_t.STRING))
                    {
                        return false;
                    }

                    m_FGDAutoVisGroups[gindex].m_Classes[cindex].szEntities.CopyAndAddToTail(szToken);
                }

                if (!GDSkipToken(tr, trtoken_t.OPERATOR, "]"))
                {
                    return false;
                }

                if (tr.PeekTokenType(szToken, szTokenSize) == trtoken_t.STRING)
                {
                    continue;
                }

                if (!GDSkipToken(tr, trtoken_t.OPERATOR, "]"))
                {
                    return false;
                }

                return true;
            }
            else
            {
                if (!GDSkipToken(tr, trtoken_t.OPERATOR, "]"))
                {
                    return false;
                }
            }
        }

        GDError(tr, "Malformed AutoVisGroup -- Last Processed: {0}", szToken);
        return false;
    }

    public CUtlVector<FGDMatExclusions_s> m_FGDMaterialExclusions;
    public CUtlVector<FGDAutoVisGroups_s> m_FGDAutoVisGroups;

    private bool ParseMapSize(TokenReader tr)
    {
        if (!GDSkipToken(tr, trtoken_t.OPERATOR, "("))
        {
            return false;
        }

        string szToken = null;

        if (!GDGetToken(tr, szToken, szToken.Length, trtoken_t.INTEGER))
        {
            return false;
        }

        int nMin = atoi(szToken);

        if (!GDSkipToken(tr, trtoken_t.OPERATOR, ","))
        {
            return false;
        }

        if (!GDGetToken(tr, szToken, szToken.Length, trtoken_t.INTEGER))
        {
            return false;
        }

        int nMax = atoi(szToken);

        if (nMin != nMax)
        {
            m_nMinMapCoord = min(nMin, nMax);
            m_nMaxMapCoord = max(nMin, nMax);
        }

        if (!GDSkipToken(tr, trtoken_t.OPERATOR, ")"))
        {
            return false;
        }

        return true;
    }

    private CUtlVector<GDclass> m_Classes;

    private int m_nMinMapCoord;
    private int m_nMaxMapCoord;

    private Vector m_InstanceOrigin;
    private QAngle m_InstanceAngle;
    private matrix4x3_t m_InstanceMat;
    private string m_InstancePrefix;
    private GDclass m_InstanceClass;
}