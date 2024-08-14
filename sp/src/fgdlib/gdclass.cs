using System.Diagnostics;

namespace FGDLib;

public class gdclass
{ 
    public const int GD_MAX_VARIABLES = 128;

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

        public bool InitFromTokens(TokenReader tr, gamedata.GameData gameData)
        {

        }

        public int GetVariableCount()
        {
            return m_nVariables;
        }

        public GDinputvariable GetVariableAt(int iIndex)
        {

        }

        public void GetHelperForGDVar(GDinputvariable pVar, CUtlVector<string> helperName)
        {

        }

        public GDinputvariable VarForName(string pszName, int piIndex = 0)
        {

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
                        m_VariableMap[iThisIndex, 1] = iVarIndex;
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
                AddVariable(pVar, pBase, iBaseIndex, i);
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

        }

        public int GetInputCount()
        {
            return m_Inputs.Count();
        }

        public CClassInput GetInput(int nIndex)
        {

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

        }

        public int GetOutputCount()
        {
            return m_Outputs.Count();
        }

        public CClassOutput GetOutput(int nIndex)
        {

        }

        public gamedata.GameData Parent;

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

        public bool GetBoundBox(Vector pfMins, Vector pfMaxs)
        {

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

        }

        protected bool ParseInput(TokenReader tr)
        {

        }

        protected bool ParseInputOutput(TokenReader tr, CClassInputOutputBase pInputOutput)
        {

        }

        protected bool ParseOutput(TokenReader tr)
        {

        }

        protected bool ParseVariable(TokenReader tr)
        {

        }

        private bool ParseBase(TokenReader tr)
        {

        }

        private bool ParseColor(TokenReader tr)
        {

        }

        private bool ParseHelper(TokenReader tr, string pszHelperName)
        {

        }

        private bool ParseSize(TokenReader tr)
        {

        }

        private bool ParseSpecifiers(TokenReader tr)
        {

        }

        private bool ParseVariables(TokenReader tr)
        {

        }
    }
}