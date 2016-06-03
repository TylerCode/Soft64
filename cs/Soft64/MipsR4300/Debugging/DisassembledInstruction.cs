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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft64.MipsR4300.Debugging
{
    public class DisassembledInstruction
    {
        private Int64 m_Address;
        private String m_Disasm;
        private MipsInstruction m_Instruction;
        private String m_FullLine;

        public DisassembledInstruction(Int64 address, String disasm, MipsInstruction instruction)
        {
            m_Address = address;
            m_Disasm = disasm;
            m_Instruction = instruction;

            Int32 hi = (Int32)(instruction.Instruction & 0xFF00) >> 16;
            Int32 lo = (Int32)(instruction.Instruction & 0x00FF);

            m_FullLine = String.Format("{0:X8} {1:X4} {2:X4} {3}",
                (UInt32)m_Address,
                hi,
                lo,
                m_Disasm);
        }

        public Int64 Address { get { return m_Address; } }

        public String Disassembly { get { return m_Disasm; } }

        public MipsInstruction Instruction { get { return m_Instruction; } }

        public String FullDisassembly { get { return m_FullLine; } }
    }
}
