using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Soft64.RCP
{
    public class RspMemory : IDisposable
    {
        private MemorySection m_SPRam;
        private MemorySection m_RspRegisterHeap;
        private IntPtr m_RspRegisterPtr;
        private Boolean m_Disposed;

        public RspMemory()
        {
            m_SPRam = new MemorySection(0x1000 * 2, 0x04000000);
            m_RspRegisterHeap = new MemorySection(1024 ^ 2, 0x04010000);
            m_RspRegisterPtr = m_RspRegisterHeap.HeapPointer;
            RegMemoryAddress = new RspRegisterAddressMode(m_RspRegisterPtr, 4 * 0);
            RegDramAddress = new RspRegisterAddress(m_RspRegisterPtr, 4 * 1);
            RegReadLength = new RspRegisterLength(m_RspRegisterPtr, 4 * 2);
            RegWriteLength = new RspRegisterLength(m_RspRegisterPtr, 4 * 3);
            RegStatusW = new RspRegisterStatusW(m_RspRegisterPtr, 4 * 4);
        }

        protected virtual void Dispose(Boolean disposing)
        {
            if (!m_Disposed)
            {
                if (disposing)
                {
                    m_SPRam.Dispose();
                    m_RspRegisterHeap.Dispose();
                }

                m_Disposed = true;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        public MemorySection SPRam => m_SPRam;
        public MemorySection RegMemoryStream => m_RspRegisterHeap;
        public RspRegisterAddressMode RegMemoryAddress { get; private set; }
        public RspRegisterAddress RegDramAddress { get; private set; }
        public RspRegisterLength RegReadLength { get; private set; }
        public RspRegisterLength RegWriteLength { get; private set; }
        public RspRegisterStatusW RegStatusW { get; private set; }
    }

    /* 
        member this.RegStatus with get() = null
        member this.RegDmaFull with get() = null
        member this.RegDmaBusy with get() = null
        member this.RegSemaphore with get() = null
        member this.RegPc with get() = null
        member this.RegImemBist with get() = null
*/

    [RegisterField("Address", 12, 0, typeof(UInt32))]
    [RegisterField("Mode", 1, 12, typeof(Boolean))]
    public sealed class RspRegisterAddressMode : MemoryMappedRegister32
    {
        internal RspRegisterAddressMode(IntPtr p, Int32 o) : base(p, o)
        {

        }

        public UInt32 Address
        {
            get { return AutoRegisterProps.GetAddress(); }
            set { AutoRegisterProps.SetAddress(value); }
        }

        public Boolean Mode
        {
            get { return AutoRegisterProps.GetMode(); }
            set { AutoRegisterProps.SetMode(value); }
        }
    }

    [RegisterField("Address", 24, 0, typeof(UInt32))]
    public sealed class RspRegisterAddress : MemoryMappedRegister32
    {
        internal RspRegisterAddress(IntPtr p, Int32 o) : base(p, o)
        {

        }

        public UInt32 Address
        {
            get { return (UInt32)AutoRegisterProps.GetAddress(); }
            set { AutoRegisterProps.SetAddress(value); }
        }
    }

    [RegisterField("Length", 12, 0, typeof(UInt32))]
    [RegisterField("Count", 8, 12, typeof(UInt32))]
    [RegisterField("Skip", 12, 20, typeof(UInt32))]
    public sealed class RspRegisterLength : MemoryMappedRegister32
    {
        internal RspRegisterLength(IntPtr p, Int32 o) : base(p, o)
        {

        }

        public UInt32 Length
        {
            get { return AutoRegisterProps.GetLength(); }
            set { AutoRegisterProps.SetLength(value); }
        }

        public UInt32 Count
        {
            get { return AutoRegisterProps.GetCount(); }
            set { AutoRegisterProps.SetCount(value); }
        }

        public UInt32 Skip
        {
            get { return AutoRegisterProps.GetSkip(); }
            set { AutoRegisterProps.SetSkip(value); }
        }
    }

    [RegisterField("CH", 1, 00, typeof(Boolean)),
      RegisterField("SH", 1, 01, typeof(Boolean)),
      RegisterField("CB", 1, 02, typeof(Boolean)),
      RegisterField("CI", 1, 03, typeof(Boolean)),
      RegisterField("SI", 1, 04, typeof(Boolean)),
      RegisterField("CS", 1, 05, typeof(Boolean)),
      RegisterField("SS", 1, 06, typeof(Boolean)),
      RegisterField("CIB", 1, 07, typeof(Boolean)),
      RegisterField("SIB", 1, 08, typeof(Boolean)),
      RegisterField("CS0", 1, 09, typeof(Boolean)),
      RegisterField("SS0", 1, 10, typeof(Boolean)),
      RegisterField("CS1", 1, 11, typeof(Boolean)),
      RegisterField("SS1", 1, 12, typeof(Boolean)),
      RegisterField("CS2", 1, 13, typeof(Boolean)),
      RegisterField("SS2", 1, 14, typeof(Boolean)),
      RegisterField("CS3", 1, 15, typeof(Boolean)),
      RegisterField("SS3", 1, 16, typeof(Boolean)),
      RegisterField("CS4", 1, 17, typeof(Boolean)),
      RegisterField("SS4", 1, 18, typeof(Boolean)),
      RegisterField("CS5", 1, 19, typeof(Boolean)),
      RegisterField("SS5", 1, 20, typeof(Boolean)),
      RegisterField("CS6", 1, 21, typeof(Boolean)),
      RegisterField("SS6", 1, 22, typeof(Boolean)),
      RegisterField("CS7", 1, 23, typeof(Boolean)),
      RegisterField("SS7", 1, 24, typeof(Boolean))]
    public sealed class RspRegisterStatusW : MemoryMappedRegister32
    {
        internal RspRegisterStatusW(IntPtr p, Int32 o) : base(p, o)
        {

        }

        public Boolean ClearHalt
        {
            set { AutoRegisterProps.SetCH(value); }
        }

        public Boolean SetHalt
        {
            set { AutoRegisterProps.SetSH(value); }
        }

        public Boolean ClearBroke
        {
            set { AutoRegisterProps.SetCB(value); }
        }

        public Boolean ClearIntr
        {
            set { AutoRegisterProps.SetCI(value); }
        }

        public Boolean SetIntr
        {
            set { AutoRegisterProps.SetSI(value); }
        }

        public Boolean ClearSingleStep
        {
            set { AutoRegisterProps.SetCS(value); }
        }

        public Boolean SetSingleStep
        {
            set { AutoRegisterProps.SetSS(value); }
        }

        public Boolean ClearIntrOnBreak
        {
            set { AutoRegisterProps.SetCIB(value); }
        }

        public Boolean SetIntrOnBreak
        {
            set { AutoRegisterProps.SetSIB(value); }
        }

        public Boolean ClearSignal0
        {
            set { AutoRegisterProps.SetCS0(value); }
        }

        public Boolean SetSignal0
        {
            set { AutoRegisterProps.SetSS0(value); }
        }

        public Boolean ClearSignal1
        {
            set { AutoRegisterProps.SetCS1(value); }
        }

        public Boolean SetSignal1
        {
            set { AutoRegisterProps.SetSS1(value); }
        }

        public Boolean ClearSignal2
        {
            set { AutoRegisterProps.SetCS2(value); }
        }

        public Boolean SetSignal2
        {
            set { AutoRegisterProps.SetSS2(value); }
        }

        public Boolean ClearSignal3
        {
            set { AutoRegisterProps.SetCS3(value); }
        }

        public Boolean SetSignal3
        {
            set { AutoRegisterProps.SetSS3(value); }
        }

        public Boolean ClearSignal4
        {
            set { AutoRegisterProps.SetCS4(value); }
        }

        public Boolean SetSignal4
        {
            set { AutoRegisterProps.SetSS4(value); }
        }

        public Boolean ClearSignal5
        {
            set { AutoRegisterProps.SetCS5(value); }
        }

        public Boolean SetSignal5
        {
            set { AutoRegisterProps.SetSS5(value); }
        }

        public Boolean ClearSignal6
        {
            set { AutoRegisterProps.SetCS6(value); }
        }

        public Boolean SetSignal6
        {
            set { AutoRegisterProps.SetSS0(value); }
        }

        public Boolean ClearSignal7
        {
            set { AutoRegisterProps.SetCS7(value); }
        }

        public Boolean SetSignal7
        {
            set { AutoRegisterProps.SetSS7(value); }
        }
    }


    public sealed class RspRegisterStatusR : MemoryMappedRegister32
    {
        public RspRegisterStatusR(IntPtr p, Int32 o) : base(p, 0)
        {

        }
    }
}


//    public interface IRspRegReadStatus
//    {
//        Boolean Halt { get; }

//        Boolean Broke { get; }

//        Boolean DmaBusy { get; }

//        Boolean DmaFull { get; }

//        Boolean IoFull { get; }

//        Boolean SingleStep { get; }

//        Boolean InterruptOnBreak { get; }

//        Boolean Signal0Set { get; }

//        Boolean Signal1Set { get; }

//        Boolean Signal2Set { get; }

//        Boolean Signal3Set { get; }

//        Boolean Signal4Set { get; }

//        Boolean Signal5Set { get; }

//        Boolean Signal6Set { get; }

//        Boolean Signal7Set { get; }
//    }



