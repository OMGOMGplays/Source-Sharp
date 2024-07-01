namespace SourceSharp.sp.src._public.zip
{
    // xzip.h

#if _WIN32
    using DWORD = ulong;
#else
    using DWORD = uint;
#endif // _WIN32

    using HZIP = xzip.HZIP;

#if _WIN32
    using ZRESULT = ulong;
#else
    using ZRESULT = uint;
#endif // _WIN32

    // xzip.cpp

    using uch = char;
    using ush = ushort;
    using ulg = ulong;
    using extent = nint;
    using Pos = nuint;
    using IPos = nint;

    using config = xzip.config;
    using ct_data = xzip.ct_data;
    using tree_desc = xzip.tree_desc;
    using iztimes = xzip.iztimes;
    using TZipFileInfo = xzip.zlist;

    public class xzip
    {
        // xzip.h

        public const int ZIP_HANDLE = 1;
        public const int ZIP_FILENAME = 2;
        public const int ZIP_MEMORY = 3;
        public const int ZIP_FOLDER = 4;

        public struct HZIP
        {
            int unused;
        }

        public static HZIP CreateZip(IPos z, uint len, DWORD flags)
        {

        }

        public static ZRESULT ZipAdd(HZIP hz, string dstzn, IntPtr src, uint len, DWORD flags)
        {

        }

        public static ZRESULT CloseZip(HZIP hz)
        {

        }

        public static ZRESULT ZipGetMemory(HZIP hz, IntPtr buf, ulong len)
        {

        }

        public static uint FormatZipMessage(ZRESULT code, string buf, uint len)
        {

        }

        // These are the result codes:
        public const uint ZR_OK = 0x00000000;           // nb. the pseudo-code zr-recent is never returned,
        public const uint ZR_RECENT = 0x00000001;       // but can be passed to FormatZipMessage.
        // The following come from general system stuff (e.g. files not openable)
        public const uint ZR_GENMASK = 0x0000FF00;
        public const uint ZR_NODUPH = 0x00000100;       // couldn't duplicate the handle
        public const uint ZR_NOFILE = 0x00000200;       // couldn't create/open the file
        public const uint ZR_NOALLOC = 0x00000300;      // failed to allocate some resource
        public const uint ZR_WRITE = 0x00000400;        // a general error writing to the file
        public const uint ZR_NOTFOUND = 0x00000500;     // couldn't find that file in the zip
        public const uint ZR_MORE = 0x00000600;         // there's still more data to be unzipped
        public const uint ZR_CORRUPT = 0x00000700;      // the zipfile is corrupt or not a zipfile
        public const uint ZR_READ = 0x00000800;         // a general error reading the file
        // The following come from mistakes on the part of the caller
        public const uint ZR_CALLERMASK = 0x00FF0000;
        public const uint ZR_ARGS = 0x00010000;         // general mistake with the arguments
        public const uint ZR_NOTMMAP = 0x00020000;      // tried to ZipGetMemory, but that only works on mmap zipfiles, which yours wasn't
        public const uint ZR_MEMSIZE = 0x00030000;      // the memory size is too small
        public const uint ZR_FAILED = 0x00040000;       // the thing was already failed when you called this function
        public const uint ZR_ENDED = 0x00050000;        // the zip creation has already been closed
        public const uint ZR_MISSIZE = 0x00060000;      // the indicated input file size turned out mistaken
        public const uint ZR_PARTIALUNZ = 0x00070000;   // the file had already been partially unzipped
        public const uint ZR_ZMODE = 0x00080000;        // tried to mix creating/opening a zip 
        // The following come from bugs within the zip library itself
        public const uint ZR_BUGMASK = 0xFF000000;
        public const uint ZR_NOTINITED = 0x01000000;    // initialisation didn't work
        public const uint ZR_SEEK = 0x02000000;         // trying to seek in an unseekable file
        public const uint ZR_NOCHANGE = 0x04000000;     // changed its mind on storage, but not allowed
        public const uint ZR_FLATE = 0x05000000;        // an internal error in the de/inflation code

        public static HZIP CreateZipZ(IntPtr z, uint len, DWORD flags)
        {

        }

        public static ZRESULT CloseZipZ(HZIP hz)
        {

        }

        public static uint FormatZipMessageZ(ZRESULT code, string buf, uint len)
        {

        }

        public static bool IsZipHandleZ(HZIP hz)
        {

        }

        // xzip.cpp

        public const int EOF = -1;

        public const int ZE_MISS = -1;
        public const int ZE_OK = 0;
        public const int ZE_EOF = 2;
        public const int ZE_FORM = 3;
        public const int ZE_MEM = 4;
        public const int ZE_LOGIC = 5;
        public const int ZE_BIG = 6;
        public const int ZE_NOTE = 7;
        public const int ZE_TEST = 8;
        public const int ZE_ABORT = 9;
        public const int ZE_TEMP = 10;
        public const int ZE_READ = 11;
        public const int ZE_NONE = 12;
        public const int ZE_NAME = 13;
        public const int ZE_WRITE = 14;
        public const int ZE_CREAT = 15;
        public const int ZE_PARMS = 16;
        public const int ZE_OPEN = 18;
        public const int ZE_MAXERR = 18;

        public const int UNKNOWN = -1;
        public const int BINARY = 0;
        public const int ASCII = 1;

        public const int BEST = -1;
        public const int STORE = 0;
        public const int DEFLATE = 8;

        public const long CRCVAL_INITIAL = 0L;

        public const int MSDOS_HIDDEN_ATTR = 0x02;
        public const int MSDOS_DIR_ATTR = 0x10;

        public const byte LOCHEAD = 26;
        public const byte CENHEAD = 42;
        public const byte ENDHEAD = 18;

        public const int EB_HEADSIZE = 4;
        public const int EB_LEN = 2;
        public const int EB_UT_MINLEN = 1;
        public const int EB_UT_FLAGS = 0;
        public const int EB_UT_TIME1 = 1;
        public const int EB_UT_FL_MTIME = 1 << 0;
        public const int EB_UT_FL_ATIME = 1 << 1;
        public const int EB_UT_FL_CTIME = 1 << 2;
        public static int EB_UT_LEN(int n) => EB_UT_MINLEN + 4 * n;
        public static readonly int EB_L_UT_SIZE = EB_HEADSIZE + EB_UT_LEN(3);
        public static readonly int EB_C_UT_SIZE = EB_HEADSIZE + EB_UT_LEN(1);

        //public static void PUTSH(int a, int f) { char _putsh_c = (char)((a) & 0xff);  } // FIXME: Implementation to be!

        public const long LOCSIG = 0x04034b50L;
        public const long CENSIG = 0x02014b50L;
        public const long ENDSIG = 0x06054b50L;
        public const long EXTLOCSIG = 0x08074b50L;

        public const int MIN_MATCH = 3;
        public const int MAX_MATCH = 258;

        public const int WSIZE = 0x8000;

        public const int MIN_LOOKAHEAD = MAX_MATCH + MIN_MATCH + 1;

        public const int MAX_DIST = WSIZE - MIN_LOOKAHEAD;

        public const int MAX_BITS = 15;

        public const int MAX_BL_BITS = 7;

        public const int LENGTH_CODES = 29;

        public const int LITERALS = 256;

        public const int END_BLOCK = 256;

        public const int L_CODES = LITERALS + 1 + LENGTH_CODES;

        public const int D_CODES = 30;

        public const int BL_CODES = 19;

        public const int STORED_BLOCK = 0;
        public const int STATIC_TREES = 1;
        public const int DYN_TREES = 2;

        public const int LIT_BUFSIZE = 0x8000;
        public const int DIST_BUFSIZE = LIT_BUFSIZE;

        public const int REP_3_6 = 16;

        public const int REPZ_3_10 = 17;

        public const int REPZ_11_138 = 18;

        public const int HEAP_SIZE = 2 * L_CODES + 1;

        public const int Buf_size = 8 * 2 * sizeof(char);

        public const int HASH_BITS = 15;

        public const uint HASH_SIZE = 1 << HASH_BITS;
        public const uint HASH_MASK = HASH_SIZE - 1;
        public const int WMASK = WSIZE - 1;

        public const int NIL = 0;

        public const int FAST = 4;
        public const int SLOW = 2;

        public const int TOO_FAR = 4096;

        public const int EQUAL = 0;

        public const int H_SHIFT = (HASH_BITS + MIN_MATCH - 1) / MIN_MATCH;

        public const int max_insert_length = max_lazy_match;

        public static int[] extra_lbits
            = { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 0 };

        public static int[] extra_dbits
            = { 0, 0, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 13, 13 };

        public static int[] extra_blbits
            = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 3, 7 };

        public static uch[] bl_order
            = { (char)16, (char)17, (char)18, (char)0, (char)8, (char)7, (char)9, (char)6, (char)10, (char)5, (char)11, (char)4, (char)12, (char)3, (char)13, (char)2, (char)14, (char)1, (char)15 };

        public struct config
        {
            public ush good_length;
            public ush max_lazy;
            public ush nice_length;
            public ush max_chain;
        }

        public static config[] configuration_table =
        {
            new config() {good_length =  0, max_lazy =   0, nice_length =   0, max_chain =    0},
            new config() {good_length =  4, max_lazy =   4, nice_length =   8, max_chain =    4},
            new config() {good_length =  4, max_lazy =   5, nice_length =  16, max_chain =    8},
            new config() {good_length =  4, max_lazy =   6, nice_length =  32, max_chain =   32},
            new config() {good_length =  4, max_lazy =   4, nice_length =  16, max_chain =   16},
            new config() {good_length =  8, max_lazy =  16, nice_length =  32, max_chain =   32},
            new config() {good_length =  8, max_lazy =  16, nice_length = 128, max_chain =  128},
            new config() {good_length =  8, max_lazy =  32, nice_length = 128, max_chain =  256},
            new config() {good_length = 32, max_lazy = 128, nice_length = 258, max_chain = 1024},
            new config() {good_length = 32, max_lazy = 258, nice_length = 258, max_chain = 4096},
        };

        public struct ct_data
        {
            public class _fc
            {
                public ush freq;
                public ush code;
            }

            public class _dl
            {
                public ush dad;
                public ush len;
            }

            public _dl dl;
            public _fc fc;
        }

        public struct tree_desc
        {
            public ct_data[] dyn_tree;
            public ct_data[] static_tree;
            public int[] extra_bits;
            public int extra_base;
            public int elems;
            public int max_length;
            public int max_code;
        }

        public class TTreeState
        {
            public TTreeState()
            {
                tree_desc a = new tree_desc() { dyn_tree = dyn_ltree, static_tree = static_ltree, extra_bits = extra_lbits, extra_base = LITERALS + 1, elems = L_CODES, max_length = MAX_BITS, max_code = 0 }; l_desc = a;
                tree_desc b = new tree_desc() { dyn_tree = dyn_dtree, static_tree = static_dtree, extra_bits = extra_dbits, extra_base = 0, elems = D_CODES, max_length = MAX_BITS, max_code = 0 }; d_desc = b;
                tree_desc c = new tree_desc() { dyn_tree = bl_tree, static_tree = null, extra_bits = extra_blbits, extra_base = 0, elems = BL_CODES, max_length = MAX_BL_BITS, max_code = 0 }; bl_desc = c;

                last_lit = 0;
                last_dist = 0;
                last_flags = 0;
            }

            public ct_data[] dyn_ltree = new ct_data[HEAP_SIZE];
            public ct_data[] dyn_dtree = new ct_data[2 * D_CODES + 1];
            public ct_data[] static_ltree = new ct_data[L_CODES + 2];
            public ct_data[] static_dtree = new ct_data[D_CODES];
            public ct_data[] bl_tree = new ct_data[2 * BL_CODES + 1];

            public tree_desc l_desc;
            public tree_desc d_desc;
            public tree_desc bl_desc;

            public ush[] bl_count = new ush[MAX_BITS + 1];

            public int[] heap = new int[2 * L_CODES + 1];
            public int heap_len;
            public int heap_max;

            public uch[] depth = new uch[2 * L_CODES + 1];

            public uch[] length_code = new uch[MAX_MATCH - MIN_MATCH + 1];

            public uch[] dist_code = new uch[512];

            public int[] base_length = new int[LENGTH_CODES];

            public int[] base_dist = new int[D_CODES];

            public uch[] l_buf = new uch[LIT_BUFSIZE];
            public uch[] d_buf = new uch[DIST_BUFSIZE];

            public uch[] flag_buf = new uch[LIT_BUFSIZE / 8];

            public uint last_lit;
            public uint last_dist;
            public uint last_flags;
            public uch flags;
            public uch flag_bit;

            public ulg opt_len;
            public ulg static_len;

            public ulg cmpr_bytelen;
            public ulg cmpr_len_bits;

            public ulg input_len;

            public ush file_type;
        }

        public class TBitState
        {
            public int flush_flg;

            public uint bi_buf;

            public int bi_valid;

            public string out_buf;

            public uint out_offset;

            public uint out_size;

            public ulg bits_sent;
        }

        public class TDeflateState
        {
            public TDeflateState()
            {
                window_size = 0;
            }

            public uch[] window = new uch[2L * WSIZE];

            public Pos[] prev = new Pos[WSIZE];
            public Pos[] head = new Pos[HASH_SIZE];

            public ulg window_size;

            public long block_start;

            public int sliding;

            public uint ins_h;

            public uint prev_length;

            public uint strstart;
            public uint match_start;
            public int eofile;
            public uint lookahead;

            public uint max_chain_length;

            public uint max_lazy_match;

            public uint good_match;

            public int nice_match;
        }

        public struct iztimes
        {
            public DateTime atime, mtime, ctime;
        }

        public struct zlist
        {
            public ush vem, ver, flg, how;
            public ulg tim, crc, siz, len;
            public extent nam, ext, cext, com;
            public ush dsk, att, lflg;
            public ulg atx, off;
            public string name;
            public string extra;
            public string cextra;
            public string comment;
            public string iname;
            public string zname;
            public int mark;
            public int trash;
            public int dosflag;
            public static zlist next;
        }

        public delegate uint READFUNC(TState state, string buf, uint size);
        public delegate uint FLUSHFUNC(object param, string buf, uint size);
        public delegate uint WRITEFUNC(object param, string buf, uint size);

        public class TState
        {
            public TState()
            {
                err = "\0";
            }

            public object param;
            public int level;
            public bool seekable;
            public READFUNC readfunc;
            public FLUSHFUNC flush_outbuf;
            public TTreeState ts;
            public TBitState bs;
            public TDeflateState ds;
            public string err;
        }

        public static void Assert(TState state = null, bool cond = false, string msg = null)
        {
            if (cond)
            {
                return;
            }

            state.err = msg;
        }

        public static void Trace(string x, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                Console.Write(string.Format(x, args));
            }
            else
            {
                Console.Write(x);
            }
        }

        public static void Tracec(bool x, string y, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                Console.Write(string.Format(y, args));
            }
            else
            {
                Console.Write(y);
            }
        }

        public static void init_block(TState state)
        {
            int n;

            for (n = 0; n < L_CODES; n++) state.ts.dyn_ltree[n].fc.freq = 0;
            for (n = 0; n < D_CODES; n++) state.ts.dyn_dtree[n].fc.freq = 0;
            for (n = 0; n < BL_CODES; n++) state.ts.bl_tree[n].fc.freq = 0;

            state.ts.dyn_ltree[END_BLOCK].fc.freq = 1;
            state.ts.opt_len = state.ts.static_len = 0L;
            state.ts.last_lit = state.ts.last_dist = state.ts.last_flags = 0;
            state.ts.flags = (char)0; state.ts.flag_bit = (char)1;
        }

        public static void pqdownheap(TState state, ct_data[] tree, int k)
        {
            int v = state.ts.heap[k];
            int j = k << 1;
            int htemp;

            while (j <= state.ts.heap_len)
            {
                if (j < state.ts.heap_len && smaller(state, tree, state.ts.heap[j + 1], state.ts.heap[j]))
                {
                    j++;
                }

                htemp = state.ts.heap[j];

                if (smaller(state, tree, v, htemp))
                {
                    break;
                }

                state.ts.heap[k] = htemp;
                k = j;

                j <<= 1;
            }

            state.ts.heap[k] = v;
        }

        public static void gen_bitlen(TState state, tree_desc desc)
        {
            ct_data[] tree = desc.dyn_tree;
            int[] extra = desc.extra_bits;
            int _base = desc.extra_base;
            int max_code = desc.max_code;
            int max_length = desc.max_length;
            ct_data[] stree = desc.static_tree;
            int h;
            int n, m;
            int bits;
            int xbits;
            ush f;
            int overflow = 0;

            for (bits = 0; bits <= MAX_BITS; bits++)
            {
                state.ts.bl_count[bits] = 0;
            }

            tree[state.ts.heap[state.ts.heap_max]].dl.len = 0;

            for (h = state.ts.heap_max + 1; h < HEAP_SIZE; h++)
            {
                n = state.ts.heap[h];
                bits = tree[tree[n].dl.dad].dl.len + 1;

                if (bits > max_length)
                {
                    bits = max_length;
                    overflow++;
                }

                tree[n].dl.len = (ush)bits;

                if (n > max_code)
                {
                    continue;
                }

                state.ts.bl_count[bits]++;
                xbits = 0;

                if (n >= _base)
                {
                    xbits = extra[n - _base];
                }

                f = tree[n].fc.freq;
                state.ts.opt_len += (ulg)f * (ulong)(bits + xbits);

                if (stree != null)
                {
                    state.ts.static_len += (ulg)f * (ulong)(stree[n].dl.len + xbits);
                }

                if (overflow == 0)
                {
                    return;
                }

                Trace("\nbit length overflow\n");

                do
                {
                    bits = max_length - 1;

                    while (state.ts.bl_count[bits] == 0)
                    {
                        bits--;
                    }

                    state.ts.bl_count[bits]--;
                    state.ts.bl_count[bits + 1] += (ush)2;
                    state.ts.bl_count[max_length]--;

                    overflow -= 2;
                } while (overflow > 0);

                for (bits = max_length; bits != 0; bits--)
                {
                    n = state.ts.bl_count[bits];

                    while (n != 0)
                    {
                        m = state.ts.heap[--h];

                        if (m > max_code)
                        {
                            continue;
                        }

                        if (tree[m].dl.len != (ush)bits)
                        {
                            Trace("code {0} bits {1}->{2}\n", m, tree[m].dl.len, bits);
                            state.ts.opt_len += ((ulong)bits - (ulong)tree[m].dl.len) * (ulong)tree[m].fc.freq;
                            tree[m].dl.len = (ush)bits;
                        }

                        n--;
                    }
                }
            }
        }

        public static void gen_codes(TState state, ct_data[] tree, int max_code)
        {
            ush[] next_code = new ush[MAX_BITS + 1];
            ush code = 0;
            int bits;
            int n;

            for (bits = 1; bits <= MAX_BITS; bits++)
            {
                next_code[bits] = code = (ush)((code + state.ts.bl_count[bits - 1]) << 1);
            }

            Assert(state, code + state.ts.bl_count[MAX_BITS] - 1 == (1 << ((ush)MAX_BITS)) - 1, "inconsistent bit counts");
            Trace("\ngen_codes: max_code {0} ", max_code);

            for (n = 0; n <= max_code; n++)
            {
                int len = tree[n].dl.len;

                if (len == 0)
                {
                    continue;
                }

                tree[n].fc.code = (ush)bi_reverse(next_code[len]++, len);
            }
        }

        public static void build_tree(TState state, tree_desc desc)
        {
            ct_data[] tree = desc.dyn_tree;
            ct_data[] stree = desc.static_tree;
            int elems = desc.elems;
            int n, m;
            int max_code = -1;
            int node = elems;

            state.ts.heap_len = 0;
            state.ts.heap_max = HEAP_SIZE;

            for (n = 0; n < elems; n++)
            {
                if (tree[n].fc.freq != 0)
                {
                    state.ts.heap[++state.ts.heap_len] = max_code = n;
                    state.ts.depth[n] = (char)0;
                }
                else
                {
                    tree[n].dl.len = 0;
                }
            }

            while (state.ts.heap_len < 2)
            {
                int newcp = state.ts.heap[++state.ts.heap_len] = (max_code < 2 ? ++max_code : 0);
                tree[newcp].fc.freq = 1;
                state.ts.depth[newcp] = (char)0;
                state.ts.opt_len--;

                if (stree != null)
                {
                    state.ts.static_len -= stree[newcp].dl.len;
                }
            }

            desc.max_code = max_code;

            for (n = state.ts.heap_len / 2; n >= 1; n--)
            {
                pqdownheap(state, tree, n);
            }

            do
            {
                pqremove(state, tree, n);
                m = state.ts.heap[SMALLEST];

                state.ts.heap[--state.ts.heap_max] = n;
                state.ts.heap[--state.ts.heap_max] = m;

                tree[node].fc.freq = (ush)(tree[n].fc.freq + tree[m].fc.freq);
                state.ts.depth[node] = (uch)(Max(state.ts.depth[n], state.ts.depth[m]) + 1);
                tree[n].dl.dad = tree[m].dl.dad = (ush)node;
                state.ts.heap[SMALLEST] = node++;
                pqdownheap(state, tree, SMALLEST);
            } while (state.ts.heap_len >= 2);

            state.ts.heap[--state.ts.heap_max] = state.ts.heap[SMALLEST];

            gen_bitlen(state, desc);

            gen_codes(state, tree, max_code);
        }

        public static void scan_tree(TState state, ct_data[] tree, int max_code)
        {
            int n;
            int prevlen = -1;
            int curlen;
            int nextlen = tree[0].dl.len;
            int count = 0;
            int max_count = 7;
            int min_count = 4;

            if (nextlen == 0)
            {
                max_count = 138;
                min_count = 3;
            }

            tree[max_code + 1].dl.len = (ush)(0);

            for (n = 0; n <= max_code; n++)
            {
                curlen = nextlen;
                nextlen = tree[n + 1].dl.len;

                if (++count < max_count && curlen == nextlen)
                {
                    continue;
                }
                else if (count < min_count)
                {
                    state.ts.bl_tree[curlen].fc.freq = (ush)(state.ts.bl_tree[curlen].fc.freq + count);
                }
                else if (curlen != 0)
                {
                    if (curlen != prevlen)
                    {
                        state.ts.bl_tree[curlen].fc.freq++;
                    }

                    state.ts.bl_tree[REP_3_6].fc.freq++;
                }
                else if (count <= 10)
                {
                    state.ts.bl_tree[REPZ_3_10].fc.freq++;
                }
                else
                {
                    state.ts.bl_tree[REPZ_11_138].fc.freq++;
                }

                count = 0;
                prevlen = curlen;

                if (nextlen == 0)
                {
                    max_count = 138;
                    min_count = 3;
                }
                else if (curlen == nextlen)
                {
                    max_count = 6;
                    min_count = 3;
                }
                else
                {
                    max_count = 7;
                    min_count = 4;
                }
            }
        }

        public static void send_tree(TState state, ct_data[] tree, int max_code)
        {
            int n;
            int prevlen = -1;
            int curlen;
            int nextlen = tree[0].dl.len;
            int count = 0;
            int max_count = 7;
            int min_count = 4;

            if (nextlen == 0)
            {
                max_count = 138;
                min_count = 3;
            }

            for (n = 0; n <= max_code; n++)
            {
                curlen = nextlen;

                if (++count < max_count && curlen == nextlen)
                {
                    continue;
                }
                else if (count < min_count)
                {
                    do
                    {
                        send_code(state, curlen, state.ts.bl_tree);
                    } while (--count != 0);
                }
                else if (curlen != 0)
                {
                    if (curlen != prevlen)
                    {
                        send_code(state, curlen, state.ts.bl_tree);
                        count--;
                    }

                    Assert(state, count >= 3 && count <= 6, "3_6?");
                    send_code(state, REP_3_6, state.ts.bl_tree);
                    send_bits(state, count - 3, 2);
                }
                else if (count <= 10)
                {
                    send_code(state, REPZ_3_10, state.ts.bl_tree);
                    send_bits(state, count - 3, 3);
                }
                else
                {
                    send_code(state, REPZ_11_138, state.ts.bl_tree);
                    send_bits(state, count - 11, 7);
                }

                count = 0;

                if (nextlen == 0)
                {
                    max_count = 138;
                    min_count = 3;
                }
                else if (curlen == nextlen)
                {
                    max_count = 6;
                    min_count = 3;
                }
                else
                {
                    max_count = 7;
                    min_count = 4;
                }
            }
        }

        public static int build_bl_tree(TState state)
        {
            int max_blindex;

            scan_tree(state, state.ts.dyn_ltree, state.ts.l_desc.max_code);
            scan_tree(state, state.ts.dyn_dtree, state.ts.d_desc.max_code);

            build_tree(state, state.ts.bl_desc);

            for (max_blindex = BL_CODES - 1; max_blindex >= 3; max_blindex--)
            {
                if (state.ts.bl_tree[bl_order[max_blindex]].dl.len != 0)
                {
                    break;
                }
            }

            state.ts.opt_len += (ulong)(3 * (max_blindex + 1) + 5 + 5 + 4);
            Trace("\nndyn trees: dyn {0}, stat {1}", state.ts.opt_len, state.ts.static_len);

            return max_blindex;
        }

        public static void send_all_trees(TState state, int lcodes, int dcodes, int blcodes)
        {
            int rank;

            Assert(state, lcodes >= 257 && dcodes >= 1 && blcodes >= 4, "not enough codes");
            Assert(state, lcodes <= L_CODES && dcodes <= D_CODES && blcodes <= BL_CODES, "too many codes");
            Trace("\nbl counts: ");
            send_bits(state, lcodes - 257, 5);
            send_bits(state, dcodes - 1, 5);
            send_bits(state, blcodes - 4, 4);

            for (rank = 0; rank < blcodes; rank++)
            {
                Trace("\nbl code {0}", bl_order[rank]);
                send_bits(state, state.ts.bl_tree[bl_order[rank]].dl.len, 3);
            }

            Trace("\nbl tree: sent {0}", state.bs.bits_sent);

            send_tree(state, state.ts.dyn_ltree, lcodes - 1);
            Trace("\nlit tree: sent {0}", state.bs.bits_sent);

            send_tree(state, state.ts.dyn_dtree, dcodes - 1);
            Trace("\ndist tree: sent {0}", state.bs.bits_sent);
        }

        public static void compress_block(TState state, ct_data[] ltree, ct_data[] dtree)
        {
            uint dist;
            int lc;
            uint lx = 0;
            uint dx = 0;
            uint fx = 0;
            uch flag = (char)0;
            uint code;
            int extra;

            if (state.ts.last_lit != 0)
            {
                do
                {
                    if ((lx & 7) == 0)
                    {
                        flag = state.ts.flag_buf[fx++];
                    }

                    lc = state.ts.l_buf[lx++];

                    if ((flag & 1) == 0)
                    {
                        send_code(state, lc, ltree);
                    }
                    else
                    {
                        code = state.ts.length_code[lc];
                        send_code(state, (int)(code + LITERALS + 1), ltree);
                        extra = extra_lbits[code];

                        if (extra != 0)
                        {
                            lc -= state.ts.base_length[code];
                            send_bits(state, lc, extra);
                        }

                        dist = state.ts.d_buf[dx++];
                        code = (uint)d_code(state, (int)dist);
                        Assert(state, code < D_CODES, "bad d_code");

                        send_code(state, (int)code, dtree);
                        extra = extra_dbits[code];

                        if (extra != 0)
                        {
                            dist -= (uint)state.ts.base_dist[code];
                            send_bits(state, (int)dist, extra);
                        }
                    }

                    flag >>= 1;
                } while (lx < state.ts.last_lit);
            }

            send_code(state, END_BLOCK, ltree);
        }

        public static void set_file_type(TState state)
        {
            int n = 0;
            uint ascii_freq = 0;
            uint bin_freq = 0;

            while (n < 7) bin_freq += state.ts.dyn_ltree[n++].fc.freq;
            while (n < 128) ascii_freq += state.ts.dyn_ltree[n++].fc.freq;
            while (n < LITERALS) bin_freq += state.ts.dyn_ltree[n++].fc.freq;
            state.ts.file_type = (ush)(bin_freq > (ascii_freq >> 2) ? BINARY : ASCII);
        }

        public static void send_bits(TState state, int value, int length)
        {
            Assert(state, length > 0 && length <= 15, "invalid length");
            state.bs.bits_sent += (ulg)length;

            state.bs.bi_buf |= (uint)(value << state.bs.bi_valid);
            state.bs.bi_valid += length;

            if (state.bs.bi_valid > Buf_size)
            {
                PUTSHORT(state, state.bs.bi_buf);
                state.bs.bi_valid -= Buf_size;
                state.bs.bi_buf = (uint)value >> (length - state.bs.bi_valid);
            }
        }

        public static void bi_init(TState state, string tgt_buf, uint tgt_size, int flsh_allowed)
        {
            state.bs.out_buf = tgt_buf;
            state.bs.out_size = tgt_size;
            state.bs.out_offset = 0;
            state.bs.flush_flg = flsh_allowed;

            state.bs.bi_buf = 0;
            state.bs.bi_valid = 0;
            state.bs.bits_sent = 0L;
        }

        public static uint bi_reverse(uint code, int len)
        {
            uint res = 0;

            do
            {
                res |= code & 1;
                code >>= 1;
                res <<= 1;
            } while (--len > 0);

            return res >> 1;
        }

        public static void bi_windup(TState state)
        {
            if (state.bs.bi_valid > 8)
            {
                PUTSHORT(state, state.bs.bi_buf);
            }
            else if (state.bs.bi_valid > 0)
            {
                PUTBYTE(state, state.bs.bi_buf);
            }

            if (state.bs.flush_flg != 0)
            {
                state.flush_outbuf(state.param, state.bs.out_buf, state.bs.out_offset);
            }

            state.bs.bi_buf = 0;
            state.bs.bi_valid = 0;
            state.bs.bits_sent = (state.bs.bits_sent + 7) & ~(ulong)7;
        }

        public static void copy_block(TState state, string block, uint len, int header)
        {
            bi_windup(state);

            if (header != 0)
            {
                PUTSHORT(state, (ush)len);
                PUTSHORT(state, (ush)~len);
                state.bs.bits_sent += 2 * 16;
            }

            if (state.bs.flush_flg != 0)
            {
                state.flush_outbuf(state.param, state.bs.out_buf, state.bs.out_offset);
                state.bs.out_offset = len;
                state.flush_outbuf(state.param, block, state.bs.out_offset);
            }
            else if (state.bs.out_offset + len > state.bs.out_size)
            {
                Assert(state, false, "output buffer too small for in-memory compression");
            }
            else
            {
                memcpy_.memcpy(state.bs.out_buf + state.bs.out_offset, block, (int)len);
                state.bs.out_offset += len;
            }

            state.bs.bits_sent += (ulg)len << 3;
        }

        public static void send_code(TState state, int c, ct_data[] tree) => send_bits(state, tree[c].fc.code, tree[c].dl.len);

        public static int d_code(TState state, int dist) => ((dist) < 256 ? state.ts.dist_code[dist] : state.ts.dist_code[256 + ((dist) >> 7)]);

        public static dynamic Max(dynamic a, dynamic b) => (a >= b ? a : b);

        public static void ct_init(TState state, ush attr)
        {
            int n;
            int bits;
            int length;
            int code;
            int dist;

            state.ts.file_type = attr;
            state.ts.cmpr_bytelen = state.ts.cmpr_len_bits = 0L;
            state.ts.input_len = 0L;

            if (state.ts.static_dtree[0].dl.len != 0)
            {
                return;
            }

            length = 0;

            for (code = 0; code < LENGTH_CODES - 1; code++)
            {
                state.ts.base_length[code] = length;

                for (n = 0; n < (1 << extra_lbits[code]); n++)
                {
                    state.ts.length_code[length++] = (uch)code;
                }
            }

            Assert(state, length == 256, "ct_init: length != 256");

            state.ts.length_code[length - 1] = (uch)code;

            dist = 0;

            for (code = 0; code < 16; code++)
            {
                state.ts.base_dist[code] = dist;

                for (n = 0; n < (1 << (extra_dbits[code] - 7)); n++)
                {
                    state.ts.dist_code[256 + dist++] = (uch)code;
                }
            }

            Assert(state, dist == 256, "ct_init: dist != 256");

            dist >>= 7;

            for (; code < D_CODES; code++)
            {
                state.ts.base_dist[code] = dist << 7;

                for (n = 0; n < (1 << (extra_dbits[code] - 7)); n++)
                {
                    state.ts.dist_code[256 + dist++] = (uch)code;
                }
            }

            Assert(state, dist == 256, "ct_init: 256+dist != 512");

            for (bits = 0; bits <= MAX_BITS; bits++)
            {
                state.ts.bl_count[bits] = 0;
            }

            n = 0;

            while (n <= 143) state.ts.static_ltree[n++].dl.len = 8; state.ts.bl_count[8]++;
            while (n <= 255) state.ts.static_ltree[n++].dl.len = 9; state.ts.bl_count[9]++;
            while (n <= 279) state.ts.static_ltree[n++].dl.len = 7; state.ts.bl_count[7]++;
            while (n <= 287) state.ts.static_ltree[n++].dl.len = 8; state.ts.bl_count[8]++;

            gen_codes(state, state.ts.static_ltree, L_CODES);

            for (n = 0; n < L_CODES; n++)
            {
                state.ts.static_dtree[n].dl.len = 5;
                state.ts.static_dtree[n].fc.code = (ush)bi_reverse((uint)n, 5);
            }

            init_block(state);
        }

        public const int SMALLEST = 1;

        public static void pqremove(TState state, ct_data[] tree, int top)
        {
            top = state.ts.heap[SMALLEST];
            state.ts.heap[SMALLEST] = state.ts.heap[state.ts.heap_len--];
            pqdownheap(state, tree, SMALLEST);
        }

        public static bool smaller(TState state, ct_data[] tree, int n, int m) => (tree[n].fc.freq < tree[m].fc.freq || (tree[n].fc.freq == tree[m].fc.freq && state.ts.depth[n] <= state.ts.depth[m]));

        public static ulg flush_block(TState state, string buf, ulg stored_len, int eof)
        {
            ulg opt_lenb, static_lenb;
            int max_blindex;

            state.ts.flag_buf[state.ts.last_flags] = state.ts.flags;

            unchecked
            {
                if (state.ts.file_type == (ush)UNKNOWN)
                {
                    set_file_type(state);
                }
            }

            build_tree(state, state.ts.l_desc);
            Trace("\nlit data: dyn {0}, stat {1}", state.ts.opt_len, state.ts.static_len);

            build_tree(state, state.ts.d_desc);
            Trace("\ndist data: dyn {0}, stat {1}", state.ts.opt_len, state.ts.static_len);

            max_blindex = build_bl_tree(state);

            opt_lenb = (state.ts.opt_len + 3 + 7) >> 3;
            static_lenb = (state.ts.static_len + 3 + 7) >> 3;
            state.ts.input_len += stored_len;

            Trace("\nopt {0}({1}) stat {2}({3}) stored {4} lit {5} dist {6} ",
                opt_lenb, state.ts.opt_len, static_lenb, state.ts.static_len, stored_len,
                state.ts.last_lit, state.ts.last_dist);

            if (static_lenb <= opt_lenb)
            {
                opt_lenb = static_lenb;
            }

            if (stored_len + 4 <= opt_lenb && buf != null)
            {
                send_bits(state, (STORED_BLOCK << 1) + eof, 3);
                state.ts.cmpr_bytelen += ((state.ts.cmpr_len_bits + 3 + 7) >> 3) + stored_len + 4;
                state.ts.cmpr_len_bits = 0L;

                copy_block(state, buf, (uint)stored_len, 1);
            }
            else if (static_lenb == opt_lenb)
            {
                send_bits(state, (STATIC_TREES << 1) + eof, 3);
                compress_block(state, state.ts.static_ltree, state.ts.static_dtree);
                state.ts.cmpr_len_bits += 3 + state.ts.static_len;
                state.ts.cmpr_bytelen += state.ts.cmpr_len_bits >> 3;
                state.ts.cmpr_len_bits &= 7L;
            }
            else
            {
                send_bits(state, (DYN_TREES << 1) + eof, 3);
                send_all_trees(state, state.ts.l_desc.max_code + 1, state.ts.d_desc.max_code + 1, max_blindex + 1);
                compress_block(state, state.ts.dyn_ltree, state.ts.dyn_dtree);
                state.ts.cmpr_len_bits += 3 + state.ts.opt_len;
                state.ts.cmpr_bytelen += state.ts.cmpr_len_bits >> 3;
                state.ts.cmpr_len_bits &= 7L;
            }

            Assert(state, ((state.ts.cmpr_bytelen << 3) + state.ts.cmpr_len_bits) == state.bs.bits_sent, "bad compressed size");
            init_block(state);

            if (eof != 0)
            {
                bi_windup(state);
                state.ts.cmpr_len_bits += 7;
            }

            Trace("\n");

            return state.ts.cmpr_bytelen + (state.ts.cmpr_len_bits >> 3);
        }

        public static int ct_tally(TState state, int dist, int lc)
        {
            state.ts.l_buf[state.ts.last_lit++] = (uch)lc;

            if (dist == 0)
            {
                state.ts.dyn_ltree[lc].fc.freq++;
            }
            else
            {
                dist--;

                Assert(state, (ush)dist < (ush)MAX_DIST &&
                       (ush)lc <= (ush)(MAX_MATCH - MIN_MATCH) &&
                       (ush)d_code(state, dist) < (ush)D_CODES, "ct_tally: bad match");

                state.ts.dyn_ltree[state.ts.length_code[lc] + LITERALS + 1].fc.freq++;
                state.ts.dyn_dtree[d_code(state, dist)].fc.freq++;

                state.ts.d_buf[state.ts.last_dist++] = (uch)dist;
                state.ts.flags |= state.ts.flag_bit;
            }

            state.ts.flag_bit <<= 1;

            if ((state.ts.last_lit & 7) == 0)
            {
                state.ts.flag_buf[state.ts.last_flags++] = state.ts.flags;
                state.ts.flags = (char)0;
                state.ts.flag_bit = (char)1;
            }

            if (state.level > 2 && (state.ts.last_lit & 0xfff) == 0)
            {
                ulg out_length = (ulg)state.ts.last_lit * 8L;
                ulg in_length = (ulg)state.ds.strstart - (ulg)state.ds.block_start;
                int dcode;

                for (dcode = 0; dcode < D_CODES; dcode++)
                {
                    out_length += (ulg)state.ts.dyn_dtree[dcode].fc.freq * (ulg)(5L + extra_dbits[dcode]);
                }

                out_length >>= 3;
                Trace("\nlast_lit {0}, last_dist {1}, in {2}, out {3}({4}) ", state.ts.last_lit, state.ts.last_dist, in_length, out_length, 100L - out_length * 100L / in_length);

                if (state.ts.last_dist < state.ts.last_lit / 2 && out_length < in_length / 2)
                {
                    return 1;
                }
            }

            return (state.ts.last_lit == LIT_BUFSIZE - 1 || state.ts.last_dist == DIST_BUFSIZE) ? 1 : 0;
        }

        public static void fill_window(TState state)
        {

        }

        public static ulg deflate_fast(TState state)
        {

        }

        public static int longest_match(TState state, IPos cur_match)
        {
            uint chain_length = state.ds.max_chain_length;
            uch[] scan = state.ds.window + state.ds.strstart; // FIXME: Pointer unpointed here in source code, un-array state.ds.window somehow to fix this
            uch[] match;
            int len;
            int best_len = (int)state.ds.prev_length;
            IPos limit = (nint)(state.ds.strstart > (IPos)MAX_DIST ? state.ds.strstart - (IPos)MAX_DIST : NIL);

            Assert(state, HASH_BITS >= 8 && MAX_MATCH == 258, "Code too clever");

            uch[] strend = state.ds.window + state.ds.strstart + MAX_MATCH; // FIXME: Ditto as above
            uch scan_end1 = scan[best_len - 1];
            uch scan_end = scan[best_len];

            if (state.ds.prev_length >= state.ds.good_match)
            {
                chain_length >>= 2;
            }

            Assert(state, state.ds.strstart <= state.ds.window_size - MIN_LOOKAHEAD, "insufficient lookahead");

            do
            {
                Assert(state, cur_match < state.ds.strstart, "no future");
                match = state.ds.window + cur_match;

                if (match[best_len] != scan_end ||
                    match[best_len - 1] != scan_end1 ||
                    match != scan ||
                    ++match != scan[1]) // FIXME: ++ on an array doesn't work... That is a pointer function! Fix!
                {
                    continue;
                }

                scan += 2;
                match++;

                do
                {

                } while (++scan == ++match && ++scan == ++match &&
                         ++scan == ++match && ++scan == ++match &&
                         ++scan == ++match && ++scan == ++match &&
                         ++scan == ++match && ++scan == ++match &&
                         scan < strend);

                Assert(state, scan <= state.ds.window + (uint)(state.ds.window_size - 1), "wild scan");

                len = MAX_MATCH - (int)(strend - scan);
                scan = strend - MAX_MATCH;
            }
        }

        public static dynamic UPDATE_HASH(dynamic h, dynamic c) => h = (((h) << H_SHIFT) ^ (c)) & HASH_MASK;

        public static void INSERT_STRING(TState state, dynamic s, dynamic match_head)
        {
            UPDATE_HASH(state.ds.ins_h, state.ds.window[s + (MIN_MATCH - 1)]);
            state.ds.prev[(s) & WMASK] = match_head = state.ds.head[state.ds.ins_h];
            state.ds.head[state.ds.ins_h] = s;
        }

        public static void lm_init(TState state, int pack_level, ush flags)
        {
            uint j;

            Assert(state, pack_level >= 1 && pack_level <= 8, "bad pack level");

            state.ds.sliding = 0;

            if (state.ds.window_size == 0L)
            {
                state.ds.sliding = 1;
                state.ds.window_size = (ulg)2L * WSIZE;
            }

            state.ds.head[HASH_SIZE - 1] = NIL;
            unsafe
            {
                memset_.memset(state.ds.head, NIL, (uint)(HASH_SIZE - 1) * state.ds.head.Length);
            }

            state.ds.max_lazy_match     = configuration_table[pack_level].max_lazy;
            state.ds.good_match         = configuration_table[pack_level].good_length;
            state.ds.nice_match         = configuration_table[pack_level].nice_length;
            state.ds.max_chain_length   = configuration_table[pack_level].max_chain;

            if (pack_level <= 2)
            {
                flags |= FAST;
            }
            else if (pack_level >= 8)
            {
                flags |= SLOW;
            }

            state.ds.strstart = 0;
            state.ds.block_start = 0L;

            j = WSIZE;
            j <<= 1;
            state.ds.lookahead = state.readfunc(state, state.ds.window.ToString(), j);

            unchecked
            {
                if (state.ds.lookahead == 0 || state.ds.lookahead == (uint)EOF)
                {
                    state.ds.eofile = 1;
                    state.ds.lookahead = 0;
                    return;
                }
            }

            state.ds.eofile = 0;

            if (state.ds.lookahead < MIN_LOOKAHEAD)
            {
                fill_window(state);
            }

            state.ds.ins_h = 0;

            for (j = 0; j < MIN_MATCH - 1; j++)
            {
                UPDATE_HASH(state.ds.ins_h, state.ds.window[j]);
            }
        }
    }
}
