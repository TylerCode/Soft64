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

using System.IO;

namespace Soft64.RCP
{
    // TODO: Track dirty writes so the CPU can invalidate its instruction cache

    public sealed class N64MemoryStream : Stream
    {
        private Stream m_RcpBusStream;

        internal N64MemoryStream(Stream rcpBusStream)
        {
            m_RcpBusStream = rcpBusStream;
        }

        public override bool CanRead
        {
            get { return m_RcpBusStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return m_RcpBusStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return m_RcpBusStream.CanWrite; }
        }

        public override void Flush()
        {
            m_RcpBusStream.Flush();
        }

        public override long Length
        {
            get { return m_RcpBusStream.Length; }
        }

        public override long Position
        {
            get
            {
                return m_RcpBusStream.Position;
            }
            set
            {
                m_RcpBusStream.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return m_RcpBusStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return m_RcpBusStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            m_RcpBusStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            m_RcpBusStream.Write(buffer, offset, count);
        }
    }
}