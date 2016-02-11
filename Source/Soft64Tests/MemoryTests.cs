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

        [Fact]
        public void MachineMemoryTests()
        {
            //N64Memory memory = new N64Memory();
            //memory.Initialize();
            //Byte[] buffer = new byte[10];
            //memory.Position = 0;
            //memory.Read(buffer, 0, 4);
            //Assert.Equal(0, buffer[0]);
            //Assert.Equal(0, buffer[1]);
            //Assert.Equal(0, buffer[2]);
            //Assert.Equal(0, buffer[3]);

            Stream[] readTable = new Stream[0x10000 - 0xFFFF];
        }
    }
}
