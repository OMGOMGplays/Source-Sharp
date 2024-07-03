global using uintptr_t = uint;
global using va_list = string;

public class vadefs
{
    public static string _ADDRESSOF(dynamic v) => (string)v;
    public static unsafe int _SLOTSIZEOF(dynamic t) => sizeof();
}

