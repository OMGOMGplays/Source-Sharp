using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using SourceSharp.SP.Public.Tier1;

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
        if (outputBuffer == null || baseData == null)
        {
            throw new ArgumentNullException("Buffers cannot be null.");
        }

        int objectSize = Marshal.SizeOf<T>();
        if (baseData.Length < objectSize * objectCount || outputBuffer.Length < objectSize * objectCount)
        {
            throw new ArgumentException("Buffer sizes are insufficient for the object count.");
        }

        int outputOffset = 0;
        int baseOffset = 0;

        nint tempPtr = Marshal.AllocHGlobal(objectSize);

        try
        {
            for (int i = 0; i < objectCount; ++i)
            {
                Marshal.Copy(baseData, baseOffset, tempPtr, objectSize);
                T tempObject = Marshal.PtrToStructure<T>(tempPtr);
                swap.SwapFieldsToTargetEndian(ref tempObject);
                Marshal.StructureToPtr(tempObject, tempPtr, false);
                Marshal.Copy(tempPtr, outputBuffer, outputOffset, objectSize);
                outputOffset += objectSize;
                baseOffset += objectSize;
            }
        }
        finally
        {
            Marshal.FreeHGlobal(tempPtr);
        }
    }

    private static unsafe void WriteObjects<T>(ref byte outputBuffer, ref byte baseData, int objectCount = 1) where T : class
    {
        int objectSize = Marshal.SizeOf<T>();

        // Pin the starting address of the byte data and use pointers for memory manipulation.
        fixed (byte* basePtr = &baseData)
        fixed (byte* outputPtr = &outputBuffer)
        {
            byte* currentBasePtr = basePtr;
            byte* currentOutputPtr = outputPtr;

            for (int i = 0; i < objectCount; ++i)
            {
                T tempObject;
                nint tempPtr = Marshal.AllocHGlobal(objectSize);

                try
                {
                    // Copy from base data memory to tempPtr (unmanaged memory).
                    Buffer.MemoryCopy(currentBasePtr, (void*)tempPtr, objectSize, objectSize);

                    // Read structure from tempPtr.
                    tempObject = Marshal.PtrToStructure<T>(tempPtr);

                    // Perform endian swap or other manipulation.
                    swap.SwapFieldsToTargetEndian(ref tempObject);

                    // Write the structure back to unmanaged memory.
                    Marshal.StructureToPtr(tempObject, tempPtr, false);

                    // Copy from tempPtr (unmanaged memory) to the output buffer.
                    Buffer.MemoryCopy((void*)tempPtr, currentOutputPtr, objectSize, objectSize);

                    // Advance the pointers by object size.
                    currentBasePtr += objectSize;
                    currentOutputPtr += objectSize;
                }
                finally
                {
                    Marshal.FreeHGlobal(tempPtr); // Free the unmanaged memory.
                }
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
        if (outputBuffer == null || baseData == null)
        {
            throw new ArgumentNullException("Buffers cannot be null.");
        }

        int objectSize = Marshal.SizeOf<T>();
        if (baseData.Length < objectSize * objectCount || outputBuffer.Length < objectSize * objectCount)
        {
            throw new ArgumentException("Buffer sizes are insufficient for the object count.");
        }

        int outputOffset = 0;
        int baseOffset = 0;

        nint tempPtr = Marshal.AllocHGlobal(objectSize);

        try
        {
            for (int i = 0; i < objectCount; ++i)
            {
                Marshal.Copy(baseData, baseOffset, tempPtr, objectSize);
                T tempObject = Marshal.PtrToStructure<T>(tempPtr);
                swap.SwapFieldsToTargetEndian(ref tempObject);
                Marshal.StructureToPtr(tempObject, tempPtr, false);
                Marshal.Copy(tempPtr, outputBuffer, outputOffset, objectSize);
                outputOffset += objectSize;
                baseOffset += objectSize;
            }
        }
        finally
        {
            Marshal.FreeHGlobal(tempPtr);
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

        TypeDescription fields = datamap.dataDesc;
        int fieldCount = datamap.dataNumFields;

        for (int i = 0; i < fieldCount; i++)
        {
            TypeDescription field = fields[i];
            ProcessField(baseAddress, data + field.fieldOffset[TD_OFFSET_NORMAL], field, pfnProcessFunc);
        }
    }

    private void ProcessFields(nint data, DataMap datamap, DataDescProcessFunc pfnProcessFunc)
    {
        ProcessFields(data, data, datamap, pfnProcessFunc);
    }

    private void ProcessFieldByName(nint baseAddress, nint data, DataMap datamap, string name, DataDescProcessFunc pfnProcessFunc)
    {
        if (datamap.baseMap != null)
        {
            ProcessFieldByName(baseAddress, data, datamap.baseMap, pfnProcessFunc);
        }

        TypeDescription fields = datamap.dataDesc;
        int fieldCount = datamap.dataNumFields;

        for (int i = 0; i < fieldCount; i++)
        {
            TypeDescription field = fields[i];

            if (field.fieldName == name)
            {
                ProcessField(baseAddress, data + field.fieldOffset[TD_OFFSET_NORMAL], field, pfnProcessFunc);
                break;
            }
        }
    }

    private void ProcessFieldByName(nint data, DataMap datamap, string name, DataDescProcessFunc pfnProcessFunc)
    {
        ProcessFieldByName(data, data, datamap, name, pfnProcessFunc);
    }

    private delegate void ProcessANIFields(nint dataBase, DataDescProcessFunc pfnProcessFunc);
    private delegate void ProcessMDLFields(nint dataBase, DataDescProcessFunc pfnProcessFunc);

    private int ByteswapStudioFile(string filename, nint outBase, nint fileBase, int filesize, StudioHDR hdr, CompressFunc compressFunc = null)
    {

    }

    private int ByteswapPHY(nint destBase, nint srcBase, int fileSize)
    {
        Debug.Assert(collision == null);

        if (collision == null)
        {
            return 0;
        }

        destBase = fileSize;

        byte src = (byte)srcBase;
        byte dest = (byte)destBase;
        VCollide collide = new { 0 };

        PhyHeader hdr = (PhyHeader)(nativeSrc ? src : dest);
        WriteObjects<PhyHeader>(ref dest, ref src);

        if (nativeSrc)
        {
            src = (byte)srcBase + hdr.size;
            dest = (byte)destBase + hdr.size;

            int bufSize = fileSize - hdr.size;
            collision.VCollideLoad(collide, hdr.solidCount, src.ToString(), bufSize, false);
        }

        for (int i = 0; i < hdr.solidCount; i++)
        {
            SwapCompactSurfaceHeader baseHdr = (SwapCompactSurfaceHeader)(nativeSrc ? src : dest);
            WriteObjects<SwapCompactSurfaceHeader>(ref dest, ref src);

            int srcIncrement = baseHdr.surfaceSize + Marshal.SizeOf<SwapCompactSurfaceHeader>();
            int destIncrement = srcIncrement;
            bool copyToSrc = !nativeSrc;

            if (baseHdr.vphysicsID != MAKEID("V", "P", "H", "Y"))
            {
                LegacySurfaceHeader legacyHdr = (LegacySurfaceHeader)(nativeSrc ? src : dest);
                WriteObjects<LegacySurfaceHeader>(ref dest, ref src);

                if (legacyHdr.dummy[2] == MAKEID("I", "V", "P", "S") || legacyHdr.dummy[2] == 0)
                {
                    srcIncrement = legacyHdr.byte_size + sizeof(int);
                    destIncrement = legacyHdr.byte_size + Marshal.SizeOf<SwapCompactSurfaceHeader>();
                    copyToSrc = false;

                    if (!nativeSrc)
                    {
                        src = dest;
                    }
                }
                else
                {
                    Debug.Assert(false);
                    return 0;
                }
            }

            if (copyToSrc)
            {
                src = dest;
            }

            src += (byte)srcIncrement;
            dest += (byte)destIncrement;
        }

        int currPos = src - (byte)srcBase;
        int remainingBytes = fileSize - currPos;
        WriteBuffer<char>(ref dest, ref src, remainingBytes);

        if (!nativeSrc)
        {
            src = (byte)srcBase + hdr.size;
            int bufSize = fileSize - hdr.size;
            collision.VCollideLoad(collide, hdr.solidCount, src.ToString(), bufSize, true);
        }

        dest = (byte)destBase + hdr.size;

        for (int i = 0; i < collide.solidCount; i++)
        {
            dest += sizeof(int);
            int offset = collision.ColliderWrite(dest.ToString(), collide.solids[i], nativeSrc);
            int destSize = nativeSrc ? SwapLong(offset) : offset;
            destSize = dest - sizeof(int);
            dest += (byte)offset;
        }

        collision.CollideUnload(ref collide);

        int newFileSize = dest - (byte)destBase + remainingBytes;

        if (compressFunc != null)
        {
            nint input = destBase;
            int inputSize = newFileSize;
            nint output;
            int outputSize;

            if (CompressFunc(input, inputSize, output, outputSize))
            {
                destBase = output;
                output = null;
                newFileSize = outputSize;
            }
        }

        return newFileSize;
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

public struct SwapCompactSurfaceHeader
{
    public int size;
    public int vphysicsID;
    public short version;
    public short modelType;
    public int surfaceSize;
    public Vector dragAxisAreas;
    public int axisMapSize;
}