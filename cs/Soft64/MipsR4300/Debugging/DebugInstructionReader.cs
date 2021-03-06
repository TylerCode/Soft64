﻿/*
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft64.MipsR4300.Debugging
{
    public sealed class DebugInstructionReader : InstructionReader
    {
        public DebugInstructionReader() : base(MemoryAccessMode.DebugVirtual)
        {

        }

        public DisassembledInstruction ReadDisasm(Boolean abiDecode)
        {
            MipsInstruction inst = ReadInstruction();
            DisassemblyString disasmStr = Disassembler.Disassemble(inst);

            return new DisassembledInstruction
                (
                  address: inst.Address,
                  disasm: $"{disasmStr.Opcode} {disasmStr.Operands}",
                  instruction: inst
                );
        }
    }
}
