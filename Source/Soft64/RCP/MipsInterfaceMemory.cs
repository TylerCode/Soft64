using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft64.RCP
{
    public class MipsInterfaceMemory : RegistersMemorySection
    {


        public MipsInterfaceMemory() : base(0x10000, 0x10, 0x04300000)
        {

        }

        [RegisterField("R0", 7, 0, typeof(Byte))]
        [RegisterField("R1", 1, 7, typeof(Boolean))]
        [RegisterField("R2", 1, 8, typeof(Boolean))]
        [RegisterField("R3", 1, 9, typeof(Boolean))]
        [RegisterField("W0", 7, 0, typeof(Byte))]
        [RegisterField("W1", 1, 7, typeof(Boolean))]
        [RegisterField("W2", 1, 8, typeof(Boolean))]
        [RegisterField("W3", 1, 9, typeof(Boolean))]
        [RegisterField("W4", 1, 10, typeof(Boolean))]
        [RegisterField("W5", 1, 11, typeof(Boolean))]
        [RegisterField("W6", 1, 12, typeof(Boolean))]
        [RegisterField("W7", 1, 13, typeof(Boolean))]
        public class R0 : MemoryMappedRegister32
        {
            public R0(RegistersMemorySection s, Int32 o) : base(s, o) { }

            public Byte InitLength
            {
                get
                {
                    return AutoRegisterProps.GetR0();
                }
                set
                {
                    AutoRegisterProps.SetW0(value);
                }
            }

            public Boolean InitMode => AutoRegisterProps.GetR1();
            public Boolean EbusTest => AutoRegisterProps.GetR2();
            public Boolean RdramRegMode => AutoRegisterProps.GetR3();
            public Boolean ClearInitMode { set { AutoRegisterProps.SetW1(value); } }
            public Boolean SetInitMode { set { AutoRegisterProps.SetW2(value); } }
            public Boolean ClearEbusTest { set { AutoRegisterProps.SetW3(value); } }
            public Boolean SetEbusTest { set { AutoRegisterProps.SetW4(value); } }
            public Boolean ClearDPInterrupt { set { AutoRegisterProps.SetW5(value); } }
            public Boolean ClearRdramReg { set { AutoRegisterProps.SetW6(value); } }
            public Boolean SetRdramReg { set { AutoRegisterProps.SetW7(value); } }
        }

        [RegisterField("R0", 8, 0, typeof(Byte))]
        [RegisterField("R1", 8, 8, typeof(Byte))]
        [RegisterField("R2", 8, 16, typeof(Byte))]
        [RegisterField("R3", 8, 24, typeof(Byte))]
        public class R1 : MemoryMappedRegister32
        {
            public R1(RegistersMemorySection s, Int32 o) : base(s, o) { }
            public Byte Io => AutoRegisterProps.GetR0();
            public Byte Rac => AutoRegisterProps.GetR1();
            public Byte Rdp => AutoRegisterProps.GetR2();
            public Byte Rsp => AutoRegisterProps.GetR3();
        }

        [RegisterField("R0", 1, 0, typeof(Boolean))]
        [RegisterField("R1", 1, 1, typeof(Boolean))]
        [RegisterField("R2", 1, 2, typeof(Boolean))]
        [RegisterField("R3", 1, 4, typeof(Boolean))]
        [RegisterField("R4", 1, 5, typeof(Boolean))]
        [RegisterField("R5", 1, 6, typeof(Boolean))]
        public class R2 : MemoryMappedRegister32
        {
            public R2(RegistersMemorySection s, Int32 o) : base(s, o) { }
            //TODO: sp intr, ...
        }
    }
}
