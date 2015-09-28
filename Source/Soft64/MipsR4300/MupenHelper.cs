using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft64.MipsR4300
{
    public sealed class MupenHelper
    {
        private MipsR4300Core m_Core;
        private InterruptQueue m_Q;
        private Boolean m_SpecialDone;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public MupenHelper(MipsR4300Core core)
        {
            m_Core = core;
        }

        /// <summary>
        /// Mupen's way of updating the Count timer register, it is a hack
        /// </summary>
        public void UpdateCount()
        {
            m_Core.State.CP0Regs.Count =
                (UInt32)((m_Core.State.PC - LastPc) >> 2) *
                CountPerOp;

            LastPc = m_Core.State.PC;
        }

        public void ExceptionGeneral()
        {
            m_Core.State.CP0Regs.Status |= 2;
            m_Core.State.CP0Regs.EPC = (UInt64)m_Core.State.PC;

            if (m_Core.State.BranchEnabled)
            {
                /* Delay Slot Exception */
                m_Core.State.CP0Regs.Cause |= 0x80000000UL;
                m_Core.State.CP0Regs.EPC -= 4;
            }
            else
            {
                m_Core.State.CP0Regs.Cause &= 0x7FFFFFFFUL;
            }

            m_Core.State.PC = 0x80000180L;
            LastPc = m_Core.State.PC;

            if (m_Core.State.BranchEnabled)
            {
                SkipJump = m_Core.State.PC;
                NextInterrupt = 0;
            }

        }

        private Node AllocNode(Pool pool)
        {
            if (pool.Index >= PoolCapacity)
                return null;

            return pool.Stack[pool.Index++];
        }

        private void FreeNode(Pool pool, Node node)
        {
            if (pool.Index == 0 || node == null)
                return;

            pool.Stack[--pool.Index] = node;
        }

        private void ClearPool(Pool pool)
        {
            for (Int32 i = 0; i < PoolCapacity; ++i)
                pool.Stack[i] = pool.Nodes[i];

            pool.Index = 0;
        }

        private void ClearQueue()
        {
            m_Q.First = null;
            ClearPool(m_Q.Pool);
        }

        public Boolean BeforeEvent(UInt32 evt1, UInt32 evt2, Int32 type2)
        {
            if (evt1 - CP0_COUNT_REG < 0x80000000)
            {
                if (evt2 - CP0_COUNT_REG < 0x80000000)
                {
                    if ((evt1 - CP0_COUNT_REG) < (evt2 - CP0_COUNT_REG))
                        return true;
                    else
                        return false;
                }
                else
                {
                    if ((CP0_COUNT_REG - evt2) < 0x10000000)
                    {
                        switch (type2)
                        {
                            case SPECIAL_INT:
                                {
                                    return m_SpecialDone;
                                }
                            default: return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        public void AddInterruptEvent(Int32 type, UInt32 delay)
        {
            AddInterruptEventCount(type, CP0_COUNT_REG + delay);
        }

        public UInt32 GetEvent(Int32 type)
        {
            Node e = m_Q.First;

            if (e == null)
                return 0;

            if (e.Data.Type == type)
                return e.Data.Count;

            for (; e.Next != null && e.Next.Data.Type != type; e = e.Next) ;

            return (e.Next != null) ? e.Next.Data.Count : 0;
        }

        public void RemoveEvent(Int32 type)
        {
            Node to_del;
            Node e = m_Q.First;

            if (e.Data.Type == type)
            {
                m_Q.First = e.Next;
                FreeNode(m_Q.Pool, e);
            }
            else
            {
                for (; e.Next != null && e.Next.Data.Type != type; e = e.Next) ;

                if (e.Next != null)
                {
                    to_del = e.Next;
                    e.Next = to_del.Next;
                    FreeNode(m_Q.Pool, to_del);
                }
            }
        }

        public void AddInterruptEventCount(Int32 type, UInt32 count)
        {
            Node evt;
            Node e;
            Boolean special = (type == SPECIAL_INT);

            if (CP0_COUNT_REG > 0x80000000)
            {
                m_SpecialDone = false;
            }

            if (GetEvent(type) > 0)
            {
                logger.Debug($"Mupen Warning: two events of type {type.ToString("X3")} in the interrpt queue");
                return;
            }

            evt = AllocNode(m_Q.Pool);

            if (evt == null)
            {
                throw new InvalidOperationException("Failed to allocate node for new interrupt event");
            }

            evt.Data.Count = count;
            evt.Data.Type = type;

            if (m_Q.First == null)
            {
                m_Q.First = evt;
                evt.Next = null;
                NextInterrupt = m_Q.First.Data.Count;
            }
            else if (BeforeEvent(count, m_Q.First.Data.Count, m_Q.First.Data.Type) && !special)
            {
                evt.Next = m_Q.First;
                m_Q.First = evt;
                NextInterrupt = m_Q.First.Data.Count;
            }
            else
            {
                for (e = m_Q.First;
                    e.Next != null && (!BeforeEvent(count, e.Next.Data.Count, e.Next.Data.Type) || special);
                    e = e.Next) ;

                if (e.Next == null)
                {
                    e.Next = evt;
                    evt.Next = null;
                }
                else
                {
                    if (!special)
                        for (; e.Next != null && e.Next.Data.Count == count; e = e.Next) ;

                    evt.Next = e.Next;
                    e.Next = evt;
                }
            }

        }

        public void RemoveInterruptEvent()
        {
            Node e;

            e = m_Q.First;
            m_Q.First = e.Next;
            FreeNode(m_Q.Pool, e);

            NextInterrupt = (
                (m_Q.First != null) &&
                (m_Q.First.Data.Count > CP0_COUNT_REG) ||
                ((CP0_COUNT_REG - m_Q.First.Data.Count) < 0x80000000)) ?
                m_Q.First.Data.Count :
                0;
        }

        public void TranslateEventQueue(UInt32 b)
        {
            Node e;

            RemoveEvent(COMPARE_INT);
            RemoveEvent(SPECIAL_INT);

            for (e = m_Q.First; e != null; e = e.Next)
            {
                e.Data.Count = (e.Data.Count - CP0_COUNT_REG) + b;
            }

            AddInterruptEventCount(COMPARE_INT, CP0_COMPARE_REG);
            AddInterruptEventCount(SPECIAL_INT, 0);
        }

        public void InitInterrupt()
        {
            m_SpecialDone = true;

            /* TODO: VI Delay to 5000 */

            ClearQueue();
            //AddInterruptEventCount(VI_INT, /* next VI */)
            AddInterruptEventCount(SPECIAL_INT, 0);
        }

        public void CheckInterrupt()
        {
            Node evt;

            RcpProcessor rcp = Machine.Current.DeviceRCP;

            if ((MI_INTR_REG & MI_INTR_MASK_REG) != 0)
            {
                CP0_CAUSE_REG = (CP0_CAUSE_REG | 0x400);
            }
            else
            {
                CP0_CAUSE_REG &= ~0x400UL;
            }

            if ((CP0_STATUS_REG & 7) != 1)
                return;

            if ((CP0_STATUS_REG & CP0_CAUSE_REG & 0xFF00) != 0)
            {
                evt = AllocNode(m_Q.Pool);

                if (evt == null)
                {
                    throw new InvalidOperationException("Mupen Error: Failed to allocate node for new interrupt");
                }

                evt.Data.Count = CP0_COUNT_REG;
                NextInterrupt = CP0_COUNT_REG;
                evt.Data.Type = CHECK_INT;

                if (m_Q.First == null)
                {
                    m_Q.First = evt;
                    evt.Next = null;
                }
                else
                {
                    evt.Next = m_Q.First;
                    m_Q.First = evt;
                }
            }
        }

        public Int32 GetNextEventType()
        {
            return (m_Q.First == null) ? 0 : m_Q.First.Data.Type;
        }

        public Int64 LastPc
        {
            get;
            set;
        }

        public UInt32 NextInterrupt
        {
            get;
            set;
        }

        public Int64 SkipJump
        {
            get;
            set;
        }

        public UInt32 CountPerOp { get; set; } = 2;

        private UInt32 CP0_COUNT_REG => m_Core.State.CP0Regs.Count;
        private UInt32 CP0_COMPARE_REG => m_Core.State.CP0Regs.Compare;

        private UInt64 CP0_CAUSE_REG
        {
            get { return m_Core.State.CP0Regs.Cause; }
            set { m_Core.State.CP0Regs.Cause = value; }
        }

        private UInt64 CP0_STATUS_REG
        {
            get { return m_Core.State.CP0Regs.Status; }
            set { m_Core.State.CP0Regs.Status = value; }
        }
        private UInt64 MI_INTR_REG => Machine.Current.DeviceRCP.MMIO_MI.Interrupts;
        private UInt64 MI_INTR_MASK_REG => Machine.Current.DeviceRCP.MMIO_MI.InterruptMask;

        private class InterruptEvent
        {
            public Int32 Type { get; set; }
            public UInt32 Count { get; set; }
        }

        private class Node
        {
            public InterruptEvent Data { get; set; }
            public Node Next { get; set; }
        }

        private class Pool
        {
            public Node[] Nodes { get; set; } = new Node[PoolCapacity];
            public Node[] Stack { get; set; } = new Node[PoolCapacity];
            public Int32 Index { get; set; }
        }

        private const Int32 PoolCapacity = 16;

        private class InterruptQueue
        {
            public Pool Pool { get; set; }
            public Node First { get; set; }
        }

        public const Int32 VI_INT = 0x001;
        public const Int32 COMPARE_INT = 0x002;
        public const Int32 CHECK_INT = 0x004;
        public const Int32 SI_INT = 0x008;
        public const Int32 PI_INT = 0x010;
        public const Int32 SPECIAL_INT = 0x020;
        public const Int32 AI_INT = 0x040;
        public const Int32 SP_INT = 0x080;
        public const Int32 DP_INT = 0x100;
        public const Int32 HW2_INT = 0x200;
        public const Int32 NMI_INT = 0x400;
    }
}
