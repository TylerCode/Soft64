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

/***********************************************************************
*************************  MEMORY MAP OVERVIEW  ***********************
***********************************************************************

 0x0000 0000 to 0x03EF FFFF RDRAM Memory
 0x03F0 0000 to 0x03FF FFFF RDRAM Registers
 0x0400 0000 to 0x040F FFFF SP Registers
 0x0410 0000 to 0x041F FFFF DP Command Registers
 0x0420 0000 to 0x042F FFFF DP Span Registers
 0x0430 0000 to 0x043F FFFF MIPS Interface (MI) Registers
 0x0440 0000 to 0x044F FFFF Video Interface (VI) Registers
 0x0450 0000 to 0x045F FFFF Audio Interface (AI) Registers
 0x0460 0000 to 0x046F FFFF Peripheral Interface (PI) Registers
 0x0470 0000 to 0x047F FFFF RDRAM Interface (RI) Registers
 0x0480 0000 to 0x048F FFFF Serial Interface (SI) Registers
 0x0490 0000 to 0x04FF FFFF Unused
 0x0500 0000 to 0x05FF FFFF Cartridge Domain 2 Address 1
 0x0600 0000 to 0x07FF FFFF Cartridge Domain 1 Address 1
 0x0800 0000 to 0x0FFF FFFF Cartridge Domain 2 Address 2
 0x1000 0000 to 0x1FBF FFFF Cartridge Domain 1 Address 2
 0x1FC0 0000 to 0x1FC0 07BF PIF Boot ROM
 0x1FC0 07C0 to 0x1FC0 07FF PIF RAM
 0x1FC0 0800 to 0x1FCF FFFF Reserved
 0x1FD0 0000 to 0x7FFF FFFF Cartridge Domain 1 Address 3
 0x8000 0000 to 0xFFFF FFFF External SysAD Device
 */

using System;
using System.IO;
using NLog;
using Soft64.RCP;
using Soft64.MipsR4300;

namespace Soft64
{
    public class RcpProcessor : IDisposable
    {
        private Boolean m_Disposed;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /* IO Interfaces */
        private ParallelInterface m_PIInterface;
        private RdramRegisters m_RdramRegs;
        private MipsInterface m_MIInterface;
        private HLEMipsMemoryBus m_MemoryBus;
        private N64MemoryStream m_MemoryStream;
        private RdramStream m_RdRam;
        private SPMemory m_SPMemory;

        /* Processor Devices */
        /* TODO: RSP Processor */
        /* TODO: RDP Processor */

        public RcpProcessor()
        {
            m_MemoryBus = new HLEMipsMemoryBus();
            m_MemoryStream = new N64MemoryStream(m_MemoryBus);
            m_PIInterface = new ParallelInterface(m_MemoryBus);
            m_RdramRegs = new RdramRegisters();
            m_MIInterface = new MipsInterface();
        }

        public void Initialize()
        {
            m_RdRam = new RdramStream();
            m_SPMemory = new SPMemory();

            m_MemoryBus.Add(0x00000000, m_RdRam);
            m_MemoryBus.Add(0x03F00000, m_RdramRegs);
            m_MemoryBus.Add(0x04000000, m_SPMemory);
            m_MemoryBus.Add(0x04300000, m_MIInterface);
            m_MemoryBus.Add(0x04600000, m_PIInterface.ParellelRegisters);
            m_MemoryBus.Add(0x05000000, m_PIInterface.ParallelBusStream);

            if (Machine.Current.DevicePIF.RomStream != null)
                m_MemoryBus.Add(0x1FC00000, Machine.Current.DevicePIF.RomStream);

            m_MemoryBus.Add(0x1FC007C0, Machine.Current.DevicePIF.RamStream);
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }

        private void Dispose(Boolean disposing)
        {
            if (!m_Disposed)
            {
                if (disposing)
                {
                    m_MemoryStream.Dispose();
                    m_MemoryBus.Dispose();
                }

                m_Disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Boolean Disposed
        {
            get { return m_Disposed; }
        }


        /* ///////////////////////////////////
           Reality CoProcessor A/D Bus
           /////////////////////////////////// */
        internal HLEMipsMemoryBus ADBusStream => m_MemoryBus;

        /* //////////////////////////////////
           Reality CoProcessor Interfaces
           ////////////////////////////////// */

        public ParallelInterface Interface_Parallel => m_PIInterface;
        public RdramRegisters Interface_RDRAM => m_RdramRegs;
        public MipsInterface Interface_MIPS => m_MIInterface;
    }
}