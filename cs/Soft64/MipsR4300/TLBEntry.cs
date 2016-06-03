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

namespace Soft64.MipsR4300
{
    /// <summary>
    /// Represents a TLB entry that is used to translate a virtual address.
    /// </summary>
    public sealed class TLBEntry : IEquatable<TLBEntry>, IEquatable<VirtualPageNumber2>
    {
        private static SortedList<Int32, Int32> s_BitCountCache = new SortedList<Int32, Int32>();

        private VirtualPageNumber2 m_Vpn2; // This key part of the entry
        private PageSize m_PageSize; // This simply stores the mask values used in TLB read operations
        private PageFrameNumber m_EntryOdd; // Odd physical page entry
        private PageFrameNumber m_EntryEven; // Even physical page entry
        private Boolean m_AddressingMode;

        public TLBEntry(Boolean addressMode, Byte asid, Int64 virtualAddress, PageSize size)
            : this(addressMode)
        {
            /* This is a hack to create entries without using the TLB regs */
            m_Vpn2 = new VirtualPageNumber2(asid, (UInt64)virtualAddress & size.AddressBaseMask);
            m_PageSize = size;
        }

        public TLBEntry(Boolean addressMode = false)
        {
            m_AddressingMode = addressMode;
            m_PageSize = new PageSize(NamedPageSize.Size4KB);
            m_Vpn2 = new VirtualPageNumber2();
            m_EntryEven = new PageFrameNumber();
            m_EntryOdd = new PageFrameNumber();
        }

        /// <summary>
        /// Gets the size of the mapped page based on the mask.
        /// </summary>
        public UInt64 PageMask
        {
            get { return m_PageSize.PageMask; }
            set { m_PageSize = new PageSize(value); }
        }

        public PageSize Size
        {
            get { return m_PageSize; }
            set { m_PageSize = value; }
        }

        /// <summary>
        /// Gets the virtual page number divided by 2.
        /// </summary>
        public VirtualPageNumber2 VPN2
        {
            get { return m_Vpn2; }
            set { m_Vpn2 = value; }
        }

        /// <summary>
        /// Gets whether this TLB entry instance can ignore the ASID.
        /// </summary>
        public Boolean IsGlobal
        {
            get { return m_EntryOdd.IsGlobal && m_EntryEven.IsGlobal; }
        }

        /// <summary>
        /// Gets the even physical frame number.
        /// </summary>
        public PageFrameNumber PfnEven
        {
            get { return m_EntryEven; }
            set { m_EntryEven = value; }
        }

        /// <summary>
        /// Gets the odd physical frame number.
        /// </summary>
        public PageFrameNumber PfnOdd
        {
            get { return m_EntryOdd; }
            set { m_EntryOdd = value; }
        }

        public bool Equals(VirtualPageNumber2 other)
        {
            return
                IsGlobal ? true : VPN2.Asid == other.Asid &&
                VPN2.Vpn2 == other.Vpn2;
        }

        public bool Equals(TLBEntry other)
        {
            if (other == null) return false;
            if (Object.ReferenceEquals(this, other)) return true;

            return Equals(m_Vpn2, other.m_Vpn2);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj as TLBEntry);
        }

        public override int GetHashCode()
        {
            return (Int32)m_Vpn2.Vpn2 ^ m_Vpn2.Asid;
        }

        public static Boolean operator ==(TLBEntry a, TLBEntry b)
        {
            if (Object.ReferenceEquals(a, b))
                return true;

            if ((Object)a == null || (Object)b == null)
                return false;

            return a.Equals(b);
        }

        public static Boolean operator !=(TLBEntry a, TLBEntry b)
        {
            return !(a == b);
        }

        public Int64 MappedVirtualAddress
        {
            get { return (Int64)VPN2.Vpn2; }
        }

        public Int64 MappedN64PhysicalAddressEven
        {
            get
            {
                return 0x80000000 | m_EntryEven.MapPhysical(m_PageSize, 0);
            }
        }

        public Int64 MappedN64PhysicalAddressOdd
        {
            get
            {
                return 0x8000000 | m_EntryOdd.MapPhysical(m_PageSize, 0);
            }
        }
    }
}