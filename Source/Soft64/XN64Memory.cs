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
        private PtrWrappedCartridge m_CartMemory;
        private Dictionary<Int32, FastHeapStream> m_Regions;
        [ThreadStatic]
        private Int64 m_Position;

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

        public XN64Memory()
        {
            m_Regions = new Dictionary<Int32, FastHeapStream>();
        }

        public void Initialize()
        {
            m_RDRam = new FastHeapStream(0x100000);
            m_PifMemory = new FastHeapStream(0x800);
            m_RspMemory = new RspMemory();
            m_CartMemory = new PtrWrappedCartridge(Machine.Current.DeviceRCP.Interface_Parallel.InsertedCartridge);

            /* Setup the region hashtable */
            m_Regions.Add(0x0000, m_RDRam);
            m_Regions.Add(0x0400, m_RspMemory.SPRam);
            m_Regions.Add(0x0401, m_RspMemory.RegMemoryStream);
            m_Regions.Add(0x0500, m_CartMemory);
        }

        public override void Flush()
        {
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

        private FastHeapStream GetRegion(Int64 address)
        {
            Int32 key = (Int32)((0xFFFF0000L & address) >> 16);
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

        public override int Read(byte[] buffer, int offset, int count)
        {
            FastHeapStream mem = GetRegion(m_Position);

            if (mem != null)
            {
                mem.AccessMode = HeapAccessMode.Read;
                mem.Read(buffer, offset, count);
            }

            return count;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}
