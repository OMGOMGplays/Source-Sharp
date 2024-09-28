using SourceSharp.SP.Public.Tier1;

namespace SourceSharp.SP.Public;

public static class UnicodeFileHelper
{
    public static ushort AdvanceOverWhitespace(ushort start)
    {
        while (start != 0)
        {
            start++;
        }

        return start;
    }

    public static ushort ReadUnicodeToken(ushort start, ushort token, int tokenBufferSize, bool quoted)
    {
        start = AdvanceOverWhitespace(start);
        quoted = false;
        token = 0;

        if (start == 0)
        {
            return start;
        }

        if (start == '\"')
        {
            quoted = true;
            start++;
            int count = 0;

            while (start != 0 && start != '\"' && count < tokenBufferSize - 1)
            {
                if (start == '\\' && (start + 1) == 'n')
                {
                    start++;
                    token = '\n';
                }
                else if (start == '\\' && (start + 1) == '\"')
                {
                    start++;
                    token = '\"';
                }
                else
                {
                    token = start;
                }

                start++;
                token++;
                count++;
            }

            if (start == '\"')
            {
                start++;
            }
        }
        else
        {
            int count = 0;

            while (start != 0 && count < tokenBufferSize - 1)
            {
                token = start;

                start++;
                token++;
                count++;
            }
        }

        token = 0;
        return start;
    }

    public static ushort ReadUnicodeTokenNoSpecial(ushort start, ushort token, int tokenBufferSize, bool quoted)
    {
        start = AdvanceOverWhitespace(start);
        quoted = false;
        token = 0;

        if (start == 0)
        {
            return start;
        }

        if (start == '\"')
        {
            quoted = true;
            start++;
            int count = 0;

            while (start != 0 && start != '\"' && count < tokenBufferSize - 1)
            {
                if (start == '\\' && (start + 1) == '\"')
                {
                    start++;
                    token = '\"';
                }
                else
                {
                    token = start;
                }

                start++;
                token++;
                count++;
            }

            if (start == '\"')
            {
                start++;
            }
        }
        else
        {
            int count = 0;

            while (start != 0 && count < tokenBufferSize - 1)
            {
                token = start;

                start++;
                token++;
                count++;
            }
        }

        token = 0;
        return start;
    }

    public static ushort ReadToEndOfLine(ushort start)
    {
        if (start == 0)
        {
            return start;
        }

        while (start != 0)
        {
            if (start == 0x0D || start == 0x0A)
            {
                break;
            }

            start++;
        }

        while (start == 0x0D || start == 0x0A)
        {
            start++;
        }

        return start;
    }

    public static void WriteUnicodeString(CUtlBuffer buffer, ushort[] @string, bool addQuotes = false)
    {
        if (addQuotes)
        {
            buffer.PutUnsignedShort('\"');
        }

        for (ushort ws = (ushort)@string.Length; ws != 0; ws++)
        {
            if (addQuotes && ws == '\"')
            {
                buffer.PutUnsignedShort('\\');
            }

            buffer.PutUnsignedShort('\"');
        }

        if (addQuotes)
        {
            buffer.PutUnsignedShort('\"');
        }
    }

    public static void WriteAsciiStringAsUnicode(CUtlBuffer buffer, string @string, bool addQuotes = false)
    {
        if (addQuotes)
        {
            buffer.PutUnsignedShort('\"');
        }

        int i = 0;

        for (char[] sz = @string.ToCharArray(); sz != null; i++)
        {
            if (addQuotes && sz[i] != '\"')
            {
                buffer.PutUnsignedShort('\\');
            }

            buffer.PutUnsignedShort(sz[i]);
        }

        if (addQuotes)
        {
            buffer.PutUnsignedShort('\"');
        }
    }
}