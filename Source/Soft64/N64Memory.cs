using Soft64.RCP;
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
            foreach (var s in m_Regions.Values)
            {
                s.Flush();
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Int32> GetRegions(Int64 address, Int32 count)
        {
            /* LINQ query on which sections are requested for access */
            Int32 lowestKey = GetKey(address);
            Int32 highestKey = GetKey(address + count);

            return
                from key in m_Regions.Keys.ToArray<Int32>()
                where key >= lowestKey && key <= highestKey
                select key;
        }

        private MemorySection GetRegion(Int64 address)
        {
            int key = GetKey(address);
            MemorySection stream = null;

            if (m_Regions.TryGetValue(key, out stream))
            {
                return stream;
            }
            else
            {
                return null;
            }
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

        private void Read(byte[] buffer, int offset, int count, Int32 key)
        {
            if (count <= 0)
                return;

            /* Pull out the memory section */
            MemorySection section = m_SectionMap[key];

            /* If the section exists, read from its data */
            if (section != null)
            {
                section.Position = Position - section.BasePosition;
                Int32 read = section.Read(buffer, offset, count);
                offset += read;
                count -= read;
                Position += read;
                Read(buffer, offset, count, (Int32)Position >> 16);
            }
            else
            {
                Position += 0x10000;
                Read(buffer, offset + 0x10000, count -= 0x10000, key + 1);
            }
        }

        private void Write(byte[] buffer, int offset, int count, Int32 key)
        {
            if (count <= 0)
                return;

            /* Pull out the memory section */
            MemorySection section = m_SectionMap[key];

            /* If the section exists, read from its data */
            if (section != null)
            {
                // TODO
                section.Position = Position - section.BasePosition;
                var write = count;
                section.Write(buffer, offset, count);
                offset += write;
                count -= write;
                Position += write;
                Write(buffer, offset, count, (Int32)Position >> 16);
            }
            else
            {
                Position += 0x10000;
                Write(buffer, offset + 0x10000, count -= 0x10000, key + 1);
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            Array.Clear(buffer, offset, count);
            Read(buffer, offset, count, (Int32)Position >> 16);
            return count;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            CheckDispose();
            var keys = GetRegions(m_Position, count);
            var newOffset = offset;
            Int64 lastPosition = 0;

            foreach (var key in keys)
            {
                /* Store the last position */
                lastPosition = Position;

                /* Turn the key into the section's offset */
                Int64 keyOffset = (Int64)(key << 16);
                MemorySection stream = m_Regions[key]; /* Get the associated stream */
                Position = keyOffset;

                /* If we skipped over non-accessable section, increment the buffer offset */
                if (Position >= lastPosition)
                {
                    newOffset += (Int32)(Position - lastPosition);
                }

                /* Do a full read when count goes past the section bounrary */
                Int64 end = Position + count;
                Int64 sEnd = Position + stream.Length;
                Int32 newCount = (Int32)((end >= sEnd) ? (end - sEnd) : (sEnd - end));
                stream.AccessMode = HeapAccessMode.Write;
                stream.Position = Position - keyOffset;
                stream.Write(buffer, newOffset, newCount);
                count -= newCount;
                newOffset += newCount;
                Position += newCount;
            }
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

                foreach (var s in m_Regions.Values)
                {
                    s.Dispose();
                }

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
