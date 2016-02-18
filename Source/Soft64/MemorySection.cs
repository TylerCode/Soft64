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
        private Int64 m_SectionSize;
        private IntPtr m_Pointer;
        private Byte* m_RawMemPointer;
        private Int32 m_HeapSize;
        private Boolean m_Disposed;
        private GCHandle m_PinnedBufferHandle;
        private Byte[] m_Buffer;


        public MemorySection (Int32 heapSize, Int64 baseAddress) : this(heapSize, heapSize, baseAddress)
        {

        }

        public MemorySection(Int64 mappedSize, Int32 heapSize, Int64 baseAddress)
        {
            m_SectionSize = mappedSize;
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
            m_Pointer = m_PinnedBufferHandle.AddrOfPinnedObject();
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
                return m_SectionSize;
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

                /* Check for possible memory violations */
                Check(BasePosition, buffer, ref offset, ref count);

                try
                {
                    Byte* ptr =(Byte *)GetPointer(false, (Int32)Position);
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
            Int64 sectionHeapSize = (Int64)m_HeapSize;

            /* If position is outside the heap size, don't do any more IO */
            if (sectionPos >= m_HeapSize)
            {
                count = 0;
            }
            else
            {
                /* Truncate */
                Int64 lastPos = sectionPos + count;
                if (lastPos >= m_HeapSize)
                {
                    count -= (Int32)(lastPos - m_HeapSize);
                }
            }

            /* Check offset and count arguments are valid for the buffer */
            if (offset >= buffer.Length || (offset + (count - 1)) >= buffer.Length)
                throw new ArgumentOutOfRangeException();
        }

        public virtual IntPtr GetPointer(bool write, Int32 offset)
        {
            return IntPtr.Add(m_Pointer, offset);
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
                /* Check for possible memory violations */
                Check(BasePosition, buffer, ref offset, ref count);

                try
                {
                    Byte* ptr = (Byte*)GetPointer(true, (Int32)Position);
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
                m_Pointer = IntPtr.Zero;

                m_Disposed = true;
            }
        }

        public Int64 BasePosition
        {
            get { return m_BaseAddress; }
        }
    }
}
