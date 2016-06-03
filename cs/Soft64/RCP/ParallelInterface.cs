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
using System.IO;
using NLog;

namespace Soft64.RCP
{
    public sealed class ParallelInterface
    {
        private Cartridge m_CurrentCartridge;
        private DiskDrive m_CurrentDiskDrive;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public event EventHandler<CartridgeChangedEventArgs> CartridgeChanged;

        public void MountCartridge(Cartridge cartridge)
        {
            if (cartridge == null)
                throw new ArgumentNullException("cartridge");

            if (m_CurrentCartridge != null)
                ReleaseCartridge();

            m_CurrentCartridge = cartridge;
            m_CurrentCartridge.Open();

            OnCartridgeChanged(cartridge);

            logger.Trace("A catridge has been inserted into the slot");
            logger.Trace("Cartridge Rom Header ==========================");
            logger.Trace("Name:         " + m_CurrentCartridge.RomImage.Name);
            logger.Trace("Serial:       " + m_CurrentCartridge.RomImage.Serial.ToString());
            logger.Trace("Computed CIC: " + m_CurrentCartridge.RomImage.BootRomInformation.CIC.GetGoodName());
            logger.Trace("Linked CIC:   " + ((m_CurrentCartridge.LockoutKey != null) ? m_CurrentCartridge.LockoutKey.ToString() : "None"));
            logger.Trace("CRC1:         " + m_CurrentCartridge.RomImage.CRC1.ToString("X8"));
            logger.Trace("CRC2:         " + m_CurrentCartridge.RomImage.CRC2.ToString("X8"));
            logger.Trace("Region:       " + m_CurrentCartridge.RomImage.Region.ToString());
            logger.Trace("VI Rate:      " + m_CurrentCartridge.RomImage.GetVIRate().ToString());
        }

        public void ReleaseCartridge()
        {
            if (m_CurrentCartridge != null)
            {
                m_CurrentCartridge = null;
                OnCartridgeChanged(null);
            }
        }

        public void MountDiskDrive(DiskDrive drive)
        {
            logger.Warn("This method is more of a stub than something functional");

            m_CurrentDiskDrive = drive;
            logger.Debug("A disk drive has been inserted into the slot");
        }

        public void ReleaseDiskDrive()
        {
            logger.Warn("This method is more of a stub than something functional");
            logger.Debug("Releasing current disk drive");

            m_CurrentDiskDrive = null;
        }

        private void OnCartridgeChanged(Cartridge cartridge)
        {
            var e = CartridgeChanged;

            if (e != null)
            {
                e(this, new CartridgeChangedEventArgs(cartridge));
            }
        }

        public override string ToString()
        {
            return "Parellel Interface";
        }

        public Cartridge InsertedCartridge => m_CurrentCartridge;

        public DiskDrive InsertedDiskDrive => m_CurrentDiskDrive;
    }
}