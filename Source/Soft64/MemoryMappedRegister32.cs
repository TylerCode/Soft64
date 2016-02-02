using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft64
{
    public unsafe abstract class MemoryMappedRegister32 : SmartRegister<UInt32>
    {
        private UInt32 * m_BasePointer;

        protected MemoryMappedRegister32(IntPtr memoryPointer, Int32 memoryOffset) : base()
        {
            m_BasePointer = (UInt32 *)IntPtr.Add(memoryPointer, memoryOffset).ToPointer();
        }

        protected override uint InternalRegister
        {
            get
            {
               return *m_BasePointer;
            }

            set
            {
                *m_BasePointer = value;
            }
        }
    }
}
