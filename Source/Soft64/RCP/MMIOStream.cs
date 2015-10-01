using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Soft64.RCP
{
    public abstract class MmioStream : Stream
    {
        private Int32 m_MemorySize;
        private Byte[] m_Buffer;
        private Int64 m_Position;
        private IntPtr m_BufferPtr;
        private GCHandle m_PtrHandle;
        private Boolean m_Disposed;

        public event EventHandler<MmioWriteEventArgs> IoWrite;
        

        protected MmioStream(Int32 memorySize)
        {
            m_MemorySize = memorySize;
            m_Buffer = new Byte[memorySize];
            m_PtrHandle = GCHandle.Alloc(m_Buffer, GCHandleType.Pinned);
            m_BufferPtr = m_PtrHandle.AddrOfPinnedObject();
        }

        protected UInt32 ReadUInt32(Int32 offset)
        {
            unsafe
            {
                return *(uint*)(byte*)(m_BufferPtr + offset);
            }
        }

        protected void Write(Int32 offset, UInt32 value)
        {
            unsafe
            {
                *((uint*)(byte*)(m_BufferPtr + offset)) = value;
            }
        }

        protected Byte ReadByte(Int32 offset)
        {
            return m_Buffer[offset];
        }

        protected void Write(Int32 offset, Byte value)
        {
            m_Buffer[offset] = value;
        }

        protected UInt16 ReadUInt16(Int32 offset)
        {
            unsafe
            {
                return *(ushort*)(byte*)(m_BufferPtr + offset);
            }
        }

        protected void Write(Int32 offset, UInt16 value)
        {
            unsafe
            {
                *((ushort*)(byte*)(m_BufferPtr + offset)) = value;
            }
        }

        protected Int32 ReadInt32(Int32 offset)
        {
            return (Int32)ReadUInt32(offset);
        }

        protected void Write(Int32 offset, Int32 value)
        {
            Write(offset, (UInt32)value);
        }

        protected Int16 ReadInt16(Int32 offset)
        {
            return (Int16)ReadUInt16(offset);
        }

        protected void Write(Int32 offset, Int16 value)
        {
            Write(offset, (UInt16)value);
        }

        protected virtual void OnWrite(Int32 offset, Int32 length)
        {
            var e = IoWrite;

            if (e != null)
            {
                e(this, new MmioWriteEventArgs(offset, length));
            }
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
        }

        public override long Length
        {
            get { return m_MemorySize; }
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

        public override int Read(byte[] buffer, int offset, int count)
        {
            for (Int32 i = 0; i < count; i++)
            {
                buffer[offset + i] = m_Buffer[(Int32)Position++];
            }

            return count;
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
            Int32 o = (Int32)Position;

            for (Int32 i = 0; i < count; i++)
            {
                m_Buffer[(Int32)Position++] = buffer[offset + i];
            }

            OnWrite(o, count);
        }

        protected override void Dispose(Boolean disposing)
        {
            if (!m_Disposed)
            {
                if (disposing)
                {
                    base.Dispose(disposing);
                }

                m_PtrHandle.Free();

                m_Disposed = true;
            }
        }
    }

    public sealed class MmioWriteEventArgs : EventArgs
    {
        public Int32 Offset { get; private set; }
        public Int32 Length { get; private set; }

        public MmioWriteEventArgs(Int32 offset, Int32 length)
        {
            Offset = offset;
            Length = length;
        }
    }
}
