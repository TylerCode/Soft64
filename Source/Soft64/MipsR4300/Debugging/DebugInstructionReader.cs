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
