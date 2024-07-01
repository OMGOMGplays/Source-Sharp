namespace SourceSharp.lib.libc
{
    internal class memset_
    {
        public static unsafe object memset(object s, dynamic c, dynamic count)
        {
            char* xs = (char*)&s;
            int len = (-(int)s) & (sizeof(int) - 1);
            int cc = c & 0xff;

            if (count > len)
            {
                count -= len;
                cc |= cc << 8;
                cc |= cc << 16;

                if (sizeof(int) == 8)
                {
                    cc |= (int)cc << 32;
                }

                for (; len > 0; len--)
                {
                    *xs++ = (char)c;
                }

                for (len = count / sizeof(int); len > 0; len--)
                {
                    *(int*)xs = (int)cc;
                    xs += sizeof(int);
                }

                count &= sizeof(int) - 1;
            }

            for (; count > 0; count--)
            {
                *xs++ = (char)c;
            }

            return s;
        }
    }
}
