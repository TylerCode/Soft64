using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soft64.RCP;
using System.IO;
using Xunit;

namespace Soft64Tests
{
    public class MemoryTests
    {
        [Fact]
        public void RspMemoryTests()
        {
            var rspMem = (IRspMemory) new RcpMemory.RspMemory();
            var writer = new BinaryWriter(rspMem.RegisterStream);
            var reader = new BinaryReader(rspMem.RegisterStream);

            writer.Write(0xDEAD);
            Assert.Equal(0xDEAD, rspMem.RegMemoryAddress.Address);
        }
    }
}
