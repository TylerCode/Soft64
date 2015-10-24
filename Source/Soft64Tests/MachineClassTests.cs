using Soft64;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Soft64Tests
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
    }
}
