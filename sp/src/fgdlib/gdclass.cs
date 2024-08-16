namespace FGDLib;

public class gdclass
{
    public const int GD_MAX_VARIABLES = 128;

}

public class GDclass
{
    private color32 m_rgbColor;

    private bool m_bBase;
    private bool m_bSolid;
    private bool m_bModel;
    private bool m_bMove;
    private bool m_bKeyFrame;
    private bool m_bPoint;
    private bool m_bNPC;
    private bool m_bFilter;
    private bool m_bHalfGridSnap;

    private bool m_bGotSize;
    private bool m_bGotColor;

    private string m_szName;
    private string m_pszDescription;

    private CUtlVector<GDinputvariable> m_Variables;
    private int m_nVariables;
    private CUtlVector<GDclass> m_Bases;

    private CClassInputList m_Inputs;
    private CClassOutputList m_Outputs;

    private CHelperInfoList m_Helpers;

    private short[,] m_VariableMap = new short[GD_MAX_VARIABLES, 2];

    private Vector m_bmins;
    private Vector m_bmaxs;

    public GDclass()
    {
        m_nVariables = 0;
        m_bBase = false;
        m_bSolid = false;
        m_bBase = false;
        m_bModel = false;
        m_bMove = false;
        m_bKeyFrame = false;
        m_bPoint = false;
        m_bNPC = false;
        m_bFilter = false;

        m_bHalfGridSnap = false;

        m_bGotSize = false;
        m_bGotColor = false;

        m_rgbColor.r = 220;
        m_rgbColor.g = 30;
        m_rgbColor.b = 220;
        m_rgbColor.a = 0;

        m_pszDescription = null;

        for (int i = 0; i < 3; i++)
        {
            m_bmins[i] = -8;
            m_bmaxs[i] = 8;
        }
    }

    // Destructor might not be necessary?

    public string GetName()
    {
        return m_szName;
    }

    public string GetDescription()
    {
        if (m_pszDescription == null)
        {
            return m_szName;
        }

        return m_pszDescription;
    }

    public bool InitFromTokens(TokenReader tr, GameData pGD)
    {
        Parent = pGD;

        for (int i = 0; i < GD_MAX_VARIABLES; i++)
        {
            m_VariableMap[i, 0] = -1;
            m_VariableMap[i, 1] = -1;
        }

        if (!ParseSpecifiers(tr))
        {
            return false;
        }

        if (!gamedata.GDSkipToken(tr, trtoken_t.OPERATOR, "="))
        {
            return false;
        }

        string szToken = string.Empty;

        if ((tr.PeekTokenType(szToken, szToken.Length) == trtoken_t.OPERATOR) && tokenreader.IsToken(szToken, ":"))
        {
            tr.NextToken(szToken, szToken.Length);

            m_pszDescription = null;

            if (!gamedata.GDGetTokenDynamic(tr, m_pszDescription, trtoken_t.STRING))
            {
                return false;
            }
        }

        if (!gamedata.GDSkipToken(tr, trtoken_t.OPERATOR, "["))
        {
            return false;
        }

        if (!ParseVariables(tr))
        {
            return false;
        }

        if (!gamedata.GDSkipToken(tr, trtoken_t.OPERATOR, "]"))
        {
            return false;
        }

        return true;
    }

    public int GetVariableCount()
    {
        return m_nVariables;
    }

    public GDinputvariable GetVariableAt(int iIndex)
    {
        if (iIndex < 0 || iIndex >= m_nVariables)
        {
            return null;
        }

        if (m_VariableMap[iIndex, 0] == -1)
        {
            return m_Variables.Element(m_VariableMap[iIndex, 1]);
        }

        GDclass pVarClass = Parent.GetClass(m_VariableMap[iIndex, 0]);

        return pVarClass.GetVariableAt(m_VariableMap[iIndex, 1]);
    }

    public void GetHelperForGDVar(GDinputvariable pVar, out CUtlVector<string> pszHelperName)
    {
        string pszName = pVar.GetName();
        pszHelperName = new();

        for (int i = 0; i < GetHelperCount(); i++)
        {
            CHelperInfo pHelper = GetHelper(i);
            int nParamCount = pHelper.GetParameterCount();

            for (int j = 0; j < nParamCount; j++)
            {
                if (string.Compare(pszName.ToLower(), pHelper.GetParameter(j)) == 0)
                {
                    pszHelperName.AddToTail(pHelper.GetName());
                }
            }
        }
    }

    public GDinputvariable VarForName(string pszName, int piIndex = 0)
    {
        for (int i = 0; i < GetVariableCount(); i++)
        {
            GDinputvariable pVar = GetVariableAt(i);

            if (string.Compare(pVar.GetName().ToLower(), pszName.ToLower()) == 0)
            {
                if (piIndex != 0)
                {
                    piIndex = i;
                }

                return pVar;
            }
        }

        return null;
    }

    public bool AddVariable(GDinputvariable pVar, GDclass pBase, int iBaseIndex, int iVarIndex)
    {
        int iThisIndex = 0;
        GDinputvariable pThisVar = VarForName(pVar.GetName(), iThisIndex);

        if (pThisVar != null)
        {
            if (pThisVar.GetType() != pVar.GetType())
            {
                return false;
            }

            GDinputvariable pAddVar;
            bool bReturn;

            if (pVar.GetType() == GDIV_TYPE.ivFlags || pVar.GetType() == GDIV_TYPE.ivChoices)
            {
                GDinputvariable pNewVar = new GDinputvariable();

                pNewVar = pVar;
                pNewVar.Merge(pThisVar);

                pAddVar = pNewVar;
                bReturn = true;
            }
            else
            {
                pAddVar = pVar;
                bReturn = true;
            }

            if (m_VariableMap[iThisIndex, 0] == -1)
            {
                int nIndex = m_Variables.Find(pThisVar);
                Debug.Assert(nIndex != -1);
                pThisVar = null;

                m_Variables.Element(nIndex) = pAddVar;
            }
            else
            {
                m_VariableMap[iThisIndex, 0] = (short)iBaseIndex;

                if (iBaseIndex == -1)
                {
                    m_Variables.AddToTail(pAddVar);
                    m_VariableMap[iThisIndex, 1] = m_Variables.Count() - 1;
                }
                else
                {
                    m_VariableMap[iThisIndex, 1] = (short)iVarIndex;
                }
            }

            return bReturn;
        }

        if (iBaseIndex == -1)
        {
            m_Variables.AddToTail(pVar);
        }

        if (m_nVariables == GD_MAX_VARIABLES)
        {
            return false;
        }

        m_VariableMap[m_nVariables, 0] = (short)iBaseIndex;
        m_VariableMap[m_nVariables, 1] = (short)iVarIndex;
        ++m_nVariables;

        return true;
    }

    public void AddBase(GDclass pBase)
    {
        int[] iBaseIndex = new int[128];
        Parent.ClassForName(pBase.GetName(), iBaseIndex);

        for (int i = 0; i < pBase.GetVariableCount(); i++)
        {
            GDinputvariable pVar = pBase.GetVariableAt(i);
            AddVariable(pVar, pBase, iBaseIndex[0], i);
        }

        int nCount = pBase.GetInputCount();

        for (int i = 0; i < nCount; i++)
        {
            CClassInput pInput = pBase.GetInput(i);

            CClassInput pNew = new CClassInput();
            pNew = pInput;
            AddInput(pNew);
        }

        nCount = pBase.GetOutputCount();

        for (int i = 0; i < nCount; i++)
        {
            CClassOutput pOutput = pBase.GetOutput(i);

            CClassOutput pNew = new CClassOutput();
            pNew = pOutput;
            AddOutput(pNew);
        }

        if (!m_bGotSize)
        {
            if (pBase.GetBoundBox(m_bmins, m_bmaxs))
            {
                m_bGotSize = true;
            }
        }

        if (!m_bGotColor)
        {
            m_rgbColor = pBase.GetColor();
            m_bGotColor = true;
        }
    }

    public void AddInput(CClassInput pInput)
    {
        Debug.Assert(pInput != null);

        if (pInput != null)
        {
            m_Inputs.AddToTail(pInput);
        }
    }

    public CClassInput FindInput(string szName)
    {
        int nCount = GetInputCount();

        for (int i = 0; i < nCount; i++)
        {
            CClassInput pInput = GetInput(i);

            if (string.Compare(pInput.GetName().ToLower(), szName.ToLower()) == 0)
            {
                return pInput;
            }
        }

        return null;
    }

    public int GetInputCount()
    {
        return m_Inputs.Count();
    }

    public CClassInput GetInput(int nIndex)
    {
        return m_Inputs.Element(nIndex);
    }

    public void AddOutput(CClassOutput pOutput)
    {
        Debug.Assert(pOutput != null);

        if (pOutput != null)
        {
            m_Outputs.AddToTail(pOutput);
        }
    }

    public CClassOutput FindOutput(string szName)
    {
        int nCount = GetOutputCount();

        for (int i = 0; i < nCount; i++)
        {
            CClassOutput pOutput = GetOutput(i);

            if (string.Compare(pOutput.GetName().ToLower(), szName.ToLower()) == 0)
            {
                return pOutput;
            }
        }

        return null;
    }

    public int GetOutputCount()
    {
        return m_Outputs.Count();
    }

    public CClassOutput GetOutput(int nIndex)
    {
        return m_Outputs.Element(nIndex);
    }

    public GameData Parent;

    public bool IsClass(string pszClass)
    {
        Debug.Assert(pszClass != null);
        return string.Compare(pszClass.ToLower(), m_szName) == 0;
    }

    public bool IsSolidClass()
    {
        return m_bSolid;
    }

    public bool IsBaseClass()
    {
        return m_bBase;
    }

    public bool IsMoveClass()
    {
        return m_bMove;
    }

    public bool IsKeyFrameClass()
    {
        return m_bKeyFrame;
    }

    public bool IsPointClass()
    {
        return m_bPoint;
    }

    public bool IsNPCClass()
    {
        return m_bNPC;
    }

    public bool IsFilterClass()
    {
        return m_bFilter;
    }

    public static bool IsNodeClass(string pszClassName)
    {
        return (string.Compare(pszClassName.ToLower(), "info_node") == 0) && (string.Compare(pszClassName.ToLower(), "info_node_link") != 0);
    }

    public bool IsNodeClass()
    {
        return IsNodeClass(m_szName);
    }

    public bool ShouldSnapToHalfGrid()
    {
        return m_bHalfGridSnap;
    }

    public void SetNPCClass(bool bNPC)
    {
        m_bNPC = bNPC;
    }

    public void SetFilterClass(bool bFilter)
    {
        m_bFilter = bFilter;
    }

    public void SetPointClass(bool bPoint)
    {
        m_bPoint = bPoint;
    }

    public void SetBaseClass(bool bBase)
    {
        m_bBase = bBase;
    }

    public void SetMoveClass(bool bMove)
    {
        m_bMove = bMove;
    }

    public void SetKeyFrameClass(bool bKeyFrame)
    {
        m_bKeyFrame = bKeyFrame;
    }

    public Vector GetMins()
    {
        return m_bmins;
    }

    public Vector GetMaxs()
    {
        return b_maxs;
    }

    public bool GetBoundBox(out Vector pfMins, out Vector pfMaxs)
    {
        if (m_bGotSize)
        {
            pfMins = new();
            pfMaxs = new();

            pfMins[0] = m_bmins[0];
            pfMins[1] = m_bmins[1];
            pfMins[2] = m_bmins[2];

            pfMaxs[0] = m_bmaxs[0];
            pfMaxs[1] = m_bmaxs[1];
            pfMaxs[2] = m_bmaxs[2];

            return m_bGotSize;
        }

        pfMins = null;
        pfMaxs = null;

        return m_bGotSize;
    }

    public bool HasBoundBox()
    {
        return m_bGotSize;
    }

    public color32 GetColor()
    {
        return m_rgbColor;
    }

    public void AddHelper(CHelperInfo pHelper)
    {
        Debug.Assert(pHelper != null);

        if (pHelper != null)
        {
            m_Helpers.AddToTail(pHelper);
        }
    }

    public int GetHelperCount()
    {
        return m_Helpers.Count();
    }

    public CHelperInfo GetHelper(int nIndex)
    {
        return m_Helpers.Element(nIndex);
    }

    protected bool ParseInput(TokenReader tr)
    {
        string szToken = string.Empty;

        if (!gamedata.GDGetToken(tr, szToken, szToken.Length, trtoken_t.IDENT, "input"))
        {
            return false;
        }

        CClassInputOutputBase pInput;

        bool bReturn = ParseInputOutput(tr, out pInput);

        if (bReturn)
        {
            AddInput((CClassInput)pInput);
        }
        else
        {
            pInput = null;
        }

        return bReturn;
    }

    protected bool ParseInputOutput(TokenReader tr, out CClassInputOutputBase pInputOutput)
    {
        string szToken = string.Empty;

        pInputOutput = new();

        if (!gamedata.GDGetToken(tr, szToken, szToken.Length, trtoken_t.IDENT))
        {
            pInputOutput = null;
            return false;
        }

        pInputOutput.SetName(szToken);

        if (!gamedata.GDGetToken(tr, szToken, szToken.Length, trtoken_t.OPERATOR, "("))
        {
            pInputOutput = null;
            return false;
        }

        if (!gamedata.GDGetToken(tr, szToken, szToken.Length, trtoken_t.IDENT))
        {
            pInputOutput = null;
            return false;
        }

        InputOutputType_t eType = pInputOutput.SetType(szToken);

        if (eType == InputOutputType_t.iotInvalid)
        {
            gamedata.GDError(tr, "bad input/output type '{0}'", szToken);
            pInputOutput = null;
            return false;
        }

        if (!gamedata.GDGetToken(tr, szToken, szToken.Length, trtoken_t.OPERATOR, ")"))
        {
            pInputOutput = null;
            return false;
        }

        if ((tr.PeekTokenType(szToken, szToken.Length)) == trtoken_t.OPERATOR && (tokenreader.IsToken(szToken, ":")))
        {
            tr.NextToken(szToken, szToken.Length);

            string pszDescription = string.Empty;

            if (!gamedata.GDGetTokenDynamic(tr, pszDescription, trtoken_t.STRING))
            {
                pInputOutput = null;
                return false;
            }

            pInputOutput.SetDescription(pszDescription);
        }

        return true;
    }

    protected bool ParseOutput(TokenReader tr)
    {
        string szToken = string.Empty;

        if (!gamedata.GDGetToken(tr, szToken, szToken.Length, trtoken_t.IDENT, "output"))
        {
            return false;
        }

        CClassInputOutputBase pOutput = new();

        bool bReturn = ParseInputOutput(tr, out pOutput);

        if (bReturn)
        {
            AddOutput((CClassOutput)pOutput);
        }
        else
        {
            pOutput = null;
        }

        return bReturn;
    }

    private bool ParseBase(TokenReader tr)
    {
        string szToken = string.Empty;

        while (true)
        {
            if (!gamedata.GDGetToken(tr, szToken, szToken.Length, trtoken_t.IDENT))
            {
                return false;
            }

            GDclass pBase = Parent.ClassForName(szToken);

            if (pBase == null)
            {
                gamedata.GDError(tr, "undefined base class '{0}'", szToken);
                return false;
            }

            AddBase(pBase);

            if (!gamedata.GDGetToken(tr, szToken, szToken.Length, trtoken_t.OPERATOR))
            {
                return false;
            }

            if (tokenreader.IsToken(szToken, ")"))
            {
                break;
            }
            else if (!tokenreader.IsToken(szToken, ","))
            {
                gamedata.GDError(tr, "expecting ',' or ')', but found {0}", szToken);
                return false;
            }
        }

        return true;
    }

    private bool ParseColor(TokenReader tr)
    {
        string szToken = string.Empty;

        if (!gamedata.GDGetToken(tr, szToken, szToken.Length, trtoken_t.INTEGER))
        {
            return false;
        }

        byte r = atoi(szToken);

        if (!gamedata.GDGetToken(tr, szToken, szToken.Length, trtoken_t.INTEGER))
        {
            return false;
        }

        byte g = atoi(szToken);

        if (!gamedata.GDGetToken(tr, szToken, szToken.Length, trtoken_t.INTEGER))
        {
            return false;
        }

        byte b = atoi(szToken);

        m_rgbColor.r = r;
        m_rgbColor.g = g;
        m_rgbColor.b = b;
        m_rgbColor.a = 0;

        m_bGotColor = true;

        if (!gamedata.GDGetToken(tr, szToken, szToken.Length, trtoken_t.OPERATOR, ")"))
        {
            return false;
        }

        return true;
    }

    private bool ParseHelper(TokenReader tr, string pszHelperName)
    {
        string szToken = string.Empty;

        CHelperInfo pHelper = new();
        pHelper.SetName(pszHelperName);

        bool bCloseParen = false;

        while (!bCloseParen)
        {
            trtoken_t eType = tr.PeekTokenType(szToken, szToken.Length);

            if (eType == trtoken_t.OPERATOR)
            {
                if (!gamedata.GDGetToken(tr, szToken, szToken.Length, trtoken_t.OPERATOR))
                {
                    pHelper = null;
                    return false;
                }

                if (tokenreader.IsToken(szToken, ")"))
                {
                    bCloseParen = true;
                }
                else if (tokenreader.IsToken(szToken, "="))
                {
                    pHelper = null;
                    return false;
                }
            }
            else
            {
                if (!gamedata.GDGetToken(tr, szToken, szToken.Length, eType))
                {
                    pHelper = null;
                    return false;
                }
                else
                {
                    pHelper.AddParameter(szToken);
                }
            }
        }

        m_Helpers.AddToTail(pHelper);

        return true;
    }

    private bool ParseSize(TokenReader tr)
    {
        string szToken = string.Empty;

        for (int i = 0; i < 3; i++)
        {
            if (!gamedata.GDGetToken(tr, szToken, szToken.Length, trtoken_t.INTEGER))
            {
                return false;
            }

            m_bmins[i] = (float)atof(szToken);
        }

        if (tr.PeekTokenType(szToken, szToken.Length) == trtoken_t.OPERATOR && tokenreader.IsToken(szToken, ","))
        {
            tr.NextToken(szToken, szToken.Length);

            for (int i = 0; i < 3; i++)
            {
                if (!gamedata.GDGetToken(tr, szToken, szToken.Length, trtoken_t.INTEGER))
                {
                    return false;
                }

                m_bmaxs[i] = (float)atof(szToken);
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                float div2 = m_bmins[i] / 2;
                m_bmaxs[i] = div2;
                m_bmins[i] = -div2;
            }
        }

        m_bGotSize = true;

        if (!gamedata.GDGetToken(tr, szToken, szToken.Length, trtoken_t.OPERATOR, ")"))
        {
            return false;
        }

        return true;
    }

    private bool ParseSpecifiers(TokenReader tr)
    {
        string szToken = string.Empty;

        while (tr.PeekTokenType() == trtoken_t.IDENT)
        {
            tr.NextToken(szToken, szToken.Length);

            if (tokenreader.IsToken(szToken, "halfgridsnap"))
            {
                m_bHalfGridSnap = true;
            }
            else
            {
                if (!gamedata.GDSkipToken(tr, trtoken_t.OPERATOR, "("))
                {
                    return false;
                }

                if (tokenreader.IsToken(szToken, "base"))
                {
                    if (!ParseBase(tr))
                    {
                        return false;
                    }
                }
                else if (tokenreader.IsToken(szToken, "size"))
                {
                    if (!ParseSize(tr))
                    {
                        return false;
                    }
                }
                else if (tokenreader.IsToken(szToken, "color"))
                {
                    if (!ParseColor(tr))
                    {
                        return false;
                    }
                }
                else if (!ParseHelper(tr, szToken))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool ParseVariables(TokenReader tr)
    {
        while (true)
        {
            string szToken = string.Empty;

            if (tr.PeekTokenType(szToken, szToken.Length) == trtoken_t.OPERATOR)
            {
                break;
            }

            if (string.Compare(szToken.ToLower(), "input") == 0)
            {
                if (!ParseInput(tr))
                {
                    return false;
                }

                continue;
            }

            if (string.Compare(szToken.ToLower(), "output") == 0)
            {
                if (!ParseOutput(tr))
                {
                    return false;
                }

                continue;
            }

            if (string.Compare(szToken.ToLower(), "key") == 0)
            {
                gamedata.GDGetToken(tr, szToken, szToken.Length);
            }

            GDinputvariable var = new();

            if (!var.InitFromTokens(tr))
            {
                var = null;
                return false;
            }

            int nDupIndex = 0;
            GDinputvariable pDupVar = VarForName(var.GetName(), nDupIndex);

            if (pDupVar != null)
            {
                if (pDupVar.GetType() != var.GetType())
                {
                    string szError = string.Empty;
                    int szErrorSize = stdlib._MAX_PATH;

                    string.Format(szError, "{0}: variable '{1}' is multiply defined with different types.", GetName(), var.GetName());
                    gamedata.GDError(tr, szError);
                }
            }

            if (!AddVariable(var, this, -1, m_Variables.Count()))
            {
                var = null;
            }
        }

        return true;
    }
}