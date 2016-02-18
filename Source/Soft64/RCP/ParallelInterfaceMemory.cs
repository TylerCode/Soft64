using System;

namespace Soft64.RCP
{
    // TODO: Implement register change thread-safe latch system
    public class ParallelInterfaceMemory : RegistersMemorySection
    {
        /* RCP Interface references */
        private RcpInterfacePi m_RcpInterfacePI;

        private _R1 m_DramAddressReg;
        private _R2 m_CartAddressReg;

        

        public ParallelInterfaceMemory() : base(0x10000, 0x35, 0x04600000)
        {
            m_DramAddressReg = new _R1(this, 4 * 0);
            m_CartAddressReg = new _R2(this, 4 * 1);

        }

        private class _R1 : MemoryMappedRegister32
        {
            public unsafe _R1(RegistersMemorySection s, Int32 o) : base(s, o)
            {
            }
            public UInt32 Address
            {
                get { return RegisterValue; }
                set { RegisterValue = value; }
            }

        }

        private class _R2 : MemoryMappedRegister32
        {
            public unsafe _R2(RegistersMemorySection s, Int32 o) : base(s, o)
            {
            }
            public UInt32 Address
            {
                get { return RegisterValue; }
                set { RegisterValue = value; }
            }
        }

        [RegisterField("R0", 1, 0, typeof(Boolean))]
        [RegisterField("R1", 1, 1, typeof(Boolean))]
        [RegisterField("R1", 1, 2, typeof(Boolean))]
        [RegisterField("W0", 1, 0, typeof(Boolean))]
        [RegisterField("W1", 1, 1, typeof(Boolean))]
        private class _R3 : MemoryMappedRegister32
        {
            public unsafe _R3(RegistersMemorySection s, Int32 o) : base(s, o)
            {
            }
            public Boolean IsDmaBusy => AutoRegisterProps.GetR0();
            public Boolean IsIOBusy => AutoRegisterProps.GetR1();
            public Boolean IsError => AutoRegisterProps.GetR2();
        }

        public uint Domain1Latency { get; internal set; }
        public uint Domain1PageSize { get; internal set; }
        public uint Domain1PulseWidth { get; internal set; }
        public uint Domain1Release { get; internal set; }
        public int Status { get; internal set; }
    }

    public interface RcpInterfacePi
    {
        Boolean IsDmaBusy { get; }
        Boolean IsIOBusy { get; }
        Boolean IsError { get; }
    }
}