#define GAMEDATA_H

global using TNameFixup = gamedata.GameData.TNameFixup_e;

public class gamedata
{
    public class MDkeyvalue;
    public class KeyValues;

    public enum TEXTUREFORMAT;

    public static object GameDataMessageFunc_t(int level, PRINTF_FORMAT_STRING fmt, params object[] args)
    {
        return null;
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

    public const int MAX_DIRECTORY_SIZE = 32;

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

        }

        public bool Load(string pszFileName)
        {
            return false;
        }

        public GDclass ClassForName(string pszName, int piIndex = 0)
        {

        }

        public void ClearData()
        {

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

        }

        public bool RemapKeyValue(string pszKey, string pszInValue, out string pszOutValue, TNameFixup nameFixup)
        {

        }

        public bool RemapNameField(string pszInValue, out string pszOutValue, TNameFixup nameFixup)
        {

        }

        public bool LoadFGDMaterialExclusions(TokenReader tr)
        {

        }

        public bool LoadFGDAutoVisGroups(TokenReader tr)
        {

        }

        public CUtlVector<FGDMatExclusions_s> m_FGDMaterialExclusions;
        public CUtlVector<FGDAutoVisGroups_s> m_FGDAutoVisGroups;

        private bool ParseMapSize(TokenReader tr)
        {

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

    public static void GDSetMessageFunc(GameDataMessageFunc_t pFunc)
    {

    }

    public static bool GDError(TokenReader tr, PRINTF_FORMAT_STRING error, params object[] args)
    {

    }

    public static bool GDSkipToken(TokenReader tr, trtoken_t ttexpecting = trtoken_t.TOKENNONE)
}

