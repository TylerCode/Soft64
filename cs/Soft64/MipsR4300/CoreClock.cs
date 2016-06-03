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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* MIPS Info
 * Opcode execution = 8 PC cycles
 * 1 PC Cycle = 2 master cycles
 * Opcod execution = 4 master cycles
 * Pipeline Size = 8 stages, so 8 level deep
 */

namespace Soft64.MipsR4300
{
    public class CoreClock
    {
        private Int64 m_CoreCycles;

        public CoreClock()
        {
            m_CoreCycles = 7;
        }

        public void Advance(Int32 cycles)
        {
            m_CoreCycles += cycles;
        }

        public void AdvanceInstruction()
        {
            m_CoreCycles += 1;
        }

        public virtual void BranchDelay()
        {
            m_CoreCycles += 3;
        }

        public virtual void LoadDelay()
        {
            m_CoreCycles += 2;
        }

        public Int32 DelaySlotCycle { get; set; }

        public Int64 Cycles
        {
            get { return m_CoreCycles; }
        }
    }
}
