using Soft64;
using Soft64.MipsR4300.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Soft64.TestUnits
{
    public class MachineClassTests
    {
        [Fact]
        public void MachineAllocation()
        {
            /* Ensure after calling machine constructor, the current instance is not null */
            Machine machine = new Machine();
            Assert.NotNull(Machine.Current);
        }

        [Fact]
        public void MachineBootTest()
        {
            var logger = Common.InitNLog();

            /* Test basic machine boot to check for errors */
            Machine machine = 
                Common.MachineFactory.Create(
                    cart:Common.MockUpCartridge(RegionType.NTSC, CICKeyType.CIC_X102), 
                    breakAtDebug:true);

            

            /* Make sure the machine is in run state and CPU is pointing first program position */
            Assert.Equal(true, machine.IsRunning);
            Assert.Equal(0xA4000040, machine.DeviceCPU.State.PC);
        }
    }
}
