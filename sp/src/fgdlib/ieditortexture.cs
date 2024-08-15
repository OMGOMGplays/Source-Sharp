namespace FGDLib;

public class ieditortexture
{
    public const double DEFAULT_TEXTURE_SCALE = 0.25;
    public const int DEFAULT_LIGHTMAP_SCALE = 16;

    public const int TEXTURE_ID_NONE = -1;

    public const int drawCaption = 0x01;
    public const int drawResizeAlways = 0x02;
    public const int drawIcons = 0x04;
    public const int drawErrors = 0x08;
    public const int drawUsageCount = 0x10;

    public CUtlVector<IEditorTexture> EditorTextureList_t;
}

public enum TEXTUREFORMAT
{
    tfNone = -1,
    tfWAD = 0,
    tfWAL = 1,
    tfWAD3 = 2,
    tfWAD4 = 3,
    tfWAD5 = 4,
    tfVMT = 5,
    tfSprite = 6,
}

public struct DrawTexData_t
{
    public int nFlags;
    public int nUsageCount;
}

public class IEditorTexture
{
    public virtual int GetImageWidth()
    {
        return 0;
    }

    public virtual int GetImageHeight()
    {
        return 0;
    }

    public virtual int GetWidth()
    {
        return 0;
    }

    public virtual int GetHeight()
    {
        return 0;
    }

    public virtual float GetDecalScale()
    {
        return 0;
    }

    public virtual string GetName()
    {
        return null;
    }

    public virtual int GetShortName(string szShortName)
    {
        return 0;
    }

    public virtual int GetKeywords(string szKeywords)
    {
        return 0;
    }

    public virtual TEXTUREFORMAT GetTextureFormat()
    {
        return TEXTUREFORMAT.tfNone;
    }

    public virtual int GetSurfaceAttributes()
    {
        return 0;
    }

    public virtual int GetSurfaceContents()
    {
        return 0;
    }

    public virtual int GetSurfaceValue()
    {
        return 0;
    }

    public virtual CPalette GetPalette()
    {
        return null;
    }

    public virtual bool HasData()
    {
        return false;
    }

    public virtual bool HasPalette()
    {
        return false;
    }

    public virtual bool Load()
    {
        return false;
    }

    public virtual void Reload()
    {
        return;
    }

    public virtual bool IsLoaded()
    {
        return false;
    }

    public virtual string GetFileName()
    {
        return null;
    }

    public virtual bool IsWater()
    {
        return false;
    }

    public virtual int GetImageDataRGB(object pData = null)
    {
        return 0;
    }

    public virtual int GetImageDataRGBA(object pData = null)
    {
        return 0;
    }

    public virtual bool HasAlpha()
    {
        return false;
    }

    public virtual bool IsDummy()
    {
        return false;
    }

    public virtual int GetTextureID()
    {
        return 0;
    }

    public virtual void SetTextureID(int nTextureID)
    {
        return;
    }

    public virtual IMaterial GetMaterial()
    {
        return null;
    }
}