using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Soft64
{
    public unsafe class MemorySection : Stream
    {
        [ThreadStatic]
        private Int64 m_Position;
        private Int64 m_BaseAddress;
        private IntPtr m_Pointer;
        private Byte* m_RawMemPointer;
        private Int32 m_HeapSize;
        private Boolean m_Disposed;
        private HeapAccessMode m_Mode;
        private GCHandle m_PinnedBufferHandle;
        private Byte[] m_Buffer;

        public MemorySection(Int32 heapSize, Int64 baseAddress)
        {
            m_BaseAddress = baseAddress;
            m_HeapSize = heapSize;
            Allocate(heapSize);
        }

        protected virtual void Allocate(Int32 size)
        {
            /* Create an instance of a managed byte buffer, to ensure everythign is set to 0 */
            m_Buffer = new Byte[size];

            /* Pin the buffer so garbage collector doesn't move it around in memory */
            m_PinnedBufferHandle = GCHandle.Alloc(m_Buffer, GCHandleType.Pinned);

            /* Setup the pointers */
            SetPointer(m_PinnedBufferHandle.AddrOfPinnedObject());
        }

        protected void SetPointer(IntPtr p)
        {
            m_Pointer = p;
            m_RawMemPointer = (Byte*)m_Pointer.ToPointer();
        }

        public HeapAccessMode AccessMode
        {
            get { return m_Mode; }
            set { m_Mode = value; }
        }

        public Boolean IsDisposed
        {
            get { return m_Disposed; }
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override long Length
        {
            get
            {
                return m_HeapSize;
            }
        }

        public override long Position
        {
            get
            {
                return m_Position;
            }

            set
            {
                m_Position = value;
            }
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            unsafe
            {
                Int32 givenCount = count;

                m_Mode = HeapAccessMode.Read;

                /* Check for possible memory violations */
                Check(BasePosition, buffer, ref offset, ref count);

                try
                {
                    Byte* ptr = GetPointer();
                    for (Int32 i = 0; i < count; i++)
                    {
                        buffer[i + offset] = *(ptr + i);
                    }
                }
                catch
                {
                    return givenCount;
                }

                return givenCount;
            }
        }

        private void Check(Int64 baseOffset, byte[] buffer, ref int offset, ref int count)
        {
            Int64 sectionPos = m_Position - baseOffset;

            if ((Int32)sectionPos >= m_HeapSize)
                count = 0;
            else
            {
                /* Crop the count if we need too, to avoid pointer violations */
                if (((Int32)sectionPos + count) >= m_HeapSize)
                {
                    count = Math.Min(0, count -= (m_HeapSize + ((Int32)sectionPos + count)) - m_HeapSize);
                }
            }

            if (offset >= buffer.Length || (offset + (count - 1)) >= buffer.Length)
                throw new ArgumentOutOfRangeException();
        }

        private unsafe Byte * GetPointer()
        {
            return (Byte*)(m_RawMemPointer + (Int32)m_Position);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            unsafe
            {
                m_Mode = HeapAccessMode.Write;

                /* Check for possible memory violations */
                Check(BasePosition, buffer, ref offset, ref count);

                try
                {
                    Byte* ptr = GetPointer();
                    for (Int32 i = 0; i < count; i++)
                    {
                        *(ptr + i) = buffer[i + offset];
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!m_Disposed)
            {
                base.Dispose(disposing);

                if (disposing)
                {

                }

                /* Unpin the buffer */
                m_PinnedBufferHandle.Free();
                SetPointer(IntPtr.Zero);

                m_Disposed = true;
            }
        }

        internal IntPtr HeapPointer
        {
            get { return m_Pointer; }
        }

        public Int64 BasePosition
        {
            get { return m_BaseAddress; }
        }
    }

    public enum HeapAccessMode : int
    {
        Read,
        Write
    }
}
