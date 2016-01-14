module RspStructures

open System
open FSharp.Core
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop
open RspConstants

#nowarn "9"

(* Define some register structures for the RSP *)
type RspMemMode =
    | Dmem = 0
    | Imem = 1

[<StructLayoutAttribute(LayoutKind.Explicit)>]
type private _RspRegisterMemoryAddress =
    struct
        [<FieldOffset(00); DefaultValue>] val mutable Register : uint32
        [<FieldOffset(00); DefaultValue>] val mutable Address : uint16
        [<FieldOffset(12); DefaultValue>] val mutable Mode : bool
    end

type internal __RspRegisterMemoryAddress() =
    let mutable _Reg = new _RspRegisterMemoryAddress()
    member this.Register with get() = _Reg.Register and set(v) = _Reg.Register <- v
    member this.Address with get() = _Reg.Address &&& 0x7FFus and set(v) = _Reg.Address <- (v &&& 0x7FFus)
    member this.Mode with get() = enum<RspMemMode>(Convert.ToInt32 _Reg.Mode) and set(v:RspMemMode) = _Reg.Mode <- int32 v |> Convert.ToBoolean