using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soft64.RCP;
using System.IO;
using Xunit;

namespace Soft64.TestUnits
{
    public class MemoryTests
    {
        [Fact]
        public void RspMemoryTests()
        {
            var rspMem = new RspMemory();
            var writer = new BinaryWriter(rspMem.RegMemoryStream);
            var reader = new BinaryReader(rspMem.RegMemoryStream);
            writer.Write(0xBEAU);
            Assert.Equal(0xBEAU, rspMem.RegMemoryAddress.Address);
        }
    }
}
