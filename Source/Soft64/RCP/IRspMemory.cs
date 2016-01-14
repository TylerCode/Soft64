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

        IRspRegReadLength RegReadLength { get; }

        SmartRegister<UInt32> RegWriteLength { get; }

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

    public interface IRspRegReadLength
    {
        Int32 Length { get; set; }

        Int32 Count { get; set; }

        Int32 Skip { get; set; }
    }
}
