using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Soft64.MipsR4300;

namespace Soft64WPF.ViewModels
{
    public class ExecutionStateViewModel : QuickDependencyObject
    {
        internal ExecutionStateViewModel()
        {
            SetValue(Cop0RegistersPK, new Cop0RegistersViewModel());
            SetValue(GPRegistersPK, AllocateReg64Array(32));
            SetValue(FPRegistersPK, AllocateRegFloatArray(32));
            SetValue(RegisterPCPK, new Register64Value());
            SetValue(RegisterHiPK, new Register64Value());
            SetValue(RegisterLoPK, new Register64Value());
        }

        public void Load()
        {
            RegisterPC.RegValue = (UInt64)CPUState.PC;
            RegisterHi.RegValue = CPUState.Hi;
            RegisterLo.RegValue = CPUState.Lo;

            for (Int32 i = 0; i < 32; i++)
            {
                GPRegisters[i].RegValue = CPUState.GPRRegs[i];
                FPRegisters[i].RegValue = CPUState.Fpr.ReadFPRDouble(i);
            }

            BranchPC = CPUState.BranchPC;
            BranchDelaySlot = CPUState.BranchDelaySlot;
            BranchEnabled = CPUState.BranchEnabled;
            NullifiedEnabled = CPUState.NullifyEnabled;
            LLBit = CPUState.LLBit;

            Cop0Registers.Load();
        }

        public void Store()
        {
            CPUState.PC = (Int64)RegisterPC.RegValue;
            CPUState.Hi = RegisterHi.RegValue;
            CPUState.Lo = RegisterLo.RegValue;

            for (Int32 i = 0; i < 32; i++)
            {
                CPUState.GPRRegs[i] = GPRegisters[i].RegValue;
                CPUState.Fpr.WriteFPRDouble(i, FPRegisters[i].RegValue);
            }

            CPUState.BranchPC = BranchPC;
            CPUState.BranchDelaySlot = BranchDelaySlot;
            CPUState.BranchEnabled = BranchEnabled;
            CPUState.NullifyEnabled = NullifiedEnabled;
            CPUState.LLBit = LLBit;

            Cop0Registers.Store();
        }

        private static DependencyPropertyKey RegisterReg64PK(String name)
        {
            return RegDPKey<ExecutionStateViewModel, Register64Value>(name);
        }

        private static DependencyPropertyKey RegisterReg64ArrayPK(String name)
        {
            return RegDPKey<ExecutionStateViewModel, Register64Value[]>(name);
        }

        private static DependencyPropertyKey RegisterRegFloatArrayPK(String name)
        {
            return RegDPKey<ExecutionStateViewModel, RegisterFPUValue[]>(name);
        }

        private static Register64Value[] AllocateReg64Array(Int32 count)
        {
            Register64Value[] v = new Register64Value[count];
            for (Int32 i = 0; i < count; i++) v[i] = new Register64Value();
            return v;
        }

        private static RegisterFPUValue[] AllocateRegFloatArray(Int32 count)
        {
            RegisterFPUValue[] v = new RegisterFPUValue[count];
            for (Int32 i = 0; i < count; i++) v[i] = new RegisterFPUValue();
            return v;
        }

        #region DP - RegisterPC
        private static readonly DependencyPropertyKey RegisterPCPK = RegisterReg64PK("RegisterPC");
        public static readonly DependencyProperty RegisterPCProperty = RegisterPCPK.DependencyProperty;
        public Register64Value RegisterPC => GetValue(RegisterPCProperty);
        #endregion

        #region DP - RegisterHi
        private static readonly DependencyPropertyKey RegisterHiPK = RegisterReg64PK("RegisterHi");
        public static readonly DependencyProperty RegisterHiProperty = RegisterHiPK.DependencyProperty;
        public Register64Value RegisterHi => GetValue(RegisterHiProperty);
        #endregion

        #region DP - RegisterLo
        private static readonly DependencyPropertyKey RegisterLoPK = RegisterReg64PK("RegisterLo");
        public static readonly DependencyProperty RegisterLoProperty = RegisterLoPK.DependencyProperty;
        public Register64Value RegisterLo => GetValue(RegisterLoProperty);
        #endregion

        #region DP - GPRegisters
        private static readonly DependencyPropertyKey GPRegistersPK = RegisterReg64ArrayPK("GPRegisters");
        public static readonly DependencyProperty GPRegistersProperty = GPRegistersPK.DependencyProperty;
        public Register64Value[] GPRegisters => GetValue(GPRegistersProperty);
        #endregion

        #region DP - FPRegisters
        private static readonly DependencyPropertyKey FPRegistersPK = RegisterRegFloatArrayPK("FPRegisters");
        public static readonly DependencyProperty FPRegistersProperty = FPRegistersPK.DependencyProperty;
        public RegisterFPUValue[] FPRegisters => GetValue(FPRegistersProperty);
        #endregion

        #region DP - BranchPC
        public static readonly DependencyProperty BranchPCProperty = RegDP<ExecutionStateViewModel, Int64>("BranchPC");
        public Int64 BranchPC { get { return GetValue(BranchPCProperty); } set { SetValue(BranchPCProperty, value); } }
        #endregion

        #region DP - BranchDelaySlot
        public static readonly DependencyProperty BranchDelaySlotProperty = RegDP<ExecutionStateViewModel, Int64>("BranchDelaySlot");
        public Int64 BranchDelaySlot { get { return GetValue(BranchDelaySlotProperty); } set { SetValue(BranchDelaySlotProperty, value); } }
        #endregion

        #region DP - BranchEnabled
        public static readonly DependencyProperty BranchEnabledProperty = RegDP<ExecutionStateViewModel, Boolean>("BranchEnabled");
        public Boolean BranchEnabled { get { return GetValue(BranchEnabledProperty); } set { SetValue(BranchEnabledProperty, value); } }
        #endregion

        #region DP - NullifiedEnabled
        public static readonly DependencyProperty NullifiedEnabledProperty = RegDP<ExecutionStateViewModel, Boolean>("NullifiedEnabled");
        public Boolean NullifiedEnabled { get { return GetValue(NullifiedEnabledProperty); } set { SetValue(NullifiedEnabledProperty, value); } }
        #endregion

        #region DP - LLBit
        public static readonly DependencyProperty LLBitProperty = RegDP<ExecutionStateViewModel, Boolean>("LLBit");
        public Boolean LLBit { get { return GetValue(LLBitProperty); } set { SetValue(LLBitProperty, value); } }
        #endregion

        #region DP - Cop0 Registers
        private static readonly DependencyPropertyKey Cop0RegistersPK = RegDPKey<ExecutionStateViewModel, Cop0RegistersViewModel>("Cop0Registers");
        public static readonly DependencyProperty Cop0RegistersProperty = Cop0RegistersPK.DependencyProperty;
        public Cop0RegistersViewModel Cop0Registers => GetValue(Cop0RegistersProperty);
        #endregion

        public ExecutionState CPUState => MachineViewModel.CurrentModel.CurrentMachine?.DeviceCPU.State;
    }
}
