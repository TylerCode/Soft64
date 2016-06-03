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
using Soft64.MipsR4300.CP1;

namespace Soft64.MipsR4300
{
    public sealed class FPUEntity
    {
        private DataFormat m_Tag;
        private UInt64 m_Long = 0;
        private UInt32 m_Word = 0;
        private Single m_Single = 0.0F;
        private Double m_Double = 0.0d;
        private Boolean m_Wide;
        private FpuRegisters m_FPR;
        private Func<FPUEntity, dynamic> m_DynamicGet;
        private Action<FPUEntity, dynamic> m_DynamicSet;
        private IntPtr m_BufferPtr;

        public FPUEntity(DataFormat format, ExecutionState state)
        {
            m_BufferPtr = Marshal.AllocHGlobal(8);

            m_Tag = format;
            m_Wide = state.CP0Regs.StatusReg.AdditionalFPR;
            m_FPR = state.Fpr;

            switch (m_Tag)
            {
                case DataFormat.Double:
                    {
                        m_DynamicGet = (e) => { return e.m_Double; };
                        m_DynamicSet = (e, v) => { e.m_Double = v; };
                        break;
                    }
                case DataFormat.Doubleword:
                    {
                        m_DynamicGet = (e) => { return e.m_Long; };
                        m_DynamicSet = (e, v) => { e.m_Long = v; };
                        break;
                    }
                case DataFormat.Single:
                    {
                        m_DynamicGet = (e) => { return e.m_Single; };
                        m_DynamicSet = (e, v) => { e.m_Single = v; };
                        break;
                    }
                case DataFormat.Word:
                    {
                        m_DynamicGet = (e) => { return e.m_Word; };
                        m_DynamicSet = (e, v) => { e.m_Word = v; };
                        break;
                    }
                default:
                case DataFormat.Reserved: throw new InvalidOperationException("Cannot return a value with reserved data format");
            }
        }

        ~FPUEntity()
        {
            Marshal.FreeHGlobal(m_BufferPtr);
        }

        private unsafe UInt32 _FIXED(Single single)
        {
            Single* ptr = (Single*)m_BufferPtr;
            *ptr = single;
            UInt32 value = *(UInt32*)ptr;
            return value;
        }

        private UInt32 _FIXED(UInt32 word)
        {
            return word;
        }

        private unsafe UInt64 _FIXED(Double d)
        {
            Double* ptr = (Double*)m_BufferPtr;
            *ptr = d;
            UInt64 value = *(UInt64*)ptr;
            return value;
        }

        private UInt64 _FIXED(UInt64 dword)
        {
            return dword;
        }

        private unsafe Single _FLOAT(UInt32 word)
        {
            UInt32* ptr = (UInt32*)m_BufferPtr;
            *ptr = word;
            Single value = *(Single *)ptr;
            return value;
        }

        private unsafe Double _FLOAT(UInt64 dword)
        {
            UInt64* ptr = (UInt64*)m_BufferPtr;
            *ptr = dword;
            Double value = *(Double*)ptr;
            return value;
        }

        public void Load(Int32 index)
        {
            if (m_Tag == DataFormat.Reserved)
                throw new InvalidOperationException("Cannot operate on reserved data formats");

            if (m_Wide)
            {
                switch (m_Tag)
                {
                    case DataFormat.Word: m_Word = m_FPR.ReadFPR32Unsigned(index); break;
                    case DataFormat.Single: m_Single = m_FPR.ReadFPRSingle(index); break;
                    case DataFormat.Doubleword: m_Long = m_FPR.ReadFPRUnsigned(index); break;
                    case DataFormat.Double: m_Double = m_FPR.ReadFPRDouble(index); break;
                    default: break;
                }
            }
            else
            {
                switch (m_Tag)
                {
                    case DataFormat.Word: m_Word = m_FPR.ReadFPR32Unsigned(index); break;
                    case DataFormat.Single: m_Single = m_FPR.ReadFPRSingle(index); break;
                    case DataFormat.Doubleword:
                        {
                            m_Long = m_FPR.ReadFPR32Unsigned(index + 1);
                            m_Long <<= 32;
                            m_Long |= m_FPR.ReadFPR32Unsigned(index);
                            break;
                        }
                    case DataFormat.Double:
                        {
                            UInt64 l = m_FPR.ReadFPR32Unsigned(index + 1);
                            l <<= 32;
                            l |= m_FPR.ReadFPR32Unsigned(index);
                            m_Double = _FLOAT(l);
                            break;
                        }
                    default: break;
                }
            }
        }

        public void Store(Int32 index)
        {
            if (m_Tag == DataFormat.Reserved)
                throw new InvalidOperationException("Cannot operate on reserved data formats");

            if (m_Wide)
            {
                if (m_Tag == DataFormat.Word || m_Tag == DataFormat.Single)
                {
                    m_FPR.WriteFPR32Unsigned(index, _FIXED(Value));
                }
                
                if (m_Tag == DataFormat.Double || m_Tag == DataFormat.Doubleword)
                {
                    m_FPR.WriteFPRUnsigned(index, _FIXED(Value));
                }
            }
            else
            {
                if (m_Tag == DataFormat.Word || m_Tag == DataFormat.Single)
                {
                    m_FPR.WriteFPR32Unsigned(index + 1, 0);
                    m_FPR.WriteFPR32Unsigned(index, _FIXED(Value));
                }

                if (m_Tag == DataFormat.Double || m_Tag == DataFormat.Doubleword)
                {
                    UInt64 value = _FIXED(Value);
                    m_FPR.WriteFPRUnsigned(index + 1, value >> 32);
                    m_FPR.WriteFPRUnsigned(index, value & 0xFFFFFFFF);
                }
            }
        }

        public dynamic Value
        {
            get { return m_DynamicGet(this); }
            set { m_DynamicSet(this, value); }
        }

        public Boolean IsNaN
        {
            get
            {
                if (m_Tag == DataFormat.Double)
                    return Double.IsNaN(m_Double);
                else if (m_Tag == DataFormat.Single)
                    return Single.IsNaN(m_Single);
                else
                    return false;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if ((obj as FPUEntity) == null)
                return false;

            return this == (FPUEntity)obj;
        }

        public static dynamic operator +(FPUEntity a, FPUEntity b)
        {
            return a.Value + b.Value;
        }

        public static Boolean operator <(FPUEntity a, FPUEntity b)
        {
            return a.Value < b.Value;
        }

        public static Boolean operator >(FPUEntity a, FPUEntity b)
        {
            return a.Value > b.Value;
        }

        public static Boolean operator ==(FPUEntity a, FPUEntity b)
        {
            return a.Value == b.Value;
        }

        public static Boolean operator !=(FPUEntity a, FPUEntity b)
        {
            return a.Value != b.Value;
        }

        public static dynamic operator /(FPUEntity a, FPUEntity b)
        {
            return a.Value / b.Value;
        }

        public static dynamic operator *(FPUEntity a, FPUEntity b)
        {
            return a.Value * b.Value;
        }

        public static dynamic operator -(FPUEntity a)
        {
            return -a.Value;
        }

        public static dynamic operator -(FPUEntity a, FPUEntity b)
        {
            return a.Value - b.Value;
        }
    }

    public enum FPURoundMode : int
    {
        Chop = 0x00000300,
        Up = 0x00000200,
        Down = 0x00000100,
        Near = 0x00000000
    }
}
