using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft64.RCP
{
    public enum RspMemMode : int
    {
        Dmem = 0,
        Imem = 1
    }

    public interface IRspMemory
    {
        /// <summary>
        /// The IMEM section of the RSP
        /// </summary>
        Stream IMemoryStream { get; }

        /// <summary>
        /// The DMEM section of the RSP
        /// </summary>
        Stream DMemoryStream { get; }

        /// <summary>
        /// The raw section of the Rsp registers
        /// </summary>
        Stream RegisterStream { get; }

        IRspRegMemoryAddress RegMemoryAddress { get; }

        IRspRegDramAddress RegDramAddress { get; }

        /// <summary>
        /// Controls reads from RDRAM to I or D memory
        /// </summary>
        IRspRegIoLength RegReadLength { get; }

        /// <summary>
        /// Controls reads from I or D memory to RDRAM
        /// </summary>
        IRspRegIoLength RegWriteLength { get; }

        IRspRegWriteStatus RegWriteonlyStatus { get; }

        SmartRegister<UInt32> RegStatus { get; }

        SmartRegister<UInt32> RegDmaFull { get; }

        SmartRegister<UInt32> RegDmaBusy { get; }

        SmartRegister<UInt32> RegSemaphore { get; }

        SmartRegister<UInt32> RegPc { get; }

        SmartRegister<UInt32> RegImemBist { get; }
    }

    public interface IRspRegMemoryAddress
    {
        Int32 Address { get; set; }

        RspMemMode Mode { get; set; }
    }

    public interface IRspRegDramAddress
    {
        Int32 Address { get; set; }
    }

    public interface IRspRegIoLength
    {
        Int32 Length { get; set; }

        Int32 Count { get; set; }

        Int32 Skip { get; set; }
    }

    public interface IRspRegReadStatus
    {
        Boolean Halt { get; }

        Boolean Broke { get; }

        Boolean DmaBusy { get; }

        Boolean DmaFull { get; }

        Boolean IoFull { get; }

        Boolean SingleStep { get; }

        Boolean InterruptOnBreak { get; }

        Boolean Signal0Set { get; }

        Boolean Signal1Set { get; }

        Boolean Signal2Set { get; }

        Boolean Signal3Set { get; }

        Boolean Signal4Set { get; }
        
        Boolean Signal5Set { get; }

        Boolean Signal6Set { get; }

        Boolean Signal7Set { get; }
    }

    public interface IRspRegWriteStatus
    {
        Boolean ClearHalt { get; set; }
        Boolean SetHalt { get; set; }
        Boolean ClearBroke { get; set; }
        Boolean ClearIntr { get; set; }
        Boolean SetIntr { get; set; }
        Boolean ClearSingleStep { get; set; }
        Boolean SetSingleStep { get; set; }
        Boolean ClearIntrOnBreak { get; set; }
        Boolean SetIntrOnBreak { get; set; }
        Boolean ClearSignal0 { get; set; }
        Boolean SetSignal0 { get; set; }
        Boolean ClearSignal1 { get; set; }
        Boolean SetSignal1 { get; set; }
        Boolean ClearSignal2 { get; set; }
        Boolean SetSignal2 { get; set; }
        Boolean ClearSignal3 { get; set; }
        Boolean SetSignal3 { get; set; }
        Boolean ClearSignal4 { get; set; }
        Boolean SetSignal4 { get; set; }
        Boolean ClearSignal5 { get; set; }
        Boolean SetSignal5 { get; set; }
        Boolean ClearSignal6 { get; set; }
        Boolean SetSignal6 { get; set; }
        Boolean ClearSignal7 { get; set; }
        Boolean SetSignal7 { get; set; }
    }
}
