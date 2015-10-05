/*
Soft64 - C# N64 Emulator
Copyright (C) Soft64 Project @ Codeplex
Copyright (C) 2013 - 2014 Bryan Perris

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>
*/

using System;
using Soft64.MipsR4300;
using System.Collections.Generic;
using OF = System.Func<Soft64.MipsR4300.MipsInstruction, System.String>;

namespace Soft64.MipsR4300.Debugging
{
    public static partial class Disassembler
    {
        /* This is here to to make sure the compiler compiles in this table initialization before anything else */

        private static Boolean m_O32;

        #region Format LUT

        public static String GPR(Int32 index)
        {
            return DecodeGprReg(index, m_O32);
        }

        public static String FPR(Int32 index)
        {
            return DecodeFpuReg(index);
        }

        public static String OpFormat(MipsInstruction inst)
        {
            switch (inst.DecodeDataFormat())
            {
                case DataFormat.Double: return ".d";
                case DataFormat.Doubleword: return ".l";
                case DataFormat.Word: return ".w";
                case DataFormat.Single: return ".s";
                default: return ".?";
            }
        }

        private static Dictionary<String, OF> s_OperandFormatLUT = new Dictionary<String, OF>
        {
            ["RT,RD,SA"] = (x) => $"{GPR(x.Rt)}, {GPR(x.Rd)}, {x.ShiftAmount}",
            ["RS,RT"] = (x) => $"{GPR(x.Rs)}, {GPR(x.Rt)}",
            ["RD"] = (x) => $"{GPR(x.Rd)}",
            ["RS"] = (x) => $"{GPR(x.Rs)}",
            ["RS,RT,RD"] = (x) => $"{GPR(x.Rs)}, {GPR(x.Rt)}, {GPR(x.Rd)}",
            ["RS,RT,RD,SA"] = (x) => $"{GPR(x.Rs)}, {GPR(x.Rt)}, {GPR(x.Rd)}, {x.ShiftAmount}",
            ["RS,IMM"] = (x) => $"{GPR(x.Rs)}, {x.Immediate:X4}",
            ["RT,IMM(RS)"] = (x) => $"{GPR(x.Rt)}, {x.Immediate:X4}({GPR(x.Rs)})",
            [""] = (x) => $"",
            ["RT,IMM"] = (x) => $"{GPR(x.Rt)}, {x.Immediate:X4}",
            ["TARGET"] = (x) => $"{x.Target:X8}",
            ["RS,RD"] = (x) => $"{GPR(x.Rs)}, {GPR(x.Rd)}",
            ["RS,RT,IMM"] = (x) => $"{GPR(x.Rs)}, {GPR(x.Rt)}, {x.Immediate:X4}",
            ["RT,FS"] = (x) => $"{OpFormat(x)} {GPR(x.Rt)}, {FPR(x.Fs)}",
            ["FS,FD"] = (x) => $"{OpFormat(x)} {FPR(x.Fs)}, {FPR(x.Fd)}",

        };

            ///* 18: BC1T type instructions */
            //(inst, o32) => { return String.Format("ND:{0}, TF:{1}, 0x{2:x4}", Convert.ToBoolean((inst.Rt >> 1) & 1), Convert.ToBoolean(inst.Rt & 1), inst.Immediate); },

            ///* 20: FPU Instructions using ft, fs, fd */
            //(inst, o32) => { return String.Format("{0}, {1}, {2}", DecodeFpuReg(inst.Rt), DecodeFpuReg(inst.Rd), DecodeFpuReg(inst.ShiftAmount)); },

            ///* 21: FPU Instructions using ft, fs */
            //(inst, o32) => { return String.Format("{0}, {1}", DecodeFpuReg(inst.Rt), DecodeFpuReg(inst.ShiftAmount)); },

            ///* 22: FPU Instructions using rt, and fs */
            //(inst, o32) => { return String.Format("{0}, {1}", DecodeGprReg(inst.Rt, o32), DecodeFpuReg(inst.Rd)); },

            ///* 23: Immediate Instructions using FPR reg */
            //(inst, o32) => { return String.Format("{0:x2}, {1}, 0x{2:x4}", inst.Rs, DecodeFpuReg(inst.Rt), inst.Immediate); },

            ///* 24: Common Immediate format */
            //(inst, o32) => { return String.Format("{0}, {1}, 0x{2:x8}",  DecodeGprReg(inst.Rs, o32), DecodeGprReg(inst.Rt, o32), inst.Immediate); },

            ///* 25: Conditional Branch formats */
            //(inst, o32) => { return String.Format("{0}, {1}, 0x{2:x8} -->{3:X8}", DecodeGprReg(inst.Rs, o32), DecodeGprReg(inst.Rt, o32), inst.Immediate,
            //                            Interpreter.BranchComputeTargetAddress(inst.Address, inst.Immediate)); },
        //};

        #endregion Format LUT

        public static string DecodeOpName(MipsInstruction asmInstruction)
        {
            if (asmInstruction.Instruction == 0)
            {
                return "nop";
            }

            String mainOp = s_OpTableMain[asmInstruction.Opcode];
            String opName = "";

            if (mainOp != null)
            {
                switch (mainOp)
                {
                    case "_SPECIAL": opName = s_OpTableSpecial[asmInstruction.Function]; break;
                    case "_REGIMM": opName = s_OpTableRegImm[asmInstruction.Rt]; break;
                    case "_COP0":
                        {
                            switch (asmInstruction.Rs)
                            {
                                case 0x10: opName = s_OpTableTlb[asmInstruction.Function]; break;
                                default: opName = s_OpTableCop0[asmInstruction.Rs]; break;
                            }
                            break;
                        }
                    case "_COP1":
                        {
                            switch (s_OpTableCop1[asmInstruction.Rs])
                            {
                                case "_BC1": opName = s_OpTableBC1[asmInstruction.Rt]; break;
                                case "_FPU": opName = s_OpTableFpu[asmInstruction.Function]; break;
                                default: opName = s_OpTableCop1[asmInstruction.Rs]; break;
                            }
                            break;
                        }
                    default: return mainOp;
                }

                return opName;
            }
            else
            {
                return null;
            }
        }

        public static string DecodeGprReg(Int32 reg, Boolean o32)
        {
            if (o32)
                return s_O32GprLabel[reg];
            else
                return s_GprLabel[reg];
        }

        private static String DecodeOperand(MipsInstruction inst, Boolean o32, OperandDictionary lut, Int32 reg, String defaultValue)
        {
            if (inst.Instruction == 0)
                return "";

            if (lut.ContainsKey(reg))
                return lut[reg](inst, o32);
            else
                return defaultValue;
        }

        public static string DecodeOperands(MipsInstruction inst, Boolean o32)
        {
            /* First figure out the general format of the instruction */
            try
            {
                switch (inst.Opcode)
                {
                    /* Special Instructions */
                    /* Defaults to dictionary of Special opcodes */
                    case 0: return DecodeOperand(inst, o32, s_OperandLUT_Special, inst.Function, s_OperandFormatLUT[4](inst, o32));

                    /* Register-Immediate Instructions */
                    case 1: return s_OperandFormatLUT[6](inst, o32);

                    /* Cop0 Instructions */
                    case 16:
                        {
                            switch (inst.Rs)
                            {
                                /* TLB Instructions */
                                case 16: return s_OperandFormatLUT[16](inst, o32);

                                /* Defaults to dictionary of Cop0 Opcodes */
                                default: return DecodeOperand(inst, o32, s_OperandLUT_Cop0, inst.Rs, s_OperandFormatLUT[05](inst, o32));
                            }
                        }

                    /* FPU Instructions */
                    case 17:
                        {
                            switch (inst.Rs)
                            {
                                /* BC1T Instructions */
                                case 08: return s_OperandFormatLUT[18](inst, o32);

                                /* All FPU Arithmetic Cases */
                                case 16: /* Fmt = Single */
                                case 17: /* Fmt = Double */
                                case 20: /* Fmt = Word */
                                case 21: /* Fmt = Long */
                                    return DecodeOperand(inst, o32, s_OperandLUT_FPU, inst.Function, s_OperandFormatLUT[04](inst, o32));

                                /* Defaults to dictionary of FPU Opcodes */
                                default: return DecodeOperand(inst, o32, s_OperandLUT_Cop1, inst.Rs, s_OperandFormatLUT[05](inst, o32));
                            }
                        }

                    /* Defaults to dictionary of Main Opcodes */
                    default: return DecodeOperand(inst, o32, s_OperandLUT_Main, inst.Opcode, s_OperandFormatLUT[24](inst, o32));
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string DecodeFpuReg(Int32 reg)
        {
            return s_Cop1RegLabel[reg];
        }

        public static string DecodeCop0Reg(Int32 reg)
        {
            return s_Cop0RegLabel[reg];
        }
    }
}