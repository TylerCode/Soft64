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
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Soft64.MipsR4300.CP1
{
    public sealed class FpuRegisters : IDisposable
    {
        private GCHandle m_RegHandle;
        private UInt64[] m_Regs;
        private IntPtr m_RegsPtr;

        public FpuRegisters()
        {
            m_Regs = new ulong[32];
            m_RegHandle = GCHandle.Alloc(m_Regs, GCHandleType.Pinned);
            m_RegsPtr = m_RegHandle.AddrOfPinnedObject();
            /* TODO: Free handle on dispose */
        }

        public void Clear()
        {
            Array.Clear(m_Regs, 0, m_Regs.Length);
        }

        public UInt64 ReadFPRUnsigned(Int32 index)
        {
            return (UInt64)m_Regs[index];
        }

        public void WriteFPRUnsigned(Int32 index, UInt64 value)
        {
            m_Regs[index] = value;
        }

        public UInt32 ReadFPR32Unsigned(Int32 index)
        {
            return (UInt32)m_Regs[index];
        }

        public void WriteFPR32Unsigned(Int32 index, UInt32 value)
        {
            m_Regs[index] = value;
        }

        public Double ReadFPRDouble(Int32 index)
        {
            unsafe
            {
                UInt64* ptr = (UInt64*)m_RegsPtr;
                return *(Double*)(ptr + index);
            }
        }

        public void WriteFPRDouble(Int32 index, Double value)
        {
            unsafe
            {
                UInt64* ptr = (UInt64*)m_RegsPtr;
                *(Double*)(ptr + index) = value;
            }
        }

        public Single ReadFPRSingle(Int32 index)
        {
            unsafe
            {
                UInt64* ptr = (UInt64*)m_RegsPtr;
                return *(Single*)(ptr + index);
            }
        }

        public void WriteFPRSingle(Int32 index, Single value)
        {
            unsafe
            {
                UInt64* ptr = (UInt64*)m_RegsPtr;
                *(Single*)(ptr + index) = value;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
