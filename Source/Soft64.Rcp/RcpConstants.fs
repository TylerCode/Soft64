module RspConstants

open System

[<Flags>]
type RcpInterruptType = 
    | Sp = 0b00000001uy 
    | Si = 0b00000010uy
    | Ai = 0b00000100uy
    | Vi = 0b00001000uy
    | Pi = 0b00010000uy
    | Dp = 0b00100000uy

[<Flags>]
type SpStatus =
    | Halt =    0b0000000000000001us
    | Broke =   0b0000000000000010us
    | DmaBusy = 0b0000000000000100us
    | DmaFull = 0b0000000000001000us
    | IoFull  = 0b0000000000010000us
    | Sstep   = 0b0000000000100000us
    | IntrBreak=0b0000000001000000us
    | Signal0 = 0b0000000010000000us
    | Signal1 = 0b0000000100000000us
    | Signal2 = 0b0000001000000000us
    | Signal3 = 0b0000010000000000us
    | Signal4 = 0b0000100000000000us
    | Signal5 = 0b0001000000000000us
    | Signal6 = 0b0010000000000000us
    | Signal7 = 0b0100000000000000us

[<Flags>]
type DpStatus = 
    | XbusDma =  0b0000000000000001us
    | Freeze =   0b0000000000000010us
    | Flush  =   0b0000000000000100us
    | StartGClk= 0b0000000000001000us
    | TmemBusy = 0b0000000000010000us
    | PipeBusy = 0b0000000000100000us
    | CmdBusy =  0b0000000001000000us
    | CBufReady= 0b0000000010000000us
    | DmaBusy  = 0b0000000100000000us
    | EndValid = 0b0000001000000000us
    | StartValid=0b0000010000000000us


[<Literal>]
let DACRATE_NTSC = 48681812

[<Literal>]
let DACRATE_PAL  = 49656530

[<Literal>]
let DACRATE_MPAL = 48628316

let R4300SpIntr = 1

(* Little endian or big endian control *)
let littleEndian = true // TODO: Set this based on the detected target host

let testLittleEndian t f = if littleEndian then t else f

let ByteAddrXor = testLittleEndian 3 0
let WordAddrXor = testLittleEndian 1 0
let Byte4XorBE a = testLittleEndian (a ^^^ 3uy) a