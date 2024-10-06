using SourceSharp.SP.Game.Client;
using SourceSharp.SP.Tier1;
using System;
using System.Diagnostics;

namespace SourceSharp.SP.Game.Client;

public interface IConCommandBaseAccessor
{
    public bool RegisterConCommandBase(ConCommandBase var);
}

public interface ICommandCallback
{
    public void CommandCallback(CCommand command);
}

public interface ICommandCompletionCallback
{
    public int CommandCompletionCallback(string partial, CUtlVector<CUtlString> commands);
}

public class ConCommandBase
{
    public delegate void CommandCallbackVoid();
    public delegate void CommandCallback(CCommand command);

    public const int COMMAND_COMPLETION_MAXITEMS = 64;
    public const int COMMAND_COMPLETION_ITEM_LENGTH = 64;

    public delegate int CommandCompletionCallback(string partial, char[,] commands);

    private ConCommandBase next;

    private bool registered;

    private string name;
    private string helpString;

    protected int flags;

    private static ConCommandBase conCommandBase;
    private static IConCommandBaseAccessor accessor;

    public void ConVar_Register(int cvarFlag, IConCommandBaseAccessor accessor)
    {

    }

    public void ConVar_PublishToVXConsole()
    {

    }

    public ConCommandBase()
    {

    }

    public ConCommandBase(string name, string helpString = null, int flags = 0)
    {

    }

    ~ConCommandBase()
    {

    }

    public virtual bool IsCommand()
    {

    }

    public virtual bool IsFlagSet(int flag)
    {

    }

    public virtual void AddFlags(int flags)
    {

    }

    public virtual string GetName()
    {

    }

    public virtual string GetHelpText()
    {

    }

    public ConCommandBase GetNext()
    {

    }

    public virtual bool IsRegistered()
    {

    }

    public virtual CVarDLLIdentifier GetDLLIdentifier()
    {

    }

    protected virtual void Create(string name, string helpString = null, int flags = 0)
    {

    }

    protected virtual void Init()
    {

    }

    protected void Shutdown()
    {

    }

    protected string CopyString(string from)
    {

    }
}

public class CCommand
{
    public const int COMMAND_MAX_ARGC = 64;
    public const int COMMAND_MAX_LENGTH = 256;

    private int argc;
    private int argv0Size;
    private char[] argSBuffer = new char[COMMAND_MAX_LENGTH];
    private byte[] argVBuffer = new byte[COMMAND_MAX_LENGTH];
    private string[] argv;

    public CCommand()
    {

    }

    public CCommand(int argc, string[] argv)
    {

    }

    public bool Tokenize(string command, CharacterSet breakSet = null)
    {

    }

    public void Reset()
    {

    }

    public int ArgC()
    {
        return argc;
    }

    public string[] ArgV()
    {
        return argc != 0 ? argv : null;
    }

    public string ArgS()
    {
        return argv0Size != 0 ? argSBuffer[argv0Size].ToString() : "";
    }

    public string GetCommandString()
    {
        return argc != 0 ? argSBuffer.ToString() : "";
    }

    public string this[int i]
    {
        get
        {
            return Arg(i);
        }

        set
        {
            if (i < 0 || i >= argc)
            {
                throw new IndexOutOfRangeException();
            }

            argv[i] = value;
        }
    }

    public string Arg(int index)
    {
        if (index < 0 || index >= argc)
        {
            return "";
        }

        return argv[index];
    }

    public string FindArg(string name)
    {

    }

    public int FindArgInt(string name, int defaultVal)
    {

    }

    public static int MaxCommandLength()
    {
        return COMMAND_MAX_LENGTH - 1;
    }

    public static CharacterSet DefaultBreakSet()
    {

    }
}

public class ConCommand : ConCommandBase
{
    public ConCommandBase BaseClass;

    private CommandCallbackVoid commandCallbackV1;
    private CommandCallback commandCallback;
    private ICommandCallback iCommandCallback;

    private CommandCompletionCallback completionCallback;
    private ICommandCompletionCallback iCompletionCallback;

    private bool hasCompletionCallback = true;
    private bool usingNewCommandCallback = true;
    private bool usingCommandCallbackInterface = true;

    public ConCommand(string name, CommandCallbackVoid callback, string helpString = null, int flags = 0, CommandCompletionCallback completionFunc = null)
    {

    }

    public ConCommand(string name, CommandCallback callback, string helpString = null, int flags = 0, CommandCompletionCallback completionFunc = null)
    {

    }

    public ConCommand(string name, ICommandCallback callback, string helpString = null, int flags = 0, ICommandCompletionCallback commandCompletionCallback = null)
    {

    }

    ~ConCommand()
    {

    }

    public virtual bool IsCommand()
    {

    }

    public virtual int AutoCompleteSuggest(string partial, CUtlVector<CUtlString> commands)
    {

    }

    public virtual bool CanAutoComplete()
    {

    }

    public virtual void Dispatch(CCommand command)
    {

    }
}

public class ConVar : ConCommandBase, IConVar
{
    public ConCommandBase BaseClass;

    private ConVar parent;

    private string defaultValue;

    private string @string;
    private int stringLength;

    private float fValue;
    private int nValue;

    private bool hasMin;
    private float minValue;
    private bool hasMax;
    private float maxValue;

    private IConVar.ChangeCallback changeCallback;

    public ConVar(string name, string defaultValue, int flags = 0)
    {

    }

    public ConVar(string name, string defaultValue, int flags, string helpString)
    {

    }

    public ConVar(string name, string defaultValue, int flags, string helpString, bool min, float minVal, bool max, float maxVal)
    {

    }

    public ConVar(string name, string defaultValue, int flags, string helpString, IConVar.ChangeCallback callback)
    {

    }

    public ConVar(string name, string defaultValue, int flags, string helpString, bool min, float minVal, bool max, float maxVal, IConVar.ChangeCallback callback)
    {

    }

    ~ConVar()
    {

    }

    public override bool IsFlagSet(int flag)
    {

    }

    public override string GetHelpText()
    {

    }

    public override bool IsRegistered()
    {

    }

    public override string GetName()
    {

    }

    public override void AddFlags(int flags)
    {

    }

    public override bool IsCommand()
    {

    }

    public void InstallChangeCallback(IConVar.ChangeCallback callback)
    {

    }

    public float GetFloat()
    {
        return parent.fValue;
    }

    public int GetInt()
    {
        return parent.nValue;
    }

    public bool GetBool()
    {
        return GetInt() != 0;
    }

    public string GetString()
    {
        if ((flags & IConVar.FCVAR_NEVER_AS_STRING) != 0)
        {
            return "FCVAR_NEVER_AS_STRING";
        }

        return parent.@string != null ? parent.@string : "";
    }

    public virtual void SetValue(string value)
    {

    }

    public virtual void SetValue(float value)
    {

    }

    public virtual void SetValue(int value)
    {

    }

    public void Revert()
    {

    }

    public bool GetMin(float minVal)
    {

    }

    public bool GetMax(float maxVal)
    {

    }

    public string GetDefault()
    {

    }

    public void SetDefault(string newDefault)
    {

    }

    protected virtual void InternalSetValue(string value)
    {

    }

    protected virtual void InternalSetFloatValue(float value)
    {

    }

    protected virtual void InternalSetIntValue(int value)
    {

    }

    protected virtual bool ClampValue(ref float value)
    {

    }

    protected virtual void ChangeStringValue(string tempVal, float oldValue)
    {

    }

    protected virtual void Create(string name, string defaultValue, int flags = 0, string helpString = null, bool min = false, float minVal = 0, bool max = false, float maxVal = 0, IConVar.ChangeCallback callback = null)
    {

    }

    protected virtual void Init()
    {

    }

    private int GetFlags()
    {
        return parent.flags;
    }

    public static void ConVar_Register(int cVarFlag = 0, IConCommandBaseAccessor accessor = null)
    {

    }

    public static void ConVar_Unregister()
    {

    }

    public static void ConVar_PrintFlags(ConCommandBase var)
    {

    }

    public static void ConVar_PrintDescription(ConCommandBase var)
    {

    }
}

public class ConVarRef
{
    private IConVar conVar;
    private ConVar conVarState;

    public ConVarRef(string name)
    {

    }

    public ConVarRef(string name, bool ignoreMissing)
    {

    }

    public ConVarRef(IConVar conVar)
    {

    }

    public void Init(string name, bool ignoreMissing)
    {

    }

    public bool IsValid()
    {

    }

    public bool IsFlagSet(int flags)
    {
        return conVar.IsFlagSet(flags);
    }

    public IConVar GetLinkedConVar()
    {
        return conVar;
    }

    public float GetFloat()
    {
        return conVarState.GetFloat();
    }

    public int GetInt()
    {
        return conVarState.GetInt();
    }

    public bool GetBool()
    {

    }

    public string GetString()
    {
        Debug.Assert(!IsFlagSet(IConVar.FCVAR_NEVER_AS_STRING));
        return conVarState.GetString();
    }

    public void SetValue(string value)
    {
        conVar.SetValue(value);
    }

    public void SetValue(float value)
    {
        conVar.SetValue(value);
    }

    public void SetValue(int value)
    {
        conVar.SetValue(value);
    }

    public void SetValue(bool value)
    {
        conVar.SetValue(value ? 1 : 0);
    }

    public string GetName()
    {
        return conVar.GetName();
    }

    public string GetDefault()
    {
        return conVarState.GetDefault();
    }
}

public class CConCommandMemberAccessor<T> : ConCommand, ICommandCallback, ICommandCompletionCallback where T : CConCommandMemberAccessor<T>
{
    public static ConCommand BaseClass;
    public delegate void MemberCommandCallback(CCommand command);
    public delegate int MemberCommandCompletionCallback(string partial, CUtlVector<CUtlString> commands);

    private T owner;
    private MemberCommandCallback func;
    private MemberCommandCompletionCallback completionFunc;

    public CConCommandMemberAccessor(T owner, string name, MemberCommandCallback callback, string helpString = null, int flags = 0, MemberCommandCompletionCallback completionFunc = null)
    {
        BaseClass = new ConCommand(name, this, helpString, flags, (completionFunc != null) ? this : null);
        this.owner = owner;
        func = callback;
        this.completionFunc = completionFunc;
    }

    ~CConCommandMemberAccessor()
    {
        Shutdown();
    }

    public void SetOwner(T owner)
    {
        this.owner = owner;
    }

    public new virtual void CommandCallback(CCommand command)
    {
        Debug.Assert(owner != null && func != null);
        owner.func(command);
    }

    public virtual int CommandCompletionCallback(string partial, CUtlVector<CUtlString> commands)
    {
        Debug.Assert(owner != null && completionFunc != null);
        return owner.completionFunc(partial, commands);
    }
}

//#define CON_COMMAND( name, description ) \
//static void name( const CCommand &args ); \
//   static ConCommand name##_command( #name, name, description ); \
//   static void name( const CCommand &args )

//#define CON_COMMAND_F( name, description, flags ) \
//   static void name( const CCommand &args ); \
//   static ConCommand name##_command( #name, name, description, flags ); \
//   static void name( const CCommand &args )

//#define CON_COMMAND_F_COMPLETION( name, description, flags, completion ) \
//    static void name( const CCommand &args ); \
//	static ConCommand name##_command( #name, name, description, flags, completion ); \
//	static void name( const CCommand &args )

//#define CON_COMMAND_EXTERN( name, _funcname, description ) \
//    void _funcname( const CCommand &args ); \
//	static ConCommand name##_command( #name, _funcname, description ); \
//	void _funcname( const CCommand &args )

//#define CON_COMMAND_EXTERN_F( name, _funcname, description, flags ) \
//    void _funcname( const CCommand &args ); \
//	static ConCommand name##_command( #name, _funcname, description, flags ); \
//	void _funcname( const CCommand &args )

//#define CON_COMMAND_MEMBER_F( _thisclass, name, _funcname, description, flags ) \
//    void _funcname( const CCommand &args );						\
//	friend class CCommandMemberInitializer_##_funcname;			\
//	class CCommandMemberInitializer_##_funcname					\
//	{															\
//	public:														\
//		CCommandMemberInitializer_##_funcname() : m_ConCommandAccessor( NULL, name, &_thisclass::_funcname, description, flags )	\
//		{														\
//			m_ConCommandAccessor.SetOwner(GET_OUTER(_thisclass, m_##_funcname##_register ) );	\
//		}														\
//	private:													\
//		CConCommandMemberAccessor<_thisclass> m_ConCommandAccessor;	\
//	};															\
//																\
//	CCommandMemberInitializer_##_funcname m_##_funcname##_register;		\