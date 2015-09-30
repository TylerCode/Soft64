using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft64.RCP
{
    public abstract class DmaEngine
    {
        protected DmaEngine(SysADBusStream sysADBusStream)
        {
            BusStream = sysADBusStream;
        }

        protected Stream BusStream { get; private set; }

        public Boolean RealDmaTiming { get; set; } = false;
    }
}
