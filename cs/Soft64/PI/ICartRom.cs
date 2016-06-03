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

namespace Soft64.PI
{
    public interface ICartRom
    {
        PiBusSpeedConfig BusConfig { get; }

        String Name { get; }

        Int32 Clockrate { get; }

        Int64 EntryPoint { get; }

        GameSerial Serial { get; }

        Int32 CRC1 { get; }

        Int32 CRC2 { get; }

        Boolean IsHeaderOnly { get; }

        IBootRom BootRomInformation { get; }

        RegionType Region { get; }

        Int32 Release { get; }

        Int32 GetAIDacRate();

        Int32 GetVIRate();

        Stream RomStream { get; }
    }
}