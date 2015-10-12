using Soft64.MipsR4300;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Soft64WPF.ViewModels
{
    public sealed class Cop0CauseRegisterViewModel : QuickDependencyObject
    {
        internal Cop0CauseRegisterViewModel()
        {

        }

        public void Load()
        {

        }

        public void Store()
        {

        }

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
