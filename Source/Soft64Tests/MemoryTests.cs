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
            /* Create an instance of the machine memory system */
            N64Memory memory = new N64Memory();

            /* Initialize memory */
            memory.Initialize();

            /* RDRAM zero test */
            StreamTest(new Byte[] { 0, 0, 0, 0 }, false, 0, memory);

            /* RDRAM read/write test */
            StreamTest(new Byte[] { 0xD, 0xE, 0xA, 0xD }, true, 0, memory);
        }

        private void StreamTest(Byte[] testData, Boolean writeTest, Int64 position, Stream stream)
        {
            Byte[] read = new byte[testData.Length];
            stream.Position = position;

            if (writeTest)
            {
                stream.Write(testData, 0, testData.Length);
            }

            Int32 readLength = stream.Read(read, 0, read.Length);

            Assert.True(readLength == read.Length, "Test read length doesn't match test data length");

            for (Int32 i = 0; i < read.Length; i++)
            {
                Assert.Equal(testData[i], read[i]);
            }
        }
    }
}
