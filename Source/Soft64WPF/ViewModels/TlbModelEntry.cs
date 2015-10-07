using Soft64.MipsR4300;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft64WPF.ViewModels
{
    public sealed class TlbModelEntry
    {
        private Int32 m_EntryIndex;
        private TLBEntry m_AssociatedEntry;

        public TlbModelEntry(Int32 index, TLBEntry entry)
        {
            m_EntryIndex = index;
            m_AssociatedEntry = entry;
        }

        public TLBEntry AssociatedEntry
        {
            get { return m_AssociatedEntry; }
        }

        public Int32 EntryIndex
        {
            get { return m_EntryIndex; }
        }
    }
}
