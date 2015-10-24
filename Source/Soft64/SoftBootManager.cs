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
using System.IO;
using NLog;
using Soft64.MipsR4300;
using Soft64.RCP;
using Soft64.PI;

namespace Soft64
{
    public enum BootMode : int
    {
        HLE_IPL,
        HLE_IPL_OLD,
        IPL_ROM,
    }

    public static class BootModeExtensions
    {
        public static String GetFriendlyName(this BootMode bootMode)
        {
            switch (bootMode)
            {
                case BootMode.HLE_IPL: return "High-Level PIF Bootstrap";
                case BootMode.IPL_ROM: return "Real PIF Bootstrap";
                default: return "Unknown Bootmode!";
            }
        }
    }

    public static class SoftBootManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void SetupExecutionState(BootMode bootMode)
        {
            if (bootMode == BootMode.IPL_ROM)
            {
                throw new InvalidOperationException("Low-level IPL booting not supported yet");

                //Machine.Current.RCP.State.PC = 0x1FC00000;

                /* TODO:
                 * Schedule CPU and RCP threads to execute MIPS Interpreter
                 * Have CPU sit idle, and RCP execute the PIF Rom
                 * */
            }
            else if (bootMode == BootMode.HLE_IPL)
            {
                if (Machine.Current.DeviceRCP.Interface_Parallel.InsertedCartridge != null)
                {
                    /* Code taken from mupen64plus-core on github */
                    ExecutionState state = Machine.Current.DeviceCPU.State;
                    state.PC = 0xA4000040;

                    /* PI Setup */
                    var pi = Machine.Current.DeviceRCP.Interface_Parallel;
                    PiBusSpeedConfig config = Cartridge.Current.RomImage.BusConfig;
                    pi.Domain1Latency = (UInt32)config.DeviceLatency;
                    pi.Domain1PageSize = (UInt32)config.PageSize;
                    pi.Domain1PulseWidth = (UInt32)config.PulseWidth;
                    pi.Domain1Release = (UInt32)config.ReleaseTime;
                    pi.Status = 0;



                    /* sp_register.sp_status_reg = 1;
                        rsp_register.rsp_pc = 0;

                        ai_register.ai_dram_addr = 0;
                        ai_register.ai_len = 0;

                        vi_register.vi_v_intr = 1023;
                        vi_register.vi_current = 0;
                        vi_register.vi_h_start = 0;

                    /* MI Setup */
                    var mi = Machine.Current.DeviceRCP.Interface_MIPS;
                    mi.Version = 0x02020102;
                    mi.Interrupts &= ~(0x10U | 0x8U | 0x4U | 0x1U);

                    logger.Debug("PIF HLE: Copying cartridge bootrom into DMEM + 0x40");
                    Machine.Current.N64MemorySafe.Position = N64MemRegions.SPDMem.ToRegionAddress() + 0x40;
                    Machine.Current.DeviceRCP.Interface_Parallel.InsertedCartridge.RomImage.BootRomInformation.CopyCode(Machine.Current.N64MemorySafe);

                    state.GPRRegs[19] = 0; /* 0: Cart, 1: DiskDrive */
                    state.GPRRegs[20] = (UInt64)((Int32)Cartridge.Current.RomImage.Region) - 1;
                    state.GPRRegs[21] = 0; /* 0: ColdReset, 1: NMI */
                    state.GPRRegs[22] = (UInt64)Cartridge.Current.RomImage.BootRomInformation.CIC.Seed();
                    state.GPRRegs[23] = 0; /* S7: Unknown */

                    /* Required by CIC X105 */
                    BinaryWriter bw = new BinaryWriter(Machine.Current.N64MemorySafe);
                    bw.BaseStream.Position = 0x04001000;
                    bw.Write(0x3c0dbfc0U);
                    bw.Write(0x8da807fcU);
                    bw.Write(0x25ad07c0U);
                    bw.Write(0x31080080U);
                    bw.Write(0x5500fffcU);
                    bw.Write(0x3c0dbfc0U);
                    bw.Write(0x8da80024U);
                    bw.Write(0x3c0bb000U);

                    /* Required by CIC X105 */
                    state.GPRRegs[11] = 0xffffffffa4000040UL; /* T3 */
                    state.GPRRegs[29] = 0xffffffffa4001ff0UL; /* SP */
                    state.GPRRegs[31] = 0xffffffffa4001550UL; /* RA */
                }
                else
                {
                    logger.Warn("No cartridge inserted, skipping IPL boot");
                    Machine.Current.DeviceCPU.State.PC = 0xBFC00000;
                }

                logger.Debug("ROM Entry Point: " + Machine.Current.DeviceCPU.State.PC.ToString("X8"));
            }
            else if (bootMode == BootMode.HLE_IPL_OLD)
            {
                if (Machine.Current.DeviceRCP.Interface_Parallel.InsertedCartridge != null)
                {
                    /* This is the older method of booting the emulator */

                    Machine.Current.DeviceCPU.State.PC = 0xA4000040;

                    /* This simulates the effects of the PIF Bootloader */
                    /* 1. Copy the game cartrdige boot rom into SP Memory
                     * 2. Load a snapshot of the CPU / Memory state generated by the real PIF boot sequence */

                    logger.Debug("PIF HLE: Copying cartridge bootrom into DMEM + 0x40");
                    Machine.Current.N64MemorySafe.Position = N64MemRegions.SPDMem.ToRegionAddress() + 0x40;
                    Machine.Current.DeviceRCP.Interface_Parallel.InsertedCartridge.RomImage.BootRomInformation.CopyCode(Machine.Current.N64MemorySafe);

                    using (Stream stream = typeof(SoftBootManager).Assembly.GetManifestResourceStream("Soft64.BootStateSnapshots.xml"))
                    {
                        CartridgeInfo info = Cartridge.Current.GetCartridgeInfo();
                        CICKeyType cic = Cartridge.Current.RomImage.BootRomInformation.CIC;
                        BootSnapReader bootsnap = new BootSnapReader(stream);
                        bootsnap.LoadBootSnap(cic, info.RegionCode);
                    }
                }
                else
                {
                    logger.Warn("No cartridge inserted, skipping IPL boot");
                    Machine.Current.DeviceCPU.State.PC = 0xBFC00000;
                }

                logger.Debug("ROM Entry Point: " + Machine.Current.DeviceCPU.State.PC.ToString("X8"));
            }
            else
            {
                throw new ArgumentException("Unknown bootmode");
            }
        }
    }
}