using SourceSharp.SP.Public.Tier1;

namespace SourceSharp.SP.Public.DmxLoader;

public class CDmxAttribute
{
    private DmAttributeType type;
    private CUtlSymbol name;
    private nint[] data;

    private static CUtlSymbolTableMT attributeNameSymbols;

    public DmAttributeType GetType()
    {
        return type;
    }

    public string GetTypeString()
    {

    }

    public bool IsA<T>()
    {
        return GetType() == CDmAttributeInfo<T>.ATTRIBUTE_TYPE;
    }

    public string GetName()
    {

    }

    public CUtlSymbol GetNameSymbol()
    {
        return name;
    }

    public void SetName(string name)
    {

    }

    public T GetValue<T>()
    {
        if (CDmAttributeInfo<T>.AttributeType() == type)
        {
            return (T)data;
        }

        T defaultValue = default;
        CDmAttributeInfo<T>.SetDefaultValue(defaultValue);
        return defaultValue;
    }

    public CUtlVector<T> GetArray<T>()
    {
        if (CDmAttributeInfo<CUtlVector<T>>.AttributeType() == type)
        {
            return (CUtlVector<T>)data;
        }

        AllocateDataMemory(CDmAttributeInfo<CUtlVector<T>>.AttributeType());
        Platform.Construct((CUtlVector<T>)data);
        return (CUtlVector<T>)data;
    }

    public string GetValueString()
    {
        if (type == DmAttributeType.AT_STRING)
        {
            return (CUtlString[])data;
        }

        return "";
    }

    public void SetValue<T>(T value)
    {
        AllocateDataMemory(CDmAttributeInfo<T>.AttributeType());
        Platform.CopyConstruct((T[])data, value);
    }

    public void SetValue(string @string)
    {

    }

    public void SetValue(nint[] buffer, long len)
    {

    }

    public void SetValue(CDmxAttribute attribute)
    {

    }

    public CUtlVector<T> GetArrayForEdit<T>()
    {

    }

    public void SetToDefaultValue()
    {

    }

    public void SetValueFromString(string value)
    {

    }

    public string GetValueAsString(byte[] buffer, long bufLen)
    {

    }

    public int GetArrayCount()
    {

    }

    public bool Unserialize(DmAttributeType type, CUtlBuffer buf)
    {

    }

    public bool UnserializeElement(DmAttributeType type, CUtlBuffer buf)
    {

    }

    public bool Serialize(CUtlBuffer buf)
    {

    }

    public bool SerializeElement(int index, CUtlBuffer buf)
    {

    }

    public bool SerializesOnMultipleLines()
    {

    }

    public static int AttributeDataSize(DmAttributeType type)
    {

    }

    private CDmxAttribute(string attributeName)
    {

    }

    private CDmxAttribute(CUtlSymbol attributeName)
    {

    }

    ~CDmxAttribute()
    {

    }

    private void AllocateDataMemory(DmAttributeType type)
    {

    }

    private void FreeDataMemory()
    {

    }

    private void SetValue(DmAttributeType type, nint[] src, int len)
    {

    }
}