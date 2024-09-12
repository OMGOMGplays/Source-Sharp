using SourceSharp.SP.Public.Tier1;
using System;
using System.Runtime.InteropServices;

namespace SourceSharp.SP.Common;

struct StudioHDR;
class IPhysicsCollision;

public class StudioByteSwap
{
    public const int BYTESWAP_ALIGNMENT_PADDING = 4096;

    private static bool verbose = true;
    private static bool nativeSrc;
    private static CByteSwap swap;
    private static IPhysicsCollision collision;
    private static CompressFunc compressFunc;

    private delegate bool CompressFunc(object input, int inputSize, object output, int outputSize);

    private void ActivateByteSwapping(bool activate)
    {
        swap.ActivateByteSwapping(activate);
        SourceIsNative(IsPC());
    }

    private void SourceIsNative(bool native)
    {
        nativeSrc = native;
    }

    private void SetVerbose(bool setVerbose)
    {
        verbose = setVerbose;
    }

    private void SetCollisionInterface(IPhysicsCollision physicsCollision)
    {
        collision = physicsCollision;
    }

    private static void WriteObjects<T>(ref byte[] outputBuffer, ref byte[] baseData, int objectCount = 1) where T : class
    {
        int objectSize = Marshal.SizeOf<T>();
        int outputOffset = 0;
        int baseOffset = 0;

        for (int i = 0; i < objectCount; ++i)
        {
            T tempObject = default;
            nint tempPtr = Marshal.AllocHGlobal(objectSize);

            try
            {
                Marshal.Copy(baseData, baseOffset, tempPtr, objectSize);
                tempObject = Marshal.PtrToStructure<T>(tempPtr);
                swap.SwapFieldsToTargetEndian(ref tempObject);
                Marshal.StructureToPtr(tempObject, tempPtr, false);
                Marshal.Copy(tempPtr, outputBuffer, outputOffset, objectSize);
                outputOffset += objectSize;
                baseOffset += objectSize;
            }
            finally
            {
                Marshal.FreeHGlobal(tempPtr);
            }
        }
    }

    private static void WriteObjects<T>(ref T[] outputBuffer, ref T[] baseData, int objectCount = 1) where T : class
    {
        for (int i = 0; i < objectCount; i++)
        {
            T tempObject = baseData[i];
            swap.SwapFieldsToTargetEndian(ref tempObject);

            outputBuffer[i] = tempObject;
        }
    }

    private static void WriteObjects<T>(byte[] outputBuffer, byte[] baseData, int objectCount = 1) where T : class
    {
        int objectSize = Marshal.SizeOf<T>();
        int outputOffset = 0;
        int baseOffset = 0;

        for (int i = 0; i < objectCount; ++i)
        {
            T tempObject = default;
            nint tempPtr = Marshal.AllocHGlobal(objectSize);

            try
            {
                Marshal.Copy(baseData, baseOffset, tempPtr, objectSize);
                tempObject = Marshal.PtrToStructure<T>(tempPtr);
                swap.SwapFieldsToTargetEndian(ref tempObject);
                Marshal.StructureToPtr(tempObject, tempPtr, false);
                Marshal.Copy(tempPtr, outputBuffer, outputOffset, objectSize);
                outputOffset += objectSize;
                baseOffset += objectSize;
            }
            finally
            {
                Marshal.FreeHGlobal(tempPtr);
            }
        }
    }

    private static void WriteObjects<T>(T[] outputBuffer, T[] baseData, int objectCount = 1) where T : class
    {
        for (int i = 0; i < objectCount; i++)
        {
            T tempObject = baseData[i];
            swap.SwapFieldsToTargetEndian(ref tempObject);

            outputBuffer[i] = tempObject;
        }
    }

    private static void WriteBuffer<T>(ref byte[] outputBuffer, ref byte[] baseData, int objectCount = 1) where T : class
    {
        int objectSize = Marshal.SizeOf<T>();
        int outputOffset = 0;
        int baseOffset = 0;

        for (int i = 0; i < objectCount; ++i)
        {
            T tempObject = default;
            nint tempPtr = Marshal.AllocHGlobal(objectSize);

            try
            {
                Marshal.Copy(baseData, baseOffset, tempPtr, objectSize);
                tempObject = Marshal.PtrToStructure<T>(tempPtr);
                swap.SwapBufferToTargetEndian(ref tempObject);
                Marshal.StructureToPtr(tempObject, tempPtr, false);
                Marshal.Copy(tempPtr, outputBuffer, outputOffset, objectSize);
                outputOffset += objectSize;
                baseOffset += objectSize;
            }
            finally
            {
                Marshal.FreeHGlobal(tempPtr);
            }
        }
    }

    private static void WriteBuffer<T>(byte[] outputBuffer, byte[] baseData, int objectCount = 1) where T : class
    {
        int objectSize = Marshal.SizeOf<T>();
        int outputOffset = 0;
        int baseOffset = 0;

        for (int i = 0; i < objectCount; ++i)
        {
            T tempObject = default;
            nint tempPtr = Marshal.AllocHGlobal(objectSize);

            try
            {
                Marshal.Copy(baseData, baseOffset, tempPtr, objectSize);
                tempObject = Marshal.PtrToStructure<T>(tempPtr);
                swap.SwapBufferToTargetEndian(ref tempObject);
                Marshal.StructureToPtr(tempObject, tempPtr, false);
                Marshal.Copy(tempPtr, outputBuffer, outputOffset, objectSize);
                outputOffset += objectSize;
                baseOffset += objectSize;
            }
            finally
            {
                Marshal.FreeHGlobal(tempPtr);
            }
        }
    }

    private T SrcNative<T>(ref T idx)
    {
        T ret = idx;

        if (!nativeSrc)
        {
            swap.SwapBuffer(ref ret, ref idx);
        }

        return ret;
    }

    private T DestNative<T>(ref T idx)
    {
        T ret = idx;

        if (nativeSrc)
        {
            swap.SwapBuffer(ref ret, ref idx);
        }

        return ret;
    }

    private delegate nint DataDescProcessFunc(nint @base, nint data, TypeDescription fields);
    private delegate nint PFNFixupFunc(nint destBase, DataDescProcessFunc dataDescProcessFunc);

    private static PFNFixupFunc pfnFileProcessFunc;
    private static StudioHDR hdr;
    private static nint dataSrcBase;
    private static nint fixpoint;
    private static int fixupBytes;

    private bool UpdateIndex(nint @base, ref int indexMember)
    {
        bool updateIndex = false;
        int idx = indexMember;

        if (@base < fixpoint)
        {
            if ((byte)@base + idx >= fixpoint)
            {
                updateIndex = true;
            }
        }
        else
        {
            if ((byte)@base + idx < fixpoint)
            {
                updateIndex = false;
            }
        }

        if (updateIndex && indexMember != 0)
        {
            indexMember = idx + fixupBytes * Math.Sign(idx);
            return true;
        }

        return false;
    }

    private int GetIntegerFromField(ref int data, FieldTypes fieldType)
    {
        if (fieldType == FieldTypes.FIELD_INTEGER)
        {
            return SrcNative(ref data);
        }
        else if (fieldType == FieldTypes.FIELD_SHORT)
        {
            return SrcNative(ref data);
        }

        Error("Byteswap macro DEFINE_INDEX using unsupported fieldtype {0}\n", fieldType);
        return 0;
    }

    private void PutIntegerInField(nint data, int index, FieldTypes fieldType)
    {
        if (fieldType == FieldTypes.FIELD_INTEGER)
        {
            data = SrcNative(ref index);
        }
        else if (fieldType == FieldTypes.FIELD_SHORT)
        {
            data = (short)SrcNative(ref index);
        }
        else
        {
            Error("Byteswap macro DEFINE_INDEX using unsupported fieldType {0}\n", fieldType);
        }
    }

    private void UpdateSrcIndexFields(nint @base, nint data, TypeDescription field)
    {
        if (field.flags &= FTYPEDESC_INDEX)
        {
            int index = GetIntegerFromField(ref data, field.fieldType);

            if (UpdateIndex(@base, ref index))
            {
                PutIntegerInField(data, index, field.fieldType);
            }
        }
    }

    private void ProcessField(nint @base, nint data, TypeDescription field, DataDescProcessFunc pfn)
    {
        if (pfn != null)
        {
            pfn.Invoke(@base, data, field);
        }
    }

    private void ProcessFields(nint baseAddress, nint data, DataMap datamap, DataDescProcessFunc pfnProcessFunc)
    {
        if (datamap.baseMap != null)
        {
            ProcessFields(baseAddress, data, datamap.baseMap, pfnProcessFunc);
        }
    }

    private int ByteswapStudioFile(string filename, nint outBase, nint fileBase, int filesize, StudioHDR hdr, CompressFunc compressFunc = null)
    {

    }

    private int ByteswapPHY(nint outBase, nint fileBase, int filesize)
    {

    }

    private int ByteswapANI(StudioHDR hdr, nint outBase, nint fileBase, int filesize)
    {

    }

    private int ByteswapVVD(nint outBase, nint fileBase, int filesize)
    {

    }

    private int ByteswapVTX(nint outBase, nint fileBase, int filesize)
    {

    }

    private int ByteswapMDL(nint outBase, nint fileBase, int filesize)
    {

    }
}