using Soft64.MipsR4300;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using StatusVM = Soft64WPF.ViewModels.Cop0StatusRegisterViewModel;

namespace Soft64WPF.ViewModels
{
    public sealed class Cop0StatusRegisterViewModel : QuickDependencyObject
    {
        internal Cop0StatusRegisterViewModel()
        {
            SetValue(RegisterPK, new Register32Value());
        }

        public void Load()
        {
            Register.RegValue = Status.RegisterValue;
            SetValue(IsOperating64PK, State.Operating64BitMode);
            SetValue(IsAddressing64PK, State.Addressing64BitMode);
            SetValue(AdditonalFPRPK, Status.AdditionalFPR);
            SetValue(InterruptsEnabledPK, Status.EnableInterrupts);
            SetValue(InterruptMask0PK, Status.InterruptMask0);
            SetValue(InterruptMask1PK, Status.InterruptMask1);
            SetValue(InterruptMask2PK, Status.InterruptMask2);
            SetValue(InterruptMask3PK, Status.InterruptMask3);
            SetValue(InterruptMask4PK, Status.InterruptMask4);
            SetValue(InterruptMask5PK, Status.InterruptMask5);
            SetValue(InterruptMask6PK, Status.InterruptMask6);
            SetValue(InterruptMask7PK, Status.InterruptMask7);
        }

        public void Store()
        {
            Status.RegisterValue = Register.RegValue;
        }

        #region DP - Register
        private static readonly DependencyPropertyKey RegisterPK = RegDPKey<StatusVM, Register32Value>("Register");
        public static readonly DependencyProperty RegisterProperty = RegisterPK.DependencyProperty;
        public Register32Value Register => GetValue(RegisterProperty);
        #endregion

        #region DP - Is Operating 64
        private static readonly DependencyPropertyKey IsOperating64PK = RegDPKey<StatusVM, Boolean>("IsOperating64");
        public static readonly DependencyProperty ComputeModeProperty = IsOperating64PK.DependencyProperty;
        public Boolean IsOperating64 => GetValue(ComputeModeProperty);
        #endregion

        #region DP - Is Addressing 64
        private static readonly DependencyPropertyKey IsAddressing64PK = RegDPKey<StatusVM, Boolean>("IsAddressing64");
        public static readonly DependencyProperty IsAdressing64Property = IsAddressing64PK.DependencyProperty;
        public Boolean IsAddressing64 => GetValue(IsAdressing64Property);
        #endregion

        #region DP - Additonal FPR
        private static readonly DependencyPropertyKey AdditonalFPRPK = RegDPKey<StatusVM, Boolean>("AddtionalFPR");
        public static readonly DependencyProperty AdditionalFPRProperty = AdditonalFPRPK.DependencyProperty;
        public Boolean AdditionalFPR => GetValue(AdditionalFPRProperty);
        #endregion

        #region DP - InterruptsEnabled
        private static readonly DependencyPropertyKey InterruptsEnabledPK = RegDPKey<StatusVM, Boolean>("InterruptsEnabled");
        public static readonly DependencyProperty InterruptsEnabledProperty = InterruptsEnabledPK.DependencyProperty;
        public Boolean InterruptsEnabled => GetValue(InterruptsEnabledProperty);
        #endregion

        #region DP - InterruptMask0
        private static readonly DependencyPropertyKey InterruptMask0PK = RegDPKey<StatusVM, Boolean>("InterruptMask0");
        public static readonly DependencyProperty InterruptMask0Property = InterruptMask0PK.DependencyProperty;
        public Boolean InterruptMask0 => GetValue(InterruptMask0Property);
        #endregion

        #region DP - InterruptMask1
        private static readonly DependencyPropertyKey InterruptMask1PK = RegDPKey<StatusVM, Boolean>("InterruptMask1");
        public static readonly DependencyProperty InterruptMask1Property = InterruptMask1PK.DependencyProperty;
        public Boolean InterruptMask1 => GetValue(InterruptMask1Property);
        #endregion

        #region DP - InterruptMask2
        private static readonly DependencyPropertyKey InterruptMask2PK = RegDPKey<StatusVM, Boolean>("InterruptMask2");
        public static readonly DependencyProperty InterruptMask2Property = InterruptMask2PK.DependencyProperty;
        public Boolean InterruptMask2 => GetValue(InterruptMask2Property);
        #endregion

        #region DP - InterruptMask3
        private static readonly DependencyPropertyKey InterruptMask3PK = RegDPKey<StatusVM, Boolean>("InterruptMask3");
        public static readonly DependencyProperty InterruptMask3Property = InterruptMask3PK.DependencyProperty;
        public Boolean InterruptMask3 => GetValue(InterruptMask3Property);
        #endregion

        #region DP - InterruptMask4
        private static readonly DependencyPropertyKey InterruptMask4PK = RegDPKey<StatusVM, Boolean>("InterruptMask4");
        public static readonly DependencyProperty InterruptMask4Property = InterruptMask4PK.DependencyProperty;
        public Boolean InterruptMask4 => GetValue(InterruptMask4Property);
        #endregion

        #region DP - InterruptMask5
        private static readonly DependencyPropertyKey InterruptMask5PK = RegDPKey<StatusVM, Boolean>("InterruptMask5");
        public static readonly DependencyProperty InterruptMask5Property = InterruptMask5PK.DependencyProperty;
        public Boolean InterruptMask5 => GetValue(InterruptMask5Property);
        #endregion

        #region DP - InterruptMask6
        private static readonly DependencyPropertyKey InterruptMask6PK = RegDPKey<StatusVM, Boolean>("InterruptMask6");
        public static readonly DependencyProperty InterruptMask6Property = InterruptMask6PK.DependencyProperty;
        public Boolean InterruptMask6 => GetValue(InterruptMask6Property);
        #endregion

        #region DP - InterruptMask7
        private static readonly DependencyPropertyKey InterruptMask7PK = RegDPKey<StatusVM, Boolean>("InterruptMask7");
        public static readonly DependencyProperty InterruptMask7Property = InterruptMask7PK.DependencyProperty;
        public Boolean InterruptMask7 => GetValue(InterruptMask7Property);
        #endregion

        public ExecutionState State => MachineViewModel.CurrentModel.CurrentMachine?.DeviceCPU.State;
        public StatusRegister Status => MachineViewModel.CurrentModel.CurrentMachine?.DeviceCPU.State.CP0Regs.StatusReg;
    }
}
