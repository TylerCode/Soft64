using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Soft64WPF.ViewModels
{
    public sealed class Cop0RegistersViewModel : QuickDependencyObject
    {
        internal Cop0RegistersViewModel()
        {
            SetValue(CountPK, new Register32Value());
            SetValue(ComparePK, new Register32Value());
        }

        public void Load()
        {

        }

        public void Store()
        {

        }

        #region DP - Count
        private static readonly DependencyPropertyKey CountPK = RegDPKey<Cop0RegistersViewModel, Register32Value>("Count");
        public static readonly DependencyProperty CountProperty = CountPK.DependencyProperty;
        public Register32Value Count => GetValue(CountProperty);
        #endregion

        #region DP - Compare
        private static readonly DependencyPropertyKey ComparePK = RegDPKey<Cop0RegistersViewModel, Register32Value>("Compare");
        public static readonly DependencyProperty CompareProperty = ComparePK.DependencyProperty;
        public Register32Value Compare => GetValue(CompareProperty);
        #endregion
    }
}
