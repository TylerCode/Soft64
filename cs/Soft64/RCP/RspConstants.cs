using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft64.RCP
{
    [Flags]
    public enum RcpInterruptType : byte
    {
        Sp = 1 << 0,
        Si = 1 << 1,
        Ai = 1 << 2,
        Vi = 1 << 3,
        Pi = 1 << 4,
        Dp = 1 << 5,
    }

    [Flags]
    public enum SpStatus : ushort
    {
        Halt = 1 << 0,
        Broke = 1 << 1,
        DmaBusy = 1 << 2,
        DmaFull = 1 << 3,
        IoFull = 1 << 4,
        Sstep = 1 << 5,
        IntrBreak = 1 << 6,
        Signal0 = 1 << 7,
        Signal1 = 1 << 8,
        Signal2 = 1 << 9,
        Signal3 = 1 << 10,
        Signal4 = 1 << 11,
        Signal5 = 1 << 12,
        Signal6 = 1 << 13,
        Signal7 = 1 << 14,
    }

    [Flags]
    public enum DpStatus : ushort
    {
        XbusDma = 1 << 0,
        Freeze = 1 << 1,
        Flush = 1 << 2,
        StartGClk = 1 << 3,
        TmemBusy = 1 << 4,
        PipeBusy = 1 << 5,
        CmdBusy = 1 << 6,
        CBufReady = 1 << 7,
        DmaBusy = 1 << 8,
        EndValid = 1 << 9,
        StartValid = 1 << 10,
    }

    public static class RspConstants
    {
        public const Int32 DACRATE_NTSC = 48681812;
        public const Int32 DACRATE_PAL = 49656530;
        public const Int32 DACRATE_MPAL = 48628316;
    }
}