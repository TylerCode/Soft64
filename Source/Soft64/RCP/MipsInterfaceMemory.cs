using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft64.RCP
{
    public class MipsInterfaceMemory : RegistersMemorySection
    {
        public R0 InitModeReg { get; private set; }
        public R1 VersionReg { get; private set; }
        public R2 IntrReg { get; private set; }
        public R3 IntrMaskReg { get; private set; }

        public MipsInterfaceMemory() : base(0x10000, 0x10, 0x04300000)
        {
            InitModeReg = new R0(this, 0);
            VersionReg = new R1(this, 1);
            IntrReg = new R2(this, 2);
            IntrMaskReg = new R3(this, 3);
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
        public sealed class R0 : MemoryMappedRegister32
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
        public sealed class R1 : MemoryMappedRegister32
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
        public sealed class R2 : MemoryMappedRegister32
        {
            public R2(RegistersMemorySection s, Int32 o) : base(s, o) { }
            public Boolean SPIntr => AutoRegisterProps.GetR0();
            public Boolean SIIntr => AutoRegisterProps.GetR1();
            public Boolean AIIntr => AutoRegisterProps.GetR2();
            public Boolean VIIntr => AutoRegisterProps.GetR3();
            public Boolean PIIntr => AutoRegisterProps.GetR4();
            public Boolean DPIntr => AutoRegisterProps.GetR5();
        }

        [RegisterField("R0", 1, 0, typeof(Boolean))]
        [RegisterField("R1", 1, 1, typeof(Boolean))]
        [RegisterField("R2", 1, 2, typeof(Boolean))]
        [RegisterField("R3", 1, 3, typeof(Boolean))]
        [RegisterField("R4", 1, 4, typeof(Boolean))]
        [RegisterField("R5", 1, 5, typeof(Boolean))]
        [RegisterField("W0", 1, 0, typeof(Boolean))]
        [RegisterField("W1", 1, 1, typeof(Boolean))]
        [RegisterField("W2", 1, 2, typeof(Boolean))]
        [RegisterField("W3", 1, 3, typeof(Boolean))]
        [RegisterField("W4", 1, 4, typeof(Boolean))]
        [RegisterField("W5", 1, 5, typeof(Boolean))]
        [RegisterField("W6", 1, 6, typeof(Boolean))]
        [RegisterField("W7", 1, 7, typeof(Boolean))]
        [RegisterField("W8", 1, 8, typeof(Boolean))]
        [RegisterField("W9", 1, 9, typeof(Boolean))]
        [RegisterField("W10", 1, 10, typeof(Boolean))]
        [RegisterField("W11", 1, 11, typeof(Boolean))]
        public sealed class R3 : MemoryMappedRegister32
        {
            public R3(RegistersMemorySection s, Int32 o) : base(s, o) { }
            public Boolean SPIntrMask => AutoRegisterProps.GetR0();
            public Boolean SIIntrMask => AutoRegisterProps.GetR1();
            public Boolean AIIntrMask => AutoRegisterProps.GetR2();
            public Boolean VIIntrMask => AutoRegisterProps.GetR3();
            public Boolean PIIntrMask => AutoRegisterProps.GetR4();
            public Boolean DPIntrMask => AutoRegisterProps.GetR5();
            public Boolean ClearSPIntr { set { AutoRegisterProps.SetW0(value); } }
            public Boolean SetSPIntr { set { AutoRegisterProps.SetW1(value); } }
            public Boolean ClearSIIntr { set { AutoRegisterProps.SetW2(value); } }
            public Boolean SetSIIntr { set { AutoRegisterProps.SetW3(value); } }
            public Boolean ClearAIIntr { set { AutoRegisterProps.SetW4(value); } }
            public Boolean SetAIIntr { set { AutoRegisterProps.SetW5(value); } }
            public Boolean ClearVIIntr { set { AutoRegisterProps.SetW6(value); } }
            public Boolean SetVIIntr { set { AutoRegisterProps.SetW7(value); } }
            public Boolean ClearPIIntr { set { AutoRegisterProps.SetW8(value); } }
            public Boolean SetPIIntr { set { AutoRegisterProps.SetW9(value); } }
            public Boolean ClearDPIntr { set { AutoRegisterProps.SetW10(value); } }
            public Boolean SetDPIntr { set { AutoRegisterProps.SetW11(value); } }
        }
    }
}
