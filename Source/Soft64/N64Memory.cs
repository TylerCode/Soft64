﻿using Soft64.RCP;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft64
{
    /* Experimental WIP fast n64 physical memory system */

    public class N64Memory : Stream
    {
        private MemorySection m_RDRam;
        private RspMemory m_RspMemory;
        private MemorySection m_PifMemory;
        private CartridgeHeapStream m_CartMemory;
        private MemorySection[] m_SectionMap;
        [ThreadStatic]
        private Int64 m_Position;
        private Boolean m_Disposed;

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
                return 0x100000000;
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

        public Boolean Disposed
        {
            get { return m_Disposed; }
        }

        public N64Memory()
        {
        }

        public void Initialize()
        {
            CheckDispose();
            m_SectionMap = new MemorySection[0x10000];
            m_RDRam = new MemorySection(0x100000, 0x00000000);
            m_PifMemory = new MemorySection(0x800, 0x1FC00000);
            m_RspMemory = new RspMemory();

            if (m_CartMemory != null)
            {
                AddStream(m_CartMemory);
            }

            /* Setup the region hashtable */
            AddStream(m_RDRam);
            AddStream(m_RspMemory.SPRam);
            AddStream(m_RspMemory.RegMemoryStream);
            AddStream(m_PifMemory);
        }

        public override void Flush()
        {
            CheckDispose();
            // TODO:
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        private static int GetKey(long address)
        {
            return (Int32)((0xFFFF0000L & address) >> 16);
        }

        private void AddStream(MemorySection stream)
        {
            Int64 byteCount = stream.Length;
            Int32 startKey = (Int32)stream.BasePosition >> 16;

        }

        private void Access(Action<Stream,byte[],int,int> op, 
                            byte[] buffer, int offset, int count, 
                            Int32 key)
        {
            if (count <= 0)
                return;

            /* Pull out the memory section */
            MemorySection section = m_SectionMap[key];

            /* If the section exists, read from its data */
            if (section != null)
            {
                /* Set the position on the stream */
                section.Position = Position - section.BasePosition;

                /* Compute the number of bytes to access */
                Int32 length = (Int32)(Math.Min(section.BasePosition + section.Length, section.Position + count) - section.Position);

                /* Call the access op */
                op(section, buffer, offset, length);

                /* Increment buffer offset, decrement count, and increment position */
                offset += length;
                count -= length;
                Position += length;

                /* Access possible section */
                Access(op, buffer, offset, count, (Int32)Position >> 16);
            }
            else
            {
                /* Skip non-existing section */
                Position += 0x10000;
                Access(op, buffer, offset + 0x10000, count -= 0x10000, key + 1);
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            Array.Clear(buffer, offset, count);
            Access((a, b, c, d) => a.Read(b, c, d), buffer, offset, count, (Int32)Position >> 16);
            return count;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            Access((a, b, c, d) => a.Write(b, c, d), buffer, offset, count, (Int32)Position >> 16);
        }

        private void CheckDispose()
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(N64Memory));
        }

        protected override void Dispose(bool disposing)
        {
            if (!m_Disposed)
            {
                base.Dispose(disposing);

                if (disposing)
                {

                }

                /* TODO: */

                m_Disposed = true;
            }
        }

        public RspMemory Rsp => m_RspMemory;

        public MemorySection Ram => m_RDRam;

        public RcpInterfaceMemory MI { get; internal set; }

        public ParallelInterfaceMemory PI { get; internal set; }

        internal void SetCartridgeSource(Cartridge cart)
        {
            m_CartMemory = new CartridgeHeapStream(cart);
        }
    }
}
