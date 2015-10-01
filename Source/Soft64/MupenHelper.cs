using NLog;
using Soft64.MipsR4300;
using Soft64.RCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soft64
{
    public static class MupenHelper
    {
        private static MipsR4300Core m_Core;
        private static InterruptQueue m_Q = new InterruptQueue();
        private static Boolean m_SpecialDone;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


        public static void SetMipsCore(MipsR4300Core core)
        {
            m_Core = core;

            LastPc = 0xa4000040;
            NextInterrupt = 624999;
            InitInterrupt();
        }

        /// <summary>
        /// Mupen's way of updating the Count timer register, it is a hack
        /// </summary>
        public static void UpdateCount()
        {
            m_Core.State.CP0Regs.Count =
                (UInt32)((m_Core.State.PC - LastPc) >> 2) *
                CountPerOp;

            LastPc = m_Core.State.PC;
        }

        public static void ExceptionGeneral()
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

        private static Node AllocNode(Pool pool)
        {
            if (pool.Index >= PoolCapacity)
                return null;

            return pool.Stack[pool.Index++];
        }

        private static void FreeNode(Pool pool, Node node)
        {
            if (pool.Index == 0 || node == null)
                return;

            pool.Stack[--pool.Index] = node;
        }

        private static void ClearPool(Pool pool)
        {
            for (Int32 i = 0; i < PoolCapacity; ++i)
                pool.Stack[i] = pool.Nodes[i];

            pool.Index = 0;
        }

        private static void ClearQueue()
        {
            m_Q.First = null;
            ClearPool(m_Q.Pool);
        }

        public static Boolean BeforeEvent(UInt32 evt1, UInt32 evt2, Int32 type2)
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

        public static void AddInterruptEvent(Int32 type, UInt32 delay)
        {
            AddInterruptEventCount(type, CP0_COUNT_REG + delay);
        }

        public static UInt32 GetEvent(Int32 type)
        {
            Node e = m_Q.First;

            if (e == null)
                return 0;

            if (e.Data.Type == type)
                return e.Data.Count;

            for (; e.Next != null && e.Next.Data.Type != type; e = e.Next) ;

            return (e.Next != null) ? e.Next.Data.Count : 0;
        }

        public static void RemoveEvent(Int32 type)
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

        public static void AddInterruptEventCount(Int32 type, UInt32 count)
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

        public static void RemoveInterruptEvent()
        {
            Node e;

            e = m_Q.First;
            m_Q.First = e.Next;
            FreeNode(m_Q.Pool, e);

            NextInterrupt = 
                ((m_Q.First != null) && ((m_Q.First.Data.Count > CP0_COUNT_REG) || ((CP0_COUNT_REG - m_Q.First.Data.Count) < 0x80000000))) ?
                m_Q.First.Data.Count :
                0;
        }

        public static void TranslateEventQueue(UInt32 b)
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

        public static void InitInterrupt()
        {
            m_SpecialDone = true;

            /* TODO: VI Delay to 5000 */

            ClearQueue();
            //AddInterruptEventCount(VI_INT, /* next VI */)
            AddInterruptEventCount(SPECIAL_INT, 0);
        }

        public static void CheckInterrupt()
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

        public static Int32 GetNextEventType()
        {
            return (m_Q.First == null) ? 0 : m_Q.First.Data.Type;
        }

        public static void RaiseMaskableInterrupt(UInt32 cause)
        {
            CP0_CAUSE_REG = (CP0_CAUSE_REG | cause) & 0xFFFFFF83;

            if ((CP0_STATUS_REG & CP0_CAUSE_REG & 0xFF00) <= 0)
                return;

            if ((CP0_STATUS_REG & 7) != 1)
                return;

            ExceptionGeneral();
        }

        public static void SpecialIntHandler()
        {
            if (CP0_COUNT_REG > 0x10000000)
                return;

            m_SpecialDone = true;
            RemoveInterruptEvent();
            AddInterruptEventCount(SPECIAL_INT, 0);
        }

        public static void CompareIntInterrupt()
        {
            RemoveInterruptEvent();
            CP0_COUNT_REG += CountPerOp;
            AddInterruptEventCount(COMPARE_INT, CP0_COMPARE_REG);
            CP0_COUNT_REG -= CountPerOp;
            RaiseMaskableInterrupt(0x8000);
        }

        public static void Hw2IntHandler()
        {
            RemoveInterruptEvent();
            CP0_STATUS_REG = (CP0_STATUS_REG & ~0x00380000UL) | 0x1000;
            CP0_CAUSE_REG = (CP0_CAUSE_REG | 0x10000) & 0xFFFFFF83UL;
            ExceptionGeneral();
        }

        public static void NmiIntHandler()
        {
            RemoveInterruptEvent();
            CP0_STATUS_REG = (CP0_STATUS_REG & ~0x00380000UL) | 0x00500004;
            CP0_CAUSE_REG = 0;

            /* R4300 Software Reset */
            SoftBootManager.SetupExecutionState(BootMode.HLE_IPL);

            CP0_COUNT_REG = 0;
            /* TODO: VI Counter = 0 */
            InitInterrupt();

            /* TODO: AI Status reg */
            CP0_ERROREPC_REG = (UInt64)m_Core.State.PC;

            if (m_Core.State.BranchEnabled)
            {
                CP0_ERROREPC_REG -= 4;
            }

            m_Core.State.BranchEnabled = false;
            LastPc = 0xA4000040;
            m_Core.State.PC = 0xA40000040;
        }

        public static void GenInterrupt()
        {
            if (SkipJump > 0)
            {
                Int64 dest = SkipJump;
                SkipJump = 0;

                NextInterrupt =
                    (m_Q.First.Data.Count > CP0_CAUSE_REG) || ((CP0_COUNT_REG - m_Q.First.Data.Count) < 0x80000000)
                    ? m_Q.First.Data.Count : 0;

                LastPc = dest;
                m_Core.State.PC = dest;
                return;
            }

            logger.Debug($"Mupen Gen Int: {m_Q.First.Data.Type.ToString("X3")}");

            switch (m_Q.First.Data.Type)
            {
                case SPECIAL_INT:
                    SpecialIntHandler();
                    break;

                case VI_INT:
                    RemoveInterruptEvent();
                    /* TODO: VI Vertuical interrupt event */
                    break;

                case COMPARE_INT:
                    CompareIntInterrupt();
                    break;

                case CHECK_INT:
                    RemoveInterruptEvent();
                    ExceptionGeneral();
                    break;

                case SI_INT:
                    RemoveInterruptEvent();
                    /* TODO: SI End DMA event */
                    break;

                case PI_INT:
                    RemoveInterruptEvent();
                    /* TODO: PI end of DMA event */
                    break;

                case AI_INT:
                    RemoveInterruptEvent();
                    /* TODO: AI End of DMA event */
                    break;

                case SP_INT:
                    RemoveInterruptEvent();
                    /* TODO: RSP interrupt event */
                    break;

                case DP_INT:
                    RemoveInterruptEvent();
                    /* TODO: RDP Interrupt event */
                    break;

                case HW2_INT:
                    NmiIntHandler();
                    break;

                case NMI_INT:
                    NmiIntHandler();
                    break;


                default:
                    {
                        logger.Debug("Mupen Msg: Unknown interrupt in event queue");
                        RemoveInterruptEvent();
                        ExceptionGeneral();
                        break;
                    }

            }
        }

        public static Int64 LastPc
        {
            get;
            set;
        }

        public static UInt32 NextInterrupt
        {
            get;
            set;
        }

        public static Int64 SkipJump
        {
            get;
            set;
        }

        public static UInt32 CountPerOp { get; set; } = 2;

        private static UInt32 CP0_COUNT_REG
        {
            get { return m_Core.State.CP0Regs.Count; }
            set { m_Core.State.CP0Regs.Count = value; }
        }

        private static UInt32 CP0_COMPARE_REG => m_Core.State.CP0Regs.Compare;

        private static UInt64 CP0_CAUSE_REG
        {
            get { return m_Core.State.CP0Regs.Cause; }
            set { m_Core.State.CP0Regs.Cause = value; }
        }

        private static UInt64 CP0_STATUS_REG
        {
            get { return m_Core.State.CP0Regs.Status; }
            set { m_Core.State.CP0Regs.Status = value; }
        }

        private static UInt64 CP0_ERROREPC_REG
        {
            get { return m_Core.State.CP0Regs.ErrorEPC; }
            set { m_Core.State.CP0Regs.ErrorEPC = value; }
        }

        private static UInt64 MI_INTR_REG => Machine.Current.DeviceRCP.Interface_MIPS.Interrupts;
        private static UInt64 MI_INTR_MASK_REG => Machine.Current.DeviceRCP.Interface_MIPS.InterruptMask;

        private class InterruptEvent
        {
            public Int32 Type { get; set; }
            public UInt32 Count { get; set; }
        }

        private class Node
        {
            public InterruptEvent Data { get; set; } = new InterruptEvent();
            public Node Next { get; set; } = null;
        }

        private class Pool
        {
            public Node[] Nodes { get; set; } = new Node[PoolCapacity];
            public Node[] Stack { get; set; } = new Node[PoolCapacity];
            public Int32 Index { get; set; }

            public Pool()
            {
                for (Int32 i = 0; i < Nodes.Length; i++)
                    Nodes[i] = new Node();
            }
        }

        private const Int32 PoolCapacity = 16;

        private class InterruptQueue
        {
            public Pool Pool { get; set; } = new Pool();
            public Node First { get; set; } = null;
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

        public const UInt32 MI_INTR_DP = 32;


        public static Boolean MipsInterface_UpdateInitMode(MipsInterface mi, UInt32 write)
        {
            Boolean clearDp = false;

            /* Set Init Length */
            mi.Mode &= ~0x7FU;
            mi.Mode |= write & 0x7F;

            /* Clear / Set Init Mode */
            if ((write & 0x80U) != 0) mi.Mode &= ~0x80U;
            if ((write & 0x100U) != 0) mi.Mode |= 0x80;

            /* Clear / Set EBus Test Mode */
            if ((write & 0x200) != 0) mi.Mode &= ~0x100U;
            if ((write & 0x400) != 0) mi.Mode |= 0x100;

            /* Clear DP Intrrupt */
            if ((write & 0x800) != 0) clearDp = true;

            /* Clear / Set RDRAM Reg Mode */
            if ((write & 0x1000) != 0) mi.Mode &= ~0x200U;
            if ((write & 0x2000) != 0) mi.Mode |= 0x200;

            return clearDp;
        }

        public static void MipsInterface_UpdateInterruptMask(MipsInterface mi, UInt32 write)
        {
            /* Clear / Set SP Mask */
            if ((write & 0x1) != 0) mi.InterruptMask &= ~0x1U;
            if ((write & 0x2) != 0) mi.InterruptMask |= 0x1;

            /* Clear / Set SI Mask */
            if ((write & 0x4) != 0) mi.InterruptMask &= ~0x2U;
            if ((write & 0x8) != 0) mi.InterruptMask |= 0x2;

            /* Clear / Set AI Mask */
            if ((write & 0x10) != 0) mi.InterruptMask &= ~0x4U;
            if ((write & 0x20) != 0) mi.InterruptMask |= 0x4;

            /* Clear / Set VI Mask */
            if ((write & 0x40) != 0) mi.InterruptMask &= ~0x8U;
            if ((write & 0x80) != 0) mi.InterruptMask |= 0x8;

            /* Clear / Set PI Mask */
            if ((write & 0x100) != 0) mi.InterruptMask &= ~0x10U;
            if ((write & 0x200) != 0) mi.InterruptMask |= 0x10;

            /* Clear / Set DP Mask */
            if ((write & 0x400) != 0) mi.InterruptMask &= ~0x20U;
            if ((write & 0x800) != 0) mi.InterruptMask |= 0x20;
        }

        public static void MipsInterface_RaiseRcpException(MipsInterface mi, UInt32 intr)
        {
            mi.Interrupts |= intr;

            if ((mi.Interrupts & mi.InterruptMask) == mi.InterruptMask)
            {
                RaiseMaskableInterrupt(0x400);
            }
        }

        public static void MipsInterface_SignalRcpInterrupt(MipsInterface mi, UInt32 intr)
        {
            mi.Interrupts |= intr;
            CheckInterrupt();
        }

        public static void MipsInterface_ClearRcpInterrupt(MipsInterface mi, UInt32 intr)
        {
            mi.Interrupts &= ~intr;
            CheckInterrupt();
        }

        public static void MipsInterface_RegWrite(MipsInterface mi, Int32 address, UInt32 value, UInt32 mask)
        {
            var reg = (mi_registers)address;

            switch (reg)
            {
                default: break;
                case mi_registers.MI_INIT_MODE_REG:
                    {
                        if (MipsInterface_UpdateInitMode(mi, value & mask))
                        {
                            MipsInterface_ClearRcpInterrupt(mi, MI_INTR_DP);
                        }

                        break;
                    }

                case mi_registers.MI_INTR_MASK_REG:
                    {
                        MipsInterface_UpdateInterruptMask(mi, value & mask);
                        CheckInterrupt();
                        UpdateCount();

                        if (NextInterrupt < CP0_COUNT_REG)
                            GenInterrupt();

                        break;
                    }
            }
        }


        private enum mi_registers : uint
        {
            MI_INIT_MODE_REG,
            MI_VERSION_REG,
            MI_INTR_REG,
            MI_INTR_MASK_REG,
            MI_REGS_COUNT
        };
    }
}
