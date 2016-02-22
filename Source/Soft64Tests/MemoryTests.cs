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
        public void MachineMemoryTests()
        {
            /* Create an instance of the machine memory system */
            N64Memory memory = new N64Memory();

            /* Initialize memory */
            memory.Initialize();

            /* RDRAM zero test */
            StreamTest("RDRAM Zero Test", new Byte[] { 0, 0, 0, 0 }, false, 0, memory);

            /* RDRAM read/write test */
            StreamTest("RDRAM Read/Write Test", new Byte[] { 0xD, 0xE, 0xA, 0xD }, true, 0, memory);

            /* RSP Register tests */
            var rspRegMem = memory.RspRegisters;
            var writer = new BinaryWriter(rspRegMem);
            var reader = new BinaryReader(rspRegMem);
            writer.Write(0xBEAU);
            Assert.Equal(0xBEAU, rspRegMem.MemoryAddressReg.Address);
        }

        private void StreamTest(String testName, Byte[] testData, Boolean writeTest, Int64 position, Stream stream)
        {
            Byte[] read = new byte[testData.Length];
            stream.Position = position;

            if (writeTest)
            {
                stream.Write(testData, 0, testData.Length);
            }

            stream.Position = position;
            Int32 readLength = stream.Read(read, 0, read.Length);

            Assert.True(readLength == read.Length, $"Test read length doesn't match test data length for test {testName}");

            for (Int32 i = 0; i < read.Length; i++)
            {
                Assert.True(testData[i] == read[i], $"Byte sample differs from test data at index {i} for test {testName}");
            }
        }
    }
}
