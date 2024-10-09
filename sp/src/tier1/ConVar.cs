using System;
using System.Diagnostics;

namespace SourceSharp.SP.Tier1;

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

    public delegate int CommandCompletionCallback(string partial, string[] commands);

    private ConCommandBase next;

    private bool registered;

    private string name;
    private string helpString;

    protected int flags;

    private static ConCommandBase conCommandBases;
    private static IConCommandBaseAccessor accessor;

    public static CDefaultAccessor defaultAccessor;

    public static void ConVar_Register(int cVarFlag, IConCommandBaseAccessor accessor)
    {
        if (Tier1.Tier1.cVar == null || CDefaultAccessor.registered)
        {
            return;
        }

        Dbg.Assert(CDefaultAccessor.dllIdentifier < 0);
        CDefaultAccessor.registered = true;
        CDefaultAccessor.cVarFlag = cVarFlag;
        CDefaultAccessor.dllIdentifier = Tier1.Tier1.cVar.AllocateDLLIdentifier();

        ConCommandBase cur, next;

        ConCommandBase.accessor = accessor != null ? accessor : defaultAccessor;
        cur = conCommandBases;

        while (cur != null)
        {
            next = cur.next;
            cur.AddFlags(cVarFlag);
            cur.Init();
            cur = next;
        }

        Tier1.Tier1.cVar.ProcessQueuedMaterialThreadConVarSets();
        conCommandBases = null;
    }

    public static void ConVar_PublishToVXConsole()
    {

    }

    public ConCommandBase()
    {
        registered = false;
        name = null;
        helpString = null;

        flags = 0;
        next = null;
    }

    public ConCommandBase(string name, string helpString, int flags)
    {
        Create(name, helpString, flags);
    }

    ~ConCommandBase()
    {

    }

    public virtual bool IsCommand()
    {
        return true;
    }

    public virtual bool IsFlagSet(int flag)
    {
        return (flag & flags) != 0;
    }

    public virtual void AddFlags(int flags)
    {
        this.flags |= flags;
    }

    public virtual string GetName()
    {
        return name;
    }

    public virtual string GetHelpText()
    {
        return helpString;
    }

    public ConCommandBase GetNext()
    {
        return next;
    }

    public virtual bool IsRegistered()
    {
        return registered;
    }

    public virtual CVarDLLIdentifier GetDLLIdentifier()
    {
        return CDefaultAccessor.dllIdentifier;
    }

    protected virtual void Create(string name, string helpString, int flags)
    {
        string empty_string = "";

        registered = false;

        Dbg.Assert(name != null);
        this.name = name;
        this.helpString = helpString != null ? helpString : empty_string;

        this.flags = flags;

        if ((flags & IConVar.FCVAR_UNREGISTERED) == 0)
        {
            next = conCommandBases;
            conCommandBases = null;
        }
        else
        {
            next = null;
        }

        if (accessor != null)
        {
            Init();
        }
    }

    protected virtual void Init()
    {
        if (accessor != null)
        {
            accessor.RegisterConCommandBase(this);
        }
    }

    protected void Shutdown()
    {
        if (Tier1.Tier1.cVar != null)
        {
            Tier1.Tier1.cVar.UnregisterCommand(this);
        }
    }

    protected string CopyString(string from)
    {
        int len;
        string to;

        len = from.Length;

        if (len <= 0)
        {
            to = new string(new char[1]);
            to.ToCharArray()[0] = '\0';
        }
        else
        {
            to = new string(new char[len + 1]);
            to = from;
        }

        return to;
    }
}

public class CCommand
{
    public const int COMMAND_MAX_ARGC = 64;
    public const int COMMAND_MAX_LENGTH = 256;

    private int argc;
    private int argv0Size;
    private char[] argSBuffer = new char[COMMAND_MAX_LENGTH];
    private byte[][] argVBuffer = new byte[COMMAND_MAX_LENGTH][];
    private byte[][] argv;

    public static CharacterSet breakSet;
    public static bool builtBreakSet = false;

    public CCommand()
    {
        if (!builtBreakSet)
        {
            builtBreakSet = true;
            CharacterSetBuild(breakSet, "{}()':");
        }

        Reset();
    }

    public CCommand(int argc, byte[][] argv)
    {
        Dbg.Assert(argc > 0);

        if (!builtBreakSet)
        {
            builtBreakSet = true;
            CharacterSetBuild(breakSet, "{}()':");
        }

        Reset();

        byte[] bBuf = argVBuffer;
        char[] sBuf = argSBuffer;
        this.argc = argc;

        for (int i = 0; i < argc; i++)
        {
            this.argv[i] = bBuf;
            int len = this.argv[i].Length;
            bBuf = argv[i];

            if (i == 0)
            {
                argv0Size = len;
            }

            bBuf += len + 1;

            bool containsSpace = argv[i] != null;

            if (containsSpace)
            {
                sBuf++ = '\"';
            }

            sBuf = argv[i];
            sBuf += len;

            if (containsSpace)
            {
                sBuf++ = '\"';
            }

            if (i != argc - 1)
            {
                sBuf++ = ' ';
            }
        }
    }

    public bool Tokenize(string command, CharacterSet breakSet)
    {
        Reset();

        if (command == null)
        {
            return false;
        }

        if (breakSet == default)
        {
            breakSet = CCommand.breakSet;
        }

        int len = command.Length;

        if (len >= COMMAND_MAX_LENGTH - 1)
        {
            Dbg.Warning("CCommand::Tokenize: Encountered command which overflows the tokenizer buffer... Skipping!\n");
            return false;
        }

        argSBuffer = command.ToCharArray();

        CUtlBuffer bufParse = new CUtlBuffer(argSBuffer, len, CUtlBuffer.BufferFlags.TEXT_BUFFER | CUtlBuffer.BufferFlags.READ_ONLY);
        int argvBufferSize = 0;

        while (bufParse.IsValid() && argc < COMMAND_MAX_ARGC)
        {
            byte[] argvBuf = argVBuffer[argvBufferSize];
            int maxLen = COMMAND_MAX_LENGTH - argvBufferSize;
            int startGet = bufParse.TellGet();
            int size = bufParse.ParseToken(breakSet, argvBuf, maxLen);

            if (size < 0)
            {
                break;
            }

            if (maxLen == size)
            {
                Reset();
                return false;
            }

            if (argc == 1)
            {
                argv0Size = bufParse.TellGet();
                bool foundEndQuote = argSBuffer[argv0Size - 1] == '\"';

                if (foundEndQuote)
                {
                    --argv0Size;
                }

                argv0Size -= size;
                Dbg.Assert(argv0Size != 0);

                bool foundStartQuote = (argv0Size > startGet) && (argSBuffer[argv0Size - 1] == '\"');
                Dbg.Assert(foundEndQuote == foundStartQuote);

                if (foundStartQuote)
                {
                    --argv0Size;
                }
            }

            argv[argc++] = argvBuf;

            if (argc >= COMMAND_MAX_ARGC)
            {
                Dbg.Warning("CCommand::Tokenize: Encountered command which overflows the argument buffer... Clamped!\n");
            }

            argvBufferSize += size + 1;
            Dbg.Assert(argvBufferSize <= COMMAND_MAX_LENGTH);
        }

        return true;
    }

    public void Reset()
    {
        argc = 0;
        argv0Size = 0;
        argSBuffer[0] = '\0';
    }

    public int ArgC()
    {
        return argc;
    }

    public byte[][] ArgV()
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

    public byte[] this[int i]
    {
        get
        {
            return [byte.Parse(Arg(i))];
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
            return null;
        }

        return argv[index].ToString();
    }

    public string FindArg(string name)
    {
        int argc = ArgC();

        for (int i = 0; i < argc; i++)
        {
            if (string.Compare(Arg(i), name) == 0)
            {
                return (i + 1) < argc ? Arg(i + 1) : "";
            }
        }

        return 0;
    }

    public int FindArgInt(string name, int defaultVal)
    {
        string val = FindArg(name);

        if (val != null)
        {
            return int.Parse(val);
        }
        else
        {
            return defaultVal;
        }
    }

    public static int MaxCommandLength()
    {
        return COMMAND_MAX_LENGTH - 1;
    }

    public static CharacterSet DefaultBreakSet()
    {
        return breakSet;
    }

    public static int DefaultCompletionFunc(string partial, char[,] commands)
    {
        return 0;
    }
}

public class ConCommand : ConCommandBase
{
    public ConCommandBase BaseClass;

    private CommandCallbackVoid commandCallbackV1;
    private CommandCallback commandCallback;
    private ICommandCallback iCommandCallback;

    private CommandCompletionCallback completionCallback;
    private ICommandCompletionCallback iCommandCompletionCallback;

    private bool hasCompletionCallback = true;
    private bool usingNewCommandCallback = true;
    private bool usingCommandCallbackInterface = true;

    public ConCommand(string name, CommandCallbackVoid callback, string helpString = null, int flags = 0, CommandCompletionCallback completionFunc = null)
    {
        commandCallbackV1 = callback;
        usingNewCommandCallback = false;
        usingCommandCallbackInterface = false;
        completionCallback = completionFunc != null ? completionFunc : CCommand.DefaultCompletionFunc;
        hasCompletionCallback = completionFunc != null ? true : false;

        base.Create(name, helpString, flags);
    }

    public ConCommand(string name, CommandCallback callback, string helpString = null, int flags = 0, CommandCompletionCallback completionFunc = null)
    {
        commandCallback = callback;
        usingNewCommandCallback = true;
        completionCallback = completionFunc != null ? completionFunc : CCommand.DefaultCompletionFunc;
        hasCompletionCallback = completionFunc != null ? true : false;
        usingCommandCallbackInterface = false;

        base.Create(name, helpString, flags);
    }

    public ConCommand(string name, ICommandCallback callback, string helpString = null, int flags = 0, ICommandCompletionCallback completionCallback = null)
    {
        iCommandCallback = callback;
        usingNewCommandCallback = true;
        iCommandCompletionCallback = completionCallback;
        hasCompletionCallback = completionCallback != null;
        usingCommandCallbackInterface = true;

        base.Create(name, helpString, flags);
    }

    ~ConCommand()
    {

    }

    public virtual bool IsCommand()
    {
        return true;
    }

    public virtual int AutoCompleteSuggest(string partial, CUtlVector<CUtlString> commands)
    {
        if (usingCommandCallbackInterface)
        {
            if (iCommandCompletionCallback == null)
            {
                return 0;
            }

            return iCommandCompletionCallback.CommandCompletionCallback(partial, commands);
        }

        Dbg.Assert(completionCallback != null);

        if (completionCallback == null)
        {
            return 0;
        }

        string[] cCommands = new string[COMMAND_COMPLETION_MAXITEMS];
        int ret = completionCallback(partial, cCommands);

        for (int i = 0; i < ret; i++)
        {
            CUtlString str = new CUtlString(cCommands[i]);
            commands.AddToTail(str);
        }

        return ret;
    }

    public virtual bool CanAutoComplete()
    {
        return hasCompletionCallback;
    }

    public virtual void Dispatch(CCommand command)
    {
        if (usingNewCommandCallback)
        {
            if (commandCallback != null)
            {
                commandCallback(command);
                return;
            }
        }
        else if (usingCommandCallbackInterface)
        {
            if (iCommandCallback != null)
            {
                iCommandCallback.CommandCallback(command);
                return;
            }
        }
        else
        {
            if (commandCallbackV1 != null)
            {
                commandCallbackV1();
                return;
            }
        }

        Dbg.Assert(false, $"Encountered ConCommand {GetName()} without a callback!\n");
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
        Create(name, defaultValue, flags);
    }

    public ConVar(string name, string defaultValue, int flags, string helpString)
    {
        Create(name, defaultValue, flags, helpString);
    }

    public ConVar(string name, string defaultValue, int flags, string helpString, bool min, float minVal, bool max, float maxVal)
    {
        Create(name, defaultValue, flags, helpString, min, minVal, max, maxVal);
    }

    public ConVar(string name, string defaultValue, int flags, string helpString, IConVar.ChangeCallback callback)
    {
        Create(name, defaultValue, flags, helpString, false, 0.0f, false, 0.0f, callback);
    }

    public ConVar(string name, string defaultValue, int flags, string helpString, bool min, float minVal, bool max, float maxVal, IConVar.ChangeCallback callback)
    {
        Create(name, defaultValue, flags, helpString, min, minVal, max, maxVal, callback);
    }

    ~ConVar()
    {
        if (@string != null)
        {
            @string = null;
        }
    }

    public override bool IsFlagSet(int flag)
    {
        return (flag & parent.flags) != 0 ? true : false;
    }

    public override string GetHelpText()
    {
        return parent.GetHelpText();
    }

    public override bool IsRegistered()
    {
        return parent.IsRegistered();
    }

    public override string GetName()
    {
        return parent.GetName();
    }

    public override void AddFlags(int flags)
    {
        parent.flags |= flags;
    }

    public override bool IsCommand()
    {
        return false;
    }

    public void InstallChangeCallback(IConVar.ChangeCallback callback)
    {
        Dbg.Assert(parent.changeCallback == null || callback == null);
        parent.changeCallback = callback;

        if (parent.changeCallback != null)
        {
            parent.changeCallback(this, @string, fValue);
        }
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
        ConVar var = parent;
        var.InternalSetValue(value);
    }

    public virtual void SetValue(float value)
    {
        ConVar var = parent;
        var.InternalSetValue(value.ToString());
    }

    public virtual void SetValue(int value)
    {
        ConVar var = parent;
        var.InternalSetValue(value.ToString());
    }

    public void Revert()
    {
        ConVar var = parent;
        var.SetValue(var.defaultValue);
    }

    public bool GetMin(ref float minVal)
    {
        minVal = parent.minValue;
        return parent.hasMin;
    }

    public bool GetMax(ref float maxVal)
    {
        maxVal = parent.maxValue;
        return parent.hasMax;
    }

    public string GetDefault()
    {
        return parent.defaultValue;
    }

    public void SetDefault(string newDefault)
    {
        string empty_string = "";
        defaultValue = newDefault != null ? newDefault : empty_string;
        Dbg.Assert(defaultValue != null);
    }

    protected virtual void InternalSetValue(string value)
    {
        if (IsFlagSet(IConVar.FCVAR_MATERIAL_THREAD_MASK))
        {
            if (Tier1.Tier1.cVar != null && !Tier1.Tier1.cVar.IsMaterialThreadSetAllowed())
            {
                Tier1.Tier1.cVar.QueueMaterialThreadSetValue(this, value);
                return;
            }
        }

        float newValue;
        string tempVal = null;
        string val;

        Dbg.Assert(parent == this);

        float oldValue = fValue;

        val = value.ToString();

        if (value == null)
        {
            newValue = 0.0f;
        }
        else
        {
            newValue = float.Parse(value);
        }

        if (ClampValue(ref newValue))
        {
            //Q_snprintf(tempVal, sizeof(tempVal), "%f", newValue);
            val = tempVal;
        }

        fValue = newValue;
        nValue = (int)fValue;

        if ((flags & IConVar.FCVAR_NEVER_AS_STRING) == 0)
        {
            ChangeStringValue(val, oldValue);
        }
    }

    protected virtual void InternalSetFloatValue(float value)
    {
        if (value == fValue)
        {
            return;
        }

        if (IsFlagSet(IConVar.FCVAR_MATERIAL_THREAD_MASK))
        {
            if (Tier1.Tier1.cVar != null && Tier1.Tier1.cVar.IsMaterialThreadSetAllowed())
            {
                Tier1.Tier1.cVar.QueueMaterialThreadSetValue(this, value);
                return;
            }
        }

        Dbg.Assert(parent == this);

        ClampValue(ref value);

        float oldValue = fValue;
        fValue = value;
        nValue = (int)fValue;

        if ((flags & IConVar.FCVAR_NEVER_AS_STRING) == 0)
        {
            string tempVal = null;
            //Q_snprintf(tempVal, sizeof(tempVal), "%f", fValue);
            ChangeStringValue(tempVal, oldValue);
        }
        else
        {
            Dbg.Assert(changeCallback == null);
        }
    }

    protected virtual void InternalSetIntValue(int value)
    {
        if (value == nValue)
        {
            return;
        }

        if (IsFlagSet(IConVar.FCVAR_MATERIAL_THREAD_MASK))
        {
            if (Tier1.Tier1.cVar != null && !Tier1.Tier1.IsMaterialThreadSetAllowed())
            {
                Tier1.Tier1.cVar.QueueMaterialThreadSetValue(this, value);
                return;
            }
        }

        Dbg.Assert(parent == this);

        float fValue = value;

        if (ClampValue(ref fValue))
        {
            value = (int)fValue;
        }

        float oldValue = this.fValue;
        this.fValue = fValue;
        nValue = value;

        if ((flags & IConVar.FCVAR_NEVER_AS_STRING) == 0)
        {
            string tempVal = null;
            //Q_snprintf(tempVal, sizeof(tempVal), "%d", nValue);
            ChangeStringValue(tempVal, oldValue);
        }
        else
        {
            Dbg.Assert(changeCallback == null);
        }
    }

    protected virtual bool ClampValue(ref float value)
    {
        if (hasMin && value < minValue)
        {
            value = minValue;
            return true;
        }

        if (hasMax && value > maxValue)
        {
            value = maxValue;
            return true;
        }

        return false;
    }

    protected virtual void ChangeStringValue(string tempVal, float oldValue)
    {
        Dbg.Assert((flags & IConVar.FCVAR_NEVER_AS_STRING) == 0);

        string _oldValue = @string;

        if (tempVal != null)
        {
            int len = tempVal.Length + 1;

            if (len > stringLength)
            {
                if (@string != null)
                {
                    @string = null;
                }

                @string = new string(new char[len]);
                stringLength = len;
            }

            @string = tempVal;
        }
        else
        {
            @string = null;
        }

        if (changeCallback != null)
        {
            changeCallback(this, _oldValue, oldValue);
        }

        Tier1.Tier1.cVar.CallGlobalChangeCallbacks(this, _oldValue, oldValue);

        _oldValue = null;
    }

    protected virtual void Create(string name, string defaultValue, int flags = 0, string helpString = null, bool min = false, float minVal = 0, bool max = false, float maxVal = 0, IConVar.ChangeCallback callback = null)
    {
        parent = this;

        SetDefault(defaultValue);

        stringLength = this.defaultValue.Length;
        @string = new string(new char[stringLength]);
        @string = this.defaultValue;

        hasMin = min;
        minValue = minVal;
        hasMax = max;
        maxValue = maxVal;

        changeCallback = callback;

        fValue = float.Parse(@string);
        nValue = int.Parse(@string);

        if (hasMin && fValue < minValue)
        {
            Dbg.Assert(false);
        }

        if (hasMax && fValue > maxValue)
        {
            Dbg.Assert(false);
        }

        base.Create(name, helpString, flags);
    }

    protected override void Init()
    {
        base.Init();
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
        if (Tier1.Tier1.cVar == null || !CDefaultAccessor.registered)
        {
            return;
        }

        Dbg.Assert(CDefaultAccessor.dllIdentifier > 0);
        Tier1.Tier1.cVar.UnregisterConCommands(CDefaultAccessor.dllIdentifier);
        CDefaultAccessor.dllIdentifier = -1;
        CDefaultAccessor.registered = false;
    }

    public static void ConVar_PrintFlags(ConCommandBase var)
    {
        bool any = false;

        if (var.IsFlagSet(IConVar.FCVAR_GAMEDLL))
        {
            Dbg.ConMsg(" game");
            any = true;
        }

        if (var.IsFlagSet(IConVar.FCVAR_CLIENTDLL))
        {
            Dbg.ConMsg(" client");
            any = true;
        }

        if (var.IsFlagSet(IConVar.FCVAR_ARCHIVE))
        {
            Dbg.ConMsg(" archive");
            any = true;
        }

        if (var.IsFlagSet(IConVar.FCVAR_NOTIFY))
        {
            Dbg.ConMsg(" notify");
            any = true;
        }

        if (var.IsFlagSet(IConVar.FCVAR_SPONLY))
        {
            Dbg.ConMsg(" singleplayer");
            any = true;
        }

        if (var.IsFlagSet(IConVar.FCVAR_NOT_CONNECTED))
        {
            Dbg.ConMsg(" notconnected");
            any = true;
        }

        if (var.IsFlagSet(IConVar.FCVAR_CHEAT))
        {
            Dbg.ConMsg(" cheat");
            any = true;
        }

        if (var.IsFlagSet(IConVar.FCVAR_REPLICATED))
        {
            Dbg.ConMsg(" replicated");
            any = true;
        }

        if (var.IsFlagSet(IConVar.FCVAR_SERVER_CAN_EXECUTE))
        {
            Dbg.ConMsg(" server_can_execute");
            any = true;
        }

        if (var.IsFlagSet(IConVar.FCVAR_CLIENTCMD_CAN_EXECUTE))
        {
            Dbg.ConMsg(" clientcmd_can_execute");
            any = true;
        }

        if (any)
        {
            Dbg.ConMsg("\n");
        }
    }

    public static void ConVar_PrintDescription(ConCommandBase var)
    {
        bool min, max;
        float minVal, maxVal;
        string str;

        Dbg.Assert(var != null);

        Color clr = new Color();
        clr.SetColor(255, 100, 100, 255);

        if (!var.IsCommand())
        {
            ConVar _var = (ConVar)var;
            ConVar_ServerBounded bounded = (ConVar_ServerBounded)_var;

            min = _var.GetMin(minVal);
            max = _var.GetMax(maxVal);

            string value = null;
            string tempVal = new string(new char[32]);

            if (bounded != null || _var.IsFlagSet(IConVar.FCVAR_NEVER_AS_STRING))
            {
                value = tempVal;

                int intVal = bounded ? bounded.GetInt() : _var.GetInt();
                float floatVal = bounded ? bounded.GetFloat() : _var.GetFloat();

                if (MathF.Abs(intVal - floatVal) < 0.000001)
                {
                    tempVal += $"{intVal}";
                }
                else
                {
                    tempVal += $"{floatVal}";
                }
            }
            else
            {
                value = _var.GetString();
            }

            if (value != null)
            {
                Dbg.ConColorMsg(clr, $"\"{_var.GetName()}\" = \"{value}\"");

                if (string.Compare(value, _var.GetDefault()) != 0)
                {
                    Dbg.ConMsg($" ( def. \"{_var.GetDefault()}\" )");
                }
            }

            if (min)
            {
                Dbg.ConMsg($" min. {minVal}");
            }

            if (max)
            {
                Dbg.ConMsg($" max. {maxVal}");
            }

            Dbg.ConMsg("\n");

            if (bounded != null && MathF.Abs(bounded.GetFloat() - _var.GetFloat()) > 0.0001f)
            {
                Dbg.ConColorMsg(clr, $"** NOTE: The real value is {_var.GetFloat():0.###} but the server has temporarily restricted it to {bounded.GetFloat():0.###} **\n");
            }
        }
        else
        {
            ConCommand _var = (ConCommand)var;
            Dbg.ConColorMsg(clr, $"\"{_var.GetName()}\"\n");
        }

        ConVar_PrintFlags(var);

        str = var.GetHelpText();

        if (str != null && str.ToCharArray()[0] != '\0')
        {
            Dbg.ConMsg($" - {str}\n");
        }
    }
}

public class ConVarRef
{
    private IConVar conVar;
    private ConVar conVarState;

    public static CEmptyConVar emptyConVar = new CEmptyConVar();

    public ConVarRef(string name)
    {
        Init(name, false);
    }

    public ConVarRef(string name, bool ignoreMissing)
    {
        Init(name, ignoreMissing);
    }

    public ConVarRef(IConVar conVar)
    {
        this.conVar = conVar != null ? conVar : emptyConVar;
        conVarState = (ConVar)this.conVar;
    }

    public void Init(string name, bool ignoreMissing)
    {
        conVar = Tier1.Tier1.cVar != null ? Tier1.Tier1.cVar.FindVar(name) : emptyConVar;

        if (conVar == null)
        {
            conVar = emptyConVar;
        }

        conVarState = (ConVar)conVar;

        if (!IsValid())
        {
            bool first = true;

            if (Tier1.Tier1.cVar != null || first)
            {
                if (!ignoreMissing)
                {
                    Dbg.Warning($"ConVarRef {name} doesn't point to an existing ConVar\n");
                }

                first = false;
            }
        }
    }

    public bool IsValid()
    {
        return conVar != emptyConVar;
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
        Dbg.Assert(!IsFlagSet(IConVar.FCVAR_NEVER_AS_STRING));
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
        Dbg.Assert(owner != null && func != null);
        owner.func(command);
    }

    public virtual int CommandCompletionCallback(string partial, CUtlVector<CUtlString> commands)
    {
        Dbg.Assert(owner != null && completionFunc != null);
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

public class CDefaultAccessor : IConCommandBaseAccessor
{
    public static ConCommandBase conCommandBase = null;
    public static IConCommandBaseAccessor accessor = null;
    public static int cVarFlag = 0;
    public static int dllIdentifier = -1;
    public static bool registered = false;

    public bool RegisterConCommandBase(ConCommandBase var)
    {
        Tier1.Tier1.cVar.RegisterConCommand(var);
        return true;
    }
}

public class CEmptyConVar : ConVar
{
    public CEmptyConVar() : base("", "0")
    {
        
    }

    public override void SetValue(string value)
    {
    }

    public override void SetValue(float value)
    {
    }

    public override void SetValue(int value)
    {
    }

    public override string GetName()
    {
        return "";
    }

    public override bool IsFlagSet(int flag)
    {
        return false;
    }
}