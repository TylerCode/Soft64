using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Soft64.CoreMachine
{
    [Serializable]
    public sealed class MachineException : Exception
    {
        public MachineException()
            : base()
        {
        }

        public MachineException(String message)
            : base(message)
        {
        }

        public MachineException(String message, Exception innerException)
            : base(message, innerException)
        {
        }

        public MachineException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
