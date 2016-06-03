/*
Soft64 - C# N64 Emulator
Copyright (C) Soft64 Project @ Codeplex
Copyright (C) 2013 - 2015 Bryan Perris
	
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
    public static class Disassembler
    {
        private static Boolean m_O32 = true;

        private static Dictionary<String, OF> s_OperandFormatLUT = new Dictionary<String, OF>
        {
            ["X"] = (x) => $"",
            ["RD,RT,SA"] = (x) => $"{GPR(x.Rd)}, {GPR(x.Rt)}, {x.ShiftAmount}",
            ["RS,RT"] = (x) => $"{GPR(x.Rs)}, {GPR(x.Rt)}",
            ["RT,RS,IMM"] = (x) => $"{GPR(x.Rt)}, {GPR(x.Rs)} {x.Immediate:X4}",
            ["RD"] = (x) => $"{GPR(x.Rd)}",
            ["CP0,RT"] = (x) => $"{CP0(x.Rd)}, {GPR(x.Rt)}",
            ["RT,CP0"] = (x) => $"{GPR(x.Rt)}, {CP0(x.Rd)}",
            ["RS"] = (x) => $"{GPR(x.Rs)}",
            ["RD,RS,RT"] = (x) => $"{GPR(x.Rd)}, {GPR(x.Rs)}, {GPR(x.Rt)}",
            ["RD,RT,RS"] = (x) => $"{GPR(x.Rd)}, {GPR(x.Rt)}, {GPR(x.Rs)}",
            ["RS,IMM"] = (x) => $"{GPR(x.Rs)}, {x.Immediate:X4}",
            ["RT,IMM(RS)"] = (x) => $"{GPR(x.Rt)}, {x.Immediate:X4}({GPR(x.Rs)})",
            ["FT,IMM(RS)"] = (x) => $"{OpFormat(x)} {FPR(x.Ft)}, {x.Immediate:X4}({GPR(x.Rs)})",
            ["RT,IMM"] = (x) => $"{GPR(x.Rt)}, {x.Immediate:X4}",
            ["RS,RD"] = (x) => $"{GPR(x.Rs)}, {GPR(x.Rd)}",
            ["RS,RT,IMM"] = (x) => $"{GPR(x.Rs)}, {GPR(x.Rt)}, {x.Immediate:X4}",
            ["RT,FS"] = (x) => $"{OpFormat(x)} {GPR(x.Rt)}, {FPR(x.Fs)}",
            ["FS,RT"] = (x) => $"{OpFormat(x)} {FPR(x.Fs)}, {GPR(x.Rt)}",
            ["FD,FS"] = (x) => $"{OpFormat(x)} {FPR(x.Fd)}, {FPR(x.Fs)}",
            ["FD,FS,FT"] = (x) => $"{OpFormat(x)} {FPR(x.Fd)}, {FPR(x.Fs)}, {FPR(x.Ft)}",
            ["COND"] = (x) => $"{Cond(x.FC)}.{OpFormat(x)} {FPR(x.Fs)}, {FPR(x.Ft)} {Convert.ToString(x.Cond, 2)}",
            ["JUMP"] = (x) => $"{x.Target:X8} ---> {JumpTarget(x):X16}",
            ["BRANCH"] = (x) => $"{GPR(x.Rs)}, {GPR(x.Rt)} {x.Immediate:X4} ---> {BranchTarget(x):X16}",
            ["BRANCH_Z"] = (x) => $"{GPR(x.Rs)}, {x.Immediate:X4} ---> {BranchTarget(x):X16}",
            ["OFFSET"] = (x) => $"{x.Immediate:X4}",
        };

        private static String[] s_O32GprLabel =
        {
            "$zero", "$at", "$v0", "$v1", "$a0", "$a1", "$a2", "$a3",
            "$t0",   "$t1", "$t2", "$t3", "$t4", "$t5", "$t6", "$t7",
            "$s0",   "$s1", "$s2", "$s3", "$s4", "$s5", "$s6", "$s7",
            "$t8",   "$t9", "$k0", "$k1", "$gp", "$sp", "$fp", "$ra",
        };

        private static String[] s_GprLabel =
        {
            "r00",  "r01", "r02", "r03", "r04", "r05", "r06", "r07",
            "r08",  "r09", "r10", "r11", "r12", "r13", "r14", "r15",
            "r16",  "r17", "r18", "r19", "r20", "r21", "r22", "r23",
            "r24",  "r25", "r26", "r27", "r28", "r29", "r30", "r31",
        };

        private static String[] s_Cop1RegLabel =
        {
            "fpr0",  "fpr1",  "fpr2",  "fpr3",  "fpr4",  "fpr5",  "fpr6",  "fpr7",
            "fpr8",  "fpr9",  "fpr10", "fpr11", "fpr12", "fpr13", "fpr14", "fpr15",
            "fpr16", "fpr17", "fpr18", "fpr19", "fpr20", "fpr21", "fpr22", "fpr23",
            "fpr24", "fpr25", "fpr26", "fpr27", "fpr28", "fpr29", "fpr30", "fpr31",
        };

        private static String[] s_Cop0RegLabel =
        {
            "Index",    "Random",   "EntryLo0", "EntryLo1",
            "Context",  "PageMask", "Wired",    "Reserved",
            "BadVAddr", "Count",    "EntryHi",  "Compare",
            "Status",   "Cause",    "EPC",      "PRId",
            "Config",   "LLAddr",   "WatchLo",  "WatchHi",
            "XContext", "Reserved", "Reserved", "Reserved",
            "Reserved", "Reserved", "PErr",     "CacheErr",
            "TagLo",    "TagHi",    "ErrorEPC", "Reserved",
        };

        private static String[] s_OpTableMain =
        {
            "_SPECIAL", "_REGIMM", "j JUMP",    "jal JUMP",   "beq BRANCH",  "bne BRANCH",   "blez BRANCH_Z",  "bgtz BRANCH_Z",
            "addi RT,RS,IMM",     "addiu RT,RS,IMM",   "slti RT,RS,IMM",  "sltiu RT,RS,IMM", "andi RT,RS,IMM", "ori RT,RS,IMM",   "xori RT,RS,IMM",  "lui RT,IMM",
            "_COP0",    "_COP1",   null,     null,   "beql BRANCH", "bnel BRANCH",  "blezl BRANCH_Z", "bgtzl BRANCH_Z",
            "daddi RT,RS,IMM",    "daddiu RT,RS,IMM",  "ldl RT,IMM(RS)",   "ldr RT,IMM(RS)",   null,   null,   null,    null,
            "lb RT,IMM(RS)",       "lh RT,IMM(RS)",      "lwl RT,IMM(RS)",   "lw RT,IMM(RS)",    "lbu RT,IMM(RS)",  "lhu RT,IMM(RS)",  "lwr RT,IMM(RS)",   "lwu RT,IMM(RS)",
            "sb RT,IMM(RS)",       "sh RT,IMM(RS)",      "swl RT,IMM(RS)",   "sw RT,IMM(RS)",    "sdl RT,IMM(RS)",  "sdr RT,IMM(RS)",  "swr RT,IMM(RS)",   "cache RT,IMM(RS)",
            "ll RT,IMM(RS)",       "lwc1 FT,IMM(RS)",    null,   null,   "lld RT,IMM(RS)",  "ldc1 FT,IMM(RS)", null,  "ld RT,IMM(RS)",
            "sc RT,IMM(RS)",       "swc1 FT,IMM(RS)",    null,   null,   "scd RT,IMM(RS)",  "sdc1 FT,IMM(RS)", null,  "sd RT,IMM(RS)",
        };

        private static String[] s_OpTableCop0 =
        {
            "mfc0 RT,CP0", "dmfc0 RT,CP0", null, null, "mtc0 CP0,RT", "dmtc0 CP0,RT", null, null,
            null,   null,    null,   null, null,   null,    null,   null,
            null, null,    null,   null, null,   null,    null,   null,
            null,   null,    null,   null, null,   null,    null,   null,
        };

        private static String[] s_OpTableTlb =
        {
            null,   "tlbr X", "tlbwi X", null, null, null, "tlbwr X", null,
            "tlbp X", null,   null,    null, null, null, null,    null,
            null,   null,   null,    null, null, null, null,    null,
            "eret X", null,   null,    null, null, null, null,    null,
            null,   null,   null,    null, null, null, null,    null,
            null,   null,   null,    null, null, null, null,    null,
        };

        private static String[] s_OpTableCop1 =
{
            "mfc1 RT,FS", "dmfc1 RT,FS", "cfc1 RT,FS", null, "mtc1 FS,RT", "dmtc1 FS,RT", "ctc1 FS,RT", null,
            "_BC1", null,    null,   null, null,   null,    null,   null,
            "_FPU", "_FPU",    null,   null, "_FPU",  "_FPU",   null,   null,
            null,  null,     null,   null, null,   null,    null,   null,
        };

        private static String[] s_OpTableBC1 =
        {
            "bc1f OFFSET",  "bc1t OFFSET",
            "bc1fl OFFSET", "bc1tl OFFSET",
        };

        private static String[] s_OpTableFpu =
        {
            "add FD,FS,FT",     "sub FD,FS,FT",     "mul FD,FS,FT",    "div FD,FS,FT",     "sqrt FD,FS",    "abs FD,FS",     "mov FD,FS",    "neg FD,FS",
            "round.l FD,FS", "trunc.l FD,FS", "ceil.l FD,FS", "floor.l FD,FS", "round.w FD,FS", "trunc.w FD,FS", "ceil.w FD,FS", "floor.w FD,FS",
            null,      null,      null,     null,      null,      null,      null,     null,
            null,      null,      null,     null,      null,      null,      null,     null,
            "cvt.s FD,FS",   "cvt.d FD,FS",   null,     null,      "cvt.w FD,FS",   "cvt.l FD,FS",   null,     null,
            null,      null,      null,     null,      null,      null,      null,     null,
            "c COND",     "c COND",    "c COND",   "c COND",   "c COND",   "c COND",   "c COND",  "c COND",
            "c COND",    "c COND",  "c COND",  "c COND",   "c COND",    "c COND",   "c COND",   "c COND",
        };

        private static String[] s_OpTableRegImm =
        {
            "bltz BRANCH_Z",   "bgez BRANCH_Z",   "bltzl BRANCH_Z",   "bgezl BRANCH_Z",   null,   null, null,   null,
            "tgei RS,IMM",   "tgeiu RS,IMM",  "tlti RS,IMM", "tltiu RS,IMM",   "teqi RS,IMM", null, "tnei RS,IMM", null,
            "bltzal BRANCH_Z", "bgezal BRANCH_Z", "bltzall BRANCH_Z", "bgezall BRANCH_Z", null,   null, null,   null,
            null,     null,     null,      null,      null,   null, null,   null,
        };

        private static String[] s_OpTableSpecial =
        {
            "sll RD,RT,SA",  null,    "srl RD,RT,SA",  "sra RD,RT,SA",  "sllv RD,RT,RS",    null,  "srlv RD,RT,RS",   "srav RD,RT,RS",
            "jr RS",   "jalr RS,RD",  null,   null,   "syscall X", "break X",  null,     "sync X",
            "mfhi RD", "mthi RD",  "mflo RD", "mtlo RD", "dsllv RD,RT,RS",   null,     "dsrlv RD,RT,RS",  "dsrav RD,RT,RS",
            "mult RS,RT", "multu RS,RT", "div RS,RT",  "divu RS,RT", "dmult RS,RT",   "dmultu RS,RT", "ddiv RS,RT",   "ddivu RS,RT",
            "add RD,RS,RT",  "addu RD,RS,RT",  "sub RD,RS,RT",  "subu RD,RS,RT", "and RD,RS,RT",     "or RD,RS,RT",     "xor RD,RS,RT",    "nor RD,RS,RT",
            null,   null,    "slt RD,RS,RT",  "sltu RD,RS,RT", "dadd RD,RS,RT",    "daddu RD,RS,RT",  "dsub RD,RS,RT",   "dsubu RD,RS,RT",
            "tge RS,RT",  "tgeu RS,RT",  "tlt RS,RT",  "tltu RS,RT", "teq RS,RT",     null,     "tne RS,RT",    null,
            "dsll RD,RT,SA", null,    "dsrl RD,RT,SA", "dsra RD,RT,SA", "dsll32 RD,RT,SA",  null,     "dsrl32 RD,RT,SA", "dsra32 RD,RT,SA",
        };

        public static String[] s_FPUCond =
        {
            "f", "un", "eq", "olt", "ult", "ole", "ule", "sf",
            "ngle", "sqe", "ngl", "lt", "nge", "le", "ngt"
        };

        public static String Cond(Int32 index)
        {
            if (index >= 0 && index <= 15)
                return s_FPUCond[index];
            else
                return index.ToString("X2");
        }

        public static String GPR(Int32 index)
        {
            if (m_O32)
                return s_O32GprLabel[index];
            else
                return s_GprLabel[index];
        }

        public static String FPR(Int32 index)
        {
            return s_Cop1RegLabel[index];
        }

        public static String CP0(Int32 index)
        {
            return s_Cop0RegLabel[index];
        }

        public static String OpFormat(MipsInstruction inst)
        {
            switch (inst.DecodeDataFormat())
            {
                case DataFormat.Double: return ".D";
                case DataFormat.Doubleword: return ".L";
                case DataFormat.Word: return ".W";
                case DataFormat.Single: return ".S";
                default: return ".?";
            }
        }

        public static Int64 BranchTarget(MipsInstruction inst)
        {
            return Interpreter.BranchComputeTargetAddress(inst);
        }

        public static Int64 JumpTarget(MipsInstruction inst)
        {
            return Interpreter.JumpComputeTargetAddress(inst);
        }

        private static void DecodeParts(MipsInstruction inst, String entity, ref String op, ref String operands)
        {
            Int32 index = entity.IndexOf(" ");
            op = entity.Substring(0, index);
            operands = s_OperandFormatLUT[entity.Substring(index + 1)](inst);
        }

        public static DisassemblyString Disassemble(MipsInstruction inst)
        {
            DisassemblyString disasm = new DisassemblyString();

            if (inst.Instruction == 0)
            {
                disasm.Opcode = "nop";
                disasm.Operands = "";
                return disasm;
            }

            String entity = s_OpTableMain[inst.Opcode];
            String op = "";
            String operands = "";

            if (entity != null)
            {
                switch (entity)
                {
                    case "_SPECIAL":
                        {
                            DecodeParts(inst, s_OpTableSpecial[inst.Function], ref op, ref operands);
                            break;
                        }

                    case "_REGIMM":
                        {
                            DecodeParts(inst, s_OpTableRegImm[inst.Rt], ref op, ref operands);
                            break;
                        }

                    case "_COP0":
                        {
                            switch (inst.Rs)
                            {
                                case 0x10: DecodeParts(inst, s_OpTableTlb[inst.Function], ref op, ref operands); break;
                                default: DecodeParts(inst, s_OpTableCop0[inst.Rs], ref op, ref operands); break;
                            }

                            break;
                        }

                    case "_COP1":
                        {
                            switch (s_OpTableCop1[inst.Rs])
                            {
                                case "_BC1": DecodeParts(inst, s_OpTableBC1[inst.Rt], ref op, ref operands); break;
                                case "_FPU": DecodeParts(inst, s_OpTableFpu[inst.Function], ref op, ref operands); break;
                                default: DecodeParts(inst, s_OpTableCop1[inst.Rs], ref op, ref operands); break;
                            }
                            break;
                        }

                    default:  DecodeParts(inst, entity, ref op, ref operands); break;
                }

                disasm.Opcode = op;
                disasm.Operands = operands;
                return disasm;
            }
            else
            {
                return null;
            }
        }


        public static Boolean DecodeABINames
        {
            get { return m_O32; }
            set { m_O32 = value; }
        }
    }

    public class DisassemblyString
    {
        public String Opcode { get; internal set; }
        public String Operands { get; internal set; }
    }
}