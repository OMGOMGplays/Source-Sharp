namespace Tier1;

public class tokenreader
{
    public static bool IsToken(string s1, string s2)
    {
        return string.Compare(s1.ToLower(), s2.ToLower()) == 0 ? true : false;
    }

    public const int MAX_TOKEN = 128 + 1;
    public const int MAX_IDENT = 64 + 1;
    public const int MAX_STRING = 128 + 1;
}

public enum trtoken_t
{
    TOKENSTRINGTOOLONG = -4,
    TOKENERROR = -3,
    TOKENNONE = -2,
    TOKENEOF = -1,
    OPERATOR,
    INTEGER,
    STRING,
    IDENT
}

public class TokenReader
{
    private int m_nLine;
    private int m_nErrorCount;

    private string m_szFileName;
    private string m_szStuffed;
    private bool m_bStuffed;
    private trtoken_t m_eStuffed;

    private StreamReader m_sr;

    public TokenReader()
    {
        m_szFileName = string.Empty;
        m_nLine = 1;
        m_nErrorCount = 0;
        m_bStuffed = false;
        m_sr = null;
    }

    public bool Open(string pszFileName)
    {
        throw new NotImplementedException();
    }

    public trtoken_t NextToken(string pszStore, int nSize)
    {
        throw new NotImplementedException();
    }

    public trtoken_t NextTokenDynamic(out string ppszStore)
    {
        throw new NotImplementedException();
    }

    public void Close()
    {
        m_sr.Close();
    }

    public void IgnoreTill(trtoken_t ttype, string pszToken)
    {
        throw new NotImplementedException();
    }

    public void Stuff(trtoken_t ttype, string pszToken)
    {
        throw new NotImplementedException();
    }

    public bool Expecting(trtoken_t ttype, string pszToken)
    {
        throw new NotImplementedException();
    }

    public string Error(string error, params object[] args)
    {
        string szErrorBuf = string.Empty;
        string.Format(szErrorBuf, "File {0}, line {1}: ", m_szFileName, m_nLine);
        m_nErrorCount++;
        return szErrorBuf;
    }

    public trtoken_t PeekTokenType(string txt = null, int maxLen = 0)
    {
        throw new NotImplementedException();
    }

    public int GetErrorCount()
    {
        return m_nErrorCount;
    }

    private TokenReader(TokenReader other)
    {
        throw new NotImplementedException();
    }

    private TokenReader Equals(TokenReader other)
    {
        throw new NotImplementedException();
    }

    private trtoken_t GetString(string pszStore, int nSize)
    {
        throw new NotImplementedException();
    }

    private bool SkipWhiteSpace()
    {
        throw new NotImplementedException();
    }
}