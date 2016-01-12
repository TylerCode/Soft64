module internal RspMemory

// Avoid warnings for unmanaged code
#nowarn "9"

open System
open Microsoft.FSharp.NativeInterop
open RspConstants
open Unmanaged
open System.IO

type RspMemoryRegisters() =
    let _Heap = new UnmanagedHeap(1024 * 1024)
    let _pUInt32 = _Heap.getPointer<uint32>()
    let read o = NativePtr.get<uint32> _pUInt32 o
    let write o v = NativePtr.set<uint32> _pUInt32 o v
    interface IDisposable with member this.Dispose() = (_Heap :> IDisposable).Dispose()
    member this.RawMemoryStream with get() = _Heap.HeapStream
    member this.MemoryAddress with get() = read 0x0 and set(value:uint32) = write 0x0 value
    member this.DramAddress with get() = read 0x4 and set(value:uint32) = write 0x4 value
    member this.ReadLength with get() = read 0x8 and set(value:uint32) = write 0x8 value
    member this.WriteLength with get() = read 0xC and set(value:uint32) = write 0xC value
    member this.Status with get() = read 0x10 and set(value:uint32) = write 0x10 value
    member this.DmaFull with get() = read 0x14 and set(value:uint32) = write 0x14 value
    member this.DmaBusy with get() = read 0x18 and set(value:uint32) = write 0x18 value
    member this.Semaphire with get() = read 0x1C and set (value:uint32) = write 0x1C value
    member this.PC with get() = read 0x40000 and set (value:uint32) = write 0x40000 value
    member this.ImemBist with get() = read 0x40004 and set (value:uint32) = write 0x40004 value