#define UTLHANDLETABLE_H

namespace SourceSharp.mp.src._public.tier1
{
    using SourceSharp.sp.src._public.zip;
    using UtlHandle_t = uint;

    public class utlhandletable
    {
        public const uint UTLHANDLE_INVALID = 0;

        public class CUtlHandleTable
        {
            protected static dynamic T;
            protected static int HandleBits = 16;

            public CUtlHandleTable()
            {
                m_nValidHandles = 0;
            }

            public UtlHandle_t AddHandle()
            {
                uint nIndex = (m_unused.Count() > 0) ? m_unused.RemoveAtHead() : m_list.AddToTail();

                EntryType_t entry = m_list[nIndex];
                entry.nInvalid = 0;
                entry.m_pData = null;

                ++m_nValidHandles;

                return CreateHandle(entry.m_nSerial, nIndex);
            }

            public void RemoveHandle(UtlHandle_t handle)
            {
                uint nIndex = GetListIndex(handle);
                xzip.Assert(nIndex < (uint)m_list.Count());

                if (nIndex >= (uint)m_list.Count())
                {
                    return;
                }

                EntryType_t entry = m_list[nIndex];
                ++entry.m_nSerial;

                if (!entry.nInvalid)
                {
                    entry.nInvalid = 1;
                    --m_nValidHandles;
                }

                entry.m_pData = null;

                bool bStopUsing = (entry.m_nSerial >= ((1 << (31 - HandleBits)) - 1));

                if (!bStopUsing)
                {
                    m_unused.Insert(nIndex);
                }
            }

            public void SetHandle(UtlHandle_t h, dynamic pData)
            {
                EntryType_t? entry = GetEntry(h, false);
                xzip.Assert(cond: entry != null);

                if (entry == null)
                {
                    return;
                }

                if (entry.nInvalid != 0)
                {
                    ++m_nValidHandles;
                    entry.nInvalid = 0;
                }

                entry.m_pData = pData;
            }

            public dynamic GetHandle(UtlHandle_t h)
            {
                EntryType_t? entry = GetEntry(h, true);
                return entry == null ? entry.m_pData : null;
            }

            public dynamic GetHandle(UtlHandle_t h, bool checkValidity)
            {
                EntryType_t? entry = GetEntry(h, checkValidity);
                return entry == null ? entry.m_pData : null;
            }

            public bool IsHandleValid(UtlHandle_t h)
            {
                if (h == UTLHANDLE_INVALID)
                {
                    return false;
                }

                uint nIndex = GetListIndex(h);
                xzip.AssertOnce(nIndex < (uint)m_list.Count());

                if (nIndex >= (uint)m_list.Count())
                {
                    return false;
                }

                EntryType_t entry = m_list[nIndex];

                if (entry.m_nSerial != GetSerialNumber(h))
                {
                    return false;
                }

                if (1 == entry.nInvalid)
                {
                    return false;
                }

                return true;
            }

            public uint GetValidHandleCount()
            {
                return m_nValidHandles;
            }

            public uint GetHandleCount()
            {
                return m_list.Count();
            }

            public UtlHandle_t GetHandleFromIndex(int i)
            {
                if (m_list[i].m_pData)
                {
                    return CreateHandle(m_list[i].m_nSerial, i);
                }

                return UTLHANDLE_INVALID;
            }

            public int GetIndexFromHandle(UtlHandle_t h)
            {
                if (h == UTLHANDLE_INVALID)
                {
                    return -1;
                }

                return (int)GetListIndex(h);
            }

            public void MarkHandleInvalid(UtlHandle_t h)
            {
                if (h == UTLHANDLE_INVALID)
                {
                    return;
                }

                uint nIndex = GetListIndex(h);
                xzip.Assert(nIndex < (uint)m_list.Count());

                if (nIndex >= (uint)m_list.Count())
                {
                    return;
                }

                EntryType_t entry = m_list[nIndex];

                if (entry.m_nSerial != GetSerialNumber(h))
                {
                    return;
                }

                if (entry.nInvalid == 0)
                {
                    --m_nValidHandles;
                    entry.nInvalid = 1;
                }
            }

            public void MarkHandleValid(UtlHandle_t h)
            {
                if (h == UTLHANDLE_INVALID)
                {
                    return;
                }

                uint nIndex = GetListIndex(h);
                xzip.Assert(nIndex < (uint)m_list.Count());

                if (nIndex >= m_list.Count())
                {
                    return;
                }

                EntryType_t entry = m_list[nIndex];

                if (entry.m_nSerial != GetSerialNumber(h))
                {
                    return;
                }

                if (entry.nInvalid == 1)
                {
                    ++m_nValidHandles;
                    entry.nInvalid = 0;
                }
            }

            private struct HandleType_t
            {
                public HandleType_t(uint i, uint s)
                {
                    nIndex = i;
                    nSerial = s;
                    xzip.Assert(cond: i < (1 << HandleBits));
                    xzip.Assert(cond: s < (1 << (31 - HandleBits)));
                }

                private uint nIndex; // : HandleBits;
                private uint nSerial; // : 31 - HandleBits;
            }

            private struct EntryType_t
            {
                public EntryType_t()
                {
                    m_nSerial = 0;
                    nInvalid = 0;
                    m_pData = 0;
                }

                public uint m_nSerial; // : 31;
                public uint nInvalid; // : 1;
                public dynamic m_pData;
            }

            private unsafe static uint GetSerialNumber(UtlHandle_t handle)
            {
                return ((HandleType_t*)&handle)->nSerial;
            }

            private unsafe static uint GetListIndex(UtlHandle_t handle)
            {
                return ((HandleType_t*)&handle)->nIndex;
            }

            private unsafe static UtlHandle_t CreateHandle(uint nSerial, uint nIndex)
            {
                HandleType_t h = new HandleType_t(nIndex, nSerial);
                return *(UtlHandle_t*)&h;
            }

            private EntryType_t? GetEntry(UtlHandle_t handle, bool checkValidity)
            {
                if (handle == UTLHANDLE_INVALID)
                {
                    return null;
                }

                uint nIndex = GetListIndex(handle);
                xzip.Assert(nIndex < (uint)m_list.Count());

                if (nIndex >= (uint)m_list.Count())
                {
                    return null;
                }

                EntryType_t entry = m_list[nIndex];

                if (entry.m_nSerial != GetSerialNumber(handle))
                {
                    return null;
                }

                if (checkValidity && (1 == entry.nInvalid))
                {
                    return null;
                }

                return entry;
            }

            private uint m_nValidHandles;
            private CUtlVector m_list;
            private CUtlQueue m_unused;
        }

        public class CUtlHandle
        {
            private dynamic T;

            public CUtlHandle()
            {
                m_handle = UTLHANDLE_INVALID;
            }

            public CUtlHandle(object pObject)
            {
                Set(pObject);
            }

            public CUtlHandle(UtlHandle_t h)
            {
                m_handle = h;
            }

            public CUtlHandle(CUtlHandle h)
            {
                m_handle = h.m_handle;
            }

            public void Set(dynamic pObject)
            {
                m_handle = pObject != null ? pObject.GetHandle() : UTLHANDLE_INVALID;
            }

            public void Set(UtlHandle_t h)
            {
                m_handle = h;
            }

            public object Get()
            {
                return T.GetPtrFromHandle(m_handle);
            }

            public bool IsValid()
            {
                return T.IsHandleValid(m_handle);
            }

            public static bool operator ==(CUtlHandle lhs, CUtlHandle rhs)
            {
                return lhs.m_handle == rhs.m_handle;
            }

            public static bool operator ==(CUtlHandle lhs, dynamic rhs)
            {
                UtlHandle_t h = rhs != null ? rhs.GetHandle() : UTLHANDLE_INVALID;
                return lhs.m_handle == h;
            }

            public static bool operator !=(CUtlHandle lhs, CUtlHandle rhs)
            {
                return lhs.m_handle != rhs.m_handle;
            }

            public static bool operator !=(CUtlHandle lhs, dynamic rhs)
            {
                UtlHandle_t h = rhs != null ? rhs.GetHandle() : UTLHANDLE_INVALID;
                return lhs.m_handle != h;
            }

            private UtlHandle_t m_handle;
        }

        public class DECLARE_HANDLES(string _className, int _handleBitCount)
        {
            public UtlHandle_t GetHandle()
            {
                return m_Handle;
            }

            public static string GetPtrFromHandle(UtlHandle_t h) 
            {
                return m_HandleTable.GetHandle(h);
            }

            public static bool IsHandleValid(UtlHandle_t h)
            {
                return m_HandleTable.IsHandleValid(h);
            }

            private UtlHandle_t m_Handle;
            private static CUtlHandleTable m_HandleTable;
        }
    }
}
