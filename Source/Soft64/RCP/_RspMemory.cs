using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Soft64.RCP
{
    public enum RspMemMode : int
    {
        Dmem = 0,
        Imem = 1
    }

    public class _RspMemory
    {
    }

    [RegisterField("Address", 0, 12, typeof(UInt32))]
    [RegisterField("Mode", 12, 1, typeof(Boolean))]
    public sealed class RspRegister_MemoryAddress : MemoryMappedRegister32
    {
        internal RspRegister_MemoryAddress(IntPtr p, Int32 o) : base (p, o)
        {

        }

        public Int64 Address
        {
            get { return (Int32)AutoRegisterProps.GetAddress(); }
            set { AutoRegisterProps.SetAddress((UInt32)(Int32)value); }
        }

        public RspMemMode Mode
        {
            get { return (RspMemMode)((Boolean)AutoRegisterProps.GetMode()).BoolToInt32(); }
            set { AutoRegisterProps.SetMode(((Int32)value).Int32ToBool()); }
        }
    }

    internal static class RegExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static Int32 BoolToInt32(this Boolean b)
        {
            return *(Int32*)(&b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static Boolean Int32ToBool(this Int32 i)
        {
            return *(Boolean*)(&i);
        }
    }

}


/* 

    (* Define a special register base that uses unmanaged code *)
type RspRegisterBase(p:nativeptr<uint32>, offset:int32) = 
    inherit SmartRegister<uint32>()
    override this.InternalRegister with get() = NativePtr.get<uint32> p offset and set(v) = NativePtr.set<uint32> p offset v

[<RegisterField("Address", 0, 12, typeof<uint32>);
  RegisterField("Mode", 12, 1, typeof<bool>)>]
type RspRegisterMemoryAddress(p, o) =
    inherit RspRegisterBase(p, o)
    interface IRspRegMemoryAddress with
        member this.Address with get() = int32 this.AutoRegisterProps?GetAddress and set(v:int32) = this.AutoRegisterProps?SetAddress v
        member this.Mode 
            with get() = enum<RspMemMode>(int32 this.AutoRegisterProps?GetMode) 
            and set(v:RspMemMode) = this.AutoRegisterProps?SetMode <| bool <| int32 v
    end

[<RegisterField("Address", 0, 24, typeof<uint32>)>]
type RspRegisterDramAddress(p, o) =
    inherit RspRegisterBase(p, o)
    interface IRspRegDramAddress with
        member this.Address with get() = int32 this.AutoRegisterProps?GetAddress and set(v:int32) = this.AutoRegisterProps?SetAddress v
    end

[<RegisterField("Length", 0, 12, typeof<int32>);
  RegisterField("Count", 12, 8, typeof<int32>);
  RegisterField("Skip", 20, 12, typeof<int32>)>]
type RspRegisterIoLength(p, o) =
    inherit RspRegisterBase(p, o)
    interface IRspRegIoLength with
        member this.Length with get() : int32 = this.AutoRegisterProps?GetLength and set(v:int32) = this.AutoRegisterProps?SetLength v
        member this.Count with get() : int32 = this.AutoRegisterProps?GetCount and set(v:int32) = this.AutoRegisterProps?SetCount v
        member this.Skip with get() : int32 = this.AutoRegisterProps?GetSkip and set(v:int32) = this.AutoRegisterProps?SetSkip v
    end

[<RegisterField("CH", 00, 1, typeof<bool>);
  RegisterField("SH", 01, 1, typeof<bool>);
  RegisterField("CB", 02, 1, typeof<bool>);
  RegisterField("CI", 03, 1, typeof<bool>);
  RegisterField("SI", 04, 1, typeof<bool>);
  RegisterField("CS", 05, 1, typeof<bool>);
  RegisterField("SS", 06, 1, typeof<bool>);
  RegisterField("CIB", 07, 1, typeof<bool>);
  RegisterField("SIB", 08, 1, typeof<bool>);
  RegisterField("CS0", 09, 1, typeof<bool>);
  RegisterField("SS0", 10, 1, typeof<bool>);
  RegisterField("CS1", 11, 1, typeof<bool>);
  RegisterField("SS1", 12, 1, typeof<bool>);
  RegisterField("CS2", 13, 1, typeof<bool>);
  RegisterField("SS2", 14, 1, typeof<bool>);
  RegisterField("CS3", 15, 1, typeof<bool>);
  RegisterField("SS3", 16, 1, typeof<bool>);
  RegisterField("CS4", 17, 1, typeof<bool>);
  RegisterField("SS4", 18, 1, typeof<bool>);
  RegisterField("CS5", 19, 1, typeof<bool>);
  RegisterField("SS5", 20, 1, typeof<bool>);
  RegisterField("CS6", 21, 1, typeof<bool>);
  RegisterField("SS6", 22, 1, typeof<bool>);
  RegisterField("CS7", 23, 1, typeof<bool>);
  RegisterField("SS7", 24, 1, typeof<bool>);>]
type RspRegisterWriteStatus(p, o) =
    inherit RspRegisterBase(p, o)
    interface IRspRegWriteStatus with
        member this.ClearHalt with get() :        bool = this.AutoRegisterProps?GetCH and set(v:bool) = this.AutoRegisterProps?SetCH v
        member this.SetHalt with get() :          bool = this.AutoRegisterProps?GetSH and set(v:bool) = this.AutoRegisterProps?SetSH v
        member this.ClearBroke with get() :       bool = this.AutoRegisterProps?GetCB and set (v:bool) = this.AutoRegisterProps?SetCB v
        member this.ClearIntr with get() :        bool = this.AutoRegisterProps?GetCI and set (v:bool) = this.AutoRegisterProps?SetCI v
        member this.SetIntr with get() :          bool = this.AutoRegisterProps?GetSI and set (v:bool) = this.AutoRegisterProps?SetSI v
        member this.ClearSingleStep with get() :  bool = this.AutoRegisterProps?GetCS and set (v:bool) = this.AutoRegisterProps?SetCS v
        member this.SetSingleStep with get() :    bool = this.AutoRegisterProps?GetSS and set(v:bool) = this.AutoRegisterProps?SetSS v
        member this.ClearIntrOnBreak with get() : bool = this.AutoRegisterProps?GetCIB and set (v:bool) = this.AutoRegisterProps?SetCIB v
        member this.SetIntrOnBreak with get() :   bool = this.AutoRegisterProps?GetSIB and set (v:bool) = this.AutoRegisterProps?SetSIB v
        member this.ClearSignal0 with get() :     bool = this.AutoRegisterProps?GetCS0 and set (v:bool) = this.AutoRegisterProps?SetCS0 v
        member this.SetSignal0 with get() :       bool = this.AutoRegisterProps?GetSS0 and set (v:bool) = this.AutoRegisterProps?SetSS0 v
        member this.ClearSignal1 with get() :     bool = this.AutoRegisterProps?GetCS1 and set (v:bool) = this.AutoRegisterProps?SetCS1 v
        member this.SetSignal1 with get() :       bool = this.AutoRegisterProps?GetSS1 and set (v:bool) = this.AutoRegisterProps?SetSS1 v
        member this.ClearSignal2 with get() :     bool = this.AutoRegisterProps?GetCS2 and set (v:bool) = this.AutoRegisterProps?SetCS2 v
        member this.SetSignal2 with get() :       bool = this.AutoRegisterProps?GetSS2 and set (v:bool) = this.AutoRegisterProps?SetSS2 v
        member this.ClearSignal3 with get() :     bool = this.AutoRegisterProps?GetCS3 and set (v:bool) = this.AutoRegisterProps?SetCS3 v
        member this.SetSignal3 with get() :       bool = this.AutoRegisterProps?GetSS3 and set (v:bool) = this.AutoRegisterProps?SetSS3 v
        member this.ClearSignal4 with get() :     bool = this.AutoRegisterProps?GetCS4 and set (v:bool) = this.AutoRegisterProps?SetCS4 v
        member this.SetSignal4 with get() :       bool = this.AutoRegisterProps?GetSS4 and set (v:bool) = this.AutoRegisterProps?SetSS4 v
        member this.ClearSignal5 with get() :     bool = this.AutoRegisterProps?GetCS5 and set (v:bool) = this.AutoRegisterProps?SetCS5 v
        member this.SetSignal5 with get() :       bool = this.AutoRegisterProps?GetSS5 and set (v:bool) = this.AutoRegisterProps?SetSS5 v
        member this.ClearSignal6 with get() :     bool = this.AutoRegisterProps?GetCS6 and set (v:bool) = this.AutoRegisterProps?SetCS6 v
        member this.SetSignal6 with get() :       bool = this.AutoRegisterProps?GetSS6 and set (v:bool) = this.AutoRegisterProps?SetSS6 v
        member this.ClearSignal7 with get() :     bool = this.AutoRegisterProps?GetCS7 and set (v:bool) = this.AutoRegisterProps?SetCS7 v
        member this.SetSignal7 with get() :       bool = this.AutoRegisterProps?GetSS7 and set (v:bool) = this.AutoRegisterProps?SetSS7 v
    end

type RspMemory() =
    let _IMemHeap = new UnmanagedHeap(0x1000)
    let _DMemHeap = new UnmanagedHeap(0x1000)
    let _RegisterHeap = new UnmanagedHeap(1024 * 1024)
    let _p = _RegisterHeap.getPointer()
    let _1 = new RspRegisterMemoryAddress(_p, 0)
    let _2 = new RspRegisterDramAddress(_p, 4)
    let _3 = new RspRegisterIoLength(_p, 8)
    let _4 = new RspRegisterIoLength(_p, 12)
    let _5 = new RspRegisterWriteStatus(_p, 16)

    interface IDisposable with 
        member this.Dispose() = 
            (_IMemHeap :> IDisposable).Dispose()
            (_DMemHeap :> IDisposable).Dispose()
            (_RegisterHeap :> IDisposable).Dispose()
    end



//    member this.RawMemoryStream with get() = _Heap.HeapStream
//    member this.MemoryAddress with get() = read 0x0 and set(value:uint32) = write 0x0 value
//    member this.DramAddress with get() = read 0x4 and set(value:uint32) = write 0x4 value
//    member this.ReadLength with get() = read 0x8 and set(value:uint32) = write 0x8 value
//    member this.WriteLength with get() = read 0xC and set(value:uint32) = write 0xC value
//    member this.Status with get() = read 0x10 and set(value:uint32) = write 0x10 value
//    member this.DmaFull with get() = read 0x14 and set(value:uint32) = write 0x14 value
//    member this.DmaBusy with get() = read 0x18 and set(value:uint32) = write 0x18 value
//    member this.Semaphire with get() = read 0x1C and set (value:uint32) = write 0x1C value
//    member this.PC with get() = read 0x40000 and set (value:uint32) = write 0x40000 value
//    member this.ImemBist with get() = read 0x40004 and set (value:uint32) = write 0x40004 value

    interface Soft64.RCP.IRspMemory with
        member this.IMemoryStream with get() = _IMemHeap.HeapStream
        member this.DMemoryStream with get() = _DMemHeap.HeapStream
        member this.RegisterStream with get() = _RegisterHeap.HeapStream
        member this.RegMemoryAddress with get() = _1 :> IRspRegMemoryAddress
        member this.RegDramAddress with get()   = _2 :> IRspRegDramAddress
        member this.RegReadLength with get()    = _3 :> IRspRegIoLength
        member this.RegWriteLength with get()   = _4 :> IRspRegIoLength
        member this.RegWriteonlyStatus with get()= _5 :> IRspRegWriteStatus


        member this.RegStatus with get() = null
        member this.RegDmaFull with get() = null
        member this.RegDmaBusy with get() = null
        member this.RegSemaphore with get() = null
        member this.RegPc with get() = null
        member this.RegImemBist with get() = null
    end
*/
