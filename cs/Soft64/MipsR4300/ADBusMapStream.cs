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

namespace Soft64.MipsR4300
{
    internal sealed class ADBusMapStream : Stream
    {
        private Int64 m_Position;
        private MipsR4300Core m_Core;

        public ADBusMapStream(MipsR4300Core core)
        {
            m_Core = core;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            m_Core.SysAdBus.Flush();
        }

        public override long Length
        {
            get { return 0x20000000; }
        }

        public override long Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            Int32 read = 0;
            m_Core.SysAdBus.Position = m_Position;
            read = m_Core.SysAdBus.Read(buffer, offset, count);
            return read;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            Int64 pos = 0;
            pos = m_Core.SysAdBus.Seek(offset, origin);
            return pos;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            m_Core.SysAdBus.Position = m_Position;
            m_Core.SysAdBus.Write(buffer, offset, count);
        }
    }
}