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
using System.Collections.Generic;
using System.IO;
using NLog;
using Soft64.IO;

namespace Soft64.RCP
{
    /// <summary>
    /// Simulates the RCP A/D Bus stream
    /// </summary>
    public sealed class SysADBusStream : UnifiedStream
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private Dictionary<Int64, String> m_NamedMountPoints =
            new Dictionary<long, string>();

        public override void Add(long key, Stream value)
        {
            String name = value.GetType().Name;

            m_NamedMountPoints.Add(key, name);

            logger.Debug(String.Format("Stream Mount: {0} -> {1}", key.ToString("X8"), name));

            base.Add(key, value);
        }

        public override long Length
        {
            get { return 0x100000000; }
        }
    }
}