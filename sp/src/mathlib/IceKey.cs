namespace Mathlib;

public class icekey
{
    public static ulong[,] ice_sbox = new ulong[4, 1024];
    public static int ice_sboxes_initialised = 0;

    public static int[,] ice_smod =
    {
        {333, 313, 505, 369},
        {379, 375, 319, 391},
        {361, 445, 451, 397},
        {397, 425, 395, 505}
    };

    public static int[,] ice_sxor =
    {
        {0x83, 0x85, 0x9b, 0xcd},
        {0xcc, 0xa7, 0xad, 0x41},
        {0x4b, 0x2e, 0xd4, 0x33},
        {0xea, 0xcb, 0x2e, 0x04}
    };

    public static ulong[] ice_pbox =
    {
        0x00000001, 0x00000080, 0x00000400, 0x00002000,
        0x00080000, 0x00200000, 0x01000000, 0x40000000,
        0x00000008, 0x00000020, 0x00000100, 0x00004000,
        0x00010000, 0x00800000, 0x04000000, 0x20000000,
        0x00000004, 0x00000010, 0x00000200, 0x00008000,
        0x00020000, 0x00400000, 0x08000000, 0x10000000,
        0x00000002, 0x00000040, 0x00000800, 0x00001000,
        0x00040000, 0x00100000, 0x02000000, 0x80000000
    };

    public static int[] ice_keyrot =
    {
        0, 1, 2, 3, 2, 1, 3, 0,
        1, 3, 2, 0, 3, 1, 0, 2
    };

    public static uint gf_mult(uint a, uint b, uint m)
    {
        uint res = 0;

        while (b != 0)
        {
            if ((b & 1) != 0)
            {
                res ^= a;
            }

            a <<= 1;
            b >>= 1;

            if (a >= 256)
            {
                a ^= m;
            }
        }

        return res;
    }

    public static ulong gf_exp7(uint b, uint m)
    {
        uint x;

        if (b == 0)
        {
            return 0;
        }

        x = gf_mult(b, b, m);
        x = gf_mult(b, x, m);
        x = gf_mult(x, x, m);
        return gf_mult(b, x, m);
    }

    public static ulong ice_perm32(ulong x)
    {
        ulong res = 0;
        ulong[] pbox = ice_pbox;

        int i = 0;

        while (x != 0)
        {
            if ((x & 1) != 0)
            {
                res |= pbox[i];
            }

            i++;
            x >>= 1;
        }

        return res;
    }

    public static void ice_sboxes_init()
    {
        int i;

        for (i = 0; i < 1024; i++)
        {
            int col = (i >> 1) & 0xff;
            int row = (i & 0x1) | ((i & 0x200) >> 8);
            ulong x;

            x = gf_exp7((uint)(col ^ ice_sxor[0, row]), (uint)ice_smod[0, row]) << 24;
            ice_sbox[0, i] = ice_perm32(x);

            x = gf_exp7((uint)(col ^ ice_sxor[1, row]), (uint)ice_smod[1, row]) << 16;
            ice_sbox[1, i] = ice_perm32(x);

            x = gf_exp7((uint)(col ^ ice_sxor[2, row]), (uint)ice_smod[2, row]) << 8;
            ice_sbox[2, i] = ice_perm32(x);

            x = gf_exp7((uint)(col ^ ice_sxor[3, row]), (uint)ice_smod[3, row]);
            ice_sbox[3, i] = ice_perm32(x);
        }
    }

    public static ulong ice_f(ulong p, IceSubkey sk)
    {
        ulong tl, tr;
        ulong al, ar;

        tl = ((p >> 16) & 0x3ff) | (((p >> 14) | (p << 18)) & 0xffc00);

        tr = (p & 0x3ff) | ((p << 2) & 0xffc00);

        al = sk.val[2] & (tl ^ tr);
        ar = al ^ tr;
        al ^= tl;

        al ^= sk.val[0];
        ar ^= sk.val[1];

        return (ice_sbox[0, al >> 10] | ice_sbox[1, al & 0x3ff] | ice_sbox[2, ar >> 10] | ice_sbox[3, 0x3ff]);
    }
}

public class IceSubkey
{
    public ulong[] val = new ulong[3];
}

public class IceKey
{
    private int _size;
    private int _rounds;
    private IceSubkey[] _keysched;

    public IceKey(int n)
    {
        if (icekey.ice_sboxes_initialised == 0)
        {
            icekey.ice_sboxes_init();
            icekey.ice_sboxes_initialised = 1;
        }

        if (n < 1)
        {
            _size = 1;
            _rounds = 8;
        }
        else
        {
            _size = n;
            _rounds = n * 16;
        }

        _keysched = new IceSubkey[_rounds];
    }

    public void set(string key)
    {
        int i;

        if (_rounds == 8)
        {
            ushort[] kb = new ushort[4];

            for (i = 0; i < 4; i++)
            {
                kb[3 - i] = (ushort)((key[i * 2] << 8) | key[i * 2 + 1]);
            }

            scheduleBuild(kb, 0, icekey.ice_keyrot);
            return;
        }

        for (i = 0; i < _size; i++)
        {
            int j;
            ushort[] kb = new ushort[4];

            for (j = 0; j < 4; j++)
            {
                kb[3 - j] = (ushort)((key[i * 8 + j * 2] << 8) | key[i * 8 + j * 2 + 1]);
            }
        }
    }

    public void encrypt(string ptext, string ctext)
    {
        int i;
        ulong l, r;

        l = (((ulong)ptext[0]) << 24) | (((ulong)ptext[1]) << 16) | (((ulong)ptext[2]) << 8) | ptext[3];
        r = (((ulong)ptext[4]) << 24) | (((ulong)ptext[5]) << 16) | (((ulong)ptext[6]) << 8) | ptext[7];

        for (i = 0; i < _rounds; i += 2)
        {
            l ^= icekey.ice_f(r, _keysched[i]);
            r ^= icekey.ice_f(l, _keysched[i + 1]);
        }

        for (i = 0; i < 4; i++)
        {
            char[] ctextArray = ctext.ToCharArray();

            ctextArray[3 - i] = (char)(r & 0xff);
            ctextArray[7 - i] = (char)(l & 0xff);

            ctext = ctextArray.ToString();

            r >>= 8;
            l >>= 8;
        }
    }

    public void decrypt(string ctext, string ptext)
    {
        int i;
        ulong l, r;

        l = (((ulong)ctext[0]) << 24) | (((ulong)ctext[1]) << 16) | (((ulong)ctext[2]) << 8) | ctext[3];
        r = (((ulong)ctext[4]) << 24) | (((ulong)ctext[5]) << 16) | (((ulong)ctext[6]) << 8) | ctext[7];

        for (i = 0; i < _rounds; i += 2)
        {
            l ^= icekey.ice_f(r, _keysched[i]);
            r ^= icekey.ice_f(l, _keysched[i + 1]);
        }

        for (i = 0; i < 4; i++)
        {
            char[] ptextArray = ptext.ToCharArray();

            ptextArray[3 - i] = (char)(r & 0xff);
            ptextArray[7 - i] = (char)(l & 0xff);

            ptext = ptextArray.ToString();

            r >>= 8;
            l >>= 8;
        }
    }

    public int keySize()
    {
        return _size * 8;
    }

    public int blockSize()
    {
        return 8;
    }

    private void scheduleBuild(ushort[] kb, int n, int[] keyrot)
    {
        int i;

        for (i = 0; i < 8; i++)
        {
            int j;
            int kr = keyrot[i];
            IceSubkey isk = _keysched[n + i];

            for (j = 0; j < 3; j++)
            {
                isk.val[j] = 0;
            }

            for (j = 0; j < 15; j++)
            {
                int k;
                ulong curr_sk = isk.val[j % 3];

                for (k = 0; k < 4; k++)
                {
                    ushort curr_kb = kb[(kr + k) % 3];
                    int bit = curr_kb & 1;

                    curr_sk = (ulong)(((int)curr_sk << 1) | bit);
                    curr_kb = (ushort)((curr_kb >> 1) | ((bit ^ 1) << 15));
                }
            }
        }
    }
}