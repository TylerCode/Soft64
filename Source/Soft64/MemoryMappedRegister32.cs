﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft64
{
    public abstract class MemoryMappedRegister32 : SmartRegister<UInt32>
    {
        private Int32 m_Offset;
        private RegistersMemorySection m_Section;

        protected MemoryMappedRegister32(RegistersMemorySection section, Int32 offset) : base()
        {
            m_Section = section;
            m_Offset = offset;
        }

        private IntPtr GetReadPointer()
        {
            return m_Section.GetPointer(false, m_Offset * 4);
        }

        private IntPtr GetWritePointer()
        {
            return m_Section.GetPointer(true, m_Offset * 4);
        }

        protected unsafe override uint ReadRegister()
        {
            return *(UInt32*)GetReadPointer();
        }

        protected unsafe override void WriteRegister(uint value)
        {
            *(UInt32*)GetWritePointer() = value;
        }

        internal unsafe UInt32 Data
        {
            get { return *(UInt32*)GetWritePointer(); }
            set { *(UInt32*)GetReadPointer() = value; }
        }
    }
}
