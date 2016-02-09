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

    public class XN64Memory
    {
        private FastHeapStream m_RDRam;
        private RspMemory m_RspMemory;

        private Dictionary<Byte, PhysRegion> m_Regions;

        public XN64Memory()
        {
            m_Regions = new Dictionary<byte, PhysRegion>();
        }

        public void Initialize()
        {
            m_RDRam = new FastHeapStream(0x03F00000);
            m_RspMemory = new RspMemory();
        }
    }

    public struct PhysRegion
    {
        public IntPtr Ptr;
        public Stream Stream;
    }
}
