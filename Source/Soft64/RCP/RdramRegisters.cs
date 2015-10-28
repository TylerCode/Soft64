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

        //RDRAM_BASE_REG - 0x03F00000

        //0x03F0 0000 to 0x03F0 0003  RDRAM_CONFIG_REG or
        //                            RDRAM_DEVICE_TYPE_REG
        //0x03F0 0004 to 0x03F0 0007  RDRAM_DEVICE_ID_REG
        //0x03F0 0008 to 0x03F0 000B  RDRAM_DELAY_REG
        //0x03F0 000C to 0x03F0 000F  RDRAM_MODE_REG
        //0x03F0 0010 to 0x03F0 0013  RDRAM_REF_INTERVAL_REG
        //0x03F0 0014 to 0x03F0 0017  RDRAM_REF_ROW_REG
        //0x03F0 0018 to 0x03F0 001B  RDRAM_RAS_INTERVAL_REG
        //0x03F0 001C to 0x03F0 001F  RDRAM_MIN_INTERVAL_REG
        //0x03F0 0020 to 0x03F0 0023  RDRAM_ADDR_SELECT_REG
        //0x03F0 0024 to 0x03F0 0027  RDRAM_DEVICE_MANUF_REG
        //0x03F0 0028 to 0x03FF FFFF  Unknown

namespace Soft64.RCP
{
    public sealed class RdramRegisters : MmioStream
    {
        private const Int32 OFFSET_RDRAM_CONFIG_REG = 0;
        private const Int32 OFFSET_RDRAM_DEVICE_ID_REG = 4;
        private const Int32 OFFSET_RDRAM_DELAY_REG = 8;
        private const Int32 OFFSET_RDRAM_MODE_REG = 0xC;
        private const Int32 OFFSET_RDRAM_REF_INTERVAL_REG = 0x10;
        private const Int32 OFFSET_RDRAM_REF_ROW_REG = 0x14;
        private const Int32 OFFSET_RDRAM_RAS_INTERVAL_REG = 0x18;
        private const Int32 OFFSET_RDRAM_MIN_INTERVAL_REG = 0x1C;
        private const Int32 OFFSET_RDRAM_ADDR_SELECT_REG = 0x20;
        private const Int32 OFFSET_RDRAM_DEVICE_MANUF_REG = 0x24;

        public RdramRegisters()
            : base(0xFFFFF)
        {
            IoWrite += RdramRegisters_IoWrite;
        }

        private void RdramRegisters_IoWrite(object sender, MmioWriteEventArgs e)
        {

        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return base.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            base.Write(buffer, offset, count);
        }

        public override string ToString()
        {
            return "Rdram Interface";
        }
    }
}
