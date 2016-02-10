using Soft64.RCP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft64
{
    /* Experimental WIP fast n64 physical memory system */

    public class XN64Memory : Stream
    {
        private FastHeapStream m_RDRam;
        private RspMemory m_RspMemory;
        private FastHeapStream m_PifMemory;
        private CartridgeHeapStream m_CartMemory;
        private Dictionary<Int32, FastHeapStream> m_Regions;
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

        public XN64Memory()
        {
            m_Regions = new Dictionary<Int32, FastHeapStream>();
        }

        public void Initialize()
        {
            CheckDispose();
            m_RDRam = new FastHeapStream(0x100000);
            m_PifMemory = new FastHeapStream(0x800);
            m_RspMemory = new RspMemory();
            m_CartMemory = new CartridgeHeapStream(Machine.Current.DeviceRCP.Interface_Parallel.InsertedCartridge);

            /* Setup the region hashtable */
            m_Regions.Add(0x0000, m_RDRam);
            m_Regions.Add(0x0400, m_RspMemory.SPRam);
            m_Regions.Add(0x0401, m_RspMemory.RegMemoryStream);
            m_Regions.Add(0x0500, m_CartMemory);
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

        private FastHeapStream GetRegion(Int64 address)
        {
            int key = GetKey(address);
            FastHeapStream stream = null;

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

        public override int Read(byte[] buffer, int offset, int count)
        {
            CheckDispose();
            var keys = GetRegions(m_Position, count);
            var givenCount = count;
            var newOffset = offset;
            Array.Clear(buffer, offset, count);
            Int64 lastPosition = 0;

            foreach (var key in keys)
            {
                /* Store the last position */
                lastPosition = Position;

                /* Turn the key into the section's offset */
                Int64 keyOffset = (Int64)(key << 16);
                FastHeapStream stream = m_Regions[key]; /* Get the associated stream */
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
                stream.AccessMode = HeapAccessMode.Read;
                stream.Position = Position - keyOffset;
                stream.Read(buffer, newOffset, newCount);
                count -= newCount;
                newOffset += newCount;
                Position += newCount;
            }

            return givenCount;
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
                FastHeapStream stream = m_Regions[key]; /* Get the associated stream */
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
                throw new ObjectDisposedException(nameof(XN64Memory));
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
    }
}
