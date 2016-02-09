using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Soft64
{
    public unsafe sealed class FastHeapStream : Stream
    {
        [ThreadStatic]
        private Int64 m_Position;
 
        private IntPtr m_Pointer;
        private Byte* m_RawMemPointer;
        private Int64 m_HeapSize;
        private Boolean m_Disposed;

        public FastHeapStream(Int32 heapSize)
        {
            m_HeapSize = heapSize;
            m_Pointer = Marshal.AllocHGlobal(heapSize);
            // TODO: make sure mem is zero'd out?
            m_RawMemPointer = (Byte*)m_Pointer.ToPointer();
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
            Int32 _count = 0;

            try
            {
                for (Int32 i = 0; i < count; i++)
                {
                    buffer[i + offset] = *(m_RawMemPointer + i);
                    _count++;
                }
            }
            catch
            {
                return _count;
            }

            return _count;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            try
            {
                for (Int32 i = 0; i < count; i++)
                {
                    *(m_RawMemPointer + i) = buffer[i + offset];
                }
            }
            catch
            {
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

                Marshal.FreeHGlobal(m_Pointer);

                m_Disposed = true;
            }
        }

        internal IntPtr HeapPointer
        {
            get { return m_Pointer; }
        }
    }
}
