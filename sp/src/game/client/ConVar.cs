﻿using SourceSharp.SP.Tier1;
using System;

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

    private int flags;

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