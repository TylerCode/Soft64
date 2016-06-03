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
using System.Text;
using System.Threading.Tasks;
using Soft64.MipsR4300;
using Soft64.MipsR4300.CP1;

namespace Soft64.MipsR4300
{
    public class MipsSnapshot
    {
        private Int64 m_PC;
        private Boolean m_Mode;
        private GPRRegisters m_GPR;
        private CP0Registers m_CP0;
        private FpuRegisters m_CP1;
        private UInt64 m_Hi;
        private UInt64 m_Lo;
        private Boolean m_LLbit;
        private UInt32 m_FCR0;
        private UInt32 m_FCR31;
        private String m_Name;

        public MipsSnapshot(String name)
        {
            m_GPR = new GPRRegisters();
            m_CP0 = new CP0Registers();
            m_CP1 = new FpuRegisters();
            m_Name = name + " Snapshot";
        }

        public Int64 PC
        {
            get { return m_PC; }
            set { m_PC = value; }
        }

        public Boolean WordMode
        {
            get { return m_Mode; }
            set { m_Mode = value; }
        }

        public GPRRegisters GPR
        {
            get
            {
                return m_GPR;
            }
        }

        public CP0Registers CP0
        {
            get
            {
                return m_CP0;
            }
        }

        public FpuRegisters FPU
        {
            get
            {
                return m_CP1;
            }
        }

        public UInt64 Hi
        {
            get { return m_Hi; }
            set { m_Hi = value; }
        }

        public UInt64 Lo
        {
            get { return m_Lo; }
            set { m_Lo = value; }
        }

        public Boolean LLbit
        {
            get { return m_LLbit; }
            set { m_LLbit = value; }
        }

        public UInt32 FCR0
        {
            get { return m_FCR0; }
            set { m_FCR0 = value; }
        }

        public UInt32 FCR31
        {
            get { return m_FCR31; }
            set { m_FCR31 = value; }
        }

        public String Name
        {
            get { return m_Name; }
        }
    }
}
