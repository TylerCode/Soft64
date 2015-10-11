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

        #region DP - Bad Virtual Address
        private static readonly DependencyPropertyKey BadVAddressPK = RegDPKey<Cop0RegistersViewModel, Register64Value>("BadVAddress");
        public static readonly DependencyProperty BadVAddressProperty = BadVAddressPK.DependencyProperty;
        public Register64Value BadVAddress => GetValue(BadVAddressProperty);
        #endregion

        #region DP - Entry Hi
        private static readonly DependencyPropertyKey EntryHiPK = RegDPKey<Cop0RegistersViewModel, Register64Value>("EntryHi");
        public static readonly DependencyProperty EntryHiProperty = EntryHiPK.DependencyProperty;
        public Register64Value EntryHi => GetValue(EntryHiProperty);
        #endregion

        #region DP - Entry Lo 0
        private static readonly DependencyPropertyKey EntryLo0PK = RegDPKey<Cop0RegistersViewModel, Register64Value>("EntryLo0");
        public static readonly DependencyProperty EntryLo0Property = EntryLo0PK.DependencyProperty;
        public Register64Value EntryLo0 => GetValue(EntryLo0Property);
        #endregion

        #region DP - Entry Lo 1
        private static readonly DependencyPropertyKey EntryLo1PK = RegDPKey<Cop0RegistersViewModel, Register64Value>("EntryLo1");
        public static readonly DependencyProperty EntryLo1Property = EntryLo1PK.DependencyProperty;
        public Register64Value EntryLo1 => GetValue(EntryLo1Property);
        #endregion


    }
}
