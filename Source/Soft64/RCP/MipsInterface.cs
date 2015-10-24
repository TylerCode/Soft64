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

namespace Soft64.RCP
{
    enum MiInterrupt
    {
        Sp = 0x01,
        Si = 0x02,
        Ai = 0x04,
        Vi = 0x08,
        Pi = 0x10,
        Dp = 0x20
    };

    public sealed class MipsInterface : MmioStream
    {
        private const Int32 OFFSET_MI_MODE_REG = 0;
        private const Int32 OFFSET_MI_VERSION_REG = 4;
        private const Int32 OFFSET_MI_INTR_REG = 8;
        private const Int32 OFFSET_MI_INTR_MASK_REG = 0xC;

        public MipsInterface() : base(0xFFFFF)
        {
            IoWrite += MipsInterface_IoWrite;
        }

        private void MipsInterface_IoWrite(object sender, MmioWriteEventArgs e)
        {
            switch (e.Length)
            {
                default: break;
                case 1: MupenHelper.MipsInterface_RegWrite(this, e.Offset, ReadByte(e.Offset), 0xFF); break;
                case 2: MupenHelper.MipsInterface_RegWrite(this, e.Offset, ReadUInt16(e.Offset), 0xFFFF); break;
                case 4: MupenHelper.MipsInterface_RegWrite(this, e.Offset, ReadUInt32(e.Offset), 0xFFFFFFFF); break;
            }
        }

        public UInt32 Mode
        {
            get { return ReadUInt32(OFFSET_MI_MODE_REG); }
            set { Write(OFFSET_MI_MODE_REG, value); }
        }

        public UInt32 Version
        {
            get { return ReadUInt32(OFFSET_MI_VERSION_REG); }
            set { Write(OFFSET_MI_VERSION_REG, value); }
        }

        public UInt32 Interrupts
        {
            get { return ReadUInt32(OFFSET_MI_INTR_REG); }
            set { Write(OFFSET_MI_INTR_REG, value); }
        }

        public UInt32 InterruptMask
        {
            get { return ReadUInt32(OFFSET_MI_INTR_MASK_REG); }
            set { Write(OFFSET_MI_INTR_MASK_REG, value); }
        }
    }
}
