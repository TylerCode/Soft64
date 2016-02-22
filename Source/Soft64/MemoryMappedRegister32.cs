using System;
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
        private Boolean m_Shared; /* All memory is shared between slave/master */
        private Int32 m_SharedStop = -1;

        protected MemoryMappedRegister32(RegistersMemorySection section, Int32 offset, Boolean shared, Int32 sharedStop) : this(section, offset, shared)
        {
            m_SharedStop = sharedStop;
        }

        protected MemoryMappedRegister32(RegistersMemorySection section, Int32 offset, Boolean shared) : this(section, offset)
        {
            m_Shared = shared;
        }

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

        protected unsafe override uint ReadRegister(Int32 offset)
        {
            if (!m_Shared && (m_SharedStop < 0 && offset >= m_SharedStop))
                return *(UInt32*)GetReadPointer();
            else
                return *(UInt32*)GetWritePointer();
        }

        protected unsafe override void WriteRegister(uint value, Int32 offset)
        {
            if (!m_Shared && (m_SharedStop < 0 && offset >= m_SharedStop))
                *(UInt32*)GetWritePointer() = value;
            else
            {
                *(UInt32*)GetReadPointer() = value;
                *(UInt32*)GetWritePointer() = value;
            }
            }

        /// <summary>
        /// Access data that is written to the master that owns the register
        /// </summary>
        public unsafe UInt32 DataToMaster
        {
            get { return *(UInt32*)GetWritePointer(); }
            set { *(UInt32*)GetWritePointer() = value; }
        }

        /// <summary>
        /// Access data that is shared between master and slave
        /// Read: Read the default data from the read buffer
        /// Write: Write the data to both read and write buffer
        /// </summary>
        public unsafe UInt32 DataShared
        {
            get { return DataToSlave; }
            set
            {
                DataToSlave = value;
                DataToMaster = value;
            }
        }

        /// <summary>
        /// Access data that is written to the slave that doesn't own the register
        /// </summary>
        public unsafe UInt32 DataToSlave
        {
            get { return *(UInt32*)GetReadPointer(); }
            set { *(UInt32*)GetReadPointer() = value; }
        }
    }
}
