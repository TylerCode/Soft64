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
    public sealed class PIRegisters : MmioStream
    {
        private const Int32 OFFSET_PI_DRAM_ADDR_REG = 0;
        private const Int32 OFFSET_PI_CART_ADDR_REG = 4;
        private const Int32 OFFSET_PI_RD_LEN_REG = 8;
        private const Int32 OFFSET_PI_WR_LEN_REG = 0xC;
        private const Int32 OFFSET_PI_STATUS_REG = 0x10;
        private const Int32 OFFSET_PI_BSD_DOM1_LAT_REG = 0x14;
        private const Int32 OFFSET_PI_BSD_DOM1_PWD_REG = 0x18;
        private const Int32 OFFSET_PI_BSD_DOM1_PGS_REG = 0x1C;
        private const Int32 OFFSET_PI_BSD_DOM1_RLS_REG = 0x20;
        private const Int32 OFFSET_PI_BSD_DOM2_LAT_REG = 0x24;
        private const Int32 OFFSET_PI_BSD_DOM2_PWD_REG = 0x28;
        private const Int32 OFFSET_PI_BSD_DOM2_PGS_REG = 0x2C;
        private const Int32 OFFSET_PI_BSD_DOM2_RLS_REG = 0x30;

        public PIRegisters() : base(0xFFFFF)
        {
            IoWrite += PIRegisters_IoWrite;
        }

        private void PIRegisters_IoWrite(object sender, MmioWriteEventArgs e)
        {
            
        }

        public UInt32 DramAddress
        {
            get { return ReadUInt32(OFFSET_PI_DRAM_ADDR_REG); }
            set { Write(OFFSET_PI_DRAM_ADDR_REG, value); }
        }

        public UInt32 CartridgeAddress
        {
            get { return ReadUInt32(OFFSET_PI_CART_ADDR_REG); }
            set { Write(OFFSET_PI_CART_ADDR_REG, value); }
        }

        public UInt32 ReadLength
        {
            get { return ReadUInt32(OFFSET_PI_RD_LEN_REG); }
            set { Write(OFFSET_PI_RD_LEN_REG, value); }
        }

        public UInt32 WriteLength
        {
            get { return ReadUInt32(OFFSET_PI_WR_LEN_REG); }
            set { Write(OFFSET_PI_WR_LEN_REG, value); }
        }

        public UInt32 Status
        {
            get { return ReadUInt32(OFFSET_PI_STATUS_REG); }
            set { Write(OFFSET_PI_STATUS_REG, value); }
        }

        public UInt32 Domain1Latency
        {
            get { return ReadUInt32(OFFSET_PI_BSD_DOM1_LAT_REG); }
            set { Write(OFFSET_PI_BSD_DOM1_LAT_REG, value); }
        }

        public UInt32 Domain1PulseWidth
        {
            get { return ReadUInt32(OFFSET_PI_BSD_DOM1_PWD_REG); }
            set { Write(OFFSET_PI_BSD_DOM1_PWD_REG, value); }
        }

        public UInt32 Domain1PageSize
        {
            get { return ReadUInt32(OFFSET_PI_BSD_DOM1_PGS_REG); }
            set { Write(OFFSET_PI_BSD_DOM1_PGS_REG, value); }
        }

        public UInt32 Domain1Release
        {
            get { return ReadUInt32(OFFSET_PI_BSD_DOM1_RLS_REG); }
            set { Write(OFFSET_PI_BSD_DOM1_RLS_REG, value); }
        }

        public UInt32 Domain2Latency
        {
            get { return ReadUInt32(OFFSET_PI_BSD_DOM2_LAT_REG); }
            set { Write(OFFSET_PI_BSD_DOM2_LAT_REG, value); }
        }

        public UInt32 Domain2PulseWidth
        {
            get { return ReadUInt32(OFFSET_PI_BSD_DOM2_PWD_REG); }
            set { Write(OFFSET_PI_BSD_DOM2_PWD_REG, value); }
        }

        public UInt32 Domain2PageSize
        {
            get { return ReadUInt32(OFFSET_PI_BSD_DOM2_PGS_REG); }
            set { Write(OFFSET_PI_BSD_DOM2_PGS_REG, value); }
        }

        public UInt32 Domain2Release
        {
            get { return ReadUInt32(OFFSET_PI_BSD_DOM2_RLS_REG); }
            set { Write(OFFSET_PI_BSD_DOM2_RLS_REG, value); }
        }
    }
}
