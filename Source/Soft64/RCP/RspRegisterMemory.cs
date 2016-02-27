using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft64.RCP
{
    public class RspRegisterMemory : RegistersMemorySection
    {
        /// <summary>
        /// I2C master configuration
        /// </summary>
        public R0 MemoryAddressReg { get; private set; }
        public R2R3 ReadLengthReg { get; private set; }
        public R2R3 WriteLengthReg { get; private set; }
        public R4 StatusReg { get; private set; }
        public R9 IBistReg { get; private set; }

        public RspRegisterMemory() : base(0xBFFFF, 0x40008, 0x04040000)
        {
            MemoryAddressReg = new R0(this, 0);
            ReadLengthReg = new R2R3(this, 2);
            WriteLengthReg = new R2R3(this, 3);
            StatusReg = new R4(this, 4);
            IBistReg = new R9(this, 9);
        }

        [RegisterField("RW0", 12, 0, typeof(UInt32))]
        [RegisterField("RW1", 1, 12, typeof(Boolean))]
        public sealed class R0 : MemoryMappedRegister32
        {
            internal R0(RegistersMemorySection s, Int32 o) : base(s, o, true) {}

            public UInt32 Address
            {
                get { return AutoRegisterProps.GetRW0(); }
                set { AutoRegisterProps.GetRW0(value); }
            }

            public Boolean Mode
            {
                get { return AutoRegisterProps.GetRW1(); }
                set { AutoRegisterProps.SetRW1(value); }
            }
        }

        /// <summary>
        /// I2C Slave configuration
        /// </summary>
        public UInt32 RdramAddressReg
        {
            get { return MemReadU32(1) & 0xFFFFFF; }
            set { MemWrite(1, value & 0xFFFFFF); }
        }

        [RegisterField("RW0", 12, 0, typeof(UInt32))]
        [RegisterField("RW1", 8, 12, typeof(UInt32))]
        [RegisterField("RW2", 12, 20, typeof(UInt32))]
        public sealed class R2R3 : MemoryMappedRegister32
        {
            internal R2R3(RegistersMemorySection s, Int32 o) : base(s, o, true) { }

            public UInt32 Length
            {
                get { return AutoRegisterProps.GetRW0(); }
                set { AutoRegisterProps.SetRW0(value); }
            }

            public UInt32 Count
            {
                get { return AutoRegisterProps.GetRW1(); }
                set { AutoRegisterProps.SetRW1(value); }
            }

            public UInt32 Skip
            {
                get { return AutoRegisterProps.GetRW2(); }
                set { AutoRegisterProps.SetRW2(value); }
            }
        }

        [RegisterField("R0", 1, 0, typeof(Boolean))]
        [RegisterField("R1", 1, 1, typeof(Boolean))]
        [RegisterField("R2", 1, 2, typeof(Boolean))]
        [RegisterField("R3", 1, 3, typeof(Boolean))]
        [RegisterField("R4", 1, 4, typeof(Boolean))]
        [RegisterField("R5", 1, 5, typeof(Boolean))]
        [RegisterField("R6", 1, 6, typeof(Boolean))]
        [RegisterField("R7", 1, 7, typeof(Boolean))]
        [RegisterField("R8", 1, 8, typeof(Boolean))]
        [RegisterField("R9", 1, 9, typeof(Boolean))]
        [RegisterField("R10", 1, 10, typeof(Boolean))]
        [RegisterField("R11", 1, 11, typeof(Boolean))]
        [RegisterField("R12", 1, 12, typeof(Boolean))]
        [RegisterField("R13", 1, 13, typeof(Boolean))]
        [RegisterField("R14", 1, 14, typeof(Boolean))]
        [RegisterField("W0", 1, 0, typeof(Boolean))]
        [RegisterField("W1", 1, 1, typeof(Boolean))]
        [RegisterField("W2", 1, 2, typeof(Boolean))]
        [RegisterField("W3", 1, 3, typeof(Boolean))]
        [RegisterField("W4", 1, 4, typeof(Boolean))]
        [RegisterField("W5", 1, 4, typeof(Boolean))]
        [RegisterField("W6", 1, 6, typeof(Boolean))]
        [RegisterField("W7", 1, 7, typeof(Boolean))]
        [RegisterField("W8", 1, 8, typeof(Boolean))]
        [RegisterField("W9", 1, 9, typeof(Boolean))]
        [RegisterField("W10", 1, 10, typeof(Boolean))]
        [RegisterField("W11", 1, 11, typeof(Boolean))]
        [RegisterField("W12", 1, 12, typeof(Boolean))]
        [RegisterField("W13", 1, 13, typeof(Boolean))]
        [RegisterField("W14", 1, 14, typeof(Boolean))]
        [RegisterField("W15", 1, 15, typeof(Boolean))]
        [RegisterField("W16", 1, 16, typeof(Boolean))]
        [RegisterField("W17", 1, 17, typeof(Boolean))]
        [RegisterField("W18", 1, 18, typeof(Boolean))]
        [RegisterField("W19", 1, 19, typeof(Boolean))]
        [RegisterField("W20", 1, 20, typeof(Boolean))]
        [RegisterField("W21", 1, 21, typeof(Boolean))]
        [RegisterField("W22", 1, 22, typeof(Boolean))]
        [RegisterField("W23", 1, 23, typeof(Boolean))]
        [RegisterField("W24", 1, 24, typeof(Boolean))]
        public sealed class R4 : MemoryMappedRegister32
        {
            internal R4(RegistersMemorySection s, Int32 o) : base(s, o) { }
            public Boolean IsHalt => AutoRegisterProps.GetR0();
            public Boolean IsBroke => AutoRegisterProps.GetR1();
            public Boolean IsDmaBusy => AutoRegisterProps.GetR2();
            public Boolean IsDmaFull => AutoRegisterProps.GetR3();
            public Boolean IsIOFull => AutoRegisterProps.GetR4();
            public Boolean IsSingleStep => AutoRegisterProps.GetR5();
            public Boolean IsInterruptOnBreak => AutoRegisterProps.GetR6();
            public Boolean IsSingal0Set => AutoRegisterProps.GetR7();
            public Boolean IsSingal1Set => AutoRegisterProps.GetR8();
            public Boolean IsSingal2Set => AutoRegisterProps.GetR9();
            public Boolean IsSingal3Set => AutoRegisterProps.GetR10();
            public Boolean IsSingal4Set => AutoRegisterProps.GetR11();
            public Boolean IsSingal5Set => AutoRegisterProps.GetR12();
            public Boolean IsSingal6Set => AutoRegisterProps.GetR13();
            public Boolean IsSingal7Set => AutoRegisterProps.GetR14();
            public Boolean ClearHalt { set { AutoRegisterProps.SetW0(value); } }
            public Boolean SetHalt { set { AutoRegisterProps.SetW1(value); } } 
            public Boolean ClearBroke { set { AutoRegisterProps.SetW2(value); } }
            public Boolean ClearInterrupt { set { AutoRegisterProps.SetW3(value); } }
            public Boolean SetInterrupt { set { AutoRegisterProps.SetW4(value); } }
            public Boolean ClearSingleStep { set { AutoRegisterProps.SetW5(value); } }
            public Boolean SetSingleStep { set { AutoRegisterProps.SetW6(value); } }
            public Boolean ClearInterruptOnBreak { set { AutoRegisterProps.SetW7(value); } }
            public Boolean SetInterruptOnBreak { set { AutoRegisterProps.SetW8(value); } }
            public Boolean ClearSignal0 { set { AutoRegisterProps.SetW9(value); } }
            public Boolean SetSignal0 { set { AutoRegisterProps.SetW10(value); } }
            public Boolean ClearSignal1 { set { AutoRegisterProps.SetW11(value); } }
            public Boolean SetSignal1 { set { AutoRegisterProps.SetW12(value); } }
            public Boolean ClearSignal2 { set { AutoRegisterProps.SetW13(value); } }
            public Boolean SetSignal2 { set { AutoRegisterProps.SetW14(value); } }
            public Boolean ClearSignal3 { set { AutoRegisterProps.SetW15(value); } }
            public Boolean SetSignal3 { set { AutoRegisterProps.SetW16(value); } }
            public Boolean ClearSignal4 { set { AutoRegisterProps.SetW17(value); } }
            public Boolean SetSignal4 { set { AutoRegisterProps.SetW18(value); } }
            public Boolean ClearSignal5 { set { AutoRegisterProps.SetW19(value); } }
            public Boolean SetSignal5 { set { AutoRegisterProps.SetW20(value); } }
            public Boolean ClearSignal6 { set { AutoRegisterProps.SetW21(value); } }
            public Boolean SetSignal6 { set { AutoRegisterProps.SetW122(value); } }
            public Boolean ClearSignal7 { set { AutoRegisterProps.SetW23(value); } }
            public Boolean SetSignal7 { set { AutoRegisterProps.SetW24(value); } }
        }

        public Boolean DmaFullReg
        {
            get { return MemReadBool(5); }
        }

        public Boolean DmaBusyReg
        {
            get { return MemReadBool(6); }
        }

        public Boolean SemaphoreReg
        {
            get { return MemReadBool(7); }
            set { MemWrite(7, value); }
        }

        public UInt32 PCReg
        {
            set { MemWrite(8, value & 0xFFF); }
        }

        [RegisterField("RW0", 1, 0, typeof(Boolean))]
        [RegisterField("RW1", 1, 1, typeof(Boolean))]
        [RegisterField("RW2", 1, 2, typeof(Boolean))]
        [RegisterField("R0", 3, 3, typeof(Byte))]
        public sealed class R9 : MemoryMappedRegister32
        {
            internal R9(RegistersMemorySection s, Int32 o) : base(s, o, true, 3) { }
            
            public Boolean BistCheck
            {
                get { return AutoRegisterProps.GetRW0(); }
                set { AutoRegisterProps.SetRW0(value); }    
            }

            public Boolean BistGo
            {
                get { return AutoRegisterProps.GetRW1(); }
                set { AutoRegisterProps.SetRW1(value); }
            }

            public Boolean BistClear
            {
                get { return AutoRegisterProps.GetRW2(); }
                set { AutoRegisterProps.SetRW2(value); }
            }

            public Byte BistFail => AutoRegisterProps.GetR0();
        }
    }
}
