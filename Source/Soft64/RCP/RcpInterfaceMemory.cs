using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft64.RCP
{
    public class RcpInterfaceMemory
    {
        public uint InterruptMask { get; internal set; }
        public uint Interrupts { get; internal set; }
        public uint Mode { get; internal set; }
        public int Version { get; internal set; }
    }
}
