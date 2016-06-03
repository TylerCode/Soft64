using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Soft64
{
    public class RegistersMemorySection : MemorySection
    {
        Byte[] m_ReadMemory;
        Byte[] m_WriteMemory;
        IntPtr m_ReadPointer;
        IntPtr m_WritePointer;
        GCHandle m_ReadHandle;
        GCHandle m_WriteHandle;
        Boolean m_Disposed;

        public RegistersMemorySection(Int64 mappingSize, Int32 heapSize, Int64 baseOffset) : base(mappingSize, heapSize, baseOffset)
        {

        }

        protected override void Allocate(int size)
        {
            m_ReadMemory = new Byte[size];
            m_WriteMemory = new Byte[size];
            m_ReadHandle = GCHandle.Alloc(m_ReadMemory, GCHandleType.Pinned);
            m_WriteHandle = GCHandle.Alloc(m_WriteMemory, GCHandleType.Pinned);
            m_ReadPointer = m_ReadHandle.AddrOfPinnedObject();
            m_WritePointer = m_WriteHandle.AddrOfPinnedObject();
        }

        public override IntPtr GetPointer(bool write, Int32 offset)
        {
            if (!write)
            {
                return IntPtr.Add(m_ReadPointer, offset);
            }
            else
            {
                return IntPtr.Add(m_WritePointer, offset);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!m_Disposed)
            {
                m_ReadHandle.Free();
                m_WriteHandle.Free();
                m_ReadPointer = IntPtr.Zero;
                m_WritePointer = IntPtr.Zero;

                m_Disposed = true;
            }
        }
    }
}
