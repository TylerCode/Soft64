module RcpMemory

// Avoid warnings for unmanaged code
#nowarn "9"

open System
open Microsoft.FSharp.NativeInterop
open RspConstants
open Unmanaged
open System.IO
open Soft64
open Soft64.RCP
open FSharp.Interop.Dynamic

type SRU32 = SmartRegister<UInt32>



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
type RspRegisterReadLength(p, o) =
    inherit RspRegisterBase(p, o)
    interface IRspRegReadLength with
        member this.Length with get() : int32 = this.AutoRegisterProps?GetLength and set(v:int32) = this.AutoRegisterProps?SetLength v
        member this.Count with get() : int32 = this.AutoRegisterProps?GetCount and set(v:int32) = this.AutoRegisterProps?SetCount v
        member this.Skip with get() : int32 = this.AutoRegisterProps?GetSkip and set(v:int32) = this.AutoRegisterProps?SetSkip v
    end

type RspMemory() =
    let _IMemHeap = new UnmanagedHeap(0x1000)
    let _DMemHeap = new UnmanagedHeap(0x1000)
    let _RegisterHeap = new UnmanagedHeap(1024 * 1024)
    let _p = _RegisterHeap.getPointer()
    let _1 = new RspRegisterMemoryAddress(_p, 0)
    let _2 = new RspRegisterDramAddress(_p, 0x4)
    let _3 = new RspRegisterReadLength(_p, 0x8)

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
        member this.RegReadLength with get()    = _3 :> IRspRegReadLength

        member this.RegWriteLength with get() = null
        member this.RegStatus with get() = null
        member this.RegDmaFull with get() = null
        member this.RegDmaBusy with get() = null
        member this.RegSemaphore with get() = null
        member this.RegPc with get() = null
        member this.RegImemBist with get() = null
    end