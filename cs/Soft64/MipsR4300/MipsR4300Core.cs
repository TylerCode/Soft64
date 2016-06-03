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
using System.IO;
using Soft64.MipsR4300;
using Soft64.MipsR4300.Debugging;

namespace Soft64.MipsR4300
{
    public class MipsR4300Core
    {
        private ExecutionState m_State;           /* Processor Register State */
        private ExecutionEngine m_ExecEngine;     /* Processor Execution Engine */
        private VMemStream m_MMU;                 /* Processor Memory Management Unit */
        private IOMonitor m_IOMonitor;
        private SychronizedStream m_SyncMMU;
        private Boolean m_DebugMemAccess;
        private Stream m_SysADBus;

        public MipsR4300Core()
        {
            m_State = new ExecutionState();
            m_MMU = new VMemStream(this);
            m_IOMonitor = new IOMonitor();
            m_SyncMMU = new SychronizedStream(m_MMU);
            m_ExecEngine = new Interpreter();
        }

        public virtual void Initialize()
        {
            /* Setup the low-level hardware state */
            SetInitialState();
        }

        public ExecutionState State
        {
            get { return m_State; }
        }

        public ExecutionEngine Engine
        {
            get { return m_ExecEngine; }
            set { m_ExecEngine = value; }
        }

        public TLBCache Tlb
        {
            get { return m_MMU.TLB; }
        }

        public Stream VirtualMemoryStream
        {
            get { return m_SyncMMU; }
        }

        public IOMonitor ResourceMonitor
        {
            get { return m_IOMonitor; }
        }

        private void SetInitialState()
        {
            if (m_ExecEngine == null)
                throw new InvalidOperationException("Execution engine cannot be null");

            /* MIPS Registers */
            m_State.GPRRegs.Clear();
            m_State.LLBit = false;
            m_State.Hi = 0;
            m_State.Lo = 0;

            /* Setup Cop0 Registers */
            m_State.CP0Regs.Cause = 0x5C;
            m_State.CP0Regs.Status = 0x34000000;
            m_State.CP0Regs.Config = 0x0006E463;
            m_State.CP0Regs.PRId = 0xB00;
            m_State.CP0Regs.Count = 0x5000;
            m_State.CP0Regs.Context = 0x7FFFF0;
            m_State.CP0Regs.ErrorEPC = 0xFFFFFFFF;
            m_State.CP0Regs.EPC = 0xFFFFFFFF;

            /* Setup Cop1 Registers */
            m_State.FCR0 = 0x511;
            m_State.FCR31.RegisterValue = 0;
            m_State.Fpr.Clear();
            FPUHardware.SetRoundingMode(m_State.FCR31.RM);

            /* Initialize the execution engine */
            m_ExecEngine.SetParent(this);

            /* Initialize the MMU */
            m_MMU.Initialize();

            /* Initialize the engine */
            m_ExecEngine.Initialize();
        }

        public MipsSnapshot CreateSnapshot()
        {
            MipsSnapshot snapshot = new MipsSnapshot("Soft64");

            snapshot.PC = m_State.PC;
            snapshot.Lo = m_State.Lo;
            snapshot.Hi = m_State.Hi;

            for (Int32 i = 0; i < 32; i++ )
            {
                snapshot.GPR[i] = m_State.GPRRegs[i];
            }

            return snapshot;
        }

        public Boolean DebugMemoryAccess
        {
            get { return m_DebugMemAccess; }
            set { m_DebugMemAccess = value; m_MMU.SetupOperations(value); }
        }

        public Stream SysAdBus { get; set; }
    }
}