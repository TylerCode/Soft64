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
using System.Runtime.InteropServices;
using System.Text;
using Soft64.MipsR4300.Debugging;

namespace Soft64.MipsR4300
{
    public struct MipsInstruction
    {
        public UInt32 Instruction { get; private set; }
        public UInt16 Immediate { get; private set; }
        public UInt32 Target { get; private set; }
        public Int32 Opcode { get; private set; }
        public Int32 Function { get; private set; }
        public Int32 Rs { get; private set; }
        public Int32 Rt { get; private set; }
        public Int32 Rd { get; private set; }
        public Int32 ShiftAmount { get; private set; }
        public Int64 Address { get; private set; }
        public Int32 Format { get; private set; }
        public Int32 Ft { get; private set; }
        public Int32 Fs { get; private set; }
        public Int32 Fd { get; private set; }

        public MipsInstruction(Int64 address, UInt32 instruction)
        {
            Instruction = instruction;
            Address = address;
            UInt16 l = (UInt16)instruction;
            UInt16 h = (UInt16)(instruction >> 16);
            Immediate = l;
            Target = Instruction & 0x3FFFFFF;
            Opcode = h >> 10;
            Rs = h >> 5 & 0x1F;
            Rt = h & 0x1F;
            Rd = l >> 11;
            ShiftAmount = h >> 6 & 0x1F;
            Function = l & 0x3F;
            Ft = Rt;
            Fs = Rd;
            Fd = ShiftAmount;
            Format = Rs;
        }

        public DataFormat DecodeDataFormat()
        {
            switch (Format)
            {
                case 16: return DataFormat.Single;
                case 17: return DataFormat.Double;
                case 20: return DataFormat.Word;
                case 21: return DataFormat.Doubleword;
                default: return DataFormat.Reserved;
            }
        }

        public override string ToString()
        {
            try
            {
                if (Instruction == 0)
                    return "nop";

                String op = Disassembler.DecodeOpName(this);

                if (op != null)
                {
                    return $"{op} {Disassembler.DecodeOperands(this, true)}";
                }
                else
                {
                    return "";
                }
            }
            catch (IndexOutOfRangeException)
            {
                return "IndexOutOfRangeException Thrown";
            }
            catch (TypeInitializationException e)
            {
                /* Duplicate key in some dictionary based table */
                return e.Message;
            }
        }
    }
}