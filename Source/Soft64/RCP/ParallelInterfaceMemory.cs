using System;

namespace Soft64.RCP
{
    // TODO: Implement register change thread-safe latch system
    public class ParallelInterfaceMemory : RegistersMemorySection
    {
        private R5 m_StatusReg;
        
        public ParallelInterfaceMemory() : base(0x10000, 0x35, 0x04600000)
        {
            m_StatusReg = new R5(this, 4);
        }

        /// <summary>
        /// Starting RDRAM address 
        /// </summary>
        public Int64 DramAddress
        {
            get { return MemReadU32(0) & 0xFFFFFF; }
            set { MemWrite(0, (UInt32)(value & 0xFFFFFF)); }
        }

        /// <summary>
        /// Starting AD16 address
        /// </summary>
        public Int64 CartAddress
        {
            get { return MemReadU32(1) & 0xFFFFFF; }
            set { MemWrite(1, (UInt32)(value & 0xFFFFFF)); }
        }

        public Int32 ReadLength
        {
            get { return (Int32)(MemReadU32(2) & 0xFFFFFF); }
            set { MemWrite(2, (UInt32)(value & 0xFFFFFF)); }
        }

        public Int32 WriteLength
        {
            get { return (Int32)(MemReadU32(2) & 0xFFFFFF); }
            set { MemWrite(3, (UInt32)(value & 0xFFFFFF)); }
        }

        [RegisterField("R0", 1, 0, typeof(Boolean))]
        [RegisterField("R1", 1, 1, typeof(Boolean))]
        [RegisterField("R1", 1, 2, typeof(Boolean))]
        [RegisterField("W0", 1, 0, typeof(Boolean))]
        [RegisterField("W1", 1, 1, typeof(Boolean))]
        public sealed class R5 : MemoryMappedRegister32
        {
            protected internal R5(RegistersMemorySection s, Int32 o) : base(s, o) { }
            public Boolean IsDmaBusy => AutoRegisterProps.GetR0();
            public Boolean IsIOBusy => AutoRegisterProps.GetR1();
            public Boolean IsError => AutoRegisterProps.GetR2();
            public Boolean Reset { set { AutoRegisterProps.SetW0(value); } }
            public Boolean Clear { set { AutoRegisterProps.SetW1(value); } }
        }

        public R5 Status => m_StatusReg;

        public Byte Domain1Latency
        {
            get { return MemReadByte(6); }
            set { MemWrite(6, value); }
        }

        public Byte Domain1PageSize
        {
            get { return MemReadByte(7); }
            set { MemWrite(7, value); }
        }

        public Byte Domain1PulseWidth
        {
            get { return MemReadByte(8); }
            set { MemWrite(8, value); }
        }

        public Byte Domain1Release
        {
            get { return MemReadByte(9); }
            set { MemWrite(9, value); }
        }

        public Byte Domain2Latency
        {
            get { return MemReadByte(10); }
            set { MemWrite(10, value); }
        }

        public Byte Domain2PageSize
        {
            get { return MemReadByte(11); }
            set { MemWrite(11, value); }
        }

        public Byte Domain2PulseWidth
        {
            get { return MemReadByte(12); }
            set { MemWrite(12, value); }
        }

        public Byte Domain2Release
        {
            get { return MemReadByte(13); }
            set { MemWrite(13, value); }
        }
    }
}