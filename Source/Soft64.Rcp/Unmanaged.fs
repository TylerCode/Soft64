module internal Unmanaged

open System
open System.Runtime.InteropServices
open FSharp.NativeInterop;
open System.IO

#nowarn "9"

type internal UnmanagedStream(p:nativeint, size:int64) =
    inherit Stream()
    let mutable _Position = 0L
    let _Pointer = NativePtr.ofNativeInt<byte> p
    override this.CanRead with get() = true
    override this.CanSeek with get() = true
    override this.CanWrite with get() = true
    override this.Position with get() = _Position and set(value:int64) = _Position <- value
    override this.Length with get() = size
    override this.Flush() = ()
    override this.Seek(offset, origin) = 0L
    override this.SetLength(size) = ()

    override this.Read(buffer, index, count) =
        let mutable count = 0
        for i = 0 to count do 
            buffer.[index + i] <- NativePtr.get<byte> _Pointer i
            count <- count + 1
        count

    override this.Write(buffer, index, count) =
        for i = 0 to count do
            NativePtr.set<byte> _Pointer i buffer.[index + i]

type internal UnmanagedHeap(size:int32) =
    let _P = Marshal.AllocHGlobal(size)
    let _S = new UnmanagedStream(_P, int64 size) :> Stream
    member this.HeapStream with get() = _S
    member this.getPointer<'T when 'T : unmanaged>() = NativePtr.ofNativeInt<'T>(_P)
    interface IDisposable with
        member this.Dispose() = Marshal.FreeHGlobal(_P)
    end