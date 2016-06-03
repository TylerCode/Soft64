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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft64
{
    internal sealed class SychronizedStream : Stream
    {
        private Stream m_ThreadUnsafeStream;
        private Object m_StreamLock;

        [ThreadStatic]
        private static Int64 s_Position; /* Each thread will have their own position variable */

        public SychronizedStream(Stream sourceStream)
        {
            m_ThreadUnsafeStream = sourceStream;
            m_StreamLock = new Object();
        }

        public override bool CanRead
        {
            get { return m_ThreadUnsafeStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return m_ThreadUnsafeStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return m_ThreadUnsafeStream.CanWrite; }
        }

        public override void Flush()
        {
            lock (m_StreamLock)
            {
                m_ThreadUnsafeStream.Flush();
            }
        }

        public override long Length
        {
            get { return m_ThreadUnsafeStream.Length; }
        }

        public override long Position
        {
            get
            {
                return s_Position;
            }
            set
            {
                s_Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            lock (m_StreamLock)
            {
                m_ThreadUnsafeStream.Position = s_Position;
                return m_ThreadUnsafeStream.Read(buffer, offset, count);
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return m_ThreadUnsafeStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            lock (m_StreamLock)
            {
                m_ThreadUnsafeStream.SetLength(value);
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            lock (m_StreamLock)
            {
                m_ThreadUnsafeStream.Position = s_Position;
                m_ThreadUnsafeStream.Write(buffer, offset, count);
            }
        }
    }
}
