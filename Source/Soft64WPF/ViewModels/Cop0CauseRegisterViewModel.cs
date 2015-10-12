using Soft64.MipsR4300;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using CauseVM = Soft64WPF.ViewModels.Cop0CauseRegisterViewModel;

namespace Soft64WPF.ViewModels
{
    public sealed class Cop0CauseRegisterViewModel : QuickDependencyObject
    {
        internal Cop0CauseRegisterViewModel()
        {
            SetValue(RegisterPK, new Register32Value());
        }

        public void Load()
        {
            Interrupt0 = Cause.IP0;
            Interrupt1 = Cause.IP1;
            Interrupt2 = Cause.IP2;
            Interrupt3 = Cause.IP3;
            Interrupt4 = Cause.IP4;
            Interrupt5 = Cause.IP5;
            Interrupt6 = Cause.IP6;
            Interrupt7 = Cause.IP7;
        }

        public void Store()
        {
            Cause.IP0 = Interrupt0;
            Cause.IP1 = Interrupt1;
            Cause.IP2 = Interrupt2;
            Cause.IP3 = Interrupt3;
            Cause.IP4 = Interrupt4;
            Cause.IP5 = Interrupt5;
            Cause.IP6 = Interrupt6;
            Cause.IP7 = Interrupt7;
        }

        #region DP - Register
        private static readonly DependencyPropertyKey RegisterPK = RegDPKey<CauseVM, Register32Value>("Register");
        public static readonly DependencyProperty RegisterProperty = RegisterPK.DependencyProperty;
        public Register32Value Register => GetValue(RegisterProperty);
        #endregion

        #region DP - Interrupt0
        public static readonly DependencyProperty Interrupt0Property = RegDP<Cop0RegistersViewModel, Boolean>("Interrupt0");
        public Boolean Interrupt0 { get { return GetValue(Interrupt0Property); } set{ SetValue(Interrupt0Property, value); } }
        #endregion

        #region DP - Interrupt1
        public static readonly DependencyProperty Interrupt1Property = RegDP<Cop0RegistersViewModel, Boolean>("Interrupt1");
        public Boolean Interrupt1 { get { return GetValue(Interrupt1Property); } set { SetValue(Interrupt1Property, value); } }
        #endregion

        #region DP - Interrupt2
        public static readonly DependencyProperty Interrupt2Property = RegDP<Cop0RegistersViewModel, Boolean>("Interrupt2");
        public Boolean Interrupt2 { get { return GetValue(Interrupt2Property); } set { SetValue(Interrupt2Property, value); } }
        #endregion

        #region DP - Interrupt3
        public static readonly DependencyProperty Interrupt3Property = RegDP<Cop0RegistersViewModel, Boolean>("Interrupt3");
        public Boolean Interrupt3 { get { return GetValue(Interrupt3Property); } set { SetValue(Interrupt3Property, value); } }
        #endregion

        #region DP - Interrupt4
        public static readonly DependencyProperty Interrupt4Property = RegDP<Cop0RegistersViewModel, Boolean>("Interrupt4");
        public Boolean Interrupt4 { get { return GetValue(Interrupt4Property); } set { SetValue(Interrupt4Property, value); } }
        #endregion

        #region DP - Interrupt5
        public static readonly DependencyProperty Interrupt5Property = RegDP<Cop0RegistersViewModel, Boolean>("Interrupt5");
        public Boolean Interrupt5 { get { return GetValue(Interrupt5Property); } set { SetValue(Interrupt5Property, value); } }
        #endregion

        #region DP - Interrupt6
        public static readonly DependencyProperty Interrupt6Property = RegDP<Cop0RegistersViewModel, Boolean>("Interrupt6");
        public Boolean Interrupt6 { get { return GetValue(Interrupt6Property); } set { SetValue(Interrupt6Property, value); } }
        #endregion

        #region DP - Interrupt7
        public static readonly DependencyProperty Interrupt7Property = RegDP<Cop0RegistersViewModel, Boolean>("Interrupt7");
        public Boolean Interrupt7 { get { return GetValue(Interrupt7Property); } set { SetValue(Interrupt7Property, value); } }
        #endregion

        public CauseRegister Cause => MachineViewModel.CurrentModel.CurrentMachine?.DeviceCPU.State.CP0Regs.CauseReg;
    }
}
