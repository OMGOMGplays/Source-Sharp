namespace SourceSharp.SP.Public;

public class BitmapFontFile
{
    public const int BITMAPFONT_ID = ('T' << 24) | ('N' << 16) | ('F' << 8) | ('V');
    public const int BITMAPFONT_VERSION = 3;

    public const int BF_BOLD = 0x0001;
    public const int BF_ITALIC = 0x0002;
    public const int BF_OUTLINED = 0x0004;
    public const int BF_DROPSHADOW = 0x0008;
    public const int BF_BLURRED = 0x0010;
    public const int BF_SCANLINES = 0x0020;
    public const int BF_ANTIALIASED = 0x0040;
    public const int BF_CUSTOM = 0x0080;
}

public struct BitmapGlyph
{
    public short x;
    public short y;
    public short w;
    public short h;
    public short a;
    public short b;
    public short c;
}

public struct BitmapFont
{
    public int id;
    public int version;
    public short pageWidth;
    public short pageHeight;
    public short maxCharWidth;
    public short maxCharHeight;
    public short flags;
    public short ascent;
    public short numGlyphs;
    public string translateTable;
}