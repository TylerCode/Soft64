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

namespace Soft64.MipsR4300
{
    public enum InterruptKind : int
    {
        None = 0x000,
        VideoInterface = 0x001,
        Compare = 0x002,
        Check = 0x004,
        SerialInterface = 0x008,
        PherpherialInterface = 0x010,
        Special = 0x020,
        AudioInterface = 0x040,
        SignalProcessor = 0x080,
        DisplayProcessor = 0x100,
        Hardware2 = 0x200,
        NonMaskable = 0x400
    }

    internal struct _InterruptEvent
    {
        public UInt64 Count;
        public InterruptKind Kind;
    }

    public class InterruptScheduler
    {
        private Queue<_InterruptEvent> m_InterruptQueue;
        private Boolean m_SpecialFinished = false;
        private ExecutionState m_MipsState;
        private UInt64 m_NextInterrupt;

        public InterruptScheduler()
        {
            m_InterruptQueue = new Queue<_InterruptEvent>();
        }

        //public void AddInterruptEvent(InterruptKind kind, UInt64 count)
        //{
        //    if (m_MipsState.CP0Regs.Count > 0x80000000)
        //        m_SpecialFinished = false;

        //    /* TODO: Need hack-fix here ? */

        //    _InterruptEvent iEvent = new _InterruptEvent();
        //    iEvent.Count = count;
        //    iEvent.Kind = kind;

        //    if (m_InterruptQueue.Count < 1)
        //    {
        //        /* First event in the queue */
        //        m_InterruptQueue.Enqueue(iEvent);
        //        m_NextInterrupt = count;
        //    }
        //    //else if ()
        //}

        //private Boolean IsBeforeEvent(UInt64 eventCount, _InterruptEvent newEvent)
        //{
        //    if ((eventCount - m_MipsState.CP0Regs.Count) < 0x80000000)
        //    {

        //    }
        //}
    }
}
