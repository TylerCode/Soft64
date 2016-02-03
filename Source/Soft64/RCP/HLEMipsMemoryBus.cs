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
using System.IO;
using NLog;
using Soft64.IO;

namespace Soft64.RCP
{
    /// <summary>
    /// Uses high level approach which used in place of Mips SysAD bus line.
    /// </summary>
    public sealed class HLEMipsMemoryBus : UnifiedStream
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public override void Add(long key, Stream value)
        {
            logger.Debug($"Stream Mounted @{key:X8} : {value.ToString()}");
            base.Add(key, value);
        }

        public override long Length
        {
            get { return 0x100000000; }
        }
    }
}