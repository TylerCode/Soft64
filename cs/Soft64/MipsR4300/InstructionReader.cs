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
using Soft64.IO;
using Soft64.MipsR4300.Debugging;

namespace Soft64.MipsR4300
{
    public enum MemoryAccessMode
    {
        /// <summary>
        /// Read only physical memory
        /// </summary>
        Physical,

        /// <summary>
        /// Read using with full blown MMU support
        /// </summary>
        Virtual,

        /// <summary>
        /// Used for debugging purposes
        /// </summary>
        DebugVirtual,

        /// <summary>
        /// Read virtual memory without cache support
        /// </summary>
        VirtualWithoutCache
    }

    public class InstructionReader : IDisposable
    {
        private Stream m_Source;
        private Boolean m_ModeInvalid;
        private BinaryReader m_BinReader;
        private Int64 m_Position;
        private Boolean m_Disposed;

        public InstructionReader(MemoryAccessMode accessMode)
        {
            switch (accessMode)
            {
                case MemoryAccessMode.Physical:
                    m_Source = Machine.Current.Memory; break;

                case MemoryAccessMode.DebugVirtual:
                    m_Source = new VMemViewStream(); break;

                case MemoryAccessMode.Virtual:
                    m_Source = Machine.Current.DeviceCPU.VirtualMemoryStream; break;

                default: m_ModeInvalid = true; break;
            }

            if (m_Source != null)
                m_BinReader = new BinaryReader(new Int32SwapStream(m_Source));
        }

        public Int64 Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        public MipsInstruction ReadInstruction()
        {
            if (m_ModeInvalid)
                throw new InvalidOperationException("Cannot read instruction with unsupported memory mode");

            if (m_Disposed)
                throw new ObjectDisposedException(this.ToString());

            m_Source.Position = Position;
            UInt32 read = m_BinReader.ReadUInt32();

            MipsInstruction inst = new MipsInstruction(Position, read);
            Position += 4;
            return inst;
        }

        private void Dispose(Boolean disposing)
        {
            if (!m_Disposed)
            {
                if (disposing)
                {
                    m_BinReader.Dispose();
                    m_Source = null;
                }

                m_Disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
