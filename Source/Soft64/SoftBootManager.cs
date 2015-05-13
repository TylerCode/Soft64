﻿/*
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
using System.IO;
using NLog;
using Soft64.MipsR4300;
using Soft64.RCP;

namespace Soft64
{
    public enum BootMode
    {
        MIPS_ELF,
        HLE_IPL,
        IPL_ROM,
    }

    public static class BootModeExtensions
    {
        public static String GetFriendlyName(this BootMode bootMode)
        {
            switch (bootMode)
            {
                case BootMode.MIPS_ELF: return "MIPS Executable File";
                case BootMode.HLE_IPL: return "High-Level PIF Bootstrap";
                case BootMode.IPL_ROM: return "Real PIF Bootstrap";
                default: return "Unknown Bootmode!";
            }
        }
    }

    public static class SoftBootManager
    {
        private static Stream s_ElfStream;
        private static String s_ElfName;
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
                if (Machine.Current.RCP.DevicePI.InsertedCartridge != null)
                {
                    SimulatePIF();
                }
                else
                {
                    logger.Warn("No cartridge inserted, skipping IPL boot");
                }

                Machine.Current.CPU.State.PC = 0xA4000040;

                logger.Debug("ROM Entry Point: " + Machine.Current.CPU.State.PC.ToString("X8"));
            }
            else if (bootMode == BootMode.MIPS_ELF)
            {
                ELFExecutable executable = new ELFExecutable(s_ElfName, s_ElfStream);
                logger.Debug("ELF Entry Point: " + executable.EntryPointAddress.ToString("X16"));


                ELFLoader.LoadElf(executable);
                logger.Trace("ELF loaded");
            }
            else
            {
                throw new ArgumentException("Unknown bootmode");
            }
        }

        private static void SimulatePIF()
        {
            /* This simulates the effects of the PIF Bootloader */

            /* Copy the cartridge bootstrap into SP Memory */
            logger.Debug("PIF HLE: Copying cartridge bootrom into DMEM + 0x40");
            Machine.Current.RCP.SafeN64Memory.Position = N64MemRegions.SPDMem.ToRegionAddress() + 0x40;
            Machine.Current.RCP.DevicePI.InsertedCartridge.RomImage.BootRomInformation.CopyCode(Machine.Current.RCP.SafeN64Memory);

            using (Stream stream = typeof(SoftBootManager).Assembly.GetManifestResourceStream("Soft64.BootStateSnapshots.xml"))
            {
                CartridgeInfo info = Cartridge.Current.GetCartridgeInfo();
                CICKeyType cic = Cartridge.Current.RomImage.BootRomInformation.CIC;
                BootSnapReader bootsnap = new BootSnapReader(stream);
                bootsnap.LoadBootSnap(cic, info.RegionCode);
            }
        }

        public static void SetElfExecutable(FileStream inStream)
        {
            s_ElfStream = inStream;
            s_ElfName = inStream.Name;
        }
    }
}