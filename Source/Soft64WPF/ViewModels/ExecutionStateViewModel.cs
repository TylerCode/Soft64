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
            SetValue(GPRegistersPK, AllocateReg64Array(32));
            SetValue(FPRegistersPK, AllocateRegFloatArray(32));
            SetValue(RegisterPCPK, new RegisterValue());
            SetValue(RegisterHiPK, new RegisterValue());
            SetValue(RegisterLoPK, new RegisterValue());
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
        }

        private static DependencyPropertyKey RegisterReg64PK(String name)
        {
            return RegDPKey<ExecutionStateViewModel, RegisterValue>(name);
        }

        private static DependencyPropertyKey RegisterReg64ArrayPK(String name)
        {
            return RegDPKey<ExecutionStateViewModel, RegisterValue[]>(name);
        }

        private static DependencyPropertyKey RegisterRegFloatArrayPK(String name)
        {
            return RegDPKey<ExecutionStateViewModel, RegisterFloatValue[]>(name);
        }

        private static RegisterValue[] AllocateReg64Array(Int32 count)
        {
            RegisterValue[] v = new RegisterValue[count];
            for (Int32 i = 0; i < count; i++) v[i] = new RegisterValue();
            return v;
        }

        private static RegisterFloatValue[] AllocateRegFloatArray(Int32 count)
        {
            RegisterFloatValue[] v = new RegisterFloatValue[count];
            for (Int32 i = 0; i < count; i++) v[i] = new RegisterFloatValue();
            return v;
        }

        #region DP - RegisterPC
        private static readonly DependencyPropertyKey RegisterPCPK = RegisterReg64PK("RegisterPC");
        public static readonly DependencyProperty RegisterPCProperty = RegisterPCPK.DependencyProperty;
        public RegisterValue RegisterPC => GetValue(RegisterPCProperty);
        #endregion

        #region DP - RegisterHi
        private static readonly DependencyPropertyKey RegisterHiPK = RegisterReg64PK("RegisterHi");
        public static readonly DependencyProperty RegisterHiProperty = RegisterHiPK.DependencyProperty;
        public RegisterValue RegisterHi => GetValue(RegisterHiProperty);
        #endregion

        #region DP - RegisterLo
        private static readonly DependencyPropertyKey RegisterLoPK = RegisterReg64PK("RegisterLo");
        public static readonly DependencyProperty RegisterLoProperty = RegisterLoPK.DependencyProperty;
        public RegisterValue RegisterLo => GetValue(RegisterLoProperty);
        #endregion

        #region DP - GPRegisters
        private static readonly DependencyPropertyKey GPRegistersPK = RegisterReg64ArrayPK("GPRegisters");
        public static readonly DependencyProperty GPRegistersProperty = GPRegistersPK.DependencyProperty;
        public RegisterValue[] GPRegisters => GetValue(GPRegistersProperty);
        #endregion

        #region DP - FPRegisters
        private static readonly DependencyPropertyKey FPRegistersPK = RegisterRegFloatArrayPK("FPRegisters");
        public static readonly DependencyProperty FPRegistersProperty = FPRegistersPK.DependencyProperty;
        public RegisterFloatValue[] FPRegisters => GetValue(FPRegistersProperty);
        #endregion

        public ExecutionState CPUState => MachineViewModel.CurrentModel.CurrentMachine.DeviceCPU.State;
    }
}
