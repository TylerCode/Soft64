using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft64UI.JAPI
{
    public sealed class MemoryJAPI
    {
        public Byte[] ReadMemory(Double address, Double length)
        {
            return new Byte[] { 0xD, 0xE, 0xA, 0xD };
        }
    }
}
