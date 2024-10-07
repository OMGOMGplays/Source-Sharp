using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SourceSharp.SP.Game.Client;

public enum SpewType
{
    SPEW_MESSAGE = 0,
    SPEW_WARNING,
    SPEW_ASSERT,
    SPEW_ERROR,
    SPEW_LOG,

    SPEW_TYPE_COUNT
}

public enum SpewRetval
{
    SPEW_DEBUGGER = 0,
    SPEW_CONTINUE,
    SPEW_ABORT
}

public class Dbg
{
    public delegate SpewRetval SpewOutputFunc(SpewType spewType, string msg);

    public static void SetSpewOutputFunc(SpewOutputFunc func)
    {

    }

    public static SpewOutputFunc GetSpewOutputFunc()
    {

    }

    public static SpewRetval DefaultSpewFunc(SpewType type, string msg)
    {

    }

    public static SpewRetval DefaultSpewFuncAbortOnAsserts(SpewType type, string msg)
    {

    }

    public static string GetSpewOutputGroup()
    {

    }

    public static int GetSpewOutputLevel()
    {

    }

    public static Color GetSpewOutputColor()
    {

    }

    public static void SpewActivate(string groupName, int level)
    {

    }

    public static bool IsSpewActive(string groupName, int level)
    {

    }

    public static void _SpewInfo(SpewType type, string file, int length)
    {

    }

    public static SpewRetval _SpewMessage(string msg, params object[] args)
    {

    }

    public static SpewRetval _DSpewMessage(SpewType type, Color color, string msg, params object[] args)
    {

    }

    public static void _ExitOnFatalAssert(string file, int line)
    {

    }

    public static bool ShouldUseNewAssertDialog()
    {

    }

    public static bool SetupWin32ConsoleIO()
    {

    }

    public static bool DoNewAssertDialog(string file, int line, string expression)
    {

    }

    public static bool AreAllAssertsDisabled()
    {

    }

    public static void SetAllAssertsDisabled(bool assertsEnabled)
    {

    }

    public delegate void AssertFailedNotifyFunc(string file, int line, string message);

    public static void SetAssertFailedNotifyFunc(AssertFailedNotifyFunc func)
    {

    }

    public static void CallAssertFailedNotifyFunc(string file, int line, string message)
    {

    }

    public static void _AssertMsg(object exp, string msg, Action executeExp, bool fatal)
    {
        if (exp == null)
        {
            _SpewInfo(SpewType.SPEW_ASSERT, null, 0);
            SpewRetval ret = _SpewMessage("{0}", msg);
            CallAssertFailedNotifyFunc(null, 0, msg);
            executeExp.Invoke();

            if (ret == SpewRetval.SPEW_DEBUGGER)
            {
                if (!ShouldUseNewAssertDialog() || DoNewAssertDialog(null, 0, msg))
                {
                    DebuggerBreak();
                }

                if (fatal)
                {
                    _ExitOnFatalAssert(null, 0);
                }
            }
        }
    }

    public static void _AssertMsgOnce(object exp, string msg, bool fatal)
    {
        bool asserted = false;

        if (!asserted)
        {
            _AssertMsg(exp, msg, /*asserted = true*/ null, fatal);
        }
    }

    public static void AssertFatal(object exp)
    {
        _AssertMsg(exp, $"Assertion Failed: {exp}", null, true);
    }

    public static void AssertFatalOnce(object exp)
    {
        _AssertMsgOnce(exp, $"Assertion Failed: {exp}", true);
    }

    public static void AssertFatalMsg(object exp, string msg, params object[] args)
    {
        _AssertMsg(exp, string.Format(msg, args), null, true);
    }

    public static void AssertFatalMsgOnce(object exp, string msg)
    {
        _AssertMsgOnce(exp, msg, true);
    }

    public static void AssertFatalFunc(object exp, Action f)
    {
        _AssertMsg(exp, $"Assertion Failed: {exp}", f, true);
    }

    public static void AssertFatalEquals(object exp, object expectedValue)
    {
        AssertFatalMsg2(exp, "Expected {0} but got {1}!", expectedValue, exp);
    }

    public static void AssertFatalFloatEquals(float exp, float expectedValue, float tol)
    {
        AssertFatalMsg2((MathF.Abs(exp) - expectedValue) <= tol, "Expected {0} but got {1}!", expectedValue, exp);
    }

    public static void VerifyFatal(object exp)
    {
        AssertFatal(exp);
    }

    public static void VerifyEqualsFatal(object exp, object expectedValue)
    {
        AssertFatalEquals(exp, expectedValue);
    }

    public static void AssertFatalMsg1(object exp, string msg, object? a1)
    {
        AssertFatalMsg(exp, msg, a1);
    }

    public static void AssertFatalMsg2(object exp, string msg, object? a1, object? a2)
    {
        AssertFatalMsg(exp, msg, a1, a2);
    }

    public static void AssertFatalMsg3(object exp, string msg, object? a1, object? a2, object? a3)
    {
        AssertFatalMsg(exp, msg, a1, a2, a3);
    }

    public static void AssertFatalMsg4(object exp, string msg, object? a1, object? a2, object? a3, object? a4)
    {
        AssertFatalMsg(exp, msg, a1, a2, a3, a4);
    }

    public static void AssertFatalMsg5(object exp, string msg, object? a1, object? a2, object? a3, object? a4, object? a5)
    {
        AssertFatalMsg(exp, msg, a1, a2, a3, a4, a5);
    }

    public static void AssertFatalMsg6(object exp, string msg, object? a1, object? a2, object? a3, object? a4, object? a5, object? a6)
    {
        AssertFatalMsg(exp, msg, a1, a2, a3, a4, a5, a6);
    }

    public static void AssertFatalMsg7(object exp, string msg, object? a1, object? a2, object? a3, object? a4, object? a5, object? a6, object? a7)
    {
        AssertFatalMsg(exp, msg, a1, a2, a3, a4, a5, a6, a7);
    }

    public static void AssertFatalMsg8(object exp, string msg, object? a1, object? a2, object? a3, object? a4, object? a5, object? a6, object? a7, object? a8)
    {
        AssertFatalMsg(exp, msg, a1, a2, a3, a4, a5, a6, a7, a8);
    }

    public static void AssertFatalMsg9(object exp, string msg, object? a1, object? a2, object? a3, object? a4, object? a5, object? a6, object? a7, object? a8, object? a9)
    {
        AssertFatalMsg(exp, msg, a1, a2, a3, a4, a5, a6, a7, a8, a9);
    }

    public static void Assert(object exp)
    {
        _AssertMsg(exp, $"Assertion Failed: {exp}", null, false);
    }

    public static void AssertMsg(object exp, string msg, params object[] args)
    {
        _AssertMsg(exp, string.Format(msg, args), null, false);
    }

    public static void AssertOnce(object exp)
    {
        _AssertMsgOnce(exp, $"Assertion Failed: {exp}", false);
    }

    public static void AssertMsgOnce(object exp, string msg)
    {
        _AssertMsgOnce(exp, msg, false);
    }

    public static void AssertFunc(object exp, Action f)
    {
        _AssertMsg(exp, $"Assertion Failed: {exp}", f, false);
    }

    public static void AssertEquals(object exp, object expectedValue)
    {
        AssertMsg2(exp == expectedValue, "Expected {0} but got {1}!", expectedValue, exp);
    }

    public static void AssertFloatEquals(float exp, float expectedValue, float tol)
    {
        AssertMsg2((MathF.Abs(exp) - expectedValue) <= tol, "Expected {0} but got {1}!", expectedValue, exp);
    }

    public static void Verify(object exp)
    {
        Assert(exp);
    }

    public static void VerifyMsg1(object exp, string msg, object? a1)
    {
        AssertMsg1(exp, msg, a1);
    }

    public static void VerifyMsg2(object exp, string msg, object? a1, object? a2)
    {
        AssertMsg2(exp, msg, a1, a2);
    }

    public static void VerifyMsg3(object exp, string msg, object? a1, object? a2, object? a3)
    {
        AssertMsg3(exp, msg, a1, a2, a3);
    }

    public static void VerifyEquals(object exp, object expectedValue)
    {
        AssertEquals(exp, expectedValue);
    }

    public static void DbgVerify(object exp)
    {
        Assert(exp);
    }

    public static void AssertMsg1(object exp, string msg, object? a1)
    {
        AssertMsg(exp, msg, a1);
    }

    public static void AssertMsg2(object exp, string msg, object? a1, object? a2)
    {
        AssertMsg(exp, msg, a1, a2);
    }

    public static void AssertMsg3(object exp, string msg, object? a1, object? a2, object? a3)
    {
        AssertMsg(exp, msg, a1, a2, a3);
    }

    public static void AssertMsg4(object exp, string msg, object? a1, object? a2, object? a3, object? a4)
    {
        AssertMsg(exp, msg, a1, a2, a3, a4);
    }

    public static void AssertMsg5(object exp, string msg, object? a1, object? a2, object? a3, object? a4, object? a5)
    {
        AssertMsg(exp, msg, a1, a2, a3, a4, a5);
    }

    public static void AssertMsg6(object exp, string msg, object? a1, object? a2, object? a3, object? a4, object? a5, object? a6)
    {
        AssertMsg(exp, msg, a1, a2, a3, a4, a5, a6);
    }

    public static void AssertMsg7(object exp, string msg, object? a1, object? a2, object? a3, object? a4, object? a5, object? a6, object? a7)
    {
        AssertMsg(exp, msg, a1, a2, a3, a4, a5, a6, a7);
    }

    public static void AssertMsg8(object exp, string msg, object? a1, object? a2, object? a3, object? a4, object? a5, object? a6, object? a7, object? a8)
    {
        AssertMsg(exp, msg, a1, a2, a3, a4, a5, a6, a7, a8);
    }

    public static void AssertMsg9(object exp, string msg, object? a1, object? a2, object? a3, object? a4, object? a5, object? a6, object? a7, object? a8, object? a9)
    {
        AssertMsg(exp, msg, a1, a2, a3, a4, a5, a6, a7, a8, a9);
    }

    public static void AssertAlways(object exp)
    {
        _AssertMsg(exp, $"Assertion Failed: {exp}", null, false);
    }

    public static void AssertMsgAlways(object exp, string msg)
    {
        _AssertMsg(exp, msg, null, false);
    }

    public static string V_STRINGIFY_INTERNAL(object v) => v.ToString();
    public static string V_STRINGIFY(object x) => V_STRINGIFY_INTERNAL(x);

#if !X360
    public static void Msg(string msg, params object[] args)
    {

    }

    public static void DMsg(string groupName, int level, string msg, params object[] args)
    {

    }

    public static void Warning(string msg, params object[] args)
    {

    }

    public static void DWarning(string groupName, int level, string msg, params object[] args)
    {

    }

    public static void Log(string msg, params object[] args)
    {

    }

    public static void DLog(string groupName, int level, string msg, params object[] args)
    {

    }
#endif // !X360

    public static void Error(string msg, params object[] args)
    {

    }

    public static void ErrorIfNot(object condition, string msg)
    {
        if (condition != null)
        {
            return;
        }
        else
        {
            Error(msg);
        }
    }

#if !X360
    public static void DevMsg(int level, string msg, params object[] args)
    {

    }

    public static void DevWarning(int level, string msg, params object[] args)
    {

    }

    public static void DevLog(int level, string msg, params object[] args)
    {

    }

    public static void DevMsg(string msg, params object[] args)
    {

    }

    public static void DevWarning(string msg, params object[] args)
    {

    }

    public static void DevLog(string msg, params object[] args)
    {

    }

    public static void ConColorMsg(int level, Color clr, string msg, params object[] args)
    {

    }

    public static void ConMsg(int level, string msg, params object[] args)
    {

    }

    public static void ConWarning(int level, string msg, params object[] args)
    {

    }

    public static void ConLog(int level, string msg, params object[] args)
    {

    }

    public static void ConColorMsg(Color clr, string msg, params object[] args)
    {

    }

    public static void ConMsg(string msg, params object[] args)
    {

    }

    public static void ConWarning(string msg, params object[] args)
    {

    }

    public static void ConLog(string msg, params object[] args)
    {

    }

    public static void ConDColorMsg(Color clr, string msg, params object[] args)
    {

    }

    public static void ConDMsg(string msg, params object[] args)
    {

    }

    public static void ConDWarning(string msg, params object[] args)
    {

    }

    public static void ConDLog(string msg, params object[] args)
    {

    }

    public static void NetMsg(int level, string msg, params object[] args)
    {

    }

    public static void NetWarning(int level, string msg, params object[] args)
    {

    }

    public static void NetLog(int level, string msg, params object[] args)
    {

    }

    public static void ValidateSpew(CValidator validator)
    {

    }
#endif // !X360

    public static void COM_TimestampedLog(string fmt, params object[] args)
    {

    }

#if DEBUG
    public static object DBG_CODE(object _code) { if (false) { return null; } else { return _code; } }
    public static object DBG_CODE_NOSCOPE(object _code) { return _code; }
    public static object DBG_DCODE(string g, int l, object _code) { if (IsSpewActive(g, l)) { return _code} else { return null; } }
    public static void DBG_BREAK() { DebuggerBreak(); }
#else
    public static object DBG_CODE(object _code) => null;
    public static object DBG_CODE_NOSCOPE(object _code) => null;
    public static object DBG_DCODE(string g, int l, object _code) => null;
    public static void DBG_BREAK() { return; }
#endif // DEBUG

    public static void _AssertValidReadPtr(object ptr, int count = 1)
    {

    }

    public static void _AssertValidWritePtr(object ptr, int count = 1)
    {

    }

    public static void _AssertValidReadWritePtr(object ptr, int count = 1)
    {

    }

    public static void AssertValidStringPtr(string ptr, int maxChar = 0xFFFFFF)
    {

    }

#if DEBUG
    public static void AssertValidReadPtr(object ptr, int count = 1)
    {
        _AssertValidReadPtr(ptr, count);
    }

    public static void AssertValidWritePtr(object ptr, int count = 1)
    {
        _AssertValidWritePtr(ptr, count);
    }

    public static void AssertValidReadWritePtr(object ptr, int count = 1)
    {
        _AssertValidReadWritePtr(ptr, count);
    }
#else
    public static void AssertValidReadPtr(object ptr, int count = 1) { return; }
    public static void AssertValidWritePtr(object ptr, int count = 1) { return; }
    public static void AssertValidReadWritePtr(object ptr, int count = 1) { return; }
#endif // DEBUG

    public void AssertValidThis()
    {
        AssertValidReadWritePtr(this, Marshal.SizeOf(this));
    }
}

public class CScopeMsg
{
    public string scope;

    public CScopeMsg(string scope)
    {
        this.scope = scope;
        Dbg.Msg("{0} {", scope);
    }

    ~CScopeMsg()
    {
        Dbg.Msg("} {0}", scope);
    }
}

#if DEBUG
public class CReentryGuard
{
    private int semaphore;

    public CReentryGuard(int semaphore)
    {
        this.semaphore = semaphore;
        this.semaphore++;
    }

    ~CReentryGuard()
    {
        --semaphore;
    }
}
#endif // DEBUG

public class CDbgFmtMsg
{
    public char[] buf = new char[256];

    public CDbgFmtMsg(string format, params object[] args)
    {
        buf = format.ToCharArray();
        buf[buf.Length - 1] = '\0';
    }

    public static explicit operator string(CDbgFmtMsg lhs)
    {
        return lhs.buf.ToString();
    }
}

#if DEBUG
public class CDataWatcher
{
    public dynamic value;

    public dynamic Set(dynamic val)
    {
        value = val;
        return value;
    }

    public dynamic GetForModify()
    {
        return value;
    }

    public static dynamic operator +(CDataWatcher lhs, dynamic rhs)
    {
        return lhs.Set(lhs.value + rhs);
    }

    public static dynamic operator -(CDataWatcher lhs, dynamic rhs)
    {
        return lhs.Set(lhs.value - rhs);
    }

    public static dynamic operator /(CDataWatcher lhs, dynamic rhs)
    {
        return lhs.Set(lhs.value / rhs);
    }

    public static dynamic operator *(CDataWatcher lhs, dynamic rhs)
    {
        return lhs.Set(lhs.value / rhs);
    }

    public static dynamic operator ^(CDataWatcher lhs, dynamic rhs)
    {
        return lhs.Set(lhs.value ^ rhs);
    }

    public static dynamic operator |(CDataWatcher lhs, dynamic rhs)
    {
        return lhs.Set(lhs.value | rhs);
    }

    public static CDataWatcher operator ++(CDataWatcher lhs)
    {
        return lhs += 1;
    }

    public static CDataWatcher operator --(CDataWatcher lhs)
    {
        return lhs -= 1;
    }
    
    public dynamic Get()
    {
        return value;
    }
}
#else
public class CDataWatcher
{
    public CDataWatcher()
    {

    }
}
#endif // DEBUG